using MediatR;

namespace Commands.Roles.CriarRole
{
    public class CriarRoleRequest : IRequest<Response>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
        public bool IsActive { get; set; } = true;
    }
}