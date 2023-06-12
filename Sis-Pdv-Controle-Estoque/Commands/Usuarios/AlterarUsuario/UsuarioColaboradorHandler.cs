using AlterarUsuario;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;

namespace Sis_Pdv_Controle_Estoque.Commands.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioHandler : Notifiable, IRequestHandler<AlterarUsuarioRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryUsuario _repositoryUsuario;

        public AlterarUsuarioHandler(IMediator mediator, IRepositoryUsuario repositoryUsuario)
        {
            _mediator = mediator;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(AlterarUsuarioRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarUsuarioRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

            Model.Usuario Usuario = new Model.Usuario()
            {
                Id = Guid.Parse(request.IdLogin),
                Login = request.Login,
                Senha = request.Senha,
                statusAtivo = request.statusAtivo
            };

            var retornoExist = _repositoryUsuario.Listar().Where(x => x.Id == Guid.Parse(request.IdLogin));

            if (!retornoExist.Any())
            {
                AddNotification("Usuario", "Usuario não existe");
                return new Response(this);
            }

            Usuario = _repositoryUsuario.Editar(Usuario);

            // var result = new { Id = Usuario.Id, NomeUsuario = Usuario.nomeUsuario };

            //Criar meu objeto de resposta
            var response = new Response(this, Usuario);

            return await Task.FromResult(response);
        }
    }
}
