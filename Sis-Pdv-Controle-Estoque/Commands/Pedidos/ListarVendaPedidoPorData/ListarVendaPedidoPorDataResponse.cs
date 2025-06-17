namespace Commands.Pedidos.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataResponse
    {
        public Guid? Id { get; set; }
        public DateTime? dataDoPedido { get; set; }
        public string formaPagamento { get; set; }
        public decimal totalPedido { get; set; }


    }
}
