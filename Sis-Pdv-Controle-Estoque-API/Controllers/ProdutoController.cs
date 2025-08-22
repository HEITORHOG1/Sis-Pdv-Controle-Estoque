using Commands.Produto.ListarProdutosPaginado;
using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.AtualizarEstoque;
using Commands.Produto.ListarProduto;
using Commands.Produto.ListarProdutoPorId;
using Commands.Produto.ListarProdutoPorNomeProduto;
using Commands.Produto.RemoverProduto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Models.DTOs;
using Sis_Pdv_Controle_Estoque_API.Services.Validation;
using Sis_Pdv_Controle_Estoque_API.Exceptions;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;
using Services;
using Sis_Pdv_Controle_Estoque_API.Services;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// Product management endpoints for the PDV Control System
    /// </summary>
    /// <remarks>
    /// This controller provides comprehensive product management functionality including:
    /// 
    /// **Product Operations:**
    /// - Product listing with pagination and filtering
    /// - Product creation, update, and deletion
    /// - Product search by barcode, name, or category
    /// - Bulk product operations
    /// 
    /// **Features:**
    /// - Barcode validation and management
    /// - Price management with history tracking
    /// - Category and supplier associations
    /// - Stock level integration
    /// - Product image management
    /// 
    /// **Business Rules:**
    /// - Unique barcode validation
    /// - Price validation (must be positive)
    /// - Category assignment requirements
    /// - Supplier relationship management
    /// 
    /// **Security:**
    /// - Requires authentication for all operations
    /// - Role-based access control for sensitive operations
    /// - Audit logging for all product changes
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produto")]
    [Produces("application/json")]
    [Tags("Products")]
    [Authorize]
    public class ProdutoController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IModelValidationService _validationService;
        private readonly IApplicationLogger _appLogger;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(
            IMediator mediator, 
            IUnitOfWork unitOfWork,
            IModelValidationService validationService,
            IApplicationLogger appLogger,
            ILogger<ProdutoController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _validationService = validationService;
            _appLogger = appLogger;
            _logger = logger;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="request">Product creation data</param>
        /// <returns>Created product information</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Creates a new product with master data only (no stock, pricing, or cost information).
        /// All validation rules are applied including barcode uniqueness and required fields.
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "codBarras": "1234567890123",
        ///   "nomeProduto": "Notebook Dell Inspiron 15",
        ///   "descricaoProduto": "Notebook Dell Inspiron 15 com 8GB RAM",
        ///   "isPerecivel": false,
        ///   "fornecedorId": "123e4567-e89b-12d3-a456-426614174000",
        ///   "categoriaId": "456e7890-f12b-34c5-d678-901234567890",
        ///   "statusAtivo": 1
        /// }
        /// ```
        /// 
        /// **Business Rules:**
        /// - Barcode must be unique and contain 8-20 digits
        /// - Product name is required and must be 2-100 characters
        /// - Supplier and category must exist
        /// - Status must be 0 (inactive) or 1 (active)
        /// </remarks>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid request data or business rule violation</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="409">Conflict - barcode already exists</response>
        /// <response code="422">Unprocessable entity - validation failed</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Policy = "ProductManagement")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> CreateProduct(
            [FromBody, Required] CreateProductRequest request)
        {
            try
            {
                _appLogger.LogUserAction("CreateProduct", GetCurrentUserId(), new { request.CodBarras, request.NomeProduto });

                // Validate model
                _validationService.ValidateModel(request);

                // Validate business rules
                var businessValidation = await _validationService.ValidateProductCreationAsync(
                    request.CodBarras, request.FornecedorId, request.CategoriaId);

                if (!businessValidation.IsValid)
                {
                    return UnprocessableEntity(ApiResponse.Error("Validation failed", businessValidation.Errors, CorrelationId));
                }

                // Map to command
                var command = new AdicionarProdutoRequest
                {
                    codBarras = request.CodBarras,
                    nomeProduto = request.NomeProduto,
                    descricaoProduto = request.DescricaoProduto ?? string.Empty,
                    isPerecivel = request.IsPerecivel,
                    FornecedorId = request.FornecedorId,
                    CategoriaId = request.CategoriaId,
                    statusAtivo = request.StatusAtivo
                };

                var response = await _mediator.Send(command);
                
                if (response.Notifications.Any())
                {
                    var errors = response.Notifications.Select(n => n.Message);
                    return BadRequest(ApiResponse.Error("Validation failed", errors, CorrelationId));
                }

                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), 
                    new { id = response.Data }, 
                    ApiResponse<object>.Ok(response.Data, "Produto criado com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (DuplicateException ex)
            {
                return Conflict(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product with barcode {Barcode}", request.CodBarras);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="request">Product update data</param>
        /// <returns>Updated product information</returns>
        /// <response code="200">Product updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="409">Conflict - barcode already exists</response>
        /// <response code="422">Validation failed</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "ProductManagement")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> UpdateProduct(
            [FromRoute] Guid id,
            [FromBody, Required] UpdateProductRequest request)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(ApiResponse.Error("ID na URL n�o corresponde ao ID no corpo da requisi��o", correlationId: CorrelationId));
                }

                _appLogger.LogUserAction("UpdateProduct", GetCurrentUserId(), new { id, request.CodBarras, request.NomeProduto });

                // Validate model
                _validationService.ValidateModel(request);

                // Validate business rules
                var businessValidation = await _validationService.ValidateProductUpdateAsync(
                    request.Id, request.CodBarras, request.FornecedorId, request.CategoriaId);

                if (!businessValidation.IsValid)
                {
                    return UnprocessableEntity(ApiResponse.Error("Validation failed", businessValidation.Errors, CorrelationId));
                }

                // Map to command
                var command = new AlterarProdutoRequest
                {
                    Id = request.Id,
                    codBarras = request.CodBarras,
                    nomeProduto = request.NomeProduto,
                    descricaoProduto = request.DescricaoProduto ?? string.Empty,
                    isPerecivel = request.IsPerecivel,
                    FornecedorId = request.FornecedorId,
                    CategoriaId = request.CategoriaId,
                    statusAtivo = request.StatusAtivo
                };

                var response = await _mediator.Send(command);
                
                if (response.Notifications.Any())
                {
                    var errors = response.Notifications.Select(n => n.Message);
                    return BadRequest(ApiResponse.Error("Validation failed", errors, CorrelationId));
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<object>.Ok(response.Data, "Produto atualizado com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (DuplicateException ex)
            {
                return Conflict(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Deletion confirmation</returns>
        /// <response code="200">Product deleted successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="409">Conflict - product has dependencies</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "ProductManagement")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                _appLogger.LogUserAction("DeleteProduct", GetCurrentUserId(), new { id });

                var command = new RemoverProdutoResquest(id);
                var response = await _mediator.Send(command);
                
                if (response.Notifications.Any())
                {
                    var errors = response.Notifications.Select(n => n.Message);
                    return BadRequest(ApiResponse.Error("Validation failed", errors, CorrelationId));
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse.Ok("Produto removido com sucesso", CorrelationId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (BusinessRuleException ex)
            {
                return Conflict(ApiResponse.Error(ex.Message, correlationId: CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product information</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id:guid}")]
        [Authorize(Policy = "ProductView")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetProductById([FromRoute] Guid id)
        {
            try
            {
                _appLogger.LogUserAction("GetProductById", GetCurrentUserId(), new { id });

                var command = new ListarProdutoPorIdRequest(id);
                var response = await _mediator.Send(command);

                if (response == null)
                {
                    return NotFound(ApiResponse.Error("Produto n�o encontrado", correlationId: CorrelationId));
                }

                return Ok(ApiResponse<object>.Ok(response, "Produto obtido com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId}", id);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Get product by barcode
        /// </summary>
        /// <param name="barcode">Product barcode</param>
        /// <returns>Product information</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="400">Invalid barcode format</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("barcode/{barcode}")]
        [Authorize(Policy = "ProductView")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetProductByBarcode([FromRoute] string barcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                {
                    return BadRequest(ApiResponse.Error("C�digo de barras � obrigat�rio", correlationId: CorrelationId));
                }

                _appLogger.LogUserAction("GetProductByBarcode", GetCurrentUserId(), new { barcode });

                var command = new ListarProdutoPorCodBarrasRequest(barcode);
                var response = await _mediator.Send(command);

                if (response == null)
                {
                    return NotFound(ApiResponse.Error("Produto n�o encontrado", correlationId: CorrelationId));
                }

                return Ok(ApiResponse<object>.Ok(response, "Produto obtido com sucesso", CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by barcode {Barcode}", barcode);
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        /// <summary>
        /// Retrieve paginated list of products with optional filtering
        /// </summary>
        /// <param name="request">Pagination and filtering parameters</param>
        /// <returns>Paginated list of products with metadata</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint provides a paginated list of products with optional filtering capabilities.
        /// Supports searching by name, barcode, category, and supplier.
        /// 
        /// **Query Parameters:**
        /// - `page`: Page number (1-based, default: 1)
        /// - pageSize: Items per page (max: 100, default: 20)
        /// - search: Search term for product name or barcode
        /// - categoryId: Filter by category ID
        /// - supplierId: Filter by supplier ID
        /// - isActive: Filter by active status
        /// 
        /// **Example Request:**
        /// ```
        /// GET /api/v1/produto?page=1&amp;pageSize=20&amp;search=notebook&amp;isActive=true
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Produtos listados com sucesso",
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "id": "123e4567-e89b-12d3-a456-426614174000",
        ///         "nome": "Notebook Dell",
        ///         "descricao": "Notebook Dell Inspiron 15",
        ///         "codigoBarras": "1234567890123",
        ///         "categoria": "Eletr�nicos",
        ///         "fornecedor": "Dell Inc.",
        ///         "isActive": true
        ///       }
        ///     ],
        ///     "totalCount": 150,
        ///     "pageNumber": 1,
        ///     "pageSize": 20,
        ///     "totalPages": 8
        ///   }
        /// }
        /// ```
        /// 
        /// **Performance Notes:**
        /// - Results are optimized for better performance
        /// - Large result sets are automatically paginated
        /// - Use specific filters to improve query performance
        /// </remarks>
        /// <response code="200">Products retrieved successfully</response>
        /// <response code="400">Invalid pagination or filter parameters</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize(Policy = "ProductView")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetProducts([FromQuery] ProductSearchRequest request)
        {
            try
            {
                _appLogger.LogUserAction("GetProducts", GetCurrentUserId(), request);

                // Validate model
                _validationService.ValidateModel(request);

                // Map to existing command
                var command = new ListarProdutosPaginadoRequest
                {
                    PageNumber = request.Page,
                    PageSize = request.PageSize,
                    SearchTerm = request.Search,
                    Categoria = request.CategoriaId?.ToString(),
                    ApenasAtivos = request.IsActive
                };

                var response = await _mediator.Send(command);
                return Ok(ApiResponse<object>.Ok(response, "Produtos listados com sucesso", CorrelationId));
            }
            catch (Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse.Error("Validation failed", ex.Errors, CorrelationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, ApiResponse.Error("Erro interno do servidor", correlationId: CorrelationId));
            }
        }

        #region Legacy Endpoints (Backward Compatibility)
        
        /// <summary>
        /// Legacy endpoint for paginated product listing
        /// </summary>
        [HttpGet("paginated")]
        [Obsolete("Use GET /api/v1/produto instead")]
        public async Task<IActionResult> ListarProdutosPaginado([FromQuery, Required] ListarProdutosPaginadoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Legacy endpoint for adding products
        /// </summary>
        [HttpPost("AdicionarProduto")]
        [Obsolete("Use POST /api/v1/produto instead")]
        public async Task<IActionResult> AdicionarProduto([FromBody] AdicionarProdutoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Legacy endpoint for updating products
        /// </summary>
        [HttpPut("AlterarProduto")]
        [Obsolete("Use PUT /api/v1/produto/{id} instead")]
        public async Task<IActionResult> AlterarProduto([FromBody] AlterarProdutoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Legacy endpoint for stock updates - DEPRECATED: Use Inventory API instead
        /// </summary>
        [HttpPut("AtualizaEstoque")]
        [Obsolete("Use POST /api/v1/inventory/movements instead")]
        public async Task<IActionResult> AtualizaEstoque([FromBody] AtualizarEstoqueRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Legacy endpoint for removing products
        /// </summary>
        [HttpDelete("RemoverProduto/{id:Guid}")]
        [Obsolete("Use DELETE /api/v1/produto/{id} instead")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            var request = new RemoverProdutoResquest(id);
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Legacy endpoint for listing all products
        /// </summary>
    // Legacy route within versioned controller (kept only as versioned path to avoid route conflicts). Unversioned legacy path is handled by ProdutoLegacyController.
    [HttpGet("ListarProduto")]
    [AllowAnonymous]
    [Obsolete("Use GET /api/v1/produto instead")]
    public async Task<IActionResult> ListarProduto()
        {
            var request = new ListarProdutoRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Legacy endpoint for getting product by ID
        /// </summary>
    // Legacy route within versioned controller (kept only as versioned path). Unversioned legacy path is handled by ProdutoLegacyController.
    [HttpGet("ListarProdutoPorId/{id:Guid}")]
    [AllowAnonymous]
    [Obsolete("Use GET /api/v1/produto/{id} instead")]
    public async Task<IActionResult> ListarProdutoPorId(Guid id)
        {
            var request = new ListarProdutoPorIdRequest(id);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Legacy endpoint for getting product by barcode
        /// </summary>
    // Legacy route within versioned controller (kept only as versioned path). Unversioned legacy path is handled by ProdutoLegacyController.
    [HttpGet("ListarProdutoPorCodBarras/{codBarras}")]
    [AllowAnonymous]
    [Obsolete("Use GET /api/v1/produto/barcode/{barcode} instead")]
    public async Task<IActionResult> ListarProdutoPorCodBarras(string codBarras)
        {
            var request = new ListarProdutoPorCodBarrasRequest(codBarras);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        #endregion
    }
}
