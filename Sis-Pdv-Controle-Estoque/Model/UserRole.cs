using Model.Base;

namespace Model
{
    public class UserRole : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        // Navigation properties
        public virtual Usuario User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}