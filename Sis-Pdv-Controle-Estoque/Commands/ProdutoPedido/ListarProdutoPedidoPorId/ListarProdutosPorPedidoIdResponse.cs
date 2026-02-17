namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutosPorPedidoIdResponse
    {
        public int QuantidadeItemPedido { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal TotalProdutoPedido { get; set; }
    }
}
