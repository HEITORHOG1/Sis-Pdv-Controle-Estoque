using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Repositories;
using Interfaces.Services;
using Model.Reports;
using Microsoft.EntityFrameworkCore;

namespace Sis_Pdv_Controle_Estoque_API.Services.Reports
{
    public partial class ReportDataService : IReportDataService
    {
        private readonly IRepositoryPedido _pedidoRepository;
        private readonly IRepositoryProduto _produtoRepository;
        private readonly IRepositoryProdutoPedido _produtoPedidoRepository;
        private readonly IRepositoryStockMovement _stockMovementRepository;

        public ReportDataService(
            IRepositoryPedido pedidoRepository,
            IRepositoryProduto produtoRepository,
            IRepositoryProdutoPedido produtoPedidoRepository,
            IRepositoryStockMovement stockMovementRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _produtoPedidoRepository = produtoPedidoRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<SalesReportData> GetSalesReportDataAsync(DateTime startDate, DateTime endDate, Guid? salesPersonId = null)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var produtosPedido = await _produtoPedidoRepository.GetAllAsync();

            var filteredPedidos = pedidos
                .Where(p => p.DataDoPedido >= startDate && p.DataDoPedido <= endDate)
                .Where(p => salesPersonId == null || p.ColaboradorId == salesPersonId)
                .ToList();

            var salesItems = new List<SalesReportItem>();

            foreach (var pedido in filteredPedidos)
            {
                var pedidoItems = produtosPedido
                    .Where(pp => pp.PedidoId == pedido.Id)
                    .ToList();

                var salesItem = new SalesReportItem
                {
                    OrderId = pedido.Id,
                    OrderDate = pedido.DataDoPedido ?? DateTime.MinValue,
                    CustomerName = pedido.Cliente?.CpfCnpj ?? "N/A",
                    CustomerDocument = pedido.Cliente?.CpfCnpj ?? "N/A",
                    SalesPersonName = pedido.Colaborador?.NomeColaborador ?? "N/A",
                    PaymentMethod = pedido.FormaPagamento ?? "N/A",
                    TotalAmount = pedido.TotalPedido,
                    TotalItems = pedidoItems.Sum(pi => pi.QuantidadeItemPedido ?? 0),
                    Items = pedidoItems.Select(pi => new SalesReportItemDetail
                    {
                        ProductName = pi.Produto?.NomeProduto ?? "N/A",
                        ProductCode = pi.CodBarras ?? "N/A",
                        Quantity = pi.QuantidadeItemPedido ?? 0,
                        UnitPrice = 0, // TODO: Implementar preço do domínio de pricing
                        TotalPrice = pi.TotalProdutoPedido ?? 0
                    }).ToList()
                };

                salesItems.Add(salesItem);
            }

            var summary = new SalesReportSummary
            {
                TotalOrders = salesItems.Count,
                TotalRevenue = salesItems.Sum(s => s.TotalAmount),
                AverageOrderValue = salesItems.Count > 0 ? salesItems.Average(s => s.TotalAmount) : 0,
                TotalItemsSold = salesItems.Sum(s => s.TotalItems),
                RevenueByPaymentMethod = salesItems
                    .GroupBy(s => s.PaymentMethod)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalAmount)),
                OrdersByPaymentMethod = salesItems
                    .GroupBy(s => s.PaymentMethod)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return new SalesReportData
            {
                StartDate = startDate,
                EndDate = endDate,
                Sales = salesItems,
                Summary = summary
            };
        }

        public async Task<InventoryReportData> GetInventoryReportDataAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            var stockMovements = await _stockMovementRepository.GetAllAsync();

            var inventoryItems = produtos.Select(p =>
            {
                var lastMovement = stockMovements
                    .Where(sm => sm.ProdutoId == p.Id)
                    .OrderByDescending(sm => sm.MovementDate)
                    .FirstOrDefault();

                var inv = p.InventoryBalance;
                var status = (inv?.IsOutOfStock() ?? false) ? "Out of Stock" : ((inv?.IsLowStock() ?? false) ? "Low Stock" : "Normal");

                return new InventoryReportItem
                {
                    ProductId = p.Id,
                    ProductName = p.NomeProduto,
                    ProductCode = p.CodBarras,
                    Category = p.Categoria?.NomeCategoria ?? "N/A",
                    Supplier = p.Fornecedor?.NomeFantasia ?? "N/A",
                    CurrentStock = (int)(inv?.CurrentStock ?? 0),
                    MinimumStock = inv?.MinimumStock ?? 0,
                    MaximumStock = inv?.MaximumStock ?? 0,
                    ReorderPoint = inv?.ReorderPoint ?? 0,
                    Location = inv?.Location ?? string.Empty,
                    UnitCost = 0,  // TODO integrate pricing
                    UnitPrice = 0, // TODO integrate pricing
                    StockValue = 0, // (inv?.CurrentStock ?? 0) * UnitCost
                    StockStatus = status,
                    LastMovementDate = lastMovement?.MovementDate,
                    IsActive = p.StatusAtivo == 1
                };
            }).ToList();

