using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Interfaces.Services;
using Model;

namespace Commands.Usuarios.Login
{
    public class LoginHandler : Notifiable, IRequestHandler<LoginRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryUserSession _repositoryUserSession;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginHandler(
            IRepositoryUsuario repositoryUsuario,
            IRepositoryUserSession repositoryUserSession,
            IPasswordService passwordService,
            IJwtTokenService jwtTokenService)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryUserSession = repositoryUserSession;
            _passwordService = passwordService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Response> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            // Buscar usuário por login
            var usuario = await _repositoryUsuario.GetByLoginAsync(request.Login);
            if (usuario == null)
            {
                AddNotification("Login", "Usuário ou senha inválidos");
                return new Response(this);
            }

            // Verificar se usuário está ativo
            if (!usuario.StatusAtivo)
            {
                AddNotification("Usuario", "Usuário inativo");
                return new Response(this);
            }

            // Verificar senha
            if (!_passwordService.VerifyPassword(request.Password, usuario.Senha))
            {
                AddNotification("Login", "Usuário ou senha inválidos");
                return new Response(this);
            }

            // Gerar tokens
            var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(usuario);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Atualizar refresh token no usuário
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            usuario.LastLoginAt = DateTime.UtcNow;

            await _repositoryUsuario.EditarAsync(usuario);

            // Criar sessão de usuário
            var userSession = new UserSession
            {
                UserId = usuario.Id,
                SessionToken = accessToken,
                IpAddress = request.IpAddress ?? "Unknown",
                UserAgent = request.UserAgent ?? "Unknown",
                LoginAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // Token expira em 1 hora
                IsActive = true
            };

            await _repositoryUserSession.AdicionarAsync(userSession);

            var result = new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600, // 1 hora em segundos
                TokenType = "Bearer",
                User = new
                {
                    Id = usuario.Id,
                    Login = usuario.Login,
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    StatusAtivo = usuario.StatusAtivo
                }
            };

            return new Response(this, result);
        }
    }
}