namespace Model.Reports
{
    public class InventoryReportData
    {
        public DateTime GeneratedAt { get; set; }
        public List<InventoryReportItem> Products { get; set; } = new();
        public InventoryReportSummary Summary { get; set; } = new();
    }

    public class InventoryReportItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string? Location { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal StockValue { get; set; }
        public string StockStatus { get; set; } = string.Empty;
        public DateTime? LastMovementDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class InventoryReportSummary
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int InactiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalStockValue { get; set; }
        public decimal AverageStockValue { get; set; }
        public Dictionary<string, int> ProductsByCategory { get; set; } = new();
        public Dictionary<string, decimal> StockValueByCategory { get; set; } = new();
    }
}