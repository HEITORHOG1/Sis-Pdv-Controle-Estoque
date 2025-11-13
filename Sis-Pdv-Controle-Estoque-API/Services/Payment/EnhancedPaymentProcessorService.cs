using Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sis_Pdv_Controle_Estoque_API.Configuration;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public class EnhancedPaymentProcessorService : IPaymentProcessorService
    {
        private readonly ILogger<EnhancedPaymentProcessorService> _logger;
        private readonly PaymentConfiguration _paymentConfig;
        private readonly Dictionary<string, PaymentProcessorResult> _transactions = new();

        public EnhancedPaymentProcessorService(
            ILogger<EnhancedPaymentProcessorService> logger,
            IOptions<PaymentConfiguration> paymentConfig)
        {
            _logger = logger;
            _paymentConfig = paymentConfig.Value;
        }

        public async Task<PaymentProcessorResult> ProcessCreditCardAsync(CreditCardPaymentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing credit card payment for amount {Amount} with {Installments} installments", 
                    request.Amount, request.Installments);

                // Validate request
                var validationResult = ValidateCreditCardRequest(request);
                if (!validationResult.IsValid)
                {
                    return PaymentProcessorResult.FailureResult(validationResult.ErrorMessage!, PaymentProcessorStatus.Rejected);
                }

                // Simulate processing delay if configured
                if (_paymentConfig.Processors.TryGetValue("Mock", out var mockConfig) && mockConfig.SimulateDelay)
                {
                    await Task.Delay(mockConfig.DelayMs, cancellationToken);
                }

                // Simulate different scenarios based on card number
                var scenario = DeterminePaymentScenario(request.CardNumber);
                
                var result = scenario switch
                {
                    PaymentScenario.InsufficientFunds => PaymentProcessorResult.FailureResult("Insufficient funds", PaymentProcessorStatus.Rejected),
                    PaymentScenario.CardExpired => PaymentProcessorResult.FailureResult("Card expired", PaymentProcessorStatus.Rejected),
                    PaymentScenario.InvalidSecurityCode => PaymentProcessorResult.FailureResult("Invalid security code", PaymentProcessorStatus.Rejected),
                    PaymentScenario.CardBlocked => PaymentProcessorResult.FailureResult("Card blocked", PaymentProcessorStatus.Rejected),
                    PaymentScenario.ProcessorError => PaymentProcessorResult.FailureResult("Processor temporarily unavailable", PaymentProcessorStatus.Error),
                    _ => PaymentProcessorResult.SuccessResult(
                        GenerateTransactionId("CC"), 
                        GenerateAuthorizationCode())
                };

                if (result.Success)
                {
                    result.ProcessorResponse = GenerateProcessorResponse("CreditCard", request.Amount, request.Installments);
                    _transactions[result.TransactionId!] = result;
                    _logger.LogInformation("Credit card payment approved. Transaction: {TransactionId}, Installments: {Installments}", 
                        result.TransactionId, request.Installments);
                }
                else
                {
                    _logger.LogWarning("Credit card payment rejected: {Error}", result.ErrorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing credit card payment");
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        public async Task<PaymentProcessorResult> ProcessDebitCardAsync(DebitCardPaymentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing debit card payment for amount {Amount}", request.Amount);

                // Validate request
                var validationResult = ValidateDebitCardRequest(request);
                if (!validationResult.IsValid)
                {
                    return PaymentProcessorResult.FailureResult(validationResult.ErrorMessage!, PaymentProcessorStatus.Rejected);
                }

                // Simulate processing delay
                if (_paymentConfig.Processors.TryGetValue("Mock", out var mockConfig) && mockConfig.SimulateDelay)
                {
                    await Task.Delay(Math.Min(mockConfig.DelayMs, _paymentConfig.Timeout.DebitCard), cancellationToken);
                }

                // Simulate different scenarios
                var scenario = DeterminePaymentScenario(request.CardNumber);
                
                var result = scenario switch
                {
                    PaymentScenario.InsufficientFunds => PaymentProcessorResult.FailureResult("Insufficient funds", PaymentProcessorStatus.Rejected),
                    PaymentScenario.CardBlocked => PaymentProcessorResult.FailureResult("Card blocked", PaymentProcessorStatus.Rejected),
                    PaymentScenario.InvalidSecurityCode => PaymentProcessorResult.FailureResult("Invalid security code", PaymentProcessorStatus.Rejected),
                    PaymentScenario.ProcessorError => PaymentProcessorResult.FailureResult("Processor temporarily unavailable", PaymentProcessorStatus.Error),
                    _ => PaymentProcessorResult.SuccessResult(
                        GenerateTransactionId("DB"), 
                        GenerateAuthorizationCode())
                };

                if (result.Success)
                {
                    result.ProcessorResponse = GenerateProcessorResponse("DebitCard", request.Amount);
                    _transactions[result.TransactionId!] = result;
                    _logger.LogInformation("Debit card payment approved. Transaction: {TransactionId}", result.TransactionId);
                }
                else
                {
                    _logger.LogWarning("Debit card payment rejected: {Error}", result.ErrorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing debit card payment");
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        public async Task<PaymentProcessorResult> ProcessPixAsync(PixPaymentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing PIX payment for amount {Amount} with key {PixKey}", 
                    request.Amount, MaskPixKey(request.PixKey));

                // Validate request
                var validationResult = ValidatePixRequest(request);
                if (!validationResult.IsValid)
                {
                    return PaymentProcessorResult.FailureResult(validationResult.ErrorMessage!, PaymentProcessorStatus.Rejected);
                }

                // Simulate processing delay
                if (_paymentConfig.Processors.TryGetValue("Mock", out var mockConfig) && mockConfig.SimulateDelay)
                {
                    await Task.Delay(Math.Min(mockConfig.DelayMs / 2, _paymentConfig.Timeout.Pix), cancellationToken);
                }

                // PIX payments are usually successful in mock, but can simulate some failures
                var scenario = DeterminePixScenario(request.PixKey);
                
                var result = scenario switch
                {
                    PaymentScenario.InvalidPixKey => PaymentProcessorResult.FailureResult("Invalid PIX key", PaymentProcessorStatus.Rejected),
                    PaymentScenario.PixUnavailable => PaymentProcessorResult.FailureResult("PIX service temporarily unavailable", PaymentProcessorStatus.Error),
                    _ => PaymentProcessorResult.SuccessResult(
                        GenerateTransactionId("PIX"), 
                        GenerateAuthorizationCode())
                };

                if (result.Success)
                {
                    result.ProcessorResponse = GenerateProcessorResponse("PIX", request.Amount, pixKey: request.PixKey);
                    _transactions[result.TransactionId!] = result;
                    _logger.LogInformation("PIX payment approved. Transaction: {TransactionId}", result.TransactionId);
                }
                else
                {
                    _logger.LogWarning("PIX payment rejected: {Error}", result.ErrorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PIX payment");
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        public async Task<PaymentProcessorResult> RefundTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing refund for transaction {TransactionId}, amount {Amount}", transactionId, amount);

                if (!_transactions.ContainsKey(transactionId))
                {
                    return PaymentProcessorResult.FailureResult("Transaction not found", PaymentProcessorStatus.Rejected);
                }

                var originalTransaction = _transactions[transactionId];
                
                if (originalTransaction.Status != PaymentProcessorStatus.Approved)
                {
                    return PaymentProcessorResult.FailureResult("Only approved transactions can be refunded", PaymentProcessorStatus.Rejected);
                }

                // Simulate processing delay
                await Task.Delay(400, cancellationToken);

                var refundResult = PaymentProcessorResult.SuccessResult(
                    GenerateTransactionId("REF"), 
                    GenerateAuthorizationCode());
                
                refundResult.Status = PaymentProcessorStatus.Refunded;
                refundResult.ProcessorResponse = GenerateRefundResponse(transactionId, amount);
                _transactions[refundResult.TransactionId!] = refundResult;

                // Update original transaction status
                originalTransaction.Status = PaymentProcessorStatus.Refunded;

                _logger.LogInformation("Refund processed successfully. Original: {OriginalTransactionId}, Refund: {RefundTransactionId}", 
                    transactionId, refundResult.TransactionId);

                return refundResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for transaction {TransactionId}", transactionId);
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        public async Task<PaymentProcessorResult> CancelTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Cancelling transaction {TransactionId}", transactionId);

                if (!_transactions.ContainsKey(transactionId))
                {
                    return PaymentProcessorResult.FailureResult("Transaction not found", PaymentProcessorStatus.Rejected);
                }

                var transaction = _transactions[transactionId];
                
                if (transaction.Status == PaymentProcessorStatus.Approved)
                {
                    return PaymentProcessorResult.FailureResult("Approved transactions cannot be cancelled, use refund instead", PaymentProcessorStatus.Rejected);
                }

                // Simulate processing delay
                await Task.Delay(200, cancellationToken);

                transaction.Status = PaymentProcessorStatus.Cancelled;
                transaction.ProcessorResponse = GenerateCancellationResponse(transactionId);

                _logger.LogInformation("Transaction {TransactionId} cancelled successfully", transactionId);

                return PaymentProcessorResult.SuccessResult(transactionId, transaction.AuthorizationCode!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling transaction {TransactionId}", transactionId);
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        public async Task<PaymentProcessorResult> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Delay(100, cancellationToken);

                if (!_transactions.ContainsKey(transactionId))
                {
                    return PaymentProcessorResult.FailureResult("Transaction not found", PaymentProcessorStatus.Rejected);
                }

                return _transactions[transactionId];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transaction status for {TransactionId}", transactionId);
                return PaymentProcessorResult.FailureResult($"Internal processor error: {ex.Message}", PaymentProcessorStatus.Error);
            }
        }

        private ValidationResult ValidateCreditCardRequest(CreditCardPaymentRequest request)
        {
            if (request.Amount <= 0)
                return ValidationResult.Invalid("Amount must be greater than zero");

            if (_paymentConfig.Validation.ValidateCardNumber && !IsValidCardNumber(request.CardNumber))
                return ValidationResult.Invalid("Invalid card number");

            if (_paymentConfig.Validation.ValidateExpiryDate && !IsValidExpiryDate(request.ExpiryDate))
                return ValidationResult.Invalid("Invalid or expired card");

            if (_paymentConfig.Validation.ValidateSecurityCode && !IsValidSecurityCode(request.SecurityCode))
                return ValidationResult.Invalid("Invalid security code");

            if (request.Installments < 1 || request.Installments > _paymentConfig.Validation.MaxInstallments)
                return ValidationResult.Invalid($"Installments must be between 1 and {_paymentConfig.Validation.MaxInstallments}");

            if (string.IsNullOrWhiteSpace(request.CardHolderName))
                return ValidationResult.Invalid("Card holder name is required");

            return ValidationResult.Valid();
        }

        private ValidationResult ValidateDebitCardRequest(DebitCardPaymentRequest request)
        {
            if (request.Amount <= 0)
                return ValidationResult.Invalid("Amount must be greater than zero");

            if (_paymentConfig.Validation.ValidateCardNumber && !IsValidCardNumber(request.CardNumber))
                return ValidationResult.Invalid("Invalid card number");

            if (_paymentConfig.Validation.ValidateExpiryDate && !IsValidExpiryDate(request.ExpiryDate))
                return ValidationResult.Invalid("Invalid or expired card");

            if (_paymentConfig.Validation.ValidateSecurityCode && !IsValidSecurityCode(request.SecurityCode))
                return ValidationResult.Invalid("Invalid security code");

            if (string.IsNullOrWhiteSpace(request.CardHolderName))
                return ValidationResult.Invalid("Card holder name is required");

            return ValidationResult.Valid();
        }

        private ValidationResult ValidatePixRequest(PixPaymentRequest request)
        {
            if (request.Amount <= 0)
                return ValidationResult.Invalid("Amount must be greater than zero");

            if (string.IsNullOrWhiteSpace(request.PixKey))
                return ValidationResult.Invalid("PIX key is required");

            if (!IsValidPixKey(request.PixKey))
                return ValidationResult.Invalid("Invalid PIX key format");

            return ValidationResult.Valid();
        }

        private static bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Remove spaces and dashes
            cardNumber = Regex.Replace(cardNumber, @"[\s-]", "");

            // Check if all digits
            if (!Regex.IsMatch(cardNumber, @"^\d+$"))
                return false;

            // Check length (13-19 digits for most cards)
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            // Luhn algorithm check (simplified)
            return IsValidLuhn(cardNumber);
        }

        private static bool IsValidLuhn(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit = (digit % 10) + 1;
                }

                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        private static bool IsValidExpiryDate(string expiryDate)
        {
            if (string.IsNullOrWhiteSpace(expiryDate))
                return false;

            // Expected format: MM/YY or MM/YYYY
            var match = Regex.Match(expiryDate, @"^(\d{2})/(\d{2}|\d{4})$");
            if (!match.Success)
                return false;

            var month = int.Parse(match.Groups[1].Value);
            var year = int.Parse(match.Groups[2].Value);

            if (month < 1 || month > 12)
                return false;

            // Convert YY to YYYY
            if (year < 100)
                year += 2000;

            var expiryDateTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return expiryDateTime >= DateTime.Now.Date;
        }

        private static bool IsValidSecurityCode(string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
                return false;

            // CVV should be 3 or 4 digits
            return Regex.IsMatch(securityCode, @"^\d{3,4}$");
        }

        private static bool IsValidPixKey(string pixKey)
        {
            if (string.IsNullOrWhiteSpace(pixKey))
                return false;

            // CPF format
            if (Regex.IsMatch(pixKey, @"^\d{11}$"))
                return true;

            // CNPJ format
            if (Regex.IsMatch(pixKey, @"^\d{14}$"))
                return true;

            // Email format
            if (Regex.IsMatch(pixKey, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return true;

            // Phone format
            if (Regex.IsMatch(pixKey, @"^\+55\d{10,11}$"))
                return true;

            // Random key format (UUID)
            if (Guid.TryParse(pixKey, out _))
                return true;

            return false;
        }

        private static PaymentScenario DeterminePaymentScenario(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return PaymentScenario.Success;

            var lastDigit = cardNumber.Last();
            
            return lastDigit switch
            {
                '1' => PaymentScenario.InsufficientFunds,
                '2' => PaymentScenario.CardExpired,
                '3' => PaymentScenario.InvalidSecurityCode,
                '4' => PaymentScenario.CardBlocked,
                '9' => PaymentScenario.ProcessorError,
                _ => PaymentScenario.Success
            };
        }

        private static PaymentScenario DeterminePixScenario(string pixKey)
        {
            if (string.IsNullOrEmpty(pixKey))
                return PaymentScenario.Success;

            if (pixKey.Contains("invalid"))
                return PaymentScenario.InvalidPixKey;

            if (pixKey.Contains("error"))
                return PaymentScenario.PixUnavailable;

            return PaymentScenario.Success;
        }

        private static string GenerateTransactionId(string prefix)
        {
            return $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        private static string GenerateAuthorizationCode()
        {
            return $"AUTH_{DateTime.UtcNow.Ticks % 1000000:D6}";
        }

        private static string GenerateProcessorResponse(string method, decimal amount, int installments = 1, string? pixKey = null)
        {
            var response = new
            {
                Method = method,
                Amount = amount,
                Installments = installments,
                PixKey = pixKey != null ? MaskPixKey(pixKey) : null,
                ProcessedAt = DateTime.UtcNow,
                ProcessorId = "MOCK_PROCESSOR_001",
                Status = "APPROVED"
            };

            return System.Text.Json.JsonSerializer.Serialize(response);
        }

        private static string GenerateRefundResponse(string originalTransactionId, decimal amount)
        {
            var response = new
            {
                OriginalTransactionId = originalTransactionId,
                RefundAmount = amount,
                ProcessedAt = DateTime.UtcNow,
                ProcessorId = "MOCK_PROCESSOR_001",
                Status = "REFUNDED"
            };

            return System.Text.Json.JsonSerializer.Serialize(response);
        }

        private static string GenerateCancellationResponse(string transactionId)
        {
            var response = new
            {
                TransactionId = transactionId,
                CancelledAt = DateTime.UtcNow,
                ProcessorId = "MOCK_PROCESSOR_001",
                Status = "CANCELLED"
            };

            return System.Text.Json.JsonSerializer.Serialize(response);
        }

        private static string MaskPixKey(string pixKey)
        {
            if (string.IsNullOrEmpty(pixKey))
                return pixKey;

            // Mask email
            if (pixKey.Contains("@"))
            {
                var parts = pixKey.Split('@');
                if (parts[0].Length > 2)
                    return $"{parts[0][..2]}***@{parts[1]}";
            }

            // Mask CPF/CNPJ
            if (Regex.IsMatch(pixKey, @"^\d+$"))
            {
                if (pixKey.Length >= 6)
                    return $"{pixKey[..3]}***{pixKey[^3..]}";
            }

            // Mask phone
            if (pixKey.StartsWith("+55"))
            {
                return $"+55***{pixKey[^4..]}";
            }

            return pixKey.Length > 6 ? $"{pixKey[..3]}***{pixKey[^3..]}" : pixKey;
        }

        private enum PaymentScenario
        {
            Success,
            InsufficientFunds,
            CardExpired,
            InvalidSecurityCode,
            CardBlocked,
            ProcessorError,
            InvalidPixKey,
            PixUnavailable
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string? ErrorMessage { get; set; }

            public static ValidationResult Valid() => new() { IsValid = true };
            public static ValidationResult Invalid(string errorMessage) => new() { IsValid = false, ErrorMessage = errorMessage };
        }
    }
}