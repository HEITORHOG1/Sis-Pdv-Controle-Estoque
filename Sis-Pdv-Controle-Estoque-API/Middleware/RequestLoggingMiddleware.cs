using Sis_Pdv_Controle_Estoque_API.Services;
using System.Diagnostics;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses with performance metrics
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationLogger _appLogger;

        public RequestLoggingMiddleware(RequestDelegate next, IApplicationLogger appLogger)
        {
            _next = next;
            _appLogger = appLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var correlationId = context.TraceIdentifier;

            // Log request
            await LogRequestAsync(context, correlationId);

            // Capture original response body stream
            var originalBodyStream = context.Response.Body;

            try
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                // Log response
                await LogResponseAsync(context, correlationId, stopwatch.ElapsedMilliseconds, responseBody);

                // Copy response back to original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
                stopwatch.Stop();
            }
        }

        private async Task LogRequestAsync(HttpContext context, string correlationId)
        {
            var request = context.Request;
            
            object? requestData = null;
            
            // Only log request body for POST, PUT, PATCH
            if (request.Method is "POST" or "PUT" or "PATCH" && 
                request.ContentLength > 0 && 
                request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;

                // Only log if body is not too large (avoid logging large files)
                if (bodyAsText.Length < 10000)
                {
                    requestData = bodyAsText;
                }
            }

            _appLogger.LogApiRequest(request.Method, request.Path, requestData, correlationId);
        }

        private async Task LogResponseAsync(HttpContext context, string correlationId, long elapsedMs, MemoryStream responseBody)
        {
            var response = context.Response;
            
            object? responseData = null;
            
            // Only log response body for successful JSON responses and if not too large
            if (response.ContentType?.Contains("application/json") == true && 
                responseBody.Length > 0 && 
                responseBody.Length < 10000)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);
                responseData = responseText;
            }

            _appLogger.LogApiResponse(
                context.Request.Method, 
                context.Request.Path, 
                response.StatusCode, 
                responseData, 
                correlationId, 
                elapsedMs);

            // Log performance metric if request took too long
            if (elapsedMs > 1000)
            {
                _appLogger.LogPerformanceMetric(
                    $"{context.Request.Method} {context.Request.Path}", 
                    elapsedMs, 
                    correlationId);
            }
        }
    }
}