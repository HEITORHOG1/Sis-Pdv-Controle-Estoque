using Sis_Pdv_Controle_Estoque_API.Services.Health;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_API.Middleware;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MetricsMiddleware> _logger;

    public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IMetricsCollectionService metricsService)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Increment request count
            metricsService.IncrementRequestCount();
            
            // Track active session if authenticated
            var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                metricsService.RecordActiveSession();
            }

            await _next(context);

            // Record response time
            stopwatch.Stop();
            metricsService.RecordResponseTime(stopwatch.ElapsedMilliseconds);

            // Log request details for monitoring
            _logger.LogInformation(
                "Request {Method} {Path} completed in {ElapsedMs}ms with status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            // Increment failed request count
            metricsService.IncrementFailedRequestCount();
            
            _logger.LogError(ex, 
                "Request {Method} {Path} failed after {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
            
            throw;
        }
        finally
        {
            // Remove active session if it was tracked
            var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                metricsService.RemoveActiveSession();
            }
        }
    }
}