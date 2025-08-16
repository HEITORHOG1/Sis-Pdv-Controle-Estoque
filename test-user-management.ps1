#!/usr/bin/env pwsh

# Test User Management System
Write-Host "Testing User Management System..." -ForegroundColor Green

$baseUrl = "https://localhost:7297/api"
$headers = @{
    "Content-Type" = "application/json"
    "Accept" = "application/json"
}

# Test 1: Register a new user
Write-Host "`n1. Testing User Registration..." -ForegroundColor Yellow
$registerRequest = @{
    Login = "testuser"
    Email = "testuser@example.com"
    Nome = "Test User"
    Senha = "password123"
    ConfirmarSenha = "password123"
    RoleIds = @()
    StatusAtivo = $true
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/register" -Method POST -Body $registerRequest -Headers $headers
    Write-Host "✓ User registration successful" -ForegroundColor Green
    Write-Host "User ID: $($registerResponse.Data.Id)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ User registration failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $errorContent = $_.Exception.Response.Content.ReadAsStringAsync().Result
        Write-Host "Error details: $errorContent" -ForegroundColor Red
    }
}

# Test 2: Login with the new user
Write-Host "`n2. Testing User Login..." -ForegroundColor Yellow
$loginRequest = @{
    Login = "testuser"
    Senha = "password123"
    LembrarMe = $false
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/login" -Method POST -Body $loginRequest -Headers $headers
    Write-Host "✓ User login successful" -ForegroundColor Green
    Write-Host "Access Token: $($loginResponse.AccessToken.Substring(0, 20))..." -ForegroundColor Cyan
    
    # Store token for authenticated requests
    $authHeaders = $headers.Clone()
    $authHeaders["Authorization"] = "Bearer $($loginResponse.AccessToken)"
    $userId = $loginResponse.User.Id
    
} catch {
    Write-Host "✗ User login failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $errorContent = $_.Exception.Response.Content.ReadAsStringAsync().Result
        Write-Host "Error details: $errorContent" -ForegroundColor Red
    }
    return
}

# Test 3: Update user profile
Write-Host "`n3. Testing Profile Update..." -ForegroundColor Yellow
$profileRequest = @{
    UsuarioId = $userId
    Nome = "Updated Test User"
    Email = "updated@example.com"
} | ConvertTo-Json

try {
    $profileResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/profile" -Method PUT -Body $profileRequest -Headers $authHeaders
    Write-Host "✓ Profile update successful" -ForegroundColor Green
    Write-Host "Updated Name: $($profileResponse.Data.Nome)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Profile update failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $errorContent = $_.Exception.Response.Content.ReadAsStringAsync().Result
        Write-Host "Error details: $errorContent" -ForegroundColor Red
    }
}

# Test 4: Change password
Write-Host "`n4. Testing Password Change..." -ForegroundColor Yellow
$passwordRequest = @{
    UsuarioId = $userId
    SenhaAtual = "password123"
    NovaSenha = "newpassword123"
    ConfirmarNovaSenha = "newpassword123"
} | ConvertTo-Json

try {
    $passwordResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/change-password" -Method POST -Body $passwordRequest -Headers $authHeaders
    Write-Host "✓ Password change successful" -ForegroundColor Green
} catch {
    Write-Host "✗ Password change failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $errorContent = $_.Exception.Response.Content.ReadAsStringAsync().Result
        Write-Host "Error details: $errorContent" -ForegroundColor Red
    }
}

# Test 5: List user sessions
Write-Host "`n5. Testing Session Listing..." -ForegroundColor Yellow
try {
    $sessionsResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/sessions/$userId" -Method GET -Headers $authHeaders
    Write-Host "✓ Session listing successful" -ForegroundColor Green
    Write-Host "Active sessions: $($sessionsResponse.Data.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Session listing failed: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $errorContent = $_.Exception.Response.Content.ReadAsStringAsync().Result
        Write-Host "Error details: $errorContent" -ForegroundColor Red
    }
}

# Test 6: Create a role (requires admin permissions)
Write-Host "`n6. Testing Role Creation..." -ForegroundColor Yellow
$roleRequest = @{
    Name = "TestRole"
    Description = "Test role for user management"
    PermissionIds = @()
    IsActive = $true
} | ConvertTo-Json

try {
    $roleResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/roles" -Method POST -Body $roleRequest -Headers $authHeaders
    Write-Host "✓ Role creation successful" -ForegroundColor Green
    Write-Host "Role ID: $($roleResponse.Data.Id)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Role creation failed (expected if user doesn't have admin permissions): $($_.Exception.Message)" -ForegroundColor Yellow
}

# Test 7: List roles
Write-Host "`n7. Testing Role Listing..." -ForegroundColor Yellow
try {
    $rolesResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/roles" -Method GET -Headers $authHeaders
    Write-Host "✓ Role listing successful" -ForegroundColor Green
    Write-Host "Total roles: $($rolesResponse.Data.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Role listing failed (expected if user doesn't have admin permissions): $($_.Exception.Message)" -ForegroundColor Yellow
}

# Test 8: List users
Write-Host "`n8. Testing User Listing..." -ForegroundColor Yellow
try {
    $usersResponse = Invoke-RestMethod -Uri "$baseUrl/UserManagement/users?pageNumber=1&pageSize=10" -Method GET -Headers $authHeaders
    Write-Host "✓ User listing successful" -ForegroundColor Green
    Write-Host "Total users: $($usersResponse.Data.TotalCount)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ User listing failed (expected if user doesn't have admin permissions): $($_.Exception.Message)" -ForegroundColor Yellow
}

Write-Host "`nUser Management System Testing Complete!" -ForegroundColor Green
Write-Host "Note: Some tests may fail if the user doesn't have admin permissions, which is expected behavior." -ForegroundColor Cyan