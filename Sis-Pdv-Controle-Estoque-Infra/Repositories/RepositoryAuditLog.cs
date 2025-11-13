using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryAuditLog : RepositoryBase<AuditLog, Guid>, IRepositoryAuditLog
    {
        private readonly PdvContext _context;

        public RepositoryAuditLog(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, Guid entityId)
        {
            return await _context.AuditLogs
                .Where(al => al.EntityName == entityName && al.EntityId == entityId && !al.IsDeleted)
                .Include(al => al.User)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.AuditLogs
                .Where(al => al.UserId == userId && !al.IsDeleted);

            if (fromDate.HasValue)
                query = query.Where(al => al.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(al => al.Timestamp <= toDate.Value);

            return await query
                .Include(al => al.User)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync();
        }
    }
}