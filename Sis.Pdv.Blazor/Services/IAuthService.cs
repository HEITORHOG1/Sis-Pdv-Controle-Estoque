using Sis.Pdv.Blazor.Models.Auth;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Contrato para autenticação via API REST.
/// </summary>
public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
