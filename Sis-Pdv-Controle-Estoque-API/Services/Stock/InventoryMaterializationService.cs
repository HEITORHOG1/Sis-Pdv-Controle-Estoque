using Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.Stock
{
    public class InventoryMaterializationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InventoryMaterializationService> _logger;
        private readonly TimeSpan _materializationInterval = TimeSpan.FromHours(1); // Run every hour

        public InventoryMaterializationService(
            IServiceProvider serviceProvider,
            ILogger<InventoryMaterializationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Inventory Materialization Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await MaterializeInventoryBalances(stoppingToken);
                    await Task.Delay(_materializationInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Inventory Materialization Service is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during inventory materialization");
                    // Wait a shorter time before retrying on error
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task MaterializeInventoryBalances(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var inventoryBalanceService = scope.ServiceProvider.GetRequiredService<IInventoryBalanceService>();

            _logger.LogInformation("Starting inventory balance materialization");
            var startTime = DateTime.UtcNow;

            try
            {
                await inventoryBalanceService.MaterializeAllBalancesAsync(cancellationToken);
                
                var duration = DateTime.UtcNow - startTime;
                _logger.LogInformation($"Inventory balance materialization completed in {duration.TotalSeconds:F2} seconds");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to materialize inventory balances");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inventory Materialization Service is stopping");
            await base.StopAsync(cancellationToken);
        }
    }
}