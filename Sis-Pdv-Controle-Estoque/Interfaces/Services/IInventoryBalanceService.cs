using Model;

namespace Interfaces.Services
{
    public interface IInventoryBalanceService
    {
        /// <summary>
        /// Calculates current stock balance for a product based on stock movements
        /// </summary>
        Task<InventoryBalance?> CalculateCurrentBalanceAsync(Guid productId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates inventory balance based on a stock movement
        /// </summary>
        Task<InventoryBalance> UpdateBalanceFromMovementAsync(StockMovement movement, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Materializes all inventory balances for performance optimization
        /// </summary>
        Task MaterializeAllBalancesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Materializes inventory balance for a specific product
        /// </summary>
        Task<InventoryBalance> MaterializeBalanceAsync(Guid productId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Validates if a stock movement would result in negative stock
        /// </summary>
        Task<StockMovementValidationResult> ValidateMovementAsync(Guid productId, decimal quantity, StockMovementType type, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Validates stock movement for perishable products with batch and expiry date requirements
        /// </summary>
        Task<StockMovementValidationResult> ValidatePerishableMovementAsync(Guid productId, decimal quantity, StockMovementType type, string? lote, DateTime? dataValidade, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets inventory balance with batch details for perishable products
        /// </summary>
        Task<InventoryBalanceWithBatches?> GetBalanceWithBatchesAsync(Guid productId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets expired batches for cleanup
        /// </summary>
        Task<IEnumerable<ExpiredBatch>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets batches expiring within specified days
        /// </summary>
        Task<IEnumerable<ExpiringBatch>> GetExpiringBatchesAsync(int daysThreshold = 30, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Recalculates balance from scratch based on all movements
        /// </summary>
        Task<InventoryBalance> RecalculateBalanceAsync(Guid productId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Processes stock movement and updates balance atomically
        /// </summary>
        Task<ProcessMovementResult> ProcessMovementAsync(CreateStockMovementRequest request, CancellationToken cancellationToken = default);
    }

    public class StockMovementValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ResultingStock { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; } = new List<string>();
    }

    public class InventoryBalanceWithBatches
    {
        public InventoryBalance Balance { get; set; } = new();
        public IEnumerable<BatchBalance> Batches { get; set; } = new List<BatchBalance>();
    }

    public class BatchBalance
    {
        public string Lote { get; set; } = string.Empty;
        public DateTime? DataValidade { get; set; }
        public decimal Quantity { get; set; }
        public bool IsExpired => DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        public bool IsNearExpiry => DataValidade.HasValue && DataValidade.Value <= DateTime.Now.AddDays(30);
    }

    public class ExpiredBatch
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime DataValidade { get; set; }
        public decimal Quantity { get; set; }
        public int DaysExpired => (DateTime.Now - DataValidade).Days;
    }

    public class ExpiringBatch
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime DataValidade { get; set; }
        public decimal Quantity { get; set; }
        public int DaysUntilExpiry => (DataValidade - DateTime.Now).Days;
    }

    public class CreateStockMovementRequest
    {
        public Guid ProdutoId { get; set; }
        public decimal Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public string? ReferenceDocument { get; set; }
        public Guid? UserId { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public IEnumerable<StockMovementDetailRequest> Details { get; set; } = new List<StockMovementDetailRequest>();
    }

    public class StockMovementDetailRequest
    {
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ProcessMovementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public StockMovement? Movement { get; set; }
        public InventoryBalance? UpdatedBalance { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}