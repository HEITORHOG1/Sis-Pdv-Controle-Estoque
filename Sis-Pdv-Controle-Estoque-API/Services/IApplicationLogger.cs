namespace Sis_Pdv_Controle_Estoque_API.Services
{
    /// <summary>
    /// Application-specific logging interface with structured logging support
    /// </summary>
    public interface IApplicationLogger
    {
        /// <summary>
        /// Logs a user action with context
        /// </summary>
        void LogUserAction(string action, Guid? userId = null, object? data = null, string? correlationId = null);

        /// <summary>
        /// Logs a business event
        /// </summary>
        void LogBusinessEvent(string eventName, object? data = null, string? correlationId = null);

        /// <summary>
        /// Logs an API request
        /// </summary>
        void LogApiRequest(string method, string path, object? requestData = null, string? correlationId = null);

        /// <summary>
        /// Logs an API response
        /// </summary>
        void LogApiResponse(string method, string path, int statusCode, object? responseData = null, string? correlationId = null, long? elapsedMs = null);

        /// <summary>
        /// Logs a database operation
        /// </summary>
        void LogDatabaseOperation(string operation, string entityName, object? data = null, string? correlationId = null);

        /// <summary>
        /// Logs a validation error
        /// </summary>
        void LogValidationError(string operation, IEnumerable<string> errors, string? correlationId = null);

        /// <summary>
        /// Logs a business rule violation
        /// </summary>
        void LogBusinessRuleViolation(string rule, string details, string? correlationId = null);

        /// <summary>
        /// Logs performance metrics
        /// </summary>
        void LogPerformanceMetric(string operation, long elapsedMs, string? correlationId = null);
    }
}