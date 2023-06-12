using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido
{
    public  class DataGrid
    {
        public int quantidadeItemPedido { get; set; }
        public string nomeProduto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal totalProdutoPedido { get; set; }
    }
}
