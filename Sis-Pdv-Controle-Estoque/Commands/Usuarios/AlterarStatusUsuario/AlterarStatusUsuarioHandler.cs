using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Usuarios.AlterarStatusUsuario
{
    public class AlterarStatusUsuarioHandler : Notifiable, IRequestHandler<AlterarStatusUsuarioRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryUserSession _repositoryUserSession;

        public AlterarStatusUsuarioHandler(
            IRepositoryUsuario repositoryUsuario,
            IRepositoryUserSession repositoryUserSession)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryUserSession = repositoryUserSession;
        }

        public async Task<Response> Handle(AlterarStatusUsuarioRequest request, CancellationToken cancellationToken)
        {
            // Buscar usuário
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Atualizar status
            usuario.StatusAtivo = request.StatusAtivo;

            // Se desativando o usuário, revogar todas as sessões ativas
            if (!request.StatusAtivo)
            {
                var activeSessions = await _repositoryUserSession.GetActiveSessionsByUserIdAsync(request.UsuarioId);
                foreach (var session in activeSessions)
                {
                    session.IsActive = false;
                    session.LogoutAt = DateTime.UtcNow;
                    await _repositoryUserSession.EditarAsync(session);
                }

                // Limpar refresh token
                usuario.RefreshToken = null;
                usuario.RefreshTokenExpiryTime = null;
            }

            await _repositoryUsuario.EditarAsync(usuario);

            var result = new
            {
                Id = usuario.Id,
                Login = usuario.Login,
                StatusAtivo = usuario.StatusAtivo,
                Motivo = request.Motivo,
                UpdatedAt = usuario.UpdatedAt
            };

            return new Response(this, result);
        }
    }
}