namespace Sis.Pdv.Blazor.Models.Auth;

/// <summary>
/// Informações do usuário autenticado.
/// </summary>
public sealed class UserInfo
{
    public Guid Id { get; init; }
    public string Login { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public List<string> Roles { get; init; } = [];
    public List<string> Permissions { get; init; } = [];
}
