using iTextSharp.text;
using iTextSharp.text.pdf;
using Model.Reports;
using System.Globalization;

namespace Sis_Pdv_Controle_Estoque_API.Services.Reports
{
    public class PdfReportGenerator
    {
        private readonly Font _titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.Black);
        private readonly Font _headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.Black);
        private readonly Font _normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.Black);
        private readonly Font _smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.Black);

        public byte[] GenerateSalesReport(SalesReportData data)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var title = new Paragraph("Relatório de Vendas", _titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Period
            var period = new Paragraph($"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}", _headerFont)
            {
                SpacingAfter = 15
            };
            document.Add(period);

            // Summary
            AddSalesSummary(document, data.Summary);

            // Sales Details
            if (data.Sales.Any())
            {
                document.Add(new Paragraph("Detalhes das Vendas", _headerFont) { SpacingBefore = 20, SpacingAfter = 10 });
                AddSalesTable(document, data.Sales);
            }

            document.Close();
            return memoryStream.ToArray();
        }

        public byte[] GenerateInventoryReport(InventoryReportData data)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var title = new Paragraph("Relatório de Estoque", _titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Generated date
            var generatedDate = new Paragraph($"Gerado em: {data.GeneratedAt:dd/MM/yyyy HH:mm}", _headerFont)
            {
                SpacingAfter = 15
            };
            document.Add(generatedDate);

            // Summary
            AddInventorySummary(document, data.Summary);

            // Inventory Details
            if (data.Products.Any())
            {
                document.Add(new Paragraph("Detalhes do Estoque", _headerFont) { SpacingBefore = 20, SpacingAfter = 10 });
                AddInventoryTable(document, data.Products);
            }

            document.Close();
            return memoryStream.ToArray();
        }

        public byte[] GenerateStockMovementReport(StockMovementReportData data)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var title = new Paragraph("Relatório de Movimentação de Estoque", _titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Period
            var period = new Paragraph($"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}", _headerFont)
            {
                SpacingAfter = 15
            };
            document.Add(period);

            // Summary
            AddStockMovementSummary(document, data.Summary);

            // Movement Details
            if (data.Movements.Any())
            {
                document.Add(new Paragraph("Detalhes das Movimentações", _headerFont) { SpacingBefore = 20, SpacingAfter = 10 });
                AddStockMovementTable(document, data.Movements);
            }

            document.Close();
            return memoryStream.ToArray();
        }

        public byte[] GenerateFinancialReport(FinancialReportData data)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var title = new Paragraph("Relatório Financeiro", _titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Period
            var period = new Paragraph($"Período: {data.StartDate:dd/MM/yyyy} a {data.EndDate:dd/MM/yyyy}", _headerFont)
            {
                SpacingAfter = 15
            };
            document.Add(period);

            // Summary
            AddFinancialSummary(document, data.Summary);

            // Daily Performance
            if (data.DailyData.Any())
            {
                document.Add(new Paragraph("Performance Diária", _headerFont) { SpacingBefore = 20, SpacingAfter = 10 });
                AddDailyPerformanceTable(document, data.DailyData);
            }

            // Product Performance
            if (data.ProductData.Any())
            {
                document.Add(new Paragraph("Performance por Produto", _headerFont) { SpacingBefore = 20, SpacingAfter = 10 });
                AddProductPerformanceTable(document, data.ProductData.Take(20).ToList());
            }

            document.Close();
            return memoryStream.ToArray();
        }

        public byte[] GenerateLowStockReport(List<InventoryReportItem> products)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var title = new Paragraph("Relatório de Produtos com Estoque Baixo", _titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Generated date
            var generatedDate = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}", _headerFont)
            {
                SpacingAfter = 15
            };
            document.Add(generatedDate);

            if (products.Any())
            {
                AddLowStockTable(document, products);
            }
            else
            {
                document.Add(new Paragraph("Nenhum produto com estoque baixo encontrado.", _normalFont));
            }

            document.Close();
            return memoryStream.ToArray();
        }

        private void AddSalesSummary(Document document, SalesReportSummary summary)
        {
            var table = new PdfPTable(2) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1 });

            // Summary data
            AddSummaryRow(table, "Total de Pedidos:", summary.TotalOrders.ToString());
            AddSummaryRow(table, "Receita Total:", summary.TotalRevenue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Valor Médio do Pedido:", summary.AverageOrderValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Total de Itens Vendidos:", summary.TotalItemsSold.ToString());

            document.Add(table);
        }

        private void AddInventorySummary(Document document, InventoryReportSummary summary)
        {
            var table = new PdfPTable(2) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1 });

            AddSummaryRow(table, "Total de Produtos:", summary.TotalProducts.ToString());
            AddSummaryRow(table, "Produtos Ativos:", summary.ActiveProducts.ToString());
            AddSummaryRow(table, "Produtos Inativos:", summary.InactiveProducts.ToString());
            AddSummaryRow(table, "Produtos com Estoque Baixo:", summary.LowStockProducts.ToString());
            AddSummaryRow(table, "Produtos sem Estoque:", summary.OutOfStockProducts.ToString());
            AddSummaryRow(table, "Valor Total do Estoque:", summary.TotalStockValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));

            document.Add(table);
        }

        private void AddStockMovementSummary(Document document, StockMovementReportSummary summary)
        {
            var table = new PdfPTable(2) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1 });

            AddSummaryRow(table, "Total de Movimentações:", summary.TotalMovements.ToString());
            AddSummaryRow(table, "Valor Total de Entradas:", summary.TotalInboundValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Valor Total de Saídas:", summary.TotalOutboundValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Valor Líquido:", summary.NetMovementValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));

            document.Add(table);
        }

        private void AddFinancialSummary(Document document, FinancialReportSummary summary)
        {
            var table = new PdfPTable(2) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1 });

            AddSummaryRow(table, "Receita Total:", summary.TotalRevenue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Custo Total:", summary.TotalCost.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Lucro Bruto:", summary.GrossProfit.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));
            AddSummaryRow(table, "Margem de Lucro:", $"{summary.GrossProfitMargin:F2}%");
            AddSummaryRow(table, "Total de Pedidos:", summary.TotalOrders.ToString());
            AddSummaryRow(table, "Valor Médio do Pedido:", summary.AverageOrderValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")));

            document.Add(table);
        }

        private void AddSummaryRow(PdfPTable table, string label, string value)
        {
            table.AddCell(new PdfPCell(new Phrase(label, _normalFont)) { Border = Rectangle.NO_BORDER, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(value, _normalFont)) { Border = Rectangle.NO_BORDER, Padding = 5 });
        }

        private void AddSalesTable(Document document, List<SalesReportItem> sales)
        {
            var table = new PdfPTable(6) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1.5f, 1.5f, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Data");
            AddTableHeader(table, "Cliente");
            AddTableHeader(table, "Vendedor");
            AddTableHeader(table, "Pagamento");
            AddTableHeader(table, "Itens");
            AddTableHeader(table, "Total");

            foreach (var sale in sales.Take(50)) // Limit to avoid too large PDFs
            {
                table.AddCell(new PdfPCell(new Phrase(sale.OrderDate.ToString("dd/MM/yyyy"), _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(sale.CustomerName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(sale.SalesPersonName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(sale.PaymentMethod, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(sale.TotalItems.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(sale.TotalAmount.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
        }

        private void AddInventoryTable(Document document, List<InventoryReportItem> products)
        {
            var table = new PdfPTable(7) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2, 1, 1, 1, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Produto");
            AddTableHeader(table, "Código");
            AddTableHeader(table, "Categoria");
            AddTableHeader(table, "Estoque");
            AddTableHeader(table, "Mín.");
            AddTableHeader(table, "Status");
            AddTableHeader(table, "Valor");

            foreach (var product in products.Take(100)) // Limit to avoid too large PDFs
            {
                table.AddCell(new PdfPCell(new Phrase(product.ProductName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.ProductCode, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.Category, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.CurrentStock.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.MinimumStock.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.StockStatus, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.StockValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
        }

        private void AddStockMovementTable(Document document, List<StockMovementReportItem> movements)
        {
            var table = new PdfPTable(6) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 2, 1, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Data");
            AddTableHeader(table, "Produto");
            AddTableHeader(table, "Tipo");
            AddTableHeader(table, "Qtd");
            AddTableHeader(table, "Motivo");
            AddTableHeader(table, "Valor");

            foreach (var movement in movements.Take(100)) // Limit to avoid too large PDFs
            {
                table.AddCell(new PdfPCell(new Phrase(movement.MovementDate.ToString("dd/MM/yyyy"), _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(movement.ProductName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(movement.MovementType, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(movement.Quantity.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(movement.Reason, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(movement.TotalValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
        }

        private void AddDailyPerformanceTable(Document document, List<FinancialReportDailyData> dailyData)
        {
            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 1, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Data");
            AddTableHeader(table, "Receita");
            AddTableHeader(table, "Custo");
            AddTableHeader(table, "Lucro");
            AddTableHeader(table, "Pedidos");

            foreach (var day in dailyData.Take(31)) // Limit to avoid too large PDFs
            {
                table.AddCell(new PdfPCell(new Phrase(day.Date.ToString("dd/MM/yyyy"), _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(day.Revenue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(day.Cost.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(day.Profit.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(day.OrderCount.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
        }

        private void AddProductPerformanceTable(Document document, List<FinancialReportProductData> productData)
        {
            var table = new PdfPTable(6) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2, 1, 1, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Produto");
            AddTableHeader(table, "Qtd");
            AddTableHeader(table, "Receita");
            AddTableHeader(table, "Custo");
            AddTableHeader(table, "Lucro");
            AddTableHeader(table, "Margem");

            foreach (var product in productData)
            {
                table.AddCell(new PdfPCell(new Phrase(product.ProductName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.QuantitySold.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.Revenue.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.Cost.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.Profit.ToString("C", CultureInfo.GetCultureInfo("pt-BR")), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase($"{product.ProfitMargin:F1}%", _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
        }

        private void AddLowStockTable(Document document, List<InventoryReportItem> products)
        {
            var table = new PdfPTable(7) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2, 1, 1, 1, 1, 1, 1 });

            // Headers
            AddTableHeader(table, "Produto");
            AddTableHeader(table, "Código");
            AddTableHeader(table, "Estoque");
            AddTableHeader(table, "Mínimo");
            AddTableHeader(table, "Ponto Reposição");
            AddTableHeader(table, "Status");
            AddTableHeader(table, "Localização");

            foreach (var product in products)
            {
                table.AddCell(new PdfPCell(new Phrase(product.ProductName, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.ProductCode, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.CurrentStock.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.MinimumStock.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.ReorderPoint.ToString(), _smallFont)) { Padding = 3, HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.StockStatus, _smallFont)) { Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(product.Location ?? "N/A", _smallFont)) { Padding = 3 });
            }

            document.Add(table);
        }

        private void AddTableHeader(PdfPTable table, string text)
        {
            var cell = new PdfPCell(new Phrase(text, _headerFont))
            {
                BackgroundColor = BaseColor.LightGray,
                Padding = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
        }
    }
}