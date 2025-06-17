
using MediatR;

namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdRequest : IRequest<ListarFornecedorPorIdResponse>
    {
        public ListarFornecedorPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
