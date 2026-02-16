using Model;
using Interfaces.Repositories.Base;

namespace Interfaces.Repositories
{
    public interface IRepositoryStockMovement : IRepositoryBase<StockMovement, Guid>
    {
        Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovement>> GetByProductIdAndDateRangeAsync(Guid productId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovement>> GetByTypeAsync(StockMovementType type, CancellationToken cancellationToken = default);
        Task<StockMovement?> GetLastMovementByProductAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovement>> GetRecentMovementsAsync(int count = 50, CancellationToken cancellationToken = default);
    }
}