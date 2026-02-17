using MediatR;

namespace Commands.ProdutoPedido.AlterarProdutoPedido
{
    public class AlterarProdutoPedidoRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string CodBarras { get; set; }
        public int QuantidadeItemPedido { get; set; }
        public decimal TotalProdutoPedido { get; set; }
    }
}
