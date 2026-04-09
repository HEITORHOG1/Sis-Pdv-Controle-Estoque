using MediatR;

namespace Commands.Usuarios.ListarUsuarioPorId
{
    public class ListarUsuarioPorIdRequest : IRequest<Response>
    {
        public ListarUsuarioPorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
