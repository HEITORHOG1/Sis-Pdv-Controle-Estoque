using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamento
{
    public class ListarDepartamentoPorIdHandler : Notifiable, IRequestHandler<ListarDepartamentoRequest, Commands.Response>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorIdHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<Commands.Response> Handle(ListarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            var grupoCollection = _repositoryDepartamento.Listar().ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

