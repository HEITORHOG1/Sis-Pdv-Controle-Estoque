using Sis_Pdv_Controle_Estoque_Form.Dto.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Dto.PDV;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Cupom;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Dinheiro;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Login;
using Sis_Pdv_Controle_Estoque_Form.Services.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Services.PDV;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    public partial class frmTelaPdv : Form
    {
        #region Campos Privados
        
        // Services
        private PdvManager _pdvManager;
        private ProdutoService _produtoService;
        private BindingList<ItemCarrinhoDto> _itensCarrinho;
        
        // Controles de estado
        private bool _isLoading = false;
        private bool _caixaAberto = false;
        private System.Windows.Forms.Timer _timerAtualizacao;
        
        // Dados do operador
        private readonly string _nomeOperador;
        private ColaboradorResponseList _colaboradorInfo;
        private Guid _colaboradorId;
        
        // Configurações
        private ConfiguracaoPdvDto _configuracoes;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmTelaPdv(string nomeOperador)
        {
            InitializeComponent();
            _nomeOperador = nomeOperador ?? throw new ArgumentNullException(nameof(nomeOperador));
            
            InicializarComponentes();
            PdvLogger.LogInicioVenda(Guid.NewGuid(), $"PDV inicializado para operador: {nomeOperador}");
        }
        
        private void InicializarComponentes()
        {
            _pdvManager = new PdvManager();
            _produtoService = new ProdutoService();
            _itensCarrinho = new BindingList<ItemCarrinhoDto>();
            _configuracoes = new ConfiguracaoPdvDto();
            
            // Configura timer para atualizações
            _timerAtualizacao = new System.Windows.Forms.Timer();
            _timerAtualizacao.Interval = 1000; // 1 segundo
            _timerAtualizacao.Tick += Timer_Tick;
            
            // Configura DataGridView
            ConfigurarDataGridView();
        }
        
        private void ConfigurarDataGridView()
        {
            dgvCarrinho.DataSource = _itensCarrinho;
            dgvCarrinho.AllowUserToAddRows = false;
            dgvCarrinho.AllowUserToDeleteRows = false;
            dgvCarrinho.ReadOnly = true;
            dgvCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrinho.MultiSelect = false;
            
            // Event handlers
            dgvCarrinho.CellFormatting += DgvCarrinho_CellFormatting;
            dgvCarrinho.RowsAdded += DgvCarrinho_RowsAdded;
        }
        
        #endregion
        
        #region Eventos do Form
        
        private async void frmTelaPdv_Load(object sender, EventArgs e)
        {
            try
            {
                SetLoadingState(true);
                
                // Inicializa interface
                InicializarInterfaceModerna();
                
                await CarregarDadosOperador();
                await AbrirCaixa();
                IniciarNovaVenda();
                _timerAtualizacao.Start();
                
                PdvLogger.LogAberturaCaixa(_nomeOperador, lblCaixa.Text);
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CarregarFormulario", ex.Message, ex);
                MessageBox.Show($"Erro ao carregar PDV: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void InicializarInterfaceModerna()
        {
            // Configura título da janela
            lblTituloPdv.Text = $"🏪 SISTEMA PDV - {Environment.MachineName}";
            
            // Configura campos iniciais
            txbQuantidade.Text = "1";
            lblTotal.Text = 0m.FormatarMoeda();
            lblSubTotal.Text = 0m.FormatarMoeda();
            lblDesconto.Text = 0m.FormatarMoeda();
            
            // Focus no campo código de barras
            txbCodBarras.Focus();
            
            // Configura status inicial
            lblStatusOperacao.Text = "🟡 Inicializando sistema...";
            lblNomeCaixa.Text = "Carregando...";
        }
        
        private async Task CarregarDadosOperador()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                lblStatusOperacao.Text = "🔍 Carregando dados do operador...";
                
                var colaboradorService = new ColaboradorService();
                _colaboradorInfo = await colaboradorService.ListarColaboradorPorNomeColaborador(_nomeOperador);
                
                sw.Stop();
                PdvLogger.LogApiCall("ListarColaboradorPorNome", "GET", sw.Elapsed, 
                    _colaboradorInfo?.success == true);
                
                if (_colaboradorInfo?.success == true && _colaboradorInfo.data?.Any() == true)
                {
                    var colaborador = _colaboradorInfo.data.First();
                    lblNomeOperador.Text = colaborador.nomeColaborador;
                    _colaboradorId = Guid.Parse(colaborador.id);
                    
                    lblStatusOperacao.Text = $"✅ Operador: {colaborador.nomeColaborador}";
                    PdvLogger.LogAutenticacao(_nomeOperador, true);
                }
                else
                {
                    // Se não encontrou o colaborador, vamos criar um modo de demonstração
                    lblNomeOperador.Text = _nomeOperador;
                    _colaboradorId = Guid.NewGuid(); // ID temporário para demonstração
                    
                    lblStatusOperacao.Text = $"⚠️ Modo Demo - Operador: {_nomeOperador}";
                    PdvLogger.LogError($"Operador '{_nomeOperador}' não encontrado, usando modo demonstração", "Autenticacao");
                    
                    MessageBox.Show($"⚠️ Operador '{_nomeOperador}' não encontrado no sistema.\n\nContinuando em MODO DEMONSTRAÇÃO.", 
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("CarregarDadosOperador", ex.Message, ex);
                
                // Modo de fallback
                lblNomeOperador.Text = _nomeOperador;
                _colaboradorId = Guid.NewGuid();
                lblStatusOperacao.Text = "⚠️ Erro ao carregar operador - Modo Demo";
                
                MessageBox.Show($"⚠️ Erro ao carregar dados do operador.\n\nContinuando em MODO DEMONSTRAÇÃO.\n\nErro: {ex.Message}", 
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private async Task AbrirCaixa()
        {
            try
            {
                lblStatusOperacao.Text = "🏪 Abrindo caixa...";
                
                var caixa = $"CAIXA-{Environment.MachineName}";
                await _pdvManager.AbrirCaixa(_nomeOperador, caixa);
                
                _caixaAberto = true;
                lblCaixa.Text = caixa;
                lblStatusCaixa.Text = "🟢 CAIXA ABERTO";
                lblStatusCaixa.ForeColor = Color.LightGreen;
                
                lblStatusOperacao.Text = "✅ Caixa aberto - Sistema pronto";
                AtualizarStatusInterface();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AbrirCaixa", ex.Message, ex);
                lblStatusOperacao.Text = "❌ Erro ao abrir caixa";
                throw;
            }
        }
        
        private void IniciarNovaVenda()
        {
            try
            {
                var venda = _pdvManager.IniciarNovaVenda();
                venda.ColaboradorId = _colaboradorId;
                venda.NomeOperador = _nomeOperador;
                
                LimparInterface();
                AtualizarStatusInterface();
                
                lblStatusOperacao.Text = "🛒 Nova venda iniciada";
                lblNomeCaixa.Text = "CAIXA LIVRE - Aguardando produtos";
                
                txbCodBarras.Focus();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("IniciarNovaVenda", ex.Message, ex);
                MessageBox.Show($"Erro ao iniciar nova venda: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion
        
        #region Eventos de Entrada de Produtos
        
        private async void txbCodBarras_TextChanged(object sender, EventArgs e)
        {
            // Só processa se o texto não estiver vazio e tiver pelo menos 8 caracteres
            if (string.IsNullOrWhiteSpace(txbCodBarras.Text) || txbCodBarras.Text.Length < 8)
                return;
                
            await ProcessarCodigoBarras(txbCodBarras.Text.Trim());
        }
        
        private async void txbCodBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            
            // Se pressionou Enter, processa o código
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await ProcessarCodigoBarras(txbCodBarras.Text.Trim());
            }
        }
        
        private async Task ProcessarCodigoBarras(string codigoBarras)
        {
            if (_isLoading || string.IsNullOrWhiteSpace(codigoBarras)) 
                return;
                
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                var produto = await _pdvManager.BuscarProdutoPorCodigo(codigoBarras);
                
                sw.Stop();
                PdvLogger.LogPerformance("BuscarProduto", sw.Elapsed);
                
                if (produto == null)
                {
                    MessageBox.Show($"Produto não encontrado: {codigoBarras}", "Produto Não Encontrado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Preenche dados do produto
                PreencherDadosProduto(produto);
                
                // Verifica alertas
                var alertas = produto.GetAlertasVenda();
                if (alertas.Any())
                {
                    var mensagem = $"ATENÇÃO:\n• {string.Join("\n• ", alertas)}\n\nDeseja continuar?";
                    var resultado = MessageBox.Show(mensagem, "Alertas do Produto", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (resultado == DialogResult.No)
                    {
                        LimparCamposProduto();
                        return;
                    }
                }
                
                // Adiciona ao carrinho
                await AdicionarItemCarrinho(produto);
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("ProcessarCodigoBarras", ex.Message, ex);
                MessageBox.Show($"Erro ao processar produto: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void PreencherDadosProduto(Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data produto)
        {
            txbDescricao.Text = produto.nomeProduto.TruncateForDisplay(50);
            txbPrecoUnit.Text = produto.precoVenda.FormatarMoeda();
            
            // Calcula total (quantidade padrão = 1)
            var quantidade = 1;
            var total = produto.precoVenda * quantidade;
            txbTotalRecebido.Text = total.FormatarMoeda();
            
            // Mostra informações de estoque
            if (produto.quatidadeEstoqueProduto <= 10)
            {
                txbDescricao.BackColor = Color.LightYellow;
                txbDescricao.Text += $" (Est: {produto.quatidadeEstoqueProduto})";
            }
            else
            {
                txbDescricao.BackColor = SystemColors.Window;
            }
        }
        
        private async Task AdicionarItemCarrinho(Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data produto)
        {
            try
            {
                var quantidade = GetQuantidadeSelecionada();
                await _pdvManager.AdicionarItem(produto.codBarras, quantidade);
                
                AtualizarCarrinho();
                AtualizarTotais();
                LimparCamposProduto();
                
                // Som de sucesso (opcional)
                System.Media.SystemSounds.Beep.Play();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AdicionarItemCarrinho", ex.Message, ex);
                MessageBox.Show($"Erro ao adicionar item: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private int GetQuantidadeSelecionada()
        {
            if (int.TryParse(txbQuantidade.Text, out int quantidade) && quantidade > 0)
                return quantidade;
            return 1;
        }
        
        #endregion
        
        #region Atualização da Interface
        
        private void AtualizarCarrinho()
        {
            try
            {
                _itensCarrinho.Clear();
                
                if (_pdvManager.VendaAtual?.Itens != null)
                {
                    foreach (var item in _pdvManager.VendaAtual.Itens)
                    {
                        _itensCarrinho.Add(item);
                    }
                }
                
                // Formatar colunas
                FormatarColunasGrid();
                AplicarCoresGrid();
                
                // Atualiza contador e valor total no título do GroupBox
                var totalItens = _itensCarrinho.Count(i => !i.Cancelado);
                var valorTotal = _itensCarrinho.Where(i => !i.Cancelado).Sum(i => i.Total);
                
                gbCarrinho.Text = $"🛒 CARRINHO DE COMPRAS ({totalItens} itens) - Total: {valorTotal:C2}";
                
                // Auto-scroll para o último item adicionado
                if (dgvCarrinho.Rows.Count > 0)
                {
                    dgvCarrinho.FirstDisplayedScrollingRowIndex = dgvCarrinho.Rows.Count - 1;
                }
                
                // Destaca total se houver itens
                if (totalItens > 0)
                {
                    gbCarrinho.ForeColor = Color.FromArgb(39, 174, 96); // Verde
                }
                else
                {
                    gbCarrinho.ForeColor = Color.FromArgb(52, 73, 94); // Padrão
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AtualizarCarrinho", ex.Message, ex);
            }
        }
        
        private void FormatarColunasGrid()
        {
            if (dgvCarrinho.Columns.Count == 0) return;
            
            try
            {
                // ✅ CARRINHO SIMPLIFICADO - APENAS COLUNAS ESSENCIAIS
                
                // 1. OCULTA TODAS as colunas que não são essenciais
                string[] colunasOcultar = {
                    "ProdutoId", "EstoqueDisponivel", "DataVencimento", 
                    "Cancelado", "CodigoBarras"
                };
                
                foreach (var coluna in colunasOcultar)
                {
                    if (dgvCarrinho.Columns[coluna] != null)
                        dgvCarrinho.Columns[coluna].Visible = false;
                }
                
                // 2. CONFIGURA as colunas essenciais visíveis
                var colunasEssenciais = new Dictionary<string, (string Header, int Width, string Format)>
                {
                    ["Codigo"] = ("Item", 60, ""),
                    ["Descricao"] = ("📦 Produto", 350, ""),
                    ["Quantidade"] = ("Qtd", 60, ""),
                    ["PrecoUnitario"] = ("Valor Unit.", 100, "C2"),
                    ["Total"] = ("💰 Total", 120, "C2")
                };
                
                foreach (var config in colunasEssenciais)
                {
                    var coluna = dgvCarrinho.Columns[config.Key];
                    if (coluna != null)
                    {
                        coluna.HeaderText = config.Value.Header;
                        coluna.Width = config.Value.Width;
                        coluna.Visible = true;
                        
                        if (!string.IsNullOrEmpty(config.Value.Format))
                        {
                            coluna.DefaultCellStyle.Format = config.Value.Format;
                        }
                        
                        // Alinhamento específico por coluna
                        switch (config.Key)
                        {
                            case "Codigo":
                            case "Quantidade":
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                break;
                            case "PrecoUnitario":
                            case "Total":
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                break;
                            case "Descricao":
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                break;
                        }
                    }
                }
                
                // 3. CONFIGURA AutoSizeMode para responsividade
                if (dgvCarrinho.Columns["Descricao"] != null)
                {
                    dgvCarrinho.Columns["Descricao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvCarrinho.Columns["Descricao"].MinimumWidth = 200;
                }
                
                // 4. ESTILO para melhor visualização
                dgvCarrinho.RowTemplate.Height = 35;
                dgvCarrinho.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
                dgvCarrinho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("FormatarColunasGrid", ex.Message, ex);
            }
        }
        
        private void AplicarCoresGrid()
        {
            try
            {
                foreach (DataGridViewRow row in dgvCarrinho.Rows)
                {
                    if (row.DataBoundItem is ItemCarrinhoDto item)
                    {
                        if (item.Cancelado)
                        {
                            // ❌ Item cancelado - vermelho com strikethrough
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                            row.DefaultCellStyle.Font = new Font(row.DefaultCellStyle.Font, FontStyle.Strikeout);
                            
                            // Adiciona ícone de cancelado na descrição
                            if (row.Cells["Descricao"] != null)
                            {
                                var descricao = item.Descricao;
                                if (!descricao.StartsWith("❌"))
                                    row.Cells["Descricao"].Value = $"❌ {descricao}";
                            }
                        }
                        else
                        {
                            var alertas = item.Validar();
                            
                            if (alertas.Any(a => a.Contains("vencido")))
                            {
                                // 🔴 Produto vencido - não pode ser vendido
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 205);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                            }
                            else if (alertas.Any(a => a.Contains("estoque")))
                            {
                                // 🟡 Estoque baixo - atenção
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                            }
                            else if (alertas.Any(a => a.Contains("vence")))
                            {
                                // 🟠 Próximo ao vencimento
                                row.DefaultCellStyle.BackColor = Color.FromArgb(254, 240, 220);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(157, 88, 36);
                            }
                            else
                            {
                                // ✅ Item normal - alternando cores para melhor leitura
                                if (row.Index % 2 == 0)
                                {
                                    row.DefaultCellStyle.BackColor = Color.White;
                                }
                                else
                                {
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                                }
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
                            }
                        }
                        
                        // Destaca valores totais com fonte em negrito
                        if (row.Cells["Total"] != null)
                        {
                            row.Cells["Total"].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                            row.Cells["Total"].Style.ForeColor = Color.FromArgb(39, 174, 96); // Verde para valores
                        }
                        
                        // Destaca código do item
                        if (row.Cells["Codigo"] != null)
                        {
                            row.Cells["Codigo"].Style.Font = new Font("Consolas", 10F, FontStyle.Bold);
                            row.Cells["Codigo"].Style.ForeColor = Color.FromArgb(52, 152, 219); // Azul para códigos
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AplicarCoresGrid", ex.Message, ex);
            }
        }
        
        private void AtualizarTotais()
        {
            try
            {
                if (_pdvManager.VendaAtual != null)
                {
                    var subtotal = _pdvManager.VendaAtual.ValorTotal;
                    var desconto = _pdvManager.VendaAtual.ValorDesconto;
                    var total = _pdvManager.VendaAtual.ValorFinal;
                    
                    lblSubTotal.Text = subtotal.FormatarMoeda();
                    lblDesconto.Text = desconto.FormatarMoeda();
                    lblTotal.Text = total.FormatarMoeda();
                    
                    // Atualiza contador de itens
                    var resumo = _pdvManager.VendaAtual.GetResumoItens();
                    lblNomeCaixa.Text = $"PROCESSANDO VENDA - {resumo}";
                    
                    // Atualiza cor do total baseado no valor
                    if (total > 0)
                    {
                        lblTotal.ForeColor = Color.Yellow;
                        lblStatusCaixa.Text = "🟡 VENDA EM ANDAMENTO";
                        lblStatusCaixa.ForeColor = Color.Yellow;
                    }
                }
                else
                {
                    lblSubTotal.Text = 0m.FormatarMoeda();
                    lblDesconto.Text = 0m.FormatarMoeda();
                    lblTotal.Text = 0m.FormatarMoeda();
                    lblNomeCaixa.Text = "CAIXA LIVRE - Aguardando produtos";
                    
                    lblStatusCaixa.Text = "🟢 CAIXA ABERTO";
                    lblStatusCaixa.ForeColor = Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AtualizarTotais", ex.Message, ex);
            }
        }
        
        private void AtualizarStatusInterface()
        {
            try
            {
                // Atualiza botões baseado no estado
                var temItens = _pdvManager.VendaAtual?.QuantidadeItensAtivos > 0;
                
                // Habilita/desabilita operações
                var podeOperar = _caixaAberto && _pdvManager.ValidarOperacao("ADICIONAR_ITEM");
                
                txbCodBarras.Enabled = podeOperar;
                txbQuantidade.Enabled = podeOperar;
                
                // Habilita botões de pagamento apenas se houver itens
                btnPagamentoDinheiro.Enabled = temItens;
                btnPagamentoCartao.Enabled = temItens;
                btnPagamentoPix.Enabled = temItens;
                btnFinalizarVenda.Enabled = temItens && !string.IsNullOrEmpty(lblFormaPagamento.Text);
                btnCancelarItem.Enabled = temItens;
                btnCancelarVenda.Enabled = temItens;
                
                // Atualiza título da janela
                var stats = _pdvManager.GetEstatisticasCaixa();
                Text = $"PDV Moderno - {stats["Operador"]} - {stats["Caixa"]}";
                
                // Atualiza status de operação
                if (temItens)
                {
                    lblStatusOperacao.Text = $"🛒 Venda em andamento - {_pdvManager.VendaAtual.QuantidadeItensAtivos} itens";
                }
                else
                {
                    lblStatusOperacao.Text = "🟢 Sistema PDV - Pronto para nova venda";
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AtualizarStatusInterface", ex.Message, ex);
            }
        }
        
        #endregion
        
        #region Eventos de Timers
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Atualiza data e hora
                lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
                
                // Atualiza status periodicamente
                AtualizarStatusInterface();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("TimerTick", ex.Message, ex);
            }
        }
        
        #endregion
        
        #region Event Handlers do DataGridView
        
        private void DgvCarrinho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                
                var item = _itensCarrinho[e.RowIndex];
                
                // Formata status
                if (dgvCarrinho.Columns[e.ColumnIndex].Name == "Status")
                {
                    e.Value = item.GetStatusDisplay();
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("FormatarCelula", ex.Message, ex);
            }
        }
        
        private void DgvCarrinho_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            AplicarCoresGrid();
        }
        
        #endregion
        
        #region Métodos de Limpeza
        
        private void LimparInterface()
        {
            _itensCarrinho.Clear();
            LimparCamposProduto();
            lblSubTotal.Text = 0m.FormatarMoeda();
            lblDesconto.Text = 0m.FormatarMoeda();
            lblTotal.Text = 0m.FormatarMoeda();
            lblNomeCaixa.Text = "CAIXA LIVRE - Aguardando produtos";
            
            // Limpa campos de pagamento
            LimparCamposPagamento();
            
            // Reset cores dos botões
            btnFinalizarVenda.BackColor = Color.FromArgb(39, 174, 96);
            btnFinalizarVenda.Enabled = false;
            
            // Atualiza GroupBox do carrinho
            gbCarrinho.Text = "🛒 CARRINHO DE COMPRAS (0 itens)";
        }
        
        private void LimparCamposProduto()
        {
            txbCodBarras.Clear();
            txbDescricao.Clear();
            txbPrecoUnit.Clear();
            txbQuantidade.Text = "1";
            txbTotalRecebido.Clear();
            txbDescricao.BackColor = SystemColors.Window;
            
            txbCodBarras.Focus();
        }
        
        private void LimparCamposPagamento()
        {
            lblFormaPagamento.Text = "---";
            lblValorAReceber.Text = 0m.FormatarMoeda();
            lblTroco.Text = 0m.FormatarMoeda();
            
            // Oculta labels de pagamento
            lblFormaPagamentoLabel.Visible = false;
            lblFormaPagamento.Visible = false;
            lblValorRecebidoLabel.Visible = false;
            lblValorAReceber.Visible = false;
            lblTrocoLabel.Visible = false;
            lblTroco.Visible = false;
        }
        
        #endregion
        
        #region Controle de Estado
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            
            if (loading)
            {
                Cursor = Cursors.WaitCursor;
                lblStatusOperacao.Text = "⏳ Processando...";
                lblStatusOperacao.ForeColor = Color.Orange;
                progressOperacao.Visible = true;
                progressOperacao.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                Cursor = Cursors.Default;
                progressOperacao.Visible = false;
                lblStatusOperacao.ForeColor = Color.White;
                if (_caixaAberto)
                {
                    lblStatusOperacao.Text = "🟢 Sistema PDV - Pronto";
                }
            }
            
            // Desabilita controles durante carregamento
            txbCodBarras.Enabled = !loading && _caixaAberto;
            txbQuantidade.Enabled = !loading && _caixaAberto;
            dgvCarrinho.Enabled = !loading;
            
            // Desabilita botões durante loading
            btnPagamentoDinheiro.Enabled = !loading;
            btnPagamentoCartao.Enabled = !loading;
            btnPagamentoPix.Enabled = !loading;
            btnFinalizarVenda.Enabled = !loading;
            btnCancelarItem.Enabled = !loading;
            btnCancelarVenda.Enabled = !loading;
        }
        
        #endregion
        
        #region Operações de Pagamento
        
        private void PagamentoDinheiro()
        {
            try
            {
                if (!ValidarVendaParaPagamento()) return;
                
                using var frmDinheiro = new frmDinheiro();
                frmDinheiro.ReceberValor(lblTotal.Text);
                
                if (frmDinheiro.ShowDialog() == DialogResult.OK)
                {
                    var valorRecebido = decimal.Parse(frmDinheiro.ValorRecibido.Replace("R$", "").Replace(",", "."));
                    var troco = decimal.Parse(frmDinheiro.Troco.Replace("R$", "").Replace(",", "."));
                    
                    ProcessarPagamento("DINHEIRO", valorRecebido, troco);
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("PagamentoDinheiro", ex.Message, ex);
                MessageBox.Show($"Erro no pagamento em dinheiro: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PagamentoCartao()
        {
            try
            {
                if (!ValidarVendaParaPagamento()) return;
                
                var valorTotal = _pdvManager.VendaAtual.ValorTotal;
                ProcessarPagamento("CARTÃO", valorTotal, 0);
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("PagamentoCartao", ex.Message, ex);
                MessageBox.Show($"Erro no pagamento com cartão: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PagamentoPix()
        {
            try
            {
                if (!ValidarVendaParaPagamento()) return;
                
                var valorTotal = _pdvManager.VendaAtual.ValorTotal;
                
                var resultado = MessageBox.Show(
                    $"💳 PAGAMENTO PIX\n\n" +
                    $"Valor Total: {valorTotal:C2}\n\n" +
                    $"Confirme o pagamento PIX no dispositivo do cliente.\n\n" +
                    $"Pagamento foi aprovado?",
                    "Pagamento PIX",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (resultado == DialogResult.Yes)
                {
                    ProcessarPagamento("PIX", valorTotal, 0);
                    lblStatusOperacao.Text = "📱 Pagamento PIX processado";
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("PagamentoPix", ex.Message, ex);
                MessageBox.Show($"Erro no pagamento PIX: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ProcessarPagamento(string formaPagamento, decimal valorRecebido, decimal troco = 0)
        {
            try
            {
                _pdvManager.DefinirFormaPagamento(formaPagamento, valorRecebido);
                
                // Atualiza interface com novos componentes
                lblFormaPagamento.Text = formaPagamento;
                lblValorAReceber.Text = valorRecebido.FormatarMoeda();
                lblTroco.Text = troco.FormatarMoeda();
                
                // Mostra campos de pagamento
                ExibirCamposPagamento();
                
                // Atualiza status
                lblStatusOperacao.Text = $"💳 Forma de pagamento: {formaPagamento}";
                lblStatusCaixa.Text = "🟡 AGUARDANDO FINALIZAÇÃO";
                lblStatusCaixa.ForeColor = Color.Yellow;
                
                // Habilita botão finalizar
                btnFinalizarVenda.Enabled = true;
                btnFinalizarVenda.BackColor = Color.FromArgb(39, 174, 96);
                
                PdvLogger.LogFormaPagamento(formaPagamento, valorRecebido, $"Troco: {troco:C2}");
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("ProcessarPagamento", ex.Message, ex);
                throw;
            }
        }
        
        private void ExibirCamposPagamento()
        {
            lblFormaPagamentoLabel.Visible = true;
            lblFormaPagamento.Visible = true;
            lblValorRecebidoLabel.Visible = true;
            lblValorAReceber.Visible = true;
            lblTrocoLabel.Visible = true;
            lblTroco.Visible = true;
        }
        
        private bool ValidarVendaParaPagamento()
        {
            if (_pdvManager.VendaAtual == null || _pdvManager.VendaAtual.QuantidadeItensAtivos == 0)
            {
                MessageBox.Show("Adicione itens à venda antes de definir a forma de pagamento!", 
                    "Venda Vazia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            return true;
        }
        
        #endregion
        
        #region Finalização e Cancelamento de Vendas
        
        private async void FinalizarVendas()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarFinalizacaoVenda()) return;
                
                SetLoadingState(true);
                
                // Solicita CPF do cliente se configurado
                var cpfCliente = "";
                if (_configuracoes.SolicitarCpfCliente)
                {
                    cpfCliente = SolicitarCpfCliente();
                }
                
                // Finaliza venda
                var pedidoId = await _pdvManager.FinalizarVenda(cpfCliente);
                
                sw.Stop();
                PdvLogger.LogPerformance("FinalizarVenda", sw.Elapsed);
                
                // Imprime cupom
                if (_configuracoes.ImprimirCupomAutomatico)
                {
                    await ImprimirCupomFiscal(pedidoId);
                }
                
                // Mostra mensagem de sucesso
                var venda = _pdvManager.VendaAtual;
                var mensagem = $"Venda finalizada com sucesso!\n\n" +
                              $"Pedido: {pedidoId}\n" +
                              $"Total: {venda.ValorTotal:C2}\n" +
                              $"Forma: {venda.FormaPagamento}\n" +
                              $"Troco: {venda.Troco:C2}";
                
                MessageBox.Show(mensagem, "Venda Finalizada", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Inicia nova venda
                IniciarNovaVenda();
                LimparCamposPagamento();
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("FinalizarVendas", ex.Message, ex);
                MessageBox.Show($"Erro ao finalizar venda: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarFinalizacaoVenda()
        {
            if (_pdvManager.VendaAtual == null)
            {
                MessageBox.Show("Não há venda ativa!", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (string.IsNullOrEmpty(lblFormaPagamento.Text))
            {
                MessageBox.Show("Defina a forma de pagamento antes de finalizar!", 
                    "Forma de Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (_pdvManager.VendaAtual.QuantidadeItensAtivos == 0)
            {
                MessageBox.Show("Adicione itens à venda!", "Venda Vazia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            var erros = _pdvManager.VendaAtual.Validar();
            if (erros.Any())
            {
                MessageBox.Show($"Venda inválida:\n• {string.Join("\n• ", erros)}", 
                    "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            return true;
        }
        
        private string SolicitarCpfCliente()
        {
            // Implementar dialog para CPF do cliente
            // Por enquanto retorna vazio
            return "";
        }
        
        private void CancelarVenda()
        {
            try
            {
                if (_pdvManager.VendaAtual == null || _pdvManager.VendaAtual.QuantidadeItensAtivos == 0)
                {
                    MessageBox.Show("Não há venda para cancelar!", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var resultado = MessageBox.Show(
                    "Deseja realmente cancelar esta venda?\n\nTodos os itens serão removidos.",
                    "Cancelar Venda", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                
                if (resultado == DialogResult.Yes)
                {
                    var motivo = "Cancelamento pelo operador";
                    if (_configuracoes.ExigirAutorizacaoCancelamento)
                    {
                        motivo = SolicitarMotivoAutorizacao();
                        if (string.IsNullOrEmpty(motivo))
                            return; // Cancelamento foi cancelado
                    }
                    
                    _pdvManager.CancelarVenda(motivo);
                    IniciarNovaVenda();
                    
                    MessageBox.Show("Venda cancelada com sucesso!", "Cancelamento", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CancelarVenda", ex.Message, ex);
                MessageBox.Show($"Erro ao cancelar venda: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private string SolicitarMotivoAutorizacao()
        {
            // Implementar dialog para autorização de cancelamento
            // Por enquanto usa autorização simples
            using var frmVerificaLogin = new frmVerificaLogin();
            if (frmVerificaLogin.ShowDialog() == DialogResult.OK)
            {
                return "Cancelamento autorizado";
            }
            return "";
        }
        
        #endregion
        
        #region Operações de Itens
        
        private void CancelarItem()
        {
            try
            {
                if (_pdvManager.VendaAtual?.QuantidadeItensAtivos == 0)
                {
                    MessageBox.Show("Não há itens para cancelar!", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var motivo = "";
                if (_configuracoes.ExigirAutorizacaoCancelamento)
                {
                    motivo = SolicitarMotivoAutorizacao();
                    if (string.IsNullOrEmpty(motivo))
                        return;
                }
                
                // Solicita qual item cancelar
                var codigo = SolicitarCodigoItem();
                if (codigo > 0)
                {
                    _pdvManager.CancelarItem(codigo, motivo);
                    AtualizarCarrinho();
                    AtualizarTotais();
                    
                    MessageBox.Show($"Item {codigo} cancelado com sucesso!", "Item Cancelado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("CancelarItem", ex.Message, ex);
                MessageBox.Show($"Erro ao cancelar item: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private int SolicitarCodigoItem()
        {
            // Implementar dialog para selecionar item
            // Por enquanto usa o primeiro item ativo
            var primeiroItem = _pdvManager.VendaAtual?.Itens.FirstOrDefault(i => !i.Cancelado);
            return primeiroItem?.Codigo ?? 0;
        }
        
        #endregion
        
        #region Impressão de Cupom
        
        private async Task ImprimirCupomFiscal(Guid pedidoId)
        {
            try
            {
                PdvLogger.LogImpressaoCupom(pedidoId, "FISCAL", false, "Iniciando impressão");
                
                using var frmCupom = new frmCupom(pedidoId.ToString(), _nomeOperador);
                
                var venda = _pdvManager.VendaAtual;
                
                // Gera cupom para cada item
                foreach (var item in venda.Itens)
                {
                    var status = item.Cancelado ? "Cancelado" : "Ativo";
                    
                    frmCupom.CumpomImpresso(
                        item.Codigo.ToString(),
                        item.CodigoBarras,
                        item.Descricao,
                        item.Quantidade.ToString(),
                        item.PrecoUnitario.ToString("F2"),
                        item.Total.ToString("F2"),
                        status,
                        venda.CpfCnpjCliente,
                        venda.ValorTotal.ToString("F2"),
                        venda.DataVenda.ToString("dd/MM/yyyy"),
                        venda.DataVenda.ToString("HH:mm:ss"),
                        lblCaixa.Text,
                        venda.FormaPagamento,
                        venda.ValorRecebido.ToString("F2"),
                        venda.Troco.ToString("F2")
                    );
                }
                
                frmCupom.ShowDialog();
                
                PdvLogger.LogImpressaoCupom(pedidoId, "FISCAL", true);
            }
            catch (Exception ex)
            {
                PdvLogger.LogImpressaoCupom(pedidoId, "FISCAL", false, ex.Message);
                MessageBox.Show($"Erro ao imprimir cupom: {ex.Message}", "Erro de Impressão",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion
        
        #region Atalhos de Teclado
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case Keys.F2:
                        FinalizarVendas();
                        return true;
                        
                    case Keys.D:
                        PagamentoDinheiro();
                        return true;
                        
                    case Keys.C:
                        PagamentoCartao();
                        return true;
                        
                    case Keys.I:
                        CancelarItem();
                        return true;
                        
                    case Keys.F8:
                        CancelarVenda();
                        return true;
                        
                    case Keys.F5:
                        LimparCamposProduto();
                        return true;
                        
                    case Keys.Escape:
                        if (_pdvManager.VendaAtual?.QuantidadeItensAtivos > 0)
                        {
                            var result = MessageBox.Show("Há uma venda em andamento. Deseja realmente sair?", 
                                "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                                return true;
                        }
                        Close();
                        return true;
                        
                    case Keys.F1:
                        MostrarAjuda();
                        return true;
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("ProcessCmdKey", ex.Message, ex);
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "ATALHOS DO PDV:\n\n" +
                       "F1 - Esta ajuda\n" +
                       "F2 - Finalizar venda\n" +
                       "F5 - Limpar campos\n" +
                       "F8 - Cancelar venda\n" +
                       "C - Pagamento cartão\n" +
                       "D - Pagamento dinheiro\n" +
                       "I - Cancelar item\n" +
                       "ESC - Sair";
            
            MessageBox.Show(ajuda, "Ajuda - Atalhos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void txbQuantidade_TextChanged(object sender, EventArgs e)
        {
            // Atualiza cálculo quando quantidade muda
            AtualizarCalculoQuantidade();
        }
        
        private void txbQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        
        private void AtualizarCalculoQuantidade()
        {
            try
            {
                if (decimal.TryParse(txbPrecoUnit.Text.Replace("R$", "").Replace(",", "."), out decimal preco) &&
                    int.TryParse(txbQuantidade.Text, out int quantidade) && quantidade > 0)
                {
                    var total = preco * quantidade;
                    txbTotalRecebido.Text = total.FormatarMoeda();
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AtualizarCalculoQuantidade", ex.Message, ex);
            }
        }
        
        #endregion
        
        #region Fechamento do Form
        
        private async void frmTelaPdv_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_pdvManager.VendaAtual?.QuantidadeItensAtivos > 0)
                {
                    var result = MessageBox.Show(
                        "Há uma venda em andamento. Deseja realmente sair?\n\nA venda será perdida.",
                        "Confirmação de Saída", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                
                // Para o timer
                _timerAtualizacao?.Stop();
                _timerAtualizacao?.Dispose();
                
                // Fecha o caixa se estiver aberto
                if (_caixaAberto)
                {
                    var totalVendas = 0; // Implementar contagem real
                    var valorFinal = 0m; // Implementar cálculo real
                    await _pdvManager.FecharCaixa(valorFinal, totalVendas);
                }
                
                PdvLogger.LogFechamentoCaixa(_nomeOperador, lblCaixa.Text, 0, 0);
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("FormClosing", ex.Message, ex);
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares (Compatibilidade)
        
        // Métodos mantidos para compatibilidade com código existente
        
        private async Task CalculoValorQauntidade()
        {
            AtualizarCalculoQuantidade();
        }
        
        private async Task CalculaTotal()
        {
            AtualizarTotais();
        }
        
        private async Task LimpaCampos()
        {
            LimparCamposProduto();
        }
        
        private void LimpaDgv()
        {
            LimparInterface();
        }
        
        private void Adicionar(string codigoBarras, string descricao)
        {
            // Método legado - nova implementação usa AdicionarItemCarrinho
        }
        
        private bool VerificaVazio()
        {
            return string.IsNullOrWhiteSpace(txbCodBarras.Text) ||
                   string.IsNullOrWhiteSpace(txbDescricao.Text) ||
                   string.IsNullOrWhiteSpace(txbPrecoUnit.Text) ||
                   string.IsNullOrWhiteSpace(txbQuantidade.Text);
        }
        
        #endregion
        
        #region Eventos Legados (Compatibilidade)
        
        private void timerData_Tick(object sender, EventArgs e)
        {
            Timer_Tick(sender, e);
        }
        
        #endregion
        
        #region Event Handlers dos Botões Modernos
        
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        
        private void btnPagamentoDinheiro_Click(object sender, EventArgs e)
        {
            PagamentoDinheiro();
        }
        
        private void btnPagamentoCartao_Click(object sender, EventArgs e)
        {
            PagamentoCartao();
        }
        
        private void btnPagamentoPix_Click(object sender, EventArgs e)
        {
            PagamentoPix();
        }
        
        private void btnCancelarItem_Click(object sender, EventArgs e)
        {
            CancelarItem();
        }
        
        private void btnCancelarVenda_Click(object sender, EventArgs e)
        {
            CancelarVenda();
        }
        
        private void btnFinalizarVenda_Click(object sender, EventArgs e)
        {
            FinalizarVendas();
        }
        
        private void btnLimparCampos_Click(object sender, EventArgs e)
        {
            LimparCamposProduto();
        }
        
        private void btnAjuda_Click(object sender, EventArgs e)
        {
            MostrarAjuda();
        }
        
        #endregion
    }
}
