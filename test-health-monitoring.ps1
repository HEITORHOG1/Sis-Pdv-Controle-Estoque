# Test script for Health Checks and Monitoring System
# This script tests all health check endpoints and monitoring functionality

$baseUrl = "https://localhost:7297"
$apiUrl = "$baseUrl/api/v1.0"

Write-Host "=== PDV System Health Checks and Monitoring Test ===" -ForegroundColor Green
Write-Host "Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host ""

# Function to make HTTP requests with error handling
function Invoke-ApiRequest {
    param(
        [string]$Url,
        [string]$Method = "GET",
        [hashtable]$Headers = @{},
        [object]$Body = $null,
        [string]$Description = ""
    )
    
    try {
        Write-Host "Testing: $Description" -ForegroundColor Cyan
        Write-Host "URL: $Url" -ForegroundColor Gray
        
        $params = @{
            Uri = $Url
            Method = $Method
            Headers = $Headers
            UseBasicParsing = $true
        }
        
        # Skip certificate validation for self-signed certificates
        if (-not ([System.Management.Automation.PSTypeName]'ServerCertificateValidationCallback').Type) {
            $certCallback = @"
                using System;
                using System.Net;
                using System.Net.Security;
                using System.Security.Cryptography.X509Certificates;
                public class ServerCertificateValidationCallback
                {
                    public static void Ignore()
                    {
                        if(ServicePointManager.ServerCertificateValidationCallback ==null)
                        {
                            ServicePointManager.ServerCertificateValidationCallback += 
                                delegate
                                (
                                    Object obj, 
                                    X509Certificate certificate, 
                                    X509Chain chain, 
                                    SslPolicyErrors errors
                                )
                                {
                                    return true;
                                };
                        }
                    }
                }
"@
            Add-Type $certCallback
        }
        [ServerCertificateValidationCallback]::Ignore()
        
        if ($Body) {
            $params.Body = $Body | ConvertTo-Json -Depth 10
            $params.ContentType = "application/json"
        }
        
        $response = Invoke-RestMethod @params
        Write-Host "SUCCESS" -ForegroundColor Green
        
        if ($response) {
            Write-Host "Response:" -ForegroundColor Yellow
            $response | ConvertTo-Json -Depth 3 | Write-Host
        }
        
        return $response
    }
    catch {
        Write-Host "FAILED: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
        }
        return $null
    }
    finally {
        Write-Host ""
    }
}

# Test 1: Basic Health Check Endpoints (No Authentication Required)
Write-Host "=== 1. Basic Health Check Endpoints ===" -ForegroundColor Magenta

# Test simple health check
Invoke-ApiRequest -Url "$baseUrl/health/simple" -Description "Simple Health Check"

# Test readiness probe
Invoke-ApiRequest -Url "$baseUrl/health/ready" -Description "Readiness Probe"

# Test liveness probe
Invoke-ApiRequest -Url "$baseUrl/health/live" -Description "Liveness Probe"

# Test detailed health check
Invoke-ApiRequest -Url "$baseUrl/health" -Description "Detailed Health Check"

# Test Health UI (should return HTML)
try {
    Write-Host "Testing: Health Check UI" -ForegroundColor Cyan
    Write-Host "URL: $baseUrl/health-ui" -ForegroundColor Gray
    
    $response = Invoke-WebRequest -Uri "$baseUrl/health-ui" -UseBasicParsing
    if ($response.StatusCode -eq 200) {
        Write-Host "SUCCESS - Health UI is accessible" -ForegroundColor Green
    }
}
catch {
    Write-Host "FAILED: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 2: Authentication for Protected Endpoints
Write-Host "=== 2. Authentication Setup ===" -ForegroundColor Magenta

# First, try to get a token (assuming we have a test user)
$loginData = @{
    username = "admin"
    password = "Admin123!"
}

$token = $null
try {
    Write-Host "Attempting to login..." -ForegroundColor Cyan
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/auth/login" -Method POST -Body ($loginData | ConvertTo-Json) -ContentType "application/json" -UseBasicParsing
    
    if ($loginResponse -and $loginResponse.data -and $loginResponse.data.token) {
        $token = $loginResponse.data.token
        Write-Host "Login successful" -ForegroundColor Green
    }
    else {
        Write-Host "Login response received but no token found" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "Login failed - will test without authentication: $($_.Exception.Message)" -ForegroundColor Yellow
}
Write-Host ""

# Prepare headers for authenticated requests
$authHeaders = @{}
if ($token) {
    $authHeaders["Authorization"] = "Bearer $token"
    Write-Host "Using authentication token for protected endpoints" -ForegroundColor Green
}
else {
    Write-Host "No authentication token - protected endpoints may fail" -ForegroundColor Yellow
}
Write-Host ""

# Test 3: Health Check Controller Endpoints
Write-Host "=== 3. Health Check Controller Endpoints ===" -ForegroundColor Magenta

# Test health status endpoint
Invoke-ApiRequest -Url "$apiUrl/healthcheck/status" -Headers $authHeaders -Description "Health Status"

# Test system metrics
Invoke-ApiRequest -Url "$apiUrl/healthcheck/metrics/system" -Headers $authHeaders -Description "System Metrics"

# Test business metrics
Invoke-ApiRequest -Url "$apiUrl/healthcheck/metrics/business" -Headers $authHeaders -Description "Business Metrics"

# Test application metrics
Invoke-ApiRequest -Url "$apiUrl/healthcheck/metrics/application" -Headers $authHeaders -Description "Application Metrics"

# Test dashboard metrics
Invoke-ApiRequest -Url "$apiUrl/healthcheck/dashboard" -Headers $authHeaders -Description "Dashboard Metrics"

# Test available components
Invoke-ApiRequest -Url "$apiUrl/healthcheck/components" -Headers $authHeaders -Description "Available Components"

# Test 4: Individual Component Health Checks
Write-Host "=== 4. Individual Component Health Checks ===" -ForegroundColor Magenta

$components = @("database", "mysql", "rabbitmq", "business-operations", "system-metrics", "memory", "disk-space")

foreach ($component in $components) {
    Invoke-ApiRequest -Url "$apiUrl/healthcheck/component/$component" -Headers $authHeaders -Description "Component: $component"
}

# Summary
Write-Host "=== Test Summary ===" -ForegroundColor Green
Write-Host "Health check and monitoring system test completed!" -ForegroundColor Green
Write-Host ""
Write-Host "Key Endpoints Tested:" -ForegroundColor Yellow
Write-Host "  /health - Detailed health status" -ForegroundColor White
Write-Host "  /health/ready - Readiness probe" -ForegroundColor White
Write-Host "  /health/live - Liveness probe" -ForegroundColor White
Write-Host "  /health/simple - Simple health check" -ForegroundColor White
Write-Host "  /health-ui - Health monitoring dashboard" -ForegroundColor White
Write-Host "  /api/v1.0/healthcheck/* - API endpoints for metrics and monitoring" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Check the Health UI at: $baseUrl/health-ui" -ForegroundColor White
Write-Host "  2. Monitor system metrics in production" -ForegroundColor White
Write-Host "  3. Set up alerting based on health check results" -ForegroundColor White
Write-Host "  4. Configure monitoring tools to use these endpoints" -ForegroundColor White