using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_API.Services.Health;

public class SystemMetricsHealthCheck : IHealthCheck
{
    private readonly ILogger<SystemMetricsHealthCheck> _logger;

    public SystemMetricsHealthCheck(ILogger<SystemMetricsHealthCheck> logger)
    {
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var data = new Dictionary<string, object>();
            var warnings = new List<string>();

            // Get system metrics
            await GetCpuUsage(data, warnings);
            await GetMemoryUsage(data, warnings);
            await GetDiskUsage(data, warnings);
            GetProcessMetrics(data, warnings);

            var status = warnings.Any() ? HealthStatus.Degraded : HealthStatus.Healthy;
            var description = status == HealthStatus.Healthy 
                ? "System metrics are within normal ranges"
                : $"System metrics have warnings: {string.Join(", ", warnings)}";

            return new HealthCheckResult(status, description, data: data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "System metrics health check failed");
            return HealthCheckResult.Unhealthy("System metrics check failed", ex);
        }
    }

    private async Task GetCpuUsage(Dictionary<string, object> data, List<string> warnings)
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            var startTime = DateTime.UtcNow;
            var startCpuUsage = process.TotalProcessorTime;
            
            await Task.Delay(1000); // Wait 1 second to measure CPU usage
            
            var endTime = DateTime.UtcNow;
            var endCpuUsage = process.TotalProcessorTime;
            
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            var cpuUsagePercent = cpuUsageTotal * 100;

            data["CpuUsagePercent"] = Math.Round(cpuUsagePercent, 2);

            if (cpuUsagePercent > 80)
            {
                warnings.Add($"High CPU usage: {cpuUsagePercent:F2}%");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get CPU usage");
            data["CpuUsageError"] = ex.Message;
        }
    }

    private async Task GetMemoryUsage(Dictionary<string, object> data, List<string> warnings)
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            var memoryUsed = process.WorkingSet64;
            var memoryUsedMB = memoryUsed / (1024 * 1024);

            // Get total system memory (this is a simplified approach)
            var totalMemory = GC.GetTotalMemory(false);
            var totalMemoryMB = totalMemory / (1024 * 1024);

            data["MemoryUsedMB"] = memoryUsedMB;
            data["MemoryUsedBytes"] = memoryUsed;
            data["GCTotalMemoryMB"] = totalMemoryMB;

            if (memoryUsedMB > 1000) // More than 1GB
            {
                warnings.Add($"High memory usage: {memoryUsedMB}MB");
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get memory usage");
            data["MemoryUsageError"] = ex.Message;
        }
    }

    private async Task GetDiskUsage(Dictionary<string, object> data, List<string> warnings)
    {
        try
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
                .ToList();

            var diskData = new List<object>();

            foreach (var drive in drives)
            {
                var totalSize = drive.TotalSize;
                var freeSpace = drive.TotalFreeSpace;
                var usedSpace = totalSize - freeSpace;
                var usagePercent = (double)usedSpace / totalSize * 100;

                var driveInfo = new
                {
                    DriveName = drive.Name,
                    TotalSizeGB = Math.Round(totalSize / (1024.0 * 1024.0 * 1024.0), 2),
                    FreeSpaceGB = Math.Round(freeSpace / (1024.0 * 1024.0 * 1024.0), 2),
                    UsedSpaceGB = Math.Round(usedSpace / (1024.0 * 1024.0 * 1024.0), 2),
                    UsagePercent = Math.Round(usagePercent, 2)
                };

                diskData.Add(driveInfo);

                if (usagePercent > 90)
                {
                    warnings.Add($"High disk usage on {drive.Name}: {usagePercent:F2}%");
                }
            }

            data["DiskUsage"] = diskData;
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get disk usage");
            data["DiskUsageError"] = ex.Message;
        }
    }

    private void GetProcessMetrics(Dictionary<string, object> data, List<string> warnings)
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            
            data["ProcessId"] = process.Id;
            data["ProcessName"] = process.ProcessName;
            data["StartTime"] = process.StartTime;
            data["Uptime"] = DateTime.Now - process.StartTime;
            data["ThreadCount"] = process.Threads.Count;
            data["HandleCount"] = process.HandleCount;

            if (process.Threads.Count > 100)
            {
                warnings.Add($"High thread count: {process.Threads.Count}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get process metrics");
            data["ProcessMetricsError"] = ex.Message;
        }
    }
}