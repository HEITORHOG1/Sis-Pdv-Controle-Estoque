namespace Sis.Pdv.Blazor.Models.Auth;

/// <summary>
/// Resposta de autenticação da API.
/// </summary>
public sealed class AuthResult
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public UserInfo? User { get; init; }
}
