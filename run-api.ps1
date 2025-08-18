param(
    [string]$Configuration = "Debug",
    [int]$Port = 7003
)

$ErrorActionPreference = 'Stop'
Write-Host "Encerrando instâncias antigas..." -ForegroundColor Yellow
Get-Process -Name 'Sis-Pdv-Controle-Estoque-API' -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue | ForEach-Object {
    try { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue } catch {}
}
Start-Sleep -Seconds 1

$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:ASPNETCORE_URLS = "http://localhost:$Port"

$sln = "c:\Users\heito\Documents\projetos\Sis-Pdv-Controle-Estoque\Sis-Pdv-Controle-Estoque.sln"
$api = "c:\Users\heito\Documents\projetos\Sis-Pdv-Controle-Estoque\Sis-Pdv-Controle-Estoque-API\Sis-Pdv-Controle-Estoque-API.csproj"

Write-Host "Limpando solução..." -ForegroundColor Yellow
& dotnet clean $sln -c $Configuration -v minimal | Write-Host

Write-Host "Compilando solução..." -ForegroundColor Yellow
& dotnet build $sln -c $Configuration -v minimal | Write-Host

Write-Host "Iniciando API em http://localhost:$Port ..." -ForegroundColor Green
& dotnet run -c $Configuration --no-build -p $api
