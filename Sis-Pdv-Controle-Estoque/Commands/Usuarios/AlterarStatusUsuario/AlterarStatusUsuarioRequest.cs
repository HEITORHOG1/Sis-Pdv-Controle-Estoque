using MediatR;

namespace Commands.Usuarios.AlterarStatusUsuario
{
    public class AlterarStatusUsuarioRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public bool StatusAtivo { get; set; }
        public string? Motivo { get; set; }
    }
}