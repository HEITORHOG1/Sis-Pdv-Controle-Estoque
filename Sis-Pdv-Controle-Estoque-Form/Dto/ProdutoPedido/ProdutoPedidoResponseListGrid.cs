namespace Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido
{
    public class ProdutoPedidoResponseListGrid
    {
        public List<object> notifications { get; set; }
        public bool success { get; set; }
        public List<DataGrid> data { get; set; }
    }
}


