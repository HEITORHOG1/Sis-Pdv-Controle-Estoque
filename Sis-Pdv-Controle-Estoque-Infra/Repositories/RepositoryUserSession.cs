using Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class RepositoryUserSession : RepositoryBase<UserSession, Guid>, IRepositoryUserSession
    {
        private readonly PdvContext _pdvContext;

        public RepositoryUserSession(PdvContext context) : base(context)
        {
            _pdvContext = context;
        }

        public async Task<IEnumerable<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId)
        {
            return await _pdvContext.UserSessions
                .Where(s => s.UserId == userId && s.IsActive && s.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(s => s.LoginAt)
                .ToListAsync();
        }

        public async Task<UserSession?> GetBySessionTokenAsync(string sessionToken)
        {
            return await _pdvContext.UserSessions
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.IsActive);
        }

        public async Task<bool> RevokeSessionAsync(Guid sessionId)
        {
            var session = await _pdvContext.UserSessions.FindAsync(sessionId);
            if (session != null)
            {
                session.IsActive = false;
                session.LogoutAt = DateTime.UtcNow;
                await _pdvContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RevokeAllUserSessionsAsync(Guid userId)
        {
            var sessions = await _pdvContext.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
                session.LogoutAt = DateTime.UtcNow;
            }

            await _pdvContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetActiveSessionCountAsync(Guid userId)
        {
            return await _pdvContext.UserSessions
                .CountAsync(s => s.UserId == userId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
        }
    }
}