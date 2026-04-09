using Commands.Cupom.AdicionarCupom;
using Commands.Cupom.AlterarCupom;
using Commands.Cupom.RemoverCupom;
using Commands.Cupom.ListarCupons;
using Commands.Cupom.ListarCupomPorId;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;
using Asp.Versioning;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// Endpoints de gerenciamento de cupons fiscais
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cupom")]
    [Produces("application/json")]
    [Tags("Cupons")]
    [Authorize]
    public class CupomController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CupomController(IMediator mediator, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar todos os cupons
        /// </summary>
        [HttpGet]
        [Route("/api/Cupom/ListarCupons")]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarCupons(CancellationToken cancellationToken)
        {
            var request = new ListarCuponsRequest();
            var response = await _mediator.Send(request, cancellationToken);
            return await ResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Buscar cupom por Id
        /// </summary>
        [HttpGet]
        [Route("/api/Cupom/ListarCupomPorId/{id:Guid}")]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarCupomPorId(Guid id, CancellationToken cancellationToken)
        {
            var request = new ListarCupomPorIdRequest(id);
            var response = await _mediator.Send(request, cancellationToken);
            return await ResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Emitir um novo cupom fiscal
        /// </summary>
        [HttpPost]
        [Route("/api/Cupom/AdicionarCupom")]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdicionarCupom([FromBody] AdicionarCupomRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return await ResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Alterar dados de um cupom
        /// </summary>
        [HttpPut]
        [Route("/api/Cupom/AlterarCupom")]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarCupom([FromBody] AlterarCupomRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return await ResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Remover um cupom (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("/api/Cupom/RemoverCupom/{id:Guid}")]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoverCupom(Guid id, CancellationToken cancellationToken)
        {
            var request = new RemoverCupomRequest(id);
            var response = await _mediator.Send(request, cancellationToken);
            return await ResponseAsync(response, cancellationToken);
        }
    }
}
