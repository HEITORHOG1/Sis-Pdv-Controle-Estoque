using System.Security.Claims;
using Interfaces.Services;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IJwtTokenService : Interfaces.Services.IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        DateTime GetTokenExpiration(string token);
    }
}