using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    /// <summary>
    /// Middleware for rate limiting requests per IP address
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly RateLimitOptions _options;

        public RateLimitingMiddleware(
            RequestDelegate next,
            IMemoryCache cache,
            ILogger<RateLimitingMiddleware> logger,
            RateLimitOptions options)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            var key = $"rate_limit_{clientId}";

            if (!_cache.TryGetValue(key, out RateLimitInfo rateLimitInfo))
            {
                rateLimitInfo = new RateLimitInfo
                {
                    RequestCount = 1,
                    WindowStart = DateTime.UtcNow
                };
                _cache.Set(key, rateLimitInfo, TimeSpan.FromMinutes(_options.WindowSizeInMinutes));
            }
            else
            {
                // Check if we're still in the same window
                if (DateTime.UtcNow - rateLimitInfo.WindowStart < TimeSpan.FromMinutes(_options.WindowSizeInMinutes))
                {
                    rateLimitInfo.RequestCount++;
                    
                    if (rateLimitInfo.RequestCount > _options.MaxRequests)
                    {
                        _logger.LogWarning("Rate limit exceeded for client {ClientId}. Request count: {RequestCount}", 
                            clientId, rateLimitInfo.RequestCount);

                        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        context.Response.Headers["Retry-After"] = "60";
                        
                        await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                        return;
                    }
                }
                else
                {
                    // Reset the window
                    rateLimitInfo.RequestCount = 1;
                    rateLimitInfo.WindowStart = DateTime.UtcNow;
                }

                _cache.Set(key, rateLimitInfo, TimeSpan.FromMinutes(_options.WindowSizeInMinutes));
            }

            // Add rate limit headers
            context.Response.Headers["X-RateLimit-Limit"] = _options.MaxRequests.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = 
                Math.Max(0, _options.MaxRequests - rateLimitInfo.RequestCount).ToString();
            context.Response.Headers["X-RateLimit-Reset"] = 
                ((DateTimeOffset)(rateLimitInfo.WindowStart.AddMinutes(_options.WindowSizeInMinutes))).ToUnixTimeSeconds().ToString();

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Try to get the real IP address from headers (for load balancers/proxies)
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

            // Fall back to connection remote IP
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }

    public class RateLimitInfo
    {
        public int RequestCount { get; set; }
        public DateTime WindowStart { get; set; }
    }

    public class RateLimitOptions
    {
        public int MaxRequests { get; set; } = 100;
        public int WindowSizeInMinutes { get; set; } = 1;
    }
}