using Sis_Pdv_Controle_Estoque_API.Models.Auth;
using System.Security.Claims;
using Interfaces;
using Interfaces.Services;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryUsuario _userRepository;
        private readonly Interfaces.Services.IJwtTokenService _jwtTokenService;
        private readonly IPasswordService _passwordService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            IRepositoryUsuario userRepository,
            IJwtTokenService jwtTokenService,
            IPasswordService passwordService,
            IPermissionService permissionService,
            ILogger<AuthenticationService> logger,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
            _permissionService = permissionService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<AuthResult> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
                
                if (user == null || !user.StatusAtivo)
                {
                    _logger.LogWarning("Authentication failed for user {Login}: User not found or inactive", request.Login);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Credenciais inválidas"
                    };
                }

                if (!_passwordService.VerifyPassword(request.Password, user.Senha))
                {
                    _logger.LogWarning("Authentication failed for user {Login}: Invalid password", request.Login);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Credenciais inválidas"
                    };
                }

                // Get user roles and permissions
                var roles = await _permissionService.GetUserRolesAsync(user.Id);
                var permissions = await _permissionService.GetUserPermissionsAsync(user.Id);

                // Create claims
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Login),
                    new(ClaimTypes.Email, user.Email),
                    new("nome", user.Nome)
                };

                // Add role claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Add permission claims
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("permission", permission));
                }

                // Generate tokens
                var accessToken = _jwtTokenService.GenerateAccessToken(claims);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                // Update user with refresh token
                var refreshTokenExpirationDays = int.Parse(_configuration["Authentication:RefreshTokenExpirationDays"] ?? "7");
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
                user.LastLoginAt = DateTime.UtcNow;

                _userRepository.Editar(user);

                _logger.LogInformation("User {Login} authenticated successfully", request.Login);

                return new AuthResult
                {
                    Success = true,
                    Message = "Autenticação realizada com sucesso",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Email = user.Email,
                        Nome = user.Nome,
                        Roles = roles,
                        Permissions = permissions
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for user {Login}", request.Login);
                return new AuthResult
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
                
                if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Refresh token validation failed: Token not found or expired");
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Token de refresh inválido ou expirado"
                    };
                }

                // Get user roles and permissions
                var roles = await _permissionService.GetUserRolesAsync(user.Id);
                var permissions = await _permissionService.GetUserPermissionsAsync(user.Id);

                // Create claims
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Login),
                    new(ClaimTypes.Email, user.Email),
                    new("nome", user.Nome)
                };

                // Add role claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Add permission claims
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("permission", permission));
                }

                // Generate new tokens
                var newAccessToken = _jwtTokenService.GenerateAccessToken(claims);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                // Update user with new refresh token
                var refreshTokenExpirationDays = int.Parse(_configuration["Authentication:RefreshTokenExpirationDays"] ?? "7");
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

                _userRepository.Editar(user);

                _logger.LogInformation("Token refreshed successfully for user {UserId}", user.Id);

                return new AuthResult
                {
                    Success = true,
                    Message = "Token renovado com sucesso",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(newAccessToken),
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Email = user.Email,
                        Nome = user.Nome,
                        Roles = roles,
                        Permissions = permissions
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return new AuthResult
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
                
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    _userRepository.Editar(user);
                    
                    _logger.LogInformation("Refresh token revoked for user {UserId}", user.Id);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking refresh token");
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_jwtTokenService.ValidateToken(token));
        }
    }
}