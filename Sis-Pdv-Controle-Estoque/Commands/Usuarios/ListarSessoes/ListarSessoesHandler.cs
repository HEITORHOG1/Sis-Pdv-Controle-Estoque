using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Usuarios.ListarSessoes
{
    public class ListarSessoesHandler : Notifiable, IRequestHandler<ListarSessoesRequest, Response>
    {
        private readonly IRepositoryUserSession _repositoryUserSession;
        private readonly IRepositoryUsuario _repositoryUsuario;

        public ListarSessoesHandler(
            IRepositoryUserSession repositoryUserSession,
            IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUserSession = repositoryUserSession;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(ListarSessoesRequest request, CancellationToken cancellationToken)
        {
            // Verificar se usuário existe
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Obter sessões ativas do usuário
            var sessoes = await _repositoryUserSession.GetActiveSessionsByUserIdAsync(request.UsuarioId);

            var result = sessoes.Select(s => new
            {
                Id = s.Id,
                SessionToken = s.SessionToken.Substring(0, Math.Min(10, s.SessionToken.Length)) + "...", // Mostrar apenas parte do token por segurança
                IpAddress = s.IpAddress,
                UserAgent = s.UserAgent,
                LoginAt = s.LoginAt,
                ExpiresAt = s.ExpiresAt,
                IsActive = s.IsActive
            });

            return new Response(this, result);
        }
    }
}