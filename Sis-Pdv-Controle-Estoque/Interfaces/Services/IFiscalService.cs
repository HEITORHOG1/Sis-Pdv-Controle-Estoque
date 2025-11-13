using Model;

namespace Interfaces.Services
{
    public interface IFiscalService
    {
        Task<FiscalReceiptResult> GenerateFiscalReceiptAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<FiscalReceiptResult> CancelFiscalReceiptAsync(Guid fiscalReceiptId, string reason, CancellationToken cancellationToken = default);
        Task<FiscalReceiptResult> GetFiscalReceiptStatusAsync(Guid fiscalReceiptId, CancellationToken cancellationToken = default);
        Task<byte[]> GenerateFiscalReceiptPdfAsync(Guid fiscalReceiptId, CancellationToken cancellationToken = default);
        Task<string> GenerateFiscalReceiptXmlAsync(Guid paymentId, CancellationToken cancellationToken = default);
    }

    public class FiscalReceiptResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public FiscalReceipt? FiscalReceipt { get; set; }
        public string? SefazProtocol { get; set; }
        public string? AccessKey { get; set; }
        public string? QrCode { get; set; }

        public static FiscalReceiptResult SuccessResult(FiscalReceipt fiscalReceipt)
        {
            return new FiscalReceiptResult
            {
                Success = true,
                FiscalReceipt = fiscalReceipt
            };
        }

        public static FiscalReceiptResult FailureResult(string errorMessage)
        {
            return new FiscalReceiptResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}