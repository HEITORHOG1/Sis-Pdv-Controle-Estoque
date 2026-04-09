using Sis.Pdv.Blazor.Core.Enum;

namespace Sis.Pdv.Blazor.Core.Converters;

public static class JSKeysConverter
{
    private const JSKeys FIRST_DIGIT = JSKeys.D0;
    private const JSKeys LAST_DIGIT = JSKeys.D9;
    private const JSKeys FIRST_ASCII = JSKeys.A;
    private const JSKeys LAST_ASCII = JSKeys.Z;
    private const JSKeys FIRST_NUMPAD_DIGIT = JSKeys.NumPad0;
    private const JSKeys LAST_NUMPAD_DIGIT = JSKeys.NumPad9;

    private static readonly Dictionary<long, JSKeys> JsKeyCodeToKeys = new()
    {
        {48, JSKeys.D0},
        {49, JSKeys.D1},
        {50, JSKeys.D2},
        {51, JSKeys.D3},
        {52, JSKeys.D4},
        {53, JSKeys.D5},
        {54, JSKeys.D6},
        {55, JSKeys.D7},
        {56, JSKeys.D8},
        {57, JSKeys.D9},

        {65, JSKeys.A},
        {66, JSKeys.B},
        {67, JSKeys.C},
        {68, JSKeys.D},
        {69, JSKeys.E},
        {70, JSKeys.F},
        {71, JSKeys.G},
        {72, JSKeys.H},
        {73, JSKeys.I},
        {74, JSKeys.J},
        {75, JSKeys.K},
        {76, JSKeys.L},
        {77, JSKeys.M},
        {78, JSKeys.N},
        {79, JSKeys.O},
        {80, JSKeys.P},
        {81, JSKeys.Q},
        {82, JSKeys.R},
        {83, JSKeys.S},
        {84, JSKeys.T},
        {85, JSKeys.U},
        {86, JSKeys.V},
        {87, JSKeys.W},
        {88, JSKeys.X},
        {89, JSKeys.Y},
        {90, JSKeys.Z},

        {13,JSKeys.Enter },
        {9, JSKeys.Tab }

    };
    private static readonly Dictionary<string, JSKeys> JsToKeys = new()
    {
        {"Unidentified", JSKeys.None},

        {"Digit0", JSKeys.D0},
        {"Digit1", JSKeys.D1},
        {"Digit2", JSKeys.D2},
        {"Digit3", JSKeys.D3},
        {"Digit4", JSKeys.D4},
        {"Digit5", JSKeys.D5},
        {"Digit6", JSKeys.D6},
        {"Digit7", JSKeys.D7},
        {"Digit8", JSKeys.D8},
        {"Digit9", JSKeys.D9},

        {"Numpad0", JSKeys.NumPad0},
        {"Numpad1", JSKeys.NumPad1},
        {"Numpad2", JSKeys.NumPad2},
        {"Numpad3", JSKeys.NumPad3},
        {"Numpad4", JSKeys.NumPad4},
        {"Numpad5", JSKeys.NumPad5},
        {"Numpad6", JSKeys.NumPad6},
        {"Numpad7", JSKeys.NumPad7},
        {"Numpad8", JSKeys.NumPad8},
        {"Numpad9", JSKeys.NumPad9},

        {"KeyA", JSKeys.A},
        {"KeyB", JSKeys.B},
        {"KeyC", JSKeys.C},
        {"KeyD", JSKeys.D},
        {"KeyE", JSKeys.E},
        {"KeyF", JSKeys.F},
        {"KeyG", JSKeys.G},
        {"KeyH", JSKeys.H},
        {"KeyI", JSKeys.I},
        {"KeyJ", JSKeys.J},
        {"KeyK", JSKeys.K},
        {"KeyL", JSKeys.L},
        {"KeyM", JSKeys.M},
        {"KeyN", JSKeys.N},
        {"KeyO", JSKeys.O},
        {"KeyP", JSKeys.P},
        {"KeyQ", JSKeys.Q},
        {"KeyR", JSKeys.R},
        {"KeyS", JSKeys.S},
        {"KeyT", JSKeys.T},
        {"KeyU", JSKeys.U},
        {"KeyV", JSKeys.V},
        {"KeyW", JSKeys.W},
        {"KeyX", JSKeys.X},
        {"KeyY", JSKeys.Y},
        {"KeyZ", JSKeys.Z},

        {"F1", JSKeys.F1},
        {"F2", JSKeys.F2},
        {"F3", JSKeys.F3},
        {"F4", JSKeys.F4},
        {"F5", JSKeys.F5},
        {"F6", JSKeys.F6},
        {"F7", JSKeys.F7},
        {"F8", JSKeys.F8},
        {"F9", JSKeys.F9},
        {"F10", JSKeys.F10},
        {"F11", JSKeys.F11},
        {"F12", JSKeys.F12},
        {"F13", JSKeys.F13},
        {"F14", JSKeys.F14},
        {"F15", JSKeys.F15},
        {"F16", JSKeys.F16},
        {"F17", JSKeys.F17},
        {"F18", JSKeys.F18},
        {"F19", JSKeys.F19},
        {"F20", JSKeys.F20},
        {"F21", JSKeys.F21},
        {"F22", JSKeys.F22},
        {"F23", JSKeys.F23},
        {"F24", JSKeys.F24},

        {"Comma", JSKeys.Comma },
        {"Period", JSKeys.Period },
        {"Semicolon", JSKeys.SemiColon },
        {"Slash", JSKeys.Slash },
        {"Quote", JSKeys.Quote },
        {"Backquote", JSKeys.Backquote },
        {"Backslash", JSKeys.Backslash},
        {"BracketLeft", JSKeys.OpenBracket },
        {"BracketRight", JSKeys.CloseBracket },
        {"Minus", JSKeys.Minus},
        {"Equal", JSKeys.Equal},
        {"IntlRo", JSKeys.IntlRo},
        {"IntlBackslash", JSKeys.IntlBackslash},
        {"ContextMenu", JSKeys.ContextMenu},

        {"NumLock", JSKeys.NumLock},
        {"CapsLock", JSKeys.CapsLock},
        {"ScrollLock", JSKeys.ScrollLock},

        {"AltLeft", JSKeys.Alt},
        {"AltRight", JSKeys.Alt},
        {"ControlLeft", JSKeys.Control},
        {"ControlRight", JSKeys.Control},
        {"MetaLeft", JSKeys.Meta},
        {"MetaRight", JSKeys.Meta},
        {"ShiftLeft", JSKeys.Shift},
        {"ShiftRight", JSKeys.Shift},

        {"Enter", JSKeys.Enter},
        {"Space", JSKeys.Space},
        {"Backspace", JSKeys.Backspace},
        {"Tab", JSKeys.Tab},
        {"Delete", JSKeys.Delete},
        {"End", JSKeys.End},
        {"Help", JSKeys.Help},
        {"Home", JSKeys.Home},
        {"Insert", JSKeys.Insert},
        {"PageDown", JSKeys.PageDown},
        {"PageUp", JSKeys.PageUp},
        {"ArrowDown", JSKeys.ArrowDown},
        {"ArrowLeft", JSKeys.ArrowLeft},
        {"ArrowRight", JSKeys.ArrowRight},
        {"ArrowUp", JSKeys.ArrowUp},
        {"Escape", JSKeys.Esc},
        {"PrintScreen", JSKeys.PrintScreen},
        {"Pause", JSKeys.Pause},

        {"NumpadComma", JSKeys.NumpadComma},
        {"NumpadDecimal", JSKeys.NumpadDecimal},
        {"NumpadDivide", JSKeys.Divide},
        {"NumpadEnter", JSKeys.Enter},
        {"NumpadEqual", JSKeys.Equal},
        {"NumpadMultiply", JSKeys.Multiply},
        {"NumpadSubtract", JSKeys.Subtract},
        {"NumpadAdd", JSKeys.Add},

        {"Clear", JSKeys.Clear},
        {"Select", JSKeys.Select},
        {"Execute", JSKeys.Execute},
    };
    private static readonly Dictionary<string, JSKeys> NumLock = new()
    {
        {"Numpad0", JSKeys.Insert},
        {"Numpad1", JSKeys.End},
        {"Numpad2", JSKeys.ArrowDown},
        {"Numpad3", JSKeys.PageDown},
        {"Numpad4", JSKeys.ArrowLeft},
        {"Numpad5", JSKeys.Clear},
        {"Numpad6", JSKeys.ArrowRight},
        {"Numpad7", JSKeys.Home},
        {"Numpad8", JSKeys.ArrowUp},
        {"Numpad9", JSKeys.PageUp},
        {"NumpadDecimal", JSKeys.Delete},
    };
    private static readonly Dictionary<string, JSKeys> ModifierKeys = new()
    {
        { "ControlLeft", JSKeys.Control},
        { "ControlRight", JSKeys.Control},

        { "ShiftLeft", JSKeys.Shift},
        { "ShiftRight", JSKeys.Shift},

        { "AltLeft", JSKeys.Alt},
        { "AltRight", JSKeys.Alt},

        { "MetaLeft", JSKeys.Meta},
        { "MetaRight", JSKeys.Meta},
    };

