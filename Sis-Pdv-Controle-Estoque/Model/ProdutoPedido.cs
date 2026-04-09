using Model.Exceptions;

namespace Model
{
    public class ProdutoPedido : EntityBase
    {
        public ProdutoPedido()
        {
        }

        public ProdutoPedido(Guid pedidoId, Guid produtoId, string codBarras,
            int quantidadeItemPedido, decimal totalProdutoPedido)
        {
            ValidarIds(pedidoId, produtoId);
            ValidarQuantidade(quantidadeItemPedido);

            PedidoId = pedidoId;
            ProdutoId = produtoId;
            CodBarras = codBarras;
            QuantidadeItemPedido = quantidadeItemPedido;
            TotalProdutoPedido = totalProdutoPedido;
        }



        public virtual Pedido? Pedido { get; set; }
        public virtual Produto? Produto { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string? CodBarras { get; set; }
        public int? QuantidadeItemPedido { get; set; }
        public decimal? TotalProdutoPedido { get; set; }


        internal void AlterarProdutoPedido(Guid pedidoId, Guid produtoId, string codBarras, int quantidadeItemPedido, decimal totalProdutoPedido)
        {
            ValidarIds(pedidoId, produtoId);
            ValidarQuantidade(quantidadeItemPedido);

            PedidoId = pedidoId;
            ProdutoId = produtoId;
            CodBarras = codBarras;
            QuantidadeItemPedido = quantidadeItemPedido;
            TotalProdutoPedido = totalProdutoPedido;
        }

        private static void ValidarIds(Guid pedidoId, Guid produtoId)
        {
            if (pedidoId == Guid.Empty)
                throw new DomainException("O Id do pedido é obrigatório.");

            if (produtoId == Guid.Empty)
                throw new DomainException("O Id do produto é obrigatório.");
        }

        private static void ValidarQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new DomainException("A quantidade do item deve ser maior que zero.");
        }
    }
}
