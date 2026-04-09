namespace Sis.Pdv.Blazor.Models.Pdv;

/// <summary>
/// Estado completo de uma venda em andamento no PDV.
/// </summary>
public sealed class VendaDto
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime DataAbertura { get; init; } = DateTime.Now;
    public Guid ColaboradorId { get; set; }
    public string NomeOperador { get; set; } = string.Empty;
    public string? CpfCnpjCliente { get; set; }

    public List<ItemCarrinhoDto> Itens { get; init; } = [];

    public string? FormaPagamento { get; set; }
    public decimal ValorRecebido { get; set; }
    public decimal ValorDesconto { get; set; }

    // Computed
    public decimal ValorTotal => Itens.Where(i => !i.Cancelado).Sum(i => i.Total);
    public decimal ValorFinal => ValorTotal - ValorDesconto;
    public decimal Troco => ValorRecebido > ValorFinal ? ValorRecebido - ValorFinal : 0m;
    public int QuantidadeItensAtivos => Itens.Count(i => !i.Cancelado);
    public bool PagamentoDefinido => !string.IsNullOrEmpty(FormaPagamento);

    /// <summary>
    /// Resumo para exibição: "3 itens | R$ 135,90"
    /// </summary>
    public string Resumo => $"{QuantidadeItensAtivos} itens | {ValorFinal:C2}";

    /// <summary>
    /// Próximo número sequencial para novo item.
    /// </summary>
    public int ProximoSequencial => Itens.Count > 0 ? Itens.Max(i => i.Sequencial) + 1 : 1;
}
