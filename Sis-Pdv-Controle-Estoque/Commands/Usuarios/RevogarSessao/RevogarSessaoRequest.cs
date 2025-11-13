using MediatR;

namespace Commands.Usuarios.RevogarSessao
{
    public class RevogarSessaoRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public Guid SessionId { get; set; }
    }
}