            var summary = new InventoryReportSummary
            {
                TotalProducts = inventoryItems.Count,
                ActiveProducts = inventoryItems.Count(i => i.IsActive),
                InactiveProducts = inventoryItems.Count(i => !i.IsActive),
                LowStockProducts = inventoryItems.Count(i => i.StockStatus == "Low Stock"),
                OutOfStockProducts = inventoryItems.Count(i => i.StockStatus == "Out of Stock"),
                TotalStockValue = inventoryItems.Sum(i => i.StockValue),
                AverageStockValue = inventoryItems.Count > 0 ? inventoryItems.Average(i => i.StockValue) : 0,
                ProductsByCategory = inventoryItems
                    .GroupBy(i => i.Category)
                    .ToDictionary(g => g.Key, g => g.Count()),
                StockValueByCategory = inventoryItems
                    .GroupBy(i => i.Category)
                    .ToDictionary(g => g.Key, g => g.Sum(i => i.StockValue))
            };

            return new InventoryReportData
            {
                GeneratedAt = DateTime.UtcNow,
                Products = inventoryItems,
                Summary = summary
            };
        }

        public async Task<StockMovementReportData> GetStockMovementReportDataAsync(DateTime startDate, DateTime endDate, Guid? productId = null)
        {
            var movements = await _stockMovementRepository.GetAllAsync();

            var filteredMovements = movements
                .Where(m => m.MovementDate >= startDate && m.MovementDate <= endDate)
                .Where(m => productId == null || m.ProdutoId == productId)
                .OrderByDescending(m => m.MovementDate)
                .ToList();

            var movementItems = filteredMovements.Select(m => new StockMovementReportItem
            {
                MovementId = m.Id,
                MovementDate = m.MovementDate,
                ProductName = m.Produto?.NomeProduto ?? "N/A",
                ProductCode = m.Produto?.CodBarras ?? "N/A",
                MovementType = m.Type.ToString(),
                Quantity = m.Quantity,
                PreviousStock = m.PreviousStock,
                NewStock = m.NewStock,
                Reason = m.Reason,
                UnitCost = m.UnitCost,
                TotalValue = Math.Abs(m.Quantity) * m.UnitCost,
                ReferenceDocument = m.ReferenceDocument,
                UserName = "N/A" // TODO: Add user name lookup when user relationship is available
            }).ToList();

            var summary = new StockMovementReportSummary
            {
                TotalMovements = movementItems.Count,
                MovementsByType = movementItems
                    .GroupBy(m => m.MovementType)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ValueByMovementType = movementItems
                    .GroupBy(m => m.MovementType)
                    .ToDictionary(g => g.Key, g => g.Sum(m => m.TotalValue)),
                TotalInboundValue = movementItems
                    .Where(m => m.Quantity > 0)
                    .Sum(m => m.TotalValue),
                TotalOutboundValue = movementItems
                    .Where(m => m.Quantity < 0)
                    .Sum(m => m.TotalValue),
                NetMovementValue = movementItems.Sum(m => m.Quantity > 0 ? m.TotalValue : -m.TotalValue)
            };

            return new StockMovementReportData
            {
                StartDate = startDate,
                EndDate = endDate,
                Movements = movementItems,
                Summary = summary
            };
        }

        public async Task<FinancialReportData> GetFinancialReportDataAsync(DateTime startDate, DateTime endDate)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var produtosPedido = await _produtoPedidoRepository.GetAllAsync();

            var filteredPedidos = pedidos
                .Where(p => p.DataDoPedido >= startDate && p.DataDoPedido <= endDate)
                .ToList();

            var dailyData = new List<FinancialReportDailyData>();
            var productData = new List<FinancialReportProductData>();

            // Calculate daily data
            var dailyGroups = filteredPedidos
                .GroupBy(p => p.DataDoPedido?.Date ?? DateTime.MinValue.Date)
                .OrderBy(g => g.Key);

            foreach (var dayGroup in dailyGroups)
            {
                var dayPedidos = dayGroup.ToList();
                var dayProdutosPedido = produtosPedido
                    .Where(pp => dayPedidos.Any(p => p.Id == pp.PedidoId))
                    .ToList();

                var dayRevenue = dayPedidos.Sum(p => p.TotalPedido);
                var dayCost = 0m; // TODO: Implementar cálculo de custo usando domínio de pricing

                dailyData.Add(new FinancialReportDailyData
                {
                    Date = dayGroup.Key,
                    Revenue = dayRevenue,
                    Cost = dayCost,
                    Profit = dayRevenue - dayCost,
                    OrderCount = dayPedidos.Count,
                    ItemsSold = dayProdutosPedido.Sum(pp => pp.QuantidadeItemPedido ?? 0)
                });
            }

            // Calculate product data
            var productGroups = produtosPedido
                .Where(pp => filteredPedidos.Any(p => p.Id == pp.PedidoId))
                .GroupBy(pp => pp.ProdutoId);

            foreach (var productGroup in productGroups)
            {
                var productItems = productGroup.ToList();
                var produto = productItems.First().Produto;

                if (produto != null)
                {
                    var quantitySold = productItems.Sum(pi => pi.QuantidadeItemPedido ?? 0);
                    var revenue = productItems.Sum(pi => pi.TotalProdutoPedido ?? 0);
                    var cost = 0m; // TODO: Implementar cálculo de custo usando domínio de pricing

                    productData.Add(new FinancialReportProductData
                    {
                        ProductName = produto.NomeProduto,
                        ProductCode = produto.CodBarras,
                        Category = produto.Categoria?.NomeCategoria ?? "N/A",
                        QuantitySold = quantitySold,
                        Revenue = revenue,
                        Cost = cost,
                        Profit = revenue - cost,
                        ProfitMargin = revenue > 0 ? ((revenue - cost) / revenue) * 100 : 0
                    });
                }
            }

            var totalRevenue = dailyData.Sum(d => d.Revenue);
            var totalCost = dailyData.Sum(d => d.Cost);

            var summary = new FinancialReportSummary
            {
                TotalRevenue = totalRevenue,
                TotalCost = totalCost,
                GrossProfit = totalRevenue - totalCost,
                GrossProfitMargin = totalRevenue > 0 ? ((totalRevenue - totalCost) / totalRevenue) * 100 : 0,
                TotalOrders = filteredPedidos.Count,
                TotalItemsSold = dailyData.Sum(d => d.ItemsSold),
                AverageOrderValue = filteredPedidos.Count > 0 ? filteredPedidos.Average(p => p.TotalPedido) : 0,
                RevenueByPaymentMethod = filteredPedidos
                    .GroupBy(p => p.FormaPagamento ?? "N/A")
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.TotalPedido)),
                ProfitByCategory = productData
                    .GroupBy(p => p.Category)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Profit))
            };

            return new FinancialReportData
            {
                StartDate = startDate,
                EndDate = endDate,
                Summary = summary,
                DailyData = dailyData,
                ProductData = productData.OrderByDescending(p => p.Revenue).ToList()
            };
        }

        public async Task<List<InventoryReportItem>> GetLowStockProductsAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();

            return produtos
                .Where(p => p.StatusAtivo == 1)
                .Select(p => new InventoryReportItem
                {
                    ProductId = p.Id,
                    ProductName = p.NomeProduto,
                    ProductCode = p.CodBarras,
                    Category = p.Categoria?.NomeCategoria ?? "N/A",
                    Supplier = p.Fornecedor?.NomeFantasia ?? "N/A",
                    CurrentStock = (int)(p.InventoryBalance?.CurrentStock ?? 0),
                    MinimumStock = p.InventoryBalance?.MinimumStock ?? 0,
                    MaximumStock = p.InventoryBalance?.MaximumStock ?? 0,
                    ReorderPoint = p.InventoryBalance?.ReorderPoint ?? 0,
                    Location = p.InventoryBalance?.Location ?? string.Empty,
                    UnitCost = 0,
                    UnitPrice = 0,
                    StockValue = 0,
                    StockStatus = (p.InventoryBalance?.IsOutOfStock() ?? false) ? "Out of Stock" : ((p.InventoryBalance?.IsLowStock() ?? false) ? "Low Stock" : "OK"),
                    IsActive = p.StatusAtivo == 1
                })
                .OrderBy(p => p.CurrentStock)
                .ToList();
        }
    }
}