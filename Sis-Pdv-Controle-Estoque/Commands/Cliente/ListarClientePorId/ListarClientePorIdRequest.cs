using MediatR;

namespace Commands.Cliente.ListarClientePorId
{
    public class ListarClientePorIdRequest : IRequest<Response>
    {
        public ListarClientePorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
