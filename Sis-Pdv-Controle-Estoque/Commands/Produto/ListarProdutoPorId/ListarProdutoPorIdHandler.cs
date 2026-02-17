using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPorIdRequest, ListarProdutoPorIdResponse>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorIdHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public Task<ListarProdutoPorIdResponse> Handle(ListarProdutoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new ListarProdutoPorIdResponse());
            }

            Model.Produto Collection = _repositoryProduto.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new ListarProdutoPorIdResponse());
            }

            //Cria objeto de resposta
            var response = (ListarProdutoPorIdResponse)Collection;

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

