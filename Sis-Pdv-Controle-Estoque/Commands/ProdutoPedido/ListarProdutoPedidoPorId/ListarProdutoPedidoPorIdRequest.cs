
using MediatR;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutoPedidoPorIdRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarProdutoPedidoPorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
