using Interfaces.Repositories;
using Interfaces.Services;
using Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public class FiscalService : IFiscalService
    {
        private readonly IRepositoryFiscalReceipt _fiscalReceiptRepository;
        private readonly IRepositoryPayment _paymentRepository;
        private readonly IRepositoryPaymentAudit _auditRepository;
        private readonly ILogger<FiscalService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IConnection _rabbitConnection;

        public FiscalService(
            IRepositoryFiscalReceipt fiscalReceiptRepository,
            IRepositoryPayment paymentRepository,
            IRepositoryPaymentAudit auditRepository,
            ILogger<FiscalService> logger,
            IConfiguration configuration,
            IConnection rabbitConnection)
        {
            _fiscalReceiptRepository = fiscalReceiptRepository;
            _paymentRepository = paymentRepository;
            _auditRepository = auditRepository;
            _logger = logger;
            _configuration = configuration;
            _rabbitConnection = rabbitConnection;
        }

        public async Task<FiscalReceiptResult> GenerateFiscalReceiptAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Generating fiscal receipt for payment {PaymentId}", paymentId);

                var payment = await _paymentRepository.GetWithItemsAsync(paymentId, cancellationToken);
                if (payment == null)
                {
                    return FiscalReceiptResult.FailureResult("Payment not found");
                }

                if (payment.Status != PaymentStatus.Processed)
                {
                    return FiscalReceiptResult.FailureResult("Payment must be processed before generating fiscal receipt");
                }

                // Check if fiscal receipt already exists
                var existingReceipt = await _fiscalReceiptRepository.GetByPaymentIdAsync(paymentId, cancellationToken);
                if (existingReceipt != null)
                {
                    return FiscalReceiptResult.SuccessResult(existingReceipt);
                }

                // Generate receipt number
                var receiptNumber = await _fiscalReceiptRepository.GenerateNextReceiptNumberAsync(cancellationToken);
                
                // Create fiscal receipt
                var fiscalReceipt = new FiscalReceipt
                {
                    PaymentId = paymentId,
                    ReceiptNumber = receiptNumber,
                    SerialNumber = GenerateSerialNumber(),
                    IssuedAt = DateTime.UtcNow,
                    Status = FiscalReceiptStatus.Pending
                };

                // Generate XML content
                fiscalReceipt.XmlContent = await GenerateFiscalReceiptXmlAsync(paymentId, cancellationToken);

                // Save fiscal receipt
                await _fiscalReceiptRepository.AdicionarAsync(fiscalReceipt);

                // Send to SEFAZ via RabbitMQ
                await SendToSefazAsync(fiscalReceipt);

                // For demo purposes, simulate SEFAZ approval
                await SimulateSefazProcessingAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} generated for payment {PaymentId}", 
                    receiptNumber, paymentId);

                return FiscalReceiptResult.SuccessResult(fiscalReceipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating fiscal receipt for payment {PaymentId}", paymentId);
                return FiscalReceiptResult.FailureResult($"Internal error: {ex.Message}");
            }
        }

        public async Task<FiscalReceiptResult> CancelFiscalReceiptAsync(Guid fiscalReceiptId, string reason, CancellationToken cancellationToken = default)
        {
            try
            {
                var fiscalReceipt = await _fiscalReceiptRepository.BuscarPorIdAsync(fiscalReceiptId);
                if (fiscalReceipt == null)
                {
                    return FiscalReceiptResult.FailureResult("Fiscal receipt not found");
                }

                if (fiscalReceipt.Status != FiscalReceiptStatus.Authorized)
                {
                    return FiscalReceiptResult.FailureResult("Only authorized receipts can be cancelled");
                }

                // Send cancellation to SEFAZ via RabbitMQ
                await SendCancellationToSefazAsync(fiscalReceipt, reason);

                // Update fiscal receipt
                fiscalReceipt.Cancel(reason);
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} cancelled. Reason: {Reason}", 
                    fiscalReceipt.ReceiptNumber, reason);

                return FiscalReceiptResult.SuccessResult(fiscalReceipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling fiscal receipt {FiscalReceiptId}", fiscalReceiptId);
                return FiscalReceiptResult.FailureResult($"Internal error: {ex.Message}");
            }
        }

        public async Task<FiscalReceiptResult> GetFiscalReceiptStatusAsync(Guid fiscalReceiptId, CancellationToken cancellationToken = default)
        {
            var fiscalReceipt = await _fiscalReceiptRepository.BuscarPorIdAsync(fiscalReceiptId);
            if (fiscalReceipt == null)
            {
                return FiscalReceiptResult.FailureResult("Fiscal receipt not found");
            }

            return FiscalReceiptResult.SuccessResult(fiscalReceipt);
        }

        public async Task<byte[]> GenerateFiscalReceiptPdfAsync(Guid fiscalReceiptId, CancellationToken cancellationToken = default)
        {
            var fiscalReceipt = await _fiscalReceiptRepository.BuscarPorIdAsync(fiscalReceiptId);
            if (fiscalReceipt == null)
            {
                throw new ArgumentException("Fiscal receipt not found");
            }

            // Generate PDF content (simplified implementation)
            var pdfContent = $@"
CUPOM FISCAL ELETRÔNICO
Número: {fiscalReceipt.ReceiptNumber}
Série: {fiscalReceipt.SerialNumber}
Data/Hora: {fiscalReceipt.IssuedAt:dd/MM/yyyy HH:mm:ss}
Status: {fiscalReceipt.Status}
Protocolo SEFAZ: {fiscalReceipt.SefazProtocol}
Chave de Acesso: {fiscalReceipt.AccessKey}
";

            return Encoding.UTF8.GetBytes(pdfContent);
        }

        public async Task<string> GenerateFiscalReceiptXmlAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetWithItemsAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                throw new ArgumentException("Payment not found");
            }

            // Generate simplified XML structure for fiscal receipt
            var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<nfce xmlns=""http://www.portalfiscal.inf.br/nfce"">
    <infNFe>
        <ide>
            <cUF>35</cUF>
            <cNF>{DateTime.UtcNow.Ticks % 100000000:D8}</cNF>
            <natOp>Venda</natOp>
            <mod>65</mod>
            <serie>1</serie>
            <nNF>{DateTime.UtcNow.Ticks % 1000000:D6}</nNF>
            <dhEmi>{payment.PaymentDate:yyyy-MM-ddTHH:mm:sszzz}</dhEmi>
            <tpNF>1</tpNF>
            <idDest>1</idDest>
            <cMunFG>3550308</cMunFG>
            <tpImp>4</tpImp>
            <tpEmis>1</tpEmis>
            <tpAmb>2</tpAmb>
            <finNFe>1</finNFe>
            <indFinal>1</indFinal>
            <indPres>1</indPres>
        </ide>
        <total>
            <ICMSTot>
                <vBC>0.00</vBC>
                <vICMS>0.00</vICMS>
                <vICMSDeson>0.00</vICMSDeson>
                <vFCP>0.00</vFCP>
                <vBCST>0.00</vBCST>
                <vST>0.00</vST>
                <vFCPST>0.00</vFCPST>
                <vFCPSTRet>0.00</vFCPSTRet>
                <vProd>{payment.TotalAmount:F2}</vProd>
                <vFrete>0.00</vFrete>
                <vSeg>0.00</vSeg>
                <vDesc>0.00</vDesc>
                <vII>0.00</vII>
                <vIPI>0.00</vIPI>
                <vIPIDevol>0.00</vIPIDevol>
                <vPIS>0.00</vPIS>
                <vCOFINS>0.00</vCOFINS>
                <vOutro>0.00</vOutro>
                <vNF>{payment.TotalAmount:F2}</vNF>
            </ICMSTot>
        </total>
    </infNFe>
