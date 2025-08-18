# Test script for Reports API
$baseUrl = "https://localhost:7297/api"

Write-Host "Testing Reports API..." -ForegroundColor Green

# First, get authentication token
Write-Host "`nStep 1: Getting authentication token..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "Admin123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "✓ Authentication successful" -ForegroundColor Green
} catch {
    Write-Host "✗ Authentication failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# Test date range (last 30 days)
$endDate = Get-Date
$startDate = $endDate.AddDays(-30)
$startDateStr = $startDate.ToString("yyyy-MM-dd")
$endDateStr = $endDate.ToString("yyyy-MM-dd")

Write-Host "`nStep 2: Testing Sales Reports..." -ForegroundColor Yellow

# Test Sales Report PDF
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/sales/pdf?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Sales Report PDF generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("sales-report.pdf", $response.Content)
        Write-Host "  - PDF saved as sales-report.pdf" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Sales Report PDF failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Sales Report Excel
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/sales/excel?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Sales Report Excel generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("sales-report.xlsx", $response.Content)
        Write-Host "  - Excel saved as sales-report.xlsx" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Sales Report Excel failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nStep 3: Testing Inventory Reports..." -ForegroundColor Yellow

# Test Inventory Report PDF
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/inventory/pdf" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Inventory Report PDF generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("inventory-report.pdf", $response.Content)
        Write-Host "  - PDF saved as inventory-report.pdf" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Inventory Report PDF failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Inventory Report Excel
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/inventory/excel" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Inventory Report Excel generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("inventory-report.xlsx", $response.Content)
        Write-Host "  - Excel saved as inventory-report.xlsx" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Inventory Report Excel failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nStep 4: Testing Stock Movement Reports..." -ForegroundColor Yellow

# Test Stock Movement Report PDF
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/stock-movements/pdf?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Stock Movement Report PDF generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("stock-movements-report.pdf", $response.Content)
        Write-Host "  - PDF saved as stock-movements-report.pdf" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Stock Movement Report PDF failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Stock Movement Report Excel
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/stock-movements/excel?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Stock Movement Report Excel generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("stock-movements-report.xlsx", $response.Content)
        Write-Host "  - Excel saved as stock-movements-report.xlsx" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Stock Movement Report Excel failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nStep 5: Testing Financial Reports..." -ForegroundColor Yellow

# Test Financial Report PDF
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/financial/pdf?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Financial Report PDF generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("financial-report.pdf", $response.Content)
        Write-Host "  - PDF saved as financial-report.pdf" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Financial Report PDF failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Financial Report Excel
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/financial/excel?startDate=$startDateStr&endDate=$endDateStr" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Financial Report Excel generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("financial-report.xlsx", $response.Content)
        Write-Host "  - Excel saved as financial-report.xlsx" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Financial Report Excel failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nStep 6: Testing Low Stock Reports..." -ForegroundColor Yellow

# Test Low Stock Report PDF
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/low-stock/pdf" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Low Stock Report PDF generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("low-stock-report.pdf", $response.Content)
        Write-Host "  - PDF saved as low-stock-report.pdf" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Low Stock Report PDF failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Low Stock Report Excel
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/reports/low-stock/excel" -Method Get -Headers $headers
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Low Stock Report Excel generated successfully" -ForegroundColor Green
        [System.IO.File]::WriteAllBytes("low-stock-report.xlsx", $response.Content)
        Write-Host "  - Excel saved as low-stock-report.xlsx" -ForegroundColor Cyan
    }
} catch {
    Write-Host "✗ Low Stock Report Excel failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nReports API testing completed!" -ForegroundColor Green
Write-Host "Generated report files are saved in the current directory." -ForegroundColor Cyan