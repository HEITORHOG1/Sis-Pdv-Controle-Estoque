using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepositoryInventoryBalance : IRepositoryBase<InventoryBalance, Guid>
    {
        Task<InventoryBalance?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<InventoryBalance>> GetLowStockBalancesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<InventoryBalance>> GetByLocationAsync(string location, CancellationToken cancellationToken = default);
        Task<bool> HasSufficientStockAsync(Guid productId, decimal quantity, CancellationToken cancellationToken = default);
    }
}