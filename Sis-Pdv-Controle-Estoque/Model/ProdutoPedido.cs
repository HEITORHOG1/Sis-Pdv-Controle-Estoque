namespace Model
{
    public class ProdutoPedido : EntityBase
    {
        public ProdutoPedido()
        {

        }
        public ProdutoPedido(Guid pedidoId,
                             Guid produtoId,
                            string CodBarras,
                            int QuantidadeItemPedido,
                            decimal TotalProdutoPedido)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            this.CodBarras = CodBarras;
            this.QuantidadeItemPedido = QuantidadeItemPedido;
            this.TotalProdutoPedido = TotalProdutoPedido;
        }



        public virtual Pedido? Pedido { get; set; }
        public virtual Produto? Produto { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string? CodBarras { get; set; }
        public int? QuantidadeItemPedido { get; set; }
        public decimal? TotalProdutoPedido { get; set; }


        internal void AlterarProdutoPedido(Guid pedidoId, Guid produtoId, string CodBarras, int QuantidadeItemPedido, decimal TotalProdutoPedido)
        {
            new Pedido { Id = pedidoId };
            new Produto { Id = produtoId };
            this.CodBarras = CodBarras;
            this.QuantidadeItemPedido = QuantidadeItemPedido;
            this.TotalProdutoPedido = TotalProdutoPedido;
        }
    }
}
