namespace Sis.Pdv.Blazor.Models.Auth;

/// <summary>
/// Payload para autenticação na API.
/// </summary>
public sealed class LoginRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}
