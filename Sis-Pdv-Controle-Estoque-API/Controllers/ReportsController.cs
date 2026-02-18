using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Interfaces.Services;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        private IActionResult HandleException(Exception ex)
        {
            _logger.LogError(ex, "Error occurred in reports controller");
            return StatusCode(500, new { message = "An error occurred while generating the report" });
        }

        /// <summary>
        /// Gera relatório de vendas em PDF
        /// </summary>
        [HttpGet("sales/pdf")]
        public async Task<IActionResult> GetSalesReportPdf(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? salesPersonId = null)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateSalesReportPdfAsync(startDate, endDate, salesPersonId);
                var fileName = $"relatorio-vendas-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de vendas em Excel
        /// </summary>
        [HttpGet("sales/excel")]
        public async Task<IActionResult> GetSalesReportExcel(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? salesPersonId = null)
        {
            try
            {
                var excelBytes = await _reportService.GenerateSalesReportExcelAsync(startDate, endDate, salesPersonId);
                var fileName = $"relatorio-vendas-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de vendas por produto em PDF
        /// </summary>
        [HttpGet("sales/products/pdf")]
        public async Task<IActionResult> GetProductSalesReportPdf(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? productId = null)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateProductSalesReportPdfAsync(startDate, endDate, productId);
                var fileName = $"relatorio-vendas-produtos-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de vendas por produto em Excel
        /// </summary>
        [HttpGet("sales/products/excel")]
        public async Task<IActionResult> GetProductSalesReportExcel(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? productId = null)
        {
            try
            {
                var excelBytes = await _reportService.GenerateProductSalesReportExcelAsync(startDate, endDate, productId);
                var fileName = $"relatorio-vendas-produtos-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de estoque em PDF
        /// </summary>
        [HttpGet("inventory/pdf")]
        public async Task<IActionResult> GetInventoryReportPdf()
        {
            try
            {
                var pdfBytes = await _reportService.GenerateInventoryReportPdfAsync();
                var fileName = $"relatorio-estoque-{DateTime.Now:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de estoque em Excel
        /// </summary>
        [HttpGet("inventory/excel")]
        public async Task<IActionResult> GetInventoryReportExcel()
        {
            try
            {
                var excelBytes = await _reportService.GenerateInventoryReportExcelAsync();
                var fileName = $"relatorio-estoque-{DateTime.Now:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de movimentação de estoque em PDF
        /// </summary>
        [HttpGet("stock-movements/pdf")]
        public async Task<IActionResult> GetStockMovementReportPdf(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? productId = null)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateStockMovementReportPdfAsync(startDate, endDate, productId);
                var fileName = $"relatorio-movimentacao-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de movimentação de estoque em Excel
        /// </summary>
        [HttpGet("stock-movements/excel")]
        public async Task<IActionResult> GetStockMovementReportExcel(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? productId = null)
        {
            try
            {
                var excelBytes = await _reportService.GenerateStockMovementReportExcelAsync(startDate, endDate, productId);
                var fileName = $"relatorio-movimentacao-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório financeiro em PDF
        /// </summary>
        [HttpGet("financial/pdf")]
        public async Task<IActionResult> GetFinancialReportPdf(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateFinancialReportPdfAsync(startDate, endDate);
                var fileName = $"relatorio-financeiro-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório financeiro em Excel
        /// </summary>
        [HttpGet("financial/excel")]
        public async Task<IActionResult> GetFinancialReportExcel(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var excelBytes = await _reportService.GenerateFinancialReportExcelAsync(startDate, endDate);
                var fileName = $"relatorio-financeiro-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de produtos com estoque baixo em PDF
        /// </summary>
        [HttpGet("low-stock/pdf")]
        public async Task<IActionResult> GetLowStockReportPdf()
        {
            try
            {
                var pdfBytes = await _reportService.GenerateLowStockReportPdfAsync();
                var fileName = $"relatorio-estoque-baixo-{DateTime.Now:yyyy-MM-dd}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gera relatório de produtos com estoque baixo em Excel
        /// </summary>
        [HttpGet("low-stock/excel")]
        public async Task<IActionResult> GetLowStockReportExcel()
        {
            try
            {
                var excelBytes = await _reportService.GenerateLowStockReportExcelAsync();
                var fileName = $"relatorio-estoque-baixo-{DateTime.Now:yyyy-MM-dd}.xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}