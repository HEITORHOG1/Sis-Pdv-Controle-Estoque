using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories
{
    public class RepositoryFiscalReceipt : RepositoryBase<FiscalReceipt, Guid>, IRepositoryFiscalReceipt
    {
        public RepositoryFiscalReceipt(PdvContext context) : base(context)
        {
        }

        public async Task<FiscalReceipt?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .FirstOrDefaultAsync(fr => fr.PaymentId == paymentId && !fr.IsDeleted, cancellationToken);
        }

        public async Task<FiscalReceipt?> GetByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .FirstOrDefaultAsync(fr => fr.ReceiptNumber == receiptNumber && !fr.IsDeleted, cancellationToken);
        }

        public async Task<FiscalReceipt?> GetByAccessKeyAsync(string accessKey, CancellationToken cancellationToken = default)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .FirstOrDefaultAsync(fr => fr.AccessKey == accessKey && !fr.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<FiscalReceipt>> GetByStatusAsync(FiscalReceiptStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .Where(fr => fr.Status == status && !fr.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<FiscalReceipt>> GetPendingReceiptsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .Where(fr => (fr.Status == FiscalReceiptStatus.Pending || fr.Status == FiscalReceiptStatus.Sent) 
                           && !fr.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<string> GenerateNextReceiptNumberAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;
            var prefix = today.ToString("yyyyMMdd");
            
            var lastReceipt = await _context.Set<FiscalReceipt>()
                .Where(fr => fr.ReceiptNumber.StartsWith(prefix) && !fr.IsDeleted)
                .OrderByDescending(fr => fr.ReceiptNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastReceipt == null)
            {
                return $"{prefix}001";
            }

            var lastNumber = lastReceipt.ReceiptNumber.Substring(8);
            if (int.TryParse(lastNumber, out var number))
            {
                return $"{prefix}{(number + 1):D3}";
            }

            return $"{prefix}001";
        }

        // Implement missing methods from IRepositoryBase
        public async Task<FiscalReceipt?> BuscarPorIdAsync(Guid id)
        {
            return await _context.Set<FiscalReceipt>()
                .Include(fr => fr.Payment)
                .FirstOrDefaultAsync(fr => fr.Id == id && !fr.IsDeleted);
        }

        public async Task<FiscalReceipt> AdicionarAsync(FiscalReceipt entity)
        {
            await _context.Set<FiscalReceipt>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FiscalReceipt> AlterarAsync(FiscalReceipt entity)
        {
            _context.Set<FiscalReceipt>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}