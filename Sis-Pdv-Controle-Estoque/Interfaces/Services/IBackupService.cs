using Sis_Pdv_Controle_Estoque.Model.Backup;

namespace Sis_Pdv_Controle_Estoque.Interfaces.Services;

public interface IBackupService
{
    Task<BackupResult> CreateDatabaseBackupAsync(BackupOptions options, CancellationToken cancellationToken = default);
    Task<BackupResult> CreateFileBackupAsync(BackupOptions options, CancellationToken cancellationToken = default);
    Task<BackupResult> CreateFullBackupAsync(BackupOptions options, CancellationToken cancellationToken = default);
    Task<BackupVerificationResult> VerifyBackupAsync(string backupPath, CancellationToken cancellationToken = default);
    Task<RestoreResult> RestoreDatabaseAsync(RestoreOptions options, CancellationToken cancellationToken = default);
    Task<RestoreResult> RestoreFilesAsync(RestoreOptions options, CancellationToken cancellationToken = default);
    Task<IEnumerable<BackupInfo>> ListBackupsAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteBackupAsync(string backupPath, CancellationToken cancellationToken = default);
}