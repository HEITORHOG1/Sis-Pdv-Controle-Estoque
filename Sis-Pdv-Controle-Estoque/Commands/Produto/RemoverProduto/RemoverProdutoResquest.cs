using MediatR;

namespace Commands.Produto.RemoverProduto
{
    public class RemoverProdutoResquest : IRequest<Response>
    {
        public RemoverProdutoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
