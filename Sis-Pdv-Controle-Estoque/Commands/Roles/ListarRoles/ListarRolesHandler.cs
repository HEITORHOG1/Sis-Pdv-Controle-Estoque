using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Roles.ListarRoles
{
    public class ListarRolesHandler : Notifiable, IRequestHandler<ListarRolesRequest, Response>
    {
        private readonly IRepositoryRole _repositoryRole;

        public ListarRolesHandler(IRepositoryRole repositoryRole)
        {
            _repositoryRole = repositoryRole;
        }

        public async Task<Response> Handle(ListarRolesRequest request, CancellationToken cancellationToken)
        {
            var roles = await _repositoryRole.ListarAsync();

            // Aplicar filtros
            if (request.IsActive.HasValue)
            {
                roles = roles.Where(r => r.IsActive == request.IsActive.Value);
            }

            var result = roles.Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt
            }).ToList();

            return new Response(this, result);
        }
    }
}