using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioHandler : Notifiable, IRequestHandler<AlterarUsuarioRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public AlterarUsuarioHandler(IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(AlterarUsuarioRequest request, CancellationToken cancellationToken)
        {

            Model.Usuario Usuario = new Model.Usuario()
            {
                Id = Guid.Parse(request.IdLogin),
                Login = request.Login,
                Senha = request.Senha,
                StatusAtivo = request.StatusAtivo
            };

            var retornoExist = _repositoryUsuario.Listar().Where(x => x.Id == Guid.Parse(request.IdLogin));

            if (!retornoExist.Any())
            {
                AddNotification("Usuario", "Usuario nï¿½o existe");
                return new Response(this);
            }

            Usuario = await _repositoryUsuario.EditarAsync(Usuario, cancellationToken);

            // var result = new { Id = Usuario.Id, NomeUsuario = Usuario.nomeUsuario };

            //Criar meu objeto de resposta
            var response = new Response(this, Usuario);

            return response;
        }
    }
}

