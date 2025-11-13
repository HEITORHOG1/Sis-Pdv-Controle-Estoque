namespace Sis_Pdv_Controle_Estoque_API.Services
{
    /// <summary>
    /// Application-specific logger implementation with structured logging
    /// </summary>
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly ILogger<ApplicationLogger> _logger;

        public ApplicationLogger(ILogger<ApplicationLogger> logger)
        {
            _logger = logger;
        }

        public void LogUserAction(string action, Guid? userId = null, object? data = null, string? correlationId = null)
        {
            _logger.LogInformation("User action performed. Action: {Action}, UserId: {UserId}, CorrelationId: {CorrelationId}, Data: {@Data}",
                action, userId, correlationId, data);
        }

        public void LogBusinessEvent(string eventName, object? data = null, string? correlationId = null)
        {
            _logger.LogInformation("Business event occurred. Event: {EventName}, CorrelationId: {CorrelationId}, Data: {@Data}",
                eventName, correlationId, data);
        }

        public void LogApiRequest(string method, string path, object? requestData = null, string? correlationId = null)
        {
            _logger.LogInformation("API request received. Method: {Method}, Path: {Path}, CorrelationId: {CorrelationId}, Data: {@RequestData}",
                method, path, correlationId, requestData);
        }

        public void LogApiResponse(string method, string path, int statusCode, object? responseData = null, string? correlationId = null, long? elapsedMs = null)
        {
            _logger.LogInformation("API response sent. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, CorrelationId: {CorrelationId}, ElapsedMs: {ElapsedMs}, Data: {@ResponseData}",
                method, path, statusCode, correlationId, elapsedMs, responseData);
        }

        public void LogDatabaseOperation(string operation, string entityName, object? data = null, string? correlationId = null)
        {
            _logger.LogInformation("Database operation performed. Operation: {Operation}, Entity: {EntityName}, CorrelationId: {CorrelationId}, Data: {@Data}",
                operation, entityName, correlationId, data);
        }

        public void LogValidationError(string operation, IEnumerable<string> errors, string? correlationId = null)
        {
            _logger.LogWarning("Validation failed. Operation: {Operation}, CorrelationId: {CorrelationId}, Errors: {@Errors}",
                operation, correlationId, errors);
        }

        public void LogBusinessRuleViolation(string rule, string details, string? correlationId = null)
        {
            _logger.LogWarning("Business rule violated. Rule: {Rule}, Details: {Details}, CorrelationId: {CorrelationId}",
                rule, details, correlationId);
        }

        public void LogPerformanceMetric(string operation, long elapsedMs, string? correlationId = null)
        {
            var logLevel = elapsedMs > 5000 ? LogLevel.Warning : LogLevel.Information;
            _logger.Log(logLevel, "Performance metric. Operation: {Operation}, ElapsedMs: {ElapsedMs}, CorrelationId: {CorrelationId}",
                operation, elapsedMs, correlationId);
        }
    }
}