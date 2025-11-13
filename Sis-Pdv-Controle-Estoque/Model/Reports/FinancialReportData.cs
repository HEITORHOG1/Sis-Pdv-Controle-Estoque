namespace Model.Reports
{
    public class FinancialReportData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public FinancialReportSummary Summary { get; set; } = new();
        public List<FinancialReportDailyData> DailyData { get; set; } = new();
        public List<FinancialReportProductData> ProductData { get; set; } = new();
    }

    public class FinancialReportSummary
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossProfitMargin { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItemsSold { get; set; }
        public decimal AverageOrderValue { get; set; }
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; } = new();
        public Dictionary<string, decimal> ProfitByCategory { get; set; } = new();
    }

    public class FinancialReportDailyData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public int OrderCount { get; set; }
        public int ItemsSold { get; set; }
    }

    public class FinancialReportProductData
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitMargin { get; set; }
    }
}