using MediatR;

namespace Commands.Cupom.ListarCupomPorId
{
    public class ListarCupomPorIdRequest : IRequest<Response>
    {
        public ListarCupomPorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
