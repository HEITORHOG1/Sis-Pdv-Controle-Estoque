using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryUserRole : RepositoryBase<UserRole, Guid>, IRepositoryUserRole
    {
        public RepositoryUserRole(PdvContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId)
        {
            return await _context.Set<UserRole>()
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .Include(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId)
        {
            return await _context.Set<UserRole>()
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId && !ur.IsDeleted);
        }

        public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.Set<UserRole>()
                .Where(ur => ur.RoleId == roleId && !ur.IsDeleted)
                .Include(ur => ur.User)
                .ToListAsync();
        }
    }
}