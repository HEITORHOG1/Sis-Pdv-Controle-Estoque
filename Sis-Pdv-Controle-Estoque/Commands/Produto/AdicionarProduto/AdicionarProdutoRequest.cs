using MediatR;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoRequest : IRequest<Response>
    {
        public string CodBarras { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal MargemLucro { get; set; }
        public DateTime DataFabricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public int QuantidadeEstoqueProduto { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CategoriaId { get; set; }
        public int StatusAtivo { get; set; }
    }
}
