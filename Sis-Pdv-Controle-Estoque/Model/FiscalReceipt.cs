using Model.Base;

namespace Model
{
    public class FiscalReceipt : EntityBase
    {
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public string ReceiptNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime IssuedAt { get; set; }
        public FiscalReceiptStatus Status { get; set; }
        public string? SefazProtocol { get; set; }
        public string? AccessKey { get; set; }
        public string? QrCode { get; set; }
        public string? XmlContent { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? SentToSefazAt { get; set; }
        public DateTime? AuthorizedAt { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }

        public void Authorize(string sefazProtocol, string accessKey, string qrCode)
        {
            SefazProtocol = sefazProtocol;
            AccessKey = accessKey;
            QrCode = qrCode;
            Status = FiscalReceiptStatus.Authorized;
            AuthorizedAt = DateTime.UtcNow;
        }

        public void Reject(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = FiscalReceiptStatus.Rejected;
        }

        public void Cancel(string reason)
        {
            CancellationReason = reason;
            Status = FiscalReceiptStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
        }
    }

    public enum FiscalReceiptStatus
    {
        Pending = 0,
        Sent = 1,
        Authorized = 2,
        Rejected = 3,
        Cancelled = 4
    }
}