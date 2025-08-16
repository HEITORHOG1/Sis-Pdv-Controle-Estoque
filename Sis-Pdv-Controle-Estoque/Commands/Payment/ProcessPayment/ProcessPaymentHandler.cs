using MediatR;
using Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Commands.Payment.ProcessPayment
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentRequest, ProcessPaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<ProcessPaymentHandler> _logger;

        public ProcessPaymentHandler(
            IPaymentService paymentService,
            ILogger<ProcessPaymentHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<ProcessPaymentResponse> Handle(ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing payment for order {OrderId} with amount {Amount}", 
                    request.OrderId, request.TotalAmount);

                var paymentRequest = new Interfaces.Services.ProcessPaymentRequest
                {
                    OrderId = request.OrderId,
                    TotalAmount = request.TotalAmount,
                    PaymentMethods = request.PaymentMethods,
                    GenerateFiscalReceipt = request.GenerateFiscalReceipt,
                    UserId = request.UserId
                };

                var result = await _paymentService.ProcessPaymentAsync(paymentRequest, cancellationToken);

                if (result.Success)
                {
                    _logger.LogInformation("Payment processed successfully for order {OrderId}. Payment ID: {PaymentId}", 
                        request.OrderId, result.Payment?.Id);

                    return new ProcessPaymentResponse
                    {
                        Success = true,
                        PaymentId = result.Payment?.Id,
                        TransactionId = result.TransactionId,
                        AuthorizationCode = result.AuthorizationCode,
                        FiscalReceiptId = result.FiscalReceipt?.Id,
                        ReceiptNumber = result.FiscalReceipt?.ReceiptNumber
                    };
                }
                else
                {
                    _logger.LogWarning("Payment processing failed for order {OrderId}: {Error}", 
                        request.OrderId, result.ErrorMessage);

                    return new ProcessPaymentResponse
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}", request.OrderId);
                
                return new ProcessPaymentResponse
                {
                    Success = false,
                    ErrorMessage = $"Internal error: {ex.Message}"
                };
            }
        }
    }
}