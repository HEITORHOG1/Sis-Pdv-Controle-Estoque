using MediatR;

namespace Commands.Cupom.RemoverCupom
{
    public class RemoverCupomRequest : IRequest<Response>
    {
        public RemoverCupomRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
