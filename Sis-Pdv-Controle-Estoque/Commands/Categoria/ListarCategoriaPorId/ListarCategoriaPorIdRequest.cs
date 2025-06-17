using MediatR;

namespace Commands.Categoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdRequest : IRequest<ListarCategoriaPorIdResponse>
    {
        public ListarCategoriaPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
