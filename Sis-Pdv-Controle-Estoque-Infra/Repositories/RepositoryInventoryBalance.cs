using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryInventoryBalance : RepositoryBase<InventoryBalance, Guid>, IRepositoryInventoryBalance
    {
        private readonly PdvContext _pdvContext;
        
        public RepositoryInventoryBalance(PdvContext context) : base(context)
        {
            _pdvContext = context;
        }

        public async Task<InventoryBalance?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.InventoryBalances
                .Include(ib => ib.Produto)
                .FirstOrDefaultAsync(ib => ib.ProdutoId == productId, cancellationToken);
        }

        public async Task<IEnumerable<InventoryBalance>> GetLowStockBalancesAsync(CancellationToken cancellationToken = default)
        {
            return await _pdvContext.InventoryBalances
                .Include(ib => ib.Produto)
                .Where(ib => ib.CurrentStock <= ib.ReorderPoint)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<InventoryBalance>> GetByLocationAsync(string location, CancellationToken cancellationToken = default)
        {
            return await _pdvContext.InventoryBalances
                .Include(ib => ib.Produto)
                .Where(ib => ib.Location == location)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasSufficientStockAsync(Guid productId, decimal quantity, CancellationToken cancellationToken = default)
        {
            var balance = await GetByProductIdAsync(productId, cancellationToken);
            return balance?.HasSufficientStock(quantity) ?? false;
        }
    }
}