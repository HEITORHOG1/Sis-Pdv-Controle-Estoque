using Microsoft.JSInterop;
using Sis.Pdv.Blazor.Core.Converters;
using Sis.Pdv.Blazor.Core.DTO;
using Sis.Pdv.Blazor.Core.Enum;

namespace Sis.Pdv.Blazor.Services;

public sealed class KeyboardService : IKeyboardService, IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<KeyboardService>? _objRef;
    private bool _initialized;

    public event EventHandler<JSKeys>? KeyDown;

    public KeyboardService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        _objRef = DotNetObjectReference.Create(this);
        await _jsRuntime.InvokeVoidAsync("window.keyboardEvents.initialize", _objRef, Array.Empty<string>());
        _initialized = true;
    }

    [JSInvokable("OnKeyDownJs")] // Garante match com chamada JS
    public Task OnKeyDownJs(KeyData keyData)
    {
        // 1. Converter key string para enum JSKeys
        // O `keyData.Code` fornece string como "F1", "Escape", "Enter"
        // O `JSKeysConverter.ConvertToKey` converte essa string.
        var key = JSKeysConverter.ConvertToKey(
            keyData.Code ?? keyData.Key, 
            keyData.CtrlKey,
            keyData.ShiftKey,
            keyData.AltKey,
            true, // NumLock active (assumimos true ou pegamos do evento se disponível)
            (long?)keyData.KeyCode
        );

        if (key != JSKeys.None && key != JSKeys.Unknown)
        {
            KeyDown?.Invoke(this, key);
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _objRef?.Dispose();
    }
}
