using MediatR;

namespace Commands.Roles.RemoverRole
{
    public class RemoverRoleRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
    }
}