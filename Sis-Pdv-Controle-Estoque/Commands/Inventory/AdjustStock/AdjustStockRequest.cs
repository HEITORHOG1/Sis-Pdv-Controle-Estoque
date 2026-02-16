using MediatR;
using Model;

namespace Commands.Inventory.AdjustStock
{
    public class AdjustStockRequest : IRequest<AdjustStockResponse>
    {
        public Guid ProductId { get; set; }
        public int NewQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? ReferenceDocument { get; set; }
        public Guid? UserId { get; set; }
    }

    public class AdjustStockResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? StockMovementId { get; set; }
        public int PreviousStock { get; set; }
        public int NewStock { get; set; }
    }
}