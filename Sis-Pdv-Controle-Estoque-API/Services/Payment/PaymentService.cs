using Interfaces.Repositories;
using Interfaces.Services;
using Model;
using Microsoft.Extensions.Logging;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryPayment _paymentRepository;
        private readonly IRepositoryPaymentAudit _auditRepository;
        private readonly IPaymentProcessorService _processorService;
        private readonly IFiscalService _fiscalService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IRepositoryPayment paymentRepository,
            IRepositoryPaymentAudit auditRepository,
            IPaymentProcessorService processorService,
            IFiscalService fiscalService,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _auditRepository = auditRepository;
            _processorService = processorService;
            _fiscalService = fiscalService;
            _logger = logger;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing payment for order {OrderId} with amount {Amount}", 
                    request.OrderId, request.TotalAmount);

                // Validate payment request
                if (!await ValidatePaymentAsync(request, cancellationToken))
                {
                    return PaymentResult.FailureResult("Invalid payment request");
                }

                // Create payment entity
                var payment = new Model.Payment
                {
                    OrderId = request.OrderId,
                    TotalAmount = request.TotalAmount,
                    Status = PaymentStatus.Pending,
                    PaymentDate = DateTime.UtcNow
                };

                // Process each payment method
                foreach (var methodRequest in request.PaymentMethods)
                {
                    var paymentItem = new PaymentItem
                    {
                        PaymentId = payment.Id,
                        Method = methodRequest.Method,
                        Amount = methodRequest.Amount,
                        CardNumber = MaskCardNumber(methodRequest.CardNumber),
                        CardHolderName = methodRequest.CardHolderName,
                        Status = PaymentItemStatus.Pending
                    };

                    // Process payment based on method
                    var processorResult = await ProcessPaymentMethodAsync(methodRequest, cancellationToken);
                    
                    if (processorResult.Success)
                    {
                        paymentItem.ProcessItem(processorResult.TransactionId!, processorResult.AuthorizationCode!);
                        payment.TransactionId = processorResult.TransactionId;
                        payment.AuthorizationCode = processorResult.AuthorizationCode;
                    }
                    else
                    {
                        paymentItem.RejectItem(processorResult.ErrorMessage!);
                        payment.FailPayment(processorResult.ErrorMessage!);
                        
                        await _auditRepository.AdicionarAsync(new PaymentAudit(
                            payment.Id, PaymentAuditAction.Failed, 
                            $"Payment failed: {processorResult.ErrorMessage}", request.UserId));

                        return PaymentResult.FailureResult(processorResult.ErrorMessage!);
                    }

                    payment.PaymentItems.Add(paymentItem);
                }

                // Mark payment as processed if all items succeeded
                payment.ProcessPayment(payment.TransactionId!, payment.AuthorizationCode!);

                // Save payment
                await _paymentRepository.AdicionarAsync(payment);

                // Create audit log
                await _auditRepository.AdicionarAsync(new PaymentAudit(
                    payment.Id, PaymentAuditAction.Processed, 
                    "Payment processed successfully", request.UserId));

                // Generate fiscal receipt if requested
                FiscalReceipt? fiscalReceipt = null;
                if (request.GenerateFiscalReceipt)
                {
                    var fiscalResult = await _fiscalService.GenerateFiscalReceiptAsync(payment.Id, cancellationToken);
                    if (fiscalResult.Success)
                    {
                        fiscalReceipt = fiscalResult.FiscalReceipt;
                        await _auditRepository.AdicionarAsync(new PaymentAudit(
                            payment.Id, PaymentAuditAction.FiscalReceiptGenerated, 
                            "Fiscal receipt generated", request.UserId));
                    }
                    else
                    {
                        _logger.LogWarning("Failed to generate fiscal receipt for payment {PaymentId}: {Error}", 
                            payment.Id, fiscalResult.ErrorMessage);
                    }
                }

                _logger.LogInformation("Payment {PaymentId} processed successfully for order {OrderId}", 
                    payment.Id, request.OrderId);

                var result = PaymentResult.SuccessResult(payment, payment.TransactionId, payment.AuthorizationCode);
                result.FiscalReceipt = fiscalReceipt;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}", request.OrderId);
                return PaymentResult.FailureResult($"Internal error: {ex.Message}");
            }
        }

        public async Task<PaymentResult> RefundPaymentAsync(Guid paymentId, decimal amount, string reason, CancellationToken cancellationToken = default)
        {
            try
            {
                var payment = await _paymentRepository.BuscarPorIdAsync(paymentId);
                if (payment == null)
                {
                    return PaymentResult.FailureResult("Payment not found");
                }

                if (payment.Status != PaymentStatus.Processed)
                {
                    return PaymentResult.FailureResult("Payment cannot be refunded");
                }

                // Process refund with payment processor
                var refundResult = await _processorService.RefundTransactionAsync(payment.TransactionId!, amount, cancellationToken);
                
                if (!refundResult.Success)
                {
                    return PaymentResult.FailureResult(refundResult.ErrorMessage!);
                }

                // Update payment status
                payment.Status = PaymentStatus.Refunded;
                await _paymentRepository.AlterarAsync(payment);

                _logger.LogInformation("Payment {PaymentId} refunded successfully. Amount: {Amount}, Reason: {Reason}", 
                    paymentId, amount, reason);

                return PaymentResult.SuccessResult(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding payment {PaymentId}", paymentId);
                return PaymentResult.FailureResult($"Internal error: {ex.Message}");
            }
        }

        public async Task<PaymentResult> CancelPaymentAsync(Guid paymentId, string reason, CancellationToken cancellationToken = default)
        {
            try
            {
                var payment = await _paymentRepository.BuscarPorIdAsync(paymentId);
                if (payment == null)
                {
                    return PaymentResult.FailureResult("Payment not found");
                }

                if (payment.Status == PaymentStatus.Processed)
                {
                    return PaymentResult.FailureResult("Processed payments cannot be cancelled, use refund instead");
                }

                // Cancel with payment processor if transaction exists
                if (!string.IsNullOrEmpty(payment.TransactionId))
                {
                    await _processorService.CancelTransactionAsync(payment.TransactionId, cancellationToken);
                }

                payment.CancelPayment();
                await _paymentRepository.AlterarAsync(payment);

                _logger.LogInformation("Payment {PaymentId} cancelled. Reason: {Reason}", paymentId, reason);

                return PaymentResult.SuccessResult(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment {PaymentId}", paymentId);
                return PaymentResult.FailureResult($"Internal error: {ex.Message}");
            }
        }

        public async Task<Model.Payment?> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await _paymentRepository.GetWithItemsAsync(paymentId, cancellationToken);
        }

        public async Task<IEnumerable<Model.Payment>> GetPaymentsByOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _paymentRepository.GetByOrderIdAsync(orderId, cancellationToken);
        }

        public async Task<bool> ValidatePaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default)
        {
            if (request.TotalAmount <= 0)
                return false;

            if (!request.PaymentMethods.Any())
                return false;

            var totalMethodAmount = request.PaymentMethods.Sum(m => m.Amount);
            if (Math.Abs(totalMethodAmount - request.TotalAmount) > 0.01m)
                return false;

            return true;
        }

        private async Task<PaymentProcessorResult> ProcessPaymentMethodAsync(PaymentMethodRequest methodRequest, CancellationToken cancellationToken)
        {
            return methodRequest.Method switch
            {
                PaymentMethod.Cash => PaymentProcessorResult.SuccessResult(
                    Guid.NewGuid().ToString(), "CASH_" + DateTime.UtcNow.Ticks),
                
                PaymentMethod.CreditCard => await _processorService.ProcessCreditCardAsync(
                    new CreditCardPaymentRequest
                    {
                        Amount = methodRequest.Amount,
                        CardNumber = methodRequest.CardNumber!,
                        CardHolderName = methodRequest.CardHolderName!,
                        ExpiryDate = methodRequest.ExpiryDate!,
                        SecurityCode = methodRequest.SecurityCode!,
                        OrderId = Guid.NewGuid().ToString()
                    }, cancellationToken),
                
                PaymentMethod.DebitCard => await _processorService.ProcessDebitCardAsync(
                    new DebitCardPaymentRequest
                    {
                        Amount = methodRequest.Amount,
                        CardNumber = methodRequest.CardNumber!,
                        CardHolderName = methodRequest.CardHolderName!,
                        ExpiryDate = methodRequest.ExpiryDate!,
                        SecurityCode = methodRequest.SecurityCode!,
                        OrderId = Guid.NewGuid().ToString()
                    }, cancellationToken),
                
                PaymentMethod.Pix => PaymentProcessorResult.SuccessResult(
                    Guid.NewGuid().ToString(), "PIX_" + DateTime.UtcNow.Ticks),
                
                _ => PaymentProcessorResult.FailureResult("Unsupported payment method")
            };
        }

        private static string? MaskCardNumber(string? cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 8)
                return cardNumber;

            return cardNumber.Substring(0, 4) + "****" + cardNumber.Substring(cardNumber.Length - 4);
        }
    }
}