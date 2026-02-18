using Commands.Pedidos.AdicionarPedido;
using Commands.Pedidos.AlterarPedido;
using Commands.Pedidos.ListarPedido;
using Commands.Pedidos.ListarPedidoPorNomeCpfCnpj;
using Commands.Pedidos.ListarPedidoPorId;
using Commands.Pedidos.ListarVendaPedidoPorData;
using Commands.Pedidos.RemoverPedido;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class PedidoController : Base.ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PedidoController> _logger;
        public PedidoController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<PedidoController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Pedido
        /// <summary>
        /// Adiciona um novo pedido.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes do novo pedido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPost]
        [Route("api/Pedido/AdicionarPedido")]
        public async Task<IActionResult> AdicionarPedido([FromBody] AdicionarPedidoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AdicionarPedido");
                var response = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("AdicionarPedido - Response: {@response}", response);
                return await ResponseAsync(response, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarPedido - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Lista todos os pedidos.
        /// </summary>
        /// <returns>Retorna uma ação com uma lista de todos os pedidos.</returns>
        [HttpGet]
        [Route("api/Pedido/ListarPedido")]
        public async Task<IActionResult> ListarPedido(CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarPedido");
                var request = new ListarPedidoRequest();
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarPedido - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarPedido - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Lista um pedido específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do pedido a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do pedido solicitado.</returns>
        [HttpGet]
        [Route("api/Pedido/ListarPedidoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarPedidoPorId(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("ListarPedidoPorId");
                var request = new ListarPedidoPorIdRequest(id);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarPedidoPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarPedidoPorId - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Lista um pedido específico com base no CNPJ fornecido.
        /// </summary>
        /// <param name="Cnpj">O CNPJ relacionado ao pedido a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do pedido solicitado.</returns>
        [HttpGet]
        [Route("api/Pedido/ListarPedidoPorCnpj/{Cnpj}")]
        public async Task<IActionResult> ListarPedidoPorCnpj(string Cnpj, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarPedidoPorCnpj");
                var request = new ListarPedidoPorNomeCpfCnpjRequest(Cnpj);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarPedidoPorCnpj - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarPedidoPorCnpj - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Altera os detalhes de um pedido existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes atualizados do pedido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/Pedido/AlterarPedido")]
        public async Task<IActionResult> AlterarPedido([FromBody] AlterarPedidoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AlterarPedido");
                var response = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("AlterarPedido - Response: {@response}", response);
                return await ResponseAsync(response, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarPedido - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Remove um pedido existente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do pedido a ser removido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpDelete]
        [Route("api/Pedido/RemoverPedido/{id:Guid}")]
        public async Task<IActionResult> RemoverPedido(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("RemoverPedido");
                var request = new RemoverPedidoRequest(id);
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("RemoverPedido - Response: {@response}", result);
                return await ResponseAsync(result, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverPedido - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        /// <summary>
        /// Lista vendas de pedidos em um intervalo de datas.
        /// </summary>
        /// <param name="DataInicio">A data inicial do intervalo.</param>
        /// <param name="DataFim">A data final do intervalo.</param>
        /// <returns>Retorna uma ação com uma lista de vendas de pedidos no intervalo de datas especificado.</returns>
        [HttpGet]
        [Route("api/Pedido/ListarVendaPedidoPorData/{DataInicio}/{DataFim}")]
        public async Task<IActionResult> ListarVendaPedidoPorData(string DataInicio, string DataFim, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("ListarVendaPedidoPorData");
                var request = new ListarVendaPedidoPorDataRequest(Convert.ToDateTime(DataInicio), Convert.ToDateTime(DataFim));
                var result = await _mediator.Send(request, cancellationToken);
                _logger.LogInformation("ListarVendaPedidoPorData - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarVendaPedidoPorData - Erro - {@ex}", ex);
                return await ResponseExceptionAsync(ex);
            }
        }
        #endregion
    }
}
