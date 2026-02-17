using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryRole : IRepositoryBase<Role, Guid>
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}