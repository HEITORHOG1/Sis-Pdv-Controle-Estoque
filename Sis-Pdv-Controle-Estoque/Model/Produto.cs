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
            bool isPerecivel,
            Guid FornecedorId,
            Guid CategoriaId,
            int statusAtivo)
        {
            CodBarras = codBarras;
            NomeProduto = nomeProduto;
            DescricaoProduto = descricaoProduto;
            IsPerecivel = isPerecivel;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            StatusAtivo = statusAtivo;
        }

        public string CodBarras { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public bool IsPerecivel { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public Guid FornecedorId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid CategoriaId { get; set; }
        public int StatusAtivo { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public virtual InventoryBalance? InventoryBalance { get; set; }

        internal void AlterarProduto(Guid id, string codBarras, string nomeProduto, string descricaoProduto,
            bool isPerecivel, Guid FornecedorId, Guid CategoriaId, int statusAtivo)
        {
            Id = id;
            CodBarras = codBarras;
            NomeProduto = nomeProduto;
            DescricaoProduto = descricaoProduto;
            IsPerecivel = isPerecivel;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            StatusAtivo = statusAtivo;
        }
        // Stock-related methods moved to InventoryBalance
        // Product domain should only handle master data
    }
}
