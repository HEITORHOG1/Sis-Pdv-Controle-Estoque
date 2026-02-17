namespace Commands.Pedidos.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataResponse
    {
        public Guid? Id { get; set; }
        public DateTime? DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }


    }
}
