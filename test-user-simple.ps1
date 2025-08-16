# Simple User Management Test

Write-Host "Testing User Management System..." -ForegroundColor Green

$baseUrl = "https://localhost:7297/api"
$headers = @{
    "Content-Type" = "application/json"
    "Accept" = "application/json"
}

# Test user registration
Write-Host "1. Testing User Registration..." -ForegroundColor Yellow

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
    Write-Host "User registration successful" -ForegroundColor Green
    Write-Host "User ID: $($registerResponse.Data.Id)" -ForegroundColor Cyan
} catch {
    Write-Host "User registration failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test login
Write-Host "2. Testing User Login..." -ForegroundColor Yellow

$loginRequest = @{
    Login = "testuser"
    Password = "password123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginRequest -Headers $headers
    Write-Host "User login successful" -ForegroundColor Green
    Write-Host "Access Token received" -ForegroundColor Cyan
} catch {
    Write-Host "User login failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "User Management System Testing Complete!" -ForegroundColor Green