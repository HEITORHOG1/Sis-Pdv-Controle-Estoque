namespace Sis.Pdv.Blazor.Models.Pdv;

/// <summary>
/// Estados possíveis da tela do PDV.
/// Controla a visibilidade dos painéis e componentes.
/// </summary>
public enum EstadoPdv
{
    /// <summary>Tela inicial — aguardando primeiro produto.</summary>
    CaixaLivre,

    /// <summary>Produto adicionado — exibe painéis de venda e NFC-e.</summary>
    VendaEmAndamento,

    /// <summary>Pagamento selecionado — exibe resumo de subtotal.</summary>
    Subtotal,

    /// <summary>Finalizando — exibe pagamento, troco e spinner.</summary>
    Pagamento,

    /// <summary>Venda concluída — exibe agradecimento.</summary>
    Finalizada
}
