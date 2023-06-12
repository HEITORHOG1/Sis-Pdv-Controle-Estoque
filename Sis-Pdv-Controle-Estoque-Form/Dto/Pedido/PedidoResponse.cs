namespace Sis_Pdv_Controle_Estoque_Form.Dto.Pedido
{
    public  class PedidoResponse
    {
        public List<object> notifications { get; set; }
        public bool success { get; set; }
        public Data data { get; set; }
    }
}


