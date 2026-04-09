using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Usuarios.ListarUsuarioPorId
{
    public class ListarUsuarioPorIdHandler : Notifiable, IRequestHandler<ListarUsuarioPorIdRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public ListarUsuarioPorIdHandler(IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(ListarUsuarioPorIdRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return new Response(this);
            }

            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.Id, cancellationToken);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado.");
                return new Response(this);
            }

            // Retornar sem expor dados sensíveis
            var result = new
            {
                usuario.Id,
                usuario.Login,
                usuario.Email,
                usuario.StatusAtivo,
                usuario.CreatedAt,
                usuario.UpdatedAt
            };

            return new Response(this, result);
        }
    }
}
