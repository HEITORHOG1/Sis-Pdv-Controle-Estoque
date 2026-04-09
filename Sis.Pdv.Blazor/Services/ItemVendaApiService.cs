using System.Net.Http.Json;
using Sis.Pdv.Blazor.Configuration;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Registra itens da venda e atualiza estoque via API REST.
/// POST /v1/produtopedido · PUT /v1/produto/estoque
/// </summary>
public sealed class ItemVendaApiService : IItemVendaApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ItemVendaApiService> _logger;

    public ItemVendaApiService(
        IHttpClientFactory httpClientFactory,
        ILogger<ItemVendaApiService> logger)
    {
        _httpClient = httpClientFactory.CreateClient(ApiSettings.HttpClientName);
        _logger = logger;
    }

    public async Task RegistrarItemAsync(
        RegistrarItemVendaRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(
            "Registrando item — Pedido: {PedidoId}, Produto: {ProdutoId}, Qtd: {Quantidade}",
            request.PedidoId, request.ProdutoId, request.Quantidade);

        var payload = new
        {
            pedidoId = request.PedidoId,
            produtoId = request.ProdutoId,
            quantidade = request.Quantidade,
            precoUnitario = request.PrecoUnitario,
            total = request.Total
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/ProdutoPedido/AdicionarProdutoPedido",
            payload,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task AtualizarEstoqueAsync(
        Guid produtoId,
        int quantidadeVendida,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(
            "Atualizando estoque — Produto: {ProdutoId}, Vendido: {Quantidade}",
            produtoId, quantidadeVendida);

        var payload = new
        {
            produtoId,
            quantidade = quantidadeVendida
        };

        var response = await _httpClient.PutAsJsonAsync(
            "/api/v1/produto/AtualizaEstoque",
            payload,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
