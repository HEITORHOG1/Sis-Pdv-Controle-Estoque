
namespace Sis.Pdv.Blazor.Messaging.Events;

public record VendaRealizadaEvent
{
    public Guid Id { get; init; }
    public Guid ColaboradorId { get; init; }
    public string NomeOperador { get; init; } = string.Empty;
    public decimal ValorTotal { get; init; }
    public decimal ValorDesconto { get; init; }
    public decimal ValorRecebido { get; init; }
    public string FormaPagamento { get; init; } = string.Empty;
    public string? CpfCnpjCliente { get; init; }
    public DateTime DataVenda { get; init; }
    public List<ItemVendaEventDto> Itens { get; init; } = new();
}

public record ItemVendaEventDto
{
    public Guid ProdutoId { get; init; }
    public string CodigoBarras { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public decimal PrecoUnitario { get; init; }
    public decimal Quantidade { get; init; }
    public int Sequencial { get; init; }
    public bool Cancelado { get; init; }
}
