using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Roles.RemoverRole
{
    public class RemoverRoleHandler : Notifiable, IRequestHandler<RemoverRoleRequest, Response>
    {
        private readonly IRepositoryRole _repositoryRole;
        private readonly IRepositoryUserRole _repositoryUserRole;
        private readonly IRepositoryRolePermission _repositoryRolePermission;

        public RemoverRoleHandler(
            IRepositoryRole repositoryRole,
            IRepositoryUserRole repositoryUserRole,
            IRepositoryRolePermission repositoryRolePermission)
        {
            _repositoryRole = repositoryRole;
            _repositoryUserRole = repositoryUserRole;
            _repositoryRolePermission = repositoryRolePermission;
        }

        public async Task<Response> Handle(RemoverRoleRequest request, CancellationToken cancellationToken)
        {
            // Buscar role
            var role = await _repositoryRole.ObterPorIdAsync(request.Id);
            if (role == null)
            {
                AddNotification("Role", "Role não encontrada");
                return new Response(this);
            }

            // Verificar se a role está sendo usada por usuários
            var userRoles = await _repositoryUserRole.GetByRoleIdAsync(request.Id);
            if (userRoles.Any())
            {
                AddNotification("Role", "Não é possível remover uma role que está sendo usada por usuários");
                return new Response(this);
            }

            // Remover permissões da role
            var rolePermissions = await _repositoryRolePermission.GetByRoleIdAsync(request.Id);
            foreach (var rolePermission in rolePermissions)
            {
                await _repositoryRolePermission.RemoverAsync(rolePermission);
            }

            // Remover role (soft delete)
            role.IsDeleted = true;
            role.DeletedAt = DateTime.UtcNow;
            await _repositoryRole.EditarAsync(role);

            return new Response(this, new { Message = "Role removida com sucesso" });
        }
    }
}