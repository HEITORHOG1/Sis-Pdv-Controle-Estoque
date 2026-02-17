using MediatR;

namespace Commands.Pedidos.AdicionarPedido
{
    public class AdicionarPedidoRequest : IRequest<Response>
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
