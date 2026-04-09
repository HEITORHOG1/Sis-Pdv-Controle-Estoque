using MediatR;

namespace Commands.Cupom.AlterarCupom
{
    public class AlterarCupomRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public DateTime DataEmissao { get; set; }
        public string NumeroSerie { get; set; }
        public string ChaveAcesso { get; set; }
        public decimal ValorTotal { get; set; }
        public string DocumentoCliente { get; set; }
    }
}
