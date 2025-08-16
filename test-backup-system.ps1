# Test script for Backup and Recovery System
# This script tests all backup and recovery functionality

$baseUrl = "https://localhost:7297/api"
$loginUrl = "$baseUrl/auth/login"
$backupUrl = "$baseUrl/backup"

Write-Host "=== PDV Backup System Test ===" -ForegroundColor Green

# Function to get auth token
function Get-AuthToken {
    $loginData = @{
        Username = "admin"
        Password = "Admin123!"
    } | ConvertTo-Json

    try {
        $response = Invoke-RestMethod -Uri $loginUrl -Method Post -Body $loginData -ContentType "application/json" -SkipCertificateCheck
        return $response.data.token
    }
    catch {
        Write-Host "Failed to authenticate: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

# Function to make authenticated requests
function Invoke-AuthenticatedRequest {
    param(
        [string]$Uri,
        [string]$Method = "GET",
        [object]$Body = $null,
        [string]$Token
    )
    
    $headers = @{
        "Authorization" = "Bearer $Token"
        "Content-Type" = "application/json"
    }
    
    try {
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Body $jsonBody -Headers $headers -SkipCertificateCheck
        } else {
            $response = Invoke-RestMethod -Uri $Uri -Method $Method -Headers $headers -SkipCertificateCheck
        }
        return $response
    }
    catch {
        Write-Host "Request failed: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response: $responseBody" -ForegroundColor Yellow
        }
        return $null
    }
}

# Get authentication token
Write-Host "`n1. Authenticating..." -ForegroundColor Yellow
$token = Get-AuthToken
if (-not $token) {
    Write-Host "Authentication failed. Exiting." -ForegroundColor Red
    exit 1
}
Write-Host "Authentication successful!" -ForegroundColor Green

# Test 1: List existing backups
Write-Host "`n2. Listing existing backups..." -ForegroundColor Yellow
$backups = Invoke-AuthenticatedRequest -Uri $backupUrl -Token $token
if ($backups) {
    Write-Host "Found $($backups.data.Count) existing backups" -ForegroundColor Green
    foreach ($backup in $backups.data) {
        Write-Host "  - $($backup.name) ($($backup.formattedSize)) - $($backup.type)" -ForegroundColor Cyan
    }
} else {
    Write-Host "Failed to list backups" -ForegroundColor Red
}

# Test 2: Create database backup
Write-Host "`n3. Creating database backup..." -ForegroundColor Yellow
$dbBackupRequest = @{
    Description = "Test database backup - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    VerifyAfterBackup = $true
    RetentionDays = 7
}

$dbBackupResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/database" -Method "POST" -Body $dbBackupRequest -Token $token
if ($dbBackupResult -and $dbBackupResult.success) {
    Write-Host "Database backup created successfully!" -ForegroundColor Green
    Write-Host "  Path: $($dbBackupResult.data.backupPath)" -ForegroundColor Cyan
    Write-Host "  Size: $('{0:N2}' -f ($dbBackupResult.data.backupSize / 1MB)) MB" -ForegroundColor Cyan
    Write-Host "  Duration: $($dbBackupResult.data.duration)" -ForegroundColor Cyan
    
    if ($dbBackupResult.data.verificationResult) {
        $verification = $dbBackupResult.data.verificationResult
        Write-Host "  Verification: $(if ($verification.isValid) { 'PASSED' } else { 'FAILED' })" -ForegroundColor $(if ($verification.isValid) { 'Green' } else { 'Red' })
    }
} else {
    Write-Host "Database backup failed!" -ForegroundColor Red
}

# Test 3: Create file backup
Write-Host "`n4. Creating file backup..." -ForegroundColor Yellow
$fileBackupRequest = @{
    Description = "Test file backup - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    VerifyAfterBackup = $true
    RetentionDays = 7
    IncludePaths = @("appsettings.json", "logs")
    ExcludePaths = @()
}

$fileBackupResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/files" -Method "POST" -Body $fileBackupRequest -Token $token
if ($fileBackupResult -and $fileBackupResult.success) {
    Write-Host "File backup created successfully!" -ForegroundColor Green
    Write-Host "  Path: $($fileBackupResult.data.backupPath)" -ForegroundColor Cyan
    Write-Host "  Size: $('{0:N2}' -f ($fileBackupResult.data.backupSize / 1KB)) KB" -ForegroundColor Cyan
    Write-Host "  Duration: $($fileBackupResult.data.duration)" -ForegroundColor Cyan
    
    if ($fileBackupResult.data.verificationResult) {
        $verification = $fileBackupResult.data.verificationResult
        Write-Host "  Verification: $(if ($verification.isValid) { 'PASSED' } else { 'FAILED' })" -ForegroundColor $(if ($verification.isValid) { 'Green' } else { 'Red' })
    }
} else {
    Write-Host "File backup failed!" -ForegroundColor Red
}

# Test 4: Create full backup
Write-Host "`n5. Creating full backup..." -ForegroundColor Yellow
$fullBackupRequest = @{
    Description = "Test full backup - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    VerifyAfterBackup = $true
    RetentionDays = 30
    IncludePaths = @("appsettings.json", "logs")
    ExcludePaths = @()
}

$fullBackupResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/full" -Method "POST" -Body $fullBackupRequest -Token $token
if ($fullBackupResult -and $fullBackupResult.success) {
    Write-Host "Full backup created successfully!" -ForegroundColor Green
    Write-Host "  Path: $($fullBackupResult.data.backupPath)" -ForegroundColor Cyan
    Write-Host "  Size: $('{0:N2}' -f ($fullBackupResult.data.backupSize / 1MB)) MB" -ForegroundColor Cyan
    Write-Host "  Duration: $($fullBackupResult.data.duration)" -ForegroundColor Cyan
    
    $metadata = $fullBackupResult.data.metadata
    if ($metadata) {
        Write-Host "  Database backup: $($metadata.DatabaseBackupPath)" -ForegroundColor Cyan
        Write-Host "  File backup: $($metadata.FileBackupPath)" -ForegroundColor Cyan
    }
} else {
    Write-Host "Full backup failed!" -ForegroundColor Red
}

# Test 5: Verify a backup
if ($dbBackupResult -and $dbBackupResult.success) {
    Write-Host "`n6. Verifying database backup..." -ForegroundColor Yellow
    $verifyRequest = @{
        BackupPath = $dbBackupResult.data.backupPath
    }
    
    $verifyResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/verify" -Method "POST" -Body $verifyRequest -Token $token
    if ($verifyResult -and $verifyResult.success) {
        $verification = $verifyResult.data
        Write-Host "Backup verification completed!" -ForegroundColor Green
        Write-Host "  Valid: $(if ($verification.isValid) { 'YES' } else { 'NO' })" -ForegroundColor $(if ($verification.isValid) { 'Green' } else { 'Red' })
        Write-Host "  Actual size: $('{0:N2}' -f ($verification.actualSize / 1MB)) MB" -ForegroundColor Cyan
        Write-Host "  Checksum: $($verification.actualChecksum)" -ForegroundColor Cyan
        
        if ($verification.validationErrors -and $verification.validationErrors.Count -gt 0) {
            Write-Host "  Validation errors:" -ForegroundColor Yellow
            foreach ($error in $verification.validationErrors) {
                Write-Host "    - $error" -ForegroundColor Yellow
            }
        }
    } else {
        Write-Host "Backup verification failed!" -ForegroundColor Red
    }
}

