﻿using Commands.Produto.ListarProdutosPaginado;
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
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

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

        public ProdutoController(IMediator mediator, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mediator = mediator;
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
        /// GET /api/v1/produto/paginated?page=1&amp;pageSize=20&amp;search=notebook&amp;isActive=true
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
        ///         "preco": 2499.99,
        ///         "codigoBarras": "1234567890123",
        ///         "categoria": "Eletrônicos",
        ///         "fornecedor": "Dell Inc.",
        ///         "estoque": 15,
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
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(Models.ApiResponse<Models.PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarProdutosPaginado([FromQuery, Required] ListarProdutosPaginadoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        // Rotas versionadas equivalentes às rotas legadas
        [HttpPost("AdicionarProduto")]
        public async Task<IActionResult> AdicionarProduto([FromBody] AdicionarProdutoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        [HttpPut("AlterarProduto")]
        public async Task<IActionResult> AlterarProduto([FromBody] AlterarProdutoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        [HttpPut("AtualizaEstoque")]
        public async Task<IActionResult> AtualizaEstoque([FromBody] AtualizarEstoqueRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        [HttpDelete("RemoverProduto/{id:Guid}")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            var request = new RemoverProdutoResquest(id);
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        [HttpGet("ListarProduto")]
        public async Task<IActionResult> ListarProduto()
        {
            var request = new ListarProdutoRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("ListarProdutoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarProdutoPorId(Guid id)
        {
            var request = new ListarProdutoPorIdRequest(id);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("ListarProdutoPorCodBarras/{codBarras}")]
        public async Task<IActionResult> ListarProdutoPorCodBarras(string codBarras)
        {
            var request = new ListarProdutoPorCodBarrasRequest(codBarras);
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}