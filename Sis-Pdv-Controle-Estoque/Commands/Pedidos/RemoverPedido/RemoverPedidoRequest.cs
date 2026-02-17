using MediatR;

namespace Commands.Pedidos.RemoverPedido
{
    public class RemoverPedidoRequest : IRequest<Response>
    {
        public RemoverPedidoRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
