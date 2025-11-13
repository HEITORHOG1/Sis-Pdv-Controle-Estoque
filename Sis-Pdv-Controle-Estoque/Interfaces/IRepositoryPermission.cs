using Interfaces.Repositories.Base;

namespace Interfaces
{
    public interface IRepositoryPermission : IRepositoryBase<Permission, Guid>
    {
        Task<Permission?> GetByNameAsync(string name);
        Task<IEnumerable<Permission>> GetByResourceAndActionAsync(string resource, string action);
    }
}