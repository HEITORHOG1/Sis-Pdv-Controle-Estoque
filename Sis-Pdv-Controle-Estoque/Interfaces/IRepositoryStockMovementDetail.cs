namespace Interfaces
{
    public interface IRepositoryStockMovementDetail : IRepositoryBase<StockMovementDetail, Guid>
    {
        Task<IEnumerable<StockMovementDetail>> GetByStockMovementIdAsync(Guid stockMovementId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovementDetail>> GetByLoteAsync(string lote, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovementDetail>> GetExpiringDetailsAsync(DateTime beforeDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockMovementDetail>> GetExpiredDetailsAsync(CancellationToken cancellationToken = default);
    }
}