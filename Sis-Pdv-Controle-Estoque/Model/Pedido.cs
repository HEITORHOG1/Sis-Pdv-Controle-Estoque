namespace Model
{
    public class Pedido : EntityBase
    {
        public Pedido()
        {
            Colaborador = new Colaborador();
            Cliente = new Cliente();
        }
        public Pedido(Guid? ColaboradorId, Guid? ClienteId, int status, DateTime dataDoPedido, string formaPagamento, decimal totalPedido)
        {
            this.ColaboradorId = ColaboradorId;
            this.ClienteId = ClienteId;
            Status = status;
            DataDoPedido = dataDoPedido;
            FormaPagamento = formaPagamento;
            TotalPedido = totalPedido;
        }
        public virtual Colaborador? Colaborador { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public int Status { get; set; }
        public DateTime? DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }
        public Guid? ColaboradorId { get; set; }
        public Guid? ClienteId { get; set; }

        internal void AlterarPedido(Guid ColaboradorId, Guid? ClienteId, int status, DateTime dataDoPedido, string formaPagamento, decimal totalPedido)
        {
            this.ColaboradorId = ColaboradorId;
            this.ClienteId = ClienteId;
            Status = status;
            DataDoPedido = dataDoPedido;
            FormaPagamento = formaPagamento;
            TotalPedido = totalPedido;
        }
    }
}