# Test 6: List backups again to see new ones
Write-Host "`n7. Listing backups after creation..." -ForegroundColor Yellow
$updatedBackups = Invoke-AuthenticatedRequest -Uri $backupUrl -Token $token
if ($updatedBackups) {
    Write-Host "Found $($updatedBackups.data.Count) total backups" -ForegroundColor Green
    foreach ($backup in $updatedBackups.data) {
        $age = [math]::Round((New-TimeSpan -Start $backup.createdAt -End (Get-Date)).TotalHours, 1)
        Write-Host "  - $($backup.name) ($($backup.formattedSize)) - $($backup.type) - ${age}h ago" -ForegroundColor Cyan
    }
} else {
    Write-Host "Failed to list updated backups" -ForegroundColor Red
}

# Test 7: Test restore functionality (database restore to test database)
if ($dbBackupResult -and $dbBackupResult.success) {
    Write-Host "`n8. Testing database restore (to test database)..." -ForegroundColor Yellow
    $restoreRequest = @{
        BackupPath = $dbBackupResult.data.backupPath
        TargetDatabase = "PDV_TEST_RESTORE"
        VerifyBeforeRestore = $true
        CreateBackupBeforeRestore = $false
    }
    
    $restoreResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/restore/database" -Method "POST" -Body $restoreRequest -Token $token
    if ($restoreResult -and $restoreResult.success) {
        Write-Host "Database restore test completed successfully!" -ForegroundColor Green
        Write-Host "  Target database: $($restoreRequest.TargetDatabase)" -ForegroundColor Cyan
        Write-Host "  Duration: $($restoreResult.data.duration)" -ForegroundColor Cyan
        Write-Host "  Restored size: $('{0:N2}' -f ($restoreResult.data.restoredSize / 1MB)) MB" -ForegroundColor Cyan
    } else {
        Write-Host "Database restore test failed!" -ForegroundColor Red
    }
}

# Test 8: Test file restore functionality
if ($fileBackupResult -and $fileBackupResult.success) {
    Write-Host "`n9. Testing file restore..." -ForegroundColor Yellow
    $testRestoreDir = "test_restore_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    
    $fileRestoreRequest = @{
        BackupPath = $fileBackupResult.data.backupPath
        TargetPath = $testRestoreDir
        VerifyBeforeRestore = $true
        OverwriteExisting = $true
    }
    
    $fileRestoreResult = Invoke-AuthenticatedRequest -Uri "$backupUrl/restore/files" -Method "POST" -Body $fileRestoreRequest -Token $token
    if ($fileRestoreResult -and $fileRestoreResult.success) {
        Write-Host "File restore test completed successfully!" -ForegroundColor Green
        Write-Host "  Target path: $testRestoreDir" -ForegroundColor Cyan
        Write-Host "  Files restored: $($fileRestoreResult.data.restoredFileCount)" -ForegroundColor Cyan
        Write-Host "  Duration: $($fileRestoreResult.data.duration)" -ForegroundColor Cyan
        
        if ($fileRestoreResult.data.restoredItems -and $fileRestoreResult.data.restoredItems.Count -gt 0) {
            Write-Host "  Restored files:" -ForegroundColor Cyan
            foreach ($item in $fileRestoreResult.data.restoredItems | Select-Object -First 5) {
                Write-Host "    - $item" -ForegroundColor Gray
            }
            if ($fileRestoreResult.data.restoredItems.Count -gt 5) {
                Write-Host "    ... and $($fileRestoreResult.data.restoredItems.Count - 5) more" -ForegroundColor Gray
            }
        }
        
        # Clean up test restore directory
        if (Test-Path $testRestoreDir) {
            Remove-Item -Path $testRestoreDir -Recurse -Force
            Write-Host "  Cleaned up test restore directory" -ForegroundColor Gray
        }
    } else {
        Write-Host "File restore test failed!" -ForegroundColor Red
    }
}

Write-Host "`n=== Backup System Test Completed ===" -ForegroundColor Green
Write-Host "All backup and recovery functionality has been tested." -ForegroundColor Cyan
Write-Host "Check the 'backups' directory for created backup files." -ForegroundColor Cyan