using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Data.Entities;
using Sis.Pdv.Blazor.Messaging.Events;

namespace Sis.Pdv.Blazor.Messaging.Consumers;

/// <summary>
/// Consumidor de eventos de alteração de preço/estoque de produtos.
/// Atualiza o banco local do PDV.
/// </summary>
public sealed class ProdutoAlteradoConsumer : IConsumer<ProdutoAlteradoEvent>
{
    private readonly PdvDbContext _context;
    private readonly ILogger<ProdutoAlteradoConsumer> _logger;

    public ProdutoAlteradoConsumer(
        PdvDbContext context,
        ILogger<ProdutoAlteradoConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProdutoAlteradoEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Recebido evento de produto alterado: {Id}", message.Id);

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == message.Id);

        if (produto is null)
        {
            // Create (se não existe, cria)
            produto = new ProdutoEntity
            {
                Id = message.Id,
                NomeProduto = message.Nome,
                CodBarras = message.CodBarras,
                PrecoVenda = message.PrecoVenda,
                PrecoCusto = message.PrecoCusto,
                QuantidadeEstoqueProduto = message.Estoque,
                DataAtualizacao = DateTime.UtcNow
            };
            _context.Produtos.Add(produto);
            _logger.LogInformation("Produto criado localmente: {Nome}", message.Nome);
        }
        else
        {
            // Update
            produto.NomeProduto = message.Nome;
            produto.CodBarras = message.CodBarras;
            produto.PrecoVenda = message.PrecoVenda;
            produto.PrecoCusto = message.PrecoCusto;
            produto.QuantidadeEstoqueProduto = message.Estoque; // Atenção: isso sobrescreve estoque local!
            produto.DataAtualizacao = DateTime.UtcNow;

            _context.Produtos.Update(produto);
            _logger.LogInformation("Produto atualizado localmente: {Nome}", message.Nome);
        }

        await _context.SaveChangesAsync();
    }
}
