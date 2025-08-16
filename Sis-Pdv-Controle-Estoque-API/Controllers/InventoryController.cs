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

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
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
        /// Ajusta o estoque de um produto
        /// </summary>
        [HttpPost("adjust-stock")]
        [Authorize(Policy = "InventoryManagement")]
        public async Task<ActionResult<ApiResponse<AdjustStockResponse>>> AdjustStock(
            [FromBody] AdjustStockRequest request,
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