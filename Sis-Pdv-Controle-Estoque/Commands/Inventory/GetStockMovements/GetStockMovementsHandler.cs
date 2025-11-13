using Interfaces.Repositories;
using MediatR;
using Model;

namespace Commands.Inventory.GetStockMovements
{
    public class GetStockMovementsHandler : IRequestHandler<GetStockMovementsRequest, GetStockMovementsResponse>
    {
        private readonly IRepositoryStockMovement _stockMovementRepository;

        public GetStockMovementsHandler(IRepositoryStockMovement stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<GetStockMovementsResponse> Handle(GetStockMovementsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<StockMovement> movements;

            if (request.ProductId.HasValue && request.StartDate.HasValue && request.EndDate.HasValue)
            {
                movements = await _stockMovementRepository.GetByProductIdAndDateRangeAsync(
                    request.ProductId.Value, 
                    request.StartDate.Value, 
                    request.EndDate.Value, 
                    cancellationToken);
            }
            else if (request.ProductId.HasValue)
            {
                movements = await _stockMovementRepository.GetByProductIdAsync(request.ProductId.Value, cancellationToken);
            }
            else if (request.Type.HasValue)
            {
                movements = await _stockMovementRepository.GetByTypeAsync(request.Type.Value, cancellationToken);
            }
            else
            {
                movements = await _stockMovementRepository.GetRecentMovementsAsync(1000, cancellationToken);
            }

            // Apply additional filters if needed
            if (request.StartDate.HasValue && !request.ProductId.HasValue)
            {
                movements = movements.Where(m => m.MovementDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue && !request.ProductId.HasValue)
            {
                movements = movements.Where(m => m.MovementDate <= request.EndDate.Value);
            }

            var totalCount = movements.Count();
            var pagedMovements = movements
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var movementDtos = pagedMovements.Select(m => new StockMovementDto
            {
                Id = m.Id,
                ProductId = m.ProdutoId,
                ProductName = m.Produto?.NomeProduto ?? "N/A",
                ProductCode = m.Produto?.CodBarras ?? "N/A",
                Quantity = m.Quantity,
                Type = m.Type,
                TypeDescription = GetTypeDescription(m.Type),
                Reason = m.Reason,
                UnitCost = m.UnitCost,
                PreviousStock = m.PreviousStock,
                NewStock = m.NewStock,
                MovementDate = m.MovementDate,
                ReferenceDocument = m.ReferenceDocument,
                UserId = m.UserId
            });

            return new GetStockMovementsResponse
            {
                Movements = movementDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        private static string GetTypeDescription(StockMovementType type)
        {
            return type switch
            {
                StockMovementType.Entry => "Entrada",
                StockMovementType.Exit => "Saída",
                StockMovementType.Adjustment => "Ajuste",
                StockMovementType.Sale => "Venda",
                StockMovementType.Return => "Devolução",
                StockMovementType.Transfer => "Transferência",
                StockMovementType.Loss => "Perda",
                _ => "Desconhecido"
            };
        }
    }
}