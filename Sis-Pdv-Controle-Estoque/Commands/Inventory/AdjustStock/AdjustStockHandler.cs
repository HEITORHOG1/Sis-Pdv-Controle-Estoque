using Interfaces.Repositories;
using MediatR;
using Model;
using Interfaces;

namespace Commands.Inventory.AdjustStock
{
    public class AdjustStockHandler : IRequestHandler<AdjustStockRequest, AdjustStockResponse>
    {
        private readonly IRepositoryProduto _productRepository;
        private readonly IRepositoryStockMovement _stockMovementRepository;
        private readonly IRepositoryInventoryBalance _inventoryBalanceRepository;

        public AdjustStockHandler(
            IRepositoryProduto productRepository,
            IRepositoryStockMovement stockMovementRepository,
            IRepositoryInventoryBalance inventoryBalanceRepository)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
            _inventoryBalanceRepository = inventoryBalanceRepository;
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

            // Get or create inventory balance
            var inventoryBalance = await _inventoryBalanceRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (inventoryBalance == null)
            {
                inventoryBalance = new InventoryBalance(request.ProductId);
                await _inventoryBalanceRepository.AddAsync(inventoryBalance, cancellationToken);
            }

            var previousStock = inventoryBalance.CurrentStock;
            var quantityDifference = request.NewQuantity - previousStock;

            // Create stock movement record
            var stockMovement = new StockMovement(
                request.ProductId,
                quantityDifference,
                StockMovementType.Adjustment,
                request.Reason,
                0, // Unit cost not available in product anymore
                previousStock,
                request.NewQuantity,
                request.ReferenceDocument,
                request.UserId
            );

            // Update inventory balance
            inventoryBalance.UpdateStock(request.NewQuantity);

            try
            {
                await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);
                await _inventoryBalanceRepository.UpdateAsync(inventoryBalance, cancellationToken);

                return new AdjustStockResponse
                {
                    Success = true,
                    Message = "Estoque ajustado com sucesso",
                    StockMovementId = stockMovement.Id,
                    PreviousStock = (int)previousStock,
                    NewStock = request.NewQuantity
                };
            }
            catch (Exception ex)
            {
                return new AdjustStockResponse
                {
                    Success = false,
                    Message = $"Erro ao ajustar estoque: {ex.Message}"
                };
            }
        }
    }
}