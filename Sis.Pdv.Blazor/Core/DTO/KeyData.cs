using System.Text.Json.Serialization;

namespace Sis.Pdv.Blazor.Core.DTO;

public class KeyData
{
    public string Key { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int KeyCode { get; set; }
    public bool CtrlKey { get; set; }
    public bool ShiftKey { get; set; }
    public bool AltKey { get; set; }
    public bool MetaKey { get; set; }
    public bool Repeat { get; set; }
}
