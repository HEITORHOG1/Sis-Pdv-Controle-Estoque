using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamentoPorId
{
    public class ListarDepartamentoPorIdHandler : Notifiable, IRequestHandler<ListarDepartamentoPorIdRequest, ListarDepartamentoPorIdResponse>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorIdHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<ListarDepartamentoPorIdResponse> Handle(ListarDepartamentoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            Model.Departamento Collection = _repositoryDepartamento.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarDepartamentoPorIdResponse)Collection;

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

