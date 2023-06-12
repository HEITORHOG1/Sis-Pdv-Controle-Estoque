using System.ComponentModel.DataAnnotations.Schema;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("ProdutoPedido")]
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
            this.NomeProduto = novaDescricao;
            this.quantidade = quantidade;
            this.valorUnitario = valorUnitario;
            this.total = total;
            this.StatusAtivo = status;
        }

        public ProdutoPedidoBase()
        {
        }
    }
}