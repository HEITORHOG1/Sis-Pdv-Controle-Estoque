using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryRole : RepositoryBase<Role, Guid>, IRepositoryRole
    {
        public RepositoryRole(PdvContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Role>()
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Role>()
                .Where(r => r.IsActive && !r.IsDeleted)
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RolePermission>()
                .Where(rp => rp.RoleId == roleId && !rp.IsDeleted)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync(cancellationToken);
        }
    }
}