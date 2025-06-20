using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorPedido
{
    public class ListarProdutoPedidoPorCodigoDeBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorCodigoDeBarrasRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorCodigoDeBarrasHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Commands.Response> Handle(ListarProdutoPedidoPorCodigoDeBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Commands.Response(this);
            }

            var Collection = _repositoryProdutoPedido.Listar()
                                .Where(x => x.CodBarras == request.CodBarras);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "ProdutoPedido NÃO ENCONTRADA");
                return new Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


