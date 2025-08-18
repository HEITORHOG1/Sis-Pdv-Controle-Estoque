using Model;

namespace Interfaces.Services
{
    public interface IPaymentProcessorService
    {
        Task<PaymentProcessorResult> ProcessCreditCardAsync(CreditCardPaymentRequest request, CancellationToken cancellationToken = default);
        Task<PaymentProcessorResult> ProcessDebitCardAsync(DebitCardPaymentRequest request, CancellationToken cancellationToken = default);
        Task<PaymentProcessorResult> ProcessPixAsync(PixPaymentRequest request, CancellationToken cancellationToken = default);
        Task<PaymentProcessorResult> RefundTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default);
        Task<PaymentProcessorResult> CancelTransactionAsync(string transactionId, CancellationToken cancellationToken = default);
        Task<PaymentProcessorResult> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken = default);
    }

    public class CreditCardPaymentRequest
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public int Installments { get; set; } = 1;
        public string OrderId { get; set; }
    }

    public class DebitCardPaymentRequest
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public string OrderId { get; set; }
    }

    public class PixPaymentRequest
    {
        public decimal Amount { get; set; }
        public string PixKey { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
    }

    public class PaymentProcessorResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TransactionId { get; set; }
        public string? AuthorizationCode { get; set; }
        public PaymentProcessorStatus Status { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? ProcessorResponse { get; set; }

        public static PaymentProcessorResult SuccessResult(string transactionId, string authorizationCode)
        {
            return new PaymentProcessorResult
            {
                Success = true,
                TransactionId = transactionId,
                AuthorizationCode = authorizationCode,
                Status = PaymentProcessorStatus.Approved,
                ProcessedAt = DateTime.UtcNow
            };
        }

        public static PaymentProcessorResult FailureResult(string errorMessage, PaymentProcessorStatus status = PaymentProcessorStatus.Rejected)
        {
            return new PaymentProcessorResult
            {
                Success = false,
                ErrorMessage = errorMessage,
                Status = status,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }

    public enum PaymentProcessorStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3,
        Refunded = 4,
        Error = 5
    }
}