using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.AlterarProdutoPedido
{
    public class AlterarProdutoPedidoHandler : Notifiable, IRequestHandler<AlterarProdutoPedidoRequest, Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public AlterarProdutoPedidoHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Response> Handle(AlterarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {

            Model.ProdutoPedido ProdutoPedido = new Model.ProdutoPedido();

            ProdutoPedido.AlterarProdutoPedido(
                                                request.PedidoId,
                                                request.ProdutoId,
                                                request.CodBarras,
                                                request.QuantidadeItemPedido,
                                                request.TotalProdutoPedido
                );

            var retornoExist = _repositoryProdutoPedido.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("ProdutoPedido", "ProdutoPedido nï¿½o encontrado com o ID informado.");
                return new Response(this);
            }

            ProdutoPedido = await _repositoryProdutoPedido.EditarAsync(ProdutoPedido, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, ProdutoPedido);

            return response;
        }
    }
}

