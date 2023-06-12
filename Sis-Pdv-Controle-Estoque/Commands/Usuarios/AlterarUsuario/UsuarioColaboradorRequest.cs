using MediatR;

namespace Sis_Pdv_Controle_Estoque.Commands.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioRequest : IRequest<Response>
    {
        public string IdLogin { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool statusAtivo { get; set; }
    }
}
