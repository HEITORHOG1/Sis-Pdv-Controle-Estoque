namespace Model
{
    public class Produto : EntityBase
    {


        public Produto()
        {
            Fornecedor = new Fornecedor();
            Categoria = new Categoria();
        }
        public Produto(
            string codBarras,
            string nomeProduto,
            string descricaoProduto,
            decimal precoCusto,
            decimal precoVenda,
            decimal margemLucro,
            DateTime dataFabricao,
            DateTime dataVencimento,
            int quatidadeEstoqueProduto,
            Guid FornecedorId,
            Guid CategoriaId,
            int statusAtivo)
        {
            CodBarras = codBarras;
            NomeProduto = nomeProduto;
            DescricaoProduto = descricaoProduto;
            PrecoCusto = precoCusto;
            PrecoVenda = precoVenda;
            MargemLucro = margemLucro;
            DataFabricao = dataFabricao;
            DataVencimento = dataVencimento;
            QuatidadeEstoqueProduto = quatidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            StatusAtivo = statusAtivo;
        }

        public string CodBarras { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal MargemLucro { get; set; }
        public DateTime DataFabricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public int QuatidadeEstoqueProduto { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public Guid FornecedorId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid CategoriaId { get; set; }
        public int StatusAtivo { get; set; }

        internal void AlterarProduto(Guid id, string codBarras, string nomeProduto, string descricaoProduto,
            decimal precoCusto, decimal precoVenda, decimal margemLucro, DateTime dataFabricao,
            DateTime dataVencimento, int quatidadeEstoqueProduto,
            Guid FornecedorId, Guid CategoriaId, int statusAtivo)
        {
            Id = id;
            CodBarras = codBarras;
            NomeProduto = nomeProduto;
            DescricaoProduto = descricaoProduto;
            PrecoCusto = precoCusto;
            PrecoVenda = precoVenda;
            MargemLucro = margemLucro;
            DataFabricao = dataFabricao;
            DataVencimento = dataVencimento;
            QuatidadeEstoqueProduto = quatidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            StatusAtivo = statusAtivo;
        }
        internal void AtualizarEstoque(Guid id, int quatidadeEstoqueProduto)
        {
            Id = id;
            QuatidadeEstoqueProduto = quatidadeEstoqueProduto;
        }
    }
}
