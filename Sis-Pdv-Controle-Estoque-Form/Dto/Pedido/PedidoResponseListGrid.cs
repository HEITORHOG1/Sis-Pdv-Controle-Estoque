namespace Sis_Pdv_Controle_Estoque_Form.Dto.Pedido
{
    public  class PedidoResponseListGrid
    {
        public List<object> notifications { get; set; }
        public bool success { get; set; }
        public List<DataGrid> data { get; set; }
    }
}


