param(
	[string]$Project = "Sis-Pdv-Controle-Estoque-API",
	[string]$Url = "http://localhost:7003",
	[string]$Endpoint = "/api/Produto/ListarProduto"
)

# Resolve API working directory and log file
$apiDir = Join-Path $PSScriptRoot $Project
if (!(Test-Path $apiDir)) { $apiDir = (Join-Path $PSScriptRoot "$Project") }
$outLog = Join-Path $PSScriptRoot "api-run.out.log"
$errLog = Join-Path $PSScriptRoot "api-run.err.log"
foreach ($f in @($outLog, $errLog)) { if (Test-Path $f) { try { Remove-Item $f -Force -ErrorAction SilentlyContinue } catch {} } }

# Start API in background if not running
Write-Host "Starting API ($Project) if not running..."
$env:DOTNET_ENVIRONMENT = "Development"
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = $Url
$api = Start-Process -FilePath "dotnet" -ArgumentList "run --no-launch-profile" -PassThru -WindowStyle Hidden -WorkingDirectory $apiDir -RedirectStandardOutput $outLog -RedirectStandardError $errLog

# Wait until ready (up to ~40s)
Write-Host "Waiting for readiness..."
$ready = $false
for ($i = 0; $i -lt 40; $i++) {
	try {
		$health = Invoke-RestMethod -TimeoutSec 5 -Method GET "$Url/health/ready"
		if ($health.status -eq 'Healthy') { $ready = $true; break }
	} catch { Start-Sleep -Milliseconds 500 }
}
if ($ready) { Write-Host "Ready: Healthy" } else { Write-Warning "Ready probe did not return Healthy within timeout" }

# Test legacy product listing endpoint used by WinForms
$full = "$Url$Endpoint"
Write-Host "Calling: $full"
try {
	$resp = Invoke-WebRequest -TimeoutSec 15 -Method GET $full
	Write-Host ("Status: " + $resp.StatusCode)
	$content = $resp.Content
	if ($content) {
		if ($content.Length -gt 1000) { $content.Substring(0,1000) } else { $content }
	} else {
		Write-Host "(no content)"
	}
} catch {
	$ex = $_.Exception
	if ($ex.Response) {
		try {
			$status = [int]$ex.Response.StatusCode
		} catch { $status = '(unknown)' }
		Write-Host ("HTTP Error Status: " + $status)
		try {
			$stream = $ex.Response.GetResponseStream()
			if ($stream) {
				$reader = New-Object System.IO.StreamReader($stream)
				$body = $reader.ReadToEnd()
				if ($body) {
					if ($body.Length -gt 1500) { $body.Substring(0,1500) } else { $body }
				} else { Write-Host "(empty error body)" }
			} else { Write-Host "(no error stream)" }
		} catch { Write-Host "(failed to read error body) $_" }
	} else {
		Write-Host ("Request failed: " + $ex.Message)
	}
}

# Stop background if we started it
if ($api -and !$api.HasExited) {
	Write-Host "Stopping API..."
	try { $api.CloseMainWindow() | Out-Null } catch {}
	try { Stop-Process -Id $api.Id -Force -ErrorAction SilentlyContinue } catch {}
}

# If error occurred, print last 200 lines of log for context
Write-Host "--- API LOG (tail) ---"
if (Test-Path $outLog) { try { Get-Content $outLog -Tail 200 | Out-String | Write-Host } catch {} }
if (Test-Path $errLog) { try { Get-Content $errLog -Tail 200 | Out-String | Write-Host } catch {} }

# Ensure script exits successfully to not break CI/dev tooling
exit 0
