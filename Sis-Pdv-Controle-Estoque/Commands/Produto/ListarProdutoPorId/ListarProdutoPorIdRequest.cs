
using MediatR;

namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdRequest : IRequest<ListarProdutoPorIdResponse>
    {
        public ListarProdutoPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
