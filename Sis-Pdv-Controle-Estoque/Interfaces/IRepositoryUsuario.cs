using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepositoryUsuario : IRepositoryBase<Usuario, Guid>
    {
        Task<Usuario?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
        Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Usuario?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default);
        Task<int> CountActiveUsersAsync(CancellationToken cancellationToken = default);
        
        // Métodos específicos para controle de status
        IQueryable<Usuario> ListarTodosAtivos(params Expression<Func<Usuario, object>>[] includeProperties);
        IQueryable<Usuario> ListarInativos(params Expression<Func<Usuario, object>>[] includeProperties);
        void Desativar(Usuario usuario);
        void Ativar(Usuario usuario);
        Task<bool> DesativarAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> AtivarAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
