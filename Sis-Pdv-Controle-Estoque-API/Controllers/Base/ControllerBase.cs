using Commands;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Models;

namespace Sis_Pdv_Controle_Estoque_API.Controllers.Base
{
    /// <summary>
    /// Enhanced base controller with standardized response handling
    /// </summary>
    public class ControllerBase : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the correlation ID from the current request
        /// </summary>
        protected string CorrelationId => HttpContext.TraceIdentifier;

        /// <summary>
        /// Legacy method for backward compatibility with existing Response class
        /// </summary>
        public async Task<IActionResult> ResponseAsync(Response response)
        {
            if (!response.Notifications.Any())
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                    
                    // Convert legacy response to new standardized format
                    return Ok(ApiResponse<object>.Ok(response.Data, "Operation completed successfully", CorrelationId));
                }
                catch (Exception ex)
                {
                    return BadRequest(ApiResponse.Error(
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
        /// Creates an error response
        /// </summary>
        protected IActionResult Error(string message, IEnumerable<string>? errors = null)
        {
            return BadRequest(ApiResponse.Error(message, errors, CorrelationId));
        }

        /// <summary>
        /// Creates an error response with single error
        /// </summary>
        protected IActionResult Error(string message, string error)
        {
            return BadRequest(ApiResponse.Error(message, error, CorrelationId));
        }

        /// <summary>
        /// Creates a not found response
        /// </summary>
        protected IActionResult NotFoundError(string message)
        {
            return NotFound(ApiResponse.Error(message, correlationId: CorrelationId));
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        public async Task<IActionResult> ResponseExceptionAsync(Exception ex)
        {
            return BadRequest(ApiResponse.Error(
                "An error occurred while processing your request",
                ex.Message,
                CorrelationId));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}