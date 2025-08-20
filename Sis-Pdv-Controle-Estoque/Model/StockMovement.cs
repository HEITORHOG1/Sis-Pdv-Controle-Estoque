using Model.Base;

namespace Model
{
    public class StockMovement : EntityBase
    {
        public StockMovement()
        {
            Produto = new Produto();
        }

        public StockMovement(
            Guid produtoId,
            decimal quantity,
            StockMovementType type,
            string reason,
            decimal unitCost,
            decimal previousStock,
            decimal newStock,
            string? referenceDocument = null,
            Guid? userId = null,
            string? lote = null,
            DateTime? dataValidade = null)
        {
            ProdutoId = produtoId;
            Quantity = quantity;
            Type = type;
            Reason = reason;
            UnitCost = unitCost;
            PreviousStock = previousStock;
            NewStock = newStock;
            ReferenceDocument = referenceDocument;
            UserId = userId;
            Lote = lote;
            DataValidade = dataValidade;
            MovementDate = DateTime.UtcNow;
        }

        public Guid ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }
        public decimal Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; }
        public decimal UnitCost { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal NewStock { get; set; }
        public DateTime MovementDate { get; set; }
        public string? ReferenceDocument { get; set; }
        public Guid? UserId { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public virtual ICollection<StockMovementDetail> MovementDetails { get; set; } = new List<StockMovementDetail>();

        public void UpdateMovement(
            decimal quantity,
            StockMovementType type,
            string reason,
            decimal unitCost,
            decimal previousStock,
            decimal newStock,
            string? referenceDocument = null,
            string? lote = null,
            DateTime? dataValidade = null)
        {
            Quantity = quantity;
            Type = type;
            Reason = reason;
            UnitCost = unitCost;
            PreviousStock = previousStock;
            NewStock = newStock;
            ReferenceDocument = referenceDocument;
            Lote = lote;
            DataValidade = dataValidade;
            MovementDate = DateTime.UtcNow;
        }

        public void AddMovementDetail(string? lote, DateTime? dataValidade, decimal quantity)
        {
            var detail = new StockMovementDetail(Id, quantity, lote, dataValidade);
            MovementDetails.Add(detail);
        }
    }

    public enum StockMovementType
    {
        Entry = 1,      // Entrada de estoque
        Exit = 2,       // Saída de estoque
        Adjustment = 3, // Ajuste de estoque
        Sale = 4,       // Venda
        Return = 5,     // Devolução
        Transfer = 6,   // Transferência
        Loss = 7        // Perda
    }
}