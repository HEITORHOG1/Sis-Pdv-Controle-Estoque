using MediatR;

namespace Commands.ProdutoPedido.RemoverProdutoPedido
{
    public class RemoverProdutoPedidoRequest : IRequest<Response>
    {
        public RemoverProdutoPedidoRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
