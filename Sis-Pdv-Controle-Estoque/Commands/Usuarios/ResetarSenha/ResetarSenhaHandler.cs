using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Interfaces.Services;

namespace Commands.Usuarios.ResetarSenha
{
    public class ResetarSenhaHandler : Notifiable, IRequestHandler<ResetarSenhaRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryUserSession _repositoryUserSession;
        private readonly IPasswordService _passwordService;

        public ResetarSenhaHandler(
            IRepositoryUsuario repositoryUsuario,
            IRepositoryUserSession repositoryUserSession,
            IPasswordService passwordService)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryUserSession = repositoryUserSession;
            _passwordService = passwordService;
        }

        public async Task<Response> Handle(ResetarSenhaRequest request, CancellationToken cancellationToken)
        {
            // Buscar usuário
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Validar confirmação de senha
            if (request.NovaSenha != request.ConfirmarSenha)
            {
                AddNotification("ConfirmarSenha", "Confirmação de senha não confere");
                return new Response(this);
            }

            // Criptografar nova senha
            var hashedPassword = _passwordService.HashPassword(request.NovaSenha);

            // Atualizar senha
            usuario.Senha = hashedPassword;

            // Se forçar troca de senha, limpar refresh token para forçar novo login
            if (request.ForcarTrocaSenha)
            {
                usuario.RefreshToken = null;
                usuario.RefreshTokenExpiryTime = null;

                // Revogar todas as sessões ativas
                var activeSessions = await _repositoryUserSession.GetActiveSessionsByUserIdAsync(request.UsuarioId);
                foreach (var session in activeSessions)
                {
                    session.IsActive = false;
                    session.LogoutAt = DateTime.UtcNow;
                    await _repositoryUserSession.EditarAsync(session);
                }
            }

            await _repositoryUsuario.EditarAsync(usuario);

            var result = new
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Message = "Senha resetada com sucesso",
                ForcarTrocaSenha = request.ForcarTrocaSenha,
                UpdatedAt = usuario.UpdatedAt
            };

            return new Response(this, result);
        }
    }
}