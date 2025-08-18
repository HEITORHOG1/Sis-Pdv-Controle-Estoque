using System.Text.Json.Serialization;

namespace Sis_Pdv_Controle_Estoque_API.Models.Health;

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;
    public TimeSpan TotalDuration { get; set; }
    public Dictionary<string, HealthCheckResult> Results { get; set; } = new();
}

public class HealthCheckResult
{
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TimeSpan Duration { get; set; }
    public Dictionary<string, object>? Data { get; set; }
    public string? Exception { get; set; }
}

public class SystemMetrics
{
    public double CpuUsagePercent { get; set; }
    public long MemoryUsedBytes { get; set; }
    public long MemoryTotalBytes { get; set; }
    public double MemoryUsagePercent { get; set; }
    public long DiskUsedBytes { get; set; }
    public long DiskTotalBytes { get; set; }
    public double DiskUsagePercent { get; set; }
    public int ActiveConnections { get; set; }
    public DateTime Timestamp { get; set; }
}

public class BusinessMetrics
{
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int ActiveUsers { get; set; }
    public int TodaySales { get; set; }
    public decimal TodayRevenue { get; set; }
    public int PendingOrders { get; set; }
    public DateTime LastBackup { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ApplicationMetrics
{
    public TimeSpan Uptime { get; set; }
    public long TotalRequests { get; set; }
    public long FailedRequests { get; set; }
    public double AverageResponseTime { get; set; }
    public double RequestsPerSecond { get; set; }
    public int ActiveSessions { get; set; }
    public DateTime Timestamp { get; set; }
}