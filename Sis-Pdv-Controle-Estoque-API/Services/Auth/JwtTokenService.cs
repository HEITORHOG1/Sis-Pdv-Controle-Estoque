using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = _configuration["Authentication:JwtSecret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpirationMinutes = int.Parse(_configuration["Authentication:TokenExpirationMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"] ?? "PDV-System",
                audience: _configuration["Authentication:Audience"] ?? "PDV-Users",
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var secretKey = _configuration["Authentication:JwtSecret"] ?? throw new InvalidOperationException("JWT Secret not configured");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = false // We don't validate lifetime here since we're dealing with expired tokens
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting principal from expired token");
                return null;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var secretKey = _configuration["Authentication:JwtSecret"] ?? throw new InvalidOperationException("JWT Secret not configured");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Authentication:Issuer"] ?? "PDV-System",
                    ValidAudience = _configuration["Authentication:Audience"] ?? "PDV-Users",
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return false;
            }
        }

        public DateTime GetTokenExpiration(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.ValidTo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting token expiration");
                return DateTime.MinValue;
            }
        }
    }
}