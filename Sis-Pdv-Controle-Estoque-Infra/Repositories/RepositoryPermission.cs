using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryPermission : RepositoryBase<Permission, Guid>, IRepositoryPermission
    {
        private readonly PdvContext _context;

        public RepositoryPermission(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
        }

        public async Task<IEnumerable<Permission>> GetByResourceAndActionAsync(string resource, string action)
        {
            return await _context.Permissions
                .Where(p => p.Resource == resource && p.Action == action && !p.IsDeleted)
                .ToListAsync();
        }
    }
}