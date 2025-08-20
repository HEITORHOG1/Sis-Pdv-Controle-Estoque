using Interfaces.Services;
using MediatR;

namespace Commands.Inventory.GetStockAlerts
{
    public class GetStockAlertsHandler : IRequestHandler<GetStockAlertsRequest, GetStockAlertsResponse>
    {
        private readonly IStockValidationService _stockValidationService;

        public GetStockAlertsHandler(IStockValidationService stockValidationService)
        {
            _stockValidationService = stockValidationService;
        }

        public async Task<GetStockAlertsResponse> Handle(GetStockAlertsRequest request, CancellationToken cancellationToken)
        {
            var alerts = await _stockValidationService.GetStockAlertsAsync(cancellationToken);

            if (request.AlertType.HasValue)
            {
                alerts = alerts.Where(a => a.AlertType == request.AlertType.Value);
            }

            var alertDtos = alerts.Select(a => new StockAlertDto
            {
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                ProductCode = a.ProductCode,
                CurrentStock = (int)a.CurrentStock,
                ReorderPoint = a.ReorderPoint,
                MinimumStock = a.MinimumStock,
                AlertType = a.AlertType,
                AlertTypeDescription = GetAlertTypeDescription(a.AlertType),
                Message = a.Message
            });

            return new GetStockAlertsResponse
            {
                Alerts = alertDtos,
                TotalCount = alertDtos.Count()
            };
        }

        private static string GetAlertTypeDescription(StockAlertType type)
        {
            return type switch
            {
                StockAlertType.LowStock => "Estoque Baixo",
                StockAlertType.OutOfStock => "Sem Estoque",
                StockAlertType.ReorderPoint => "Ponto de Reposição",
                _ => "Desconhecido"
            };
        }
    }
}