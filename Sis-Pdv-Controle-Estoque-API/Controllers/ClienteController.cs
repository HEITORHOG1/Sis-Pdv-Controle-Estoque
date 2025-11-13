using Commands.Cliente.ListarClientesPaginado;
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
    /// Customer management endpoints for the PDV Control System
    /// </summary>
    /// <remarks>
    /// This controller provides comprehensive customer management functionality including:
    /// 
    /// **Customer Operations:**
    /// - Customer registration and profile management
    /// - Customer listing with pagination and search
    /// - Customer update and status management
    /// - Customer purchase history and analytics
    /// 
    /// **Features:**
    /// - CPF/CNPJ validation for Brazilian customers
    /// - Address management with CEP lookup
    /// - Contact information management
    /// - Customer loyalty program integration
    /// - Purchase history tracking
    /// 
    /// **Business Rules:**
    /// - Unique CPF/CNPJ validation
    /// - Email format validation
    /// - Phone number format validation
    /// - Address validation requirements
    /// 
    /// **Security:**
    /// - Requires authentication for all operations
    /// - Customer data privacy protection
    /// - Audit logging for customer data changes
    /// - LGPD compliance for data handling
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cliente")]
    [Produces("application/json")]
    [Tags("Customers")]
    [Authorize]
    public class ClienteController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieve paginated list of customers with optional filtering
        /// </summary>
        /// <param name="request">Pagination and filtering parameters</param>
        /// <returns>Paginated list of customers with metadata</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint provides a paginated list of customers with optional filtering capabilities.
        /// Supports searching by name, CPF/CNPJ, email, and phone number.
        /// 
        /// **Query Parameters:**
        /// - page: Page number (1-based, default: 1)
        /// - pageSize: Items per page (max: 100, default: 20)
        /// - search: Search term for customer name, CPF/CNPJ, or email
        /// - isActive: Filter by active status
        /// - customerType: Filter by customer type (Individual/Corporate)
        /// 
        /// **Example Request:**
        /// ```
        /// GET /api/v1/cliente/paginated?page=1&amp;pageSize=20&amp;search=João&amp;isActive=true
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Clientes listados com sucesso",
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "id": "123e4567-e89b-12d3-a456-426614174000",
        ///         "nome": "João Silva",
        ///         "email": "joao@email.com",
        ///         "telefone": "(11) 99999-9999",
        ///         "cpf": "123.456.789-00",
        ///         "endereco": "Rua das Flores, 123",
        ///         "isActive": true,
        ///         "totalPurchases": 15,
        ///         "lastPurchaseDate": "2024-01-15T10:30:00Z"
        ///       }
        ///     ],
        ///     "totalCount": 250,
        ///     "pageNumber": 1,
        ///     "pageSize": 20,
        ///     "totalPages": 13
        ///   }
        /// }
        /// ```
        /// 
        /// **Privacy Notes:**
        /// - Sensitive data is masked according to LGPD requirements
        /// - Full customer details require appropriate permissions
        /// - Customer consent is tracked for data usage
        /// </remarks>
        /// <response code="200">Customers retrieved successfully</response>
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
        public async Task<IActionResult> ListarClientesPaginado([FromQuery, Required] ListarClientesPaginadoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }
    }
}