# Test script for caching and performance optimizations
Write-Host "Testing Caching and Performance Optimizations" -ForegroundColor Green

# Test 1: Check if the API starts successfully
Write-Host "`n1. Testing API startup..." -ForegroundColor Yellow
try {
    $process = Start-Process -FilePath "dotnet" -ArgumentList "run --project Sis-Pdv-Controle-Estoque-API" -PassThru -WindowStyle Hidden
    Start-Sleep -Seconds 10
    
    # Test if API is responding
    $response = Invoke-WebRequest -Uri "http://localhost:5000/api/health" -Method GET -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ API started successfully" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ API startup failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Test paginated endpoints (if API is running)
Write-Host "`n2. Testing paginated endpoints..." -ForegroundColor Yellow
try {
    # Test products pagination
    $productsResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/produto/paginated?pageNumber=1&pageSize=10" -Method GET -TimeoutSec 5
    if ($productsResponse.StatusCode -eq 200) {
        Write-Host "✓ Products pagination endpoint working" -ForegroundColor Green
    }
    
    # Test clients pagination
    $clientsResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/cliente/paginated?pageNumber=1&pageSize=10" -Method GET -TimeoutSec 5
    if ($clientsResponse.StatusCode -eq 200) {
        Write-Host "✓ Clients pagination endpoint working" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ Pagination endpoints test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Test cache management endpoints
Write-Host "`n3. Testing cache management..." -ForegroundColor Yellow
try {
    # Test cache clear
    $cacheResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/cache/produtos" -Method DELETE -TimeoutSec 5
    if ($cacheResponse.StatusCode -eq 200) {
        Write-Host "✓ Cache management endpoint working" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ Cache management test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Clean up
if ($process -and !$process.HasExited) {
    Write-Host "`n4. Cleaning up..." -ForegroundColor Yellow
    Stop-Process -Id $process.Id -Force
    Write-Host "✓ API process stopped" -ForegroundColor Green
}

Write-Host "`nTest completed!" -ForegroundColor Green
Write-Host "Note: Some tests may fail if authentication is required or if there's no data in the database." -ForegroundColor Cyan