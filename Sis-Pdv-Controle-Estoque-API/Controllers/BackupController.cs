using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque.Interfaces.Services;
using Sis_Pdv_Controle_Estoque.Model.Backup;
using Sis_Pdv_Controle_Estoque_API.Models;

namespace Sis_Pdv_Controle_Estoque_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BackupController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly IBackupService _backupService;
    private readonly ILogger<BackupController> _logger;

    public BackupController(IBackupService backupService, ILogger<BackupController> logger)
    {
        _backupService = backupService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a database backup
    /// </summary>
    [HttpPost("database")]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<BackupResult>>> CreateDatabaseBackup(
        [FromBody] CreateBackupRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new BackupOptions
            {
                Type = BackupType.Database,
                Description = request.Description,
                VerifyAfterBackup = request.VerifyAfterBackup,
                RetentionDays = request.RetentionDays
            };

            var result = await _backupService.CreateDatabaseBackupAsync(options, cancellationToken);
            
            if (result.Success)
            {
                _logger.LogInformation("Database backup created successfully: {BackupPath}", result.BackupPath);
                return Ok(new ApiResponse<BackupResult> { Success = true, Data = result, Message = "Database backup created successfully" });
            }
            else
            {
                _logger.LogWarning("Database backup failed: {Error}", result.ErrorMessage);
                return BadRequest(new ApiResponse<BackupResult> { Success = false, Message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating database backup");
            return StatusCode(500, new ApiResponse<BackupResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Creates a file backup
    /// </summary>
    [HttpPost("files")]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<BackupResult>>> CreateFileBackup(
        [FromBody] CreateFileBackupRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new BackupOptions
            {
                Type = BackupType.Files,
                Description = request.Description,
                VerifyAfterBackup = request.VerifyAfterBackup,
                RetentionDays = request.RetentionDays,
                IncludePaths = (request.IncludePaths ?? Enumerable.Empty<string>()).ToList(),
                ExcludePaths = (request.ExcludePaths ?? Enumerable.Empty<string>()).ToList()
            };

            var result = await _backupService.CreateFileBackupAsync(options, cancellationToken);
            
            if (result.Success)
            {
                _logger.LogInformation("File backup created successfully: {BackupPath}", result.BackupPath);
                return Ok(new ApiResponse<BackupResult> { Success = true, Data = result, Message = "File backup created successfully" });
            }
            else
            {
                _logger.LogWarning("File backup failed: {Error}", result.ErrorMessage);
                return BadRequest(new ApiResponse<BackupResult> { Success = false, Message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating file backup");
            return StatusCode(500, new ApiResponse<BackupResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Creates a full backup (database + files)
    /// </summary>
    [HttpPost("full")]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<BackupResult>>> CreateFullBackup(
        [FromBody] CreateFileBackupRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new BackupOptions
            {
                Type = BackupType.Full,
                Description = request.Description,
                VerifyAfterBackup = request.VerifyAfterBackup,
                RetentionDays = request.RetentionDays,
                IncludePaths = (request.IncludePaths ?? Enumerable.Empty<string>()).ToList(),
                ExcludePaths = (request.ExcludePaths ?? Enumerable.Empty<string>()).ToList()
            };

            var result = await _backupService.CreateFullBackupAsync(options, cancellationToken);
            
            if (result.Success)
            {
                _logger.LogInformation("Full backup created successfully: {BackupPath}", result.BackupPath);
                return Ok(new ApiResponse<BackupResult> { Success = true, Data = result, Message = "Full backup created successfully" });
            }
            else
            {
                _logger.LogWarning("Full backup failed: {Error}", result.ErrorMessage);
                return BadRequest(new ApiResponse<BackupResult> { Success = false, Message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating full backup");
            return StatusCode(500, new ApiResponse<BackupResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Verifies a backup file
    /// </summary>
    [HttpPost("verify")]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<BackupVerificationResult>>> VerifyBackup(
        [FromBody] VerifyBackupRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _backupService.VerifyBackupAsync(request.BackupPath, cancellationToken);
            
            if (result.IsValid)
            {
                _logger.LogInformation("Backup verification successful: {BackupPath}", request.BackupPath);
                return Ok(new ApiResponse<BackupVerificationResult> { Success = true, Data = result, Message = "Backup verification successful" });
            }
            else
            {
                _logger.LogWarning("Backup verification failed: {BackupPath}, Error: {Error}", 
                    request.BackupPath, result.ErrorMessage);
                return Ok(new ApiResponse<BackupVerificationResult> { Success = true, Data = result, Message = "Backup verification completed" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying backup: {BackupPath}", request.BackupPath);
            return StatusCode(500, new ApiResponse<BackupVerificationResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Restores database from backup
    /// </summary>
    [HttpPost("restore/database")]
    [Authorize(Policy = "RequireRestorePermission")]
    public async Task<ActionResult<ApiResponse<RestoreResult>>> RestoreDatabase(
        [FromBody] RestoreDatabaseRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new RestoreOptions
            {
                BackupPath = request.BackupPath,
                TargetDatabase = request.TargetDatabase,
                VerifyBeforeRestore = request.VerifyBeforeRestore,
                CreateBackupBeforeRestore = request.CreateBackupBeforeRestore
            };

            var result = await _backupService.RestoreDatabaseAsync(options, cancellationToken);
            
            if (result.Success)
            {
                _logger.LogInformation("Database restore completed successfully from: {BackupPath}", request.BackupPath);
                return Ok(new ApiResponse<RestoreResult> { Success = true, Data = result, Message = "Database restore completed successfully" });
            }
            else
            {
                _logger.LogWarning("Database restore failed: {Error}", result.ErrorMessage);
                return BadRequest(new ApiResponse<RestoreResult> { Success = false, Message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring database from: {BackupPath}", request.BackupPath);
            return StatusCode(500, new ApiResponse<RestoreResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Restores files from backup
    /// </summary>
    [HttpPost("restore/files")]
    [Authorize(Policy = "RequireRestorePermission")]
    public async Task<ActionResult<ApiResponse<RestoreResult>>> RestoreFiles(
        [FromBody] RestoreFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new RestoreOptions
            {
                BackupPath = request.BackupPath,
                TargetPath = request.TargetPath,
                VerifyBeforeRestore = request.VerifyBeforeRestore,
                OverwriteExisting = request.OverwriteExisting
            };

            var result = await _backupService.RestoreFilesAsync(options, cancellationToken);
            
            if (result.Success)
            {
                _logger.LogInformation("File restore completed successfully from: {BackupPath} to: {TargetPath}", 
                    request.BackupPath, request.TargetPath);
                return Ok(new ApiResponse<RestoreResult> { Success = true, Data = result, Message = "File restore completed successfully" });
            }
            else
            {
                _logger.LogWarning("File restore failed: {Error}", result.ErrorMessage);
                return BadRequest(new ApiResponse<RestoreResult> { Success = false, Message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring files from: {BackupPath}", request.BackupPath);
            return StatusCode(500, new ApiResponse<RestoreResult> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Lists all available backups
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BackupInfo>>>> ListBackups(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var backups = await _backupService.ListBackupsAsync(cancellationToken);
            return Ok(new ApiResponse<IEnumerable<BackupInfo>> { Success = true, Data = backups, Message = "Backups retrieved successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing backups");
            return StatusCode(500, new ApiResponse<IEnumerable<BackupInfo>> { Success = false, Message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Deletes a backup
    /// </summary>
    [HttpDelete("{backupId}")]
    [Authorize(Policy = "RequireBackupPermission")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBackup(
        string backupId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, you'd map backupId to actual file path
            // For now, we'll assume backupId is the file name
            var backups = await _backupService.ListBackupsAsync(cancellationToken);
            var backup = backups.FirstOrDefault(b => b.Name == backupId);
            
            if (backup == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Backup not found" });
            }

            var result = await _backupService.DeleteBackupAsync(backup.Path, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Backup deleted successfully: {BackupId}", backupId);
                return Ok(new ApiResponse<bool> { Success = true, Data = true, Message = "Backup deleted successfully" });
            }
            else
            {
                return BadRequest(new ApiResponse<bool> { Success = false, Message = "Failed to delete backup" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backup: {BackupId}", backupId);
            return StatusCode(500, new ApiResponse<bool> { Success = false, Message = "Internal server error occurred" });
        }
    }
}

// Request DTOs
public class CreateBackupRequest
{
    public string Description { get; set; } = string.Empty;
    public bool VerifyAfterBackup { get; set; } = true;
    public int RetentionDays { get; set; } = 30;
}

public class CreateFileBackupRequest : CreateBackupRequest
{
    public IEnumerable<string>? IncludePaths { get; set; }
    public IEnumerable<string>? ExcludePaths { get; set; }
}

public class VerifyBackupRequest
{
    public string BackupPath { get; set; } = string.Empty;
}

public class RestoreDatabaseRequest
{
    public string BackupPath { get; set; } = string.Empty;
    public string? TargetDatabase { get; set; }
    public bool VerifyBeforeRestore { get; set; } = true;
    public bool CreateBackupBeforeRestore { get; set; } = true;
}

public class RestoreFilesRequest
{
    public string BackupPath { get; set; } = string.Empty;
    public string TargetPath { get; set; } = string.Empty;
    public bool VerifyBeforeRestore { get; set; } = true;
    public bool OverwriteExisting { get; set; } = false;
}