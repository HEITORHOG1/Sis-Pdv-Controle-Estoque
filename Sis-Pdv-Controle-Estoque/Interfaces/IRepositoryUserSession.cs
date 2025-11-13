namespace Interfaces
{
    public interface IRepositoryUserSession : IRepositoryBase<UserSession, Guid>
    {
        Task<IEnumerable<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId);
        Task<UserSession?> GetBySessionTokenAsync(string sessionToken);
        Task<bool> RevokeSessionAsync(Guid sessionId);
        Task<bool> RevokeAllUserSessionsAsync(Guid userId);
        Task<int> GetActiveSessionCountAsync(Guid userId);
    }
}