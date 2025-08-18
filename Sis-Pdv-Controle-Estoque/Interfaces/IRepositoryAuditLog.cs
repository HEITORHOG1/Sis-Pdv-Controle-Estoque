using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryAuditLog : IRepositoryBase<AuditLog, Guid>
    {
        Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, Guid entityId);
        Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null);
    }
}