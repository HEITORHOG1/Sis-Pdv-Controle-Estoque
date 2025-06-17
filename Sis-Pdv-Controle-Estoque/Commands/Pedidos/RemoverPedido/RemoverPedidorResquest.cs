using MediatR;

namespace Commands.Pedidos.RemoverPedido
{
    public class RemoverPedidoResquest : IRequest<Response>
    {
        public RemoverPedidoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
