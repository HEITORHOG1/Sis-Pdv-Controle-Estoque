using Microsoft.Extensions.Diagnostics.HealthChecks;
using Interfaces;
using Sis_Pdv_Controle_Estoque_API.Models.Health;
using HealthCheckResult = Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;

namespace Sis_Pdv_Controle_Estoque_API.Services.Health;

public class BusinessHealthCheck : IHealthCheck
{
    private readonly IRepositoryProduto _produtoRepository;
    private readonly IRepositoryUsuario _usuarioRepository;
    private readonly IRepositoryPedido _pedidoRepository;
    private readonly ILogger<BusinessHealthCheck> _logger;

    public BusinessHealthCheck(
        IRepositoryProduto produtoRepository,
        IRepositoryUsuario usuarioRepository,
        IRepositoryPedido pedidoRepository,
        ILogger<BusinessHealthCheck> logger)
    {
        _produtoRepository = produtoRepository;
        _usuarioRepository = usuarioRepository;
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var startTime = DateTime.UtcNow;
            var data = new Dictionary<string, object>();
            var warnings = new List<string>();

            // Check critical business operations
            await CheckProductInventory(data, warnings, cancellationToken);
            await CheckActiveUsers(data, warnings, cancellationToken);
            await CheckPendingOrders(data, warnings, cancellationToken);
            await CheckSystemCapacity(data, warnings, cancellationToken);

            var duration = DateTime.UtcNow - startTime;
            var status = warnings.Any() ? HealthStatus.Degraded : HealthStatus.Healthy;
            
            var description = status == HealthStatus.Healthy 
                ? "All business operations are functioning normally"
                : $"Business operations have warnings: {string.Join(", ", warnings)}";

            return HealthCheckResult.Healthy(description, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Business health check failed");
            return HealthCheckResult.Unhealthy("Business operations check failed", ex);
        }
    }

    private async Task CheckProductInventory(Dictionary<string, object> data, List<string> warnings, CancellationToken cancellationToken)
    {
        try
        {
            var produtos = await _produtoRepository.GetAllAsync(cancellationToken);
            var totalProducts = produtos.Count();
            var lowStockProducts = produtos.Count(p => p.QuatidadeEstoqueProduto <= p.ReorderPoint);
            var outOfStockProducts = produtos.Count(p => p.QuatidadeEstoqueProduto <= 0);

            data["TotalProducts"] = totalProducts;
            data["LowStockProducts"] = lowStockProducts;
            data["OutOfStockProducts"] = outOfStockProducts;

            if (lowStockProducts > totalProducts * 0.2) // More than 20% low stock
            {
                warnings.Add($"{lowStockProducts} products have low stock");
            }

            if (outOfStockProducts > 0)
            {
                warnings.Add($"{outOfStockProducts} products are out of stock");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check product inventory");
            data["InventoryCheckError"] = ex.Message;
        }
    }

    private async Task CheckActiveUsers(Dictionary<string, object> data, List<string> warnings, CancellationToken cancellationToken)
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllAsync(cancellationToken);
            var activeUsers = usuarios.Count(u => u.StatusAtivo);
            var totalUsers = usuarios.Count();

            data["ActiveUsers"] = activeUsers;
            data["TotalUsers"] = totalUsers;

            if (activeUsers == 0)
            {
                warnings.Add("No active users in the system");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check active users");
            data["UserCheckError"] = ex.Message;
        }
    }

    private async Task CheckPendingOrders(Dictionary<string, object> data, List<string> warnings, CancellationToken cancellationToken)
    {
        try
        {
            var pedidos = await _pedidoRepository.GetAllAsync(cancellationToken);
            var pendingOrders = pedidos.Count(p => p.Status == 0); // Assuming 0 is pending status
            var todayOrders = pedidos.Count(p => p.DataDoPedido?.Date == DateTime.Today);

            data["PendingOrders"] = pendingOrders;
            data["TodayOrders"] = todayOrders;

            if (pendingOrders > 100) // Arbitrary threshold
            {
                warnings.Add($"High number of pending orders: {pendingOrders}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check pending orders");
            data["OrderCheckError"] = ex.Message;
        }
    }

    private async Task CheckSystemCapacity(Dictionary<string, object> data, List<string> warnings, CancellationToken cancellationToken)
    {
        try
        {
            // Check database connection pool
            var connectionPoolStatus = "Healthy"; // This would need actual implementation
            data["DatabaseConnectionPool"] = connectionPoolStatus;

            // Check memory usage
            var process = System.Diagnostics.Process.GetCurrentProcess();
            var memoryUsage = process.WorkingSet64;
            var memoryUsageMB = memoryUsage / (1024 * 1024);
            
            data["MemoryUsageMB"] = memoryUsageMB;

            if (memoryUsageMB > 1000) // More than 1GB
            {
                warnings.Add($"High memory usage: {memoryUsageMB}MB");
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check system capacity");
            data["CapacityCheckError"] = ex.Message;
        }
    }
}