using MediatR;

namespace Commands.ProdutoPedido.AdicionarProdutoPedido
{
    public class AdicionarProdutoPedidoRequest : IRequest<Response>
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string codBarras { get; set; }
        public int quantidadeItemPedido { get; set; }
        public decimal totalProdutoPedido { get; set; }
    }
}
