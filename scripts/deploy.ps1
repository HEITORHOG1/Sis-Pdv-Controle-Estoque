#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Development", "Staging", "Production")]
    [string]$Environment,
    
    [string]$Version = "latest",
    [switch]$SkipBuild,
    [switch]$SkipMigrations,
    [switch]$Force
)

$ErrorActionPreference = "Stop"

Write-Host "Starting deployment for environment: $Environment" -ForegroundColor Green

# Set environment-specific variables
$ComposeFile = switch ($Environment) {
    "Development" { "docker-compose.development.yml" }
    "Staging" { "docker-compose.staging.yml" }
    "Production" { "docker-compose.yml" }
}

$EnvFile = ".env.$($Environment.ToLower())"

# Check if environment file exists
if (-not (Test-Path $EnvFile)) {
    Write-Error "Environment file $EnvFile not found. Please create it with required environment variables."
    exit 1
}

# Load environment variables
Write-Host "Loading environment variables from $EnvFile" -ForegroundColor Yellow
Get-Content $EnvFile | ForEach-Object {
    if ($_ -match '^([^=]+)=(.*)$') {
        [Environment]::SetEnvironmentVariable($matches[1], $matches[2])
    }
}

# Validate required environment variables
$RequiredVars = @(
    "DB_PASSWORD",
    "RABBITMQ_PASSWORD", 
    "JWT_SECRET"
)

if ($Environment -ne "Development") {
    $RequiredVars += @("MYSQL_ROOT_PASSWORD", "PDV_ENCRYPTION_KEY")
}

foreach ($var in $RequiredVars) {
    if (-not [Environment]::GetEnvironmentVariable($var)) {
        Write-Error "Required environment variable $var is not set"
        exit 1
    }
}

try {
    # Stop existing containers if Force is specified
    if ($Force) {
        Write-Host "Stopping existing containers..." -ForegroundColor Yellow
        docker-compose -f $ComposeFile down --remove-orphans
    }

    # Build images if not skipped
    if (-not $SkipBuild) {
        Write-Host "Building Docker images..." -ForegroundColor Yellow
        docker-compose -f $ComposeFile build --no-cache
    }

    # Start infrastructure services first
    Write-Host "Starting infrastructure services..." -ForegroundColor Yellow
    docker-compose -f $ComposeFile up -d mysql rabbitmq

    # Wait for services to be healthy
    Write-Host "Waiting for infrastructure services to be ready..." -ForegroundColor Yellow
    $timeout = 120
    $elapsed = 0
    
    do {
        Start-Sleep 5
        $elapsed += 5
        $mysqlHealth = docker inspect --format='{{.State.Health.Status}}' "pdv-mysql$(if ($Environment -ne 'Production') { '-' + $Environment.ToLower() })"
        $rabbitmqHealth = docker inspect --format='{{.State.Health.Status}}' "pdv-rabbitmq$(if ($Environment -ne 'Production') { '-' + $Environment.ToLower() })"
        
        Write-Host "MySQL: $mysqlHealth, RabbitMQ: $rabbitmqHealth" -ForegroundColor Cyan
        
        if ($elapsed -ge $timeout) {
            Write-Error "Timeout waiting for infrastructure services to be ready"
            exit 1
        }
    } while ($mysqlHealth -ne "healthy" -or $rabbitmqHealth -ne "healthy")

    # Run database migrations if not skipped
    if (-not $SkipMigrations) {
        Write-Host "Running database migrations..." -ForegroundColor Yellow
        
        # Create a temporary container to run migrations
        $migrationContainer = "pdv-migration-$([System.Guid]::NewGuid().ToString('N').Substring(0,8))"
        
        docker run --rm --name $migrationContainer `
            --network "$(Split-Path -Leaf (Get-Location))_pdv-$(if ($Environment -eq 'Production') { 'network' } else { $Environment.ToLower() + '-network' })" `
            -e ASPNETCORE_ENVIRONMENT=$Environment `
            -e DB_SERVER="mysql$(if ($Environment -ne 'Production') { '-' + $Environment.ToLower() })" `
            -e DB_NAME="PDV_$(if ($Environment -eq 'Production') { 'PROD' } else { $Environment.ToUpper() })" `
            -e DB_USER="$(if ($Environment -eq 'Development') { 'root' } else { 'pdvuser' })" `
            -e DB_PASSWORD="$([Environment]::GetEnvironmentVariable('DB_PASSWORD'))" `
            pdv-api:latest `
            dotnet ef database update --no-build
    }

    # Start the API service
    Write-Host "Starting PDV API service..." -ForegroundColor Yellow
    docker-compose -f $ComposeFile up -d pdv-api

    # Wait for API to be healthy
    Write-Host "Waiting for API service to be ready..." -ForegroundColor Yellow
    $timeout = 60
    $elapsed = 0
    
    do {
        Start-Sleep 5
        $elapsed += 5
        
        try {
            $response = Invoke-WebRequest -Uri "http://localhost:$(if ($Environment -eq 'Development') { '5000' } else { '8080' })/health" -TimeoutSec 5
            if ($response.StatusCode -eq 200) {
                Write-Host "API service is ready!" -ForegroundColor Green
                break
            }
        }
        catch {
            Write-Host "API not ready yet..." -ForegroundColor Yellow
        }
        
        if ($elapsed -ge $timeout) {
            Write-Error "Timeout waiting for API service to be ready"
            exit 1
        }
    } while ($true)

    # Start remaining services (nginx for production)
    if ($Environment -eq "Production") {
        Write-Host "Starting Nginx reverse proxy..." -ForegroundColor Yellow
        docker-compose -f $ComposeFile up -d nginx
    }

    Write-Host "Deployment completed successfully!" -ForegroundColor Green
    Write-Host "Services are running on:" -ForegroundColor Cyan
    
    switch ($Environment) {
        "Development" {
            Write-Host "  API: http://localhost:5000" -ForegroundColor White
            Write-Host "  Health: http://localhost:5000/health" -ForegroundColor White
            Write-Host "  Swagger: http://localhost:5000/swagger" -ForegroundColor White
            Write-Host "  RabbitMQ Management: http://localhost:15674" -ForegroundColor White
        }
        "Staging" {
            Write-Host "  API: http://localhost:8080" -ForegroundColor White
            Write-Host "  Health: http://localhost:8080/health" -ForegroundColor White
            Write-Host "  RabbitMQ Management: http://localhost:15673" -ForegroundColor White
        }
        "Production" {
            Write-Host "  API: http://localhost (via Nginx)" -ForegroundColor White
            Write-Host "  Health: http://localhost/health" -ForegroundColor White
            Write-Host "  RabbitMQ Management: http://localhost:15672" -ForegroundColor White
        }
    }

}
catch {
    Write-Error "Deployment failed: $($_.Exception.Message)"
    
    # Show container logs for debugging
    Write-Host "Container logs:" -ForegroundColor Red
    docker-compose -f $ComposeFile logs --tail=50
    
    exit 1
}