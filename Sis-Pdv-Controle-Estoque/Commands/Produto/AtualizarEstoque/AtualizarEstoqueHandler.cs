using MediatR;
using prmToolkit.NotificationPattern;
using Model;
using Interfaces.Repositories;

namespace Commands.Produto.AtualizarEstoque
{
    public class AtualizarEstoqueHandler : Notifiable, IRequestHandler<AtualizarEstoqueRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;
        private readonly IRepositoryInventoryBalance _inventoryBalanceRepository;
        private readonly IRepositoryStockMovement _stockMovementRepository;

        public AtualizarEstoqueHandler(
            IMediator mediator, 
            IRepositoryProduto repositoryProduto,
            IRepositoryInventoryBalance inventoryBalanceRepository,
            IRepositoryStockMovement stockMovementRepository)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
            _inventoryBalanceRepository = inventoryBalanceRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<Response> Handle(AtualizarEstoqueRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AtualizarEstoqueRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

            var produto = _repositoryProduto.Listar().Where(x => x.Id == request.Id).FirstOrDefault();

            if (produto == null)
            {
                AddNotification("Produto", "Produto não encontrado");
                return new Response(this);
            }

            // Get or create inventory balance
            var inventoryBalance = await _inventoryBalanceRepository.GetByProductIdAsync(request.Id, cancellationToken);
            if (inventoryBalance == null)
            {
                inventoryBalance = new InventoryBalance(request.Id);
                await _inventoryBalanceRepository.AddAsync(inventoryBalance, cancellationToken);
            }

            if (inventoryBalance.CurrentStock <= 0)
            {
                AddNotification("Produto", "Produto Sem Estoque");
                return new Response(this);
            }
            if (request.quatidadeEstoqueProduto > inventoryBalance.CurrentStock)
            {
                AddNotification("Produto", "Quantidade em estoque " + inventoryBalance.CurrentStock);
                return new Response(this);
            }

            var previousStock = inventoryBalance.CurrentStock;
            var newStock = inventoryBalance.CurrentStock - request.quatidadeEstoqueProduto;

            // Create stock movement record
            var stockMovement = new StockMovement(
                request.Id,
                -request.quatidadeEstoqueProduto, // Negative for exit
                StockMovementType.Exit,
                "Saída de estoque via atualização",
                0, // Unit cost not available
                previousStock,
                newStock,
                null,
                null
            );

            // Update inventory balance
            inventoryBalance.UpdateStock(newStock);

            try
            {
                await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);
                await _inventoryBalanceRepository.UpdateAsync(inventoryBalance, cancellationToken);

                //Criar meu objeto de resposta
                var response = new Response(this, produto);
                
                Console.WriteLine($"Estoque atualizado. Novo estoque: {newStock}");

                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                AddNotification("Error", $"Erro ao atualizar estoque: {ex.Message}");
                return new Response(this);
            }
        }
    }
}
