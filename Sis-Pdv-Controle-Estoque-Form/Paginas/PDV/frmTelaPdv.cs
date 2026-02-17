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
        
        // Debounce para entrada de codigo de barras (scanner vs digitacao manual)
        private System.Windows.Forms.Timer _timerDebounceBarcode;
        private DateTime _lastKeystroke = DateTime.MinValue;
        private int _keystrokeCount = 0;
        private const int SCANNER_DEBOUNCE_MS = 150;  // Scanner envia tudo em < 100ms
        private const int SCANNER_MIN_CHARS = 8;       // Codigo de barras minimo
        
        // Dados do operador
        private readonly string _nomeOperador;
        private ColaboradorResponseList _colaboradorInfo;
        private Guid _colaboradorId;
        
        // Configura��es
        private ConfiguracaoPdvDto _configuracoes;
        
        #endregion
        
        #region Construtor e Inicializa��o
        
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
            
            // Configura timer para atualiza��es
            _timerAtualizacao = new System.Windows.Forms.Timer();
            _timerAtualizacao.Interval = 1000; // 1 segundo
            _timerAtualizacao.Tick += Timer_Tick;
            
            // Configura DataGridView
            ConfigurarDataGridView();
        }
        
        private void ConfigurarDataGridView()
        {
            // Desabilita AutoGenerate para controlar as colunas manualmente
            dgvCarrinho.AutoGenerateColumns = false;
            
            dgvCarrinho.AllowUserToAddRows = false;
            dgvCarrinho.AllowUserToDeleteRows = false;
            dgvCarrinho.ReadOnly = true;
            dgvCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrinho.MultiSelect = false;
            
            // Event handlers
            dgvCarrinho.CellFormatting += DgvCarrinho_CellFormatting;
            dgvCarrinho.RowsAdded += DgvCarrinho_RowsAdded;
        }
        
        /// <summary>
        /// Configura as colunas do grid antes de receber dados,
        /// mostrando headers corretos desde o inicio.
        /// </summary>
        private void ConfigurarColunasGridInicial()
        {
            dgvCarrinho.Columns.Clear();
            
            // Coluna Item (numero sequencial)
            dgvCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Codigo",
                HeaderText = "Item",
                DataPropertyName = "Codigo",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Consolas", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 152, 219)
                }
            });
            
            // Coluna Descricao do Produto (preenche espaco)
            dgvCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descricao",
                HeaderText = "Produto",
                DataPropertyName = "Descricao",
                MinimumWidth = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });
            
            // Coluna Quantidade
            dgvCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantidade",
                HeaderText = "Qtd",
                DataPropertyName = "Quantidade",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                }
            });
            
            // Coluna Preco Unitario
            dgvCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecoUnitario",
                HeaderText = "Valor Unit.",
                DataPropertyName = "PrecoUnitario",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "C2"
                }
            });
            
            // Coluna Total
            dgvCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "C2",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(39, 174, 96)
                }
            });
            
            // Estilo geral das linhas
            dgvCarrinho.RowTemplate.Height = 40;
            dgvCarrinho.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCarrinho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            
            // Vincula os dados
            dgvCarrinho.DataSource = _itensCarrinho;
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
            // Configura titulo da janela
            lblTituloPdv.Text = $"SISTEMA PDV - {Environment.MachineName}";
            
            // Configura campos iniciais
            txbQuantidade.Text = "1";
            lblTotal.Text = 0m.FormatarMoeda();
            lblSubTotal.Text = 0m.FormatarMoeda();
            lblDesconto.Text = 0m.FormatarMoeda();
            
            // Focus no campo codigo de barras
            txbCodBarras.Focus();
            
            // Configura status inicial
            lblStatusOperacao.Text = "Inicializando sistema...";
            lblNomeCaixa.Text = "Carregando...";
            
            // Configura colunas do grid ANTES de receber dados
            ConfigurarColunasGridInicial();
        }
        
        private async Task CarregarDadosOperador()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                lblStatusOperacao.Text = "Carregando dados do operador...";
                
                var colaboradorService = new ColaboradorService();
                _colaboradorInfo = await colaboradorService.ListarColaboradorPorNomeColaborador(_nomeOperador);
                
                sw.Stop();
                PdvLogger.LogApiCall("ListarColaboradorPorNome", "GET", sw.Elapsed, 
                    _colaboradorInfo?.success == true);
                
                if (_colaboradorInfo?.success == true && _colaboradorInfo.data?.Any() == true)
                {
                    var colaborador = _colaboradorInfo.data.First();
                    lblNomeOperador.Text = colaborador.NomeColaborador;
                    _colaboradorId = Guid.Parse(colaborador.id);
                    
                    lblStatusOperacao.Text = $"Operador: {colaborador.NomeColaborador}";
                    PdvLogger.LogAutenticacao(_nomeOperador, true);
                }
                else
                {
                    // Colaborador nao encontrado por nome — tenta buscar qualquer colaborador ativo como fallback
                    PdvLogger.LogError($"Operador '{_nomeOperador}' nao encontrado por nome, buscando fallback", "Autenticacao");
                    
                    var todosResp = await colaboradorService.ListarColaborador();
                    if (todosResp?.success == true && todosResp.data?.Any() == true)
                    {
                        var primeiro = todosResp.data.First();
                        lblNomeOperador.Text = _nomeOperador;
                        _colaboradorId = Guid.Parse(primeiro.id);
                        
                        lblStatusOperacao.Text = $"Operador: {_nomeOperador} (ID: {primeiro.NomeColaborador})";
                        PdvLogger.LogAutenticacao(_nomeOperador, true, $"Fallback para colaborador {primeiro.NomeColaborador}");
                    }
                    else
                    {
                        // Nenhum colaborador no sistema — operacao impossivel
                        lblNomeOperador.Text = _nomeOperador;
                        _colaboradorId = Guid.Empty;
                        lblStatusOperacao.Text = "ERRO: Nenhum colaborador cadastrado";
                        
                        MessageBox.Show($"Nenhum colaborador cadastrado no sistema.\n\nCadastre colaboradores antes de operar o PDV.",
                            "Erro Critico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("CarregarDadosOperador", ex.Message, ex);
                
                // Erro de conexao — bloqueia operacao (nao usa Guid aleatorio)
                lblNomeOperador.Text = _nomeOperador;
                _colaboradorId = Guid.Empty;
                lblStatusOperacao.Text = "Erro ao carregar operador";
                
                MessageBox.Show($"Erro ao carregar dados do operador.\n\nVerifique a conexao com a API.\n\nErro: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private async Task AbrirCaixa()
        {
            try
            {
                lblStatusOperacao.Text = "Abrindo caixa...";
                
                var caixa = $"CAIXA-{Environment.MachineName}";
                await _pdvManager.AbrirCaixa(_nomeOperador, caixa);
                
                _caixaAberto = true;
                lblCaixa.Text = caixa;
                lblStatusCaixa.Text = "CAIXA ABERTO";
                lblStatusCaixa.ForeColor = Color.LightGreen;
                
                lblStatusOperacao.Text = "Caixa aberto - Sistema pronto";
                AtualizarStatusInterface();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AbrirCaixa", ex.Message, ex);
                lblStatusOperacao.Text = "Erro ao abrir caixa";
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
                
                lblStatusOperacao.Text = "Nova venda iniciada";
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
            var texto = txbCodBarras.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(texto))
            {
                // Campo vazio — reseta feedback
                lblUltimoItemNome.Text = "---";
                lblUltimoItemNome.ForeColor = Color.White;
                lblUltimoItemPreco.Text = 0m.FormatarMoeda();
                lblUltimoItemQtd.Text = "";
                return;
            }
            
            // Feedback visual: mostra o que esta sendo digitado
            lblUltimoItemNome.Text = $"Codigo: {texto}";
            lblUltimoItemNome.ForeColor = Color.LightGray;
            lblUltimoItemPreco.Text = "";
            lblUltimoItemQtd.Text = texto.Length < SCANNER_MIN_CHARS 
                ? $"Digite {SCANNER_MIN_CHARS - texto.Length} digitos a mais ou ENTER" 
                : "Pressione ENTER para buscar";
            
            // Detecta scanner: caracteres chegam muito rapido (< 50ms cada)
            var agora = DateTime.Now;
            var intervalo = (agora - _lastKeystroke).TotalMilliseconds;
            _lastKeystroke = agora;
            
            if (intervalo < 80) // Input rapido = scanner
                _keystrokeCount++;
            else
                _keystrokeCount = 1; // Reset para digitacao manual
            
            // Scanner detectado: inicia debounce curto para aguardar fim do scan
            if (_keystrokeCount >= 5 && texto.Length >= SCANNER_MIN_CHARS)
            {
                _timerDebounceBarcode?.Stop();
                _timerDebounceBarcode = new System.Windows.Forms.Timer();
                _timerDebounceBarcode.Interval = SCANNER_DEBOUNCE_MS;
                _timerDebounceBarcode.Tick += async (s, ev) =>
                {
                    _timerDebounceBarcode.Stop();
                    _timerDebounceBarcode.Dispose();
                    _timerDebounceBarcode = null;
                    _keystrokeCount = 0;
                    await ProcessarCodigoBarras(txbCodBarras.Text.Trim());
                };
                _timerDebounceBarcode.Start();
            }
        }
        
        private async void txbCodBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite numeros e controle (backspace, etc)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            
            // Enter = busca manual
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                
                // Cancela debounce do scanner se estiver ativo
                _timerDebounceBarcode?.Stop();
                _timerDebounceBarcode?.Dispose();
                _timerDebounceBarcode = null;
                _keystrokeCount = 0;
                
                var texto = txbCodBarras.Text.Trim();
                if (string.IsNullOrWhiteSpace(texto))
                    return;
                
                await ProcessarCodigoBarras(texto);
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
                
                // Feedback visual: buscando...
                lblUltimoItemNome.Text = "Buscando...";
                lblUltimoItemNome.ForeColor = Color.Yellow;
                lblUltimoItemPreco.Text = "";
                lblUltimoItemQtd.Text = "";
                Application.DoEvents();
                
                var produto = await _pdvManager.BuscarProdutoPorCodigo(codigoBarras);
                
                sw.Stop();
                PdvLogger.LogPerformance("BuscarProduto", sw.Elapsed);
                
                if (produto == null)
                {
                    // Feedback visual no painel — sem MessageBox bloqueante
                    lblUltimoItemNome.Text = $"NAO ENCONTRADO";
                    lblUltimoItemNome.ForeColor = Color.FromArgb(231, 76, 60); // Vermelho
                    lblUltimoItemPreco.Text = "";
                    lblUltimoItemQtd.Text = $"Codigo: {codigoBarras}";
                    lblUltimoItemQtd.ForeColor = Color.FromArgb(231, 76, 60);
                    
                    System.Media.SystemSounds.Hand.Play(); // Som de erro
                    
                    // Seleciona texto para facilitar redigitacao
                    txbCodBarras.SelectAll();
                    txbCodBarras.Focus();
                    return;
                }
                
                // Preenche dados do produto
                PreencherDadosProduto(produto);
                
                // Verifica alertas
                var alertas = produto.GetAlertasVenda();
                if (alertas.Any())
                {
                    // Mostra alertas no painel antes de perguntar
                    lblUltimoItemQtd.Text = "ALERTA DE ESTOQUE";
                    lblUltimoItemQtd.ForeColor = Color.Orange;
                    
                    var mensagem = $"ATENCAO:\n- {string.Join("\n- ", alertas)}\n\nDeseja continuar?";
                    var resultado = MessageBox.Show(mensagem, "Alertas do Produto", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (resultado == DialogResult.No)
                    {
                        LimparCamposProduto();
                        return;
                    }
                }
                
                // Adiciona ao carrinho
                AdicionarItemCarrinho(produto);
            }
            catch (Exception ex)
            {
                sw.Stop();
                PdvLogger.LogError("ProcessarCodigoBarras", ex.Message, ex);
                
                // Feedback visual de erro — sem MessageBox
                lblUltimoItemNome.Text = "ERRO NA BUSCA";
                lblUltimoItemNome.ForeColor = Color.FromArgb(231, 76, 60);
                lblUltimoItemPreco.Text = "";
                lblUltimoItemQtd.Text = "Tente novamente";
                lblUltimoItemQtd.ForeColor = Color.FromArgb(231, 76, 60);
                
                txbCodBarras.SelectAll();
                txbCodBarras.Focus();
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void PreencherDadosProduto(Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data produto)
        {
            txbDescricao.Text = produto.NomeProduto.TruncateForDisplay(50);
            txbPrecoUnit.Text = produto.PrecoVenda.FormatarMoeda();
            
            var quantidade = GetQuantidadeSelecionada();
            var total = produto.PrecoVenda * quantidade;
            txbTotalRecebido.Text = total.FormatarMoeda();
            
            // Atualiza display "ULTIMO ITEM" (como monitor de cliente)
            lblUltimoItemNome.Text = produto.NomeProduto.TruncateForDisplay(40);
            lblUltimoItemPreco.Text = total.FormatarMoeda();
            lblUltimoItemQtd.Text = quantidade > 1 ? $"{quantidade}x {produto.PrecoVenda.FormatarMoeda()}" : "";
            
            // Alerta visual de estoque baixo
            if (produto.QuantidadeEstoqueProduto <= 10)
            {
                lblUltimoItemQtd.Text = $"Estoque: {produto.QuantidadeEstoqueProduto}";
                lblUltimoItemQtd.ForeColor = Color.Orange;
            }
            else
            {
                lblUltimoItemQtd.ForeColor = Color.LightGray;
            }
        }
        
        private void AdicionarItemCarrinho(Sis_Pdv_Controle_Estoque_Form.Dto.Produto.Data produto)
        {
            try
            {
                var quantidade = GetQuantidadeSelecionada();
                // Usa o overload que recebe o produto diretamente (sem chamar API de novo)
                _pdvManager.AdicionarItem(produto, quantidade);
                
                AtualizarCarrinho();
                AtualizarTotais();
                
                // Atualiza ULTIMO ITEM apos adicionar ao carrinho
                // (mostra dados consolidados do carrinho, nao do scan individual)
                var itemNoCarrinho = _pdvManager.VendaAtual?.Itens
                    .LastOrDefault(i => i.CodigoBarras == produto.CodBarras && !i.Cancelado);
                if (itemNoCarrinho != null)
                {
                    lblUltimoItemNome.Text = itemNoCarrinho.Descricao.Length > 40 
                        ? itemNoCarrinho.Descricao.Substring(0, 37) + "..." 
                        : itemNoCarrinho.Descricao;
                    lblUltimoItemNome.ForeColor = Color.White;
                    lblUltimoItemPreco.Text = itemNoCarrinho.Total.FormatarMoeda();
                    lblUltimoItemQtd.Text = itemNoCarrinho.Quantidade > 1 
                        ? $"{itemNoCarrinho.Quantidade}x {itemNoCarrinho.PrecoUnitario.FormatarMoeda()}" 
                        : "";
                    lblUltimoItemQtd.ForeColor = Color.LightGray;
                }
                
                // Limpa barcode e reseta quantidade para proximo item
                txbCodBarras.Clear();
                txbQuantidade.Text = "1";
                _keystrokeCount = 0;
                txbCodBarras.Focus();
                
                // Som de sucesso
                System.Media.SystemSounds.Beep.Play();
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AdicionarItemCarrinho", ex.Message, ex);
                
                // Mostra erro no painel, nao em MessageBox
                lblUltimoItemNome.Text = "ERRO AO ADICIONAR";
                lblUltimoItemNome.ForeColor = Color.FromArgb(231, 76, 60);
                lblUltimoItemPreco.Text = "";
                lblUltimoItemQtd.Text = ex.Message.Length > 50 ? ex.Message.Substring(0, 47) + "..." : ex.Message;
                lblUltimoItemQtd.ForeColor = Color.FromArgb(231, 76, 60);
                System.Media.SystemSounds.Hand.Play();
                
                txbCodBarras.SelectAll();
                txbCodBarras.Focus();
            }
        }
        
        private int GetQuantidadeSelecionada()
        {
            if (int.TryParse(txbQuantidade.Text, out int quantidade) && quantidade > 0)
                return quantidade;
            return 1;
        }
        
        #endregion
        
        #region Atualiza��o da Interface
        
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
                
                // Atualiza contador e valor total no t�tulo do GroupBox
                var totalItens = _itensCarrinho.Count(i => !i.Cancelado);
                var valorTotal = _itensCarrinho.Where(i => !i.Cancelado).Sum(i => i.Total);
                
                gbCarrinho.Text = $"CARRINHO DE COMPRAS ({totalItens} itens) - Total: {valorTotal:C2}";
                
                // Auto-scroll para o �ltimo item adicionado
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
                    gbCarrinho.ForeColor = Color.FromArgb(52, 73, 94); // Padr�o
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("AtualizarCarrinho", ex.Message, ex);
            }
        }
        
        private void FormatarColunasGrid()
        {
            // Colunas ja estao pre-configuradas em ConfigurarColunasGridInicial().
            // Este metodo apenas garante que a formatacao continua correta
            // apos o DataSource ser atualizado.
            if (dgvCarrinho.Columns.Count == 0) return;
            
            try
            {
                // Aplica estilo de linhas
                dgvCarrinho.RowTemplate.Height = 40;
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
                            // ? Item cancelado - vermelho com strikethrough
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                            row.DefaultCellStyle.Font = new Font(row.DefaultCellStyle.Font, FontStyle.Strikeout);
                            
                            // Adiciona �cone de cancelado na descri��o
                            if (row.Cells["Descricao"] != null)
                            {
                                var descricao = item.Descricao;
                                if (!descricao.StartsWith("[X]"))
                                    row.Cells["Descricao"].Value = $"[X] {descricao}";
                            }
                        }
                        else
                        {
                            var alertas = item.Validar();
                            
                            if (alertas.Any(a => a.Contains("vencido")))
                            {
                                // ?? Produto vencido - n�o pode ser vendido
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 205);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                            }
                            else if (alertas.Any(a => a.Contains("estoque")))
                            {
                                // ?? Estoque baixo - aten��o
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                            }
                            else if (alertas.Any(a => a.Contains("vence")))
                            {
                                // ?? Pr�ximo ao vencimento
                                row.DefaultCellStyle.BackColor = Color.FromArgb(254, 240, 220);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(157, 88, 36);
                            }
                            else
                            {
                                // ? Item normal - alternando cores para melhor leitura
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
                        
                        // Destaca c�digo do item
                        if (row.Cells["Codigo"] != null)
                        {
                            row.Cells["Codigo"].Style.Font = new Font("Consolas", 10F, FontStyle.Bold);
                            row.Cells["Codigo"].Style.ForeColor = Color.FromArgb(52, 152, 219); // Azul para c�digos
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
                        lblStatusCaixa.Text = "VENDA EM ANDAMENTO";
                        lblStatusCaixa.ForeColor = Color.Yellow;
                    }
                }
                else
                {
                    lblSubTotal.Text = 0m.FormatarMoeda();
                    lblDesconto.Text = 0m.FormatarMoeda();
                    lblTotal.Text = 0m.FormatarMoeda();
                    lblNomeCaixa.Text = "CAIXA LIVRE - Aguardando produtos";
                    
                    lblStatusCaixa.Text = "CAIXA ABERTO";
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
                // Atualiza bot�es baseado no estado
                var temItens = _pdvManager.VendaAtual?.QuantidadeItensAtivos > 0;
                
                // Habilita/desabilita opera��es
                var podeOperar = _caixaAberto && _pdvManager.ValidarOperacao("ADICIONAR_ITEM");
                
                txbCodBarras.Enabled = podeOperar;
                txbQuantidade.Enabled = podeOperar;
                
                // Habilita bot�es de pagamento apenas se houver itens
                btnPagamentoDinheiro.Enabled = temItens;
                btnPagamentoCartao.Enabled = temItens;
                btnPagamentoPix.Enabled = temItens;
                btnFinalizarVenda.Enabled = temItens && !string.IsNullOrEmpty(lblFormaPagamento.Text);
                btnCancelarItem.Enabled = temItens;
                btnCancelarVenda.Enabled = temItens;
                
                // Atualiza t�tulo da janela
                var stats = _pdvManager.GetEstatisticasCaixa();
                Text = $"PDV Moderno - {stats["Operador"]} - {stats["Caixa"]}";
                
                // Atualiza status de opera��o
                if (temItens)
                {
                    lblStatusOperacao.Text = $"Venda em andamento - {_pdvManager.VendaAtual.QuantidadeItensAtivos} itens";
                }
                else
                {
                    lblStatusOperacao.Text = "Sistema PDV - Pronto para nova venda";
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
        
        #region M�todos de Limpeza
        
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
            
            // Reset cores dos bot�es
            btnFinalizarVenda.BackColor = Color.FromArgb(39, 174, 96);
            btnFinalizarVenda.Enabled = false;
            
            // Atualiza GroupBox do carrinho
            gbCarrinho.Text = "CARRINHO DE COMPRAS (0 itens)";
        }
        
        private void LimparCamposProduto()
        {
            txbCodBarras.Clear();
            txbDescricao.Clear();
            txbPrecoUnit.Clear();
            txbQuantidade.Text = "1";
            txbTotalRecebido.Clear();
            
            // Reseta display ULTIMO ITEM
            lblUltimoItemNome.Text = "---";
            lblUltimoItemNome.ForeColor = Color.White;
            lblUltimoItemPreco.Text = 0m.FormatarMoeda();
            lblUltimoItemQtd.Text = "";
            lblUltimoItemQtd.ForeColor = Color.LightGray;
            
            // Reseta contadores do scanner
            _keystrokeCount = 0;
            
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
                lblStatusOperacao.Text = "Processando...";
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
                    lblStatusOperacao.Text = "Sistema PDV - Pronto";
                }
            }
            
            // Desabilita controles durante carregamento
            txbCodBarras.Enabled = !loading && _caixaAberto;
            txbQuantidade.Enabled = !loading && _caixaAberto;
            dgvCarrinho.Enabled = !loading;
            
            // Desabilita bot�es durante loading
            btnPagamentoDinheiro.Enabled = !loading;
            btnPagamentoCartao.Enabled = !loading;
            btnPagamentoPix.Enabled = !loading;
            btnFinalizarVenda.Enabled = !loading;
            btnCancelarItem.Enabled = !loading;
            btnCancelarVenda.Enabled = !loading;
        }
        
        #endregion
        
        #region Opera��es de Pagamento
        
        private void PagamentoDinheiro()
        {
            try
            {
                if (!ValidarVendaParaPagamento()) return;
                
                using var frmDinheiro = new frmDinheiro();
                frmDinheiro.ReceberValor(lblTotal.Text);
                
                if (frmDinheiro.ShowDialog() == DialogResult.OK)
                {
                    // Usa propriedades decimais diretamente (sem parsing de string)
                    var valorRecebido = frmDinheiro.ValorRecebidoDecimal;
                    var troco = frmDinheiro.TrocoDecimal;
                    
                    ProcessarPagamento("Dinheiro", valorRecebido, troco);
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
                
                var valorFinal = _pdvManager.VendaAtual.ValorFinal;
                ProcessarPagamento("Cartao de Credito", valorFinal, 0);
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("PagamentoCartao", ex.Message, ex);
                MessageBox.Show($"Erro no pagamento com cartao: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PagamentoPix()
        {
            try
            {
                if (!ValidarVendaParaPagamento()) return;
                
                var valorFinal = _pdvManager.VendaAtual.ValorFinal;
                
                var resultado = MessageBox.Show(
                    $"PAGAMENTO PIX\n\n" +
                    $"Valor: {valorFinal:C2}\n\n" +
                    $"Confirme o pagamento PIX no dispositivo do cliente.\n\n" +
                    $"Pagamento foi aprovado?",
                    "Pagamento PIX",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (resultado == DialogResult.Yes)
                {
                    ProcessarPagamento("PIX", valorFinal, 0);
                }
            }
            catch (Exception ex)
            {
                PdvLogger.LogError("PagamentoPix", ex.Message, ex);
                MessageBox.Show($"Erro no pagamento PIX: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ProcessarPagamento(string FormaPagamento, decimal valorRecebido, decimal troco = 0)
        {
            try
            {
                _pdvManager.DefinirFormaPagamento(FormaPagamento, valorRecebido);
                
                // Atualiza interface com novos componentes
                lblFormaPagamento.Text = FormaPagamento;
                lblValorAReceber.Text = valorRecebido.FormatarMoeda();
                lblTroco.Text = troco.FormatarMoeda();
                
                // Mostra campos de pagamento
                ExibirCamposPagamento();
                
                // Atualiza status
                lblStatusOperacao.Text = $"Forma de pagamento: {FormaPagamento}";
                lblStatusCaixa.Text = "AGUARDANDO FINALIZACAO";
                lblStatusCaixa.ForeColor = Color.Yellow;
                
                // Habilita bot�o finalizar
                btnFinalizarVenda.Enabled = true;
                btnFinalizarVenda.BackColor = Color.FromArgb(39, 174, 96);
                
                PdvLogger.LogFormaPagamento(FormaPagamento, valorRecebido, $"Troco: {troco:C2}");
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
                MessageBox.Show("Adicione itens � venda antes de definir a forma de pagamento!", 
                    "Venda Vazia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            return true;
        }
        
        #endregion
        
        #region Finaliza��o e Cancelamento de Vendas
        
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
                
                // Imprime cupom (modal — operador ve o recibo e fecha)
                if (_configuracoes.ImprimirCupomAutomatico)
                {
                    await ImprimirCupomFiscal(pedidoId);
                }
                
                // Som de sucesso
                System.Media.SystemSounds.Asterisk.Play();
                
                // Inicia nova venda — limpa tela e volta para PDV LIVRE
                IniciarNovaVenda();
                
                // Atualiza status do header imediatamente (sem esperar timer)
                lblStatusCaixa.Text = "CAIXA ABERTO";
                lblStatusCaixa.ForeColor = Color.LightGreen;
                lblStatusOperacao.Text = "Venda finalizada com sucesso! Aguardando nova venda...";
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
            if (_colaboradorId == Guid.Empty)
            {
                MessageBox.Show("Operador nao identificado!\n\nFecha o PDV e faca login novamente.", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if (_pdvManager.VendaAtual == null)
            {
                MessageBox.Show("N\u00e3o h\u00e1 venda ativa!", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (string.IsNullOrEmpty(lblFormaPagamento.Text) || lblFormaPagamento.Text == "---")
            {
                MessageBox.Show("Defina a forma de pagamento antes de finalizar!\n\nF3 = Dinheiro\nF4 = Cartao\nF6 = PIX", 
                    "Forma de Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (_pdvManager.VendaAtual.QuantidadeItensAtivos == 0)
            {
                MessageBox.Show("Adicione itens � venda!", "Venda Vazia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            var erros = _pdvManager.VendaAtual.Validar();
            if (erros.Any())
            {
                MessageBox.Show($"Venda inv�lida:\n� {string.Join("\n� ", erros)}", 
                    "Valida��o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("N�o h� venda para cancelar!", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var resultado = MessageBox.Show(
                    "Deseja realmente cancelar esta venda?\n\nTodos os itens ser�o removidos.",
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
            // Implementar dialog para autoriza��o de cancelamento
            // Por enquanto usa autoriza��o simples
            using var frmVerificaLogin = new frmVerificaLogin();
            if (frmVerificaLogin.ShowDialog() == DialogResult.OK)
            {
                return "Cancelamento autorizado";
            }
            return "";
        }
        
        #endregion
        
        #region Opera��es de Itens
        
        private void CancelarItem()
        {
            try
            {
                if (_pdvManager.VendaAtual?.QuantidadeItensAtivos == 0)
                {
                    MessageBox.Show("N�o h� itens para cancelar!", "Aviso", 
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
            // Usa o item selecionado no grid
            if (dgvCarrinho.CurrentRow != null)
            {
                var rowIndex = dgvCarrinho.CurrentRow.Index;
                if (rowIndex >= 0 && rowIndex < _itensCarrinho.Count)
                {
                    var item = _itensCarrinho[rowIndex];
                    if (!item.Cancelado)
                    {
                        var confirma = MessageBox.Show(
                            $"Cancelar item {item.Codigo}?\n\n" +
                            $"{item.Descricao}\n" +
                            $"Qtd: {item.Quantidade} x {item.PrecoUnitario.FormatarMoeda()} = {item.Total.FormatarMoeda()}",
                            "Confirmar Cancelamento",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        
                        if (confirma == DialogResult.Yes)
                            return item.Codigo;
                    }
                    else
                    {
                        MessageBox.Show("Este item ja esta cancelado.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um item no carrinho para cancelar.", "Selecione um Item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return 0;
        }
        
        #endregion
        
        #region Impress�o de Cupom
        
        private async Task ImprimirCupomFiscal(Guid pedidoId)
        {
            try
            {
                PdvLogger.LogImpressaoCupom(pedidoId, "FISCAL", false, "Iniciando impress�o");
                
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
                MessageBox.Show($"Erro ao imprimir cupom: {ex.Message}", "Erro de Impress�o",
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
                        
                    case Keys.F3:
                        PagamentoDinheiro();
                        return true;
                        
                    case Keys.F4:
                        PagamentoCartao();
                        return true;
                        
                    case Keys.F6:
                        PagamentoPix();
                        return true;
                        
                    case Keys.F7:
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
                            var result = MessageBox.Show("H� uma venda em andamento. Deseja realmente sair?", 
                                "Confirma��o", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                       "F3 - Pagamento dinheiro\n" +
                       "F4 - Pagamento cartao\n" +
                       "F5 - Limpar campos\n" +
                       "F6 - Pagamento PIX\n" +
                       "F7 - Cancelar item\n" +
                       "F8 - Cancelar venda\n" +
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
            // Atualiza c�lculo quando quantidade muda
            AtualizarCalculoQuantidade();
        }
        
        private void txbQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas n�meros
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
                    
                    // Atualiza display ULTIMO ITEM com nova quantidade
                    lblUltimoItemPreco.Text = total.FormatarMoeda();
                    lblUltimoItemQtd.Text = quantidade > 1 ? $"{quantidade}x {preco.FormatarMoeda()}" : "";
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
                        "H� uma venda em andamento. Deseja realmente sair?\n\nA venda ser� perdida.",
                        "Confirma��o de Sa�da", 
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
                    var valorFinal = 0m; // Implementar c�lculo real
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
        
        #region M�todos Auxiliares (Compatibilidade)
        
        // M�todos mantidos para compatibilidade com c�digo existente
        
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
            // M�todo legado - nova implementa��o usa AdicionarItemCarrinho
        }
        
        private bool VerificaVazio()
        {
            return string.IsNullOrWhiteSpace(txbCodBarras.Text) ||
                   string.IsNullOrWhiteSpace(txbQuantidade.Text);
        }
        
        #endregion
        
        #region Eventos Legados (Compatibilidade)
        
        private void timerData_Tick(object sender, EventArgs e)
        {
            Timer_Tick(sender, e);
        }
        
        #endregion
        
        #region Event Handlers dos Bot�es Modernos
        
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
