using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Usuarios.AtribuirRoles
{
    public class AtribuirRolesHandler : Notifiable, IRequestHandler<AtribuirRolesRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryRole _repositoryRole;
        private readonly IRepositoryUserRole _repositoryUserRole;

        public AtribuirRolesHandler(
            IRepositoryUsuario repositoryUsuario,
            IRepositoryRole repositoryRole,
            IRepositoryUserRole repositoryUserRole)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryRole = repositoryRole;
            _repositoryUserRole = repositoryUserRole;
        }

        public async Task<Response> Handle(AtribuirRolesRequest request, CancellationToken cancellationToken)
        {
            // Verificar se usuário existe
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Verificar se as roles existem
            if (request.RoleIds.Any())
            {
                var existingRoles = await _repositoryRole.ListarAsync();
                var existingRoleIds = existingRoles.Select(r => r.Id).ToList();
                var invalidRoleIds = request.RoleIds.Except(existingRoleIds).ToList();

                if (invalidRoleIds.Any())
                {
                    AddNotification("RoleIds", $"Roles inválidas: {string.Join(", ", invalidRoleIds)}");
                    return new Response(this);
                }
            }

            // Remover roles existentes do usuário
            var existingUserRoles = await _repositoryUserRole.ListarAsync();
            var userRolesToRemove = existingUserRoles.Where(ur => ur.UserId == request.UsuarioId).ToList();

            foreach (var userRole in userRolesToRemove)
            {
                await _repositoryUserRole.RemoverAsync(userRole.Id);
            }

            // Adicionar novas roles
            foreach (var roleId in request.RoleIds)
            {
                var userRole = new Model.UserRole
                {
                    UserId = request.UsuarioId,
                    RoleId = roleId
                };
                await _repositoryUserRole.AdicionarAsync(userRole);
            }

            return new Response(this, new { Message = "Roles atribuídas com sucesso" });
        }
    }
}