    /// <summary>
    /// Efetua a conversão do caractere capturado pelo JavaScript para o enumerador JSKeys
    /// </summary>
    /// <param name="jsKeyCode">tecla informada</param>
    /// <param name="control">Modificador Control pressionado</param>
    /// <param name="shift">Modificador Shift pressionado</param>
    /// <param name="alt">Modificador Alt pressionado</param>
    /// <returns>Enum Flag JSKeys
    /// <para>
    /// Caso tela não tenha sido mapeada retorna JSKeys.Unknown
    /// </para>
    /// </returns>
    public static JSKeys ConvertToKey(string? jsKeyCode, bool control, bool shift, bool alt, bool numLockActive, long? keycode)
    {
        JSKeys result = JSKeys.None;

        if (jsKeyCode is null)
            return result;

        if (string.IsNullOrEmpty(jsKeyCode) && keycode is not null && JsKeyCodeToKeys.TryGetValue(keycode.GetValueOrDefault(), out JSKeys keyFromKeyCode))
            return keyFromKeyCode;//nesse fluxo retorna direto sem validar os dados restantes, pois nao foi uma tecla pressionada no teclado, logo fluxo de shift, control e alt nao se aplicam.
        if (ModifierKeys.TryGetValue(jsKeyCode, out JSKeys modifierKey))
            result |= modifierKey;
        else if (numLockActive == false && NumLock.TryGetValue(jsKeyCode, out JSKeys value))
            result |= value;
        else if (JsToKeys.TryGetValue(jsKeyCode, out JSKeys jsKey))
            result |= jsKey;
        else
            return result |= JSKeys.Unknown;

        if (control)
            result |= JSKeys.Control;
        if (shift)
            result |= JSKeys.Shift;
        if (alt)
            result |= JSKeys.Alt;

        return result;
    }

