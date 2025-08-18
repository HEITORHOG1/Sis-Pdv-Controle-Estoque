using Model.Base;

namespace Model
{
    public class Permission : EntityBase
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}