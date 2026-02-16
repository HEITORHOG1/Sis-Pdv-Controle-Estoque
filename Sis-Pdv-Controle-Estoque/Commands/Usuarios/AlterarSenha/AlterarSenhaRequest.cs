using MediatR;

namespace Commands.Usuarios.AlterarSenha
{
    public class AlterarSenhaRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}