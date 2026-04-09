namespace Sis.Pdv.Blazor.Models.Produto;

/// <summary>
/// Produto retornado pela API REST.
/// Propriedades espelham o contrato da API (camelCase via JSON policy).
/// </summary>
public sealed class ProdutoDto
{
    public string Id { get; init; } = string.Empty;
    public string NomeProduto { get; init; } = string.Empty;
    public string CodBarras { get; init; } = string.Empty;
    public decimal PrecoVenda { get; init; }
    public decimal PrecoCusto { get; init; }
    public int QuantidadeEstoqueProduto { get; init; }
    public string? Descricao { get; init; }
    public string? CategoriaId { get; init; }
    public string? FornecedorId { get; init; }
    public DateTime? DataVencimento { get; init; }

    /// <summary>
    /// Retorna alertas relevantes para a operação de venda.
    /// </summary>
    public IReadOnlyList<string> ObterAlertasVenda()
    {
        var alertas = new List<string>();

        if (QuantidadeEstoqueProduto <= 0)
            alertas.Add("Produto sem estoque disponível.");

        if (QuantidadeEstoqueProduto is > 0 and <= 5)
            alertas.Add($"Estoque baixo: apenas {QuantidadeEstoqueProduto} unidades.");

        if (DataVencimento.HasValue && DataVencimento.Value <= DateTime.Today)
            alertas.Add("Produto VENCIDO — não deve ser vendido.");

        if (DataVencimento.HasValue && DataVencimento.Value <= DateTime.Today.AddDays(30))
            alertas.Add($"Produto próximo ao vencimento: {DataVencimento.Value:dd/MM/yyyy}.");

        return alertas;
    }
}
