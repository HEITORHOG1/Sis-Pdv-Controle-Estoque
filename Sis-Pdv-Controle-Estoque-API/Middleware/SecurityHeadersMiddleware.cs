using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    /// <summary>
    /// Middleware to add security headers to all responses
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            var isSwaggerOrHealth = path.StartsWith("/swagger") || path.StartsWith("/api-docs") || path.StartsWith("/health");

            // Always relax headers for Swagger and Health endpoints to avoid CSP/COEP/COOP issues
            if (!isSwaggerOrHealth)
            {
                AddSecurityHeaders(context.Response);
            }

            await _next(context);
        }

        private static void AddSecurityHeaders(HttpResponse response)
        {
            // Prevent clickjacking attacks
            response.Headers["X-Frame-Options"] = "DENY";

            // Prevent MIME type sniffing
            response.Headers["X-Content-Type-Options"] = "nosniff";

            // Enable XSS protection
            response.Headers["X-XSS-Protection"] = "1; mode=block";

            // Strict Transport Security (HSTS) - only meaningful over HTTPS
            if (response.HttpContext.Request.IsHttps)
            {
                response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
            }

            // Content Security Policy
            response.Headers["Content-Security-Policy"] = 
                "default-src 'self' data: blob:; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' blob:; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https: blob:; " +
                "font-src 'self' data:; " +
                "connect-src 'self' http: https:; " +
                "frame-ancestors 'none'";

            // Referrer Policy
            response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Permissions Policy (formerly Feature Policy)
            response.Headers["Permissions-Policy"] = 
                "camera=(), " +
                "microphone=(), " +
                "geolocation=(), " +
                "payment=()";

            // Remove server information
            response.Headers.Remove("Server");
            response.Headers.Remove("X-Powered-By");
            response.Headers.Remove("X-AspNet-Version");
            response.Headers.Remove("X-AspNetMvc-Version");

            // NOTE: Do NOT set COEP/COOP/CORP globally as they may break Swagger UI and other tooling
            // If you need them, scope them only to specific endpoints that require cross-origin isolation
        }
    }
}