using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Usuarios.RevogarSessao
{
    public class RevogarSessaoHandler : Notifiable, IRequestHandler<RevogarSessaoRequest, Response>
    {
        private readonly IRepositoryUserSession _repositoryUserSession;
        private readonly IRepositoryUsuario _repositoryUsuario;

        public RevogarSessaoHandler(
            IRepositoryUserSession repositoryUserSession,
            IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUserSession = repositoryUserSession;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(RevogarSessaoRequest request, CancellationToken cancellationToken)
        {
            // Verificar se usuário existe
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Verificar se a sessão existe e pertence ao usuário
            var sessao = await _repositoryUserSession.ObterPorIdAsync(request.SessionId);
            if (sessao == null)
            {
                AddNotification("Sessao", "Sessão não encontrada");
                return new Response(this);
            }

            if (sessao.UserId != request.UsuarioId)
            {
                AddNotification("Sessao", "Sessão não pertence ao usuário");
                return new Response(this);
            }

            // Revogar sessão
            var success = await _repositoryUserSession.RevokeSessionAsync(request.SessionId);
            if (!success)
            {
                AddNotification("Sessao", "Erro ao revogar sessão");
                return new Response(this);
            }

            return new Response(this, new { Message = "Sessão revogada com sucesso" });
        }
    }
}