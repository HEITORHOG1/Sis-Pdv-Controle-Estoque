using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutoPedidoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorIdRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorIdHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Commands.Response> Handle(ListarProdutoPedidoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            var Collection =
                _repositoryProdutoPedido.ListarProdutosPorPedidoId(request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            //Cria objeto de resposta
            var response = new Commands.Response(this, Collection.Result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

