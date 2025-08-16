#!/usr/bin/env pwsh
param(
    [string]$BaseUrl = "http://localhost:5000",
    [string]$Environment = "Development"
)

$ErrorActionPreference = "Continue"

Write-Host "Testing Configuration Management System" -ForegroundColor Green
Write-Host "Base URL: $BaseUrl" -ForegroundColor Cyan
Write-Host "Environment: $Environment" -ForegroundColor Cyan
Write-Host "=" * 50

# Test results
$TestResults = @()

# Test 1: Configuration File Validation
Write-Host "Testing: Configuration Files" -ForegroundColor Yellow

$configFiles = @(
    "Sis-Pdv-Controle-Estoque-API/appsettings.json",
    "Sis-Pdv-Controle-Estoque-API/appsettings.Development.json",
    "Sis-Pdv-Controle-Estoque-API/appsettings.Staging.json",
    "Sis-Pdv-Controle-Estoque-API/appsettings.Production.json"
)

foreach ($file in $configFiles) {
    if (Test-Path $file) {
        try {
            $content = Get-Content $file -Raw | ConvertFrom-Json
            Write-Host "✓ $file - Valid JSON" -ForegroundColor Green
            
            $TestResults += @{
                Name = "Config File: $file"
                Status = "Success"
                Response = "Valid JSON structure"
            }
        }
        catch {
            Write-Host "✗ $file - Invalid JSON" -ForegroundColor Red
            Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
            
            $TestResults += @{
                Name = "Config File: $file"
                Status = "Failed"
                Response = "Invalid JSON: $($_.Exception.Message)"
            }
        }
    }
    else {
        Write-Host "✗ $file - File not found" -ForegroundColor Red
        
        $TestResults += @{
            Name = "Config File: $file"
            Status = "Failed"
            Response = "File not found"
        }
    }
}
Write-Host ""

# Test 2: Docker Configuration Validation
Write-Host "Testing: Docker Configuration" -ForegroundColor Yellow

$dockerFiles = @(
    "Dockerfile",
    "Dockerfile.development",
    "docker-compose.yml",
    "docker-compose.staging.yml",
    "docker-compose.development.yml",
    ".dockerignore"
)

foreach ($file in $dockerFiles) {
    if (Test-Path $file) {
        Write-Host "✓ $file - Exists" -ForegroundColor Green
        
        $TestResults += @{
            Name = "Docker File: $file"
            Status = "Success"
            Response = "File exists"
        }
    }
    else {
        Write-Host "✗ $file - Missing" -ForegroundColor Red
        
        $TestResults += @{
            Name = "Docker File: $file"
            Status = "Failed"
            Response = "File missing"
        }
    }
}
Write-Host ""

# Test 3: Environment File Template
Write-Host "Testing: Environment File Template" -ForegroundColor Yellow

if (Test-Path ".env.example") {
    Write-Host "✓ .env.example - Exists" -ForegroundColor Green
    
    $TestResults += @{
        Name = "Environment Template"
        Status = "Success"
        Response = "Template file exists"
    }
}
else {
    Write-Host "✗ .env.example - Missing" -ForegroundColor Red
    
    $TestResults += @{
        Name = "Environment Template"
        Status = "Failed"
        Response = "Template file missing"
    }
}
Write-Host ""

# Test 4: Deployment Scripts
Write-Host "Testing: Deployment Scripts" -ForegroundColor Yellow

$scripts = @(
    "scripts/deploy.ps1",
    "scripts/setup-environment.ps1"
)

foreach ($script in $scripts) {
    if (Test-Path $script) {
        Write-Host "✓ $script - Exists" -ForegroundColor Green
        
        $TestResults += @{
            Name = "Script: $script"
            Status = "Success"
            Response = "Script exists"
        }
    }
    else {
        Write-Host "✗ $script - Missing" -ForegroundColor Red
        
        $TestResults += @{
            Name = "Script: $script"
            Status = "Failed"
            Response = "Script missing"
        }
    }
}
Write-Host ""

# Test 5: Configuration Classes
Write-Host "Testing: Configuration Classes" -ForegroundColor Yellow

$configClasses = @(
    "Sis-Pdv-Controle-Estoque-API/Configuration/ConfigurationOptions.cs",
    "Sis-Pdv-Controle-Estoque-API/Configuration/SecureConfigurationService.cs",
    "Sis-Pdv-Controle-Estoque-API/Configuration/ConfigurationValidator.cs",
    "Sis-Pdv-Controle-Estoque-API/Configuration/ConfigurationExtensions.cs"
)

foreach ($file in $configClasses) {
    if (Test-Path $file) {
        Write-Host "✓ $file - Exists" -ForegroundColor Green
        
        $TestResults += @{
            Name = "Config Class: $file"
            Status = "Success"
            Response = "File exists"
        }
    }
    else {
        Write-Host "✗ $file - Missing" -ForegroundColor Red
        
        $TestResults += @{
            Name = "Config Class: $file"
            Status = "Failed"
            Response = "File missing"
        }
    }
}
Write-Host ""

# Summary
Write-Host "Test Summary" -ForegroundColor Green
Write-Host "=" * 50

$successCount = ($TestResults | Where-Object { $_.Status -eq "Success" }).Count
$failCount = ($TestResults | Where-Object { $_.Status -eq "Failed" }).Count
$totalCount = $TestResults.Count

Write-Host "Total Tests: $totalCount" -ForegroundColor Cyan
Write-Host "Passed: $successCount" -ForegroundColor Green
Write-Host "Failed: $failCount" -ForegroundColor Red

if ($totalCount -gt 0) {
    Write-Host "Success Rate: $([math]::Round(($successCount / $totalCount) * 100, 2))%" -ForegroundColor Cyan
}

if ($failCount -gt 0) {
    Write-Host "`nFailed Tests:" -ForegroundColor Red
    $TestResults | Where-Object { $_.Status -eq "Failed" } | ForEach-Object {
        Write-Host "  - $($_.Name): $($_.Response)" -ForegroundColor Red
    }
}

Write-Host "`nConfiguration Management Test Completed!" -ForegroundColor Green

# Return appropriate exit code
if ($failCount -gt 0) {
    exit 1
}
else {
    exit 0
}