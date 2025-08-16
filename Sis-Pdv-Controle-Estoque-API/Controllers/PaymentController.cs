using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Commands.Payment.ProcessPayment;
using Commands.Payment.RefundPayment;
using Commands.Payment.CancelPayment;
using Interfaces.Services;
using Sis_Pdv_Controle_Estoque_API.Services.Payment;


namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PaymentController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentReconciliationService _reconciliationService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IMediator mediator,
            IPaymentService paymentService,
            IPaymentReconciliationService reconciliationService,
            ILogger<PaymentController> logger,
            Repositories.Transactions.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mediator = mediator;
            _paymentService = paymentService;
            _reconciliationService = reconciliationService;
            _logger = logger;
        }

        /// <summary>
        /// Process a payment for an order
        /// </summary>
        /// <param name="request">Payment processing request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Payment processing result</returns>
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Commands.Payment.ProcessPayment.ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing payment request for order {OrderId}", request.OrderId);

                var result = await _mediator.Send(request, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            paymentId = result.PaymentId,
                            transactionId = result.TransactionId,
                            authorizationCode = result.AuthorizationCode,
                            fiscalReceiptId = result.FiscalReceiptId,
                            receiptNumber = result.ReceiptNumber
                        },
                        message = "Payment processed successfully"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}", request.OrderId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while processing payment"
                });
            }
        }

        /// <summary>
        /// Refund a payment
        /// </summary>
        /// <param name="request">Refund request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Refund result</returns>
        [HttpPost("refund")]
        public async Task<IActionResult> RefundPayment([FromBody] RefundPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing refund request for payment {PaymentId}", request.PaymentId);

                var result = await _mediator.Send(request, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            refundId = result.RefundId,
                            refundTransactionId = result.RefundTransactionId,
                            refundAmount = result.RefundAmount,
                            processedAt = result.ProcessedAt
                        },
                        message = "Refund processed successfully"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", request.PaymentId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while processing refund"
                });
            }
        }

        /// <summary>
        /// Cancel a payment
        /// </summary>
        /// <param name="request">Cancellation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Cancellation result</returns>
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPayment([FromBody] CancelPaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing cancellation request for payment {PaymentId}", request.PaymentId);

                var result = await _mediator.Send(request, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            paymentId = result.PaymentId,
                            cancelledAt = result.CancelledAt
                        },
                        message = "Payment cancelled successfully"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment {PaymentId}", request.PaymentId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while cancelling payment"
                });
            }
        }

        /// <summary>
        /// Get payment details
        /// </summary>
        /// <param name="paymentId">Payment ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Payment details</returns>
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(Guid paymentId, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentService.GetPaymentAsync(paymentId, cancellationToken);

                if (payment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Payment not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = payment.Id,
                        orderId = payment.OrderId,
                        totalAmount = payment.TotalAmount,
                        status = payment.Status.ToString(),
                        paymentDate = payment.PaymentDate,
                        transactionId = payment.TransactionId,
                        authorizationCode = payment.AuthorizationCode,
                        paymentItems = payment.PaymentItems.Select(item => new
                        {
                            id = item.Id,
                            method = item.Method.ToString(),
                            amount = item.Amount,
                            status = item.Status.ToString(),
                            cardNumber = item.CardNumber,
                            cardHolderName = item.CardHolderName,
                            installments = item.Installments,
                            pixKey = item.PixKey,
                            processorTransactionId = item.ProcessorTransactionId,
                            authorizationCode = item.AuthorizationCode,
                            processedAt = item.ProcessedAt
                        }),
                        fiscalReceipt = payment.FiscalReceipt != null ? new
                        {
                            id = payment.FiscalReceipt.Id,
                            receiptNumber = payment.FiscalReceipt.ReceiptNumber,
                            status = payment.FiscalReceipt.Status.ToString(),
                            sefazProtocol = payment.FiscalReceipt.SefazProtocol,
                            accessKey = payment.FiscalReceipt.AccessKey,
                            qrCode = payment.FiscalReceipt.QrCode,
                            issuedAt = payment.FiscalReceipt.IssuedAt
                        } : null
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment {PaymentId}", paymentId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while retrieving payment"
                });
            }
        }

        /// <summary>
        /// Get payments for an order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of payments for the order</returns>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrder(Guid orderId, CancellationToken cancellationToken)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByOrderAsync(orderId, cancellationToken);

                return Ok(new
                {
                    success = true,
                    data = payments.Select(payment => new
                    {
                        id = payment.Id,
                        totalAmount = payment.TotalAmount,
                        status = payment.Status.ToString(),
                        paymentDate = payment.PaymentDate,
                        transactionId = payment.TransactionId,
                        paymentItemsCount = payment.PaymentItems.Count
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments for order {OrderId}", orderId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while retrieving payments"
                });
            }
        }

        /// <summary>
        /// Reconcile payments for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Reconciliation result</returns>
        [HttpPost("reconcile")]
        public async Task<IActionResult> ReconcilePayments([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting payment reconciliation for period {StartDate} to {EndDate}", startDate, endDate);

                var result = await _reconciliationService.ReconcilePaymentsAsync(startDate, endDate, cancellationToken);

                return Ok(new
                {
                    success = result.Success,
                    data = new
                    {
                        startDate = result.StartDate,
                        endDate = result.EndDate,
                        totalPayments = result.TotalPayments,
                        reconciledCount = result.ReconciledCount,
                        failedCount = result.FailedCount,
                        discrepanciesCount = result.Discrepancies.Count(),
                        discrepancies = result.Discrepancies.Select(d => new
                        {
                            id = d.Id,
                            paymentId = d.PaymentId,
                            type = d.Type.ToString(),
                            description = d.Description,
                            severity = d.Severity.ToString(),
                            detectedAt = d.DetectedAt,
                            isResolved = d.IsResolved
                        }),
                        processedAt = result.ProcessedAt
                    },
                    message = result.Success ? "Reconciliation completed successfully" : result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment reconciliation");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred during reconciliation"
                });
            }
        }

        /// <summary>
        /// Get payment discrepancies for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of payment discrepancies</returns>
        [HttpGet("discrepancies")]
        public async Task<IActionResult> GetPaymentDiscrepancies([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                var discrepancies = await _reconciliationService.GetPaymentDiscrepanciesAsync(startDate, endDate, cancellationToken);

                return Ok(new
                {
                    success = true,
                    data = discrepancies.Select(d => new
                    {
                        id = d.Id,
                        paymentId = d.PaymentId,
                        type = d.Type.ToString(),
                        description = d.Description,
                        severity = d.Severity.ToString(),
                        detectedAt = d.DetectedAt,
                        isResolved = d.IsResolved,
                        resolution = d.Resolution,
                        resolvedAt = d.ResolvedAt,
                        resolvedBy = d.ResolvedBy
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment discrepancies");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while retrieving discrepancies"
                });
            }
        }

        /// <summary>
        /// Resolve a payment discrepancy
        /// </summary>
        /// <param name="discrepancyId">Discrepancy ID</param>
        /// <param name="request">Resolution request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Resolution result</returns>
        [HttpPost("discrepancies/{discrepancyId}/resolve")]
        public async Task<IActionResult> ResolveDiscrepancy(Guid discrepancyId, [FromBody] ResolveDiscrepancyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetCurrentUserId();

                var success = await _reconciliationService.ResolveDiscrepancyAsync(discrepancyId, request.Resolution, userId, cancellationToken);

                if (success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Discrepancy resolved successfully"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Failed to resolve discrepancy"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving discrepancy {DiscrepancyId}", discrepancyId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while resolving discrepancy"
                });
            }
        }
    }

    public class ResolveDiscrepancyRequest
    {
        public string Resolution { get; set; } = string.Empty;
    }
}