using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedorPorNomeFornecedor
{
    public class ListarFornecedorPorNomeFornecedorHandler : Notifiable, IRequestHandler<ListarFornecedorPorNomeFornecedorRequest, ListarFornecedorPorNomeFornecedorResponse>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorNomeFornecedorHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public Task<ListarFornecedorPorNomeFornecedorResponse> Handle(ListarFornecedorPorNomeFornecedorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new ListarFornecedorPorNomeFornecedorResponse(this));
            }

            var Collection = _repositoryFornecedor.Listar().Where(x => x.Cnpj == request.Cnpj);
            if (!Collection.Any())
            {
                AddNotification("ATEN��O", "Fornecedor N�O ENCONTRADA");
                return Task.FromResult(new ListarFornecedorPorNomeFornecedorResponse(this));
            }

            //Criar meu objeto de resposta
            var response = new ListarFornecedorPorNomeFornecedorResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


