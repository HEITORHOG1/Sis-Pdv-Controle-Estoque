using System.Net.Http.Json;
using Sis.Pdv.Blazor.Configuration;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Registra vendas chamando a API de pedidos (nomenclatura legada).
/// POST /v1/pedido
/// </summary>
public sealed class VendaApiService : IVendaApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VendaApiService> _logger;

    public VendaApiService(
        IHttpClientFactory httpClientFactory,
        ILogger<VendaApiService> logger)
    {
        _httpClient = httpClientFactory.CreateClient(ApiSettings.HttpClientName);
        _logger = logger;
    }

    public async Task<Guid> RegistrarVendaAsync(
        RegistrarVendaRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Registrando venda — Valor: {ValorTotal:C2}, Pagamento: {FormaPagamento}",
            request.ValorTotal, request.FormaPagamento);

        // API usa "pedido" como nome da entidade (legado)
        var payload = new
        {
            colaboradorId = request.ColaboradorId,
            valorTotal = request.ValorTotal,
            valorDesconto = request.ValorDesconto,
            formaPagamento = request.FormaPagamento,
            cpfCnpjCliente = request.CpfCnpjCliente
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/Pedido/AdicionarPedido",
            payload,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<VendaResult>(
            cancellationToken: cancellationToken);

        var vendaId = result?.Id ?? Guid.Empty;

        _logger.LogInformation("Venda registrada com ID: {VendaId}", vendaId);

        return vendaId;
    }

    private sealed class VendaResult
    {
        public Guid Id { get; init; }
    }
}
