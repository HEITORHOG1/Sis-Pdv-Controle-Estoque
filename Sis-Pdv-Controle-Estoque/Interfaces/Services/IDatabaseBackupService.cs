using Sis_Pdv_Controle_Estoque.Model.Backup;

namespace Sis_Pdv_Controle_Estoque.Interfaces.Services;

public interface IDatabaseBackupService
{
    Task<BackupResult> CreateBackupAsync(string backupPath, CancellationToken cancellationToken = default);
    Task<BackupVerificationResult> VerifyBackupAsync(string backupPath, CancellationToken cancellationToken = default);
    Task<RestoreResult> RestoreBackupAsync(string backupPath, string? targetDatabase = null, CancellationToken cancellationToken = default);
    Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
}