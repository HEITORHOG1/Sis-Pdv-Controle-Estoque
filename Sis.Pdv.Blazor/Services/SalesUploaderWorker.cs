using MassTransit;
using Sis.Pdv.Blazor.Messaging.Events;
using Sis.Pdv.Blazor.Models.Pdv;
using Sis.Pdv.Blazor.Repositories;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Worker que roda em background para enviar vendas locais para o servidor central.
/// Processa vendas com flag Sincronizada = false e publica eventos no RabbitMQ.
/// </summary>
public sealed class SalesUploaderWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SalesUploaderWorker> _logger;
    private readonly IBus _bus;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);

    public SalesUploaderWorker(
        IServiceProvider serviceProvider,
        ILogger<SalesUploaderWorker> logger,
        IBus bus)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SalesUploaderWorker iniciado. Intervalo de verificação: {Interval}s", _checkInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarUploadVendasAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico no loop de upload de vendas.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ProcessarUploadVendasAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        // Obtemos o repositório dentro do escopo (deve ser Scoped no DI)
        var repository = scope.ServiceProvider.GetRequiredService<IVendaRepository>();

        // 1. Buscar vendas não sincronizadas (lote de 50)
        var vendasPendentes = await repository.BuscarVendasPendentesSincronizacaoAsync(ct);

        if (!vendasPendentes.Any())
        {
            return;
        }

        _logger.LogInformation("Encontradas {Count} vendas pendentes de sincronização.", vendasPendentes.Count());

        var idsSincronizados = new List<Guid>();

        foreach (var venda in vendasPendentes)
        {
            try
            {
                // 2. Mapear para Evento de Integração
                var evento = new VendaRealizadaEvent
                {
                    Id = venda.Id,
                    ColaboradorId = venda.ColaboradorId,
                    NomeOperador = venda.NomeOperador,
                    ValorDesconto = venda.ValorDesconto,
                    ValorRecebido = venda.ValorRecebido,
                    FormaPagamento = venda.FormaPagamento ?? string.Empty,
                    CpfCnpjCliente = venda.CpfCnpjCliente,
                    DataVenda = venda.DataAbertura,
                    Itens = venda.Itens.Select(i => new ItemVendaEventDto
                    {
                        ProdutoId = i.ProdutoId,
                        CodigoBarras = i.CodigoBarras,
                        Descricao = i.Descricao,
                        PrecoUnitario = i.PrecoUnitario,
                        Quantidade = i.Quantidade,
                        Sequencial = i.Sequencial,
                        Cancelado = i.Cancelado
                    }).ToList()
                };
                
                // Recalcular Total pois o DTO pode ter lógica computed não serializada
                // Mas o repositório deve ter preenchido.
                // Vou assumir que o DTO tem propriedades get/set ou é calculado pelos itens.
                // Mas o evento precisa do valor fixo.
                evento = evento with { ValorTotal = evento.Itens.Where(i => !i.Cancelado).Sum(i => i.PrecoUnitario * i.Quantidade) - evento.ValorDesconto };

                // 3. Publicar no RabbitMQ
                await _bus.Publish(evento, ct);

                idsSincronizados.Add(venda.Id);
                _logger.LogInformation("Venda {Id} enviada para fila.", venda.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar venda {Id} para RabbitMQ.", venda.Id);
                // Não adiciona à lista de sincronizados, tentará novamente no próximo ciclo
            }
        }

        // 4. Marcar como sincronizadas em lote (para as que tiveram sucesso)
        if (idsSincronizados.Any())
        {
            await repository.MarcarComoSincronizadasAsync(idsSincronizados, ct);
            _logger.LogInformation("Lote de {Count} vendas marcado como sincronizado.", idsSincronizados.Count);
        }
    }
}
