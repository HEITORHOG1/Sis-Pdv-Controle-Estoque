using Interfaces.Repositories.Base;
using Model;

namespace Interfaces.Repositories
{
    public interface IRepositoryPaymentAudit : IRepositoryBase<PaymentAudit>
    {
        Task<IEnumerable<PaymentAudit>> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentAudit>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentAudit>> GetByActionAsync(PaymentAuditAction action, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentAudit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}