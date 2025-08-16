# Test script for Inventory Management functionality
# This script tests the new inventory tracking features

$baseUrl = "https://localhost:7297/api"
$headers = @{
    "Content-Type" = "application/json"
    "Accept" = "application/json"
}

Write-Host "=== Testing Inventory Management System ===" -ForegroundColor Green

# First, let's get an auth token (assuming we have a test user)
Write-Host "`n1. Getting authentication token..." -ForegroundColor Yellow

$loginRequest = @{
    Username = "admin"
    Password = "Admin123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method POST -Body $loginRequest -Headers $headers
    $token = $loginResponse.data.token
    $headers["Authorization"] = "Bearer $token"
    Write-Host "✓ Authentication successful" -ForegroundColor Green
} catch {
    Write-Host "✗ Authentication failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Test 2: Get stock alerts
Write-Host "`n2. Testing stock alerts..." -ForegroundColor Yellow
try {
    $alertsResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/alerts" -Method GET -Headers $headers
    Write-Host "✓ Stock alerts retrieved successfully" -ForegroundColor Green
    Write-Host "   Total alerts: $($alertsResponse.data.totalCount)" -ForegroundColor Cyan
    
    if ($alertsResponse.data.alerts.Count -gt 0) {
        Write-Host "   Sample alerts:" -ForegroundColor Cyan
        $alertsResponse.data.alerts | Select-Object -First 3 | ForEach-Object {
            Write-Host "   - $($_.productName): $($_.message)" -ForegroundColor White
        }
    }
} catch {
    Write-Host "✗ Failed to get stock alerts: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Get stock movements
Write-Host "`n3. Testing stock movements query..." -ForegroundColor Yellow
try {
    $movementsResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/movements?pageSize=10" -Method GET -Headers $headers
    Write-Host "✓ Stock movements retrieved successfully" -ForegroundColor Green
    Write-Host "   Total movements: $($movementsResponse.data.totalCount)" -ForegroundColor Cyan
    
    if ($movementsResponse.data.movements.Count -gt 0) {
        Write-Host "   Recent movements:" -ForegroundColor Cyan
        $movementsResponse.data.movements | Select-Object -First 3 | ForEach-Object {
            Write-Host "   - $($_.productName): $($_.typeDescription) - Qty: $($_.quantity)" -ForegroundColor White
        }
    }
} catch {
    Write-Host "✗ Failed to get stock movements: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Get a product to test stock validation
Write-Host "`n4. Getting a product for testing..." -ForegroundColor Yellow
try {
    $productsResponse = Invoke-RestMethod -Uri "$baseUrl/produto/listar-produtos-paginado?pageSize=1" -Method GET -Headers $headers
    
    if ($productsResponse.data.items.Count -gt 0) {
        $testProduct = $productsResponse.data.items[0]
        $productId = $testProduct.id
        Write-Host "✓ Test product found: $($testProduct.nomeProduto)" -ForegroundColor Green
        Write-Host "   Current stock: $($testProduct.quatidadeEstoqueProduto)" -ForegroundColor Cyan
        
        # Test 5: Validate stock availability
        Write-Host "`n5. Testing stock validation..." -ForegroundColor Yellow
        $stockValidationRequest = @{
            ProductId = $productId
            RequestedQuantity = 1
        } | ConvertTo-Json
        
        try {
            $validationResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/validate-stock" -Method POST -Body $stockValidationRequest -Headers $headers
            Write-Host "✓ Stock validation successful" -ForegroundColor Green
            Write-Host "   Is valid: $($validationResponse.data.isValid)" -ForegroundColor Cyan
            Write-Host "   Message: $($validationResponse.data.message)" -ForegroundColor Cyan
        } catch {
            Write-Host "✗ Stock validation failed: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        # Test 6: Adjust stock
        Write-Host "`n6. Testing stock adjustment..." -ForegroundColor Yellow
        $currentStock = [int]$testProduct.quatidadeEstoqueProduto
        $newStock = $currentStock + 10
        
        $adjustStockRequest = @{
            ProductId = $productId
            NewQuantity = $newStock
            Reason = "Teste de ajuste de estoque via API"
            ReferenceDocument = "TEST-001"
        } | ConvertTo-Json
        
        try {
            $adjustResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/adjust-stock" -Method POST -Body $adjustStockRequest -Headers $headers
            Write-Host "✓ Stock adjustment successful" -ForegroundColor Green
            Write-Host "   Previous stock: $($adjustResponse.data.previousStock)" -ForegroundColor Cyan
            Write-Host "   New stock: $($adjustResponse.data.newStock)" -ForegroundColor Cyan
            Write-Host "   Movement ID: $($adjustResponse.data.stockMovementId)" -ForegroundColor Cyan
        } catch {
            Write-Host "✗ Stock adjustment failed: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        # Test 7: Validate stock with insufficient quantity
        Write-Host "`n7. Testing stock validation with insufficient quantity..." -ForegroundColor Yellow
        $insufficientStockRequest = @{
            ProductId = $productId
            RequestedQuantity = 99999
        } | ConvertTo-Json
        
        try {
            $insufficientValidationResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/validate-stock" -Method POST -Body $insufficientStockRequest -Headers $headers
            Write-Host "✓ Insufficient stock validation successful" -ForegroundColor Green
            Write-Host "   Is valid: $($insufficientValidationResponse.data.isValid)" -ForegroundColor Cyan
            Write-Host "   Message: $($insufficientValidationResponse.data.message)" -ForegroundColor Cyan
            
            if ($insufficientValidationResponse.data.errors.Count -gt 0) {
                Write-Host "   Errors:" -ForegroundColor Cyan
                $insufficientValidationResponse.data.errors | ForEach-Object {
                    Write-Host "   - $($_.errorMessage)" -ForegroundColor White
                }
            }
        } catch {
            Write-Host "✗ Insufficient stock validation failed: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        # Test 8: Batch stock validation
        Write-Host "`n8. Testing batch stock validation..." -ForegroundColor Yellow
        $batchValidationRequest = @(
            @{
                ProductId = $productId
                RequestedQuantity = 1
            },
            @{
                ProductId = $productId
                RequestedQuantity = 5
            }
        ) | ConvertTo-Json
        
        try {
            $batchValidationResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/validate-stock-batch" -Method POST -Body $batchValidationRequest -Headers $headers
            Write-Host "✓ Batch stock validation successful" -ForegroundColor Green
            Write-Host "   Is valid: $($batchValidationResponse.data.isValid)" -ForegroundColor Cyan
            Write-Host "   Message: $($batchValidationResponse.data.message)" -ForegroundColor Cyan
        } catch {
            Write-Host "✗ Batch stock validation failed: $($_.Exception.Message)" -ForegroundColor Red
        }
        
    } else {
        Write-Host "✗ No products found for testing" -ForegroundColor Red
    }
} catch {
    Write-Host "✗ Failed to get products: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 9: Get stock movements for specific product
if ($productId) {
    Write-Host "`n9. Testing product-specific stock movements..." -ForegroundColor Yellow
    try {
        $productMovementsResponse = Invoke-RestMethod -Uri "$baseUrl/inventory/movements?productId=$productId&pageSize=5" -Method GET -Headers $headers
        Write-Host "✓ Product stock movements retrieved successfully" -ForegroundColor Green
        Write-Host "   Movements for this product: $($productMovementsResponse.data.totalCount)" -ForegroundColor Cyan
        
        if ($productMovementsResponse.data.movements.Count -gt 0) {
            Write-Host "   Recent movements for this product:" -ForegroundColor Cyan
            $productMovementsResponse.data.movements | ForEach-Object {
                Write-Host "   - $($_.movementDate): $($_.typeDescription) - Qty: $($_.quantity) - Reason: $($_.reason)" -ForegroundColor White
            }
        }
    } catch {
        Write-Host "✗ Failed to get product stock movements: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== Inventory Management Tests Completed ===" -ForegroundColor Green
Write-Host "Check the results above to verify all functionality is working correctly." -ForegroundColor Yellow