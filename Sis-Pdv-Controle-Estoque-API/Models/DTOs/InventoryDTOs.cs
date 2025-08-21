using System.ComponentModel.DataAnnotations;
using Sis_Pdv_Controle_Estoque_API.Models.Validation;

namespace Sis_Pdv_Controle_Estoque_API.Models.DTOs
{
    /// <summary>
    /// DTO for creating stock movements
    /// </summary>
    public class CreateStockMovementRequest
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        [NotEmptyGuid]
        public Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório")]
        [Range(1, 4, ErrorMessage = "Tipo de movimentação inválido")]
        public int Type { get; set; } // 1=Entry, 2=Exit, 3=Adjustment, 4=Transfer

        [Required(ErrorMessage = "O motivo da movimentação é obrigatório")]
        [MovementReason]
        public string Reason { get; set; } = string.Empty;

        [PositiveDecimal]
        public decimal? UnitCost { get; set; }

        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres")]
        public string? Lote { get; set; }

        public DateTime? DataValidade { get; set; }

        [StringLength(100, ErrorMessage = "A referência deve ter no máximo 100 caracteres")]
        public string? Reference { get; set; }
    }

    /// <summary>
    /// DTO for stock movement response
    /// </summary>
    public class StockMovementResponse
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public decimal Quantity { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public decimal? UnitCost { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal NewStock { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? Reference { get; set; }
        public DateTime MovementDate { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
    }

    /// <summary>
    /// DTO for stock validation request
    /// </summary>
    public class StockValidationRequest
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        [NotEmptyGuid]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "A quantidade solicitada é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public decimal RequestedQuantity { get; set; }

        /// <summary>
        /// Optional batch number for perishable products
        /// </summary>
        public string? Lote { get; set; }
    }

    /// <summary>
    /// DTO for stock validation result
    /// </summary>
    public class StockValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<StockValidationItem> Items { get; set; } = new List<StockValidationItem>();
    }

    /// <summary>
    /// Individual stock validation item
    /// </summary>
    public class StockValidationItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public decimal ShortageQuantity { get; set; }
        public string? Lote { get; set; }
    }

    /// <summary>
    /// DTO for stock validation response
    /// </summary>
    public class StockValidationResponse
    {
        public bool IsAvailable { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal ShortageQuantity => Math.Max(0, RequestedQuantity - AvailableQuantity);
        public string Message { get; set; } = string.Empty;
        public IEnumerable<BatchInfo>? AvailableBatches { get; set; }
    }

    /// <summary>
    /// DTO for batch information
    /// </summary>
    public class BatchInfo
    {
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsExpired => DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        public bool IsExpiringSoon => DataValidade.HasValue && DataValidade.Value <= DateTime.Now.AddDays(30);
    }

    /// <summary>
    /// DTO for inventory balance response
    /// </summary>
    public class InventoryBalanceResponse
    {
        public Guid ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal ReservedStock { get; set; }
        public decimal AvailableStock => CurrentStock - ReservedStock;
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string? Location { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsLowStock => AvailableStock <= ReorderPoint;
        public bool IsOutOfStock => AvailableStock <= 0;
        public IEnumerable<BatchInfo>? Batches { get; set; }
    }

    /// <summary>
    /// DTO for stock movement filtering
    /// </summary>
    public class StockMovementFilterRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int PageSize { get; set; } = 20;

        public Guid? ProdutoId { get; set; }

        public int? Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? UserId { get; set; }

        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres")]
        public string? Lote { get; set; }

        [StringLength(100, ErrorMessage = "A referência deve ter no máximo 100 caracteres")]
        public string? Reference { get; set; }
    }

    /// <summary>
    /// DTO for paginated stock movement results
    /// </summary>
    public class PagedStockMovementResponse
    {
        public IEnumerable<StockMovementResponse> Items { get; set; } = new List<StockMovementResponse>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    /// <summary>
    /// Stock alert types
    /// </summary>
    public enum StockAlertType
    {
        OutOfStock = 1,
        LowStock = 2,
        ExpiringBatch = 3,
        ExpiredBatch = 4
    }

    /// <summary>
    /// DTO for stock alerts
    /// </summary>
    public class StockAlertResponse
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string CodBarras { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MinimumStock { get; set; }
        public string AlertType { get; set; } = string.Empty; // "OutOfStock", "LowStock", "ExpiringBatch"
        public string Message { get; set; } = string.Empty;
        public DateTime AlertDate { get; set; }
        public IEnumerable<BatchInfo>? ExpiringBatches { get; set; }
    }

    /// <summary>
    /// Batch balance information
    /// </summary>
    public class BatchBalance
    {
        public string Lote { get; set; } = string.Empty;
        public DateTime? DataValidade { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsExpired => DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        public bool IsExpiringSoon => DataValidade.HasValue && DataValidade.Value <= DateTime.Now.AddDays(30);
        public int DaysUntilExpiry => DataValidade.HasValue ? 
            Math.Max(0, (int)(DataValidade.Value - DateTime.Now).TotalDays) : int.MaxValue;
    }

    /// <summary>
    /// Expired batch information
    /// </summary>
    public class ExpiredBatch
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime DataValidade { get; set; }
        public decimal Quantity { get; set; }
        public int DaysExpired => (int)(DateTime.Now - DataValidade).TotalDays;
    }

    /// <summary>
    /// Expiring batch information
    /// </summary>
    public class ExpiringBatch
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime DataValidade { get; set; }
        public decimal Quantity { get; set; }
        public int DaysUntilExpiry => Math.Max(0, (int)(DataValidade - DateTime.Now).TotalDays);
    }
}