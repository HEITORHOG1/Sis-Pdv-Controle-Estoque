using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Globalization;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Dinheiro
{
    public partial class frmDinheiro : Form
    {
        #region Campos Privados
        
        private bool _isLoading = false;
        private bool _valorValido = false;
        private decimal _totalVenda = 0m;
        private decimal _valorRecebido = 0m;
        private decimal _troco = 0m;
        private decimal _valorAReceber = 0m;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmDinheiro()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Configura estado inicial
            ConfigurarEstadoInicial();
            AtualizarStatusInterface();
            
            // Log de inicialização
            MoneyLogger.LogInfo("Formulário de pagamento em dinheiro inicializado", "Startup");
        }
        
        private void ConfigurarEstadoInicial()
        {
            txbValorRecebido.Text = "R$ 0,00";
            lblSubTotalValor.Text = "R$ 0,00";
            lblValorAReceber.Text = "R$ 0,00";
            lblValorTroco.Text = "R$ 0,00";
            
            _valorValido = false;
            AtualizarCoresCalculos();
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public string ValorRecibido
        {
            get { return FormatarMoeda(_valorRecebido); }
            set 
            { 
                if (decimal.TryParse(LimparMoeda(value), out decimal valor))
                {
                    _valorRecebido = valor;
                    txbValorRecebido.Text = FormatarMoeda(valor);
                    CalcularTroco();
                }
            }
        }
        
        public string Troco
        {
            get { return FormatarMoeda(_troco); }
            set 
            { 
                if (decimal.TryParse(LimparMoeda(value), out decimal valor))
                {
                    _troco = valor;
                    lblValorTroco.Text = FormatarMoeda(valor);
                }
            }
        }
        
        public decimal ValorTotalVenda => _totalVenda;
        public decimal ValorRecebidoDecimal => _valorRecebido;
        public decimal TrocoDecimal => _troco;
        public bool PagamentoConfirmado { get; private set; } = false;
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            ProcessarConfirmacao();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        #endregion
        
        #region Eventos do Campo de Entrada
        
        private void txbValorRecebido_Enter(object sender, EventArgs e)
        {
            if (txbValorRecebido.Text == "R$ 0,00")
            {
                txbValorRecebido.Text = "";
                lblInputIcon.Text = "✏️";
                lblInputIcon.ForeColor = Color.FromArgb(52, 152, 219);
            }
            
            // Remove formatação para edição
            if (_valorRecebido > 0)
            {
                txbValorRecebido.Text = _valorRecebido.ToString("F2");
            }
        }
        
        private void txbValorRecebido_Leave(object sender, EventArgs e)
        {
            // Aplica formatação de moeda
            if (string.IsNullOrWhiteSpace(txbValorRecebido.Text))
            {
                _valorRecebido = 0;
                txbValorRecebido.Text = "R$ 0,00";
                lblInputIcon.Text = "💵";
                lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
            else
            {
                FormatarCampoMoeda();
            }
            
            CalcularTroco();
            AtualizarStatusInterface();
        }
        
        private void txbValorRecebido_TextChanged(object sender, EventArgs e)
        {
            // Aplica formatação em tempo real apenas se não estiver em edição
            if (!txbValorRecebido.Focused)
            {
                FormatarCampoMoeda();
            }
            
            ValidarEntrada();
        }
        
        private void txbValorRecebido_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números, vírgula, ponto e backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && 
                e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
                ExibirFeedbackEntradaInvalida();
                return;
            }
            
            // Substitui ponto por vírgula
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
            
            // Permite apenas uma vírgula
            if (e.KeyChar == ',' && txbValorRecebido.Text.Contains(','))
            {
                e.Handled = true;
                return;
            }
            
            // Se pressionou Enter, confirma
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                ProcessarConfirmacao();
            }
        }
        
        #endregion
        
        #region Eventos de Sugestões
        
        private void btnSugestao_Click(object sender, EventArgs e)
        {
            try
            {
                var botao = sender as Button;
                if (botao == null) return;
                
                decimal valorSugerido = 0;
                
                if (botao == btnSugestaoExato)
                {
                    valorSugerido = _totalVenda;
                }
                else if (botao == btnSugestao20)
                {
                    valorSugerido = _totalVenda + 20m;
                }
                else if (botao == btnSugestao50)
                {
                    valorSugerido = _totalVenda + 50m;
                }
                else if (botao == btnSugestao100)
                {
                    valorSugerido = _totalVenda + 100m;
                }
                
                // Aplica o valor sugerido
                _valorRecebido = valorSugerido;
                txbValorRecebido.Text = FormatarMoeda(valorSugerido);
                
                CalcularTroco();
                AtualizarStatusInterface();
                
                // Destaca o botão selecionado temporariamente
                DestacaBotaoSugestao(botao);
                
                MoneyLogger.LogInfo($"Valor sugerido selecionado: {FormatarMoeda(valorSugerido)}", "UserAction");
            }
            catch (Exception ex)
            {
                MoneyLogger.LogError($"Erro ao aplicar sugestão: {ex.Message}", "Suggestion", ex);
            }
        }
        
        private void DestacaBotaoSugestao(Button botao)
        {
            var corOriginal = botao.BackColor;
            botao.BackColor = Color.FromArgb(241, 196, 15); // Amarelo destaque
            
            // Timer para restaurar cor original
            var timer = new System.Windows.Forms.Timer { Interval = 500 };
            timer.Tick += (s, e) =>
            {
                botao.BackColor = corOriginal;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void frmDinheiro_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ProcessarConfirmacao();
                    break;
                case Keys.Escape:
                    FecharFormulario(false);
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
                case Keys.D1:
                    btnSugestaoExato.PerformClick();
                    break;
                case Keys.D2:
                    btnSugestao20.PerformClick();
                    break;
                case Keys.D3:
                    btnSugestao50.PerformClick();
                    break;
                case Keys.D4:
                    btnSugestao100.PerformClick();
                    break;
            }
        }
        
        private void frmDinheiro_Load(object sender, EventArgs e)
        {
            // Configura valores iniciais
            if (_totalVenda > 0)
            {
                lblSubTotalValor.Text = FormatarMoeda(_totalVenda);
                _valorAReceber = _totalVenda;
                lblValorAReceber.Text = FormatarMoeda(_valorAReceber);
                AtualizarBotoesSugestao();
            }
            
            // Foco no campo de entrada
            txbValorRecebido.Focus();
            txbValorRecebido.SelectAll();
            
            MoneyLogger.LogInfo($"Formulário carregado - Total da venda: {FormatarMoeda(_totalVenda)}", "UserInterface");
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Formatação e Validação
        
        private void FormatarCampoMoeda()
        {
            try
            {
                var textoLimpo = LimparMoeda(txbValorRecebido.Text);
                
                if (decimal.TryParse(textoLimpo, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out decimal valor))
                {
                    _valorRecebido = valor;
                    
                    // Só reformata se não estiver em foco (evita interferir na digitação)
                    if (!txbValorRecebido.Focused)
                    {
                        txbValorRecebido.Text = FormatarMoeda(valor);
                    }
                }
                else
                {
                    _valorRecebido = 0;
                }
            }
            catch (Exception ex)
            {
                MoneyLogger.LogError($"Erro na formatação de moeda: {ex.Message}", "Formatting", ex);
                _valorRecebido = 0;
            }
        }
        
        private void ValidarEntrada()
        {
            _valorValido = _valorRecebido >= _totalVenda;
            
            // Atualiza ícone baseado na validação
            if (_valorRecebido == 0)
            {
                lblInputIcon.Text = "💵";
                lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
            else if (_valorValido)
            {
                lblInputIcon.Text = "✅";
                lblInputIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
            else
            {
                lblInputIcon.Text = "⚠️";
                lblInputIcon.ForeColor = Color.FromArgb(230, 126, 34);
            }
            
            AtualizarCoresCalculos();
        }
        
        private void CalcularTroco()
        {
            try
            {
                if (_valorRecebido >= _totalVenda)
                {
                    _troco = _valorRecebido - _totalVenda;
                    _valorAReceber = 0;
                }
                else
                {
                    _troco = 0;
                    _valorAReceber = _totalVenda - _valorRecebido;
                }
                
                // Atualiza interface
                lblValorTroco.Text = FormatarMoeda(_troco);
                lblValorAReceber.Text = FormatarMoeda(_valorAReceber);
                
                ValidarEntrada();
                AtualizarStatusInterface();
                
                MoneyLogger.LogInfo($"Cálculo: Recebido={FormatarMoeda(_valorRecebido)}, Troco={FormatarMoeda(_troco)}, A Receber={FormatarMoeda(_valorAReceber)}", "Calculation");
            }
            catch (Exception ex)
            {
                MoneyLogger.LogError($"Erro no cálculo do troco: {ex.Message}", "Calculation", ex);
            }
        }
        
        private void AtualizarCoresCalculos()
        {
            // Cores baseadas no status do pagamento
            if (_valorAReceber > 0)
            {
                // Ainda falta dinheiro
                pnAReceber.BackColor = Color.FromArgb(255, 248, 220); // Amarelo claro
                lblValorAReceber.ForeColor = Color.FromArgb(230, 126, 34); // Laranja
            }
            else
            {
                // Pagamento completo
                pnAReceber.BackColor = Color.FromArgb(236, 240, 241); // Cinza claro
                lblValorAReceber.ForeColor = Color.FromArgb(149, 165, 166); // Cinza
            }
            
            if (_troco > 0)
            {
                // Há troco
                pnTroco.BackColor = Color.FromArgb(235, 251, 238); // Verde claro
                lblValorTroco.ForeColor = Color.FromArgb(46, 204, 113); // Verde
            }
            else
            {
                // Sem troco
                pnTroco.BackColor = Color.FromArgb(236, 240, 241); // Cinza claro
                lblValorTroco.ForeColor = Color.FromArgb(149, 165, 166); // Cinza
            }
        }
        
        #endregion
        
        #region Processamento e Validação
        
        private async void ProcessarConfirmacao()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarPagamento()) return;
                
                SetLoadingState(true);
                
                // Simula processamento
                await Task.Delay(500);
                
                sw.Stop();
                MoneyLogger.LogInfo($"Pagamento confirmado - Total: {FormatarMoeda(_totalVenda)}, Recebido: {FormatarMoeda(_valorRecebido)}, Troco: {FormatarMoeda(_troco)}", "Payment");
                MoneyLogger.LogPerformance("ProcessarPagamento", sw.Elapsed);
                
                FecharFormulario(true);
            }
            catch (Exception ex)
            {
                sw.Stop();
                MoneyLogger.LogError($"Erro ao processar pagamento: {ex.Message}", "Payment", ex);
                ExibirErroProcessamento($"Erro interno: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarPagamento()
        {
            if (_totalVenda <= 0)
            {
                ExibirErroProcessamento("Valor total da venda inválido.");
                return false;
            }
            
            if (_valorRecebido <= 0)
            {
                ExibirErroProcessamento("Digite um valor recebido válido.");
                txbValorRecebido.Focus();
                return false;
            }
            
            if (_valorRecebido < _totalVenda)
            {
                var faltam = _totalVenda - _valorRecebido;
                ExibirErroProcessamento($"Valor insuficiente!\n\nFaltam: {FormatarMoeda(faltam)}");
                txbValorRecebido.Focus();
                return false;
            }
            
            // Confirmação para valores muito altos (possível erro de digitação)
            if (_valorRecebido > _totalVenda * 10)
            {
                var resultado = MessageBox.Show(
                    $"⚠️ ATENÇÃO - Valor Alto Detectado\n\n" +
                    $"Total da venda: {FormatarMoeda(_totalVenda)}\n" +
                    $"Valor digitado: {FormatarMoeda(_valorRecebido)}\n" +
                    $"Troco: {FormatarMoeda(_troco)}\n\n" +
                    $"Confirma este valor?",
                    "Confirmação de Valor",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (resultado == DialogResult.No)
                {
                    txbValorRecebido.Focus();
                    txbValorRecebido.SelectAll();
                    return false;
                }
            }
            
            return true;
        }
        
        private void ExibirErroProcessamento(string mensagem)
        {
            MessageBox.Show(mensagem, "❌ Erro no Pagamento",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            MoneyLogger.LogWarning($"Erro de validação: {mensagem}", "Validation");
        }
        
        private void ExibirFeedbackEntradaInvalida()
        {
            // Feedback visual rápido
            var corOriginal = pnInput.BackColor;
            pnInput.BackColor = Color.FromArgb(255, 235, 235);
            
            // Timer para restaurar cor original
            var timer = new System.Windows.Forms.Timer { Interval = 200 };
            timer.Tick += (s, e) =>
            {
                pnInput.BackColor = corOriginal;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
            
            // Atualiza ícone
            lblInputIcon.Text = "❌";
            lblInputIcon.ForeColor = Color.FromArgb(231, 76, 60);
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Processando pagamento...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txbValorRecebido.Enabled = false;
                btnOK.Enabled = false;
                btnCancelar.Enabled = false;
                
                // Desabilita sugestões
                DesabilitarBotoesSugestao(false);
            }
            else
            {
                if (_valorValido && _valorAReceber == 0)
                {
                    lblStatus.Text = $"✅ Pagamento válido - Troco: {FormatarMoeda(_troco)}";
                    lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                    btnOK.BackColor = Color.FromArgb(46, 204, 113);
                }
                else if (_valorRecebido > 0 && _valorAReceber > 0)
                {
                    lblStatus.Text = $"⏳ Faltam: {FormatarMoeda(_valorAReceber)}";
                    lblStatus.ForeColor = Color.FromArgb(230, 126, 34);
                    btnOK.BackColor = Color.FromArgb(149, 165, 166);
                }
                else
                {
                    lblStatus.Text = "💰 Digite o valor recebido em dinheiro";
                    lblStatus.ForeColor = Color.White;
                    btnOK.BackColor = Color.FromArgb(149, 165, 166);
                }
                
                progressBar.Visible = false;
                
                // Habilita controles
                txbValorRecebido.Enabled = true;
                btnOK.Enabled = true;
                btnCancelar.Enabled = true;
                
                // Habilita sugestões
                DesabilitarBotoesSugestao(true);
            }
        }
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            AtualizarStatusInterface();
            
            if (loading)
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        
        private void AtualizarBotoesSugestao()
        {
            // Atualiza textos dos botões com valores baseados no total
            btnSugestaoExato.Text = $"✓ {FormatarMoeda(_totalVenda)}";
            btnSugestao20.Text = $"💵 {FormatarMoeda(_totalVenda + 20)}";
            btnSugestao50.Text = $"💵 {FormatarMoeda(_totalVenda + 50)}";
            btnSugestao100.Text = $"💵 {FormatarMoeda(_totalVenda + 100)}";
        }
        
        private void DesabilitarBotoesSugestao(bool habilitar)
        {
            btnSugestaoExato.Enabled = habilitar;
            btnSugestao20.Enabled = habilitar;
            btnSugestao50.Enabled = habilitar;
            btnSugestao100.Enabled = habilitar;
        }
        
        #endregion
        
        #region Métodos Públicos
        
        public void ReceberValor(string total)
        {
            try
            {
                var valorLimpo = LimparMoeda(total);
                if (decimal.TryParse(valorLimpo, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out decimal valor))
                {
                    _totalVenda = valor;
                    lblSubTotalValor.Text = FormatarMoeda(valor);
                    _valorAReceber = valor;
                    lblValorAReceber.Text = FormatarMoeda(valor);
                    
                    AtualizarBotoesSugestao();
                    AtualizarStatusInterface();
                    
                    MoneyLogger.LogInfo($"Valor total recebido: {FormatarMoeda(valor)}", "Setup");
                }
                else
                {
                    MoneyLogger.LogError($"Valor inválido recebido: {total}", "Setup");
                }
            }
            catch (Exception ex)
            {
                MoneyLogger.LogError($"Erro ao receber valor: {ex.Message}", "Setup", ex);
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        private string FormatarMoeda(decimal valor)
        {
            return valor.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
        }
        
        private string LimparMoeda(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "0";
            
            return texto
                .Replace("R$", "")
                .Replace(".", "")
                .Replace(" ", "")
                .Trim();
        }
        
        private void FecharFormulario(bool confirmado)
        {
            try
            {
                PagamentoConfirmado = confirmado;
                
                var resultado = confirmado ? "confirmado" : "cancelado";
                MoneyLogger.LogInfo($"Pagamento em dinheiro {resultado}", "UserAction");
                
                this.DialogResult = confirmado ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MoneyLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - PAGAMENTO EM DINHEIRO\n\n" +
                       "💰 COMO USAR:\n" +
                       "• Digite o valor recebido do cliente\n" +
                       "• O sistema calcula automaticamente o troco\n" +
                       "• Use os botões de sugestão para valores comuns\n" +
                       "• Confirme quando o valor estiver correto\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER - Confirmar pagamento\n" +
                       "• ESC - Cancelar operação\n" +
                       "• 1 - Valor exato\n" +
                       "• 2 - Total + R$ 20\n" +
                       "• 3 - Total + R$ 50\n" +
                       "• 4 - Total + R$ 100\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "💡 DICAS:\n" +
                       "• O valor mínimo é o total da venda\n" +
                       "• Para valores altos, o sistema pede confirmação\n" +
                       "• Use apenas números, vírgulas e pontos\n" +
                       "• O troco é calculado automaticamente";
            
            MessageBox.Show(ajuda, "Ajuda - Pagamento em Dinheiro",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantido para compatibilidade com código existente
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    btnOK.Focus();
                    ProcessarConfirmacao();
                    return true;
                case Keys.Escape:
                    txbValorRecebido.Text = "R$ 0,00";
                    FecharFormulario(false);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        // Método legado de formatação de moeda (mantido para compatibilidade)
        public static void Moeda(ref TextBox txt)
        {
            try
            {
                string n = txt.Text.Replace(",", "").Replace(".", "").Replace("R$", "").Replace(" ", "");
                if (string.IsNullOrEmpty(n)) n = "0";
                
                n = n.PadLeft(3, '0');
                if (n.Length > 3 && n.Substring(0, 1) == "0")
                    n = n.Substring(1, n.Length - 1);
                
                double v = Convert.ToDouble(n) / 100;
                txt.Text = string.Format("R$ {0:N2}", v);
                txt.SelectionStart = txt.Text.Length;
            }
            catch (Exception)
            {
                txt.Text = "R$ 0,00";
            }
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class MoneyLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [Money-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Console.WriteLine($"[WARN] [Money-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [Money-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogPerformance(string operation, TimeSpan duration)
            {
                Console.WriteLine($"[PERF] [Money] {operation} - {duration.TotalMilliseconds}ms");
            }
        }
        
        #endregion
    }
}