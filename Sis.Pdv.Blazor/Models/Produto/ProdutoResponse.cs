namespace Sis.Pdv.Blazor.Models.Produto;

/// <summary>
/// Resposta da API ao listar/buscar produtos.
/// </summary>
public sealed class ProdutoResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public List<ProdutoDto> Data { get; init; } = [];
}
