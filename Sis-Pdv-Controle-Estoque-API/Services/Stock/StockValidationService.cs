using Interfaces;
using Interfaces.Services;
using Model;

namespace Services.Stock
{
    public class StockValidationService : IStockValidationService
    {
        private readonly IRepositoryProduto _productRepository;
        private readonly IRepositoryInventoryBalance _inventoryRepository;

        public StockValidationService(IRepositoryProduto productRepository, IRepositoryInventoryBalance inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<StockValidationResult> ValidateStockAvailabilityAsync(Guid productId, decimal requestedQuantity, CancellationToken cancellationToken = default)
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

            var inventory = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
            if (inventory == null || !inventory.HasSufficientStock(requestedQuantity))
            {
                var availableStock = inventory?.AvailableStock ?? 0;
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
                            AvailableQuantity = availableStock,
                            ErrorMessage = $"Estoque insuficiente. Disponível: {availableStock}, Solicitado: {requestedQuantity}"
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

                var inventory = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
                if (inventory == null || !inventory.HasSufficientStock(request.RequestedQuantity))
                {
                    var availableStock = inventory?.AvailableStock ?? 0;
                    errors.Add(new StockValidationError
                    {
                        ProductId = request.ProductId,
                        ProductName = product.NomeProduto,
                        RequestedQuantity = request.RequestedQuantity,
                        AvailableQuantity = availableStock,
                        ErrorMessage = $"Estoque insuficiente para {product.NomeProduto}. Disponível: {availableStock}, Solicitado: {request.RequestedQuantity}"
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
            var lowStockBalances = await _inventoryRepository.GetLowStockBalancesAsync(cancellationToken);
            return lowStockBalances.Where(ib => ib.Produto.StatusAtivo == 1).Select(ib => ib.Produto);
        }

        public async Task<IEnumerable<Produto>> GetOutOfStockProductsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            var outOfStockProducts = new List<Produto>();
            
            foreach (var product in products.Where(p => p.StatusAtivo == 1))
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(product.Id, cancellationToken);
                if (inventory != null && inventory.IsOutOfStock())
                {
                    outOfStockProducts.Add(product);
                }
            }
            
            return outOfStockProducts;
        }

        public async Task<IEnumerable<StockAlert>> GetStockAlertsAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            var alerts = new List<StockAlert>();

            foreach (var product in products.Where(p => p.StatusAtivo == 1))
            {
                var inventory = await _inventoryRepository.GetByProductIdAsync(product.Id, cancellationToken);
                if (inventory == null) continue;

                if (inventory.IsOutOfStock())
                {
                    alerts.Add(new StockAlert
                    {
                        ProductId = product.Id,
                        ProductName = product.NomeProduto,
                        ProductCode = product.CodBarras,
                        CurrentStock = inventory.CurrentStock,
                        ReorderPoint = inventory.ReorderPoint,
                        MinimumStock = inventory.MinimumStock,
                        AlertType = StockAlertType.OutOfStock,
                        Message = $"Produto {product.NomeProduto} está sem estoque"
                    });
                }
                else if (inventory.IsLowStock())
                {
                    alerts.Add(new StockAlert
                    {
                        ProductId = product.Id,
                        ProductName = product.NomeProduto,
                        ProductCode = product.CodBarras,
                        CurrentStock = inventory.CurrentStock,
                        ReorderPoint = inventory.ReorderPoint,
                        MinimumStock = inventory.MinimumStock,
                        AlertType = StockAlertType.LowStock,
                        Message = $"Produto {product.NomeProduto} está com estoque baixo (atual: {inventory.CurrentStock}, ponto de reposição: {inventory.ReorderPoint})"
                    });
                }
            }

            return alerts.OrderBy(a => a.AlertType).ThenBy(a => a.ProductName);
        }
    }
}