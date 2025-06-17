using Commands.ProdutoPedido.AdicionarProdutoPedido;
using Commands.ProdutoPedido.AlterarProdutoPedido;
using Commands.ProdutoPedido.ListarProdutoPedido;
using Commands.ProdutoPedido.ListarProdutoPedidoPorId;
using Commands.ProdutoPedido.RemoverProdutoPedido;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class ProdutoPedidoController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProdutoPedidoController> _logger;
        public ProdutoPedidoController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<ProdutoPedidoController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region ProdutoPedido
        /// <summary>
        /// Adiciona um novo ProdutoPedido.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes do novo ProdutoPedido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPost]
        [Route("api/ProdutoPedido/AdicionarProdutoPedido")]
        public async Task<IActionResult> AdicionarProdutoPedido([FromBody] AdicionarProdutoPedidoRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarProdutoPedido");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarProdutoPedido - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarProdutoPedido - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista todos os ProdutoPedidos.
        /// </summary>
        /// <returns>Retorna uma ação com uma lista de todos os ProdutoPedidos.</returns>
        [HttpGet]
        [Route("api/ProdutoPedido/ListarProdutoPedido")]
        public async Task<IActionResult> ListarProdutoPedido()
        {

            try
            {
                _logger.LogInformation("ListarProdutoPedido");
                var request = new ListarProdutoPedidoRequest();
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPedido - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutoPedido - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um ProdutoPedido específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do ProdutoPedido a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do ProdutoPedido solicitado.</returns>
        [HttpGet]
        [Route("api/ProdutoPedido/ListarProdutoPedidoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarProdutoPedidoPorId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarProdutoPedidoPorId");
                var request = new ListarProdutoPedidoPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPedidoPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutoPedidoPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um ProdutoPedido específico com base no código de barras fornecido.
        /// </summary>
        /// <param name="CpfCnpj">O código de barras do ProdutoPedido a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do ProdutoPedido solicitado.</returns>
        [HttpGet]
        [Route("api/ProdutoPedido/ListarProdutoPedidoPorCodigoDeBarras/{codBarras}")]
        public async Task<IActionResult> ListarProdutoPedidoPorCodigoDeBarras(string CpfCnpj)
        {

            try
            {
                _logger.LogInformation("ListarProdutoPedidoPorCodigoDeBarras");
                var request = new ListarProdutoPedidoPorCodigoDeBarrasRequest(CpfCnpj);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPedidoPorCodigoDeBarras - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutoPedidoPorCodigoDeBarras - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Altera os detalhes de um ProdutoPedido existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes atualizados do ProdutoPedido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/ProdutoPedido/AlterarProdutoPedido")]
        public async Task<IActionResult> AlterarProdutoPedido([FromBody] AlterarProdutoPedidoRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarProdutoPedido");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarProdutoPedido - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarProdutoPedido - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Remove um ProdutoPedido existente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do ProdutoPedido a ser removido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpDelete]
        [Route("api/ProdutoPedido/RemoverProdutoPedido/{id:Guid}")]
        public async Task<IActionResult> RemoverProdutoPedido(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverProdutoPedido");
                var request = new RemoverProdutoPedidoResquest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverProdutoPedido - Response: {@response}", result);
                return await ResponseAsync(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverProdutoPedido - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista os produtos associados a um pedido específico, com base no ID do pedido fornecido.
        /// </summary>
        /// <param name="id">O ID do pedido para recuperar os produtos associados.</param>
        /// <returns>Retorna uma ação com os produtos do pedido solicitado.</returns>
        [HttpGet]
        [Route("api/ProdutoPedido/ListarProdutosPorPedidoId/{id:Guid}")]
        public async Task<IActionResult> ListarProdutosPorPedidoId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarProdutosPorPedidoId");
                var request = new ListarProdutoPedidoPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutosPorPedidoId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutosPorPedidoId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
