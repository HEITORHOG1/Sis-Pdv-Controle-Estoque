using Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Produto : EntityBase
    {


        // Construtor para EF Core
        public Produto()
        {
            Fornecedor = new Fornecedor();
            Categoria = new Categoria();
        }
        
        public Produto(
            string CodBarras,
            string NomeProduto,
            string DescricaoProduto,
            decimal PrecoCusto,
            decimal PrecoVenda,
            decimal MargemLucro,
            DateTime DataFabricao,
            DateTime DataVencimento,
            int QuantidadeEstoqueProduto,
            Guid FornecedorId,
            Guid CategoriaId,
            int StatusAtivo,
            decimal minimumStock = 0,
            decimal maximumStock = 0,
            decimal reorderPoint = 0,
            string? location = null)
        {
            ValidarCodigoBarras(CodBarras);
            ValidarNomeProduto(NomeProduto);
            ValidarPrecos(PrecoCusto, PrecoVenda);
            ValidarDatas(DataFabricao, DataVencimento);
            ValidarEstoque(QuantidadeEstoqueProduto, minimumStock, maximumStock, reorderPoint);
            ValidarFornecedorId(FornecedorId);
            ValidarCategoriaId(CategoriaId);
            
            this.CodBarras = CodBarras;
            this.NomeProduto = NomeProduto;
            this.DescricaoProduto = DescricaoProduto;
            this.PrecoCusto = PrecoCusto;
            this.PrecoVenda = PrecoVenda;
            this.MargemLucro = MargemLucro;
            this.DataFabricao = DataFabricao;
            this.DataVencimento = DataVencimento;
            this.QuantidadeEstoqueProduto = QuantidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            this.StatusAtivo = StatusAtivo;
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
        [Column("QuatidadeEstoqueProduto")] // Mantém nome da coluna no DB para compatibilidade
        public int QuantidadeEstoqueProduto { get; set; }
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

        internal void AlterarProduto(Guid id, string CodBarras, string NomeProduto, string DescricaoProduto,
            decimal PrecoCusto, decimal PrecoVenda, decimal MargemLucro, DateTime DataFabricao,
            DateTime DataVencimento, int QuantidadeEstoqueProduto,
            Guid FornecedorId, Guid CategoriaId, int StatusAtivo,
            decimal minimumStock = 0, decimal maximumStock = 0, decimal reorderPoint = 0, string? location = null)
        {
            Id = id;
            this.CodBarras = CodBarras;
            this.NomeProduto = NomeProduto;
            this.DescricaoProduto = DescricaoProduto;
            this.PrecoCusto = PrecoCusto;
            this.PrecoVenda = PrecoVenda;
            this.MargemLucro = MargemLucro;
            this.DataFabricao = DataFabricao;
            this.DataVencimento = DataVencimento;
            this.QuantidadeEstoqueProduto = QuantidadeEstoqueProduto;
            this.FornecedorId = FornecedorId;
            this.CategoriaId = CategoriaId;
            this.StatusAtivo = StatusAtivo;
            MinimumStock = minimumStock;
            MaximumStock = maximumStock;
            ReorderPoint = reorderPoint;
            Location = location;
        }
        internal void AtualizarEstoque(Guid id, int QuantidadeEstoqueProduto)
        {
            Id = id;
            this.QuantidadeEstoqueProduto = QuantidadeEstoqueProduto;
        }

        public bool IsLowStock()
        {
            return QuantidadeEstoqueProduto <= ReorderPoint;
        }

        public bool IsOutOfStock()
        {
            return QuantidadeEstoqueProduto <= 0;
        }

        public bool HasSufficientStock(int requestedQuantity)
        {
            return QuantidadeEstoqueProduto >= requestedQuantity;
        }

        public void UpdateStock(int newQuantity, string reason, Guid? userId = null)
        {
            if (newQuantity < 0)
                throw new DomainException("A quantidade em estoque não pode ser negativa");
                
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Motivo da movimentação de estoque é obrigatório");
            
            var previousStock = QuantidadeEstoqueProduto;
            QuantidadeEstoqueProduto = newQuantity;
            
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
        
        // Métodos de validação privados
        private static void ValidarCodigoBarras(string CodBarras)
        {
            if (string.IsNullOrWhiteSpace(CodBarras))
                throw new DomainException("Código de barras é obrigatório");
        }
        
        private static void ValidarNomeProduto(string NomeProduto)
        {
            if (string.IsNullOrWhiteSpace(NomeProduto))
                throw new DomainException("Nome do produto é obrigatório");
                
            if (NomeProduto.Length < 3)
                throw new DomainException("Nome do produto deve ter no mínimo 3 caracteres");
        }
        
        private static void ValidarPrecos(decimal PrecoCusto, decimal PrecoVenda)
        {
            if (PrecoCusto < 0)
                throw new DomainException("Preço de custo não pode ser negativo");
                
            if (PrecoVenda <= 0)
                throw new DomainException("Preço de venda deve ser maior que zero");
                
            if (PrecoVenda < PrecoCusto)
                throw new DomainException("Preço de venda não pode ser menor que o preço de custo");
        }
        
        private static void ValidarDatas(DateTime DataFabricao, DateTime DataVencimento)
        {
            if (DataVencimento <= DataFabricao)
                throw new DomainException("Data de vencimento deve ser posterior à data de fabricação");
        }
        
        private static void ValidarEstoque(int quantidade, decimal minimo, decimal maximo, decimal pontoReposicao)
        {
            if (quantidade < 0)
                throw new DomainException("Quantidade em estoque não pode ser negativa");
                
            if (minimo < 0)
                throw new DomainException("Estoque mínimo não pode ser negativo");
                
            if (maximo > 0 && maximo < minimo)
                throw new DomainException("Estoque máximo não pode ser menor que o estoque mínimo");
                
            if (pontoReposicao > 0 && pontoReposicao < minimo)
                throw new DomainException("Ponto de reposição não pode ser menor que o estoque mínimo");
        }
        
        private static void ValidarFornecedorId(Guid fornecedorId)
        {
            if (fornecedorId == Guid.Empty)
                throw new DomainException("Fornecedor é obrigatório");
        }
        
        private static void ValidarCategoriaId(Guid categoriaId)
        {
            if (categoriaId == Guid.Empty)
                throw new DomainException("Categoria é obrigatória");
        }
    }
}
