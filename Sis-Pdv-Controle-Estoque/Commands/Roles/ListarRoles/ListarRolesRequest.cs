using MediatR;

namespace Commands.Roles.ListarRoles
{
    public class ListarRolesRequest : IRequest<Response>
    {
        public bool? IsActive { get; set; }
    }
}