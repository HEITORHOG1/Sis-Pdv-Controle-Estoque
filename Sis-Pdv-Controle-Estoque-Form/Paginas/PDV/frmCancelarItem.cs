using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    public partial class frmCancelarItem : Form
    {
        #region Campos Privados
        
        private bool _isLoading = false;
        private bool _isValid = false;
        private const string PLACEHOLDER_TEXT = "Digite o número do item...";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmCancelarItem()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Configura estado inicial
            ConfigurarPlaceholder();
            AtualizarStatusInterface();
            
            // Log de inicialização
            CancelItemLogger.LogInfo("Formulário de cancelar item inicializado", "Startup");
        }
        
        private void ConfigurarPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(txbItemCancelar.Text))
            {
                txbItemCancelar.Text = PLACEHOLDER_TEXT;
                txbItemCancelar.ForeColor = Color.FromArgb(149, 165, 166);
                _isValid = false;
            }
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public string Parametro
        {
            get { 
                return txbItemCancelar.Text == PLACEHOLDER_TEXT ? string.Empty : txbItemCancelar.Text.Trim(); 
            }
            set { 
                if (!string.IsNullOrWhiteSpace(value))
                {
                    txbItemCancelar.Text = value;
                    txbItemCancelar.ForeColor = Color.FromArgb(52, 73, 94);
                    _isValid = true;
                    AtualizarStatusInterface();
                }
                else
                {
                    ConfigurarPlaceholder();
                }
            }
        }
        
        public DialogResult Resultado { get; private set; } = DialogResult.Cancel;
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario(DialogResult.Cancel);
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessarConfirmacao();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharFormulario(DialogResult.Cancel);
        }
        
        #endregion
        
        #region Eventos do Campo de Entrada
        
        private void txbItemCancelar_Enter(object sender, EventArgs e)
        {
            if (txbItemCancelar.Text == PLACEHOLDER_TEXT)
            {
                txbItemCancelar.Text = "";
                txbItemCancelar.ForeColor = Color.FromArgb(52, 73, 94);
                lblInputIcon.Text = "✏️";
                lblInputIcon.ForeColor = Color.FromArgb(52, 152, 219);
            }
        }
        
        private void txbItemCancelar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbItemCancelar.Text))
            {
                ConfigurarPlaceholder();
                lblInputIcon.Text = "🔢";
                lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
        }
        
        private void txbItemCancelar_TextChanged(object sender, EventArgs e)
        {
            ValidarEntrada();
            AtualizarStatusInterface();
        }
        
        private void txbItemCancelar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números e backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                
                // Feedback visual para entrada inválida
                ExibirFeedbackEntradaInvalida();
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
        
        #region Eventos de Teclado e Form
        
        private void frmCancelarItem_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ProcessarConfirmacao();
                    break;
                case Keys.Escape:
                    FecharFormulario(DialogResult.Cancel);
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
            }
        }
        
        private void frmCancelarItem_Load(object sender, EventArgs e)
        {
            // Foco no campo de entrada
            txbItemCancelar.Focus();
            CancelItemLogger.LogInfo("Formulário carregado e pronto para uso", "UserInterface");
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Validação e Processamento
        
        private void ValidarEntrada()
        {
            var texto = txbItemCancelar.Text;
            
            if (texto == PLACEHOLDER_TEXT || string.IsNullOrWhiteSpace(texto))
            {
                _isValid = false;
                return;
            }
            
            // Valida se é um número positivo
            if (int.TryParse(texto, out int numero) && numero > 0)
            {
                _isValid = true;
                lblInputIcon.Text = "✅";
                lblInputIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
            else
            {
                _isValid = false;
                lblInputIcon.Text = "❌";
                lblInputIcon.ForeColor = Color.FromArgb(231, 76, 60);
            }
        }
        
        private void ProcessarConfirmacao()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!_isValid)
                {
                    ExibirMensagemValidacao();
                    return;
                }
                
                var numeroItem = txbItemCancelar.Text.Trim();
                
                if (string.IsNullOrWhiteSpace(numeroItem) || numeroItem == PLACEHOLDER_TEXT)
                {
                    ExibirMensagemValidacao("Por favor, digite o número do item.");
                    return;
                }
                
                if (!int.TryParse(numeroItem, out int numero) || numero <= 0)
                {
                    ExibirMensagemValidacao("Digite um número válido maior que zero.");
                    return;
                }
                
                sw.Stop();
                CancelItemLogger.LogInfo($"Item #{numero} selecionado para cancelamento", "UserAction");
                CancelItemLogger.LogPerformance("ProcessarConfirmacao", sw.Elapsed);
                
                FecharFormulario(DialogResult.OK);
            }
            catch (Exception ex)
            {
                sw.Stop();
                CancelItemLogger.LogError($"Erro ao processar confirmação: {ex.Message}", "Processing", ex);
                ExibirMensagemValidacao("Erro interno. Tente novamente.");
            }
        }
        
        private void ExibirMensagemValidacao(string mensagem = null)
        {
            mensagem ??= "Por favor, digite um número de item válido.";
            
            MessageBox.Show(
                mensagem,
                "❌ Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            
            // Retorna foco para o campo
            txbItemCancelar.Focus();
            txbItemCancelar.SelectAll();
            
            CancelItemLogger.LogWarning($"Validação falhou: {mensagem}", "Validation");
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
            lblInputIcon.Text = "⚠️";
            lblInputIcon.ForeColor = Color.FromArgb(230, 126, 34);
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Processando...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txbItemCancelar.Enabled = false;
                btnOk.Enabled = false;
                btnCancelar.Enabled = false;
            }
            else
            {
                if (_isValid)
                {
                    lblStatus.Text = "✅ Número válido - Pronto para cancelar";
                    lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                    btnOk.BackColor = Color.FromArgb(46, 204, 113);
                }
                else
                {
                    lblStatus.Text = "🟡 Digite o número do item para cancelar";
                    lblStatus.ForeColor = Color.White;
                    btnOk.BackColor = Color.FromArgb(149, 165, 166);
                }
                
                progressBar.Visible = false;
                
                // Habilita controles
                txbItemCancelar.Enabled = true;
                btnOk.Enabled = true;
                btnCancelar.Enabled = true;
            }
        }
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            AtualizarStatusInterface();
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        private void FecharFormulario(DialogResult resultado)
        {
            try
            {
                Resultado = resultado;
                
                var acao = resultado == DialogResult.OK ? "confirmado" : "cancelado";
                CancelItemLogger.LogInfo($"Formulário de cancelar item {acao}", "UserAction");
                
                this.DialogResult = resultado;
                this.Close();
            }
            catch (Exception ex)
            {
                CancelItemLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - CANCELAR ITEM\n\n" +
                       "📝 COMO USAR:\n" +
                       "• Digite o número sequencial do item no carrinho\n" +
                       "• Pressione ENTER ou clique em 'Confirmar'\n" +
                       "• O item será marcado para cancelamento\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER - Confirmar cancelamento\n" +
                       "• ESC - Cancelar operação\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "⚠️ ATENÇÃO:\n" +
                       "• Apenas números são aceitos\n" +
                       "• O item deve existir no carrinho\n" +
                       "• A operação pode ser desfeita posteriormente";
            
            MessageBox.Show(ajuda, "Ajuda - Cancelar Item",
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
                    btnOk.Focus();
                    ProcessarConfirmacao();
                    return true;
                case Keys.Escape:
                    btnCancelar.Focus();
                    FecharFormulario(DialogResult.Cancel);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class CancelItemLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [CancelItem-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [CancelItem-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [CancelItem-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogPerformance(string operation, TimeSpan duration)
            {
                Debug.WriteLine($"[PERF] [CancelItem] {operation} - {duration.TotalMilliseconds}ms");
            }
        }
        
        #endregion
    }
}