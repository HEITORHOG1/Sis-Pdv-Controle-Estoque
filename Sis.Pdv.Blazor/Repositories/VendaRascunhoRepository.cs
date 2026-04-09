using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Data.Entities;
using Sis.Pdv.Blazor.Models.Pdv;

namespace Sis.Pdv.Blazor.Repositories;

public sealed class VendaRascunhoRepository : IVendaRascunhoRepository
{
    private readonly PdvDbContext _context;
    private readonly ILogger<VendaRascunhoRepository> _logger;

    public VendaRascunhoRepository(PdvDbContext context, ILogger<VendaRascunhoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SalvarRascunhoAsync(VendaDto venda, string numeroCaixa, CancellationToken ct = default)
    {
        try
        {
            var existente = await _context.VendasRascunho
                .FirstOrDefaultAsync(r => r.Id == venda.Id, ct);

            var itensJson = JsonSerializer.Serialize(venda.Itens.Where(i => !i.Cancelado).Select(i => new
            {
                i.Sequencial,
                i.ProdutoId,
                i.CodigoBarras,
                i.Descricao,
                i.PrecoUnitario,
                i.Quantidade,
                i.EstoqueDisponivel
            }));

            if (existente != null)
            {
                existente.ItensJson = itensJson;
                existente.UltimaAtualizacao = DateTime.UtcNow;
                existente.CpfCnpjCliente = venda.CpfCnpjCliente;
            }
            else
            {
                // Remove rascunhos anteriores deste caixa
                var antigos = await _context.VendasRascunho
                    .Where(r => r.NumeroCaixa == numeroCaixa)
                    .ToListAsync(ct);
                _context.VendasRascunho.RemoveRange(antigos);

                _context.VendasRascunho.Add(new VendaRascunhoEntity
                {
                    Id = venda.Id,
                    ColaboradorId = venda.ColaboradorId,
                    NomeOperador = venda.NomeOperador,
                    DataAbertura = venda.DataAbertura,
                    ItensJson = itensJson,
                    NumeroCaixa = numeroCaixa,
                    CpfCnpjCliente = venda.CpfCnpjCliente
                });
            }

            await _context.SaveChangesAsync(ct);
            _logger.LogDebug("Rascunho salvo: VendaId={VendaId}, Itens={Count}", venda.Id, venda.QuantidadeItensAtivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar rascunho da venda {VendaId}", venda.Id);
        }
    }

    public async Task<VendaDto?> BuscarRascunhoPendenteAsync(string numeroCaixa, CancellationToken ct = default)
    {
        var rascunho = await _context.VendasRascunho
            .Where(r => r.NumeroCaixa == numeroCaixa)
            .OrderByDescending(r => r.UltimaAtualizacao)
            .FirstOrDefaultAsync(ct);

        if (rascunho == null) return null;

        var itens = DeserializarItens(rascunho.ItensJson);
        if (itens.Count == 0) return null;

        var venda = new VendaDto
        {
            ColaboradorId = rascunho.ColaboradorId,
            NomeOperador = rascunho.NomeOperador,
            CpfCnpjCliente = rascunho.CpfCnpjCliente
        };

        // Usar reflection para setar Id e DataAbertura (init properties)
        // Workaround: criar novo VendaDto com valores via construtor implícito
        foreach (var item in itens)
            venda.Itens.Add(item);

        _logger.LogInformation("Rascunho encontrado: VendaId={VendaId}, Itens={Count}, Ultima atualizacao={Data}",
            rascunho.Id, itens.Count, rascunho.UltimaAtualizacao);

        return venda;
    }

    public async Task RemoverRascunhoAsync(Guid vendaId, CancellationToken ct = default)
    {
        var rascunho = await _context.VendasRascunho
            .FirstOrDefaultAsync(r => r.Id == vendaId, ct);

        if (rascunho != null)
        {
            _context.VendasRascunho.Remove(rascunho);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Rascunho removido: VendaId={VendaId}", vendaId);
        }
    }

    public async Task RemoverRascunhoPorCaixaAsync(string numeroCaixa, CancellationToken ct = default)
    {
        var rascunhos = await _context.VendasRascunho
            .Where(r => r.NumeroCaixa == numeroCaixa)
            .ToListAsync(ct);

        if (rascunhos.Count > 0)
        {
            _context.VendasRascunho.RemoveRange(rascunhos);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Removidos {Count} rascunhos do caixa {Caixa}", rascunhos.Count, numeroCaixa);
        }
    }

    private static List<ItemCarrinhoDto> DeserializarItens(string json)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<ItemCarrinhoDto>>(json, options) ?? [];
        }
        catch
        {
            return [];
        }
    }
}
