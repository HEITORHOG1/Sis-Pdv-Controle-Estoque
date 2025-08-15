# Simple test script to verify API functionality
Write-Host "Testing PDV API endpoints..." -ForegroundColor Green

# Test health endpoint
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/health" -Method GET
    Write-Host "✅ Health endpoint working:" -ForegroundColor Green
    Write-Host ($response | ConvertTo-Json -Depth 3) -ForegroundColor Cyan
} catch {
    Write-Host "❌ Health endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test detailed health endpoint
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/health/detailed" -Method GET
    Write-Host "✅ Detailed health endpoint working:" -ForegroundColor Green
    Write-Host ($response | ConvertTo-Json -Depth 3) -ForegroundColor Cyan
} catch {
    Write-Host "❌ Detailed health endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test existing categoria endpoint
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/Categoria/ListarCategoria" -Method GET
    Write-Host "✅ Categoria endpoint working:" -ForegroundColor Green
    Write-Host ($response | ConvertTo-Json -Depth 3) -ForegroundColor Cyan
} catch {
    Write-Host "❌ Categoria endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Test completed!" -ForegroundColor Green