using Model.Exceptions;

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
            ValidarCodigoBarras(codBarras);
            ValidarNomeProduto(nomeProduto);
            ValidarPrecos(precoCusto, precoVenda);
            ValidarDatas(dataFabricao, dataVencimento);
            ValidarEstoque(quatidadeEstoqueProduto, minimumStock, maximumStock, reorderPoint);
            ValidarFornecedorId(FornecedorId);
            ValidarCategoriaId(CategoriaId);
            
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
            if (newQuantity < 0)
                throw new DomainException("A quantidade em estoque não pode ser negativa");
                
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Motivo da movimentação de estoque é obrigatório");
            
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
        
        // Métodos de validação privados
        private static void ValidarCodigoBarras(string codBarras)
        {
            if (string.IsNullOrWhiteSpace(codBarras))
                throw new DomainException("Código de barras é obrigatório");
        }
        
        private static void ValidarNomeProduto(string nomeProduto)
        {
            if (string.IsNullOrWhiteSpace(nomeProduto))
                throw new DomainException("Nome do produto é obrigatório");
                
            if (nomeProduto.Length < 3)
                throw new DomainException("Nome do produto deve ter no mínimo 3 caracteres");
        }
        
        private static void ValidarPrecos(decimal precoCusto, decimal precoVenda)
        {
            if (precoCusto < 0)
                throw new DomainException("Preço de custo não pode ser negativo");
                
            if (precoVenda <= 0)
                throw new DomainException("Preço de venda deve ser maior que zero");
                
            if (precoVenda < precoCusto)
                throw new DomainException("Preço de venda não pode ser menor que o preço de custo");
        }
        
        private static void ValidarDatas(DateTime dataFabricao, DateTime dataVencimento)
        {
            if (dataVencimento <= dataFabricao)
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
