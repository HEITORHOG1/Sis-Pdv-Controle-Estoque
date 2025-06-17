using MediatR;

namespace Commands.Pedidos.ListarPedidorPorId
{
    public class ListarPedidoPorIdRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarPedidoPorIdRequest(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }

    }
}
