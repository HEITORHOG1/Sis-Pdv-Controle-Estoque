using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryRolePermission : IRepositoryBase<RolePermission, Guid>
    {
        Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId);
        Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId);
        Task<bool> RoleHasPermissionAsync(Guid roleId, Guid permissionId);
    }
}