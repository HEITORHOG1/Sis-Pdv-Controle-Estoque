using MediatR;

namespace Commands.Usuarios.AtribuirRoles
{
    public class AtribuirRolesRequest : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}