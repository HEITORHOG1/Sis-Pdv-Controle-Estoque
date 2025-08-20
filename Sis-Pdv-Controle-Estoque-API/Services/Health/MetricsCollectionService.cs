using Interfaces;
using Sis_Pdv_Controle_Estoque_API.Models.Health;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_API.Services.Health;

public interface IMetricsCollectionService
{
    Task<SystemMetrics> GetSystemMetricsAsync(CancellationToken cancellationToken = default);
    Task<BusinessMetrics> GetBusinessMetricsAsync(CancellationToken cancellationToken = default);
    Task<ApplicationMetrics> GetApplicationMetricsAsync(CancellationToken cancellationToken = default);
    void IncrementRequestCount();
    void IncrementFailedRequestCount();
    void RecordResponseTime(double responseTimeMs);
    void RecordActiveSession();
    void RemoveActiveSession();
}

public class MetricsCollectionService : IMetricsCollectionService
{
    private readonly IRepositoryProduto _produtoRepository;
    private readonly IRepositoryUsuario _usuarioRepository;
    private readonly IRepositoryPedido _pedidoRepository;
    private readonly ILogger<MetricsCollectionService> _logger;
    
    private static long _totalRequests = 0;
    private static long _failedRequests = 0;
    private static readonly List<double> _responseTimes = new();
    private static int _activeSessions = 0;
    private static readonly DateTime _startTime = DateTime.UtcNow;
    private static readonly object _lock = new();

    public MetricsCollectionService(
        IRepositoryProduto produtoRepository,
        IRepositoryUsuario usuarioRepository,
        IRepositoryPedido pedidoRepository,
        ILogger<MetricsCollectionService> logger)
    {
        _produtoRepository = produtoRepository;
        _usuarioRepository = usuarioRepository;
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<SystemMetrics> GetSystemMetricsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            
            // Get memory metrics
            var memoryUsed = process.WorkingSet64;
            var totalMemory = GC.GetTotalMemory(false);

            // Get disk metrics for the current drive
            var currentDrive = new DriveInfo(Directory.GetCurrentDirectory());
            var diskTotal = currentDrive.TotalSize;
            var diskFree = currentDrive.TotalFreeSpace;
            var diskUsed = diskTotal - diskFree;

            // Calculate CPU usage (simplified)
            var cpuUsage = await GetCpuUsageAsync();

            return new SystemMetrics
            {
                CpuUsagePercent = cpuUsage,
                MemoryUsedBytes = memoryUsed,
                MemoryTotalBytes = totalMemory,
                MemoryUsagePercent = (double)memoryUsed / totalMemory * 100,
                DiskUsedBytes = diskUsed,
                DiskTotalBytes = diskTotal,
                DiskUsagePercent = (double)diskUsed / diskTotal * 100,
                ActiveConnections = _activeSessions,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect system metrics");
            throw;
        }
    }

    public async Task<BusinessMetrics> GetBusinessMetricsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var produtos = await _produtoRepository.GetAllAsync(cancellationToken);
            var usuarios = await _usuarioRepository.GetAllAsync(cancellationToken);
            var pedidos = await _pedidoRepository.GetAllAsync(cancellationToken);

            var totalProducts = produtos.Count();
            var lowStockProducts = 0; // TODO: Implementar usando InventoryBalance
            var activeUsers = usuarios.Count(u => u.StatusAtivo);
            
            var todayPedidos = pedidos.Where(p => p.DataDoPedido?.Date == DateTime.Today).ToList();
            var todaySales = todayPedidos.Count();
            var todayRevenue = todayPedidos.Sum(p => p.TotalPedido);
            var pendingOrders = pedidos.Count(p => p.Status == 0); // Assuming 0 is pending status

            // Get last backup time (this would need to be implemented based on your backup system)
            var lastBackup = DateTime.UtcNow.AddHours(-1); // Placeholder

            return new BusinessMetrics
            {
                TotalProducts = totalProducts,
                LowStockProducts = lowStockProducts,
                ActiveUsers = activeUsers,
                TodaySales = todaySales,
                TodayRevenue = todayRevenue,
                PendingOrders = pendingOrders,
                LastBackup = lastBackup,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect business metrics");
            throw;
        }
    }

    public async Task<ApplicationMetrics> GetApplicationMetricsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            lock (_lock)
            {
                var uptime = DateTime.UtcNow - _startTime;
                var averageResponseTime = _responseTimes.Any() ? _responseTimes.Average() : 0;
                var requestsPerSecond = _totalRequests / Math.Max(uptime.TotalSeconds, 1);

                return new ApplicationMetrics
                {
                    Uptime = uptime,
                    TotalRequests = _totalRequests,
                    FailedRequests = _failedRequests,
                    AverageResponseTime = Math.Round(averageResponseTime, 2),
                    RequestsPerSecond = Math.Round(requestsPerSecond, 2),
                    ActiveSessions = _activeSessions,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect application metrics");
            throw;
        }
    }

    public void IncrementRequestCount()
    {
        Interlocked.Increment(ref _totalRequests);
    }

    public void IncrementFailedRequestCount()
    {
        Interlocked.Increment(ref _failedRequests);
    }

    public void RecordResponseTime(double responseTimeMs)
    {
        lock (_lock)
        {
            _responseTimes.Add(responseTimeMs);
            
            // Keep only the last 1000 response times to prevent memory issues
            if (_responseTimes.Count > 1000)
            {
                _responseTimes.RemoveAt(0);
            }
        }
    }

    public void RecordActiveSession()
    {
        Interlocked.Increment(ref _activeSessions);
    }

    public void RemoveActiveSession()
    {
        Interlocked.Decrement(ref _activeSessions);
    }

    private async Task<double> GetCpuUsageAsync()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            var startTime = DateTime.UtcNow;
            var startCpuUsage = process.TotalProcessorTime;
            
            await Task.Delay(100); // Short delay to measure CPU usage
            
            var endTime = DateTime.UtcNow;
            var endCpuUsage = process.TotalProcessorTime;
            
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            
            return Math.Round(cpuUsageTotal * 100, 2);
        }
        catch
        {
            return 0;
        }
    }
}