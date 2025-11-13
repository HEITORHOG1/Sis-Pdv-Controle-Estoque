using MediatR;
using Model;
using Interfaces.Services;

namespace Commands.Payment.ProcessPayment
{
    public class ProcessPaymentRequest : IRequest<ProcessPaymentResponse>
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentMethodRequest> PaymentMethods { get; set; } = new();
        public bool GenerateFiscalReceipt { get; set; } = true;
        public Guid UserId { get; set; }
    }

    public class ProcessPaymentResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? PaymentId { get; set; }
        public string? TransactionId { get; set; }
        public string? AuthorizationCode { get; set; }
        public Guid? FiscalReceiptId { get; set; }
        public string? ReceiptNumber { get; set; }
    }
}