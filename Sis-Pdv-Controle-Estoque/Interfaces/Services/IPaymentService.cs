using Model;

namespace Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default);
        Task<PaymentResult> RefundPaymentAsync(Guid paymentId, decimal amount, string reason, CancellationToken cancellationToken = default);
        Task<PaymentResult> CancelPaymentAsync(Guid paymentId, string reason, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<bool> ValidatePaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    }

    public class ProcessPaymentRequest
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentMethodRequest> PaymentMethods { get; set; } = new();
        public bool GenerateFiscalReceipt { get; set; } = true;
        public Guid UserId { get; set; }
    }

    public class PaymentMethodRequest
    {
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? SecurityCode { get; set; }
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Payment? Payment { get; set; }
        public string? TransactionId { get; set; }
        public string? AuthorizationCode { get; set; }
        public FiscalReceipt? FiscalReceipt { get; set; }

        public static PaymentResult SuccessResult(Payment payment, string? transactionId = null, string? authorizationCode = null)
        {
            return new PaymentResult
            {
                Success = true,
                Payment = payment,
                TransactionId = transactionId,
                AuthorizationCode = authorizationCode
            };
        }

        public static PaymentResult FailureResult(string errorMessage)
        {
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}