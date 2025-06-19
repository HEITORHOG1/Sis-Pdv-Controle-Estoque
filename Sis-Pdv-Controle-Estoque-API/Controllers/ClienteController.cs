using Commands.Cliente.AdicionarCliente;
using Commands.Cliente.ListarClientePorCpfCnpj;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class ClienteController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClienteController> _logger;
        public ClienteController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<ClienteController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #region Cliente
        /// <summary>
        /// Adiciona um novo cliente ao sistema.
        /// </summary>
        /// <param name="request">O objeto contendo as informações do novo cliente.</param>
        /// <returns>Retorna a resposta da solicitação de adição.</returns>
        [HttpPost]
        [Route("api/Cliente/AdicionarCliente")]
        public async Task<IActionResult> AdicionarCliente([FromBody] AdicionarClienteRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarCliente");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarCliente - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarCliente - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Recupera um cliente específico pelo seu CPF ou CNPJ.
        /// </summary>
        /// <param name="CpfCnpj">O CPF ou CNPJ do cliente que se deseja recuperar.</param>
        /// <returns>Retorna o cliente que corresponde ao CPF ou CNPJ fornecido.</returns>
        [HttpGet]
        [Route("api/Cliente/ListarClientePorCpfCnpj/{CpfCnpj}")]
        public async Task<IActionResult> ListarClientePorCpfCnpj(string CpfCnpj)
        {

            try
            {
                _logger.LogInformation("ListarClientePorCpfCnpj");
                var request = new ListarClientePorCpfCnpjRequest(CpfCnpj);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarClientePorCpfCnpj - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarClientePorCpfCnpj - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }

        #endregion
    }
}
