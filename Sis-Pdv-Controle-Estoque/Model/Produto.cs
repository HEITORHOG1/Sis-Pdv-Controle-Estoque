using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Produto")]
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
            this.codBarras = codBarras;
            this.nomeProduto = nomeProduto;
            this.descricaoProduto = descricaoProduto;
            this.precoCusto = precoCusto;
            this.precoVenda = precoVenda;
            this.margemLucro = margemLucro;
            this.dataFabricao = dataFabricao;
            this.dataVencimento = dataVencimento;
            this.quatidadeEstoqueProduto = quatidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            this.statusAtivo = statusAtivo;
        }

        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public decimal precoCusto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal margemLucro { get; set; }
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public Guid FornecedorId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid CategoriaId { get; set; }
        public int statusAtivo { get; set; }

        internal void AlterarProduto(Guid id, string codBarras, string nomeProduto, string descricaoProduto,
            decimal precoCusto, decimal precoVenda, decimal margemLucro, DateTime dataFabricao,
            DateTime dataVencimento, int quatidadeEstoqueProduto,
            Guid FornecedorId, Guid CategoriaId, int statusAtivo)
        {
            Id = id;
            this.codBarras = codBarras;
            this.nomeProduto = nomeProduto;
            this.descricaoProduto = descricaoProduto;
            this.precoCusto = precoCusto;
            this.precoVenda = precoVenda;
            this.margemLucro = margemLucro;
            this.dataFabricao = dataFabricao;
            this.dataVencimento = dataVencimento;
            this.quatidadeEstoqueProduto = quatidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            this.statusAtivo = statusAtivo;
        }
        internal void AtualizarEstoque(Guid id, int quatidadeEstoqueProduto)
        {
            Id = id;
            this.quatidadeEstoqueProduto = quatidadeEstoqueProduto;
        }
    }
}
