using System.Security.Claims;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for certain paths
            var path = context.Request.Path.Value?.ToLower();
            if (ShouldSkipAuthentication(path))
            {
                await _next(context);
                return;
            }

            // Check if user is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Add user context information for logging
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = context.User.FindFirst(ClaimTypes.Name)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    context.Items["UserId"] = userId;
                    context.Items["UserName"] = userName;
                }
            }

            await _next(context);
        }

        private static bool ShouldSkipAuthentication(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var skipPaths = new[]
            {
                "/auth/login",
                "/auth/refresh",
                "/health",
                "/swagger",
                "/api-docs"
            };

            return skipPaths.Any(skipPath => path.StartsWith(skipPath));
        }
    }
}