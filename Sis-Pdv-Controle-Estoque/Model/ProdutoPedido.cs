namespace Model
{
    public class ProdutoPedido : EntityBase
    {
        public ProdutoPedido()
        {

        }
        public ProdutoPedido(Guid pedidoId,
                             Guid produtoId,
                            string codBarras,
                            int quantidadeItemPedido,
                            decimal totalProdutoPedido)
        {
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
            new Pedido { Id = pedidoId };
            new Produto { Id = produtoId };
            CodBarras = codBarras;
            QuantidadeItemPedido = quantidadeItemPedido;
            TotalProdutoPedido = totalProdutoPedido;
        }
    }
}
