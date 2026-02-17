using Commands;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Models;

namespace Sis_Pdv_Controle_Estoque_API.Controllers.Base
{
    /// <summary>
    /// Enhanced base controller with standardized response handling.
    /// Inherits from Microsoft.AspNetCore.Mvc.ControllerBase (API controller base).
    /// </summary>
    public class ApiControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the correlation ID from the current request
        /// </summary>
        protected string CorrelationId => HttpContext.TraceIdentifier;

        /// <summary>
        /// Gets the current user ID from claims
        /// </summary>
        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        /// <summary>
        /// Legacy method for backward compatibility with existing Response class.
        /// Note: SaveChangesAsync here is a workaround - ideally this should be handled by the handler/service layer.
        /// </summary>
        [NonAction]
        public async Task<IActionResult> ResponseAsync(Response response, CancellationToken cancellationToken = default)
        {
            if (!response.Notifications.Any())
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    
                    // Convert legacy response to new standardized format
                    return Ok(ApiResponse<object>.Ok(response.Data, "Operation completed successfully", CorrelationId));
                }
                catch (Exception ex)
                {
                    // Return 500 Internal Server Error for exceptions, not BadRequest
                    return StatusCode(500, ApiResponse.Error(
                        "An internal server error occurred. Please contact support if the problem persists.",
                        ex.Message,
                        CorrelationId));
                }
            }
            else
            {
                var errors = response.Notifications.Select(n => n.Message);
                return BadRequest(ApiResponse.Error("Validation failed", errors, CorrelationId));
            }
        }

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        protected IActionResult Success<T>(T data, string? message = null)
        {
            return Ok(ApiResponse<T>.Ok(data, message, CorrelationId));
        }

        /// <summary>
        /// Creates a successful response without data
        /// </summary>
        protected IActionResult Success(string? message = null)
        {
            return Ok(ApiResponse.Ok(message, CorrelationId));
        }

        /// <summary>
        /// Creates an error response (400 Bad Request)
        /// </summary>
        protected IActionResult Error(string message, IEnumerable<string>? errors = null)
        {
            return BadRequest(ApiResponse.Error(message, errors, CorrelationId));
        }

        /// <summary>
        /// Creates an error response with single error (400 Bad Request)
        /// </summary>
        protected IActionResult Error(string message, string error)
        {
            return BadRequest(ApiResponse.Error(message, error, CorrelationId));
        }

        /// <summary>
        /// Creates an internal server error response (500)
        /// </summary>
        protected IActionResult InternalError(string message, string? detail = null)
        {
            return StatusCode(500, ApiResponse.Error(message, detail, CorrelationId));
        }

        /// <summary>
        /// Creates a not found response
        /// </summary>
        protected IActionResult NotFoundError(string message)
        {
            return NotFound(ApiResponse.Error(message, correlationId: CorrelationId));
        }

        /// <summary>
        /// Legacy method for backward compatibility.
        /// Returns 500 Internal Server Error for exceptions.
        /// </summary>
        [NonAction]
        public Task<IActionResult> ResponseExceptionAsync(Exception ex)
        {
            return Task.FromResult<IActionResult>(StatusCode(500, ApiResponse.Error(
                "An error occurred while processing your request",
                ex.Message,
                CorrelationId)));
        }
    }
    
    /// <summary>
    /// Backward compatibility alias. Use ApiControllerBase instead.
    /// </summary>
    [Obsolete("Use ApiControllerBase instead. This alias will be removed in a future version.")]
    public class ControllerBase : ApiControllerBase
    {
        public ControllerBase(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}