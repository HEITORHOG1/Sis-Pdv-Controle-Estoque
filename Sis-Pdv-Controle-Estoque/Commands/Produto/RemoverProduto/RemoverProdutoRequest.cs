using MediatR;

namespace Commands.Produto.RemoverProduto
{
    public class RemoverProdutoRequest : IRequest<Response>
    {
        public RemoverProdutoRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
