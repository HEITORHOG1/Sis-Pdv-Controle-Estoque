using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Data.Entities;
using Sis.Pdv.Blazor.Models.Produto;

namespace Sis.Pdv.Blazor.Repositories;

/// <summary>
/// Acesso a produtos no banco local do PDV via EF Core.
/// Dados sincronizados do banco principal via RabbitMQ.
/// </summary>
public sealed class ProdutoRepository : IProdutoRepository
{
    private readonly PdvDbContext _context;
    private readonly ILogger<ProdutoRepository> _logger;

    public ProdutoRepository(
        PdvDbContext context,
        ILogger<ProdutoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProdutoDto?> BuscarPorCodigoBarrasAsync(
        string codigoBarras,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Buscando produto por código: {CodigoBarras}", codigoBarras);

        var entity = await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CodBarras == codigoBarras, cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Produto não encontrado: {CodigoBarras}", codigoBarras);
            return null;
        }

        _logger.LogInformation(
            "Produto encontrado: {Nome} — R$ {Preco:N2}",
            entity.NomeProduto, entity.PrecoVenda);

        return MapearParaDto(entity);
    }

    public async Task<IReadOnlyList<ProdutoDto>> ListarTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Produtos
            .AsNoTracking()
            .OrderBy(p => p.NomeProduto)
            .ToListAsync(cancellationToken);

        return entities.Select(MapearParaDto).ToList();
    }

    public async Task<ProdutoDto?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return entity is null ? null : MapearParaDto(entity);
    }

    private static ProdutoDto MapearParaDto(ProdutoEntity entity)
    {
        return new ProdutoDto
        {
            Id = entity.Id.ToString(),
            NomeProduto = entity.NomeProduto,
            CodBarras = entity.CodBarras,
            PrecoVenda = entity.PrecoVenda,
            PrecoCusto = entity.PrecoCusto,
            QuantidadeEstoqueProduto = entity.QuantidadeEstoqueProduto,
            Descricao = entity.Descricao,
            CategoriaId = entity.CategoriaId?.ToString(),
            FornecedorId = entity.FornecedorId?.ToString(),
            DataVencimento = entity.DataVencimento
        };
    }
}
