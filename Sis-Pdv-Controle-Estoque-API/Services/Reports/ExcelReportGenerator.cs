using OfficeOpenXml;
using OfficeOpenXml.Style;
using Model.Reports;
using System.Drawing;

namespace Sis_Pdv_Controle_Estoque_API.Services.Reports
{
    public class ExcelReportGenerator
    {
        public ExcelReportGenerator()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] GenerateSalesReport(SalesReportData data)
        {
            using var package = new ExcelPackage();
            
            // Summary worksheet
            var summarySheet = package.Workbook.Worksheets.Add("Resumo");
            CreateSalesSummarySheet(summarySheet, data);

            // Details worksheet
            var detailsSheet = package.Workbook.Worksheets.Add("Detalhes");
            CreateSalesDetailsSheet(detailsSheet, data.Sales);

            return package.GetAsByteArray();
        }

        public byte[] GenerateInventoryReport(InventoryReportData data)
        {
            using var package = new ExcelPackage();
            
            // Summary worksheet
            var summarySheet = package.Workbook.Worksheets.Add("Resumo");
            CreateInventorySummarySheet(summarySheet, data);

            // Details worksheet
            var detailsSheet = package.Workbook.Worksheets.Add("Produtos");
            CreateInventoryDetailsSheet(detailsSheet, data.Products);

            return package.GetAsByteArray();
        }

        public byte[] GenerateStockMovementReport(StockMovementReportData data)
        {
            using var package = new ExcelPackage();
            
            // Summary worksheet
            var summarySheet = package.Workbook.Worksheets.Add("Resumo");
            CreateStockMovementSummarySheet(summarySheet, data);

            // Details worksheet
            var detailsSheet = package.Workbook.Worksheets.Add("Movimentações");
            CreateStockMovementDetailsSheet(detailsSheet, data.Movements);

            return package.GetAsByteArray();
        }

        public byte[] GenerateFinancialReport(FinancialReportData data)
        {
            using var package = new ExcelPackage();
            
            // Summary worksheet
            var summarySheet = package.Workbook.Worksheets.Add("Resumo");
            CreateFinancialSummarySheet(summarySheet, data);

            // Daily data worksheet
            var dailySheet = package.Workbook.Worksheets.Add("Performance Diária");
            CreateDailyPerformanceSheet(dailySheet, data.DailyData);

            // Product data worksheet
            var productSheet = package.Workbook.Worksheets.Add("Performance Produtos");
            CreateProductPerformanceSheet(productSheet, data.ProductData);

            return package.GetAsByteArray();
        }

        public byte[] GenerateLowStockReport(List<InventoryReportItem> products)
        {
            using var package = new ExcelPackage();
            
            var worksheet = package.Workbook.Worksheets.Add("Estoque Baixo");
            CreateLowStockSheet(worksheet, products);

            return package.GetAsByteArray();
        }

