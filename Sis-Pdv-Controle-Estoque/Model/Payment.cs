using Model.Base;

namespace Model
{
    public class Payment : EntityBase
    {
        public Payment()
        {
            PaymentItems = new List<PaymentItem>();
        }

        public Guid OrderId { get; set; }
        public virtual Pedido Order { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? ProcessorResponse { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public virtual ICollection<PaymentItem> PaymentItems { get; set; }
        public virtual FiscalReceipt? FiscalReceipt { get; set; }

        public void ProcessPayment(string transactionId, string authorizationCode)
        {
            TransactionId = transactionId;
            AuthorizationCode = authorizationCode;
            Status = PaymentStatus.Processed;
            ProcessedAt = DateTime.UtcNow;
        }

        public void FailPayment(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = PaymentStatus.Failed;
            ProcessedAt = DateTime.UtcNow;
        }

        public void CancelPayment()
        {
            Status = PaymentStatus.Cancelled;
            ProcessedAt = DateTime.UtcNow;
        }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Processed = 2,
        Failed = 3,
        Cancelled = 4,
        Refunded = 5
    }
}