using Commands.Departamento.AdicionarDepartamento;
using Commands.Departamento.AlterarDepartamento;
using Commands.Departamento.ListarDepartamento;
using Commands.Departamento.ListarDepartamentoPorId;
using Commands.Departamento.ListarDepartamentoPorNomeDepartamento;
using Commands.Departamento.RemoverDepartamento;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Asp.Versioning;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class DepartamentoController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DepartamentoController> _logger;
        public DepartamentoController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<DepartamentoController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Departamento
        /// <summary>
        /// Adiciona um novo departamento.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes do novo departamento.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPost]
        [Route("api/Departamento/AdicionarDepartamento")]
        public async Task<IActionResult> AdicionarDepartamento([FromBody] AdicionarDepartamentoRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarDepartamento");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarDepartamento - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarDepartamento - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista todos os departamentos.
        /// </summary>
        /// <returns>Retorna uma ação com uma lista de todos os departamentos.</returns>
        [HttpGet]
        [Route("api/Departamento/ListarDepartamento")]
        public async Task<IActionResult> ListarDepartamento()
        {

            try
            {
                _logger.LogInformation("ListarDepartamento");
                var request = new ListarDepartamentoRequest();
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarDepartamento - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarDepartamento - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um departamento específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do departamento a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do departamento solicitado.</returns>
        [HttpGet]
        [Route("api/Departamento/ListarDepartamentoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarDepartamentoPorId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarDepartamentoPorId");
                var request = new ListarDepartamentoPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarDepartamentoPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("ListarDepartamentoPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um departamento específico com base no nome do departamento fornecido.
        /// </summary>
        /// <param name="NomeDepartamento">O nome do departamento a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do departamento solicitado.</returns>
        [HttpGet]
        [Route("api/Departamento/ListarDepartamentoPorNomeDepartamento/{NomeDepartamento}")]
        public async Task<IActionResult> ListarDepartamentoPorNomeDepartamento(string NomeDepartamento)
        {

            try
            {
                _logger.LogInformation("ListarDepartamentoPorNomeDepartamento");
                var request = new ListarDepartamentoPorNomeDepartamentoRequest(NomeDepartamento);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarDepartamentoPorNomeDepartamento - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarDepartamentoPorNomeDepartamento - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Altera os detalhes de um departamento existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes atualizados do departamento.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/Departamento/AlterarDepartamento")]
        public async Task<IActionResult> AlterarDepartamento([FromBody] AlterarDepartamentoRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarDepartamento");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarDepartamento - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarDepartamento - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Remove um departamento existente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do departamento a ser removido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpDelete]
        [Route("api/Departamento/RemoverDepartamento/{id:Guid}")]
        public async Task<IActionResult> RemoverDepartamento(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverDepartamento");
                var request = new RemoverDepartamentoResquest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverDepartamento - Response: {@response}", result);
                return await ResponseAsync(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverDepartamento - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
