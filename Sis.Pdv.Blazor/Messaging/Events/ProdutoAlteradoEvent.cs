namespace Sis.Pdv.Blazor.Messaging.Events;

/// <summary>
/// Evento de notificação de alteração de produto.
/// Contrato compartilhado com o sistema de gestão.
/// </summary>
public record ProdutoAlteradoEvent(
    Guid Id,
    string Nome,
    string CodBarras,
    decimal PrecoVenda,
    decimal PrecoCusto,
    int Estoque
);
