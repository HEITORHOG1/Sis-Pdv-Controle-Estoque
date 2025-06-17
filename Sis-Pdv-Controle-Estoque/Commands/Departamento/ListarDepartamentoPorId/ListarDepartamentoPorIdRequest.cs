
using MediatR;

namespace Commands.Departamento.ListarDepartamentoPorId
{
    public class ListarDepartamentoPorIdRequest : IRequest<ListarDepartamentoPorIdResponse>
    {
        public ListarDepartamentoPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
