using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryUsuario : RepositoryBase<Usuario, Guid>, IRepositoryUsuario
    {
        private readonly PdvContext _context;

        public RepositoryUsuario(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByLoginAsync(string login)
        {
            return await _context.Usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Login == login && !u.IsDeleted);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<Usuario?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && !u.IsDeleted);
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .Include(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Select(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => !rp.IsDeleted)
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => !rp.IsDeleted)
                .AnyAsync(rp => rp.Permission.Name == permission);
        }
    }
}
