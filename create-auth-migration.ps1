# PowerShell script to create authentication migration
Write-Host "Creating authentication migration..." -ForegroundColor Green

# Navigate to the infrastructure project
Set-Location "Sis-Pdv-Controle-Estoque-Infra"

# Add migration
dotnet ef migrations add AddAuthenticationEntities --startup-project ../Sis-Pdv-Controle-Estoque-API

Write-Host "Migration created successfully!" -ForegroundColor Green
Write-Host "To apply the migration, run: dotnet ef database update --startup-project ../Sis-Pdv-Controle-Estoque-API" -ForegroundColor Yellow

# Return to root directory
Set-Location ..