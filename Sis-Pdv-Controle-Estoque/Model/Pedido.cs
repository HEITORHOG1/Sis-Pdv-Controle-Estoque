using Model.Exceptions;

namespace Model
{
    public class Pedido : EntityBase
    {
        public Pedido()
        {
        }

        public Pedido(Guid? colaboradorId, Guid? clienteId, int status, DateTime dataDoPedido, string formaPagamento, decimal totalPedido)
        {
            ValidarFormaPagamento(formaPagamento);
            ValidarTotalPedido(totalPedido);

            ColaboradorId = colaboradorId;
            ClienteId = clienteId;
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

        internal void AlterarPedido(Guid colaboradorId, Guid? clienteId, int status, DateTime dataDoPedido, string formaPagamento, decimal totalPedido)
        {
            ValidarFormaPagamento(formaPagamento);
            ValidarTotalPedido(totalPedido);

            ColaboradorId = colaboradorId;
            ClienteId = clienteId;
            Status = status;
            DataDoPedido = dataDoPedido;
            FormaPagamento = formaPagamento;
            TotalPedido = totalPedido;
        }

        private static void ValidarFormaPagamento(string formaPagamento)
        {
            if (string.IsNullOrWhiteSpace(formaPagamento))
                throw new DomainException("A forma de pagamento é obrigatória.");
        }

        private static void ValidarTotalPedido(decimal totalPedido)
        {
            if (totalPedido < 0)
                throw new DomainException("O total do pedido não pode ser negativo.");
        }
    }
}
