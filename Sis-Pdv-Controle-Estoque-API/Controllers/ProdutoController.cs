using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.AtualizarEstoque;
using Commands.Produto.ListarProduto;
using Commands.Produto.ListarProdutoPorId;
using Commands.Produto.ListarProdutoPorNomeProduto;
using Commands.Produto.RemoverProduto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    public class ProdutoController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProdutoController> _logger;
        public ProdutoController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<ProdutoController> logger) : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Produto
        /// <summary>
        /// Adiciona um novo produto.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes do novo produto.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPost]
        [Route("api/Produto/AdicionarProduto")]
        public async Task<IActionResult> AdicionarProduto([FromBody] AdicionarProdutoRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarProduto");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarProduto - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdicionarProduto - Erro - {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista todos os produtos.
        /// </summary>
        /// <returns>Retorna uma ação com uma lista de todos os produtos.</returns>
        [HttpGet]
        [Route("api/Produto/ListarProduto")]
        public async Task<IActionResult> ListarProduto()
        {

            try
            {
                _logger.LogInformation("ListarProduto");
                var request = new ListarProdutoRequest();
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProduto - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProduto - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um produto específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do produto a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do produto solicitado.</returns>
        [HttpGet]
        [Route("api/Produto/ListarProdutoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarProdutoPorId(Guid id)
        {

            try
            {
                _logger.LogInformation("ListarProdutoPorId");
                var request = new ListarProdutoPorIdRequest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPorId - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutoPorId - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lista um produto específico com base no código de barras fornecido.
        /// </summary>
        /// <param name="codBarras">O código de barras do produto a ser recuperado.</param>
        /// <returns>Retorna uma ação com os detalhes do produto solicitado.</returns>
        [HttpGet]
        [Route("api/Produto/ListarProdutoPorCodBarras/{codBarras}")]
        public async Task<IActionResult> ListarProdutoPorCodBarras(string codBarras)
        {

            try
            {
                _logger.LogInformation("ListarProdutoPorCodBarras");
                var request = new ListarProdutoPorCodBarrasRequest(codBarras);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPorCodBarras - Response: {@response}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ListarProdutoPorCodBarras - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Altera os detalhes de um produto existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém os detalhes atualizados do produto.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/Produto/AlterarProduto")]
        public async Task<IActionResult> AlterarProduto([FromBody] AlterarProdutoRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarProduto");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarProduto - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AlterarProduto - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Atualiza o estoque de um produto existente.
        /// </summary>
        /// <param name="request">O objeto de solicitação que contém as informações atualizadas do estoque do produto.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpPut]
        [Route("api/Produto/AtualizaEstoque")]
        public async Task<IActionResult> AtualizaEstoque([FromBody] AtualizarEstoqueRequest request)
        {
            try
            {
                _logger.LogInformation("AtualizaEstoque");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AtualizaEstoque - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AtualizaEstoque - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Remove um produto existente com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do produto a ser removido.</param>
        /// <returns>Retorna uma ação com o resultado da operação.</returns>
        [HttpDelete]
        [Route("api/Produto/RemoverProduto/{id:Guid}")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverProduto");
                var request = new RemoverProdutoResquest(id);
                var result = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverProduto - Response: {@response}", result);
                return await ResponseAsync(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoverProduto - Erro - {@ex}", ex);
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
