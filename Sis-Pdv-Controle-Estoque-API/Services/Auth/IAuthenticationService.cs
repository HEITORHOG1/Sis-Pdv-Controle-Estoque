using Sis_Pdv_Controle_Estoque_API.Models.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthResult> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<AuthResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}