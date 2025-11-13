using MediatR;
using Model;

namespace Commands.Inventory.GetStockMovements
{
    public class GetStockMovementsRequest : IRequest<GetStockMovementsResponse>
    {
        public Guid? ProductId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public StockMovementType? Type { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class GetStockMovementsResponse
    {
        public IEnumerable<StockMovementDto> Movements { get; set; } = new List<StockMovementDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class StockMovementDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string TypeDescription { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal NewStock { get; set; }
        public DateTime MovementDate { get; set; }
        public string? ReferenceDocument { get; set; }
        public Guid? UserId { get; set; }
    }
}