using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Model;

namespace Commands.Roles.AtualizarRole
{
    public class AtualizarRoleHandler : Notifiable, IRequestHandler<AtualizarRoleRequest, Response>
    {
        private readonly IRepositoryRole _repositoryRole;
        private readonly IRepositoryRolePermission _repositoryRolePermission;
        private readonly IRepositoryPermission _repositoryPermission;

        public AtualizarRoleHandler(
            IRepositoryRole repositoryRole,
            IRepositoryRolePermission repositoryRolePermission,
            IRepositoryPermission repositoryPermission)
        {
            _repositoryRole = repositoryRole;
            _repositoryRolePermission = repositoryRolePermission;
            _repositoryPermission = repositoryPermission;
        }

        public async Task<Response> Handle(AtualizarRoleRequest request, CancellationToken cancellationToken)
        {
            // Buscar role existente
            var role = await _repositoryRole.ObterPorIdAsync(request.Id);
            if (role == null)
            {
                AddNotification("Role", "Role não encontrada");
                return new Response(this);
            }

            // Verificar se já existe outra role com o mesmo nome
            var existingRole = await _repositoryRole.GetByNameAsync(request.Name);
            if (existingRole != null && existingRole.Id != request.Id)
            {
                AddNotification("Name", "Já existe uma role com este nome");
                return new Response(this);
            }

            // Atualizar dados da role
            role.Name = request.Name;
            role.Description = request.Description;
            role.IsActive = request.IsActive;

            await _repositoryRole.EditarAsync(role);

            // Atualizar permissões da role
            if (request.PermissionIds.Any())
            {
                // Remover permissões existentes
                var existingRolePermissions = await _repositoryRolePermission.GetByRoleIdAsync(role.Id);
                foreach (var rolePermission in existingRolePermissions)
                {
                    await _repositoryRolePermission.RemoverAsync(rolePermission);
                }

                // Adicionar novas permissões
                foreach (var permissionId in request.PermissionIds)
                {
                    var permission = await _repositoryPermission.ObterPorIdAsync(permissionId);
                    if (permission != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permissionId
                        };
                        await _repositoryRolePermission.AdicionarAsync(rolePermission);
                    }
                }
            }

            var result = new
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                UpdatedAt = role.UpdatedAt
            };

            return new Response(this, result);
        }
    }
}