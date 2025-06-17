using MediatR;

namespace Commands.Pedidos.ListarPedidorPorId
{
    public class ListarPedidoPorIdRequest : IRequest<Commands.Response>
    {
        public ListarPedidoPorIdRequest(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }

    }
}
