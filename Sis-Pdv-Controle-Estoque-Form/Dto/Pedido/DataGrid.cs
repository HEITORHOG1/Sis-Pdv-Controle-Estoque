namespace Sis_Pdv_Controle_Estoque_Form.Dto.Pedido
{
    public class DataGrid
    {
        public DateTime DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }
        public Guid Id { get; set; }
    }
}
