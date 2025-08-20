using MediatR;

namespace Commands.Produto.ListarProduto
{
    public class ListarProdutoRequest : IRequest<Commands.Response>
    {
        public Guid? Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public bool isPerecivel { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeCategoria { get; set; }
        public int statusAtivo { get; set; }
    }
}