    public static string ConvertToString(this JSKeys key)
    {
        return key.ToString();
    }

    /// <summary>
    /// Efetua a conversão do caractere capturado pelo JavaScript para o enumerador JSKeys
    /// </summary>
    /// <param name="jsKeyCode">tecla informada</param>
    /// <returns>Enum Flag JSKeys
    /// <para>Caso tela não tenha sido mapeada retorna JSKeys.Unknown</para>
    /// </returns>
    public static JSKeys ConvertToKey(this string jsKeyCode) => ConvertToKey(jsKeyCode, false, false, false, true,null);



    /// <summary>
    /// Verifica se a tecla pertence ao conjunto de teclas alfabéticas
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True = Pertence | False = Não Pertence</returns>
    public static bool IsLetter(this JSKeys key)
    {
        return key >= FIRST_ASCII && key <= LAST_ASCII;
    }


    /// <summary>
    /// Verifica se a tecla pertence ao conjunto de teclas Modificadoras
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True = Pertence | False = Não Pertence</returns>
    public static bool IsModifierKeys(this JSKeys key)
    {
        return key == JSKeys.Control || key == JSKeys.Shift || key == JSKeys.Alt || key == JSKeys.Meta;
    }


    #region Numeros

    /// <summary>
    /// Verifica se a tecla pertence ao conjunto de teclas númericas
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True = Pertence | False = Não Pertence</returns>
    public static bool IsNumber(this JSKeys key)
    {
        return key >= FIRST_DIGIT && key <= LAST_DIGIT || key >= FIRST_NUMPAD_DIGIT && key <= LAST_NUMPAD_DIGIT;
    }

    /// <summary>
    /// Retorna se a tecla informada corresponde ao número especificado.
    /// </summary>
    /// <param name="key">Tecla pressionada.</param>
    /// <param name="number">Número a ser comparado.</param>
    /// <returns>True se a tecla for o número informado; caso contrário, false.</returns>
    public static bool IsNumber(this JSKeys key, int number)
    {
        return GetNumber(key) == number;
    }

    /// <summary>
    /// Retorna o numero, dependendo da tecla informada.
    /// <para>se caso a tecla não for numerica retorna null</para>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int? GetNumber(this JSKeys key)
    {
        return key switch
        {
            JSKeys.D0 or JSKeys.NumPad0 => 0,
            JSKeys.D1 or JSKeys.NumPad1 => 1,
            JSKeys.D2 or JSKeys.NumPad2 => 2,
            JSKeys.D3 or JSKeys.NumPad3 => 3,
            JSKeys.D4 or JSKeys.NumPad4 => 4,
            JSKeys.D5 or JSKeys.NumPad5 => 5,
            JSKeys.D6 or JSKeys.NumPad6 => 6,
            JSKeys.D7 or JSKeys.NumPad7 => 7,
            JSKeys.D8 or JSKeys.NumPad8 => 8,
            JSKeys.D9 or JSKeys.NumPad9 => 9,
            _ => null
        };
    }


    #endregion
}
