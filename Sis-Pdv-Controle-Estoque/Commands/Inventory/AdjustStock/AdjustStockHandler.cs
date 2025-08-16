using Interfaces.Repositories;
using MediatR;
using Model;

namespace Commands.Inventory.AdjustStock
{
    public class AdjustStockHandler : IRequestHandler<AdjustStockRequest, AdjustStockResponse>
    {
        private readonly IRepositoryProduto _productRepository;
        private readonly IRepositoryStockMovement _stockMovementRepository;

        public AdjustStockHandler(
            IRepositoryProduto productRepository,
            IRepositoryStockMovement stockMovementRepository)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
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

            var previousStock = product.QuatidadeEstoqueProduto;
            var quantityDifference = request.NewQuantity - previousStock;

            // Create stock movement record
            var stockMovement = new StockMovement(
                request.ProductId,
                quantityDifference,
                StockMovementType.Adjustment,
                request.Reason,
                product.PrecoCusto,
                previousStock,
                request.NewQuantity,
                request.ReferenceDocument,
                request.UserId
            );

            // Update product stock
            product.QuatidadeEstoqueProduto = request.NewQuantity;

            try
            {
                await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);
                await _productRepository.UpdateAsync(product, cancellationToken);

                return new AdjustStockResponse
                {
                    Success = true,
                    Message = "Estoque ajustado com sucesso",
                    StockMovementId = stockMovement.Id,
                    PreviousStock = previousStock,
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