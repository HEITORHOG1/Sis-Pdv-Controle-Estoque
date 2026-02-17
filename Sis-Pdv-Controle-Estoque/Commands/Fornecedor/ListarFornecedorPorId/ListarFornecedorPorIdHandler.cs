using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdHandler : Notifiable, IRequestHandler<ListarFornecedorPorIdRequest, ListarFornecedorPorIdResponse>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorIdHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public Task<ListarFornecedorPorIdResponse> Handle(ListarFornecedorPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            Model.Fornecedor Collection = _repositoryFornecedor.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarFornecedorPorIdResponse)Collection;

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

