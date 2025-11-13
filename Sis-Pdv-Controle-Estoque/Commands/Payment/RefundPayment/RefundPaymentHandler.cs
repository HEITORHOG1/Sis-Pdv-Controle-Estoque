using MediatR;
using Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Commands.Payment.RefundPayment
{
    public class RefundPaymentHandler : IRequestHandler<RefundPaymentRequest, RefundPaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<RefundPaymentHandler> _logger;

        public RefundPaymentHandler(
            IPaymentService paymentService,
            ILogger<RefundPaymentHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<RefundPaymentResponse> Handle(RefundPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing refund for payment {PaymentId} with amount {Amount}. Reason: {Reason}", 
                    request.PaymentId, request.Amount, request.Reason);

                var result = await _paymentService.RefundPaymentAsync(request.PaymentId, request.Amount, request.Reason, cancellationToken);

                if (result.Success)
                {
                    _logger.LogInformation("Refund processed successfully for payment {PaymentId}. Transaction: {TransactionId}", 
                        request.PaymentId, result.TransactionId);

                    return new RefundPaymentResponse
                    {
                        Success = true,
                        RefundId = result.Payment?.Id,
                        RefundTransactionId = result.TransactionId,
                        RefundAmount = request.Amount,
                        ProcessedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    _logger.LogWarning("Refund processing failed for payment {PaymentId}: {Error}", 
                        request.PaymentId, result.ErrorMessage);

                    return new RefundPaymentResponse
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", request.PaymentId);
                
                return new RefundPaymentResponse
                {
                    Success = false,
                    ErrorMessage = $"Internal error: {ex.Message}"
                };
            }
        }
    }
}