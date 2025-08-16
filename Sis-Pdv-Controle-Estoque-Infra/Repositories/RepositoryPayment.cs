using Interfaces.Repositories;
using Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryPayment : RepositoryBase<Payment, Guid>, IRepositoryPayment
    {
        public RepositoryPayment(PdvContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Payment>()
                .Include(p => p.PaymentItems)
                .Include(p => p.FiscalReceipt)
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<Payment?> GetWithItemsAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Payment>()
                .Include(p => p.PaymentItems)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted, cancellationToken);
        }

        public async Task<Payment?> GetWithFiscalReceiptAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Payment>()
                .Include(p => p.PaymentItems)
                .Include(p => p.FiscalReceipt)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Payment>()
                .Include(p => p.PaymentItems)
                .Where(p => p.Status == status && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Payment>()
                .Include(p => p.PaymentItems)
                .Include(p => p.Order)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && !p.IsDeleted)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalAmountByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.Set<Payment>()
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate < endDate 
                           && p.Status == PaymentStatus.Processed && !p.IsDeleted)
                .SumAsync(p => p.TotalAmount, cancellationToken);
        }

        // Implement missing methods from IRepositoryBase
        public async Task<Payment?> BuscarPorIdAsync(Guid id)
        {
            return await GetWithItemsAsync(id);
        }

        public async Task<Payment> AlterarAsync(Payment entity)
        {
            _context.Set<Payment>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await GetByDateRangeAsync(startDate, endDate, cancellationToken);
        }
    }
}