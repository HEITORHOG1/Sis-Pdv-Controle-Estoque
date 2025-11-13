using MediatR;

namespace Commands.Permissions.ListarPermissions
{
    public class ListarPermissionsRequest : IRequest<Response>
    {
        public string? Resource { get; set; }
        public string? Action { get; set; }
    }
}