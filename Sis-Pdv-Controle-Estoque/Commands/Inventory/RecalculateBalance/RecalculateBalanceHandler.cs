using Interfaces;
using Interfaces.Services;
using MediatR;

namespace Commands.Inventory.RecalculateBalance
{
    public class RecalculateBalanceHandler : IRequestHandler<RecalculateBalanceRequest, RecalculateBalanceResponse>
    {
        private readonly IInventoryBalanceService _inventoryBalanceService;
        private readonly IRepositoryProduto _productRepository;
        private readonly IRepositoryInventoryBalance _inventoryBalanceRepository;

        public RecalculateBalanceHandler(
            IInventoryBalanceService inventoryBalanceService,
            IRepositoryProduto productRepository,
            IRepositoryInventoryBalance inventoryBalanceRepository)
        {
            _inventoryBalanceService = inventoryBalanceService;
            _productRepository = productRepository;
            _inventoryBalanceRepository = inventoryBalanceRepository;
        }

        public async Task<RecalculateBalanceResponse> Handle(RecalculateBalanceRequest request, CancellationToken cancellationToken)
        {
            var results = new List<ProductBalanceResult>();
            var errors = new List<string>();
            var processedCount = 0;

            try
            {
                if (request.ProductId.HasValue)
                {
                    // Recalculate single product
                    var result = await RecalculateProductBalance(request.ProductId.Value, cancellationToken);
                    if (result != null)
                    {
                        results.Add(result);
                        processedCount = 1;
                    }
                    else
                    {
                        errors.Add($"Produto {request.ProductId} não encontrado");
                    }
                }
                else
                {
                    // Recalculate all products
                    var products = await _productRepository.GetAllAsync(cancellationToken);
                    var activeProducts = products.Where(p => p.StatusAtivo == 1).ToList();

                    foreach (var product in activeProducts)
                    {
                        try
                        {
                            var result = await RecalculateProductBalance(product.Id, cancellationToken);
                            if (result != null)
                            {
                                results.Add(result);
                            }
                            processedCount++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Erro ao recalcular produto {product.NomeProduto} (ID: {product.Id}): {ex.Message}");
                        }
                    }
                }

                var discrepancies = results.Where(r => r.HasDiscrepancy).ToList();
                var message = request.ProductId.HasValue
                    ? $"Recálculo concluído para produto {request.ProductId}"
                    : $"Recálculo concluído para {processedCount} produtos. {discrepancies.Count} discrepâncias encontradas.";

                return new RecalculateBalanceResponse
                {
                    Success = true,
                    Message = message,
                    ProcessedProducts = processedCount,
                    Results = results,
                    Errors = errors
                };
            }
            catch (Exception ex)
            {
                return new RecalculateBalanceResponse
                {
                    Success = false,
                    Message = $"Erro durante recálculo: {ex.Message}",
                    ProcessedProducts = processedCount,
                    Results = results,
                    Errors = errors.Concat(new[] { ex.Message })
                };
            }
        }

        private async Task<ProductBalanceResult?> RecalculateProductBalance(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                return null;
            }

            var existingBalance = await _inventoryBalanceRepository.GetByProductIdAsync(productId, cancellationToken);
            var previousBalance = existingBalance?.CurrentStock ?? 0;

            var recalculatedBalance = await _inventoryBalanceService.RecalculateBalanceAsync(productId, cancellationToken);

            return new ProductBalanceResult
            {
                ProductId = productId,
                ProductName = product.NomeProduto,
                PreviousBalance = previousBalance,
                NewBalance = recalculatedBalance.CurrentStock
            };
        }
    }
}