using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sis.Pdv.Blazor.ViewModels.Base;

/// <summary>
/// Base para todos os ViewModels do MVVM.
/// Implementa INotifyPropertyChanged e notificação para re-render do Blazor.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Evento usado pelo componente Blazor para chamar StateHasChanged().
    /// </summary>
    public event Action? StateChanged;

    /// <summary>
    /// Atualiza propriedade e notifica se houve mudança.
    /// </summary>
    protected bool SetProperty<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        NotifyStateChanged();
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected void NotifyStateChanged()
        => StateChanged?.Invoke();
}
