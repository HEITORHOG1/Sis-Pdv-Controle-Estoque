using Interfaces;
using Interfaces.Services;
using Model;

namespace Services.Stock
{
    public class StockValidationService : IStockValidationService
    {
        private readonly IRepositoryProduto _productRepository;

        public StockValidationService(IRepositoryProduto productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<StockValidationResult> ValidateStockAvailabilityAsync(Guid productId, int requestedQuantity, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            
            if (product == null)
            {
                return new StockValidationResult
                {
                    IsValid = false,
                    Message = "Produto não encontrado",
                    Errors = new List<StockValidationError>
                    {
                        new StockValidationError
                        {
                            ProductId = productId,
                            ProductName = "Produto não encontrado",
                            RequestedQuantity = requestedQuantity,
                            AvailableQuantity = 0,
                            ErrorMessage = "Produto não encontrado no sistema"
                        }
                    }
                };
            }

            if (!product.HasSufficientStock(requestedQuantity))
            {
                return new StockValidationResult
                {
                    IsValid = false,
                    Message = "Estoque insuficiente",
                    Errors = new List<StockValidationError>
                    {
                        new StockValidationError
                        {
                            ProductId = productId,
                            ProductName = product.NomeProduto,
                            RequestedQuantity = requestedQuantity,
                            AvailableQuantity = product.QuantidadeEstoqueProduto,
                            ErrorMessage = $"Estoque insuficiente. Disponível: {product.QuantidadeEstoqueProduto}, Solicitado: {requestedQuantity}"
                        }
                    }
                };
            }

            return new StockValidationResult
            {
                IsValid = true,
                Message = "Estoque disponível"
            };
        }

        public async Task<StockValidationResult> ValidateStockAvailabilityAsync(IEnumerable<StockValidationRequest> requests, CancellationToken cancellationToken = default)
        {
            var errors = new List<StockValidationError>();
            var productIds = requests.Select(r => r.ProductId).Distinct().ToList();
            
            var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
            var productDict = products.ToDictionary(p => p.Id, p => p);

            foreach (var request in requests)
            {
                if (!productDict.TryGetValue(request.ProductId, out var product))
                {
                    errors.Add(new StockValidationError
                    {
                        ProductId = request.ProductId,
                        ProductName = "Produto não encontrado",
                        RequestedQuantity = request.RequestedQuantity,
                        AvailableQuantity = 0,
                        ErrorMessage = "Produto não encontrado no sistema"
                    });
                    continue;
                }

                if (!product.HasSufficientStock(request.RequestedQuantity))
                {
                    errors.Add(new StockValidationError
                    {
                        ProductId = request.ProductId,
                        ProductName = product.NomeProduto,
                        RequestedQuantity = request.RequestedQuantity,
                        AvailableQuantity = product.QuantidadeEstoqueProduto,
                        ErrorMessage = $"Estoque insuficiente para {product.NomeProduto}. Disponível: {product.QuantidadeEstoqueProduto}, Solicitado: {request.RequestedQuantity}"
                    });
                }
            }

            return new StockValidationResult
            {
                IsValid = !errors.Any(),
                Message = errors.Any() ? "Alguns produtos não possuem estoque suficiente" : "Todos os produtos possuem estoque disponível",
                Errors = errors
            };
        }

        public async Task<IEnumerable<Produto>> GetLowStockProductsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            return products.Where(p => p.IsLowStock() && p.StatusAtivo == 1);
        }

        public async Task<IEnumerable<Produto>> GetOutOfStockProductsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            return products.Where(p => p.IsOutOfStock() && p.StatusAtivo == 1);
        }

        public async Task<IEnumerable<StockAlert>> GetStockAlertsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            var alerts = new List<StockAlert>();

            foreach (var product in products.Where(p => p.StatusAtivo == 1))
            {
                if (product.IsOutOfStock())
                {
                    alerts.Add(new StockAlert
                    {
                        ProductId = product.Id,
                        ProductName = product.NomeProduto,
                        ProductCode = product.CodBarras,
                        CurrentStock = product.QuantidadeEstoqueProduto,
                        ReorderPoint = product.ReorderPoint,
                        MinimumStock = product.MinimumStock,
                        AlertType = StockAlertType.OutOfStock,
                        Message = $"Produto {product.NomeProduto} está sem estoque"
                    });
                }
                else if (product.IsLowStock())
                {
                    alerts.Add(new StockAlert
                    {
                        ProductId = product.Id,
                        ProductName = product.NomeProduto,
                        ProductCode = product.CodBarras,
                        CurrentStock = product.QuantidadeEstoqueProduto,
                        ReorderPoint = product.ReorderPoint,
                        MinimumStock = product.MinimumStock,
                        AlertType = StockAlertType.LowStock,
                        Message = $"Produto {product.NomeProduto} está com estoque baixo (atual: {product.QuantidadeEstoqueProduto}, ponto de reposição: {product.ReorderPoint})"
                    });
                }
            }

            return alerts.OrderBy(a => a.AlertType).ThenBy(a => a.ProductName);
        }
    }
}