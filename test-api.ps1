# Simple test script to verify API functionality (DEV @ http://localhost:7003)
param([int]$Port = 7003)

Write-Host "Testing PDV API endpoints on port $Port..." -ForegroundColor Green

function Test-Endpoint {
    param([string]$Url)
    try {
        $r = Invoke-WebRequest -UseBasicParsing -TimeoutSec 10 -Uri $Url
        Write-Host ("✅ {0} -> {1} {2}" -f $Url, $r.StatusCode, $r.Headers['Content-Type']) -ForegroundColor Green
        if ($r.Content) { ($r.Content.Substring(0, [Math]::Min(200, $r.Content.Length))) | Out-Host }
    } catch {
        Write-Host ("❌ {0} -> {1}" -f $Url, $_.Exception.Message) -ForegroundColor Red
    }
}

Test-Endpoint -Url "http://localhost:$Port/health"
Test-Endpoint -Url "http://localhost:$Port/health/ready"
Test-Endpoint -Url "http://localhost:$Port/health/live"
Test-Endpoint -Url "http://localhost:$Port/swagger/v1/swagger.json"
Test-Endpoint -Url "http://localhost:$Port/api-docs"

Write-Host "Test completed!" -ForegroundColor Green