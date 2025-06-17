
using MediatR;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutoPedidoPorIdRequest : IRequest<Commands.Response>
    {
        public ListarProdutoPedidoPorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
