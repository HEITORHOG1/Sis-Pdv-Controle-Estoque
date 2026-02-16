using Sis_Pdv_Controle_Estoque_Form.Dto.PDV;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Services.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Services.PDV
{
    /// <summary>
    /// Gerenciador centralizado para operações do PDV
    /// </summary>
    public class PdvManager
    {
        private readonly ProdutoService _produtoService;
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly ProdutoPedidoService _produtoPedidoService;
        private readonly ConfiguracaoPdvDto _configuracao;
        
        public VendaDto VendaAtual { get; private set; }
        public string OperadorAtual { get; private set; }
        public string CaixaAtual { get; private set; }
        public bool CaixaAberto { get; private set; }
        
        public PdvManager()
        {
            _produtoService = new ProdutoService();
            _pedidoService = new PedidoService();
            _clienteService = new ClienteService();
            _produtoPedidoService = new ProdutoPedidoService();
            _configuracao = CarregarConfiguracoes();
            
            PdvLogger.LogInicioVenda(Guid.NewGuid(), "PDV Manager inicializado");
        }
        
        #region Operações de Caixa
        
        /// <summary>
        /// Abre o caixa para operação
        /// </summary>
        public async Task<bool> AbrirCaixa(string operador, string caixa, decimal valorInicial = 0)
        {
            try
            {
                if (CaixaAberto)
                {
                    throw new InvalidOperationException("Caixa já está aberto");
                }
                
                OperadorAtual = operador;
                CaixaAtual = caixa;
                CaixaAberto = true;
                
                PdvLogger.LogAberturaCaixa(operador, caixa, valorInicial);
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AbrirCaixa", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Fecha o caixa
        /// </summary>
        public async Task<bool> FecharCaixa(decimal valorFinal, int totalVendas)
        {
            try
            {
                if (!CaixaAberto)
                {
                    throw new InvalidOperationException("Caixa não está aberto");
                }
                
                // Verifica se há venda em andamento
                if (VendaAtual != null && VendaAtual.StatusVenda == "ABERTA")
                {
                    throw new InvalidOperationException("Existe venda em andamento. Finalize ou cancele antes de fechar o caixa.");
                }
                
                PdvLogger.LogFechamentoCaixa(OperadorAtual, CaixaAtual, valorFinal, totalVendas);
                
                CaixaAberto = false;
                OperadorAtual = string.Empty;
                CaixaAtual = string.Empty;
                VendaAtual = null;
                
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("FecharCaixa", ex.Message, ex);
                throw;
            }
        }
        
        #endregion
        
        #region Operações de Venda
        
        /// <summary>
        /// Inicia uma nova venda
        /// </summary>
        public VendaDto IniciarNovaVenda()
        {
            try
            {
                if (!CaixaAberto)
                {
                    throw new InvalidOperationException("Caixa deve estar aberto para iniciar venda");
                }
                
                if (VendaAtual != null && VendaAtual.StatusVenda == "ABERTA")
                {
                    throw new InvalidOperationException("Já existe uma venda em andamento");
                }
                
                VendaAtual = new VendaDto
                {
                    Id = Guid.NewGuid(),
                    DataVenda = DateTime.Now,
                    NomeOperador = OperadorAtual,
                    StatusVenda = "ABERTA"
                };
                
                PdvLogger.LogInicioVenda(VendaAtual.Id, OperadorAtual);
                return VendaAtual;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("IniciarNovaVenda", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Busca produto por código de barras
        /// </summary>
        public async Task<Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data> BuscarProdutoPorCodigo(string codigoBarras)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrWhiteSpace(codigoBarras))
                {
                    throw new ArgumentException("Código de barras é obrigatório");
                }
                
                var response = await _produtoService.ListarProdutoPorCodBarras(codigoBarras);
                
                sw.Stop();
                PdvLogger.LogApiCall("ListarProdutoPorCodBarras", "GET", sw.Elapsed, response.IsValidResponse());
                
                if (response.IsValidResponse() && response.data?.Any() == true)
                {
                    var produto = response.data.First();
                    PdvLogger.LogBuscarProduto(codigoBarras, true, produto.nomeProduto);
                    return produto;
                }
                
                PdvLogger.LogBuscarProduto(codigoBarras, false);
                return null;
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("BuscarProdutoPorCodigo", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adiciona item ao carrinho
        /// </summary>
        public async Task<bool> AdicionarItem(string codigoBarras, int quantidade = 1)
        {
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                var produto = await BuscarProdutoPorCodigo(codigoBarras);
                if (produto == null)
                {
                    throw new ArgumentException($"Produto não encontrado: {codigoBarras}");
                }
                
                // Verifica se produto pode ser vendido
                if (!produto.PodeSerVendido())
                {
                    var alertas = produto.GetAlertasVenda();
                    throw new InvalidOperationException($"Produto não pode ser vendido: {string.Join(", ", alertas)}");
                }
                
                // Verifica estoque
                if (quantidade > produto.quatidadeEstoqueProduto && !_configuracao.PermitirVendaSemEstoque)
                {
                    PdvLogger.LogAlertaEstoque(codigoBarras, produto.nomeProduto, produto.quatidadeEstoqueProduto, quantidade);
                    throw new InvalidOperationException($"Quantidade solicitada ({quantidade}) maior que estoque disponível ({produto.quatidadeEstoqueProduto})");
                }
                
                // Verifica vencimento
                if (produto.dataVencimento > DateTime.MinValue && _configuracao.AlertarProdutoVencimento)
                {
                    var diasVencimento = (produto.dataVencimento - DateTime.Now).Days;
                    if (diasVencimento <= 0)
                    {
                        PdvLogger.LogAlertaVencimento(codigoBarras, produto.nomeProduto, produto.dataVencimento, diasVencimento);
                        throw new InvalidOperationException("Produto vencido não pode ser vendido");
                    }
                    else if (diasVencimento <= _configuracao.DiasAlertaVencimento)
                    {
                        PdvLogger.LogAlertaVencimento(codigoBarras, produto.nomeProduto, produto.dataVencimento, diasVencimento);
                        // Continua a venda mas registra o alerta
                    }
                }
                
                var item = produto.ToItemCarrinho(quantidade);
                VendaAtual.AdicionarItem(item);
                
                PdvLogger.LogAdicionarItem(codigoBarras, produto.nomeProduto, quantidade, produto.precoVenda, item.Total);
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AdicionarItem", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Cancela item do carrinho
        /// </summary>
        public bool CancelarItem(int codigo, string motivo = "")
        {
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                var item = VendaAtual.Itens.FirstOrDefault(i => i.Codigo == codigo);
                if (item == null)
                {
                    throw new ArgumentException($"Item não encontrado: {codigo}");
                }
                
                if (item.Cancelado)
                {
                    throw new InvalidOperationException("Item já está cancelado");
                }
                
                VendaAtual.CancelarItem(codigo);
                PdvLogger.LogCancelarItem(codigo, item.CodigoBarras, item.Descricao, OperadorAtual, motivo);
                
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CancelarItem", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Altera quantidade de um item
        /// </summary>
        public async Task<bool> AlterarQuantidade(int codigo, int novaQuantidade)
        {
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                if (!_configuracao.PermitirAlterarQuantidade)
                {
                    throw new InvalidOperationException("Alteração de quantidade não permitida");
                }
                
                var item = VendaAtual.Itens.FirstOrDefault(i => i.Codigo == codigo);
                if (item == null)
                {
                    throw new ArgumentException($"Item não encontrado: {codigo}");
                }
                
                if (item.Cancelado)
                {
                    throw new InvalidOperationException("Não é possível alterar quantidade de item cancelado");
                }
                
                if (novaQuantidade <= 0)
                {
                    throw new ArgumentException("Quantidade deve ser maior que zero");
                }
                
                // Verifica estoque novamente
                if (novaQuantidade > item.EstoqueDisponivel && !_configuracao.PermitirVendaSemEstoque)
                {
                    throw new InvalidOperationException($"Quantidade solicitada ({novaQuantidade}) maior que estoque disponível ({item.EstoqueDisponivel})");
                }
                
                var quantidadeAnterior = item.Quantidade;
                item.Quantidade = novaQuantidade;
                
                PdvLogger.LogAlterarQuantidade(item.CodigoBarras, quantidadeAnterior, novaQuantidade, OperadorAtual);
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AlterarQuantidade", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Define forma de pagamento
        /// </summary>
        public void DefinirFormaPagamento(string formaPagamento, decimal valorRecebido)
        {
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                if (string.IsNullOrWhiteSpace(formaPagamento))
                {
                    throw new ArgumentException("Forma de pagamento é obrigatória");
                }
                
                if (valorRecebido < VendaAtual.ValorFinal)
                {
                    throw new ArgumentException("Valor recebido deve ser maior ou igual ao valor final");
                }
                
                VendaAtual.FormaPagamento = formaPagamento.ToUpper();
                VendaAtual.ValorRecebido = valorRecebido;
                
                PdvLogger.LogFormaPagamento(formaPagamento, valorRecebido, $"Troco: {VendaAtual.Troco:C2}");
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("DefinirFormaPagamento", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Aplica desconto na venda
        /// </summary>
        public void AplicarDesconto(decimal valorDesconto, string motivo = "")
        {
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                if (valorDesconto < 0)
                {
                    throw new ArgumentException("Desconto não pode ser negativo");
                }
                
                var percentual = VendaAtual.ValorTotal.CalcularPercentualDesconto(valorDesconto);
                if (percentual > _configuracao.DescontoMaximo)
                {
                    throw new InvalidOperationException($"Desconto não pode ser maior que {_configuracao.DescontoMaximo}%");
                }
                
                VendaAtual.ValorDesconto = valorDesconto;
                PdvLogger.LogDesconto(VendaAtual.ValorTotal, valorDesconto, percentual, OperadorAtual, motivo);
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AplicarDesconto", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Finaliza a venda
        /// </summary>
        public async Task<Guid> FinalizarVenda(string cpfCnpjCliente = "")
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (VendaAtual == null || VendaAtual.StatusVenda != "ABERTA")
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                // Valida venda
                var erros = VendaAtual.Validar();
                if (erros.Any())
                {
                    throw new InvalidOperationException($"Venda inválida: {string.Join(", ", erros)}");
                }
                
                if (!VendaAtual.PodeSerFinalizada())
                {
                    throw new InvalidOperationException("Venda não pode ser finalizada");
                }
                
                // Registra cliente se informado
                Guid? clienteId = null;
                if (!string.IsNullOrWhiteSpace(cpfCnpjCliente) && cpfCnpjCliente.IsValidCpfCnpj())
                {
                    VendaAtual.CpfCnpjCliente = cpfCnpjCliente;
                    // Aqui poderia cadastrar o cliente se necessário
                }
                
                // Cria pedido na API
                var pedidoDto = new Dto.Pedido.PedidoDto
                {
                    ClienteId = clienteId,
                    ColaboradorId = VendaAtual.ColaboradorId,
                    dataDoPedido = VendaAtual.DataVenda,
                    totalPedido = VendaAtual.ValorFinal,
                    formaPagamento = VendaAtual.FormaPagamento
                };
                
                var responsePedido = await _pedidoService.AdicionarPedido(pedidoDto);
                if (!responsePedido.IsValidResponse())
                {
                    throw new InvalidOperationException("Erro ao criar pedido");
                }
                
                var pedidoId = responsePedido.data.Id;
                
                // Adiciona itens do pedido
                foreach (var item in VendaAtual.Itens.Where(i => !i.Cancelado))
                {
                    var produtoPedidoDto = new Dto.ProdutoPedido.ProdutoPedidoDto
                    {
                        codBarras = item.CodigoBarras,
                        ProdutoId = item.ProdutoId,
                        PedidoId = pedidoId,
                        quantidadeItemPedido = item.Quantidade,
                        totalProdutoPedido = item.Total
                    };
                    
                    await _produtoPedidoService.AdicionarProdutoPedido(produtoPedidoDto);
                    
                    // Atualiza estoque
                    var produtoEstoque = new ProdutoDto
                    {
                        Id = item.ProdutoId,
                        quatidadeEstoqueProduto = item.Quantidade
                    };
                    
                    await _produtoService.AtualizarEstoque(produtoEstoque);
                }
                
                VendaAtual.StatusVenda = "FINALIZADA";
                VendaAtual.Id = pedidoId;
                
                sw.Stop();
                PdvLogger.LogFinalizarVenda(pedidoId, VendaAtual.FormaPagamento, VendaAtual.ValorTotal, 
                    VendaAtual.ValorRecebido, VendaAtual.Troco, VendaAtual.QuantidadeItensAtivos);
                
                return pedidoId;
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("FinalizarVenda", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Cancela a venda atual
        /// </summary>
        public void CancelarVenda(string motivo = "")
        {
            try
            {
                if (VendaAtual == null)
                {
                    throw new InvalidOperationException("Não há venda ativa");
                }
                
                if (_configuracao.ExigirAutorizacaoCancelamento)
                {
                    // Aqui seria implementada a autorização
                    // Por enquanto, apenas registra no log
                }
                
                var vendaId = VendaAtual.Id;
                VendaAtual.StatusVenda = "CANCELADA";
                
                PdvLogger.LogCancelarVenda(vendaId, OperadorAtual, motivo);
                
                VendaAtual = null;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CancelarVenda", ex.Message, ex);
                throw;
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        /// <summary>
        /// Carrega configurações do PDV
        /// </summary>
        private ConfiguracaoPdvDto CarregarConfiguracoes()
        {
            // Por enquanto retorna configuração padrão
            // Posteriormente pode ser carregada de arquivo de configuração ou banco
            return new ConfiguracaoPdvDto();
        }
        
        /// <summary>
        /// Obtém resumo da venda atual
        /// </summary>
        public string GetResumoVendaAtual()
        {
            if (VendaAtual == null)
                return "Nenhuma venda ativa";
            
            return VendaAtual.GetResumoVenda();
        }
        
        /// <summary>
        /// Obtém estatísticas do caixa
        /// </summary>
        public Dictionary<string, object> GetEstatisticasCaixa()
        {
            var stats = new Dictionary<string, object>
            {
                ["CaixaAberto"] = CaixaAberto,
                ["Operador"] = OperadorAtual ?? "N/A",
                ["Caixa"] = CaixaAtual ?? "N/A",
                ["VendaAtiva"] = VendaAtual != null,
                ["StatusVenda"] = VendaAtual?.StatusVenda ?? "N/A"
            };
            
            if (VendaAtual != null)
            {
                stats["ItensCarrinho"] = VendaAtual.QuantidadeItensAtivos;
                stats["ValorTotal"] = VendaAtual.ValorTotal;
                stats["ValorFinal"] = VendaAtual.ValorFinal;
            }
            
            return stats;
        }
        
        /// <summary>
        /// Valida se operação pode ser executada
        /// </summary>
        public bool ValidarOperacao(string operacao)
        {
            if (!CaixaAberto && operacao != "ABRIR_CAIXA")
                return false;
            
            if (operacao == "FINALIZAR_VENDA" || operacao == "CANCELAR_VENDA")
                return VendaAtual != null && VendaAtual.StatusVenda == "ABERTA";
            
            if (operacao == "ADICIONAR_ITEM" || operacao == "CANCELAR_ITEM")
                return VendaAtual != null && VendaAtual.StatusVenda == "ABERTA";
            
            return true;
        }
        
        #endregion
    }
}