using Sis.Pdv.Blazor.Models.Produto;

namespace Sis.Pdv.Blazor.Repositories;

/// <summary>
/// Contrato para acesso a produtos no banco local do PDV (MySQL).
/// Leitura local para baixa latência — dados sincronizados via RabbitMQ.
/// </summary>
public interface IProdutoRepository
{
    Task<ProdutoDto?> BuscarPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProdutoDto>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<ProdutoDto?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
