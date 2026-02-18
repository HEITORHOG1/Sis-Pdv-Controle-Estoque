using Commands.Inventory.AdjustStock;
using Commands.Inventory.GetStockAlerts;
using Commands.Inventory.GetStockMovements;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Services;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Services;
using Microsoft.Extensions.Logging;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Controllers
{
    /// <summary>
    /// Inventory management endpoints for the PDV Control System
    /// </summary>
    /// <remarks>
    /// This controller provides comprehensive inventory management functionality including:
    /// 
    /// **Stock Operations:**
    /// - Real-time stock level monitoring
    /// - Stock adjustments with audit trail
    /// - Stock movement history tracking
    /// - Bulk stock operations
    /// 
    /// **Inventory Control:**
    /// - Low stock alerts and notifications
    /// - Reorder point management
    /// - Stock take and reconciliation
    /// - Inventory valuation reports
    /// 
    /// **Movement Tracking:**
    /// - All stock movements are logged with reasons
    /// - User attribution for all changes
    /// - Timestamp tracking for audit purposes
    /// - Movement type categorization (Sale, Purchase, Adjustment, etc.)
    /// 
    /// **Business Rules:**
    /// - Stock cannot go below zero for regular products
    /// - All adjustments require a reason
    /// - Automatic alerts when stock falls below reorder point
    /// - Integration with sales system for real-time updates
    /// 
    /// **Security:**
    /// - Requires inventory management permissions
    /// - All operations are logged for audit purposes
    /// - User attribution for all stock changes
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/inventory")]
    [Produces("application/json")]
    [Tags("Inventory Management")]
    [Authorize]
    public class InventoryController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationLogger _appLogger;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IMediator mediator, IApplicationLogger appLogger, ILogger<InventoryController> logger)
        {
            _mediator = mediator;
            _appLogger = appLogger;
            _logger = logger;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }

        /// <summary>
        /// Adjust stock level for a specific product
        /// </summary>
        /// <param name="request">Stock adjustment details including product ID, quantity change, and reason</param>
        /// <param name="cancellationToken">Cancellation token for the operation</param>
        /// <returns>Stock adjustment confirmation with updated stock levels</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint allows authorized users to adjust stock levels for products.
        /// All adjustments are logged with user attribution and timestamps for audit purposes.
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///   "quantity": 50,
        ///   "reason": "Stock replenishment from supplier",
        ///   "unitCost": 25.50,
        ///   "reference": "PO-2024-001"
        /// }
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Estoque ajustado com sucesso",
        ///   "data": {
        ///     "movementId": "456e7890-f12b-34c5-d678-901234567890",
        ///     "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///     "previousStock": 25,
        ///     "adjustmentQuantity": 50,
        ///     "newStock": 75,
        ///     "reason": "Stock replenishment from supplier",
        ///     "adjustedBy": "admin@pdvsystem.com",
        ///     "adjustedAt": "2024-01-15T10:30:00Z"
        ///   }
        /// }
        /// ```
        /// 
        /// **Business Rules:**
        /// - Quantity can be positive (increase) or negative (decrease)
        /// - Reason is mandatory for all adjustments
        /// - Stock cannot go below zero unless specifically allowed
        /// - All adjustments trigger reorder point checks
        /// - Cost information is optional but recommended for valuation
        /// 
        /// **Permissions Required:**
        /// - `inventory.manage` permission
        /// - Active user account
        /// - Valid authentication token
        /// </remarks>
        /// <response code="200">Stock adjusted successfully</response>
        /// <response code="400">Invalid request data or business rule violation</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="404">Product not found</response>
        /// <response code="409">Conflict - insufficient stock for negative adjustment</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("adjust")]
        [Authorize(Policy = "InventoryManagement")]
        [ProducesResponseType(typeof(ApiResponse<AdjustStockResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<AdjustStockResponse>>> AdjustStock(
            [FromBody, Required] AdjustStockRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("AdjustStock", GetCurrentUserId(), new { request.ProductId, request.NewQuantity, request.Reason });
                
                var result = await _mediator.Send(request, cancellationToken);
                
                if (result.Success)
                {
                    return Ok(ApiResponse<AdjustStockResponse>.Ok(result, "Estoque ajustado com sucesso"));
                }
                
                return BadRequest(ApiResponse<AdjustStockResponse>.Error(result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao ajustar estoque do produto {ProductId}", request.ProductId);
                return StatusCode(500, ApiResponse<AdjustStockResponse>.Error("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Obtém o histórico de movimentações de estoque
        /// </summary>
        [HttpGet("movements")]
        [Authorize(Policy = "InventoryView")]
        public async Task<ActionResult<ApiResponse<GetStockMovementsResponse>>> GetStockMovements(
            [FromQuery] GetStockMovementsRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetStockMovements", GetCurrentUserId(), request);
                
                var result = await _mediator.Send(request, cancellationToken);
                
                return Ok(ApiResponse<GetStockMovementsResponse>.Ok(result, "Movimentações obtidas com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter movimentações de estoque");
                return StatusCode(500, ApiResponse<GetStockMovementsResponse>.Error("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Obtém alertas de estoque (produtos com estoque baixo ou zerado)
        /// </summary>
        [HttpGet("alerts")]
        [Authorize(Policy = "InventoryView")]
        public async Task<ActionResult<ApiResponse<GetStockAlertsResponse>>> GetStockAlerts(
            [FromQuery] StockAlertType? alertType,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetStockAlerts", GetCurrentUserId(), new { alertType });
                
                var request = new GetStockAlertsRequest { AlertType = alertType };
                var result = await _mediator.Send(request, cancellationToken);
                
                return Ok(ApiResponse<GetStockAlertsResponse>.Ok(result, "Alertas obtidos com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alertas de estoque");
                return StatusCode(500, ApiResponse<GetStockAlertsResponse>.Error("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Valida disponibilidade de estoque para um produto
        /// </summary>
        [HttpPost("validate-stock")]
        [Authorize(Policy = "InventoryView")]
        public async Task<ActionResult<ApiResponse<StockValidationResult>>> ValidateStock(
            [FromBody] ValidateStockRequest request,
            [FromServices] IStockValidationService stockValidationService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("ValidateStock", GetCurrentUserId(), request);
                
                var result = await stockValidationService.ValidateStockAvailabilityAsync(
                    request.ProductId, 
                    request.RequestedQuantity, 
                    cancellationToken);
                
                return Ok(ApiResponse<StockValidationResult>.Ok(result, "Validação realizada com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar estoque do produto {ProductId}", request.ProductId);
                return StatusCode(500, ApiResponse<StockValidationResult>.Error("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Valida disponibilidade de estoque para múltiplos produtos
        /// </summary>
        [HttpPost("validate-stock-batch")]
        [Authorize(Policy = "InventoryView")]
        public async Task<ActionResult<ApiResponse<StockValidationResult>>> ValidateStockBatch(
            [FromBody] IEnumerable<StockValidationRequest> requests,
            [FromServices] IStockValidationService stockValidationService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("ValidateStockBatch", GetCurrentUserId(), requests);
                
                var result = await stockValidationService.ValidateStockAvailabilityAsync(requests, cancellationToken);
                
                return Ok(ApiResponse<StockValidationResult>.Ok(result, "Validação em lote realizada com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar estoque em lote");
                return StatusCode(500, ApiResponse<StockValidationResult>.Error("Erro interno do servidor"));
            }
        }
    }

    public class ValidateStockRequest
    {
        public Guid ProductId { get; set; }
        public int RequestedQuantity { get; set; }
    }
}