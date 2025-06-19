namespace Model
{
    public class ProdutoPedidoBase
    {
        public string codItem { get; set; }
        public string codigoBarras { get; set; }
        public string quantidade { get; set; }
        public string valorUnitario { get; set; }
        public string total { get; set; }
        public string NomeProduto { get; set; }
        public string StatusAtivo { get; set; }
        public ProdutoPedidoBase(string codItem, string codigoBarras, string novaDescricao, string quantidade, string valorUnitario, string total, string status)
        {
            this.codItem = codItem;
            this.codigoBarras = codigoBarras;
            NomeProduto = novaDescricao;
            this.quantidade = quantidade;
            this.valorUnitario = valorUnitario;
            this.total = total;
            StatusAtivo = status;
        }

        public ProdutoPedidoBase()
        {
        }
    }
}