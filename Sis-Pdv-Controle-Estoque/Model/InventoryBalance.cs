using Model.Base;

namespace Model
{
    public class InventoryBalance : EntityBase
    {
        public InventoryBalance()
        {
            Produto = new Produto();
        }

        public InventoryBalance(
            Guid produtoId,
            decimal currentStock = 0,
            decimal reservedStock = 0,
            decimal minimumStock = 0,
            decimal maximumStock = 0,
            decimal reorderPoint = 0,
            string? location = null)
        {
            ProdutoId = produtoId;
            CurrentStock = currentStock;
            ReservedStock = reservedStock;
            MinimumStock = minimumStock;
            MaximumStock = maximumStock;
            ReorderPoint = reorderPoint;
            Location = location;
            LastUpdated = DateTime.UtcNow;
        }

        public Guid ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal ReservedStock { get; set; }
        public decimal AvailableStock => CurrentStock - ReservedStock;
        public DateTime LastUpdated { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string? Location { get; set; }

        public bool IsLowStock()
        {
            return CurrentStock <= ReorderPoint;
        }

        public bool IsOutOfStock()
        {
            return CurrentStock <= 0;
        }

        public bool HasSufficientStock(decimal requestedQuantity)
        {
            return AvailableStock >= requestedQuantity;
        }

        public void UpdateStock(decimal newCurrentStock, decimal newReservedStock = 0)
        {
            CurrentStock = newCurrentStock;
            ReservedStock = newReservedStock;
            LastUpdated = DateTime.UtcNow;
        }

        public void ReserveStock(decimal quantity)
        {
            if (AvailableStock < quantity)
                throw new InvalidOperationException("Insufficient available stock to reserve");
            
            ReservedStock += quantity;
            LastUpdated = DateTime.UtcNow;
        }

        public void ReleaseReservedStock(decimal quantity)
        {
            if (ReservedStock < quantity)
                throw new InvalidOperationException("Cannot release more stock than is reserved");
            
            ReservedStock -= quantity;
            LastUpdated = DateTime.UtcNow;
        }
    }
}