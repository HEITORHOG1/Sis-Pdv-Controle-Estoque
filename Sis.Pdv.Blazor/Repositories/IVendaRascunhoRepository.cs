using Sis.Pdv.Blazor.Models.Pdv;

namespace Sis.Pdv.Blazor.Repositories;

public interface IVendaRascunhoRepository
{
    /// <summary>
    /// Salva ou atualiza o rascunho da venda em andamento.
    /// Apenas um rascunho ativo por caixa.
    /// </summary>
    Task SalvarRascunhoAsync(VendaDto venda, string numeroCaixa, CancellationToken ct = default);

    /// <summary>
    /// Busca rascunho pendente para um caixa especifico.
    /// Retorna null se nao tem rascunho.
    /// </summary>
    Task<VendaDto?> BuscarRascunhoPendenteAsync(string numeroCaixa, CancellationToken ct = default);

    /// <summary>
    /// Remove o rascunho apos a venda ser finalizada ou cancelada.
    /// </summary>
    Task RemoverRascunhoAsync(Guid vendaId, CancellationToken ct = default);

    Task RemoverRascunhoPorCaixaAsync(string numeroCaixa, CancellationToken ct = default);
}
