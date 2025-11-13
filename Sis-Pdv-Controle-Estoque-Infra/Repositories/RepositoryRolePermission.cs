using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryRolePermission : RepositoryBase<RolePermission, Guid>, IRepositoryRolePermission
    {
        private readonly PdvContext _context;

        public RepositoryRolePermission(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && !rp.IsDeleted)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId && !rp.IsDeleted)
                .Include(rp => rp.Role)
                .ToListAsync();
        }

        public async Task<bool> RoleHasPermissionAsync(Guid roleId, Guid permissionId)
        {
            return await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && !rp.IsDeleted);
        }
    }
}