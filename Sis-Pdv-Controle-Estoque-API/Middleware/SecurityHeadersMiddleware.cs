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
            // Add security headers
            AddSecurityHeaders(context.Response);

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

            // Strict Transport Security (HSTS)
            response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

            // Content Security Policy
            response.Headers["Content-Security-Policy"] = 
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self'; " +
                "connect-src 'self'; " +
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

            // Cross-Origin Embedder Policy
            response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";

            // Cross-Origin Opener Policy
            response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";

            // Cross-Origin Resource Policy
            response.Headers["Cross-Origin-Resource-Policy"] = "same-origin";
        }
    }
}