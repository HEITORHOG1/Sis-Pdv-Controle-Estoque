using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamentoPorId
{
    public class ListarDepartamentoPorIdHandler : Notifiable, IRequestHandler<ListarDepartamentoPorIdRequest, ListarDepartamentoPorIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorIdHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<ListarDepartamentoPorIdResponse> Handle(ListarDepartamentoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return null;
            }

            Model.Departamento Collection = _repositoryDepartamento.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarDepartamentoPorIdResponse)Collection;

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

