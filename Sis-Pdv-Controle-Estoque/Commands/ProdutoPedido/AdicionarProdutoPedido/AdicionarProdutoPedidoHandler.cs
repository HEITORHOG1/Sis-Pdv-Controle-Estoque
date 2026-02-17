using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.AdicionarProdutoPedido
{
    public class AdicionarProdutoPedidoHandler : Notifiable, IRequestHandler<AdicionarProdutoPedidoRequest, Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public AdicionarProdutoPedidoHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Response> Handle(AdicionarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {

            Model.ProdutoPedido ProdutoPedido = new(
                                                     request.PedidoId,
                                                     request.ProdutoId,
                                                     request.CodBarras,
                                                     request.QuantidadeItemPedido,
                                                     request.TotalProdutoPedido
                                                    );

            if (IsInvalid())
            {
                return new Response(this);
            }

            ProdutoPedido = await _repositoryProdutoPedido.AdicionarAsync(ProdutoPedido, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, ProdutoPedido);

            return response;
        }
    }
}

