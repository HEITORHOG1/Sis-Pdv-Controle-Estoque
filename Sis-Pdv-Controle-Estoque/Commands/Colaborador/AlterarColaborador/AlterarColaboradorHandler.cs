using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorHandler : Notifiable, IRequestHandler<AlterarColaboradorRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryUsuario _repositoryUsuario;
        public AlterarColaboradorHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador, IRepositoryUsuario repositoryUsuario)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(AlterarColaboradorRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarColaboradorRequestValidator();

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

            Model.Colaborador Colaborador = new Model.Colaborador();

            var retornoExist = _repositoryColaborador.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Colaborador", "Colaborador não existe");
                return new Response(this);
            }
            if (string.IsNullOrEmpty(request.Usuario.Id.ToString()))
            {
                AddNotification("Colaborador", "Colaborador não existe");
                return new Response(this);
            }

            var _usuario = new Model.Colaborador
            {
                Usuario = new Model.Usuario
                {
                    Id = request.Usuario.Id,
                    Login = request.Usuario.Login,
                    Senha = request.Usuario.Senha,
                    statusAtivo = request.Usuario.statusAtivo
                }
            };
            Colaborador.AlterarColaborador(request.Id,
                                            request.nomeColaborador,
                                            request.DepartamentoId,
                                            request.cpfColaborador,
                                            request.cargoColaborador,
                                            request.telefoneColaborador,
                                            request.emailPessoalColaborador,
                                            request.emailCorporativo,
                                            request.Usuario);





            _repositoryUsuario.Editar(Colaborador.Usuario);
            Colaborador = _repositoryColaborador.Editar(Colaborador);

            //Criar meu objeto de resposta
            var response = new Response(this, Colaborador);

            return await Task.FromResult(response);
        }
    }
}
