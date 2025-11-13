using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryStockMovement : RepositoryBase<StockMovement, Guid>, IRepositoryStockMovement
    {
        private readonly PdvContext _pdvContext;

        public RepositoryStockMovement(PdvContext context) : base(context)
        {
            _pdvContext = context;
        }

        public async Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovements
                .Where(sm => sm.ProdutoId == productId)
                .Include(sm => sm.Produto)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovement>> GetByProductIdAndDateRangeAsync(Guid productId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovements
                .Where(sm => sm.ProdutoId == productId && 
                           sm.MovementDate >= startDate && 
                           sm.MovementDate <= endDate)
                .Include(sm => sm.Produto)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovement>> GetByTypeAsync(StockMovementType type, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovements
                .Where(sm => sm.Type == type)
                .Include(sm => sm.Produto)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<StockMovement?> GetLastMovementByProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovements
                .Where(sm => sm.ProdutoId == productId)
                .Include(sm => sm.Produto)
                .OrderByDescending(sm => sm.MovementDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<StockMovement>> GetRecentMovementsAsync(int count = 50, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.StockMovements
                .Include(sm => sm.Produto)
                .OrderByDescending(sm => sm.MovementDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}