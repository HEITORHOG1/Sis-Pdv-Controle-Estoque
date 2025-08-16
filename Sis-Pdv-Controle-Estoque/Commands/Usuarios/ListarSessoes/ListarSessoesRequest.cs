using MediatR;

namespace Commands.Usuarios.ListarSessoes
{
    public class ListarSessoesRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
    }
}