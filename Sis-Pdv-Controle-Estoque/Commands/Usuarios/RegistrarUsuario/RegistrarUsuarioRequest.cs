using MediatR;

namespace Commands.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioRequest : IRequest<Response>
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
        public bool StatusAtivo { get; set; } = true;
    }
}