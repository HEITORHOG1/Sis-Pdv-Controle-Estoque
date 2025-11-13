using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepositoryUsuario : IRepositoryBase<Usuario, Guid>
    {
        Task<Usuario?> GetByLoginAsync(string login);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByRefreshTokenAsync(string refreshToken);
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        Task<int> CountActiveUsersAsync();
        
        // Métodos específicos para controle de status
        IQueryable<Usuario> ListarTodosAtivos(params Expression<Func<Usuario, object>>[] includeProperties);
        IQueryable<Usuario> ListarInativos(params Expression<Func<Usuario, object>>[] includeProperties);
        void Desativar(Usuario usuario);
        void Ativar(Usuario usuario);
        Task<bool> DesativarAsync(Guid id);
        Task<bool> AtivarAsync(Guid id);
    }
}
