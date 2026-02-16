namespace Model
{
    public class ProdutoPedidoBase
    {
        public string CodItem { get; set; }
        public string CodigoBarras { get; set; }
        public string Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string Total { get; set; }
        public string NomeProduto { get; set; }
        public string StatusAtivo { get; set; }
        public ProdutoPedidoBase(string codItem, string codigoBarras, string novaDescricao, string quantidade, string valorUnitario, string total, string status)
        {
            CodItem = codItem;
            CodigoBarras = codigoBarras;
            NomeProduto = novaDescricao;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Total = total;
            StatusAtivo = status;
        }

        public ProdutoPedidoBase()
        {
        }
    }
}