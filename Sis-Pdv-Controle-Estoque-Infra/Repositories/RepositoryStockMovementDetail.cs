using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryStockMovementDetail : RepositoryBase<StockMovementDetail, Guid>, IRepositoryStockMovementDetail
    {
        private readonly PdvContext _pdvContext;
        
        public RepositoryStockMovementDetail(PdvContext context) : base(context)
        {
            _pdvContext = context;
        }

        public async Task<IEnumerable<StockMovementDetail>> GetByStockMovementIdAsync(Guid stockMovementId, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovementDetails
                .Include(smd => smd.StockMovement)
                .Where(smd => smd.StockMovementId == stockMovementId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovementDetail>> GetByLoteAsync(string lote, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovementDetails
                .Include(smd => smd.StockMovement)
                .Where(smd => smd.Lote == lote)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovementDetail>> GetExpiringDetailsAsync(DateTime beforeDate, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovementDetails
                .Include(smd => smd.StockMovement)
                .Where(smd => smd.DataValidade.HasValue && smd.DataValidade.Value <= beforeDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovementDetail>> GetExpiredDetailsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            return await _pdvContext.StockMovementDetails
                .Include(smd => smd.StockMovement)
                .Where(smd => smd.DataValidade.HasValue && smd.DataValidade.Value < now)
                .ToListAsync(cancellationToken);
        }
    }
}