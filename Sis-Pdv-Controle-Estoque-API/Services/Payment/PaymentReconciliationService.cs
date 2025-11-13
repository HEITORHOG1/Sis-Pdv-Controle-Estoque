using Interfaces.Repositories;
using Interfaces.Services;
using Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_API.Services.Payment
{
    public interface IPaymentReconciliationService
    {
        Task<ReconciliationResult> ReconcilePaymentsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<ReconciliationResult> ReconcilePaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentDiscrepancy>> GetPaymentDiscrepanciesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<bool> ResolveDiscrepancyAsync(Guid discrepancyId, string resolution, Guid userId, CancellationToken cancellationToken = default);
    }

    public class PaymentReconciliationService : IPaymentReconciliationService
    {
        private readonly IRepositoryPayment _paymentRepository;
        private readonly IRepositoryPaymentAudit _auditRepository;
        private readonly IPaymentProcessorService _processorService;
        private readonly ILogger<PaymentReconciliationService> _logger;

        public PaymentReconciliationService(
            IRepositoryPayment paymentRepository,
            IRepositoryPaymentAudit auditRepository,
            IPaymentProcessorService processorService,
            ILogger<PaymentReconciliationService> logger)
        {
            _paymentRepository = paymentRepository;
            _auditRepository = auditRepository;
            _processorService = processorService;
            _logger = logger;
        }

        public async Task<ReconciliationResult> ReconcilePaymentsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting payment reconciliation for period {StartDate} to {EndDate}", startDate, endDate);

                var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate, cancellationToken);
                var result = new ReconciliationResult
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalPayments = payments.Count(),
                    ProcessedAt = DateTime.UtcNow
                };

                var discrepancies = new List<PaymentDiscrepancy>();
                var reconciledCount = 0;
                var failedCount = 0;

                foreach (var payment in payments)
                {
                    try
                    {
                        var paymentResult = await ReconcilePaymentAsync(payment.Id, cancellationToken);
                        
                        if (paymentResult.Success)
                        {
                            reconciledCount++;
                            result.ReconciledPayments.Add(payment.Id);
                        }
                        else
                        {
                            failedCount++;
                            result.FailedReconciliations.Add(payment.Id);
                        }

                        discrepancies.AddRange(paymentResult.Discrepancies);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error reconciling payment {PaymentId}", payment.Id);
                        failedCount++;
                        result.FailedReconciliations.Add(payment.Id);
                    }
                }

                result.ReconciledCount = reconciledCount;
                result.FailedCount = failedCount;
                result.Discrepancies = discrepancies;
                result.Success = failedCount == 0;

                _logger.LogInformation("Payment reconciliation completed. Reconciled: {ReconciledCount}, Failed: {FailedCount}, Discrepancies: {DiscrepancyCount}", 
                    reconciledCount, failedCount, discrepancies.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment reconciliation");
                return ReconciliationResult.FailureResult($"Reconciliation failed: {ex.Message}");
            }
        }

        public async Task<ReconciliationResult> ReconcilePaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var payment = await _paymentRepository.GetWithItemsAsync(paymentId, cancellationToken);
                if (payment == null)
                {
                    return ReconciliationResult.FailureResult("Payment not found");
                }

                var result = new ReconciliationResult
                {
                    TotalPayments = 1,
                    ProcessedAt = DateTime.UtcNow
                };

                var discrepancies = new List<PaymentDiscrepancy>();

                // Check payment status consistency
                await ValidatePaymentStatusAsync(payment, discrepancies);

                // Check payment items consistency
                await ValidatePaymentItemsAsync(payment, discrepancies);

                // Check processor transaction status
                await ValidateProcessorTransactionsAsync(payment, discrepancies, cancellationToken);

                // Check fiscal receipt consistency
                await ValidateFiscalReceiptAsync(payment, discrepancies);

                result.Discrepancies = discrepancies;
                result.Success = discrepancies.Count == 0;

                if (result.Success)
                {
                    result.ReconciledPayments.Add(paymentId);
                    result.ReconciledCount = 1;
                }
                else
                {
                    result.FailedReconciliations.Add(paymentId);
                    result.FailedCount = 1;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reconciling payment {PaymentId}", paymentId);
                return ReconciliationResult.FailureResult($"Payment reconciliation failed: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PaymentDiscrepancy>> GetPaymentDiscrepanciesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var reconciliationResult = await ReconcilePaymentsAsync(startDate, endDate, cancellationToken);
            return reconciliationResult.Discrepancies;
        }

        public async Task<bool> ResolveDiscrepancyAsync(Guid discrepancyId, string resolution, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // In a real implementation, you would store discrepancies in the database
                // For now, we'll just log the resolution
                _logger.LogInformation("Resolving discrepancy {DiscrepancyId} by user {UserId}. Resolution: {Resolution}", 
                    discrepancyId, userId, resolution);

                // Create audit log for discrepancy resolution
                var auditLog = new PaymentAudit(
                    Guid.Empty, // Would be the actual payment ID
                    PaymentAuditAction.Created, // Would be a new enum value for discrepancy resolution
                    $"Discrepancy {discrepancyId} resolved: {resolution}",
                    userId);

                await _auditRepository.AdicionarAsync(auditLog);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving discrepancy {DiscrepancyId}", discrepancyId);
                return false;
            }
        }

        private async Task ValidatePaymentStatusAsync(Model.Payment payment, List<PaymentDiscrepancy> discrepancies)
        {
            // Check if payment status is consistent with payment items
            var approvedItems = payment.PaymentItems.Count(i => i.Status == PaymentItemStatus.Approved);
            var rejectedItems = payment.PaymentItems.Count(i => i.Status == PaymentItemStatus.Rejected);

            if (payment.Status == PaymentStatus.Processed && approvedItems == 0)
            {
                discrepancies.Add(new PaymentDiscrepancy
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Type = DiscrepancyType.StatusInconsistency,
                    Description = "Payment marked as processed but no approved payment items found",
                    Severity = DiscrepancySeverity.High,
                    DetectedAt = DateTime.UtcNow
                });
            }

            if (payment.Status == PaymentStatus.Failed && rejectedItems == 0)
            {
                discrepancies.Add(new PaymentDiscrepancy
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Type = DiscrepancyType.StatusInconsistency,
                    Description = "Payment marked as failed but no rejected payment items found",
                    Severity = DiscrepancySeverity.Medium,
                    DetectedAt = DateTime.UtcNow
                });
            }
        }

        private async Task ValidatePaymentItemsAsync(Model.Payment payment, List<PaymentDiscrepancy> discrepancies)
        {
            var totalItemAmount = payment.PaymentItems.Sum(i => i.Amount);
            
            if (Math.Abs(totalItemAmount - payment.TotalAmount) > 0.01m)
            {
                discrepancies.Add(new PaymentDiscrepancy
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Type = DiscrepancyType.AmountMismatch,
                    Description = $"Payment total ({payment.TotalAmount:C}) does not match sum of payment items ({totalItemAmount:C})",
                    Severity = DiscrepancySeverity.High,
                    DetectedAt = DateTime.UtcNow
                });
            }

            // Check for duplicate transaction IDs
            var transactionIds = payment.PaymentItems
                .Where(i => !string.IsNullOrEmpty(i.ProcessorTransactionId))
                .GroupBy(i => i.ProcessorTransactionId)
                .Where(g => g.Count() > 1);

            foreach (var group in transactionIds)
            {
                discrepancies.Add(new PaymentDiscrepancy
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Type = DiscrepancyType.DuplicateTransaction,
                    Description = $"Duplicate transaction ID found: {group.Key}",
                    Severity = DiscrepancySeverity.High,
                    DetectedAt = DateTime.UtcNow
                });
            }
        }

        private async Task ValidateProcessorTransactionsAsync(Model.Payment payment, List<PaymentDiscrepancy> discrepancies, CancellationToken cancellationToken)
        {
            foreach (var item in payment.PaymentItems.Where(i => !string.IsNullOrEmpty(i.ProcessorTransactionId)))
            {
                try
                {
                    var processorResult = await _processorService.GetTransactionStatusAsync(item.ProcessorTransactionId!, cancellationToken);
                    
                    if (!processorResult.Success)
                    {
                        discrepancies.Add(new PaymentDiscrepancy
                        {
                            Id = Guid.NewGuid(),
                            PaymentId = payment.Id,
                            Type = DiscrepancyType.ProcessorMismatch,
                            Description = $"Transaction {item.ProcessorTransactionId} not found in processor",
                            Severity = DiscrepancySeverity.High,
                            DetectedAt = DateTime.UtcNow
                        });
                        continue;
                    }

                    // Check status consistency
                    var expectedStatus = item.Status switch
                    {
                        PaymentItemStatus.Approved => PaymentProcessorStatus.Approved,
                        PaymentItemStatus.Rejected => PaymentProcessorStatus.Rejected,
                        PaymentItemStatus.Cancelled => PaymentProcessorStatus.Cancelled,
                        _ => PaymentProcessorStatus.Pending
                    };

                    if (processorResult.Status != expectedStatus)
                    {
                        discrepancies.Add(new PaymentDiscrepancy
                        {
                            Id = Guid.NewGuid(),
                            PaymentId = payment.Id,
                            Type = DiscrepancyType.ProcessorMismatch,
                            Description = $"Status mismatch for transaction {item.ProcessorTransactionId}. Local: {item.Status}, Processor: {processorResult.Status}",
                            Severity = DiscrepancySeverity.Medium,
                            DetectedAt = DateTime.UtcNow
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not validate processor transaction {TransactionId}", item.ProcessorTransactionId);
                }
            }
        }

        private async Task ValidateFiscalReceiptAsync(Model.Payment payment, List<PaymentDiscrepancy> discrepancies)
        {
            // Check if fiscal receipt exists for processed payments
            if (payment.Status == PaymentStatus.Processed && payment.FiscalReceipt == null)
            {
                discrepancies.Add(new PaymentDiscrepancy
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    Type = DiscrepancyType.MissingFiscalReceipt,
                    Description = "Processed payment missing fiscal receipt",
                    Severity = DiscrepancySeverity.Medium,
                    DetectedAt = DateTime.UtcNow
                });
            }

            // Check fiscal receipt status consistency
            if (payment.FiscalReceipt != null)
            {
                if (payment.Status == PaymentStatus.Processed && 
                    payment.FiscalReceipt.Status != FiscalReceiptStatus.Authorized &&
                    payment.FiscalReceipt.Status != FiscalReceiptStatus.Pending &&
                    payment.FiscalReceipt.Status != FiscalReceiptStatus.Sent)
                {
                    discrepancies.Add(new PaymentDiscrepancy
                    {
                        Id = Guid.NewGuid(),
                        PaymentId = payment.Id,
                        Type = DiscrepancyType.FiscalReceiptInconsistency,
                        Description = $"Processed payment has fiscal receipt with invalid status: {payment.FiscalReceipt.Status}",
                        Severity = DiscrepancySeverity.Medium,
                        DetectedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }

    public class ReconciliationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalPayments { get; set; }
        public int ReconciledCount { get; set; }
        public int FailedCount { get; set; }
        public List<Guid> ReconciledPayments { get; set; } = new();
        public List<Guid> FailedReconciliations { get; set; } = new();
        public IEnumerable<PaymentDiscrepancy> Discrepancies { get; set; } = new List<PaymentDiscrepancy>();
        public DateTime ProcessedAt { get; set; }

        public static ReconciliationResult FailureResult(string errorMessage)
        {
            return new ReconciliationResult
            {
                Success = false,
                ErrorMessage = errorMessage,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }

    public class PaymentDiscrepancy
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public DiscrepancyType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DiscrepancySeverity Severity { get; set; }
        public DateTime DetectedAt { get; set; }
        public bool IsResolved { get; set; }
        public string? Resolution { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? ResolvedBy { get; set; }
    }

    public enum DiscrepancyType
    {
        StatusInconsistency = 1,
        AmountMismatch = 2,
        ProcessorMismatch = 3,
        DuplicateTransaction = 4,
        MissingFiscalReceipt = 5,
        FiscalReceiptInconsistency = 6
    }

    public enum DiscrepancySeverity
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}