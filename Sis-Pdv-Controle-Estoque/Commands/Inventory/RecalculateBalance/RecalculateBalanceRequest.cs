using MediatR;

namespace Commands.Inventory.RecalculateBalance
{
    public class RecalculateBalanceRequest : IRequest<RecalculateBalanceResponse>
    {
        public Guid? ProductId { get; set; } // If null, recalculate all products
        public bool ForceRecalculation { get; set; } = false;
    }

    public class RecalculateBalanceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ProcessedProducts { get; set; }
        public IEnumerable<ProductBalanceResult> Results { get; set; } = new List<ProductBalanceResult>();
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }

    public class ProductBalanceResult
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
        public decimal Difference => NewBalance - PreviousBalance;
        public bool HasDiscrepancy => Math.Abs(Difference) > 0.001m;
    }
}