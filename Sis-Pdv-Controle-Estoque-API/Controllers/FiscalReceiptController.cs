using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Interfaces.Services;


namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class FiscalReceiptController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IFiscalService _fiscalService;
        private readonly ILogger<FiscalReceiptController> _logger;

        public FiscalReceiptController(
            IFiscalService fiscalService,
            ILogger<FiscalReceiptController> logger)
        {
            _fiscalService = fiscalService;
            _logger = logger;
        }

        /// <summary>
        /// Generate fiscal receipt for a payment
        /// </summary>
        /// <param name="paymentId">Payment ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Fiscal receipt generation result</returns>
        [HttpPost("generate/{paymentId}")]
        public async Task<IActionResult> GenerateFiscalReceipt(Guid paymentId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Generating fiscal receipt for payment {PaymentId}", paymentId);

                var result = await _fiscalService.GenerateFiscalReceiptAsync(paymentId, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            fiscalReceiptId = result.FiscalReceipt!.Id,
                            receiptNumber = result.FiscalReceipt.ReceiptNumber,
                            serialNumber = result.FiscalReceipt.SerialNumber,
                            status = result.FiscalReceipt.Status.ToString(),
                            issuedAt = result.FiscalReceipt.IssuedAt,
                            sefazProtocol = result.FiscalReceipt.SefazProtocol,
                            accessKey = result.FiscalReceipt.AccessKey,
                            qrCode = result.FiscalReceipt.QrCode
                        },
                        message = "Fiscal receipt generated successfully"
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
                _logger.LogError(ex, "Error generating fiscal receipt for payment {PaymentId}", paymentId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while generating fiscal receipt"
                });
            }
        }

        /// <summary>
        /// Cancel a fiscal receipt
        /// </summary>
        /// <param name="fiscalReceiptId">Fiscal receipt ID</param>
        /// <param name="request">Cancellation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Cancellation result</returns>
        [HttpPost("{fiscalReceiptId}/cancel")]
        public async Task<IActionResult> CancelFiscalReceipt(Guid fiscalReceiptId, [FromBody] CancelFiscalReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Cancelling fiscal receipt {FiscalReceiptId}. Reason: {Reason}", 
                    fiscalReceiptId, request.Reason);

                var result = await _fiscalService.CancelFiscalReceiptAsync(fiscalReceiptId, request.Reason, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            fiscalReceiptId = result.FiscalReceipt!.Id,
                            status = result.FiscalReceipt.Status.ToString(),
                            cancellationReason = result.FiscalReceipt.CancellationReason,
                            cancelledAt = result.FiscalReceipt.CancelledAt
                        },
                        message = "Fiscal receipt cancelled successfully"
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
                _logger.LogError(ex, "Error cancelling fiscal receipt {FiscalReceiptId}", fiscalReceiptId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while cancelling fiscal receipt"
                });
            }
        }

        /// <summary>
        /// Get fiscal receipt status
        /// </summary>
        /// <param name="fiscalReceiptId">Fiscal receipt ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Fiscal receipt status</returns>
        [HttpGet("{fiscalReceiptId}/status")]
        public async Task<IActionResult> GetFiscalReceiptStatus(Guid fiscalReceiptId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fiscalService.GetFiscalReceiptStatusAsync(fiscalReceiptId, cancellationToken);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            id = result.FiscalReceipt!.Id,
                            receiptNumber = result.FiscalReceipt.ReceiptNumber,
                            serialNumber = result.FiscalReceipt.SerialNumber,
                            status = result.FiscalReceipt.Status.ToString(),
                            issuedAt = result.FiscalReceipt.IssuedAt,
                            sefazProtocol = result.FiscalReceipt.SefazProtocol,
                            accessKey = result.FiscalReceipt.AccessKey,
                            qrCode = result.FiscalReceipt.QrCode,
                            sentToSefazAt = result.FiscalReceipt.SentToSefazAt,
                            authorizedAt = result.FiscalReceipt.AuthorizedAt,
                            errorMessage = result.FiscalReceipt.ErrorMessage,
                            cancellationReason = result.FiscalReceipt.CancellationReason,
                            cancelledAt = result.FiscalReceipt.CancelledAt
                        }
                    });
                }

                return NotFound(new
                {
                    success = false,
                    message = result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving fiscal receipt status {FiscalReceiptId}", fiscalReceiptId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while retrieving fiscal receipt status"
                });
            }
        }

        /// <summary>
        /// Download fiscal receipt PDF
        /// </summary>
        /// <param name="fiscalReceiptId">Fiscal receipt ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PDF file</returns>
        [HttpGet("{fiscalReceiptId}/pdf")]
        public async Task<IActionResult> DownloadFiscalReceiptPdf(Guid fiscalReceiptId, CancellationToken cancellationToken)
        {
            try
            {
                var pdfBytes = await _fiscalService.GenerateFiscalReceiptPdfAsync(fiscalReceiptId, cancellationToken);

                return File(pdfBytes, "application/pdf", $"fiscal-receipt-{fiscalReceiptId}.pdf");
            }
            catch (ArgumentException)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Fiscal receipt not found"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for fiscal receipt {FiscalReceiptId}", fiscalReceiptId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while generating PDF"
                });
            }
        }

        /// <summary>
        /// Download fiscal receipt XML
        /// </summary>
        /// <param name="fiscalReceiptId">Fiscal receipt ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>XML file</returns>
        [HttpGet("{fiscalReceiptId}/xml")]
        public async Task<IActionResult> DownloadFiscalReceiptXml(Guid fiscalReceiptId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fiscalService.GetFiscalReceiptStatusAsync(fiscalReceiptId, cancellationToken);

                if (!result.Success || result.FiscalReceipt == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Fiscal receipt not found"
                    });
                }

                if (string.IsNullOrEmpty(result.FiscalReceipt.XmlContent))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "XML content not available for this fiscal receipt"
                    });
                }

                var xmlBytes = System.Text.Encoding.UTF8.GetBytes(result.FiscalReceipt.XmlContent);
                return File(xmlBytes, "application/xml", $"fiscal-receipt-{fiscalReceiptId}.xml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading XML for fiscal receipt {FiscalReceiptId}", fiscalReceiptId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while downloading XML"
                });
            }
        }

        /// <summary>
        /// Generate fiscal receipt XML for a payment (preview)
        /// </summary>
        /// <param name="paymentId">Payment ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Generated XML content</returns>
        [HttpGet("preview/{paymentId}/xml")]
        public async Task<IActionResult> PreviewFiscalReceiptXml(Guid paymentId, CancellationToken cancellationToken)
        {
            try
            {
                var xmlContent = await _fiscalService.GenerateFiscalReceiptXmlAsync(paymentId, cancellationToken);

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        paymentId = paymentId,
                        xmlContent = xmlContent,
                        generatedAt = DateTime.UtcNow
                    }
                });
            }
            catch (ArgumentException)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Payment not found"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating XML preview for payment {PaymentId}", paymentId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error occurred while generating XML preview"
                });
            }
        }
    }

    public class CancelFiscalReceiptRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}