        private void CreateSalesSummarySheet(ExcelWorksheet worksheet, SalesReportData data)
        {
            // Title
            worksheet.Cells[1, 1].Value = "Relatório de Vendas";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            // Period
            worksheet.Cells[2, 1].Value = $"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}";
            worksheet.Cells[2, 1].Style.Font.Bold = true;

            // Summary data
            var row = 4;
            worksheet.Cells[row, 1].Value = "Total de Pedidos:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalOrders;
            row++;

            worksheet.Cells[row, 1].Value = "Receita Total:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalRevenue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Valor Médio do Pedido:";
            worksheet.Cells[row, 2].Value = data.Summary.AverageOrderValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Total de Itens Vendidos:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalItemsSold;
            row += 2;

            // Payment methods
            if (data.Summary.RevenueByPaymentMethod.Any())
            {
                worksheet.Cells[row, 1].Value = "Receita por Forma de Pagamento:";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;

                foreach (var payment in data.Summary.RevenueByPaymentMethod)
                {
                    worksheet.Cells[row, 1].Value = payment.Key;
                    worksheet.Cells[row, 2].Value = payment.Value;
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
                    row++;
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateSalesDetailsSheet(ExcelWorksheet worksheet, List<SalesReportItem> sales)
        {
            // Headers
            var headers = new[] { "Data", "Cliente", "Vendedor", "Forma Pagamento", "Total Itens", "Valor Total" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 2;
            foreach (var sale in sales)
            {
                worksheet.Cells[row, 1].Value = sale.OrderDate;
                worksheet.Cells[row, 1].Style.Numberformat.Format = "dd/mm/yyyy";
                worksheet.Cells[row, 2].Value = sale.CustomerName;
                worksheet.Cells[row, 3].Value = sale.SalesPersonName;
                worksheet.Cells[row, 4].Value = sale.PaymentMethod;
                worksheet.Cells[row, 5].Value = sale.TotalItems;
                worksheet.Cells[row, 6].Value = sale.TotalAmount;
                worksheet.Cells[row, 6].Style.Numberformat.Format = "R$ #,##0.00";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateInventorySummarySheet(ExcelWorksheet worksheet, InventoryReportData data)
        {
            // Title
            worksheet.Cells[1, 1].Value = "Relatório de Estoque";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            // Generated date
            worksheet.Cells[2, 1].Value = $"Gerado em: {data.GeneratedAt:dd/MM/yyyy HH:mm}";
            worksheet.Cells[2, 1].Style.Font.Bold = true;

            // Summary data
            var row = 4;
            worksheet.Cells[row, 1].Value = "Total de Produtos:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalProducts;
            row++;

            worksheet.Cells[row, 1].Value = "Produtos Ativos:";
            worksheet.Cells[row, 2].Value = data.Summary.ActiveProducts;
            row++;

            worksheet.Cells[row, 1].Value = "Produtos Inativos:";
            worksheet.Cells[row, 2].Value = data.Summary.InactiveProducts;
            row++;

            worksheet.Cells[row, 1].Value = "Produtos com Estoque Baixo:";
            worksheet.Cells[row, 2].Value = data.Summary.LowStockProducts;
            row++;

            worksheet.Cells[row, 1].Value = "Produtos sem Estoque:";
            worksheet.Cells[row, 2].Value = data.Summary.OutOfStockProducts;
            row++;

            worksheet.Cells[row, 1].Value = "Valor Total do Estoque:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalStockValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row += 2;

            // Categories
            if (data.Summary.ProductsByCategory.Any())
            {
                worksheet.Cells[row, 1].Value = "Produtos por Categoria:";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;

                foreach (var category in data.Summary.ProductsByCategory)
                {
                    worksheet.Cells[row, 1].Value = category.Key;
                    worksheet.Cells[row, 2].Value = category.Value;
                    row++;
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateInventoryDetailsSheet(ExcelWorksheet worksheet, List<InventoryReportItem> products)
        {
            // Headers
            var headers = new[] { "Produto", "Código", "Categoria", "Fornecedor", "Estoque Atual", "Estoque Mínimo", "Ponto Reposição", "Status", "Valor Unitário", "Valor Total", "Localização" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 2;
            foreach (var product in products)
            {
                worksheet.Cells[row, 1].Value = product.ProductName;
                worksheet.Cells[row, 2].Value = product.ProductCode;
                worksheet.Cells[row, 3].Value = product.Category;
                worksheet.Cells[row, 4].Value = product.Supplier;
                worksheet.Cells[row, 5].Value = product.CurrentStock;
                worksheet.Cells[row, 6].Value = product.MinimumStock;
                worksheet.Cells[row, 7].Value = product.ReorderPoint;
                worksheet.Cells[row, 8].Value = product.StockStatus;
                worksheet.Cells[row, 9].Value = product.UnitCost;
                worksheet.Cells[row, 9].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 10].Value = product.StockValue;
                worksheet.Cells[row, 10].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 11].Value = product.Location ?? "";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateStockMovementSummarySheet(ExcelWorksheet worksheet, StockMovementReportData data)
        {
            // Title
            worksheet.Cells[1, 1].Value = "Relatório de Movimentação de Estoque";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            // Period
            worksheet.Cells[2, 1].Value = $"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}";
            worksheet.Cells[2, 1].Style.Font.Bold = true;

            // Summary data
            var row = 4;
            worksheet.Cells[row, 1].Value = "Total de Movimentações:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalMovements;
            row++;

            worksheet.Cells[row, 1].Value = "Valor Total de Entradas:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalInboundValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Valor Total de Saídas:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalOutboundValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Valor Líquido:";
            worksheet.Cells[row, 2].Value = data.Summary.NetMovementValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row += 2;

            // Movement types
            if (data.Summary.MovementsByType.Any())
            {
                worksheet.Cells[row, 1].Value = "Movimentações por Tipo:";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;

                foreach (var movement in data.Summary.MovementsByType)
                {
                    worksheet.Cells[row, 1].Value = movement.Key;
                    worksheet.Cells[row, 2].Value = movement.Value;
                    row++;
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateStockMovementDetailsSheet(ExcelWorksheet worksheet, List<StockMovementReportItem> movements)
        {
            // Headers
            var headers = new[] { "Data", "Produto", "Código", "Tipo", "Quantidade", "Estoque Anterior", "Novo Estoque", "Motivo", "Valor Unitário", "Valor Total", "Documento" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 2;
            foreach (var movement in movements)
            {
                worksheet.Cells[row, 1].Value = movement.MovementDate;
                worksheet.Cells[row, 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                worksheet.Cells[row, 2].Value = movement.ProductName;
                worksheet.Cells[row, 3].Value = movement.ProductCode;
                worksheet.Cells[row, 4].Value = movement.MovementType;
                worksheet.Cells[row, 5].Value = movement.Quantity;
                worksheet.Cells[row, 6].Value = movement.PreviousStock;
                worksheet.Cells[row, 7].Value = movement.NewStock;
                worksheet.Cells[row, 8].Value = movement.Reason;
                worksheet.Cells[row, 9].Value = movement.UnitCost;
                worksheet.Cells[row, 9].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 10].Value = movement.TotalValue;
                worksheet.Cells[row, 10].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 11].Value = movement.ReferenceDocument ?? "";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateFinancialSummarySheet(ExcelWorksheet worksheet, FinancialReportData data)
        {
            // Title
            worksheet.Cells[1, 1].Value = "Relatório Financeiro";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            // Period
            worksheet.Cells[2, 1].Value = $"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}";
            worksheet.Cells[2, 1].Style.Font.Bold = true;

            // Summary data
            var row = 4;
            worksheet.Cells[row, 1].Value = "Receita Total:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalRevenue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Custo Total:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalCost;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Lucro Bruto:";
            worksheet.Cells[row, 2].Value = data.Summary.GrossProfit;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row++;

            worksheet.Cells[row, 1].Value = "Margem de Lucro:";
            worksheet.Cells[row, 2].Value = data.Summary.GrossProfitMargin / 100;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "0.00%";
            row++;

            worksheet.Cells[row, 1].Value = "Total de Pedidos:";
            worksheet.Cells[row, 2].Value = data.Summary.TotalOrders;
            row++;

            worksheet.Cells[row, 1].Value = "Valor Médio do Pedido:";
            worksheet.Cells[row, 2].Value = data.Summary.AverageOrderValue;
            worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
            row += 2;

            // Payment methods
            if (data.Summary.RevenueByPaymentMethod.Any())
            {
                worksheet.Cells[row, 1].Value = "Receita por Forma de Pagamento:";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;

                foreach (var payment in data.Summary.RevenueByPaymentMethod)
                {
                    worksheet.Cells[row, 1].Value = payment.Key;
                    worksheet.Cells[row, 2].Value = payment.Value;
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
                    row++;
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateDailyPerformanceSheet(ExcelWorksheet worksheet, List<FinancialReportDailyData> dailyData)
        {
            // Headers
            var headers = new[] { "Data", "Receita", "Custo", "Lucro", "Margem", "Pedidos", "Itens Vendidos" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 2;
            foreach (var day in dailyData)
            {
                worksheet.Cells[row, 1].Value = day.Date;
                worksheet.Cells[row, 1].Style.Numberformat.Format = "dd/mm/yyyy";
                worksheet.Cells[row, 2].Value = day.Revenue;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 3].Value = day.Cost;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 4].Value = day.Profit;
                worksheet.Cells[row, 4].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 5].Value = day.Revenue > 0 ? day.Profit / day.Revenue : 0;
                worksheet.Cells[row, 5].Style.Numberformat.Format = "0.00%";
                worksheet.Cells[row, 6].Value = day.OrderCount;
                worksheet.Cells[row, 7].Value = day.ItemsSold;
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateProductPerformanceSheet(ExcelWorksheet worksheet, List<FinancialReportProductData> productData)
        {
            // Headers
            var headers = new[] { "Produto", "Código", "Categoria", "Qtd Vendida", "Receita", "Custo", "Lucro", "Margem" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 2;
            foreach (var product in productData)
            {
                worksheet.Cells[row, 1].Value = product.ProductName;
                worksheet.Cells[row, 2].Value = product.ProductCode;
                worksheet.Cells[row, 3].Value = product.Category;
                worksheet.Cells[row, 4].Value = product.QuantitySold;
                worksheet.Cells[row, 5].Value = product.Revenue;
                worksheet.Cells[row, 5].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 6].Value = product.Cost;
                worksheet.Cells[row, 6].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 7].Value = product.Profit;
                worksheet.Cells[row, 7].Style.Numberformat.Format = "R$ #,##0.00";
                worksheet.Cells[row, 8].Value = product.ProfitMargin / 100;
                worksheet.Cells[row, 8].Style.Numberformat.Format = "0.00%";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }

        private void CreateLowStockSheet(ExcelWorksheet worksheet, List<InventoryReportItem> products)
        {
            // Title
            worksheet.Cells[1, 1].Value = "Produtos com Estoque Baixo";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            // Generated date
            worksheet.Cells[2, 1].Value = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Cells[2, 1].Style.Font.Bold = true;

            // Headers
            var headers = new[] { "Produto", "Código", "Categoria", "Estoque Atual", "Estoque Mínimo", "Ponto Reposição", "Status", "Localização" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[4, i + 1].Value = headers[i];
                worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                worksheet.Cells[4, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Data
            var row = 5;
            foreach (var product in products)
            {
                worksheet.Cells[row, 1].Value = product.ProductName;
                worksheet.Cells[row, 2].Value = product.ProductCode;
                worksheet.Cells[row, 3].Value = product.Category;
                worksheet.Cells[row, 4].Value = product.CurrentStock;
                worksheet.Cells[row, 5].Value = product.MinimumStock;
                worksheet.Cells[row, 6].Value = product.ReorderPoint;
                worksheet.Cells[row, 7].Value = product.StockStatus;
                worksheet.Cells[row, 8].Value = product.Location ?? "";

                // Color coding for status
                if (product.StockStatus == "Out of Stock")
                {
                    worksheet.Cells[row, 1, row, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 8].Style.Fill.BackgroundColor.SetColor(Color.LightCoral);
                }
                else if (product.StockStatus == "Low Stock")
                {
                    worksheet.Cells[row, 1, row, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 8].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                }

                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
        }
    }
}