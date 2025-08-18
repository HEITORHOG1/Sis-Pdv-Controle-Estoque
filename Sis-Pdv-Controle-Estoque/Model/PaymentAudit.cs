using Model.Base;

namespace Model
{
    public class PaymentAudit : EntityBase
    {
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public PaymentAuditAction Action { get; set; }
        public string Description { get; set; }
        public string? PreviousData { get; set; }
        public string? NewData { get; set; }
        public Guid UserId { get; set; }
        public virtual Usuario User { get; set; }
        public DateTime ActionDate { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public PaymentAudit(Guid paymentId, PaymentAuditAction action, string description, Guid userId)
        {
            PaymentId = paymentId;
            Action = action;
            Description = description;
            UserId = userId;
            ActionDate = DateTime.UtcNow;
        }

        public PaymentAudit() { }
    }

    public enum PaymentAuditAction
    {
        Created = 0,
        Processed = 1,
        Failed = 2,
        Cancelled = 3,
        Refunded = 4,
        FiscalReceiptGenerated = 5,
        FiscalReceiptAuthorized = 6,
        FiscalReceiptRejected = 7,
        FiscalReceiptCancelled = 8
    }
}