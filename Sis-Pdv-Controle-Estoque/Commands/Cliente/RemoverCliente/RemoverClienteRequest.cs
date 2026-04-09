using MediatR;

namespace Commands.Cliente.RemoverCliente
{
    public class RemoverClienteRequest : IRequest<Response>
    {
        public RemoverClienteRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
