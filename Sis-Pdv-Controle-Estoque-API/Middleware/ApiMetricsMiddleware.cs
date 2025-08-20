using System.Diagnostics;
using System.Security.Claims;
using Sis_Pdv_Controle_Estoque_API.Services.Metrics;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    /// <summary>
    /// Middleware for capturing API metrics and structured logging
    /// </summary>
    public class ApiMetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiMetricsMiddleware> _logger;
        private readonly IApiMetricsService _metricsService;

        public ApiMetricsMiddleware(
            RequestDelegate next,
            ILogger<ApiMetricsMiddleware> logger,
            IApiMetricsService metricsService)
        {
            _next = next;
            _logger = logger;
            _metricsService = metricsService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip metrics for non-API endpoints
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            var correlationId = context.TraceIdentifier;
            var userId = GetUserId(context);
            var endpoint = GetEndpoint(context);
            var method = context.Request.Method;

            // Add correlation ID to response headers
            context.Response.Headers.Add("X-Correlation-ID", correlationId);

            // Log request start
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId,
                ["UserId"] = userId?.ToString() ?? "anonymous",
                ["Endpoint"] = endpoint,
                ["Method"] = method,
                ["UserAgent"] = context.Request.Headers.UserAgent.ToString(),
                ["RemoteIP"] = GetClientIpAddress(context)
            });

            _logger.LogInformation("API Request Started: {Method} {Endpoint}", method, endpoint);

            Exception? exception = null;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                context.Response.StatusCode = 500;
                _logger.LogError(ex, "Unhandled exception in API request");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                var statusCode = context.Response.StatusCode;

                // Record metrics
                _metricsService.RecordApiRequest(endpoint, method, statusCode, duration, userId);

                // Log request completion
                if (exception != null)
                {
                    _logger.LogError("API Request Failed: {Method} {Endpoint} - {StatusCode} ({Duration}ms) - Exception: {Exception}",
                        method, endpoint, statusCode, duration, exception.Message);
                }
                else if (statusCode >= 400)
                {
                    _logger.LogWarning("API Request Completed with Error: {Method} {Endpoint} - {StatusCode} ({Duration}ms)",
                        method, endpoint, statusCode, duration);
                }
                else
                {
                    _logger.LogInformation("API Request Completed: {Method} {Endpoint} - {StatusCode} ({Duration}ms)",
                        method, endpoint, statusCode, duration);
                }

                // Record specific error metrics
                if (statusCode >= 400)
                {
                    RecordErrorMetrics(endpoint, statusCode, exception);
                }
            }
        }

        private Guid? GetUserId(HttpContext context)
        {
            var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        private string GetEndpoint(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            var routeTemplate = context.GetRouteData()?.Values["action"]?.ToString();
            
            // Try to get the route template for better grouping
            if (!string.IsNullOrEmpty(routeTemplate))
            {
                var controller = context.GetRouteData()?.Values["controller"]?.ToString();
                return $"{controller}/{routeTemplate}";
            }

            return path;
        }

        private string GetClientIpAddress(HttpContext context)
        {
            // Check for forwarded IP first (in case of proxy/load balancer)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private void RecordErrorMetrics(string endpoint, int statusCode, Exception? exception)
        {
            var errorType = statusCode switch
            {
                400 => "BadRequest",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "NotFound",
                409 => "Conflict",
                422 => "ValidationError",
                >= 500 => "InternalServerError",
                _ => "ClientError"
            };

            if (exception != null)
            {
                _metricsService.RecordBusinessRuleViolation(endpoint, errorType, exception.Message);
            }
        }
    }

    /// <summary>
    /// Extension methods for registering the API metrics middleware
    /// </summary>
    public static class ApiMetricsMiddlewareExtensions
    {
        /// <summary>
        /// Adds the API metrics middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseApiMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiMetricsMiddleware>();
        }
    }
}