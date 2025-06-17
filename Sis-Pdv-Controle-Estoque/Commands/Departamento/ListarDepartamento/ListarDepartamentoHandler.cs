using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamento
{
    public class ListarDepartamentoPorIdHandler : Notifiable, IRequestHandler<ListarDepartamentoRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorIdHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Commands.Response> Handle(ListarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            var grupoCollection = _repositoryDepartamento.Listar().ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

