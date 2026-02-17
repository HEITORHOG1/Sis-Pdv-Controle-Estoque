using MediatR;

namespace Commands.ProdutoPedido.AdicionarProdutoPedido
{
    public class AdicionarProdutoPedidoRequest : IRequest<Response>
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string CodBarras { get; set; }
        public int QuantidadeItemPedido { get; set; }
        public decimal TotalProdutoPedido { get; set; }
    }
}
