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
            int statusAtivo,
            decimal minimumStock = 0,
            decimal maximumStock = 0,
            decimal reorderPoint = 0,
            string? location = null)
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
            MinimumStock = minimumStock;
            MaximumStock = maximumStock;
            ReorderPoint = reorderPoint;
            Location = location;
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
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string? Location { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public Guid FornecedorId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid CategoriaId { get; set; }
        public int StatusAtivo { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        internal void AlterarProduto(Guid id, string codBarras, string nomeProduto, string descricaoProduto,
            decimal precoCusto, decimal precoVenda, decimal margemLucro, DateTime dataFabricao,
            DateTime dataVencimento, int quatidadeEstoqueProduto,
            Guid FornecedorId, Guid CategoriaId, int statusAtivo,
            decimal minimumStock = 0, decimal maximumStock = 0, decimal reorderPoint = 0, string? location = null)
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
            MinimumStock = minimumStock;
            MaximumStock = maximumStock;
            ReorderPoint = reorderPoint;
            Location = location;
        }
        internal void AtualizarEstoque(Guid id, int quatidadeEstoqueProduto)
        {
            Id = id;
            QuatidadeEstoqueProduto = quatidadeEstoqueProduto;
        }

        public bool IsLowStock()
        {
            return QuatidadeEstoqueProduto <= ReorderPoint;
        }

        public bool IsOutOfStock()
        {
            return QuatidadeEstoqueProduto <= 0;
        }

        public bool HasSufficientStock(int requestedQuantity)
        {
            return QuatidadeEstoqueProduto >= requestedQuantity;
        }

        public void UpdateStock(int newQuantity, string reason, Guid? userId = null)
        {
            var previousStock = QuatidadeEstoqueProduto;
            QuatidadeEstoqueProduto = newQuantity;
            
            // Create stock movement record
            var movement = new StockMovement(
                Id,
                newQuantity - previousStock,
                newQuantity > previousStock ? StockMovementType.Entry : StockMovementType.Exit,
                reason,
                PrecoCusto,
                previousStock,
                newQuantity,
                null,
                userId
            );
            
            StockMovements.Add(movement);
        }
    }
}
