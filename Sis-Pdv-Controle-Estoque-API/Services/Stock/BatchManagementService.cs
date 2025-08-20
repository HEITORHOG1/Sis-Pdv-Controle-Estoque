using Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sis_Pdv_Controle_Estoque_API.Services;

namespace Services.Stock
{
    public class BatchManagementService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BatchManagementService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6); // Check every 6 hours

        public BatchManagementService(
            IServiceProvider serviceProvider,
            ILogger<BatchManagementService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Batch Management Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpiryChecks(stoppingToken);
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Batch Management Service is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during batch management processing");
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
        }

        private async Task ProcessExpiryChecks(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var inventoryBalanceService = scope.ServiceProvider.GetRequiredService<IInventoryBalanceService>();
            var applicationLogger = scope.ServiceProvider.GetRequiredService<IApplicationLogger>();

            try
            {
                _logger.LogInformation("Starting batch expiry checks");

                // Check for expired batches
                var expiredBatches = await inventoryBalanceService.GetExpiredBatchesAsync(cancellationToken);
                if (expiredBatches.Any())
                {
                    _logger.LogWarning($"Found {expiredBatches.Count()} expired batches");
                    
                    foreach (var expiredBatch in expiredBatches)
                    {
                        applicationLogger.LogBusinessEvent("ExpiredBatch", new { 
                            ProductName = expiredBatch.ProductName, 
                            ProductId = expiredBatch.ProductId,
                            Batch = expiredBatch.Lote, 
                            ExpiredDate = expiredBatch.DataValidade, 
                            Quantity = expiredBatch.Quantity, 
                            DaysExpired = expiredBatch.DaysExpired 
                        });
                    }
                }

                // Check for expiring batches (next 30 days)
                var expiringBatches = await inventoryBalanceService.GetExpiringBatchesAsync(30, cancellationToken);
                if (expiringBatches.Any())
                {
                    _logger.LogInformation($"Found {expiringBatches.Count()} batches expiring in the next 30 days");
                    
                    foreach (var expiringBatch in expiringBatches.Where(b => b.DaysUntilExpiry <= 7))
                    {
                        applicationLogger.LogBusinessEvent("ExpiringBatch", new { 
                            ProductName = expiringBatch.ProductName, 
                            ProductId = expiringBatch.ProductId,
                            Batch = expiringBatch.Lote, 
                            ExpiryDate = expiringBatch.DataValidade, 
                            Quantity = expiringBatch.Quantity, 
                            DaysUntilExpiry = expiringBatch.DaysUntilExpiry 
                        });
                    }
                }

                _logger.LogInformation("Batch expiry checks completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process batch expiry checks");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Batch Management Service is stopping");
            await base.StopAsync(cancellationToken);
        }
    }
}