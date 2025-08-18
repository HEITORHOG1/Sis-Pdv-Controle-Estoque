using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.AlterarCategoria;
using Commands.Categoria.ListarCategoria;
using Commands.Categoria.ListarCategoriaPorId;
using Commands.Categoria.ListarCategoriaPorNomeCategoria;
using Commands.Categoria.RemoverCategoria;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// Category management endpoints for the PDV Control System
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categoria")]
    [Produces("application/json")]
    [Tags("Categories")]
    [Authorize]
    public class CategoriaController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriaController> _logger;
        
        public CategoriaController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<CategoriaController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Categoria
        /// <summary>
        /// Create a new product category
        /// </summary>
        /// <param name="request">Category creation details including name, description, and parent category</param>
        /// <returns>Created category information with assigned ID</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint creates a new product category in the system.
        /// Categories help organize products and enable better inventory management.
        /// 
        /// **Request Body:**
        /// ```json
        /// {
        ///   "nome": "Eletrônicos",
        ///   "descricao": "Produtos eletrônicos e tecnologia",
        ///   "parentCategoryId": null,
        ///   "isActive": true,
        ///   "sortOrder": 1
        /// }
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Categoria criada com sucesso",
        ///   "data": {
        ///     "id": "123e4567-e89b-12d3-a456-426614174000",
        ///     "nome": "Eletrônicos",
        ///     "descricao": "Produtos eletrônicos e tecnologia",
        ///     "parentCategoryId": null,
        ///     "isActive": true,
        ///     "sortOrder": 1,
        ///     "productCount": 0,
        ///     "createdAt": "2024-01-15T10:30:00Z",
        ///     "createdBy": "admin@pdvsystem.com"
        ///   }
        /// }
        /// ```
        /// 
        /// **Business Rules:**
        /// - Category name must be unique within the same parent level
        /// - Description is optional but recommended
        /// - Parent category must exist if specified
        /// - Sort order determines display sequence
        /// - Maximum hierarchy depth is 5 levels
        /// 
        /// **Validation Rules:**
        /// - Name: Required, 2-100 characters
        /// - Description: Optional, max 500 characters
        /// - Parent category must be active if specified
        /// </remarks>
        /// <response code="201">Category created successfully</response>
        /// <response code="400">Invalid request data or validation errors</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="409">Conflict - category name already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("/api/Categoria/AdicionarCategoria")]
        public async Task<IActionResult> AdicionarCategoria([FromBody] AdicionarCategoriaRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarCategoria");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarCategoria - Response: {@response}", response);

                return await ResponseAsync(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("AdicionarCategoria - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todas as categorias presentes no sistema.
        /// </summary>
        /// <returns>Retorna uma lista de todas as categorias.</returns>
        [HttpGet]
        [Route("/api/Categoria/ListarCategoria")]
        public async Task<IActionResult> ListarCategoria()
        {
            _logger.LogInformation("ListarCategoria operation started. CorrelationId: {CorrelationId}", CorrelationId);
            
            var request = new ListarCategoriaRequest();
            var result = await _mediator.Send(request, CancellationToken.None);
            
            _logger.LogInformation("ListarCategoria operation completed successfully. CorrelationId: {CorrelationId}", CorrelationId);
            
            return Ok(result);
        }

        /// <summary>
        /// Recupera uma categoria específica por seu identificador.
        /// </summary>
        /// <param name="id">O identificador da categoria que se deseja recuperar.</param>
        /// <returns>Retorna a categoria que corresponde ao identificador fornecido.</returns>

        [HttpGet]
        [Route("/api/Categoria/ListarCategoriaPorId/{id:Guid}")]
        public async Task<IActionResult> ListarCategoriaPorId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarCategoriaPorId");
                var request = new ListarCategoriaPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarCategoriaPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("ListarCategoriaPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Recupera uma categoria específica por seu nome.
        /// </summary>
        /// <param name="NomeCategoria">O nome da categoria que se deseja recuperar.</param>
        /// <returns>Retorna a categoria que corresponde ao nome fornecido.</returns>

        [HttpGet]
        [Route("/api/Categoria/ListarCategoriaPorNomeCategoria/{NomeCategoria}")]
        public async Task<IActionResult> ListarCategoriaPorNomeCategoria(string NomeCategoria)
        {

            try
            {
                _logger.LogInformation("ListarCategoriaPorNomeCategoria");
                var request = new ListarCategoriaPorNomeCategoriaRequest(NomeCategoria);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarCategoriaPorNomeCategoria - Response: {@response}", result);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("ListarCategoriaPorNomeCategoria - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza as informações de uma categoria específica.
        /// </summary>
        /// <param name="request">O objeto contendo as novas informações da categoria.</param>
        /// <returns>Retorna a resposta da solicitação de atualização.</returns>

        [HttpPut]
        [Route("/api/Categoria/AlterarCategoria")]
        public async Task<IActionResult> AlterarCategoria([FromBody] AlterarCategoriaRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarCategoria");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarCategoria - Response: {@response}", response);
                
                return await ResponseAsync(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("AlterarCategoria - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma categoria específica do sistema.
        /// </summary>
        /// <param name="id">O identificador da categoria que se deseja remover.</param>
        /// <returns>Retorna a resposta da solicitação de remoção.</returns>
        [HttpDelete]
        [Route("/api/Categoria/RemoverCategoria/{id:Guid}")]
        public async Task<IActionResult> RemoverCategoria(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverCategoria");
                var request = new RemoverCategoriaResquest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverCategoria - Response: {@response}", result);
                
                return await ResponseAsync(result);

            }
            catch (System.Exception ex)
            {
                _logger.LogError("RemoverCategoria - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
