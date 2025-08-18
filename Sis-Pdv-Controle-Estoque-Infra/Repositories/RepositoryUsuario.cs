using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using System.Linq.Expressions;

namespace Repositories
{
    public class RepositoryUsuario : RepositoryBase<Usuario, Guid>, IRepositoryUsuario
    {
        private readonly PdvContext _context;

        public RepositoryUsuario(PdvContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Override do método Listar para filtrar usuários ativos e não deletados
        /// </summary>
        public override IQueryable<Usuario> Listar(params Expression<Func<Usuario, object>>[] includeProperties)
        {
            IQueryable<Usuario> query = _context.Set<Usuario>()
                .Where(x => !x.IsDeleted && x.StatusAtivo);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista usuários incluindo inativos mas excluindo deletados
        /// </summary>
        public IQueryable<Usuario> ListarTodosAtivos(params Expression<Func<Usuario, object>>[] includeProperties)
        {
            IQueryable<Usuario> query = _context.Set<Usuario>().Where(x => !x.IsDeleted);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Lista apenas usuários inativos (StatusAtivo = false) mas não deletados
        /// </summary>
        public IQueryable<Usuario> ListarInativos(params Expression<Func<Usuario, object>>[] includeProperties)
        {
            IQueryable<Usuario> query = _context.Set<Usuario>()
                .Where(x => !x.IsDeleted && !x.StatusAtivo);

            if (includeProperties.Any())
            {
                return Include(query, includeProperties);
            }

            return query;
        }

        /// <summary>
        /// Desativa um usuário (StatusAtivo = false) sem deletá-lo
        /// </summary>
        public void Desativar(Usuario usuario)
        {
            usuario.StatusAtivo = false;
            usuario.UpdatedAt = DateTime.UtcNow;
            _context.Entry(usuario).State = EntityState.Modified;
        }

        /// <summary>
        /// Ativa um usuário (StatusAtivo = true)
        /// </summary>
        public void Ativar(Usuario usuario)
        {
            usuario.StatusAtivo = true;
            usuario.UpdatedAt = DateTime.UtcNow;
            _context.Entry(usuario).State = EntityState.Modified;
        }

        /// <summary>
        /// Desativa um usuário pelo ID
        /// </summary>
        public async Task<bool> DesativarAsync(Guid id)
        {
            var usuario = await ObterPorIdAsync(id);
            if (usuario != null)
            {
                Desativar(usuario);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ativa um usuário pelo ID
        /// </summary>
        public async Task<bool> AtivarAsync(Guid id)
        {
            var usuario = await ListarTodosAtivos().FirstOrDefaultAsync(x => x.Id == id);
            if (usuario != null)
            {
                Ativar(usuario);
                return true;
            }
            return false;
        }

        public async Task<Usuario?> GetByLoginAsync(string login)
        {
            return await _context.Usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Login == login && !u.IsDeleted && u.StatusAtivo);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted && u.StatusAtivo);
        }

        public async Task<Usuario?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && !u.IsDeleted && u.StatusAtivo);
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

        public async Task<int> CountActiveUsersAsync()
        {
            return await _context.Usuarios
                .CountAsync(u => u.StatusAtivo && !u.IsDeleted);
        }

        /// <summary>
        /// Método auxiliar para Include (precisa ser protegido para override funcionar)
        /// </summary>
        protected new IQueryable<Usuario> Include(IQueryable<Usuario> query, params Expression<Func<Usuario, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            return query;
        }
    }
}
