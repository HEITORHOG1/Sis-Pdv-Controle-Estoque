using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Commands.Fornecedor.ListarFornecedor;
using Commands.Fornecedor.ListarFornecedorPorId;
using Commands.Fornecedor.ListarFornecedorPorNomeFornecedor;
using Commands.Fornecedor.RemoverFornecedor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class FornecedorController : Base.ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FornecedorController> _logger;
        public FornecedorController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<FornecedorController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Fornecedor
        /// <summary>
        /// Adiciona um novo fornecedor.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes do novo fornecedor.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPost]
        [Route("api/Fornecedor/AdicionarFornecedor")]
        public async Task<IActionResult> AdicionarFornecedor([FromBody] AdicionarFornecedorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AdicionarFornecedor");
                var response = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("AdicionarFornecedor - Response: {@response}", response);
                return await ResponseAsync(response, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarFornecedor - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista todos os fornecedores.
        /// </summary>
        /// <returns>Retorna uma ação com uma lista de todos os fornecedores.</returns>
        [HttpGet]
        [Route("api/Fornecedor/ListarFornecedor")]
        public async Task<IActionResult> ListarFornecedor(CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarFornecedor");
                var request = new ListarFornecedorRequest();
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarFornecedor - Response: {@response}", result);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("ListarFornecedor - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um fornecedor específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do fornecedor a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do fornecedor solicitado.</returns>
        [HttpGet]
        [Route("api/Fornecedor/ListarFornecedorPorId/{id:Guid}")]
        public async Task<IActionResult> ListarFornecedorPorId(Guid id, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarFornecedorPorId");
                var request = new ListarFornecedorPorIdRequest(id);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarFornecedorPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarFornecedorPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um fornecedor específico com base no CNPJ fornecido.
        /// </summary>
        /// <param name="Cnpj">O CNPJ do fornecedor a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do fornecedor solicitado.</returns>
        [HttpGet]
        [Route("api/Fornecedor/ListarFornecedorPorNomeFornecedor/{Cnpj}")]
        public async Task<IActionResult> ListarFornecedorPorNomeFornecedor(string Cnpj, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarFornecedorPorNomeFornecedor");
                var request = new ListarFornecedorPorNomeFornecedorRequest(Cnpj);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarFornecedorPorNomeFornecedor - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarFornecedorPorNomeFornecedor - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Altera os detalhes de um fornecedor existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes atualizados do fornecedor.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/Fornecedor/AlterarFornecedor")]
        public async Task<IActionResult> AlterarFornecedor([FromBody] AlterarFornecedorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AlterarFornecedor");
                var response = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("AlterarFornecedor - Response: {@response}", response);
                return await ResponseAsync(response, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarFornecedor - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Remove um fornecedor existente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do fornecedor a ser removido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpDelete]
        [Route("api/Fornecedor/RemoverFornecedor/{id:Guid}")]
        public async Task<IActionResult> RemoverFornecedor(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("RemoverFornecedor");
                var request = new RemoverFornecedorRequest(id);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("RemoverFornecedor - Response: {@response}", result);
                return await ResponseAsync(result, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverFornecedor - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
