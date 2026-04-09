
using MassTransit;
using MediatR;
using Sis.Pdv.Blazor.Messaging.Events;
using Commands.Pedidos.ProcessarVendaPdv;

namespace Sis_Pdv_Controle_Estoque_API.Messaging.Consumers;

public class VendaRealizadaConsumer : IConsumer<VendaRealizadaEvent>
{
    private readonly ILogger<VendaRealizadaConsumer> _logger;
    private readonly IMediator _mediator;

    public VendaRealizadaConsumer(
        ILogger<VendaRealizadaConsumer> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<VendaRealizadaEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation("RabbitMQ: Recebida venda {VendaId} do PDV.", msg.Id);

        try
        {
            // Mapeia Evento (Infra/Messaging) -> Comando (Domain/Core)
            var command = new ProcessarVendaPdvCommand
            {
                Id = msg.Id,
                ColaboradorId = msg.ColaboradorId,
                TotalPedido = msg.ValorTotal,
                FormaPagamento = msg.FormaPagamento,
                DataDoPedido = msg.DataVenda,
                Itens = msg.Itens.Select(i => new ItemProcessarVendaDto
                {
                    ProdutoId = i.ProdutoId,
                    Preco = i.PrecoUnitario,
                    Quantidade = (int)i.Quantidade,
                    CodBarras = i.CodigoBarras
                }).ToList()
            };

            // Dispara lógica de domínio transacional
            var response = await _mediator.Send(command, context.CancellationToken);

            if (!response.Success)
            {
                // Se a lógica de domínio retornou falha explícita (não tratada)
                throw new InvalidOperationException($"Falha no processamento: {response.Data}");
            }
            
            _logger.LogInformation("Venda {VendaId} integrada ao banco central com sucesso.", msg.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consumir venda {VendaId}.", msg.Id);
            throw; // Permite que o MassTransit faça retry/dead-letter
        }
    }
}
