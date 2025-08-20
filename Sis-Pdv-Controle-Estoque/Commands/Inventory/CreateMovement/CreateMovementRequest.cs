using MediatR;
using Model;

namespace Commands.Inventory.CreateMovement
{
    public class CreateMovementRequest : IRequest<CreateMovementResponse>
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
        public IEnumerable<MovementDetailRequest> Details { get; set; } = new List<MovementDetailRequest>();
    }

    public class MovementDetailRequest
    {
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CreateMovementResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? MovementId { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal NewStock { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; } = new List<string>();
    }
}