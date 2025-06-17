using MediatR;

namespace Commands.ProdutoPedido.AlterarProdutoPedido
{
    public class AlterarProdutoPedidoRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string codBarras { get; set; }
        public int quantidadeItemPedido { get; set; }
        public decimal totalProdutoPedido { get; set; }
    }
}
