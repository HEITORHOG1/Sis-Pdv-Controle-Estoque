namespace Model
{
    public class Pedido : EntityBase
    {
        public Pedido()
        {
            Colaborador = new Colaborador();
            Cliente = new Cliente();
        }
        public Pedido(Guid? colaboradorId, Guid? clienteId, int status, DateTime DataDoPedido, string FormaPagamento, decimal TotalPedido)
        {
            this.ColaboradorId = colaboradorId;
            this.ClienteId = clienteId;
            Status = status;
            this.DataDoPedido = DataDoPedido;
            this.FormaPagamento = FormaPagamento;
            this.TotalPedido = TotalPedido;
        }
        public virtual Colaborador? Colaborador { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public int Status { get; set; }
        public DateTime? DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }
        public Guid? ColaboradorId { get; set; }
        public Guid? ClienteId { get; set; }

        internal void AlterarPedido(Guid colaboradorId, Guid? clienteId, int status, DateTime DataDoPedido, string FormaPagamento, decimal TotalPedido)
        {
            this.ColaboradorId = colaboradorId;
            this.ClienteId = clienteId;
            Status = status;
            this.DataDoPedido = DataDoPedido;
            this.FormaPagamento = FormaPagamento;
            this.TotalPedido = TotalPedido;
        }
    }
}
