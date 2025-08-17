# Dev helper: stop locked processes, clean, build, run API on http://localhost:7003 and verify endpoints
param(
    [int]$Port = 7003,
    [switch]$NoBuild,
    [switch]$Foreground
)

$ErrorActionPreference = 'Stop'
Write-Host "== PDV API dev-run ==" -ForegroundColor Cyan

function Stop-LockingProcesses {
    param([int]$Port)
    Write-Host "Stopping running API processes and freeing port $Port..." -ForegroundColor Yellow
    try {
        Get-Process -Name 'Sis-Pdv-Controle-Estoque-API' -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
    } catch {}
    try {
        # Kill any dotnet process hosting our API assembly
        Get-CimInstance Win32_Process | Where-Object { $_.Name -eq 'dotnet.exe' -and ($_.CommandLine -match 'Sis-Pdv-Controle-Estoque-API\.dll' -or $_.CommandLine -match 'Sis-Pdv-Controle-Estoque-API\.csproj') } |
            ForEach-Object { try { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue } catch {} }
    } catch {}
    try {
        # Free port if still occupied
        Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue |
            ForEach-Object { try { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue } catch {} }
    } catch {}
}

function Test-Endpoint {
    param([string]$Url)
    try {
        $resp = Invoke-WebRequest -UseBasicParsing -TimeoutSec 10 -Uri $Url
        return @{ Url = $Url; Status = $resp.StatusCode; ContentType = $resp.Headers['Content-Type']; Length = $resp.RawContentLength }
    } catch {
        return @{ Url = $Url; Error = $_.Exception.Message }
    }
}

function Wait-ForUrl {
    param([string]$Url, [int]$TimeoutSec = 30)
    $sw = [Diagnostics.Stopwatch]::StartNew()
    while ($sw.Elapsed.TotalSeconds -lt $TimeoutSec) {
        try {
            $r = Invoke-WebRequest -UseBasicParsing -TimeoutSec 5 -Uri $Url
            if ($r.StatusCode -ge 200 -and $r.StatusCode -lt 500) { return $true }
        } catch {}
        Start-Sleep -Milliseconds 500
    }
    return $false
}

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Stop-LockingProcesses -Port $Port

if (-not $NoBuild) {
    Write-Host "Cleaning and building solution..." -ForegroundColor Yellow
    dotnet clean "$root\Sis-Pdv-Controle-Estoque.sln" -c Debug -v minimal
    dotnet build "$root\Sis-Pdv-Controle-Estoque.sln" -c Debug -v minimal
}

Write-Host "Starting API on http://localhost:$Port ..." -ForegroundColor Yellow
$env:ASPNETCORE_URLS = "http://localhost:$Port"

$apiProj = "$root\Sis-Pdv-Controle-Estoque-API\Sis-Pdv-Controle-Estoque-API.csproj"
if ($Foreground) {
    dotnet run -c Debug --no-build -p $apiProj
    exit $LASTEXITCODE
} else {
    # Start in background window
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = "dotnet"
    $psi.Arguments = "run -c Debug --no-build -p `"$apiProj`""
    $psi.UseShellExecute = $true
    $psi.WorkingDirectory = "$root"
    $proc = [System.Diagnostics.Process]::Start($psi)
}

# Wait API become ready
if (-not (Wait-ForUrl -Url "http://localhost:$Port/health" -TimeoutSec 40)) {
    Write-Host "API did not become ready within timeout." -ForegroundColor Red
} else {
    Write-Host "API is up." -ForegroundColor Green
}

# Smoke tests
$checks = @(
    "http://localhost:$Port/swagger/v1/swagger.json",
    "http://localhost:$Port/api-docs",
    "http://localhost:$Port/health"
)
$results = $checks | ForEach-Object { Test-Endpoint -Url $_ }
$results | ForEach-Object {
    if ($_.Error) { Write-Host ("[FAIL] {0} -> {1}" -f $_.Url, $_.Error) -ForegroundColor Red }
    else { Write-Host ("[OK] {0} -> {1} {2} length={3}" -f $_.Url, $_.Status, $_.ContentType, $_.Length) -ForegroundColor Green }
}

Write-Host "Done." -ForegroundColor Cyan
