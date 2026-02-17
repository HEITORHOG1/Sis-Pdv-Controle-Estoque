using MediatR;

namespace Commands.Produto.AtualizarEstoque
{
    public class AtualizarEstoqueRequest : IRequest<Response>
    {

        public Guid Id { get; set; }
        public int QuantidadeEstoqueProduto { get; set; }
    }
}
