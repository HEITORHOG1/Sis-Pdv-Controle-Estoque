using Interfaces.Repositories.Base;
using Model;

namespace Interfaces.Repositories
{
    public interface IRepositoryPayment : IRepositoryBase<Payment>
    {
        Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Payment?> GetWithItemsAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<Payment?> GetWithFiscalReceiptAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalAmountByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    }
}