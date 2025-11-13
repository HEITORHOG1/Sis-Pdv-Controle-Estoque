using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Model;

namespace Commands.Usuarios.ListarUsuarios
{
    public class ListarUsuariosHandler : Notifiable, IRequestHandler<ListarUsuariosRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public ListarUsuariosHandler(IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(ListarUsuariosRequest request, CancellationToken cancellationToken)
        {
            var usuarios = await _repositoryUsuario.ListarAsync();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                usuarios = usuarios.Where(u => 
                    u.Login.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (u.Nome != null && u.Nome.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Email != null && u.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (request.StatusAtivo.HasValue)
            {
                usuarios = usuarios.Where(u => u.StatusAtivo == request.StatusAtivo.Value);
            }

            var totalCount = usuarios.Count();

            // Aplicar paginação
            var pagedUsuarios = usuarios
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new
                {
                    Id = u.Id,
                    Login = u.Login,
                    Email = u.Email,
                    Nome = u.Nome,
                    StatusAtivo = u.StatusAtivo,
                    LastLoginAt = u.LastLoginAt,
                    CreatedAt = u.CreatedAt
                })
                .ToList();

            var result = new PagedResult<object>
            {
                Items = pagedUsuarios,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return new Response(this, result);
        }
    }
}