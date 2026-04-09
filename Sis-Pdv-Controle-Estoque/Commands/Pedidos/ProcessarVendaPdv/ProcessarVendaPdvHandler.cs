
using MediatR;
using Microsoft.Extensions.Logging;
using Model;
using Interfaces;
using Commands;

namespace Commands.Pedidos.ProcessarVendaPdv
{
    public class ProcessarVendaPdvHandler : IRequestHandler<ProcessarVendaPdvCommand, Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;
        private readonly IRepositoryProduto _repositoryProduto;
        private readonly IRepositoryCliente _repositoryCliente;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProcessarVendaPdvHandler> _logger;

        public ProcessarVendaPdvHandler(
            IRepositoryPedido repositoryPedido,
            IRepositoryProdutoPedido repositoryProdutoPedido,
            IRepositoryProduto repositoryProduto,
            IRepositoryCliente repositoryCliente,
            IUnitOfWork unitOfWork,
            ILogger<ProcessarVendaPdvHandler> logger)
        {
            _repositoryPedido = repositoryPedido;
            _repositoryProdutoPedido = repositoryProdutoPedido;
            _repositoryProduto = repositoryProduto;
            _repositoryCliente = repositoryCliente;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response> Handle(ProcessarVendaPdvCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Idempotência: Verificar se já existe
                var existe = await _repositoryPedido.ObterPorIdAsync(request.Id, cancellationToken);
                if (existe != null)
                {
                    _logger.LogInformation("Venda {Id} já existe no banco central. Ignorando processamento repetido.", request.Id);
                    return new Response { Success = true, Data = "Venda já processada anteriormente." };
                }

                _logger.LogInformation("Iniciando processamento da venda {Id}. Itens: {Count}", request.Id, request.Itens.Count);

                // 2. Buscar ClienteId pelo CPF/CNPJ se informado
                Guid? clienteId = null;
                if (!string.IsNullOrWhiteSpace(request.CpfCnpjCliente))
                {
                    var cliente = await _repositoryCliente.ObterPorAsync(
                        c => c.CpfCnpj == request.CpfCnpjCliente, cancellationToken);
                    clienteId = cliente?.Id;
                }

                // 3. Criar Entidade Pedido
                var pedido = new Pedido(
                    request.ColaboradorId,
                    clienteId,
                    1, // Status: "Realizado/Pago"
                    request.DataDoPedido,
                    request.FormaPagamento ?? "DINHEIRO",
                    request.TotalPedido
                );
                
                // Forçar o ID que veio do PDV para garantir rastreabilidade
                pedido.Id = request.Id;

                await _repositoryPedido.AdicionarAsync(pedido, cancellationToken);
                
                // 3. Processar Itens e Estoque
                var listaItens = new List<Model.ProdutoPedido>();

                foreach (var itemDto in request.Itens)
                {
                    if (itemDto.Quantidade <= 0) continue;

                    var totalItem = itemDto.Preco * itemDto.Quantidade;

                    var item = new Model.ProdutoPedido(
                        pedido.Id,
                        itemDto.ProdutoId,
                        itemDto.CodBarras ?? "",
                        itemDto.Quantidade, 
                        totalItem
                    );
                    
                    listaItens.Add(item);

                    // Baixar Estoque
                    var produto = await _repositoryProduto.ObterPorIdAsync(itemDto.ProdutoId, cancellationToken);
                    if (produto != null)
                    {
                        var novaQtd = produto.QuantidadeEstoqueProduto - itemDto.Quantidade;
                        produto.UpdateStock(novaQtd, $"Venda PDV {request.Id}", request.ColaboradorId);
                        await _repositoryProduto.EditarAsync(produto, cancellationToken);
                    }
                }

                await _repositoryProdutoPedido.AdicionarListaAsync(listaItens, cancellationToken);

                // 4. Persistir tudo atomicamente
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Venda {Id} persistida com sucesso.", request.Id);
                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico ao processar venda {Id}.", request.Id);
                // Retornar Success=false faz o Consumer possivelmente jogar para DLQ ou Retry
                return new Response { Success = false, Data = ex.Message };
            }
        }
    }
}
