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
    /// <summary>
    /// Controlador legado para manter compatibilidade com o cliente WinForms
    /// (rotas /api/Produto/...). Encaminha as requisições para os handlers via MediatR.
    /// </summary>
    public class ProdutoLegacyController : Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProdutoLegacyController> _logger;

        public ProdutoLegacyController(IMediator mediator, IUnitOfWork unitOfWork, ILogger<ProdutoLegacyController> logger)
            : base(unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Endpoints legados
        [HttpPost]
        [Route("api/Produto/AdicionarProduto")]
        public async Task<IActionResult> AdicionarProduto([FromBody] AdicionarProdutoRequest request)
        {
            try
            {
                _logger.LogInformation("AdicionarProduto (legacy)");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AdicionarProduto - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AdicionarProduto - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpPut]
        [Route("api/Produto/AlterarProduto")]
        public async Task<IActionResult> AlterarProduto([FromBody] AlterarProdutoRequest request)
        {
            try
            {
                _logger.LogInformation("AlterarProduto (legacy)");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AlterarProduto - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AlterarProduto - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpPut]
        [Route("api/Produto/AtualizaEstoque")]
        public async Task<IActionResult> AtualizaEstoque([FromBody] AtualizarEstoqueRequest request)
        {
            try
            {
                _logger.LogInformation("AtualizaEstoque (legacy)");
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("AtualizaEstoque - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AtualizaEstoque - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpDelete]
        [Route("api/Produto/RemoverProduto/{id:Guid}")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            try
            {
                _logger.LogInformation("RemoverProduto (legacy)");
                var request = new RemoverProdutoResquest(id);
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("RemoverProduto - Response: {@response}", response);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoverProduto - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpGet]
        [Route("api/Produto/ListarProduto")]
        public async Task<IActionResult> ListarProduto()
        {
            try
            {
                _logger.LogInformation("ListarProduto (legacy)");
                var request = new ListarProdutoRequest();
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProduto - Response: {@response}", response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarProduto - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpGet]
        [Route("api/Produto/ListarProdutoPorId/{id:Guid}")]
        public async Task<IActionResult> ListarProdutoPorId(Guid id)
        {
            try
            {
                _logger.LogInformation("ListarProdutoPorId (legacy)");
                var request = new ListarProdutoPorIdRequest(id);
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPorId - Response: {@response}", response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarProdutoPorId - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }

        [HttpGet]
        [Route("api/Produto/ListarProdutoPorCodBarras/{codBarras}")]
        public async Task<IActionResult> ListarProdutoPorCodBarras(string codBarras)
        {
            try
            {
                _logger.LogInformation("ListarProdutoPorCodBarras (legacy)");
                var request = new ListarProdutoPorCodBarrasRequest(codBarras);
                var response = await _mediator.Send(request, CancellationToken.None);
                _logger.LogInformation("ListarProdutoPorCodBarras - Response: {@response}", response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarProdutoPorCodBarras - Erro");
                return await ResponseExceptionAsync(ex);
            }
        }
        #endregion
    }
}
