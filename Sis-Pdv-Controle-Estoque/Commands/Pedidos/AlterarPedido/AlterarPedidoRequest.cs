using MediatR;

namespace Commands.Pedidos.AlterarPedido
{
    public class AlterarPedidoRequest : IRequest<Commands.Response>
    {
        public Guid Id { get; set; }
        public Guid ColaboradorId { get; set; }
        public Guid? ClienteId { get; set; }
        public int Status { get; set; }
        public DateTime DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }
    }
}
