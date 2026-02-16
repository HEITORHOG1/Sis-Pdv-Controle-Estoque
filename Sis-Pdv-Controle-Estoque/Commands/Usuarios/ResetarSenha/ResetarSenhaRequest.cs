using MediatR;

namespace Commands.Usuarios.ResetarSenha
{
    public class ResetarSenhaRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
        public bool ForcarTrocaSenha { get; set; } = true;
    }
}