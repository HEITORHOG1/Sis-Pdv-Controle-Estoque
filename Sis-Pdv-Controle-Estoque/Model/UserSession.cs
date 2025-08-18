using Model.Base;

namespace Model
{
    public class UserSession : EntityBase
    {
        public Guid UserId { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime LoginAt { get; set; }
        public DateTime? LogoutAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Usuario User { get; set; } = null!;
    }
}