# PowerShell script to test authentication endpoints
Write-Host "Testing Authentication System..." -ForegroundColor Green

$baseUrl = "https://localhost:7001/api"

# Test 1: Seed authentication data (Development only)
Write-Host "`n1. Seeding authentication data..." -ForegroundColor Yellow
try {
    $seedResponse = Invoke-RestMethod -Uri "$baseUrl/seed/auth" -Method POST -ContentType "application/json"
    Write-Host "Seed Response: $($seedResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Green
} catch {
    Write-Host "Seed Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Login with default admin user
Write-Host "`n2. Testing login..." -ForegroundColor Yellow
$loginData = @{
    login = "admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    Write-Host "Login Response: $($loginResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Green
    
    if ($loginResponse.success -and $loginResponse.data.accessToken) {
        $accessToken = $loginResponse.data.accessToken
        $refreshToken = $loginResponse.data.refreshToken
        
        # Test 3: Get current user info
        Write-Host "`n3. Testing get current user..." -ForegroundColor Yellow
        $headers = @{
            "Authorization" = "Bearer $accessToken"
        }
        
        try {
            $userResponse = Invoke-RestMethod -Uri "$baseUrl/auth/me" -Method GET -Headers $headers
            Write-Host "User Info Response: $($userResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Green
        } catch {
            Write-Host "User Info Error: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        # Test 4: Refresh token
        Write-Host "`n4. Testing token refresh..." -ForegroundColor Yellow
        $refreshData = @{
            refreshToken = $refreshToken
        } | ConvertTo-Json
        
        try {
            $refreshResponse = Invoke-RestMethod -Uri "$baseUrl/auth/refresh" -Method POST -Body $refreshData -ContentType "application/json"
            Write-Host "Refresh Response: $($refreshResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Green
        } catch {
            Write-Host "Refresh Error: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        # Test 5: Logout
        Write-Host "`n5. Testing logout..." -ForegroundColor Yellow
        try {
            $logoutResponse = Invoke-RestMethod -Uri "$baseUrl/auth/logout" -Method POST -Body $refreshData -ContentType "application/json"
            Write-Host "Logout Response: $($logoutResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Green
        } catch {
            Write-Host "Logout Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
} catch {
    Write-Host "Login Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nAuthentication testing completed!" -ForegroundColor Green