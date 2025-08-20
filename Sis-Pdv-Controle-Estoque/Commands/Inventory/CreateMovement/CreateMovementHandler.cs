using Interfaces.Services;
using MediatR;

namespace Commands.Inventory.CreateMovement
{
    public class CreateMovementHandler : IRequestHandler<CreateMovementRequest, CreateMovementResponse>
    {
        private readonly IInventoryBalanceService _inventoryBalanceService;

        public CreateMovementHandler(IInventoryBalanceService inventoryBalanceService)
        {
            _inventoryBalanceService = inventoryBalanceService;
        }

        public async Task<CreateMovementResponse> Handle(CreateMovementRequest request, CancellationToken cancellationToken)
        {
            // Map to service request
            var serviceRequest = new CreateStockMovementRequest
            {
                ProdutoId = request.ProdutoId,
                Quantity = request.Quantity,
                Type = request.Type,
                Reason = request.Reason,
                UnitCost = request.UnitCost,
                ReferenceDocument = request.ReferenceDocument,
                UserId = request.UserId,
                Lote = request.Lote,
                DataValidade = request.DataValidade,
                Details = request.Details.Select(d => new StockMovementDetailRequest
                {
                    Lote = d.Lote,
                    DataValidade = d.DataValidade,
                    Quantity = d.Quantity
                })
            };

            // Process the movement
            var result = await _inventoryBalanceService.ProcessMovementAsync(serviceRequest, cancellationToken);

            return new CreateMovementResponse
            {
                Success = result.Success,
                Message = result.Message,
                MovementId = result.Movement?.Id,
                PreviousStock = result.Movement?.PreviousStock ?? 0,
                NewStock = result.UpdatedBalance?.CurrentStock ?? 0,
                ValidationErrors = result.Errors
            };
        }
    }
}