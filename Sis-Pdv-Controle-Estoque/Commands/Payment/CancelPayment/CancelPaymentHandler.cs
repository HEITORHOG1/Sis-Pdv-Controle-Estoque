using MediatR;
using Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Commands.Payment.CancelPayment
{
    public class CancelPaymentHandler : IRequestHandler<CancelPaymentRequest, CancelPaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<CancelPaymentHandler> _logger;

        public CancelPaymentHandler(
            IPaymentService paymentService,
            ILogger<CancelPaymentHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<CancelPaymentResponse> Handle(CancelPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Cancelling payment {PaymentId}. Reason: {Reason}", 
                    request.PaymentId, request.Reason);

                var result = await _paymentService.CancelPaymentAsync(request.PaymentId, request.Reason, cancellationToken);

                if (result.Success)
                {
                    _logger.LogInformation("Payment {PaymentId} cancelled successfully", request.PaymentId);

                    return new CancelPaymentResponse
                    {
                        Success = true,
                        PaymentId = result.Payment?.Id,
                        CancelledAt = DateTime.UtcNow
                    };
                }
                else
                {
                    _logger.LogWarning("Payment cancellation failed for payment {PaymentId}: {Error}", 
                        request.PaymentId, result.ErrorMessage);

                    return new CancelPaymentResponse
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment {PaymentId}", request.PaymentId);
                
                return new CancelPaymentResponse
                {
                    Success = false,
                    ErrorMessage = $"Internal error: {ex.Message}"
                };
            }
        }
    }
}