using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedorPorNomeDepartamento
{
    public class ListarFornecedorPorNomeFornecedorHandler : Notifiable, IRequestHandler<ListarFornecedorPorNomeFornecedorRequest, ListarFornecedorPorNomeFornecedorResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorNomeFornecedorHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<ListarFornecedorPorNomeFornecedorResponse> Handle(ListarFornecedorPorNomeFornecedorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new ListarFornecedorPorNomeFornecedorResponse(this);
            }

            var Collection = _repositoryFornecedor.Listar().Where(x => x.Cnpj == request.Cnpj);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "Fornecedor NÃO ENCONTRADA");
                return new ListarFornecedorPorNomeFornecedorResponse(this);
            }

            //Criar meu objeto de resposta
            var response = new ListarFornecedorPorNomeFornecedorResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


