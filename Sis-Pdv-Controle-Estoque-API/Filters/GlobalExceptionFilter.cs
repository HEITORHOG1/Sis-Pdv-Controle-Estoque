using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sis_Pdv_Controle_Estoque_API.Exceptions;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Services.Metrics;
using System.Net;

namespace Sis_Pdv_Controle_Estoque_API.Filters
{
    /// <summary>
    /// Global exception filter for consistent error handling and logging
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IApiMetricsService _metricsService;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger,
            IApiMetricsService metricsService,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _metricsService = metricsService;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            var correlationId = context.HttpContext.TraceIdentifier;
            var endpoint = GetEndpoint(context.HttpContext);
            
            _logger.LogError(context.Exception, 
                "Unhandled exception in {Endpoint} - CorrelationId: {CorrelationId}", 
                endpoint, correlationId);

            var (statusCode, message, errors) = MapException(context.Exception);

            // Record metrics
            _metricsService.RecordBusinessRuleViolation(endpoint, context.Exception.GetType().Name, context.Exception.Message);

            // Create standardized error response
            var response = CreateErrorResponse(message, errors, correlationId, context.Exception);

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;
        }

        private (HttpStatusCode statusCode, string message, IEnumerable<string>? errors) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    "Dados de entrada inválidos",
                    validationEx.Errors
                ),
                
                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    notFoundEx.Message,
                    null
                ),
                
                DuplicateException duplicateEx => (
                    HttpStatusCode.Conflict,
                    duplicateEx.Message,
                    null
                ),
                
                BusinessRuleException businessEx => (
                    HttpStatusCode.UnprocessableEntity,
                    businessEx.Message,
                    null
                ),
                
                UnauthorizedException unauthorizedEx => (
                    HttpStatusCode.Unauthorized,
                    unauthorizedEx.Message,
                    null
                ),
                
                ArgumentException argEx => (
                    HttpStatusCode.BadRequest,
                    "Parâmetros inválidos fornecidos",
                    new[] { argEx.Message }
                ),
                
                InvalidOperationException invalidOpEx => (
                    HttpStatusCode.BadRequest,
                    "Operação inválida",
                    new[] { invalidOpEx.Message }
                ),
                
                TimeoutException timeoutEx => (
                    HttpStatusCode.RequestTimeout,
                    "A operação excedeu o tempo limite",
                    new[] { timeoutEx.Message }
                ),
                
                _ => (
                    HttpStatusCode.InternalServerError,
                    "Ocorreu um erro interno no servidor",
                    null
                )
            };
        }

        private ApiResponse CreateErrorResponse(string message, IEnumerable<string>? errors, string correlationId, Exception exception)
        {
            var response = ApiResponse.Error(message, errors, correlationId);

            // Add additional debug information in development environment
            if (_environment.IsDevelopment())
            {
                var debugErrors = new List<string>();
                if (errors != null)
                {
                    debugErrors.AddRange(errors);
                }
                
                debugErrors.Add($"Exception Type: {exception.GetType().Name}");
                debugErrors.Add($"Stack Trace: {exception.StackTrace}");
                
                if (exception.InnerException != null)
                {
                    debugErrors.Add($"Inner Exception: {exception.InnerException.Message}");
                }

                return ApiResponse.Error(message, debugErrors, correlationId);
            }

            return response;
        }

        private string GetEndpoint(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            var routeData = context.GetRouteData();
            
            if (routeData?.Values != null)
            {
                var controller = routeData.Values["controller"]?.ToString();
                var action = routeData.Values["action"]?.ToString();
                
                if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
                {
                    return $"{controller}/{action}";
                }
            }

            return path;
        }
    }
}