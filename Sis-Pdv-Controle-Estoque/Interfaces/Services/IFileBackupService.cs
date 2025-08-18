using Sis_Pdv_Controle_Estoque.Model.Backup;

namespace Sis_Pdv_Controle_Estoque.Interfaces.Services;

public interface IFileBackupService
{
    Task<BackupResult> CreateBackupAsync(IEnumerable<string> sourcePaths, string backupPath, CancellationToken cancellationToken = default);
    Task<BackupVerificationResult> VerifyBackupAsync(string backupPath, CancellationToken cancellationToken = default);
    Task<RestoreResult> RestoreBackupAsync(string backupPath, string targetPath, CancellationToken cancellationToken = default);
    Task<long> CalculateBackupSizeAsync(IEnumerable<string> sourcePaths, CancellationToken cancellationToken = default);
}