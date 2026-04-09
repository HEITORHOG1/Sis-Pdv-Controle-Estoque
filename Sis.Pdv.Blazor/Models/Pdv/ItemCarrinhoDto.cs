namespace Sis.Pdv.Blazor.Models.Pdv;

/// <summary>
/// Item individual no carrinho de venda do PDV.
/// </summary>
public sealed class ItemCarrinhoDto
{
    public int Sequencial { get; set; }
    public string CodigoBarras { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public decimal PrecoUnitario { get; init; }
    public int Quantidade { get; set; } = 1;
    public bool Cancelado { get; set; }

    public Guid ProdutoId { get; init; }
    public int EstoqueDisponivel { get; init; }
    public DateTime? DataVencimento { get; init; }

    public decimal Total => Cancelado ? 0m : PrecoUnitario * Quantidade;

    /// <summary>
    /// Texto formatado para exibição no NFC-e.
    /// Exemplo: "001 - 7891234567890 - TUBO SOLDAVEL PB 6M"
    /// </summary>
    public string TextoNfce => $"{Sequencial:D3} - {CodigoBarras} - {Descricao}";

    /// <summary>
    /// Detalhe de quantidade e unidade.
    /// Exemplo: "1,000 CDA 45,90"
    /// </summary>
    public string DetalheNfce => $"{Quantidade:N3} CDA {PrecoUnitario:N2}";
}
