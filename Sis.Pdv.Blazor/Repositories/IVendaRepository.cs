using Sis.Pdv.Blazor.Models.Pdv;

namespace Sis.Pdv.Blazor.Repositories;

/// <summary>
/// Contrato para persistência de vendas no banco local do PDV.
/// </summary>
public interface IVendaRepository
{
    /// <summary>
    /// Salva uma nova venda e seus itens no banco local.
    /// A venda é marcada inicialmente como Sincronizada = false.
    /// </summary>
    Task SalvarVendaAsync(VendaDto venda, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vendas pendentes de sincronização para envio ao servidor central.
    /// </summary>
    Task<IEnumerable<VendaDto>> BuscarVendasPendentesSincronizacaoAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca um lote de vendas como sincronizado após upload com sucesso.
    /// </summary>
    Task MarcarComoSincronizadasAsync(IEnumerable<Guid> vendaIds, CancellationToken cancellationToken = default);
}
