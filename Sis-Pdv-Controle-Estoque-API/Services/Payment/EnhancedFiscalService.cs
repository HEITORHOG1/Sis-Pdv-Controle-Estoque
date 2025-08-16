using Interfaces.Repositories;
using Interfaces.Services;
using Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sis_Pdv_Controle_Estoque_API.Configuration;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public class EnhancedFiscalService : IFiscalService
    {
        private readonly IRepositoryFiscalReceipt _fiscalReceiptRepository;
        private readonly IRepositoryPayment _paymentRepository;
        private readonly IRepositoryPaymentAudit _auditRepository;
        private readonly ILogger<EnhancedFiscalService> _logger;
        private readonly SefazConfiguration _sefazConfig;
        private readonly IConnection _rabbitConnection;

        public EnhancedFiscalService(
            IRepositoryFiscalReceipt fiscalReceiptRepository,
            IRepositoryPayment paymentRepository,
            IRepositoryPaymentAudit auditRepository,
            ILogger<EnhancedFiscalService> logger,
            IOptions<SefazConfiguration> sefazConfig,
            IConnection rabbitConnection)
        {
            _fiscalReceiptRepository = fiscalReceiptRepository;
            _paymentRepository = paymentRepository;
            _auditRepository = auditRepository;
            _logger = logger;
            _sefazConfig = sefazConfig.Value;
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

                // Generate XML content based on SEFAZ standards
                fiscalReceipt.XmlContent = await GenerateNFCeXmlAsync(payment, fiscalReceipt, cancellationToken);

                // Save fiscal receipt
                await _fiscalReceiptRepository.AdicionarAsync(fiscalReceipt);

                // Create audit log
                await _auditRepository.AdicionarAsync(new PaymentAudit(
                    paymentId, PaymentAuditAction.FiscalReceiptGenerated, 
                    "Fiscal receipt generated", Guid.Empty)); // TODO: Get current user ID

                // Check if should send to SEFAZ or go to contingency
                if (await ShouldSendToSefazAsync())
                {
                    await SendToSefazAsync(fiscalReceipt);
                }
                else
                {
                    await ProcessContingencyAsync(fiscalReceipt);
                }

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

                // Create audit log
                await _auditRepository.AdicionarAsync(new PaymentAudit(
                    fiscalReceipt.PaymentId, PaymentAuditAction.FiscalReceiptCancelled, 
                    $"Fiscal receipt cancelled: {reason}", Guid.Empty));

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

            // Generate enhanced PDF content
            var pdfContent = GenerateFiscalReceiptPdfContent(fiscalReceipt);
            return Encoding.UTF8.GetBytes(pdfContent);
        }

        public async Task<string> GenerateFiscalReceiptXmlAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetWithItemsAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                throw new ArgumentException("Payment not found");
            }

            var fiscalReceipt = new FiscalReceipt
            {
                PaymentId = paymentId,
                ReceiptNumber = await _fiscalReceiptRepository.GenerateNextReceiptNumberAsync(cancellationToken),
                SerialNumber = GenerateSerialNumber(),
                IssuedAt = DateTime.UtcNow
            };

            return await GenerateNFCeXmlAsync(payment, fiscalReceipt, cancellationToken);
        }

        private async Task<string> GenerateNFCeXmlAsync(Model.Payment payment, FiscalReceipt fiscalReceipt, CancellationToken cancellationToken)
        {
            try
            {
                // Generate access key
                var accessKey = GenerateAccessKey(fiscalReceipt);
                
                // Create NFCe XML structure following SEFAZ standards
                var nfce = new XElement("nfce",
                    new XAttribute("xmlns", "http://www.portalfiscal.inf.br/nfce"),
                    new XElement("infNFe",
                        new XAttribute("Id", $"NFe{accessKey}"),
                        
                        // Identification
                        new XElement("ide",
                            new XElement("cUF", GetUFCode(_sefazConfig.UF)),
                            new XElement("cNF", GenerateRandomCode(8)),
                            new XElement("natOp", "Venda"),
                            new XElement("mod", "65"), // NFC-e
                            new XElement("serie", "1"),
                            new XElement("nNF", fiscalReceipt.ReceiptNumber),
                            new XElement("dhEmi", payment.PaymentDate.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                            new XElement("tpNF", "1"), // Saída
                            new XElement("idDest", "1"), // Operação interna
                            new XElement("cMunFG", _sefazConfig.Endereco.CodigoMunicipio),
                            new XElement("tpImp", "4"), // NFC-e
                            new XElement("tpEmis", _sefazConfig.Contingencia.Habilitada ? _sefazConfig.Contingencia.TipoEmissao.ToString() : "1"),
                            new XElement("tpAmb", _sefazConfig.WebService.Ambiente.ToString()),
                            new XElement("finNFe", "1"), // Normal
                            new XElement("indFinal", "1"), // Consumidor final
                            new XElement("indPres", "1") // Presencial
                        ),

                        // Emitter
                        new XElement("emit",
                            new XElement("CNPJ", _sefazConfig.CNPJ),
                            new XElement("xNome", _sefazConfig.RazaoSocial),
                            new XElement("xFant", _sefazConfig.NomeFantasia),
                            new XElement("enderEmit",
                                new XElement("xLgr", _sefazConfig.Endereco.Logradouro),
                                new XElement("nro", _sefazConfig.Endereco.Numero),
                                new XElement("xBairro", _sefazConfig.Endereco.Bairro),
                                new XElement("cMun", _sefazConfig.Endereco.CodigoMunicipio),
                                new XElement("xMun", _sefazConfig.Endereco.NomeMunicipio),
                                new XElement("UF", _sefazConfig.Endereco.UF),
                                new XElement("CEP", _sefazConfig.Endereco.CEP)
                            ),
                            new XElement("IE", _sefazConfig.InscricaoEstadual),
                            new XElement("CRT", "3") // Regime tributário
                        ),

                        // Products/Services (simplified for payment)
                        GenerateProductsXml(payment),

                        // Totals
                        new XElement("total",
                            new XElement("ICMSTot",
                                new XElement("vBC", "0.00"),
                                new XElement("vICMS", "0.00"),
                                new XElement("vICMSDeson", "0.00"),
                                new XElement("vFCP", "0.00"),
                                new XElement("vBCST", "0.00"),
                                new XElement("vST", "0.00"),
                                new XElement("vFCPST", "0.00"),
                                new XElement("vFCPSTRet", "0.00"),
                                new XElement("vProd", payment.TotalAmount.ToString("F2")),
                                new XElement("vFrete", "0.00"),
                                new XElement("vSeg", "0.00"),
                                new XElement("vDesc", "0.00"),
                                new XElement("vII", "0.00"),
                                new XElement("vIPI", "0.00"),
                                new XElement("vIPIDevol", "0.00"),
                                new XElement("vPIS", "0.00"),
                                new XElement("vCOFINS", "0.00"),
                                new XElement("vOutro", "0.00"),
                                new XElement("vNF", payment.TotalAmount.ToString("F2"))
                            )
                        ),

                        // Payment information
                        GeneratePaymentXml(payment),

                        // Additional information
                        new XElement("infAdic",
                            new XElement("infCpl", $"Pagamento processado em {payment.PaymentDate:dd/MM/yyyy HH:mm:ss}")
                        )
                    )
                );

                return nfce.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating NFCe XML for payment {PaymentId}", payment.Id);
                throw;
            }
        }

        private XElement GenerateProductsXml(Model.Payment payment)
        {
            // Simplified product generation - in real scenario, get from order items
            return new XElement("det",
                new XAttribute("nItem", "1"),
                new XElement("prod",
                    new XElement("cProd", "001"),
                    new XElement("cEAN", ""),
                    new XElement("xProd", "Produto/Serviço"),
                    new XElement("NCM", "99999999"),
                    new XElement("CFOP", "5102"),
                    new XElement("uCom", "UN"),
                    new XElement("qCom", "1.0000"),
                    new XElement("vUnCom", payment.TotalAmount.ToString("F4")),
                    new XElement("vProd", payment.TotalAmount.ToString("F2")),
                    new XElement("cEANTrib", ""),
                    new XElement("uTrib", "UN"),
                    new XElement("qTrib", "1.0000"),
                    new XElement("vUnTrib", payment.TotalAmount.ToString("F4")),
                    new XElement("indTot", "1")
                ),
                new XElement("imposto",
                    new XElement("ICMS",
                        new XElement("ICMSSN102",
                            new XElement("orig", "0"),
                            new XElement("CSOSN", "102")
                        )
                    )
                )
            );
        }

        private XElement GeneratePaymentXml(Model.Payment payment)
        {
            var paymentXml = new XElement("pag");

            foreach (var item in payment.PaymentItems)
            {
                var detPag = new XElement("detPag",
                    new XElement("tPag", GetPaymentTypeCode(item.Method)),
                    new XElement("vPag", item.Amount.ToString("F2"))
                );

                if (item.Method == PaymentMethod.CreditCard || item.Method == PaymentMethod.DebitCard)
                {
                    detPag.Add(new XElement("card",
                        new XElement("tpIntegra", "1"), // Integrado
                        new XElement("CNPJ", "99999999000191"), // CNPJ da operadora
                        new XElement("tBand", "01"), // Bandeira do cartão
                        new XElement("cAut", item.AuthorizationCode ?? "")
                    ));
                }

                paymentXml.Add(detPag);
            }

            return paymentXml;
        }

        private async Task<bool> ShouldSendToSefazAsync()
        {
            // Check if SEFAZ is available and not in contingency mode
            // In a real implementation, this would check SEFAZ availability
            return !_sefazConfig.Contingencia.Habilitada;
        }

    private async Task SendToSefazAsync(FiscalReceipt fiscalReceipt)
        {
            try
            {
        await using var channel = await _rabbitConnection.CreateChannelAsync();
                
                // Declare exchange and queue for SEFAZ integration
                await channel.ExchangeDeclareAsync("sefaz.exchange", ExchangeType.Direct, durable: true);
                await channel.QueueDeclareAsync("sefaz.fiscal.receipt", durable: true, exclusive: false, autoDelete: false);
                await channel.QueueBindAsync("sefaz.fiscal.receipt", "sefaz.exchange", "fiscal.receipt");

                var message = new SefazMessage
                {
                    FiscalReceiptId = fiscalReceipt.Id,
                    ReceiptNumber = fiscalReceipt.ReceiptNumber,
                    XmlContent = fiscalReceipt.XmlContent!,
                    Environment = _sefazConfig.Environment,
                    UF = _sefazConfig.UF,
                    Timestamp = DateTime.UtcNow,
                    RetryCount = 0,
                    MaxRetries = _sefazConfig.Configuracoes.TentativasEnvio
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                
                await channel.BasicPublishAsync(
                    exchange: "sefaz.exchange",
                    routingKey: "fiscal.receipt",
                    body: body);

                fiscalReceipt.Status = FiscalReceiptStatus.Sent;
                fiscalReceipt.SentToSefazAt = DateTime.UtcNow;
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} sent to SEFAZ via RabbitMQ", 
                    fiscalReceipt.ReceiptNumber);

                // Simulate SEFAZ processing for demo
                _ = Task.Run(async () => await SimulateSefazProcessingAsync(fiscalReceipt));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending fiscal receipt {ReceiptNumber} to SEFAZ", 
                    fiscalReceipt.ReceiptNumber);
                
                // If sending fails, process in contingency
                await ProcessContingencyAsync(fiscalReceipt);
            }
        }

        private async Task ProcessContingencyAsync(FiscalReceipt fiscalReceipt)
        {
            try
            {
                _logger.LogWarning("Processing fiscal receipt {ReceiptNumber} in contingency mode", 
                    fiscalReceipt.ReceiptNumber);

                // In contingency mode, generate local authorization
                var accessKey = GenerateAccessKey(fiscalReceipt);
                var protocol = $"CONT{DateTime.UtcNow:yyyyMMddHHmmss}{DateTime.UtcNow.Ticks % 1000:D3}";
                var qrCode = GenerateContingencyQrCode(accessKey);

                fiscalReceipt.Authorize(protocol, accessKey, qrCode);
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} authorized in contingency mode. Protocol: {Protocol}", 
                    fiscalReceipt.ReceiptNumber, protocol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing fiscal receipt {ReceiptNumber} in contingency", 
                    fiscalReceipt.ReceiptNumber);
                
                fiscalReceipt.Reject($"Contingency processing failed: {ex.Message}");
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);
            }
        }

        private async Task SendCancellationToSefazAsync(FiscalReceipt fiscalReceipt, string reason)
        {
            try
            {
                await using var channel = await _rabbitConnection.CreateChannelAsync();
                
                var message = new SefazCancellationMessage
                {
                    FiscalReceiptId = fiscalReceipt.Id,
                    ReceiptNumber = fiscalReceipt.ReceiptNumber,
                    AccessKey = fiscalReceipt.AccessKey!,
                    CancellationReason = reason,
                    Environment = _sefazConfig.Environment,
                    UF = _sefazConfig.UF,
                    Timestamp = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                
                await channel.BasicPublishAsync(
                    exchange: "sefaz.exchange",
                    routingKey: "fiscal.receipt.cancellation",
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
            try
            {
                // Simulate SEFAZ processing delay
                await Task.Delay(_sefazConfig.Configuracoes.IntervaloTentativas);

                // Simulate approval (in real implementation, this would come from SEFAZ response)
                var accessKey = GenerateAccessKey(fiscalReceipt);
                var protocol = GenerateSefazProtocol();
                var qrCode = GenerateQrCode(accessKey);

                fiscalReceipt.Authorize(protocol, accessKey, qrCode);
                await _fiscalReceiptRepository.AlterarAsync(fiscalReceipt);

                _logger.LogInformation("Fiscal receipt {ReceiptNumber} authorized by SEFAZ. Protocol: {Protocol}", 
                    fiscalReceipt.ReceiptNumber, protocol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SEFAZ simulation for receipt {ReceiptNumber}", 
                    fiscalReceipt.ReceiptNumber);
            }
        }

        private string GenerateFiscalReceiptPdfContent(FiscalReceipt fiscalReceipt)
        {
            return $@"
===============================================
           CUPOM FISCAL ELETRÔNICO
===============================================

{_sefazConfig.RazaoSocial}
{_sefazConfig.NomeFantasia}
CNPJ: {_sefazConfig.CNPJ}
IE: {_sefazConfig.InscricaoEstadual}

{_sefazConfig.Endereco.Logradouro}, {_sefazConfig.Endereco.Numero}
{_sefazConfig.Endereco.Bairro} - {_sefazConfig.Endereco.NomeMunicipio}/{_sefazConfig.Endereco.UF}
CEP: {_sefazConfig.Endereco.CEP}

===============================================
NFC-e Nº: {fiscalReceipt.ReceiptNumber}
Série: {fiscalReceipt.SerialNumber}
Data/Hora: {fiscalReceipt.IssuedAt:dd/MM/yyyy HH:mm:ss}
Status: {fiscalReceipt.Status}

{(fiscalReceipt.Status == FiscalReceiptStatus.Authorized ? 
$@"Protocolo SEFAZ: {fiscalReceipt.SefazProtocol}
Chave de Acesso: {fiscalReceipt.AccessKey}

QR Code: {fiscalReceipt.QrCode}" : "")}

===============================================
Ambiente: {_sefazConfig.Environment}
{(fiscalReceipt.Status == FiscalReceiptStatus.Authorized ? "DOCUMENTO FISCAL VÁLIDO" : "DOCUMENTO EM PROCESSAMENTO")}
===============================================
";
        }

        private static string GenerateSerialNumber()
        {
            return $"001{DateTime.UtcNow:yyyyMMdd}{DateTime.UtcNow.Ticks % 1000:D3}";
        }

        private string GenerateAccessKey(FiscalReceipt fiscalReceipt)
        {
            // Generate a 44-digit access key following SEFAZ pattern
            var uf = GetUFCode(_sefazConfig.UF);
            var aamm = DateTime.UtcNow.ToString("yyMM");
            var cnpj = _sefazConfig.CNPJ;
            var mod = "65"; // NFC-e
            var serie = "001";
            var nnf = fiscalReceipt.ReceiptNumber.PadLeft(9, '0');
            var tpEmis = _sefazConfig.Contingencia.Habilitada ? _sefazConfig.Contingencia.TipoEmissao.ToString() : "1";
            var cNF = GenerateRandomCode(8);

            var accessKeyWithoutDV = $"{uf}{aamm}{cnpj}{mod}{serie}{nnf}{tpEmis}{cNF}";
            var dv = CalculateAccessKeyDigit(accessKeyWithoutDV);

            return accessKeyWithoutDV + dv;
        }

        private static string GenerateSefazProtocol()
        {
            return $"135{DateTime.UtcNow:yyyyMMddHHmmss}{DateTime.UtcNow.Ticks % 1000:D3}";
        }

        private static string GenerateQrCode(string accessKey)
        {
            return $"https://www.sefaz.sp.gov.br/nfce/qrcode?p={accessKey}|2|2|1|{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        private static string GenerateContingencyQrCode(string accessKey)
        {
            return $"CONTINGENCIA:{accessKey}|{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        private static string GenerateRandomCode(int length)
        {
            var random = new Random();
            var code = "";
            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            return code;
        }

        private static string GetUFCode(string uf)
        {
            return uf.ToUpper() switch
            {
                "SP" => "35",
                "RJ" => "33",
                "MG" => "31",
                "RS" => "43",
                "PR" => "41",
                "SC" => "42",
                _ => "35" // Default to SP
            };
        }

        private static string GetPaymentTypeCode(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Cash => "01",
                PaymentMethod.CreditCard => "03",
                PaymentMethod.DebitCard => "04",
                PaymentMethod.Pix => "17",
                PaymentMethod.BankTransfer => "05",
                PaymentMethod.Check => "02",
                PaymentMethod.Voucher => "99",
                _ => "99"
            };
        }

        private static string CalculateAccessKeyDigit(string accessKey)
        {
            // Simplified DV calculation - in real implementation use proper algorithm
            var sum = accessKey.Select((c, i) => int.Parse(c.ToString()) * ((i % 8) + 2)).Sum();
            var remainder = sum % 11;
            var dv = remainder < 2 ? 0 : 11 - remainder;
            return dv.ToString();
        }
    }

    public class SefazMessage
    {
        public Guid FiscalReceiptId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public string XmlContent { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; }
    }

    public class SefazCancellationMessage
    {
        public Guid FiscalReceiptId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string CancellationReason { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}