using Interfaces.Repositories;
using Interfaces.Services;
using MediatR;
using Model;
using Interfaces;

namespace Commands.Inventory.AdjustStock
{
    public class AdjustStockHandler : IRequestHandler<AdjustStockRequest, AdjustStockResponse>
    {
        private readonly IRepositoryProduto _productRepository;
        private readonly IInventoryBalanceService _inventoryBalanceService;

        public AdjustStockHandler(
            IRepositoryProduto productRepository,
            IInventoryBalanceService inventoryBalanceService)
        {
            _productRepository = productRepository;
            _inventoryBalanceService = inventoryBalanceService;
        }

        public async Task<AdjustStockResponse> Handle(AdjustStockRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            
            if (product == null)
            {
                return new AdjustStockResponse
                {
                    Success = false,
                    Message = "Produto não encontrado"
                };
            }

            // Create stock movement request
            var movementRequest = new CreateStockMovementRequest
            {
                ProdutoId = request.ProductId,
                Quantity = request.NewQuantity, // For adjustments, quantity represents the new total
                Type = StockMovementType.Adjustment,
                Reason = request.Reason,
                UnitCost = 0, // Unit cost not available for adjustments
                ReferenceDocument = request.ReferenceDocument,
                UserId = request.UserId
            };

            // Process the movement using the inventory balance service
            var result = await _inventoryBalanceService.ProcessMovementAsync(movementRequest, cancellationToken);

            if (result.Success)
            {
                return new AdjustStockResponse
                {
                    Success = true,
                    Message = result.Message,
                    StockMovementId = result.Movement?.Id ?? Guid.Empty,
                    PreviousStock = (int)(result.Movement?.PreviousStock ?? 0),
                    NewStock = (int)(result.UpdatedBalance?.CurrentStock ?? 0)
                };
            }
            else
            {
                return new AdjustStockResponse
                {
                    Success = false,
                    Message = result.Message
                };
            }
        }
    }
}