using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryPermission : RepositoryBase<Permission, Guid>, IRepositoryPermission
    {
        public RepositoryPermission(PdvContext context) : base(context)
        {
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Set<Permission>()
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
        }

        public async Task<IEnumerable<Permission>> GetByResourceAndActionAsync(string resource, string action)
        {
            return await _context.Set<Permission>()
                .Where(p => p.Resource == resource && p.Action == action && !p.IsDeleted)
                .ToListAsync();
        }
    }
}