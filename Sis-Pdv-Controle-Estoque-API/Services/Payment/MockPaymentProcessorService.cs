using Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public class MockPaymentProcessorService : IPaymentProcessorService
    {
        private readonly ILogger<MockPaymentProcessorService> _logger;
        private readonly Dictionary<string, PaymentProcessorResult> _transactions = new();

        public MockPaymentProcessorService(ILogger<MockPaymentProcessorService> logger)
        {
            _logger = logger;
        }

        public async Task<PaymentProcessorResult> ProcessCreditCardAsync(CreditCardPaymentRequest request, CancellationToken cancellationToken = default)
        {
            await Task.Delay(500, cancellationToken); // Simulate processing time

            _logger.LogInformation("Processing credit card payment for amount {Amount}", request.Amount);

            // Simulate different scenarios based on card number
            var lastDigit = request.CardNumber.Last();
            
            var result = lastDigit switch
            {
                '1' => PaymentProcessorResult.FailureResult("Insufficient funds", PaymentProcessorStatus.Rejected),
                '2' => PaymentProcessorResult.FailureResult("Card expired", PaymentProcessorStatus.Rejected),
                '3' => PaymentProcessorResult.FailureResult("Invalid security code", PaymentProcessorStatus.Rejected),
                _ => PaymentProcessorResult.SuccessResult(
                    GenerateTransactionId(), 
                    GenerateAuthorizationCode())
            };

            if (result.Success)
            {
                _transactions[result.TransactionId!] = result;
                _logger.LogInformation("Credit card payment approved. Transaction: {TransactionId}", result.TransactionId);
            }
            else
            {
                _logger.LogWarning("Credit card payment rejected: {Error}", result.ErrorMessage);
            }

            return result;
        }

        public async Task<PaymentProcessorResult> ProcessDebitCardAsync(DebitCardPaymentRequest request, CancellationToken cancellationToken = default)
        {
            await Task.Delay(300, cancellationToken); // Simulate processing time

            _logger.LogInformation("Processing debit card payment for amount {Amount}", request.Amount);

            // Simulate different scenarios based on card number
            var lastDigit = request.CardNumber.Last();
            
            var result = lastDigit switch
            {
                '1' => PaymentProcessorResult.FailureResult("Insufficient funds", PaymentProcessorStatus.Rejected),
                '2' => PaymentProcessorResult.FailureResult("Card blocked", PaymentProcessorStatus.Rejected),
                _ => PaymentProcessorResult.SuccessResult(
                    GenerateTransactionId(), 
                    GenerateAuthorizationCode())
            };

            if (result.Success)
            {
                _transactions[result.TransactionId!] = result;
                _logger.LogInformation("Debit card payment approved. Transaction: {TransactionId}", result.TransactionId);
            }
            else
            {
                _logger.LogWarning("Debit card payment rejected: {Error}", result.ErrorMessage);
            }

            return result;
        }

        public async Task<PaymentProcessorResult> ProcessPixAsync(PixPaymentRequest request, CancellationToken cancellationToken = default)
        {
            await Task.Delay(200, cancellationToken); // Simulate processing time

            _logger.LogInformation("Processing PIX payment for amount {Amount}", request.Amount);

            // PIX payments are usually successful in mock
            var result = PaymentProcessorResult.SuccessResult(
                GenerateTransactionId(), 
                GenerateAuthorizationCode());

            _transactions[result.TransactionId!] = result;
            _logger.LogInformation("PIX payment approved. Transaction: {TransactionId}", result.TransactionId);

            return result;
        }

        public async Task<PaymentProcessorResult> RefundTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default)
        {
            await Task.Delay(400, cancellationToken); // Simulate processing time

            _logger.LogInformation("Processing refund for transaction {TransactionId}, amount {Amount}", transactionId, amount);

            if (!_transactions.ContainsKey(transactionId))
            {
                return PaymentProcessorResult.FailureResult("Transaction not found");
            }

            var refundResult = PaymentProcessorResult.SuccessResult(
                GenerateTransactionId(), 
                GenerateAuthorizationCode());
            
            refundResult.Status = PaymentProcessorStatus.Refunded;
            _transactions[refundResult.TransactionId!] = refundResult;

            _logger.LogInformation("Refund processed successfully. Refund Transaction: {TransactionId}", refundResult.TransactionId);

            return refundResult;
        }

        public async Task<PaymentProcessorResult> CancelTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(200, cancellationToken); // Simulate processing time

            _logger.LogInformation("Cancelling transaction {TransactionId}", transactionId);

            if (!_transactions.ContainsKey(transactionId))
            {
                return PaymentProcessorResult.FailureResult("Transaction not found");
            }

            var transaction = _transactions[transactionId];
            transaction.Status = PaymentProcessorStatus.Cancelled;

            _logger.LogInformation("Transaction {TransactionId} cancelled successfully", transactionId);

            return PaymentProcessorResult.SuccessResult(transactionId, transaction.AuthorizationCode!);
        }

        public async Task<PaymentProcessorResult> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken); // Simulate processing time

            if (!_transactions.ContainsKey(transactionId))
            {
                return PaymentProcessorResult.FailureResult("Transaction not found");
            }

            return _transactions[transactionId];
        }

        private static string GenerateTransactionId()
        {
            return $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        private static string GenerateAuthorizationCode()
        {
            return $"AUTH_{DateTime.UtcNow.Ticks % 1000000:D6}";
        }
    }
}