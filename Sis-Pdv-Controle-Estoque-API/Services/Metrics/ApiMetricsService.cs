using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Sis_Pdv_Controle_Estoque_API.Services.Metrics
{
    /// <summary>
    /// Service for collecting and reporting API metrics
    /// </summary>
    public interface IApiMetricsService
    {
        /// <summary>
        /// Records API request metrics
        /// </summary>
        void RecordApiRequest(string endpoint, string method, int statusCode, long durationMs, Guid? userId = null);

        /// <summary>
        /// Records validation error metrics
        /// </summary>
        void RecordValidationError(string endpoint, string errorType, IEnumerable<string> errors);

        /// <summary>
        /// Records business rule violation metrics
        /// </summary>
        void RecordBusinessRuleViolation(string endpoint, string ruleType, string details);

        /// <summary>
        /// Records database operation metrics
        /// </summary>
        void RecordDatabaseOperation(string operation, string entityType, long durationMs, bool success);

        /// <summary>
        /// Records inventory operation metrics
        /// </summary>
        void RecordInventoryOperation(string operation, Guid productId, decimal quantity, bool success);

        /// <summary>
        /// Records product operation metrics
        /// </summary>
        void RecordProductOperation(string operation, Guid productId, bool success);
    }

    public class ApiMetricsService : IApiMetricsService
    {
        private readonly Meter _meter;
        private readonly Counter<long> _apiRequestCounter;
        private readonly Histogram<long> _apiRequestDuration;
        private readonly Counter<long> _validationErrorCounter;
        private readonly Counter<long> _businessRuleViolationCounter;
        private readonly Counter<long> _databaseOperationCounter;
        private readonly Histogram<long> _databaseOperationDuration;
        private readonly Counter<long> _inventoryOperationCounter;
        private readonly Counter<long> _productOperationCounter;
        private readonly ILogger<ApiMetricsService> _logger;

        public ApiMetricsService(ILogger<ApiMetricsService> logger)
        {
            _logger = logger;
            _meter = new Meter("Sis.Pdv.ControleEstoque.API", "1.0.0");

            // Initialize counters and histograms
            _apiRequestCounter = _meter.CreateCounter<long>(
                "api_requests_total",
                "requests",
                "Total number of API requests");

            _apiRequestDuration = _meter.CreateHistogram<long>(
                "api_request_duration_ms",
                "milliseconds",
                "Duration of API requests in milliseconds");

            _validationErrorCounter = _meter.CreateCounter<long>(
                "validation_errors_total",
                "errors",
                "Total number of validation errors");

            _businessRuleViolationCounter = _meter.CreateCounter<long>(
                "business_rule_violations_total",
                "violations",
                "Total number of business rule violations");

            _databaseOperationCounter = _meter.CreateCounter<long>(
                "database_operations_total",
                "operations",
                "Total number of database operations");

            _databaseOperationDuration = _meter.CreateHistogram<long>(
                "database_operation_duration_ms",
                "milliseconds",
                "Duration of database operations in milliseconds");

            _inventoryOperationCounter = _meter.CreateCounter<long>(
                "inventory_operations_total",
                "operations",
                "Total number of inventory operations");

            _productOperationCounter = _meter.CreateCounter<long>(
                "product_operations_total",
                "operations",
                "Total number of product operations");
        }

        public void RecordApiRequest(string endpoint, string method, int statusCode, long durationMs, Guid? userId = null)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("endpoint", endpoint),
                new("method", method),
                new("status_code", statusCode),
                new("status_class", GetStatusClass(statusCode)),
                new("user_id", userId?.ToString() ?? "anonymous")
            };

            _apiRequestCounter.Add(1, tags);
            _apiRequestDuration.Record(durationMs, tags);

            _logger.LogInformation("API Request: {Method} {Endpoint} - {StatusCode} ({Duration}ms) - User: {UserId}",
                method, endpoint, statusCode, durationMs, userId);
        }

        public void RecordValidationError(string endpoint, string errorType, IEnumerable<string> errors)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("endpoint", endpoint),
                new("error_type", errorType),
                new("error_count", errors.Count())
            };

            _validationErrorCounter.Add(1, tags);

            _logger.LogWarning("Validation Error: {Endpoint} - {ErrorType} - Errors: {Errors}",
                endpoint, errorType, string.Join("; ", errors));
        }

        public void RecordBusinessRuleViolation(string endpoint, string ruleType, string details)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("endpoint", endpoint),
                new("rule_type", ruleType)
            };

            _businessRuleViolationCounter.Add(1, tags);

            _logger.LogWarning("Business Rule Violation: {Endpoint} - {RuleType} - {Details}",
                endpoint, ruleType, details);
        }

        public void RecordDatabaseOperation(string operation, string entityType, long durationMs, bool success)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("operation", operation),
                new("entity_type", entityType),
                new("success", success)
            };

            _databaseOperationCounter.Add(1, tags);
            _databaseOperationDuration.Record(durationMs, tags);

            if (success)
            {
                _logger.LogDebug("Database Operation: {Operation} on {EntityType} completed in {Duration}ms",
                    operation, entityType, durationMs);
            }
            else
            {
                _logger.LogError("Database Operation Failed: {Operation} on {EntityType} after {Duration}ms",
                    operation, entityType, durationMs);
            }
        }

        public void RecordInventoryOperation(string operation, Guid productId, decimal quantity, bool success)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("operation", operation),
                new("product_id", productId.ToString()),
                new("success", success)
            };

            _inventoryOperationCounter.Add(1, tags);

            _logger.LogInformation("Inventory Operation: {Operation} - Product: {ProductId} - Quantity: {Quantity} - Success: {Success}",
                operation, productId, quantity, success);
        }

        public void RecordProductOperation(string operation, Guid productId, bool success)
        {
            var tags = new KeyValuePair<string, object?>[]
            {
                new("operation", operation),
                new("product_id", productId.ToString()),
                new("success", success)
            };

            _productOperationCounter.Add(1, tags);

            _logger.LogInformation("Product Operation: {Operation} - Product: {ProductId} - Success: {Success}",
                operation, productId, success);
        }

        private static string GetStatusClass(int statusCode)
        {
            return statusCode switch
            {
                >= 200 and < 300 => "2xx",
                >= 300 and < 400 => "3xx",
                >= 400 and < 500 => "4xx",
                >= 500 => "5xx",
                _ => "unknown"
            };
        }

        public void Dispose()
        {
            _meter?.Dispose();
        }
    }
}