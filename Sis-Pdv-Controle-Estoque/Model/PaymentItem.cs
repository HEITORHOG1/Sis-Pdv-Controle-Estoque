using Model.Base;

namespace Model
{
    public class PaymentItem : EntityBase
    {
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? CardNumber { get; set; } // Masked card number
        public string? CardHolderName { get; set; }
        public string? ProcessorTransactionId { get; set; }
        public string? AuthorizationCode { get; set; }
        public PaymentItemStatus Status { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
        
        // Enhanced payment fields
        public int Installments { get; set; } = 1;
        public string? PixKey { get; set; }
        public string? PixQrCode { get; set; }
        public string? BankCode { get; set; }
        public string? AgencyNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? CheckNumber { get; set; }
        public string? VoucherCode { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? Fee { get; set; }
        public string? ProcessorName { get; set; }
        public string? ProcessorResponse { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public void ProcessItem(string processorTransactionId, string authorizationCode)
        {
            ProcessorTransactionId = processorTransactionId;
            AuthorizationCode = authorizationCode;
            Status = PaymentItemStatus.Approved;
            ProcessedAt = DateTime.UtcNow;
        }

        public void RejectItem(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = PaymentItemStatus.Rejected;
            ProcessedAt = DateTime.UtcNow;
        }
    }

    public enum PaymentMethod
    {
        Cash = 0,
        CreditCard = 1,
        DebitCard = 2,
        Pix = 3,
        BankTransfer = 4,
        Check = 5,
        Voucher = 6
    }

    public enum PaymentItemStatus
    {
        Pending = 0,
        Processing = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4
    }
}