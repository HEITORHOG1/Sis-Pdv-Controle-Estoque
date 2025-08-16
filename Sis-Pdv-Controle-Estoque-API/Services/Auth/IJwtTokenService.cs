using System.Security.Claims;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
        DateTime GetTokenExpiration(string token);
    }
}