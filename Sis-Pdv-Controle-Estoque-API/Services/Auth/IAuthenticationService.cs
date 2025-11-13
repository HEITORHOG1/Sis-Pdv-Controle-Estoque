using Sis_Pdv_Controle_Estoque_API.Models.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthResult> AuthenticateAsync(LoginRequest request);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}