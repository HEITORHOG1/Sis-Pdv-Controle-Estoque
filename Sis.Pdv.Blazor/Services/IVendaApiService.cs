namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Contrato para registrar vendas na API REST.
/// Internamente chama /v1/pedido (nomenclatura legada do banco).
/// </summary>
public interface IVendaApiService
{
    Task<Guid> RegistrarVendaAsync(RegistrarVendaRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Payload para registro de venda na API.
/// </summary>
public sealed class RegistrarVendaRequest
{
    public required Guid ColaboradorId { get; init; }
    public required decimal ValorTotal { get; init; }
    public required string FormaPagamento { get; init; }
    public decimal ValorDesconto { get; init; }
    public string? CpfCnpjCliente { get; init; }
}
