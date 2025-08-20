using Interfaces;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Sis_Pdv_Controle_Estoque_API.Services;

namespace Services.Stock
{
    public class InventoryBalanceService : IInventoryBalanceService
    {
        private readonly IRepositoryInventoryBalance _inventoryRepository;
        private readonly IRepositoryStockMovement _stockMovementRepository;
        private readonly IRepositoryStockMovementDetail _stockMovementDetailRepository;
        private readonly IRepositoryProduto _productRepository;
        private readonly ILogger<InventoryBalanceService> _logger;

        public InventoryBalanceService(
            IRepositoryInventoryBalance inventoryRepository,
            IRepositoryStockMovement stockMovementRepository,
            IRepositoryStockMovementDetail stockMovementDetailRepository,
            IRepositoryProduto productRepository,
            ILogger<InventoryBalanceService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _stockMovementRepository = stockMovementRepository;
            _stockMovementDetailRepository = stockMovementDetailRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<InventoryBalance?> CalculateCurrentBalanceAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Calculating current balance for product {productId}");

                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
                if (product == null)
                {
                    _logger.LogWarning($"Product {productId} not found");
                    return null;
                }

                var movements = await _stockMovementRepository.GetByProductIdAsync(productId, cancellationToken);
                
                decimal currentStock = 0;
                foreach (var movement in movements.OrderBy(m => m.MovementDate))
                {
                    switch (movement.Type)
                    {
                        case StockMovementType.Entry:
                        case StockMovementType.Return:
                            currentStock += movement.Quantity;
                            break;
                        case StockMovementType.Exit:
                        case StockMovementType.Sale:
                        case StockMovementType.Loss:
                            currentStock -= movement.Quantity;
                            break;
                        case StockMovementType.Adjustment:
                            currentStock = movement.NewStock;
                            break;
                        case StockMovementType.Transfer:
                            // For transfers, we need to check if it's incoming or outgoing
                            // This would need additional logic based on location/warehouse
                            currentStock += movement.Quantity; // Simplified for now
                            break;
                    }
                }

                var existingBalance = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
                if (existingBalance != null)
                {
                    existingBalance.UpdateStock(currentStock);
                    return existingBalance;
                }

                return new InventoryBalance(productId, currentStock);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating balance for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<InventoryBalance> UpdateBalanceFromMovementAsync(StockMovement movement, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Updating balance from movement {movement.Id} for product {movement.ProdutoId}");

                var existingBalance = await _inventoryRepository.GetByProductIdAsync(movement.ProdutoId, cancellationToken);
                
                if (existingBalance == null)
                {
                    // Create new balance if it doesn't exist
                    existingBalance = new InventoryBalance(movement.ProdutoId);
                    await _inventoryRepository.AddAsync(existingBalance, cancellationToken);
                }

                decimal newStock = existingBalance.CurrentStock;
                
                switch (movement.Type)
                {
                    case StockMovementType.Entry:
                    case StockMovementType.Return:
                        newStock += movement.Quantity;
                        break;
                    case StockMovementType.Exit:
                    case StockMovementType.Sale:
                    case StockMovementType.Loss:
                        newStock -= movement.Quantity;
                        break;
                    case StockMovementType.Adjustment:
                        newStock = movement.NewStock;
                        break;
                    case StockMovementType.Transfer:
                        newStock += movement.Quantity; // Simplified
                        break;
                }

                existingBalance.UpdateStock(newStock);
                await _inventoryRepository.UpdateAsync(existingBalance, cancellationToken);

                _logger.LogInformation($"Balance updated for product {movement.ProdutoId}: {existingBalance.CurrentStock}");
                return existingBalance;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating balance from movement {movement.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task MaterializeAllBalancesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting materialization of all inventory balances");

                var products = await _productRepository.GetAllAsync(cancellationToken);
                var processedCount = 0;

                foreach (var product in products.Where(p => p.StatusAtivo == 1))
                {
                    await MaterializeBalanceAsync(product.Id, cancellationToken);
                    processedCount++;

                    if (processedCount % 100 == 0)
                    {
                        _logger.LogInformation($"Materialized {processedCount} product balances");
                    }
                }

                _logger.LogInformation($"Completed materialization of {processedCount} inventory balances");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error materializing all balances: {ex.Message}");
                throw;
            }
        }

        public async Task<InventoryBalance> MaterializeBalanceAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var calculatedBalance = await CalculateCurrentBalanceAsync(productId, cancellationToken);
                if (calculatedBalance == null)
                {
                    throw new InvalidOperationException($"Cannot calculate balance for product {productId}");
                }

                var existingBalance = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
                
                if (existingBalance == null)
                {
                    await _inventoryRepository.AddAsync(calculatedBalance, cancellationToken);
                    return calculatedBalance;
                }
                else
                {
                    existingBalance.UpdateStock(calculatedBalance.CurrentStock, calculatedBalance.ReservedStock);
                    await _inventoryRepository.UpdateAsync(existingBalance, cancellationToken);
                    return existingBalance;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error materializing balance for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<StockMovementValidationResult> ValidateMovementAsync(Guid productId, decimal quantity, StockMovementType type, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
                if (product == null)
                {
                    return new StockMovementValidationResult
                    {
                        IsValid = false,
                        Message = "Produto não encontrado",
                        ValidationErrors = new[] { "Produto não encontrado no sistema" }
                    };
                }

                var balance = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
                var currentStock = balance?.CurrentStock ?? 0;

                var validationErrors = new List<string>();

                // Validate quantity
                if (quantity <= 0)
                {
                    validationErrors.Add("Quantidade deve ser maior que zero");
                }

                // Validate stock for outgoing movements
                decimal resultingStock = currentStock;
                if (type == StockMovementType.Exit || type == StockMovementType.Sale || type == StockMovementType.Loss)
                {
                    resultingStock = currentStock - quantity;
                    if (resultingStock < 0)
                    {
                        validationErrors.Add($"Estoque insuficiente. Disponível: {currentStock}, Solicitado: {quantity}");
                    }
                }
                else if (type == StockMovementType.Entry || type == StockMovementType.Return)
                {
                    resultingStock = currentStock + quantity;
                }

                return new StockMovementValidationResult
                {
                    IsValid = !validationErrors.Any(),
                    Message = validationErrors.Any() ? string.Join("; ", validationErrors) : "Movimentação válida",
                    CurrentStock = currentStock,
                    ResultingStock = resultingStock,
                    ValidationErrors = validationErrors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating movement for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<StockMovementValidationResult> ValidatePerishableMovementAsync(Guid productId, decimal quantity, StockMovementType type, string? lote, DateTime? dataValidade, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
                if (product == null)
                {
                    return new StockMovementValidationResult
                    {
                        IsValid = false,
                        Message = "Produto não encontrado",
                        ValidationErrors = new[] { "Produto não encontrado no sistema" }
                    };
                }

                var validationErrors = new List<string>();

                // First validate basic movement
                var basicValidation = await ValidateMovementAsync(productId, quantity, type, cancellationToken);
                if (!basicValidation.IsValid)
                {
                    validationErrors.AddRange(basicValidation.ValidationErrors);
                }

                // Validate perishable product requirements
                if (product.IsPerecivel)
                {
                    if (type == StockMovementType.Entry || type == StockMovementType.Return)
                    {
                        if (string.IsNullOrWhiteSpace(lote))
                        {
                            validationErrors.Add("Lote é obrigatório para produtos perecíveis");
                        }

                        if (!dataValidade.HasValue)
                        {
                            validationErrors.Add("Data de validade é obrigatória para produtos perecíveis");
                        }
                        else if (dataValidade.Value <= DateTime.Now)
                        {
                            validationErrors.Add("Data de validade deve ser futura");
                        }
                    }
                }

                return new StockMovementValidationResult
                {
                    IsValid = !validationErrors.Any(),
                    Message = validationErrors.Any() ? string.Join("; ", validationErrors) : "Movimentação válida",
                    CurrentStock = basicValidation.CurrentStock,
                    ResultingStock = basicValidation.ResultingStock,
                    ValidationErrors = validationErrors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating perishable movement for product {productId}: {ex.Message}");
                throw;
            }
        }
    
    public async Task<InventoryBalanceWithBatches?> GetBalanceWithBatchesAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var balance = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
                if (balance == null)
                {
                    return null;
                }

                var movements = await _stockMovementRepository.GetByProductIdAsync(productId, cancellationToken);
                var batchBalances = new Dictionary<string, BatchBalance>();

                foreach (var movement in movements.OrderBy(m => m.MovementDate))
                {
                    if (!string.IsNullOrEmpty(movement.Lote))
                    {
                        var key = $"{movement.Lote}_{movement.DataValidade?.ToString("yyyy-MM-dd") ?? "no-expiry"}";
                        
                        if (!batchBalances.ContainsKey(key))
                        {
                            batchBalances[key] = new BatchBalance
                            {
                                Lote = movement.Lote,
                                DataValidade = movement.DataValidade,
                                Quantity = 0
                            };
                        }

                        switch (movement.Type)
                        {
                            case StockMovementType.Entry:
                            case StockMovementType.Return:
                                batchBalances[key].Quantity += movement.Quantity;
                                break;
                            case StockMovementType.Exit:
                            case StockMovementType.Sale:
                            case StockMovementType.Loss:
                                batchBalances[key].Quantity -= movement.Quantity;
                                break;
                        }
                    }
                }

                // Filter out batches with zero or negative quantities
                var activeBatches = batchBalances.Values.Where(b => b.Quantity > 0).ToList();

                return new InventoryBalanceWithBatches
                {
                    Balance = balance,
                    Batches = activeBatches
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting balance with batches for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ExpiredBatch>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var expiredDetails = await _stockMovementDetailRepository.GetExpiredDetailsAsync(cancellationToken);
                var expiredBatches = new List<ExpiredBatch>();

                var groupedByProduct = expiredDetails.GroupBy(d => d.StockMovement.ProdutoId);

                foreach (var productGroup in groupedByProduct)
                {
                    var product = await _productRepository.GetByIdAsync(productGroup.Key, cancellationToken);
                    if (product == null) continue;

                    var batchGroups = productGroup.GroupBy(d => new { d.Lote, d.DataValidade });
                    
                    foreach (var batchGroup in batchGroups)
                    {
                        if (!batchGroup.Key.DataValidade.HasValue || string.IsNullOrEmpty(batchGroup.Key.Lote))
                            continue;

                        var totalQuantity = batchGroup.Sum(d => 
                            d.StockMovement.Type == StockMovementType.Entry || d.StockMovement.Type == StockMovementType.Return 
                                ? d.Quantity 
                                : -d.Quantity);

                        if (totalQuantity > 0)
                        {
                            expiredBatches.Add(new ExpiredBatch
                            {
                                ProductId = product.Id,
                                ProductName = product.NomeProduto,
                                Lote = batchGroup.Key.Lote,
                                DataValidade = batchGroup.Key.DataValidade.Value,
                                Quantity = totalQuantity
                            });
                        }
                    }
                }

                return expiredBatches.OrderBy(b => b.DataValidade);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting expired batches: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ExpiringBatch>> GetExpiringBatchesAsync(int daysThreshold = 30, CancellationToken cancellationToken = default)
        {
            try
            {
                var thresholdDate = DateTime.Now.AddDays(daysThreshold);
                var expiringDetails = await _stockMovementDetailRepository.GetExpiringDetailsAsync(thresholdDate, cancellationToken);
                var expiringBatches = new List<ExpiringBatch>();

                var groupedByProduct = expiringDetails.GroupBy(d => d.StockMovement.ProdutoId);

                foreach (var productGroup in groupedByProduct)
                {
                    var product = await _productRepository.GetByIdAsync(productGroup.Key, cancellationToken);
                    if (product == null) continue;

                    var batchGroups = productGroup.GroupBy(d => new { d.Lote, d.DataValidade });
                    
                    foreach (var batchGroup in batchGroups)
                    {
                        if (!batchGroup.Key.DataValidade.HasValue || string.IsNullOrEmpty(batchGroup.Key.Lote))
                            continue;

                        // Skip already expired batches
                        if (batchGroup.Key.DataValidade.Value < DateTime.Now)
                            continue;

                        var totalQuantity = batchGroup.Sum(d => 
                            d.StockMovement.Type == StockMovementType.Entry || d.StockMovement.Type == StockMovementType.Return 
                                ? d.Quantity 
                                : -d.Quantity);

                        if (totalQuantity > 0)
                        {
                            expiringBatches.Add(new ExpiringBatch
                            {
                                ProductId = product.Id,
                                ProductName = product.NomeProduto,
                                Lote = batchGroup.Key.Lote,
                                DataValidade = batchGroup.Key.DataValidade.Value,
                                Quantity = totalQuantity
                            });
                        }
                    }
                }

                return expiringBatches.OrderBy(b => b.DataValidade);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting expiring batches: {ex.Message}");
                throw;
            }
        }

        public async Task<InventoryBalance> RecalculateBalanceAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Recalculating balance from scratch for product {productId}");

                var calculatedBalance = await CalculateCurrentBalanceAsync(productId, cancellationToken);
                if (calculatedBalance == null)
                {
                    throw new InvalidOperationException($"Cannot calculate balance for product {productId}");
                }

                var existingBalance = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
                
                if (existingBalance == null)
                {
                    await _inventoryRepository.AddAsync(calculatedBalance, cancellationToken);
                    _logger.LogInformation($"Created new balance for product {productId}: {calculatedBalance.CurrentStock}");
                    return calculatedBalance;
                }
                else
                {
                    var oldStock = existingBalance.CurrentStock;
                    existingBalance.UpdateStock(calculatedBalance.CurrentStock, calculatedBalance.ReservedStock);
                    await _inventoryRepository.UpdateAsync(existingBalance, cancellationToken);
                    
                    _logger.LogInformation($"Recalculated balance for product {productId}: {oldStock} -> {existingBalance.CurrentStock}");
                    return existingBalance;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error recalculating balance for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<ProcessMovementResult> ProcessMovementAsync(CreateStockMovementRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Processing stock movement for product {request.ProdutoId}");

                var product = await _productRepository.GetByIdAsync(request.ProdutoId, cancellationToken);
                if (product == null)
                {
                    return new ProcessMovementResult
                    {
                        Success = false,
                        Message = "Produto não encontrado",
                        Errors = new[] { "Produto não encontrado no sistema" }
                    };
                }

                // Validate movement
                var validation = product.IsPerecivel 
                    ? await ValidatePerishableMovementAsync(request.ProdutoId, request.Quantity, request.Type, request.Lote, request.DataValidade, cancellationToken)
                    : await ValidateMovementAsync(request.ProdutoId, request.Quantity, request.Type, cancellationToken);

                if (!validation.IsValid)
                {
                    return new ProcessMovementResult
                    {
                        Success = false,
                        Message = validation.Message,
                        Errors = validation.ValidationErrors
                    };
                }

                // Get current balance
                var currentBalance = await _inventoryRepository.GetByProductIdAsync(request.ProdutoId, cancellationToken);
                var previousStock = currentBalance?.CurrentStock ?? 0;

                // Calculate new stock
                decimal newStock = previousStock;
                switch (request.Type)
                {
                    case StockMovementType.Entry:
                    case StockMovementType.Return:
                        newStock += request.Quantity;
                        break;
                    case StockMovementType.Exit:
                    case StockMovementType.Sale:
                    case StockMovementType.Loss:
                        newStock -= request.Quantity;
                        break;
                    case StockMovementType.Adjustment:
                        newStock = request.Quantity; // For adjustments, quantity represents the new total
                        break;
                    case StockMovementType.Transfer:
                        newStock += request.Quantity; // Simplified
                        break;
                }

                // Create stock movement
                var movement = new StockMovement(
                    request.ProdutoId,
                    request.Quantity,
                    request.Type,
                    request.Reason,
                    request.UnitCost,
                    previousStock,
                    newStock,
                    request.ReferenceDocument,
                    request.UserId,
                    request.Lote,
                    request.DataValidade);

                await _stockMovementRepository.AddAsync(movement, cancellationToken);

                // Add movement details if provided
                foreach (var detail in request.Details)
                {
                    var movementDetail = new StockMovementDetail(movement.Id, detail.Quantity, detail.Lote, detail.DataValidade);
                    await _stockMovementDetailRepository.AddAsync(movementDetail, cancellationToken);
                }

                // Update balance
                var updatedBalance = await UpdateBalanceFromMovementAsync(movement, cancellationToken);

                _logger.LogInformation($"Successfully processed movement {movement.Id} for product {request.ProdutoId}");

                return new ProcessMovementResult
                {
                    Success = true,
                    Message = "Movimentação processada com sucesso",
                    Movement = movement,
                    UpdatedBalance = updatedBalance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing movement for product {request.ProdutoId}: {ex.Message}");
                return new ProcessMovementResult
                {
                    Success = false,
                    Message = "Erro interno do sistema",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}