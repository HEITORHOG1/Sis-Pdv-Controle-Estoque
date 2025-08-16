using Model;

namespace Interfaces.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateSalesReportPdfAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null);
        Task<byte[]> GenerateSalesReportExcelAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null);
        
        Task<byte[]> GenerateProductSalesReportPdfAsync(DateTime startDate, DateTime endDate, Guid? productId = null);
        Task<byte[]> GenerateProductSalesReportExcelAsync(DateTime startDate, DateTime endDate, Guid? productId = null);
        
        Task<byte[]> GenerateInventoryReportPdfAsync();
        Task<byte[]> GenerateInventoryReportExcelAsync();
        
        Task<byte[]> GenerateStockMovementReportPdfAsync(DateTime startDate, DateTime endDate, Guid? productId = null);
        Task<byte[]> GenerateStockMovementReportExcelAsync(DateTime startDate, DateTime endDate, Guid? productId = null);
        
        Task<byte[]> GenerateFinancialReportPdfAsync(DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateFinancialReportExcelAsync(DateTime startDate, DateTime endDate);
        
        Task<byte[]> GenerateLowStockReportPdfAsync();
        Task<byte[]> GenerateLowStockReportExcelAsync();
    }
}