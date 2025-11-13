using Model.Reports;

namespace Interfaces.Services
{
    public interface IReportDataService
    {
        Task<SalesReportData> GetSalesReportDataAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null);
        Task<InventoryReportData> GetInventoryReportDataAsync();
        Task<StockMovementReportData> GetStockMovementReportDataAsync(DateTime startDate, DateTime endDate, Guid? productId = null);
        Task<FinancialReportData> GetFinancialReportDataAsync(DateTime startDate, DateTime endDate);
        Task<List<InventoryReportItem>> GetLowStockProductsAsync();
    }
}