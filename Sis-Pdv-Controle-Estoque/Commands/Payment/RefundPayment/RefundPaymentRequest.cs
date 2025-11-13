using MediatR;

namespace Commands.Payment.RefundPayment
{
    public class RefundPaymentRequest : IRequest<RefundPaymentResponse>
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }

    public class RefundPaymentResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? RefundId { get; set; }
        public string? RefundTransactionId { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}