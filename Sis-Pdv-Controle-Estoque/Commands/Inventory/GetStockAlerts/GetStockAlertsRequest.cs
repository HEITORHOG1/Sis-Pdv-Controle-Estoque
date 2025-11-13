using Interfaces.Services;
using MediatR;

namespace Commands.Inventory.GetStockAlerts
{
    public class GetStockAlertsRequest : IRequest<GetStockAlertsResponse>
    {
        public StockAlertType? AlertType { get; set; }
    }

    public class GetStockAlertsResponse
    {
        public IEnumerable<StockAlertDto> Alerts { get; set; } = new List<StockAlertDto>();
        public int TotalCount { get; set; }
    }

    public class StockAlertDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MinimumStock { get; set; }
        public StockAlertType AlertType { get; set; }
        public string AlertTypeDescription { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}