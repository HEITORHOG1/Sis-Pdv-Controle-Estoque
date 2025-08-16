using MediatR;

namespace Commands.Roles.AtualizarRole
{
    public class AtualizarRoleRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}