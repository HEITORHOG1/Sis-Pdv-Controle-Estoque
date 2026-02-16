using MediatR;
using Model;

namespace Commands.Cliente.ListarClientesPaginado
{
    public class ListarClientesPaginadoRequest : PagedRequest, IRequest<Response>
    {
        public string? TipoCliente { get; set; }
        public bool? ApenasAtivos { get; set; } = true;
    }
}