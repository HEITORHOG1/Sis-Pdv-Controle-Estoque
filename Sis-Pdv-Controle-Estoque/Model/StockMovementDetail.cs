using Model.Base;

namespace Model
{
    public class StockMovementDetail : EntityBase
    {
        public StockMovementDetail()
        {
            StockMovement = new StockMovement();
        }

        public StockMovementDetail(
            Guid stockMovementId,
            decimal quantity,
            string? lote = null,
            DateTime? dataValidade = null)
        {
            StockMovementId = stockMovementId;
            Quantity = quantity;
            Lote = lote;
            DataValidade = dataValidade;
        }

        public Guid StockMovementId { get; set; }
        public virtual StockMovement StockMovement { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal Quantity { get; set; }

        public bool IsExpired()
        {
            return DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        }

        public bool IsNearExpiry(int daysThreshold = 30)
        {
            return DataValidade.HasValue && 
                   DataValidade.Value <= DateTime.Now.AddDays(daysThreshold);
        }

        public void UpdateDetail(
            decimal quantity,
            string? lote = null,
            DateTime? dataValidade = null)
        {
            Quantity = quantity;
            Lote = lote;
            DataValidade = dataValidade;
        }
    }
}