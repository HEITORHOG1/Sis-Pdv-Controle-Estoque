using Commands.Inventory.AdjustStock;
using Commands.Inventory.GetStockAlerts;
using Commands.Inventory.GetStockMovements;
using Commands.Inventory.CreateMovement;
using Commands.Inventory.RecalculateBalance;
using Model;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Services;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Models.DTOs;
using Sis_Pdv_Controle_Estoque_API.Services;
using Interfaces.Services;
using Sis_Pdv_Controle_Estoque_API.Models.Validation;
using Sis_Pdv_Controle_Estoque_API.Services.Validation;
using Sis_Pdv_Controle_Estoque_API.Services;
using Sis_Pdv_Controle_Estoque_API.Exceptions;
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
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationLogger _appLogger;
        private readonly ILogger<InventoryController> _logger;
        private readonly IModelValidationService _validationService;

        public InventoryController(
            IMediator mediator, 
            IApplicationLogger appLogger, 
            ILogger<InventoryController> logger,
            IModelValidationService validationService)
        {
            _mediator = mediator;
            _appLogger = appLogger;
            _logger = logger;
            _validationService = validationService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }

        /// <summary>
        /// Gets the correlation ID from the current request
        /// </summary>
        private string CorrelationId => HttpContext.TraceIdentifier;

        #region Enhanced REST Endpoints

        /// <summary>
        /// Create a new stock movement with comprehensive validation
        /// </summary>
        /// <param name="request">Stock movement creation data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created stock movement information</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Creates a new stock movement with full validation including business rules.
        /// Supports different movement types: Entry (1), Exit (2), Adjustment (3), Transfer (4).
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "produtoId": "123e4567-e89b-12d3-a456-426614174000",
        ///   "quantity": 50.0,
        ///   "type": 1,
        ///   "reason": "Recebimento de mercadoria do fornecedor",
        ///   "unitCost": 25.50,
        ///   "lote": "LOT001",
        ///   "dataValidade": "2024-12-31T00:00:00Z",
        ///   "reference": "NF-12345"
        /// }
        /// ```
        /// 
        /// **Business Rules:**
        /// - Products must exist and be active
        /// - Perishable products require batch and expiry date
        /// - Exit movements require sufficient stock
        /// - All movements require a valid reason
        /// </remarks>
        /// <response code="201">Stock movement created successfully</response>
        /// <response code="400">Invalid request data or business rule violation</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="404">Product not found</response>
        /// <response code="409">Conflict - insufficient stock for exit movement</response>
        /// <response code="422">Unprocessable entity - validation failed</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("movements")]
        [Authorize(Policy = "InventoryManagement")]
        [ProducesResponseType(typeof(ApiResponse<CreateMovementResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CreateMovementResponse>>> CreateMovement(
            [FromBody, Required] CreateStockMovementRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("CreateMovement", GetCurrentUserId(), new { request.ProdutoId, request.Type, request.Quantity });

                // Validate model
                _validationService.ValidateModel(request);

                // Validate business rules
                var businessValidation = await _validationService.ValidateStockMovementAsync(
                    request.ProdutoId, request.Quantity, request.Type, request.Lote, request.DataValidade);

                if (!businessValidation.IsValid)
                {
                    return UnprocessableEntity(ApiResponse.Error("Validation failed", businessValidation.Errors, CorrelationId));
                }

                // Map to command
                var command = new CreateMovementRequest
                {
                    ProdutoId = request.ProdutoId,
                    Quantity = request.Quantity,
                    Type = request.Type,
                    Reason = request.Reason,
                    UnitCost = request.UnitCost,
                    Lote = request.Lote,
                    DataValidade = request.DataValidade,
                    Reference = request.Reference,
                    UserId = GetCurrentUserId()
                };

                var result = await _mediator.Send(command, cancellationToken);
                
                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetStockMovements), 
                        new { }, 
                        ApiResponse<CreateMovementResponse>.Ok(result, "Movimentação criada com sucesso", CorrelationId));
                }
                
                return BadRequest(ApiResponse<CreateMovementResponse>.Error(result.Message, correlationId: CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.BusinessRuleException ex)
            {
                return Conflict(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock movement for product {ProductId}", request.ProdutoId);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
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
        /// Get paginated stock movement history with filtering
        /// </summary>
        /// <param name="request">Filter and pagination parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of stock movements</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves stock movement history with optional filtering by product, date range, movement type, etc.
        /// 
        /// **Query Parameters:**
        /// - `page`: Page number (1-based, default: 1)
        /// - `pageSize`: Items per page (max: 100, default: 20)
        /// - `produtoId`: Filter by product ID
        /// - `type`: Filter by movement type (1=Entry, 2=Exit, 3=Adjustment, 4=Transfer)
        /// - `startDate`: Filter movements from this date
        /// - `endDate`: Filter movements until this date
        /// - `userId`: Filter by user who created the movement
        /// - `lote`: Filter by batch number
        /// - `reference`: Filter by reference number
        /// 
        /// **Example Request:**
        /// ```
        /// GET /api/v1/inventory/movements?page=1&pageSize=20&type=1&startDate=2024-01-01
        /// ```
        /// </remarks>
        /// <response code="200">Stock movements retrieved successfully</response>
        /// <response code="400">Invalid filter parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("movements")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<PagedStockMovementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PagedStockMovementResponse>>> GetStockMovements(
            [FromQuery] StockMovementFilterRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetStockMovements", GetCurrentUserId(), request);

                // Validate model
                _validationService.ValidateModel(request);

                // Map to existing command (assuming it exists)
                var command = new GetStockMovementsRequest
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    ProdutoId = request.ProdutoId,
                    Type = request.Type,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    UserId = request.UserId,
                    Lote = request.Lote,
                    Reference = request.Reference
                };
                
                var result = await _mediator.Send(command, cancellationToken);
                
                return Ok(ApiResponse<GetStockMovementsResponse>.Ok(result, "Movimentações obtidas com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock movements");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get stock alerts for products with low stock, out of stock, or expiring batches
        /// </summary>
        /// <param name="alertType">Type of alerts to retrieve (optional)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of stock alerts</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves stock alerts based on configured thresholds and business rules.
        /// 
        /// **Alert Types:**
        /// - `OutOfStock`: Products with zero available stock
        /// - `LowStock`: Products below reorder point
        /// - `ExpiringBatch`: Batches expiring within 30 days
        /// - `ExpiredBatch`: Batches that have already expired
        /// 
        /// **Example Request:**
        /// ```
        /// GET /api/v1/inventory/alerts?alertType=LowStock
        /// ```
        /// </remarks>
        /// <response code="200">Stock alerts retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("alerts")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StockAlertResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockAlertResponse>>>> GetStockAlerts(
            [FromQuery] StockAlertType? alertType,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetStockAlerts", GetCurrentUserId(), new { alertType });
                
                var request = new GetStockAlertsRequest { AlertType = alertType };
                var result = await _mediator.Send(request, cancellationToken);
                
                return Ok(ApiResponse<GetStockAlertsResponse>.Ok(result, "Alertas obtidos com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock alerts");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Validate stock availability for a single product
        /// </summary>
        /// <param name="request">Stock validation request</param>
        /// <param name="stockValidationService">Stock validation service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Stock validation result</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Validates if sufficient stock is available for a specific product and quantity.
        /// Useful for pre-validating sales orders or reservations.
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///   "requestedQuantity": 10.0,
        ///   "lote": "LOT001"
        /// }
        /// ```
        /// 
        /// **Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "data": {
        ///     "isValid": true,
        ///     "message": "Stock available",
        ///     "items": [
        ///       {
        ///         "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///         "productName": "Product Name",
        ///         "requestedQuantity": 10.0,
        ///         "availableQuantity": 25.0,
        ///         "isAvailable": true,
        ///         "shortageQuantity": 0
        ///       }
        ///     ]
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Stock validation completed successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("validate-stock")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<StockValidationResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StockValidationResult>>> ValidateStock(
            [FromBody, Required] StockValidationRequest request,
            [FromServices] IStockValidationService stockValidationService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("ValidateStock", GetCurrentUserId(), request);

                // Validate model
                _validationService.ValidateModel(request);
                
                var result = await stockValidationService.ValidateStockAvailabilityAsync(
                    request.ProductId, 
                    request.RequestedQuantity, 
                    cancellationToken);
                
                return Ok(ApiResponse<StockValidationResult>.Ok(result, "Validação realizada com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stock for product {ProductId}", request.ProductId);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Validate stock availability for multiple products in batch
        /// </summary>
        /// <param name="requests">List of stock validation requests</param>
        /// <param name="stockValidationService">Stock validation service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Batch stock validation result</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Validates stock availability for multiple products in a single request.
        /// Useful for validating entire shopping carts or sales orders.
        /// 
        /// **Request Body:**
        /// ```json
        /// [
        ///   {
        ///     "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///     "requestedQuantity": 10.0,
        ///     "lote": "LOT001"
        ///   },
        ///   {
        ///     "productId": "456e7890-f12b-34c5-d678-901234567890",
        ///     "requestedQuantity": 5.0
        ///   }
        /// ]
        /// ```
        /// 
        /// **Response:**
        /// Returns validation results for all products, indicating which ones have sufficient stock.
        /// </remarks>
        /// <response code="200">Batch stock validation completed successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("validate-stock-batch")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<StockValidationResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StockValidationResult>>> ValidateStockBatch(
            [FromBody, Required] IEnumerable<StockValidationRequest> requests,
            [FromServices] IStockValidationService stockValidationService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("ValidateStockBatch", GetCurrentUserId(), requests);

                // Validate each request in the batch
                foreach (var request in requests)
                {
                    _validationService.ValidateModel(request);
                }
                
                var result = await stockValidationService.ValidateStockAvailabilityAsync(requests, cancellationToken);
                
                return Ok(ApiResponse<StockValidationResult>.Ok(result, "Validação em lote realizada com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stock in batch");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }



        /// <summary>
        /// Get current inventory balance for a product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="inventoryBalanceService">Inventory balance service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current inventory balance information</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves the current inventory balance for a specific product including:
        /// - Current stock quantity
        /// - Reserved stock
        /// - Available stock (current - reserved)
        /// - Minimum/maximum stock levels
        /// - Reorder point
        /// - Location information
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "data": {
        ///     "produtoId": "123e4567-e89b-12d3-a456-426614174000",
        ///     "produtoNome": "Product Name",
        ///     "currentStock": 100.0,
        ///     "reservedStock": 10.0,
        ///     "availableStock": 90.0,
        ///     "minimumStock": 20.0,
        ///     "reorderPoint": 30.0,
        ///     "isLowStock": false,
        ///     "isOutOfStock": false
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Inventory balance retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("balance/{productId:guid}")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<InventoryBalanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<InventoryBalanceResponse>>> GetBalance(
            [FromRoute] Guid productId,
            [FromServices] IInventoryBalanceService inventoryBalanceService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetBalance", GetCurrentUserId(), new { productId });
                
                var balance = await inventoryBalanceService.CalculateCurrentBalanceAsync(productId, cancellationToken);
                
                if (balance == null)
                {
                    return NotFound(ApiResponse.Error("Produto não encontrado", correlationId: CorrelationId));
                }
                
                // Map to response DTO
                var response = new InventoryBalanceResponse
                {
                    ProdutoId = balance.ProdutoId,
                    ProdutoNome = balance.Produto?.NomeProduto,
                    CurrentStock = balance.CurrentStock,
                    ReservedStock = balance.ReservedStock,
                    MinimumStock = balance.MinimumStock,
                    MaximumStock = balance.MaximumStock,
                    ReorderPoint = balance.ReorderPoint,
                    Location = balance.Location,
                    LastUpdated = balance.LastUpdated
                };
                
                return Ok(ApiResponse<InventoryBalanceResponse>.Ok(response, "Saldo obtido com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting balance for product {ProductId}", productId);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get inventory balance with batch details for perishable products
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="inventoryBalanceService">Inventory balance service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Inventory balance with detailed batch information</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves inventory balance with detailed batch information for perishable products.
        /// Includes batch numbers, expiry dates, and quantities per batch.
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "data": {
        ///     "produtoId": "123e4567-e89b-12d3-a456-426614174000",
        ///     "currentStock": 100.0,
        ///     "availableStock": 90.0,
        ///     "batches": [
        ///       {
        ///         "lote": "LOT001",
        ///         "dataValidade": "2024-12-31T00:00:00Z",
        ///         "availableQuantity": 50.0,
        ///         "isExpired": false,
        ///         "isExpiringSoon": false
        ///       }
        ///     ]
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Inventory balance with batches retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("balance/{productId:guid}/batches")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<InventoryBalanceWithBatches>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<InventoryBalanceWithBatches>>> GetBalanceWithBatches(
            [FromRoute] Guid productId,
            [FromServices] IInventoryBalanceService inventoryBalanceService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetBalanceWithBatches", GetCurrentUserId(), new { productId });
                
                var balanceWithBatches = await inventoryBalanceService.GetBalanceWithBatchesAsync(productId, cancellationToken);
                
                if (balanceWithBatches == null)
                {
                    return NotFound(ApiResponse.Error("Produto não encontrado", correlationId: CorrelationId));
                }
                
                return Ok(ApiResponse<InventoryBalanceWithBatches>.Ok(balanceWithBatches, "Saldo com lotes obtido com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting balance with batches for product {ProductId}", productId);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get expired batches for cleanup and disposal
        /// </summary>
        /// <param name="inventoryBalanceService">Inventory balance service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of expired batches that need attention</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves all batches that have passed their expiry date and need to be disposed of or written off.
        /// Useful for inventory cleanup and compliance with food safety regulations.
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "data": [
        ///     {
        ///       "produtoId": "123e4567-e89b-12d3-a456-426614174000",
        ///       "produtoNome": "Product Name",
        ///       "codBarras": "1234567890123",
        ///       "lote": "LOT001",
        ///       "dataValidade": "2024-01-01T00:00:00Z",
        ///       "quantity": 10.0,
        ///       "daysExpired": 15
        ///     }
        ///   ]
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Expired batches retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("batches/expired")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExpiredBatch>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpiredBatch>>>> GetExpiredBatches(
            [FromServices] IInventoryBalanceService inventoryBalanceService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("GetExpiredBatches", GetCurrentUserId(), null);
                
                var expiredBatches = await inventoryBalanceService.GetExpiredBatchesAsync(cancellationToken);
                
                return Ok(ApiResponse<IEnumerable<ExpiredBatch>>.Ok(expiredBatches, "Lotes vencidos obtidos com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expired batches");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get batches expiring within specified days threshold
        /// </summary>
        /// <param name="inventoryBalanceService">Inventory balance service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="daysThreshold">Number of days threshold for expiry warning (default: 30)</param>
        /// <returns>List of batches expiring within the specified threshold</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Retrieves batches that will expire within the specified number of days.
        /// Useful for proactive inventory management and sales prioritization.
        /// 
        /// **Query Parameters:**
        /// - `daysThreshold`: Number of days to look ahead for expiring batches (default: 30)
        /// 
        /// **Example Request:**
        /// ```
        /// GET /api/v1/inventory/batches/expiring?daysThreshold=7
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "data": [
        ///     {
        ///       "produtoId": "123e4567-e89b-12d3-a456-426614174000",
        ///       "produtoNome": "Product Name",
        ///       "lote": "LOT001",
        ///       "dataValidade": "2024-02-15T00:00:00Z",
        ///       "quantity": 25.0,
        ///       "daysToExpiry": 5
        ///     }
        ///   ]
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Expiring batches retrieved successfully</response>
        /// <response code="400">Invalid days threshold parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("batches/expiring")]
        [Authorize(Policy = "InventoryView")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExpiringBatch>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpiringBatch>>>> GetExpiringBatches(
            [FromServices] IInventoryBalanceService inventoryBalanceService,
            CancellationToken cancellationToken,
            [FromQuery, Range(1, 365, ErrorMessage = "Days threshold must be between 1 and 365")] int daysThreshold = 30)
        {
            try
            {
                _appLogger.LogUserAction("GetExpiringBatches", GetCurrentUserId(), new { daysThreshold });
                
                var expiringBatches = await inventoryBalanceService.GetExpiringBatchesAsync(daysThreshold, cancellationToken);
                
                return Ok(ApiResponse<IEnumerable<ExpiringBatch>>.Ok(expiringBatches, "Lotes próximos ao vencimento obtidos com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring batches with threshold {DaysThreshold}", daysThreshold);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Recalculate inventory balance for a product or all products
        /// </summary>
        /// <param name="request">Recalculation request parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Recalculation result with statistics</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Recalculates inventory balances based on stock movements. This is useful for:
        /// - Fixing data inconsistencies
        /// - Periodic balance verification
        /// - After data migration or bulk operations
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "productId": "123e4567-e89b-12d3-a456-426614174000",
        ///   "forceRecalculation": true
        /// }
        /// ```
        /// 
        /// **Note:** If `productId` is null, all products will be recalculated.
        /// </remarks>
        /// <response code="200">Recalculation completed successfully</response>
        /// <response code="400">Invalid request parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("recalculate")]
        [Authorize(Policy = "InventoryManagement")]
        [ProducesResponseType(typeof(ApiResponse<RecalculateBalanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RecalculateBalanceResponse>>> RecalculateBalance(
            [FromBody, Required] RecalculateBalanceRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("RecalculateBalance", GetCurrentUserId(), new { request.ProductId, request.ForceRecalculation });

                // Validate model
                _validationService.ValidateModel(request);
                
                var result = await _mediator.Send(request, cancellationToken);
                
                if (result.Success)
                {
                    return Ok(ApiResponse<RecalculateBalanceResponse>.Ok(result, "Recálculo concluído com sucesso", CorrelationId));
                }
                
                return BadRequest(ApiResponse.Error(result.Message, correlationId: CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recalculating balances");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Materialize inventory balances for performance optimization
        /// </summary>
        /// <param name="inventoryBalanceService">Inventory balance service</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Materialization completion confirmation</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Materializes inventory balances by pre-calculating and storing current stock levels.
        /// This improves query performance for frequently accessed balance information.
        /// 
        /// **When to Use:**
        /// - After bulk stock movements
        /// - During off-peak hours for performance optimization
        /// - After system maintenance or data migration
        /// 
        /// **Note:** This is a resource-intensive operation and should be used judiciously.
        /// </remarks>
        /// <response code="200">Materialization completed successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("materialize")]
        [Authorize(Policy = "InventoryManagement")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> MaterializeBalances(
            [FromServices] IInventoryBalanceService inventoryBalanceService,
            CancellationToken cancellationToken)
        {
            try
            {
                _appLogger.LogUserAction("MaterializeBalances", GetCurrentUserId(), null);
                
                await inventoryBalanceService.MaterializeAllBalancesAsync(cancellationToken);
                
                return Ok(ApiResponse<string>.Ok("Materialização concluída", "Saldos materializados com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error materializing balances");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }
    }

    /// <summary>
    /// Request model for single product stock validation
    /// </summary>
    public class ValidateStockRequest
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        [NotEmptyGuid]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "A quantidade solicitada é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public decimal RequestedQuantity { get; set; }
    }
}