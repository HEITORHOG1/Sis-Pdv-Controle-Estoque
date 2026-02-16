namespace Model.Reports
{
    public class SalesReportData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SalesReportItem> Sales { get; set; } = new();
        public SalesReportSummary Summary { get; set; } = new();
    }

    public class SalesReportItem
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerDocument { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public List<SalesReportItemDetail> Items { get; set; } = new();
    }

    public class SalesReportItemDetail
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class SalesReportSummary
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalItemsSold { get; set; }
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; } = new();
        public Dictionary<string, int> OrdersByPaymentMethod { get; set; } = new();
    }
}