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
    }
}
