using Model.Base;

namespace Model
{
    public class AuditLog : EntityBase
    {
        public string EntityName { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Changes { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        // Navigation property
        public virtual Usuario User { get; set; } = null!;
    }
}