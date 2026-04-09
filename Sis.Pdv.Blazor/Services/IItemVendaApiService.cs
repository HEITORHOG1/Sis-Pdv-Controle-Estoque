namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Contrato para registro de itens da venda na API REST.
/// Internamente chama /v1/produtopedido (nomenclatura legada).
/// </summary>
public interface IItemVendaApiService
{
    Task RegistrarItemAsync(RegistrarItemVendaRequest request, CancellationToken cancellationToken = default);
    Task AtualizarEstoqueAsync(Guid produtoId, int quantidadeVendida, CancellationToken cancellationToken = default);
}

/// <summary>
/// Payload para registro de item da venda.
/// </summary>
public sealed class RegistrarItemVendaRequest
{
    public required Guid PedidoId { get; init; }
    public required Guid ProdutoId { get; init; }
    public required int Quantidade { get; init; }
    public required decimal PrecoUnitario { get; init; }
    public required decimal Total { get; init; }
}
