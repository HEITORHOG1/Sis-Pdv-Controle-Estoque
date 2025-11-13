using System.Text.Json.Serialization;

namespace Sis_Pdv_Controle_Estoque_API.Models
{
    /// <summary>
    /// Standard API response wrapper for all endpoints
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The actual data payload
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        /// <summary>
        /// Human-readable message describing the result
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        /// <summary>
        /// List of validation errors or warnings
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? Errors { get; set; }

        /// <summary>
        /// Correlation ID for request tracking
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Timestamp of the response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        public static ApiResponse<T> Ok(T data, string? message = null, string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a successful response without data
        /// </summary>
        public static ApiResponse<T> Ok(string? message = null, string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static ApiResponse<T> Error(string message, IEnumerable<string>? errors = null, string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates an error response with single error
        /// </summary>
        public static ApiResponse<T> Error(string message, string error, string? correlationId = null)
        {
            return Error(message, new[] { error }, correlationId);
        }
    }

    /// <summary>
    /// Non-generic API response for operations that don't return data
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Creates a successful response without data
        /// </summary>
        public static new ApiResponse Ok(string? message = null, string? correlationId = null)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static new ApiResponse Error(string message, IEnumerable<string>? errors = null, string? correlationId = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates an error response with single error
        /// </summary>
        public static new ApiResponse Error(string message, string error, string? correlationId = null)
        {
            return Error(message, new[] { error }, correlationId);
        }
    }
}