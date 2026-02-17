using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutoPedidoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorIdRequest, Commands.Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorIdHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public Task<Commands.Response> Handle(ListarProdutoPedidoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            var Collection =
                _repositoryProdutoPedido.ListarProdutosPorPedidoId(request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            //Cria objeto de resposta
            var response = new Commands.Response(this, Collection.Result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

