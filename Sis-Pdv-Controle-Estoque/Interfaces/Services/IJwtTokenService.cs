using Model;
using System.Security.Claims;

namespace Interfaces.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(Usuario usuario);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        Task<Usuario?> GetUserFromTokenAsync(string token);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        DateTime GetTokenExpiration(string token);
    }
}