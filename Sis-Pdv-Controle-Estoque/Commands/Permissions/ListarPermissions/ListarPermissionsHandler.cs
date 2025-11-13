using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Permissions.ListarPermissions
{
    public class ListarPermissionsHandler : Notifiable, IRequestHandler<ListarPermissionsRequest, Response>
    {
        private readonly IRepositoryPermission _repositoryPermission;

        public ListarPermissionsHandler(IRepositoryPermission repositoryPermission)
        {
            _repositoryPermission = repositoryPermission;
        }

        public async Task<Response> Handle(ListarPermissionsRequest request, CancellationToken cancellationToken)
        {
            var permissions = await _repositoryPermission.ListarAsync();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(request.Resource))
            {
                permissions = permissions.Where(p => p.Resource.Contains(request.Resource, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Action))
            {
                permissions = permissions.Where(p => p.Action.Contains(request.Action, StringComparison.OrdinalIgnoreCase));
            }

            var result = permissions.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Resource = p.Resource,
                Action = p.Action,
                CreatedAt = p.CreatedAt
            }).ToList();

            return new Response(this, result);
        }
    }
}