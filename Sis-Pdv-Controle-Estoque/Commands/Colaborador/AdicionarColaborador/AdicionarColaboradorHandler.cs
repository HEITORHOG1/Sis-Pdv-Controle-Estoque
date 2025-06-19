using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.AdicionarColaborador
{
    public class AdicionarColaboradorHandler : Notifiable, IRequestHandler<AdicionarColaboradorRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public AdicionarColaboradorHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Response> Handle(AdicionarColaboradorRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarColaboradorRequestValidator();

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



            Model.Colaborador Colaborador = new(
                                            request.Id,
                                            request.nomeColaborador,
                                            request.DepartamentoId,
                                            request.cpfColaborador,
                                            request.cargoColaborador,
                                            request.telefoneColaborador,
                                            request.emailPessoalColaborador,
                                            request.emailCorporativo,
                                            request.Usuario);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Colaborador = _repositoryColaborador.Adicionar(Colaborador);

            //Criar meu objeto de resposta
            var response = new Response(this, Colaborador);

            return await Task.FromResult(response);
        }
    }
}
