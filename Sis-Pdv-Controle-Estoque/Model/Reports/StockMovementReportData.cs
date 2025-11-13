namespace Model.Reports
{
    public class StockMovementReportData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<StockMovementReportItem> Movements { get; set; } = new();
        public StockMovementReportSummary Summary { get; set; } = new();
    }

    public class StockMovementReportItem
    {
        public Guid MovementId { get; set; }
        public DateTime MovementDate { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal PreviousStock { get; set; }
        public decimal NewStock { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string? ReferenceDocument { get; set; }
        public string? UserName { get; set; }
    }

    public class StockMovementReportSummary
    {
        public int TotalMovements { get; set; }
        public Dictionary<string, int> MovementsByType { get; set; } = new();
        public Dictionary<string, decimal> ValueByMovementType { get; set; } = new();
        public decimal TotalInboundValue { get; set; }
        public decimal TotalOutboundValue { get; set; }
        public decimal NetMovementValue { get; set; }
    }
}