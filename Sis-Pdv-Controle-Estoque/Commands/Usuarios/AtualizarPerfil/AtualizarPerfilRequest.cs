using MediatR;

namespace Commands.Usuarios.AtualizarPerfil
{
    public class AtualizarPerfilRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}