using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.ListarDepartamentoPorNomeDepartamento
{
    public class ListarDepartamentoPorNomeDepartamentoHandler : Notifiable, IRequestHandler<ListarDepartamentoPorNomeDepartamentoRequest, ListarDepartamentoPorNomeDepartamentoResponse>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorNomeDepartamentoHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<ListarDepartamentoPorNomeDepartamentoResponse> Handle(ListarDepartamentoPorNomeDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new ListarDepartamentoPorNomeDepartamentoResponse(this));
            }

            var Collection = _repositoryDepartamento.Listar().Where(x => x.NomeDepartamento == request.NomeDepartamento);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "Departamento NÃO ENCONTRADA");
                return Task.FromResult(new ListarDepartamentoPorNomeDepartamentoResponse(this));
            }

            //Criar meu objeto de resposta
            var response = new ListarDepartamentoPorNomeDepartamentoResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


