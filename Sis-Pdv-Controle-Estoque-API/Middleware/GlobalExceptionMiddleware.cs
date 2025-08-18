using Sis_Pdv_Controle_Estoque_API.Exceptions;
using Sis_Pdv_Controle_Estoque_API.Models;
using System.Net;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_API.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns standardized error responses
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = context.TraceIdentifier;
            
            // Log the exception with context
            _logger.LogError(exception, 
                "An unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}",
                correlationId, context.Request.Path, context.Request.Method);

            var response = CreateErrorResponse(exception, correlationId);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(exception);

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static ApiResponse CreateErrorResponse(Exception exception, string correlationId)
        {
            return exception switch
            {
                ValidationException validationEx => ApiResponse.Error(
                    "Validation failed", 
                    validationEx.Errors, 
                    correlationId),

                NotFoundException notFoundEx => ApiResponse.Error(
                    notFoundEx.Message, 
                    correlationId: correlationId),

                DuplicateException duplicateEx => ApiResponse.Error(
                    duplicateEx.Message, 
                    correlationId: correlationId),

                BusinessRuleException businessEx => ApiResponse.Error(
                    businessEx.Message, 
                    correlationId: correlationId),

                UnauthorizedException unauthorizedEx => ApiResponse.Error(
                    unauthorizedEx.Message, 
                    correlationId: correlationId),

                BusinessException businessEx => ApiResponse.Error(
                    businessEx.Message, 
                    correlationId: correlationId),

                _ => ApiResponse.Error(
                    "An internal server error occurred. Please contact support if the problem persists.", 
                    correlationId: correlationId)
            };
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                DuplicateException => (int)HttpStatusCode.Conflict,
                BusinessRuleException => (int)HttpStatusCode.BadRequest,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                BusinessException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
}