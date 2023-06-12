using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.AlterarCategoria;
using Commands.Categoria.ListarCategoria;
using Commands.Categoria.ListarCategoria.ListarCategoriaPorNomeCategoria;
using Commands.Categoria.ListarCategoriaPorId;
using Commands.Categoria.RemoverCategoria;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{

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
        /// Adiciona uma nova categoria ao sistema.
        /// </summary>
        /// <param name="request">O objeto contendo as informações da nova categoria.</param>
        /// <returns>Retorna a resposta da solicitação de adição.</returns>
        [HttpPost]
        [Route("api/Categoria/AdicionarCategoria")]
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
        [Route("api/Categoria/ListarCategoria")]
        public async Task<IActionResult> ListarCategoria()
        {

            try
            {
                _logger.LogInformation("ListarCategoria");
                var request = new ListarCategoriaRequest();
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarCategoria - Response: {@response}", result);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("ListarCategoria - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Recupera uma categoria específica por seu identificador.
        /// </summary>
        /// <param name="id">O identificador da categoria que se deseja recuperar.</param>
        /// <returns>Retorna a categoria que corresponde ao identificador fornecido.</returns>

        [HttpGet]
        [Route("api/Categoria/ListarCategoriaPorId/{id:Guid}")]
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
        [Route("api/Categoria/ListarCategoriaPorNomeCategoria/{NomeCategoria}")]
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
        [Route("api/Categoria/AlterarCategoria")]
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
        [Route("api/Categoria/RemoverCategoria/{id:Guid}")]
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
