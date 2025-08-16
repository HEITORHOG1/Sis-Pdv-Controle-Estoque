using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryPaymentAudit : RepositoryBase<PaymentAudit, Guid>, IRepositoryPaymentAudit
    {
        public RepositoryPaymentAudit(PdvContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PaymentAudit>> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<PaymentAudit>()
                .Include(pa => pa.Payment)
                .Include(pa => pa.User)
                .Where(pa => pa.PaymentId == paymentId && !pa.IsDeleted)
                .OrderByDescending(pa => pa.ActionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PaymentAudit>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<PaymentAudit>()
                .Include(pa => pa.Payment)
                .Include(pa => pa.User)
                .Where(pa => pa.UserId == userId && !pa.IsDeleted)
                .OrderByDescending(pa => pa.ActionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PaymentAudit>> GetByActionAsync(PaymentAuditAction action, CancellationToken cancellationToken = default)
        {
            return await _context.Set<PaymentAudit>()
                .Include(pa => pa.Payment)
                .Include(pa => pa.User)
                .Where(pa => pa.Action == action && !pa.IsDeleted)
                .OrderByDescending(pa => pa.ActionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PaymentAudit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<PaymentAudit>()
                .Include(pa => pa.Payment)
                .Include(pa => pa.User)
                .Where(pa => pa.ActionDate >= startDate && pa.ActionDate <= endDate && !pa.IsDeleted)
                .OrderByDescending(pa => pa.ActionDate)
                .ToListAsync(cancellationToken);
        }

        // Implement missing methods from IRepositoryBase
        public async Task<PaymentAudit> AdicionarAsync(PaymentAudit entity)
        {
            await _context.Set<PaymentAudit>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}