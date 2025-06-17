using MediatR;

namespace Commands.Produto.AlterarProduto
{
    public class AlterarProdutoRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public decimal precoCusto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal margemLucro { get; set; }
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CategoriaId { get; set; }
        public int statusAtivo { get; set; }
    }
}
