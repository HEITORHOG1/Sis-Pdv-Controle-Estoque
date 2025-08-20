using MediatR;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoRequest : IRequest<Response>
    {
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public bool isPerecivel { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CategoriaId { get; set; }
        public int statusAtivo { get; set; }
    }
}
