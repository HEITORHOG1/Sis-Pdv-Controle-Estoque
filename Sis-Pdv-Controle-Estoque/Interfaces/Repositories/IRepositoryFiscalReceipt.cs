using Interfaces.Repositories.Base;
using Model;

namespace Interfaces.Repositories
{
    public interface IRepositoryFiscalReceipt : IRepositoryBase<FiscalReceipt, Guid>
    {
        Task<FiscalReceipt?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<FiscalReceipt?> GetByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken = default);
        Task<FiscalReceipt?> GetByAccessKeyAsync(string accessKey, CancellationToken cancellationToken = default);
        Task<IEnumerable<FiscalReceipt>> GetByStatusAsync(FiscalReceiptStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<FiscalReceipt>> GetPendingReceiptsAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateNextReceiptNumberAsync(CancellationToken cancellationToken = default);
        Task<FiscalReceipt?> BuscarPorIdAsync(Guid id);

        Task<FiscalReceipt> AlterarAsync(FiscalReceipt entity);
    }
}