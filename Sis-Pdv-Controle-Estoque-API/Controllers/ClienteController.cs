using Commands.Cliente.ListarClientesPaginado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lista clientes com paginação e filtros
        /// </summary>
        /// <param name="request">Parâmetros de paginação e filtros</param>
        /// <returns>Lista paginada de clientes</returns>
        [HttpGet("paginated")]
        public async Task<IActionResult> ListarClientesPaginado([FromQuery] ListarClientesPaginadoRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }
    }
}