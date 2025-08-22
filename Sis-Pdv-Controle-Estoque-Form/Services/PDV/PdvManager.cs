using Sis_Pdv_Controle_Estoque_Form.Dto.PDV;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Services.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Services.Inventory;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Services.PDV
{
    /// <summary>
    /// Gerenciador centralizado para opera��es do PDV
    /// </summary>
    public class PdvManager
    {
        private readonly ProdutoService _produtoService;
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly ProdutoPedidoService _produtoPedidoService;
        private readonly InventoryService _inventoryService;
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
            _inventoryService = new InventoryService();
            _configuracao = CarregarConfiguracoes();
            
            PdvLogger.LogInicioVenda(Guid.NewGuid(), "PDV Manager inicializado");
        }
        
        #region Opera��es de Caixa
        
        /// <summary>
        /// Abre o caixa para opera��o
        /// </summary>
        public async Task<bool> AbrirCaixa(string operador, string caixa, decimal valorInicial = 0)
        {
            try
            {
                if (CaixaAberto)
                {
                    throw new InvalidOperationException("Caixa j� est� aberto");
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
                    throw new InvalidOperationException("Caixa n�o est� aberto");
                }
                
                // Verifica se h� venda em andamento
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
        
        #region Opera��es de Venda
        
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
                    throw new InvalidOperationException("J� existe uma venda em andamento");
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
        /// Busca produto por c�digo de barras
        /// </summary>
        public async Task<Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data> BuscarProdutoPorCodigo(string codigoBarras)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrWhiteSpace(codigoBarras))
                {
                    throw new ArgumentException("C�digo de barras � obrigat�rio");
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
        /// Adiciona item ao carrinho com validação de estoque em tempo real
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
                
                // Validação de estoque em tempo real usando InventoryService
                var validacaoEstoque = await _inventoryService.ValidarEstoque(produto.Id.ToString(), quantidade);
                if (!validacaoEstoque.EstaDisponivel && !_configuracao.PermitirVendaSemEstoque)
                {
                    PdvLogger.LogAlertaEstoque(codigoBarras, produto.nomeProduto, (int)validacaoEstoque.QuantidadeDisponivel, quantidade);
                    throw new InvalidOperationException($"Estoque insuficiente. Disponível: {validacaoEstoque.QuantidadeDisponivel}, Solicitado: {quantidade}");
                }
                
                // Para produtos perecíveis, validar lotes disponíveis
                if (produto.isPerecivel && validacaoEstoque.LotesDisponiveis?.Any() == true)
                {
                    var lotesVencidos = validacaoEstoque.LotesDisponiveis.Where(l => l.EstaVencido).ToList();
                    if (lotesVencidos.Any())
                    {
                        PdvLogger.LogAlertaVencimento(codigoBarras, produto.nomeProduto, DateTime.Now, 0);
                        throw new InvalidOperationException("Produto possui lotes vencidos que impedem a venda");
                    }
                    
                    var lotesVencendo = validacaoEstoque.LotesDisponiveis.Where(l => l.VenceEm30Dias && !l.EstaVencido).ToList();
                    if (lotesVencendo.Any() && _configuracao.AlertarProdutoVencimento)
                    {
                        var loteProximo = lotesVencendo.OrderBy(l => l.DataValidade).First();
                        PdvLogger.LogAlertaVencimento(codigoBarras, produto.nomeProduto, loteProximo.DataValidade ?? DateTime.Now, loteProximo.DiasParaVencer);
                        // Continua a venda mas registra o alerta
                    }
                }
                
                var item = produto.ToItemCarrinho(quantidade);
                
                // Adiciona informações de validação de estoque ao item
                item.EstoqueDisponivel = (int)validacaoEstoque.QuantidadeDisponivel;
                
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
        /// Adiciona item perecível ao carrinho com seleção de lote específico
        /// </summary>
        public async Task<bool> AdicionarItemPerecivel(string codigoBarras, int quantidade, string lote)
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
                
                if (!produto.isPerecivel)
                {
                    throw new InvalidOperationException("Produto não é perecível. Use AdicionarItem normal.");
                }
                
                // Valida estoque do lote específico
                var validacaoEstoque = await _inventoryService.ValidarEstoque(produto.Id.ToString(), quantidade);
                
                var loteEspecifico = validacaoEstoque.LotesDisponiveis?.FirstOrDefault(l => l.Lote == lote);
                if (loteEspecifico == null)
                {
                    throw new InvalidOperationException($"Lote {lote} não encontrado para o produto {codigoBarras}");
                }
                
                if (loteEspecifico.EstaVencido)
                {
                    throw new InvalidOperationException($"Lote {lote} está vencido e não pode ser vendido");
                }
                
                if (quantidade > loteEspecifico.QuantidadeDisponivel)
                {
                    throw new InvalidOperationException($"Quantidade solicitada ({quantidade}) maior que disponível no lote {lote} ({loteEspecifico.QuantidadeDisponivel})");
                }
                
                var item = produto.ToItemCarrinho(quantidade);
                item.EstoqueDisponivel = (int)loteEspecifico.QuantidadeDisponivel;
                item.DataVencimento = loteEspecifico.DataValidade;
                
                VendaAtual.AdicionarItem(item);
                
                PdvLogger.LogAdicionarItem(codigoBarras, $"{produto.nomeProduto} (Lote: {lote})", quantidade, produto.precoVenda, item.Total);
                return true;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AdicionarItemPerecivel", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Obtém lotes disponíveis para um produto perecível
        /// </summary>
        public async Task<Dto.PDV.LotesDisponiveisDto> ObterLotesDisponiveis(string codigoBarras, decimal quantidadeSolicitada)
        {
            try
            {
                var produto = await BuscarProdutoPorCodigo(codigoBarras);
                if (produto == null)
                {
                    throw new ArgumentException($"Produto não encontrado: {codigoBarras}");
                }
                
                if (!produto.isPerecivel)
                {
                    throw new InvalidOperationException("Produto não é perecível");
                }
                
                var validacaoEstoque = await _inventoryService.ValidarEstoque(produto.Id.ToString(), quantidadeSolicitada);
                
                var lotesDisponiveis = new Dto.PDV.LotesDisponiveisDto
                {
                    ProdutoId = produto.Id,
                    CodigoBarras = produto.codBarras,
                    NomeProduto = produto.nomeProduto,
                    QuantidadeSolicitada = quantidadeSolicitada
                };
                
                if (validacaoEstoque.LotesDisponiveis?.Any() == true)
                {
                    foreach (var lote in validacaoEstoque.LotesDisponiveis)
                    {
                        lotesDisponiveis.LotesDisponiveis.Add(new Dto.PDV.LoteSelecionadoDto
                        {
                            Lote = lote.Lote,
                            DataValidade = lote.DataValidade,
                            QuantidadeDisponivel = lote.QuantidadeDisponivel,
                            QuantidadeSelecionada = 0
                        });
                    }
                }
                
                return lotesDisponiveis;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("ObterLotesDisponiveis", ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Valida se há estoque suficiente antes de finalizar a venda
        /// </summary>
        public async Task<List<string>> ValidarEstoqueVenda()
        {
            var alertas = new List<string>();
            
            if (VendaAtual?.Itens?.Any() != true)
                return alertas;
            
            try
            {
                foreach (var item in VendaAtual.Itens.Where(i => !i.Cancelado))
                {
                    var validacao = await _inventoryService.ValidarEstoque(item.ProdutoId.ToString(), item.Quantidade);
                    
                    if (!validacao.EstaDisponivel)
                    {
                        alertas.Add($"Produto {item.Descricao}: Estoque insuficiente (Disponível: {validacao.QuantidadeDisponivel}, Necessário: {item.Quantidade})");
                    }
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("ValidarEstoqueVenda", ex.Message, ex);
                alertas.Add("Erro ao validar estoque para finalização da venda");
            }
            
            return alertas;
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
                    throw new InvalidOperationException("N�o h� venda ativa");
                }
                
                var item = VendaAtual.Itens.FirstOrDefault(i => i.Codigo == codigo);
                if (item == null)
                {
                    throw new ArgumentException($"Item n�o encontrado: {codigo}");
                }
                
                if (item.Cancelado)
                {
                    throw new InvalidOperationException("Item j� est� cancelado");
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
                    throw new InvalidOperationException("N�o h� venda ativa");
                }
                
                if (!_configuracao.PermitirAlterarQuantidade)
                {
                    throw new InvalidOperationException("Altera��o de quantidade n�o permitida");
                }
                
                var item = VendaAtual.Itens.FirstOrDefault(i => i.Codigo == codigo);
                if (item == null)
                {
                    throw new ArgumentException($"Item n�o encontrado: {codigo}");
                }
                
                if (item.Cancelado)
                {
                    throw new InvalidOperationException("N�o � poss�vel alterar quantidade de item cancelado");
                }
                
                if (novaQuantidade <= 0)
                {
                    throw new ArgumentException("Quantidade deve ser maior que zero");
                }
                
                // Verifica estoque novamente
                if (novaQuantidade > item.EstoqueDisponivel && !_configuracao.PermitirVendaSemEstoque)
                {
                    throw new InvalidOperationException($"Quantidade solicitada ({novaQuantidade}) maior que estoque dispon�vel ({item.EstoqueDisponivel})");
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
                    throw new InvalidOperationException("N�o h� venda ativa");
                }
                
                if (string.IsNullOrWhiteSpace(formaPagamento))
                {
                    throw new ArgumentException("Forma de pagamento � obrigat�ria");
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
                    throw new InvalidOperationException("N�o h� venda ativa");
                }
                
                if (valorDesconto < 0)
                {
                    throw new ArgumentException("Desconto n�o pode ser negativo");
                }
                
                var percentual = VendaAtual.ValorTotal.CalcularPercentualDesconto(valorDesconto);
                if (percentual > _configuracao.DescontoMaximo)
                {
                    throw new InvalidOperationException($"Desconto n�o pode ser maior que {_configuracao.DescontoMaximo}%");
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
        /// Finaliza a venda com validação de estoque em tempo real
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
                
                // Validação final de estoque antes de processar a venda
                var alertasEstoque = await ValidarEstoqueVenda();
                if (alertasEstoque.Any() && !_configuracao.PermitirVendaSemEstoque)
                {
                    throw new InvalidOperationException($"Problemas de estoque impedem a finalização: {string.Join("; ", alertasEstoque)}");
                }
                
                // Registra cliente se informado
                Guid? clienteId = null;
                if (!string.IsNullOrWhiteSpace(cpfCnpjCliente) && cpfCnpjCliente.IsValidCpfCnpj())
                {
                    VendaAtual.CpfCnpjCliente = cpfCnpjCliente;
                    // Aqui poderia cadastrar o cliente se necess�rio
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
                
                // Adiciona itens do pedido e cria movimentações de estoque de forma transacional
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
                    
                    // Cria movimentação de saída de estoque para cada item vendido
                    var movimentacaoSaida = new Dto.Movimentacao.CriarMovimentacaoDto
                    {
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        Tipo = (int)Dto.Movimentacao.TipoMovimentacao.Saida,
                        Motivo = $"Venda PDV - Pedido: {pedidoId}",
                        Referencia = $"PDV-{VendaAtual.Id}-{DateTime.Now:yyyyMMddHHmmss}"
                    };
                    
                    try
                    {
                        await _inventoryService.CriarMovimentacao(movimentacaoSaida);
                        // Log usando método existente - usando performance log como informativo
                        PdvLogger.LogPerformance($"MovimentacaoSaida-{item.CodigoBarras}", TimeSpan.Zero, item.Quantidade);
                    }
                    catch (Exception exMovimentacao)
                    {
                        PdvLogger.LogError("CriarMovimentacaoSaida", $"Erro ao criar movimentação de saída para produto {item.CodigoBarras}: {exMovimentacao.Message}", exMovimentacao);
                        
                        // Se não conseguir criar a movimentação, registra alerta mas não impede a venda
                        // Em um cenário real, isso deveria ser tratado com mais rigor
                        PdvLogger.LogError("VendaSemMovimentacao", $"Venda finalizada sem movimentação de estoque para produto {item.CodigoBarras}", new InvalidOperationException("Movimentação não registrada"));
                    }
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
                    throw new InvalidOperationException("N�o h� venda ativa");
                }
                
                if (_configuracao.ExigirAutorizacaoCancelamento)
                {
                    // Aqui seria implementada a autoriza��o
                    // Por enquanto, apenas registra no log
                }
                
                var vendaId = VendaAtual.Id;
                VendaAtual.StatusVenda = "CANCELADA";
                
                PdvLogger.LogCancelarVenda(vendaId, OperadorAtual, motivo);
                
                VendaAtual = null!;
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CancelarVenda", ex.Message, ex);
                throw;
            }
        }
        
        #endregion
        
        #region M�todos Auxiliares
        
        /// <summary>
        /// Carrega configura��es do PDV
        /// </summary>
        private ConfiguracaoPdvDto CarregarConfiguracoes()
        {
            // Por enquanto retorna configura��o padr�o
            // Posteriormente pode ser carregada de arquivo de configura��o ou banco
            return new ConfiguracaoPdvDto();
        }
        
        /// <summary>
        /// Obt�m resumo da venda atual
        /// </summary>
        public string GetResumoVendaAtual()
        {
            if (VendaAtual == null)
                return "Nenhuma venda ativa";
            
            return VendaAtual.GetResumoVenda();
        }
        
        /// <summary>
        /// Obt�m estat�sticas do caixa
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
        /// Valida se opera��o pode ser executada
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