using Model;

namespace Interfaces.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(Usuario usuario);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        Task<Usuario?> GetUserFromTokenAsync(string token);
    }
}