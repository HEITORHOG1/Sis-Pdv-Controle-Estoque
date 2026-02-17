using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedor
{
    public class ListarFornecedorPorIdHandler : Notifiable, IRequestHandler<ListarFornecedorRequest, Commands.Response>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorIdHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Commands.Response> Handle(ListarFornecedorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "Request não pode ser nulo");
                return new Commands.Response(this);
            }

            var grupoCollection = await _repositoryFornecedor.ListarAsync();

            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return response;
        }
    }
}

