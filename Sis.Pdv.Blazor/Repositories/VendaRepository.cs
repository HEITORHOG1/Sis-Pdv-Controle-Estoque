using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Data.Entities;
using Sis.Pdv.Blazor.Models.Pdv;

namespace Sis.Pdv.Blazor.Repositories;

/// <summary>
/// Implementação do acesso a dados de Vendas para o banco local MySQL.
/// Responsável por gerenciar a persistência de vendas e o status de sincronização.
/// </summary>
public sealed class VendaRepository : IVendaRepository
{
    private readonly PdvDbContext _context;
    private readonly ILogger<VendaRepository> _logger;

    public VendaRepository(
        PdvDbContext context,
        ILogger<VendaRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SalvarVendaAsync(VendaDto venda, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Iniciando persistência local da venda {VendaId}", venda.Id);

        // Mapear DTO para Entidade
        var vendaEntity = new VendaEntity
        {
            Id = venda.Id,
            ColaboradorId = venda.ColaboradorId,
            NomeOperador = venda.NomeOperador,
            ValorTotal = venda.ValorTotal,
            ValorDesconto = venda.ValorDesconto,
            ValorPago = venda.ValorTotal, // Assumindo pagamento integral no checkout
            Troco = venda.Troco,
            FormaPagamento = venda.FormaPagamento ?? string.Empty,
            CpfCnpjCliente = venda.CpfCnpjCliente,
            DataVenda = venda.DataAbertura,
            Cancelada = false,
            Sincronizada = false // Importante: venda nasce offline e será sincronizada depois
        };

        foreach (var item in venda.Itens)
        {
            vendaEntity.Itens.Add(new ItemVendaEntity
            {
                Id = Guid.NewGuid(),
                VendaId = venda.Id,
                ProdutoId = item.ProdutoId,
                Sequencial = item.Sequencial,
                CodigoBarras = item.CodigoBarras,
                Descricao = item.Descricao,
                PrecoUnitario = item.PrecoUnitario,
                Quantidade = item.Quantidade,
                Total = item.Total,
                Cancelado = item.Cancelado
            });
        }

        try
        {
            // Salva de forma transacional usando EF Core
            _context.Vendas.Add(vendaEntity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Venda {VendaId} salva localmente com sucesso.", venda.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar venda localmente: {VendaId}", venda.Id);
            throw; // Repassa erro para ViewModel tratar e informar usuário
        }
    }

    public async Task<IEnumerable<VendaDto>> BuscarVendasPendentesSincronizacaoAsync(CancellationToken cancellationToken = default)
    {
        var vendasEntities = await _context.Vendas
            .Include(v => v.Itens)
            .Where(v => !v.Sincronizada)
            .OrderBy(v => v.DataVenda)
            .Take(50) // Limita lote para não sobrecarregar
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        // Mapear de volta para DTO (simplificado)
        // Aqui mapeia apenas o suficiente para o envio, ou usa a mesma estrutura
        return vendasEntities.Select(v => new VendaDto
        {
            Id = v.Id,
            DataAbertura = v.DataVenda,
            ColaboradorId = v.ColaboradorId,
            NomeOperador = v.NomeOperador,
            // ValorTotal é computed, não setamos
            ValorDesconto = v.ValorDesconto,
            ValorRecebido = v.ValorPago, 
            FormaPagamento = v.FormaPagamento,
            CpfCnpjCliente = v.CpfCnpjCliente,
            
            Itens = v.Itens.Select(i => new ItemCarrinhoDto
            {
                Sequencial = i.Sequencial,
                ProdutoId = i.ProdutoId,
                CodigoBarras = i.CodigoBarras,
                Descricao = i.Descricao,
                PrecoUnitario = i.PrecoUnitario,
                Quantidade = i.Quantidade,
                // Total é computed
                Cancelado = i.Cancelado
            }).ToList()
        });
    }

    public async Task MarcarComoSincronizadasAsync(IEnumerable<Guid> vendaIds, CancellationToken cancellationToken = default)
    {
        var vendas = await _context.Vendas
            .Where(v => vendaIds.Contains(v.Id))
            .ToListAsync(cancellationToken);

        foreach (var venda in vendas)
        {
            venda.Sincronizada = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{Count} vendas marcadas como sincronizadas.", vendas.Count);
    }
}
