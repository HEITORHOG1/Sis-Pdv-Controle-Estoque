using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedido
{
    public class ListarProdutoPedidoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPedidoRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorIdHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Commands.Response> Handle(ListarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            var grupoCollection = _repositoryProdutoPedido.Listar().ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