</nfce>";

            return xml;
        }

        private async Task SendToSefazAsync(FiscalReceipt fiscalReceipt)
        {
            try
            {
                using var channel = _rabbitConnection.CreateModel();
                
                // Declare exchange and queue for SEFAZ integration
                channel.ExchangeDeclare("sefaz.exchange", ExchangeType.Direct, durable: true);
                channel.QueueDeclare("sefaz.fiscal.receipt", durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind("sefaz.fiscal.receipt", "sefaz.exchange", "fiscal.receipt");

                var message = new
                {
                    FiscalReceiptId = fiscalReceipt.Id,
                    ReceiptNumber = fiscalReceipt.ReceiptNumber,
                    XmlContent = fiscalReceipt.XmlContent,
                    Timestamp = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                
                channel.BasicPublish(
                    exchange: "sefaz.exchange",
                    routingKey: "fiscal.receipt",
                    basicProperties: null,
                    body: body);

                fiscalReceipt.Status = FiscalReceiptStatus.Sent;
                fiscalReceipt.SentToSefazAt = DateTime.UtcNow;
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} sent to SEFAZ via RabbitMQ", 
                    fiscalReceipt.ReceiptNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending fiscal receipt {ReceiptNumber} to SEFAZ", 
                    fiscalReceipt.ReceiptNumber);
                throw;
            }
        }

        private async Task SendCancellationToSefazAsync(FiscalReceipt fiscalReceipt, string reason)
        {
            try
            {
                using var channel = _rabbitConnection.CreateModel();
                
                var message = new
                {
                    FiscalReceiptId = fiscalReceipt.Id,
                    ReceiptNumber = fiscalReceipt.ReceiptNumber,
                    AccessKey = fiscalReceipt.AccessKey,
                    CancellationReason = reason,
                    Timestamp = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                
                channel.BasicPublish(
                    exchange: "sefaz.exchange",
                    routingKey: "fiscal.receipt.cancellation",
                    basicProperties: null,
                    body: body);

                _logger.LogInformation("Fiscal receipt cancellation {ReceiptNumber} sent to SEFAZ via RabbitMQ", 
                    fiscalReceipt.ReceiptNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending fiscal receipt cancellation {ReceiptNumber} to SEFAZ", 
                    fiscalReceipt.ReceiptNumber);
                throw;
            }
        }

        private async Task SimulateSefazProcessingAsync(FiscalReceipt fiscalReceipt)
        {
            // Simulate SEFAZ processing delay
            await Task.Delay(2000);

            // Simulate approval (in real implementation, this would come from SEFAZ response)
            var accessKey = GenerateAccessKey();
            var protocol = GenerateSefazProtocol();
            var qrCode = GenerateQrCode(accessKey);

            fiscalReceipt.Authorize(protocol, accessKey, qrCode);
            await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

            _logger.LogInformation("Fiscal receipt {ReceiptNumber} authorized by SEFAZ. Protocol: {Protocol}", 
                fiscalReceipt.ReceiptNumber, protocol);
        }

        private static string GenerateSerialNumber()
        {
            return $"001{DateTime.UtcNow:yyyyMMdd}{DateTime.UtcNow.Ticks % 1000:D3}";
        }

        private static string GenerateAccessKey()
        {
            // Generate a 44-digit access key (simplified)
            var random = new Random();
            var accessKey = "";
            for (int i = 0; i < 44; i++)
            {
                accessKey += random.Next(0, 10).ToString();
            }
            return accessKey;
        }

        private static string GenerateSefazProtocol()
        {
            return $"135{DateTime.UtcNow:yyyyMMddHHmmss}{DateTime.UtcNow.Ticks % 1000:D3}";
        }

        private static string GenerateQrCode(string accessKey)
        {
            return $"https://www.sefaz.sp.gov.br/nfce/qrcode?p={accessKey}|2|2|1|{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}