using MediatR;

namespace Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdRequest : IRequest<ListarColaboradorPorIdResponse>
    {
        public ListarColaboradorPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
