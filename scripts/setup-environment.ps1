#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Development", "Staging", "Production")]
    [string]$Environment,
    
    [switch]$Force
)

$ErrorActionPreference = "Stop"

Write-Host "Setting up environment: $Environment" -ForegroundColor Green

$EnvFile = ".env.$($Environment.ToLower())"

# Check if environment file already exists
if ((Test-Path $EnvFile) -and -not $Force) {
    Write-Warning "Environment file $EnvFile already exists. Use -Force to overwrite."
    exit 1
}

# Generate secure random values
function New-SecurePassword {
    param([int]$Length = 32)
    
    $chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*"
    $password = ""
    
    for ($i = 0; $i -lt $Length; $i++) {
        $password += $chars[(Get-Random -Maximum $chars.Length)]
    }
    
    return $password
}

function New-JwtSecret {
    $bytes = New-Object byte[] 64
    [System.Security.Cryptography.RandomNumberGenerator]::Fill($bytes)
    return [Convert]::ToBase64String($bytes)
}

# Environment-specific configurations
$config = switch ($Environment) {
    "Development" {
        @{
            DB_PASSWORD = "root"
            MYSQL_ROOT_PASSWORD = "root"
            RABBITMQ_PASSWORD = "guest"
            JWT_SECRET = "PDV-Development-Secret-Key-For-JWT-Token-Generation-2024"
            PDV_ENCRYPTION_KEY = "PDV-Dev-Encryption-Key-32-Chars!"
        }
    }
    "Staging" {
        @{
            DB_PASSWORD = New-SecurePassword -Length 24
            MYSQL_ROOT_PASSWORD = New-SecurePassword -Length 24
            RABBITMQ_PASSWORD = New-SecurePassword -Length 24
            JWT_SECRET = New-JwtSecret
            PDV_ENCRYPTION_KEY = New-SecurePassword -Length 32
        }
    }
    "Production" {
        @{
            DB_PASSWORD = New-SecurePassword -Length 32
            MYSQL_ROOT_PASSWORD = New-SecurePassword -Length 32
            RABBITMQ_PASSWORD = New-SecurePassword -Length 32
            JWT_SECRET = New-JwtSecret
            PDV_ENCRYPTION_KEY = New-SecurePassword -Length 32
        }
    }
}

# Create environment file
Write-Host "Creating environment file: $EnvFile" -ForegroundColor Yellow

$envContent = @"
# PDV System Environment Configuration - $Environment
# Generated on $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")

# Database Configuration
DB_PASSWORD=$($config.DB_PASSWORD)
MYSQL_ROOT_PASSWORD=$($config.MYSQL_ROOT_PASSWORD)

# RabbitMQ Configuration
RABBITMQ_PASSWORD=$($config.RABBITMQ_PASSWORD)

# Authentication Configuration
JWT_SECRET=$($config.JWT_SECRET)

# Security Configuration
PDV_ENCRYPTION_KEY=$($config.PDV_ENCRYPTION_KEY)

# Additional Environment Variables (customize as needed)
# ASPNETCORE_ENVIRONMENT=$Environment
# SERILOG_MINIMUM_LEVEL=$(if ($Environment -eq 'Production') { 'Warning' } elseif ($Environment -eq 'Staging') { 'Information' } else { 'Debug' })
"@

$envContent | Out-File -FilePath $EnvFile -Encoding UTF8

Write-Host "Environment file created successfully!" -ForegroundColor Green

# Set appropriate file permissions (Windows)
if ($IsWindows -or $PSVersionTable.PSVersion.Major -le 5) {
    try {
        $acl = Get-Acl $EnvFile
        $acl.SetAccessRuleProtection($true, $false)
        
        # Remove all existing access rules
        $acl.Access | ForEach-Object { $acl.RemoveAccessRule($_) }
        
        # Add access for current user only
        $currentUser = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
        $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($currentUser, "FullControl", "Allow")
        $acl.SetAccessRule($accessRule)
        
        Set-Acl -Path $EnvFile -AclObject $acl
        Write-Host "File permissions set to restrict access to current user only." -ForegroundColor Yellow
    }
    catch {
        Write-Warning "Could not set restrictive file permissions: $($_.Exception.Message)"
    }
}

# Display configuration summary (with masked sensitive values)
Write-Host "`nEnvironment Configuration Summary:" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan

foreach ($key in $config.Keys) {
    $value = $config[$key]
    $maskedValue = if ($value.Length -gt 8) {
        $value.Substring(0, 4) + "****" + $value.Substring($value.Length - 4)
    } else {
        "****"
    }
    Write-Host "$key = $maskedValue" -ForegroundColor White
}

Write-Host "`nNext Steps:" -ForegroundColor Green
Write-Host "1. Review and customize the environment file: $EnvFile" -ForegroundColor White
Write-Host "2. Ensure all required services are available" -ForegroundColor White
Write-Host "3. Run deployment: .\scripts\deploy.ps1 -Environment $Environment" -ForegroundColor White

if ($Environment -ne "Development") {
    Write-Host "`nSecurity Notes:" -ForegroundColor Red
    Write-Host "- Store the environment file securely" -ForegroundColor Yellow
    Write-Host "- Consider using a secrets management system in production" -ForegroundColor Yellow
    Write-Host "- Regularly rotate passwords and secrets" -ForegroundColor Yellow
    Write-Host "- Never commit environment files to version control" -ForegroundColor Yellow
}