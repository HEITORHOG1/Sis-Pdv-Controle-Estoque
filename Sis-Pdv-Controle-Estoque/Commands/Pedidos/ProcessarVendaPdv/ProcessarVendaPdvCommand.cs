
using MediatR;

namespace Commands.Pedidos.ProcessarVendaPdv
{
    public class ProcessarVendaPdvCommand : IRequest<Response>
    {
        public Guid Id { get; set; }
        public Guid ColaboradorId { get; set; }
        public decimal TotalPedido { get; set; }
        public string? FormaPagamento { get; set; }
        public DateTime DataDoPedido { get; set; }
        public string? CpfCnpjCliente { get; set; }
        public List<ItemProcessarVendaDto> Itens { get; set; } = new();

        public ProcessarVendaPdvCommand() { }

        public ProcessarVendaPdvCommand(Guid id, Guid colaboradorId, decimal total, string formaPagto, DateTime data, List<ItemProcessarVendaDto> itens)
        {
            Id = id;
            ColaboradorId = colaboradorId;
            TotalPedido = total;
            FormaPagamento = formaPagto;
            DataDoPedido = data;
            Itens = itens;
        }
    }

    public class ItemProcessarVendaDto
    {
        public Guid ProdutoId { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string? CodBarras { get; set; }
    }
}
