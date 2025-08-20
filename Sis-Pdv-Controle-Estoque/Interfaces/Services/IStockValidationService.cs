using Model;

namespace Interfaces.Services
{
    public interface IStockValidationService
    {
        Task<StockValidationResult> ValidateStockAvailabilityAsync(Guid productId, decimal requestedQuantity, CancellationToken cancellationToken = default);
        Task<StockValidationResult> ValidateStockAvailabilityAsync(IEnumerable<StockValidationRequest> requests, CancellationToken cancellationToken = default);
        Task<IEnumerable<Produto>> GetLowStockProductsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Produto>> GetOutOfStockProductsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<StockAlert>> GetStockAlertsAsync(CancellationToken cancellationToken = default);
    }

    public class StockValidationRequest
    {
        public Guid ProductId { get; set; }
        public decimal RequestedQuantity { get; set; }
    }

    public class StockValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<StockValidationError> Errors { get; set; } = new List<StockValidationError>();
    }

    public class StockValidationError
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class StockAlert
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MinimumStock { get; set; }
        public StockAlertType AlertType { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public enum StockAlertType
    {
        LowStock = 1,
        OutOfStock = 2,
        ReorderPoint = 3
    }
}