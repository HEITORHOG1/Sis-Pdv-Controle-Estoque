using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Roles.CriarRole
{
    public class CriarRoleHandler : Notifiable, IRequestHandler<CriarRoleRequest, Response>
    {
        private readonly IRepositoryRole _repositoryRole;
        private readonly IRepositoryPermission _repositoryPermission;
        private readonly IRepositoryRolePermission _repositoryRolePermission;

        public CriarRoleHandler(
            IRepositoryRole repositoryRole,
            IRepositoryPermission repositoryPermission,
            IRepositoryRolePermission repositoryRolePermission)
        {
            _repositoryRole = repositoryRole;
            _repositoryPermission = repositoryPermission;
            _repositoryRolePermission = repositoryRolePermission;
        }

        public async Task<Response> Handle(CriarRoleRequest request, CancellationToken cancellationToken)
        {
            // Validar requisição
            var validator = new CriarRoleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }
                return new Response(this);
            }

            // Verificar se role já existe
            var existingRoles = await _repositoryRole.ListarAsync();
            if (existingRoles.Any(r => r.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                AddNotification("Name", "Role com este nome já existe");
                return new Response(this);
            }

            // Verificar se as permissões existem
            if (request.PermissionIds.Any())
            {
                var existingPermissions = await _repositoryPermission.ListarAsync();
                var existingPermissionIds = existingPermissions.Select(p => p.Id).ToList();
                var invalidPermissionIds = request.PermissionIds.Except(existingPermissionIds).ToList();

                if (invalidPermissionIds.Any())
                {
                    AddNotification("PermissionIds", $"Permissões inválidas: {string.Join(", ", invalidPermissionIds)}");
                    return new Response(this);
                }
            }

            // Criar role
            var role = new Model.Role
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };

            role = await _repositoryRole.AdicionarAsync(role);

            // Adicionar permissões à role
            if (request.PermissionIds.Any())
            {
                foreach (var permissionId in request.PermissionIds)
                {
                    var rolePermission = new Model.RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId
                    };
                    await _repositoryRolePermission.AdicionarAsync(rolePermission);
                }
            }

            var result = new
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                PermissionIds = request.PermissionIds
            };

            return new Response(this, result);
        }
    }
}