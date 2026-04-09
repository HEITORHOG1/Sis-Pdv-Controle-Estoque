using Sis.Pdv.Blazor.Core.Enum;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Serviço centralizado para manipulação de eventos de teclado globais.
/// Integra com JSInterop para capturar teclas (F1, F5, F12, Esc) e disparar ações.
/// </summary>
public interface IKeyboardService
{
    /// <summary>
    /// Evento disparado quando uma tecla mapeada é pressionada.
    /// O argumento é o enum <see cref="JSKeys"/>.
    /// </summary>
    event EventHandler<JSKeys>? KeyDown;

    /// <summary>
    /// Inicializa a escuta de eventos via JavaScript.
    /// Deve ser chamado no `OnAfterRenderAsync` do MainLayout.
    /// </summary>
    Task InitializeAsync();
}
