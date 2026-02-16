using MediatR;

namespace Commands.Payment.CancelPayment
{
    public class CancelPaymentRequest : IRequest<CancelPaymentResponse>
    {
        public Guid PaymentId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }

    public class CancelPaymentResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? PaymentId { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}