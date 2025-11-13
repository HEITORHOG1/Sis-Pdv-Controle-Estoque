using Interfaces.Services;
using Model.Reports;

namespace Sis_Pdv_Controle_Estoque_API.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportDataService _reportDataService;
        private readonly PdfReportGenerator _pdfGenerator;
        private readonly ExcelReportGenerator _excelGenerator;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportDataService reportDataService,
            ILogger<ReportService> logger)
        {
            _reportDataService = reportDataService;
            _pdfGenerator = new PdfReportGenerator();
            _excelGenerator = new ExcelReportGenerator();
            _logger = logger;
        }

        public async Task<byte[]> GenerateSalesReportPdfAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null)
        {
            try
            {
                _logger.LogInformation("Generating sales report PDF for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetSalesReportDataAsync(startDate, endDate, salesPersonId);
                var pdfBytes = _pdfGenerator.GenerateSalesReport(data);
                
                _logger.LogInformation("Sales report PDF generated successfully with {SalesCount} sales", data.Sales.Count);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateSalesReportExcelAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null)
        {
            try
            {
                _logger.LogInformation("Generating sales report Excel for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetSalesReportDataAsync(startDate, endDate, salesPersonId);
                var excelBytes = _excelGenerator.GenerateSalesReport(data);
                
                _logger.LogInformation("Sales report Excel generated successfully with {SalesCount} sales", data.Sales.Count);
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales report Excel");
                throw;
            }
        }

        public async Task<byte[]> GenerateProductSalesReportPdfAsync(DateTime startDate, DateTime endDate, Guid? productId = null)
        {
            try
            {
                _logger.LogInformation("Generating product sales report PDF for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetSalesReportDataAsync(startDate, endDate);
                
                // Filter by product if specified
                if (productId.HasValue)
                {
                    data.Sales = data.Sales
                        .Where(s => s.Items.Any(i => i.ProductCode != null))
                        .ToList();
                }
                
                var pdfBytes = _pdfGenerator.GenerateSalesReport(data);
                
                _logger.LogInformation("Product sales report PDF generated successfully");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating product sales report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateProductSalesReportExcelAsync(DateTime startDate, DateTime endDate, Guid? productId = null)
        {
            try
            {
                _logger.LogInformation("Generating product sales report Excel for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetSalesReportDataAsync(startDate, endDate);
                
                // Filter by product if specified
                if (productId.HasValue)
                {
                    data.Sales = data.Sales
                        .Where(s => s.Items.Any(i => i.ProductCode != null))
                        .ToList();
                }
                
                var excelBytes = _excelGenerator.GenerateSalesReport(data);
                
                _logger.LogInformation("Product sales report Excel generated successfully");
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating product sales report Excel");
                throw;
            }
        }

        public async Task<byte[]> GenerateInventoryReportPdfAsync()
        {
            try
            {
                _logger.LogInformation("Generating inventory report PDF");
                
                var data = await _reportDataService.GetInventoryReportDataAsync();
                var pdfBytes = _pdfGenerator.GenerateInventoryReport(data);
                
                _logger.LogInformation("Inventory report PDF generated successfully with {ProductCount} products", data.Products.Count);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating inventory report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateInventoryReportExcelAsync()
        {
            try
            {
                _logger.LogInformation("Generating inventory report Excel");
                
                var data = await _reportDataService.GetInventoryReportDataAsync();
                var excelBytes = _excelGenerator.GenerateInventoryReport(data);
                
                _logger.LogInformation("Inventory report Excel generated successfully with {ProductCount} products", data.Products.Count);
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating inventory report Excel");
                throw;
            }
        }

        public async Task<byte[]> GenerateStockMovementReportPdfAsync(DateTime startDate, DateTime endDate, Guid? productId = null)
        {
            try
            {
                _logger.LogInformation("Generating stock movement report PDF for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetStockMovementReportDataAsync(startDate, endDate, productId);
                var pdfBytes = _pdfGenerator.GenerateStockMovementReport(data);
                
                _logger.LogInformation("Stock movement report PDF generated successfully with {MovementCount} movements", data.Movements.Count);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating stock movement report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateStockMovementReportExcelAsync(DateTime startDate, DateTime endDate, Guid? productId = null)
        {
            try
            {
                _logger.LogInformation("Generating stock movement report Excel for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetStockMovementReportDataAsync(startDate, endDate, productId);
                var excelBytes = _excelGenerator.GenerateStockMovementReport(data);
                
                _logger.LogInformation("Stock movement report Excel generated successfully with {MovementCount} movements", data.Movements.Count);
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating stock movement report Excel");
                throw;
            }
        }

        public async Task<byte[]> GenerateFinancialReportPdfAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Generating financial report PDF for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetFinancialReportDataAsync(startDate, endDate);
                var pdfBytes = _pdfGenerator.GenerateFinancialReport(data);
                
                _logger.LogInformation("Financial report PDF generated successfully");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating financial report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateFinancialReportExcelAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Generating financial report Excel for period {StartDate} to {EndDate}", startDate, endDate);
                
                var data = await _reportDataService.GetFinancialReportDataAsync(startDate, endDate);
                var excelBytes = _excelGenerator.GenerateFinancialReport(data);
                
                _logger.LogInformation("Financial report Excel generated successfully");
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating financial report Excel");
                throw;
            }
        }

        public async Task<byte[]> GenerateLowStockReportPdfAsync()
        {
            try
            {
                _logger.LogInformation("Generating low stock report PDF");
                
                var products = await _reportDataService.GetLowStockProductsAsync();
                var pdfBytes = _pdfGenerator.GenerateLowStockReport(products);
                
                _logger.LogInformation("Low stock report PDF generated successfully with {ProductCount} products", products.Count);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating low stock report PDF");
                throw;
            }
        }

        public async Task<byte[]> GenerateLowStockReportExcelAsync()
        {
            try
            {
                _logger.LogInformation("Generating low stock report Excel");
                
                var products = await _reportDataService.GetLowStockProductsAsync();
                var excelBytes = _excelGenerator.GenerateLowStockReport(products);
                
                _logger.LogInformation("Low stock report Excel generated successfully with {ProductCount} products", products.Count);
                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating low stock report Excel");
                throw;
            }
        }
    }
}