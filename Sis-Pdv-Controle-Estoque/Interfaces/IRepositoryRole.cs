using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryRole : IRepositoryBase<Role, Guid>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
    }
}