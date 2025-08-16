using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryUserRole : IRepositoryBase<UserRole, Guid>
    {
        Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId);
        Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
    }
}