using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.AdicionarColaborador;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.AlterarColaborador;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaborador;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorId;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorNomeColaborador;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.RemoverColaborador;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.ValidarColaboradorLogin;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class ColaboradorController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ColaboradorController> _logger;
        public ColaboradorController(IMediator mediator, IUnitOfWork unitOfWork , ILogger<ColaboradorController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Colaborador
        /// <summary>
        /// Valida o login do colaborador.
        /// </summary>
        /// <param name="Login">O login do colaborador.</param>
        /// <param name="Senha">A senha do colaborador.</param>
        /// <returns>Retorna a resposta da validação do login.</returns>
        [HttpGet]
        [Route("api/Colaborador/ValidarLogin/{Login}/{Senha}")]
        public async Task<IActionResult> ValidaLogin(string Login , string Senha)
        {
            try
            {
                _logger.LogInformation("ValidarLogin");
                var request = new ValidarColaboradorLoginRequest(Login, Senha);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ValidarLogin - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ValidarLogin - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Adiciona um novo colaborador ao sistema.
        /// </summary>
        /// <param name="request">O objeto contendo as informações do novo colaborador.</param>
        /// <returns>Retorna a resposta da solicitação de adição.</returns>
        [HttpPost]
        [Route("api/Colaborador/AdicionarColaborador")]
        public async Task<IActionResult> AdicionarColaborador([FromBody] AdicionarColaboradorRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarColaborador");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarColaborador - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarColaborador - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista todos os colaboradores no sistema.
        /// </summary>
        /// <returns>Retorna uma lista de todos os colaboradores.</returns>
        [HttpGet]
        [Route("api/Colaborador/ListarColaborador")]
        public async Task<IActionResult> ListarColaborador()
        {

            try
            {
                _logger.LogInformation("ListarColaborador");
                var request = new ListarColaboradorRequest();
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarColaborador - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarColaborador - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Recupera um colaborador específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do colaborador que se deseja recuperar.</param>
        /// <returns>Retorna o colaborador que corresponde ao ID fornecido.</returns>
        [HttpGet]
        [Route("api/Colaborador/ListarColaboradorPorId/{id:Guid}")]
        public async Task<IActionResult> ListarColaboradorPorId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarColaboradorPorId");
                var request = new ListarColaboradorPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarColaboradorPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarColaboradorPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Recupera um colaborador específico pelo seu nome.
        /// </summary>
        /// <param name="NomeColaborador">O nome do colaborador que se deseja recuperar.</param>
        /// <returns>Retorna o colaborador que corresponde ao nome fornecido.</returns>
        [HttpGet]
        [Route("api/Colaborador/ListarColaboradorPorNomeColaborador/{NomeColaborador}")]
        public async Task<IActionResult> ListarColaboradorPorNomeColaborador(string NomeColaborador)
        {

            try
            {
                _logger.LogInformation("ListarColaboradorPorNomeColaborador");
                var request = new ListarColaboradorPorNomeColaboradorRequest(NomeColaborador);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarColaboradorPorNomeColaborador - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarColaboradorPorNomeColaborador - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Altera as informações de um colaborador existente.
        /// </summary>
        /// <param name="request">O objeto contendo as novas informações do colaborador.</param>
        /// <returns>Retorna a resposta da solicitação de alteração.</returns>
        [HttpPut]
        [Route("api/Colaborador/AlterarColaborador")]
        public async Task<IActionResult> AlterarColaborador([FromBody] AlterarColaboradorRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarColaborador");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarColaborador - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarColaborador - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Remove um colaborador do sistema.
        /// </summary>
        /// <param name="id">O ID do colaborador que se deseja remover.</param>
        /// <returns>Retorna a resposta da solicitação de remoção.</returns>
        [HttpDelete]
        [Route("api/Colaborador/RemoverColaborador/{id:Guid}")]
        public async Task<IActionResult> RemoverColaborador(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverColaborador");
                var request = new RemoverColaboradorResquest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverColaborador - Response: {@response}", result);
                return await ResponseAsync(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverColaborador - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        
        #endregion
    }
}
