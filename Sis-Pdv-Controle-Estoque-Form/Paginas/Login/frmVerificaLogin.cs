using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    public partial class frmVerificaLogin : Form
    {
        #region Campos Privados
        
        private ColaboradorService _colaboradorService;
        private bool _isLoading = false;
        private bool _isValid = false;
        private bool _validador = false;
        private const string PLACEHOLDER_USER = "Digite o usuário administrativo...";
        private const string PLACEHOLDER_PASS = "Digite a senha...";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmVerificaLogin()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Inicializa serviços
            _colaboradorService = new ColaboradorService();
            
            // Configura placeholders iniciais
            ConfigurarPlaceholders();
            
            // Configura eventos de teclado
            this.KeyPreview = true;
            
            // Foco inicial no campo usuário
            txbUsuario.Focus();
            
            // Log de inicialização
            VerifyLoginLogger.LogInfo("Sistema de verificação de login inicializado", "Startup");
        }
        
        private void ConfigurarPlaceholders()
        {
            // Usuário
            if (string.IsNullOrWhiteSpace(txbUsuario.Text))
            {
                txbUsuario.Text = PLACEHOLDER_USER;
                txbUsuario.ForeColor = Color.FromArgb(149, 165, 166);
            }
            
            // Senha - mantém sem placeholder por segurança
            txbSenha.UseSystemPasswordChar = true;
            
            AtualizarStatusInterface();
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public bool Validador 
        { 
            get { return _validador; } 
            set { _validador = value; }
        }
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        private async void btnOk_Click(object sender, EventArgs e)
        {
            await ProcessarVerificacao();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        #endregion
        
        #region Eventos dos Campos de Entrada
        
        private void txbUsuario_Enter(object sender, EventArgs e)
        {
            if (txbUsuario.Text == PLACEHOLDER_USER)
            {
                txbUsuario.Text = "";
                txbUsuario.ForeColor = Color.FromArgb(52, 73, 94);
                lblUsuarioIcon.Text = "✏️";
                lblUsuarioIcon.ForeColor = Color.FromArgb(52, 152, 219);
            }
        }
        
        private void txbUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUsuario.Text))
            {
                txbUsuario.Text = PLACEHOLDER_USER;
                txbUsuario.ForeColor = Color.FromArgb(149, 165, 166);
                lblUsuarioIcon.Text = "👤";
                lblUsuarioIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
            ValidarCampos();
        }
        
        private void txbUsuario_TextChanged(object sender, EventArgs e)
        {
            if (txbUsuario.Text != PLACEHOLDER_USER)
            {
                ValidarCampos();
                AtualizarStatusInterface();
            }
        }
        
        private void txbSenha_Enter(object sender, EventArgs e)
        {
            lblSenhaIcon.Text = "🔓";
            lblSenhaIcon.ForeColor = Color.FromArgb(52, 152, 219);
        }
        
        private void txbSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbSenha.Text))
            {
                lblSenhaIcon.Text = "🔒";
                lblSenhaIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
            ValidarCampos();
        }
        
        private void txbSenha_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
            AtualizarStatusInterface();
        }
        
        private void txbSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter no campo senha = verificar acesso
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                _ = ProcessarVerificacao();
            }
        }
        
        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txbSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void frmVerificaLogin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    _ = ProcessarVerificacao();
                    break;
                case Keys.Escape:
                    FecharFormulario(false);
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
            }
        }
        
        private void frmVerificaLogin_Load(object sender, EventArgs e)
        {
            // Foco no campo de usuário
            txbUsuario.Focus();
            VerifyLoginLogger.LogInfo("Formulário de verificação carregado", "UserInterface");
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Validação e Processamento
        
        private void ValidarCampos()
        {
            var usuarioValido = !string.IsNullOrWhiteSpace(txbUsuario.Text) && 
                               txbUsuario.Text != PLACEHOLDER_USER &&
                               txbUsuario.Text.Length >= 3;
            
            var senhaValida = !string.IsNullOrWhiteSpace(txbSenha.Text) &&
                             txbSenha.Text.Length >= 4;
            
            _isValid = usuarioValido && senhaValida;
            
            // Atualiza ícones baseado na validação
            if (usuarioValido)
            {
                lblUsuarioIcon.Text = "✅";
                lblUsuarioIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
            else if (!string.IsNullOrWhiteSpace(txbUsuario.Text) && txbUsuario.Text != PLACEHOLDER_USER)
            {
                lblUsuarioIcon.Text = "⚠️";
                lblUsuarioIcon.ForeColor = Color.FromArgb(230, 126, 34);
            }
            
            if (senhaValida)
            {
                lblSenhaIcon.Text = "🔓";
                lblSenhaIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
            else if (!string.IsNullOrWhiteSpace(txbSenha.Text))
            {
                lblSenhaIcon.Text = "⚠️";
                lblSenhaIcon.ForeColor = Color.FromArgb(230, 126, 34);
            }
        }
        
        private async Task ProcessarVerificacao()
        {
            if (_isLoading) return;
            
            var sw = Stopwatch.StartNew();
            try
            {
                // Validações básicas
                if (!ValidarEntradas()) return;
                
                SetLoadingState(true);
                
                var usuario = txbUsuario.Text.Trim();
                var senha = txbSenha.Text;
                
                VerifyLoginLogger.LogInfo($"Tentativa de verificação para usuário: {usuario}", "Authentication");
                
                var response = await _colaboradorService.ValidarLogin(usuario, senha);
                
                sw.Stop();
                VerifyLoginLogger.LogApiCall("ValidarLogin", "POST", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    var cargo = response.data?.cargoColaborador ?? "";
                    
                    if (cargo.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                        cargo.Equals("Administrator", StringComparison.OrdinalIgnoreCase) ||
                        cargo.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    {
                        VerifyLoginLogger.LogInfo($"Acesso administrativo concedido para: {usuario} (Cargo: {cargo})", "Authorization");
                        
                        // Exibe feedback de sucesso
                        lblStatus.Text = "✅ Acesso administrativo confirmado!";
                        lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                        
                        // Pequeno delay para feedback visual
                        await Task.Delay(1000);
                        
                        FecharFormulario(true);
                    }
                    else
                    {
                        VerifyLoginLogger.LogWarning($"Acesso negado para usuário: {usuario} (Cargo: {cargo})", "Authorization");
                        ExibirErroAcesso("Usuário não possui autorização administrativa.\n\n" +
                                        $"Cargo atual: {cargo}\n" +
                                        "É necessário ser Administrador para esta operação.");
                    }
                }
                else
                {
                    VerifyLoginLogger.LogWarning($"Credenciais inválidas para usuário: {usuario}", "Authentication");
                    ExibirErroAcesso("Usuário ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                VerifyLoginLogger.LogError($"Erro durante verificação: {ex.Message}", "Authentication", ex);
                ExibirErroAcesso($"Erro ao conectar com o servidor:\n{ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarEntradas()
        {
            // Valida usuário
            if (string.IsNullOrWhiteSpace(txbUsuario.Text) || txbUsuario.Text == PLACEHOLDER_USER)
            {
                MessageBox.Show("Por favor, digite o usuário administrativo.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbUsuario.Focus();
                return false;
            }
            
            // Valida senha
            if (string.IsNullOrWhiteSpace(txbSenha.Text))
            {
                MessageBox.Show("Por favor, digite a senha.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbSenha.Focus();
                return false;
            }
            
            // Validações de formato
            if (txbUsuario.Text.Length < 3)
            {
                MessageBox.Show("O usuário deve ter pelo menos 3 caracteres.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbUsuario.Focus();
                return false;
            }
            
            if (txbSenha.Text.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbSenha.Focus();
                return false;
            }
            
            return true;
        }
        
        private void ExibirErroAcesso(string mensagem)
        {
            MessageBox.Show(mensagem, "❌ Acesso Negado",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            // Limpa apenas a senha por segurança
            txbSenha.Clear();
            chkMostrarSenha.Checked = false;
            
            // Foco no campo apropriado
            if (txbUsuario.Text == PLACEHOLDER_USER || string.IsNullOrWhiteSpace(txbUsuario.Text))
            {
                txbUsuario.Focus();
            }
            else
            {
                txbSenha.Focus();
            }
            
            ValidarCampos();
            AtualizarStatusInterface();
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Verificando credenciais...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txbUsuario.Enabled = false;
                txbSenha.Enabled = false;
                btnOk.Enabled = false;
                btnCancelar.Enabled = false;
                chkMostrarSenha.Enabled = false;
            }
            else
            {
                if (_isValid)
                {
                    lblStatus.Text = "🔐 Credenciais válidas - Pronto para verificar";
                    lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                    btnOk.BackColor = Color.FromArgb(46, 204, 113);
                }
                else
                {
                    lblStatus.Text = "🔓 Digite suas credenciais para acesso administrativo";
                    lblStatus.ForeColor = Color.White;
                    btnOk.BackColor = Color.FromArgb(149, 165, 166);
                }
                
                progressBar.Visible = false;
                
                // Habilita controles
                txbUsuario.Enabled = true;
                txbSenha.Enabled = true;
                btnOk.Enabled = true;
                btnCancelar.Enabled = true;
                chkMostrarSenha.Enabled = true;
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
        
        #endregion
        
        #region Métodos Auxiliares
        
        private void FecharFormulario(bool validador)
        {
            try
            {
                _validador = validador;
                
                var resultado = validador ? "confirmado com sucesso" : "cancelado";
                VerifyLoginLogger.LogInfo($"Verificação de acesso {resultado}", "UserAction");
                
                this.DialogResult = validador ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                VerifyLoginLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - VERIFICAÇÃO DE ACESSO\n\n" +
                       "🔐 PROPÓSITO:\n" +
                       "Este formulário verifica se você possui\n" +
                       "credenciais administrativas para executar\n" +
                       "operações sensíveis do sistema.\n\n" +
                       "👤 REQUISITOS:\n" +
                       "• Usuário: Login de administrador\n" +
                       "• Senha: Senha administrativa válida\n" +
                       "• Cargo: Deve ser 'Admin' ou 'Administrador'\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER - Verificar credenciais\n" +
                       "• ESC - Cancelar operação\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "🔒 SEGURANÇA:\n" +
                       "• Todas as tentativas são registradas\n" +
                       "• Apenas administradores têm acesso\n" +
                       "• Credenciais são validadas em tempo real";
            
            MessageBox.Show(ajuda, "Ajuda - Verificação de Acesso",
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
                    _ = ProcessarVerificacao();
                    return true;
                case Keys.Escape:
                    btnCancelar.Focus();
                    FecharFormulario(false);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class VerifyLoginLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [VerifyLogin-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Console.WriteLine($"[WARN] [VerifyLogin-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [VerifyLogin-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Console.WriteLine($"[API] [VerifyLogin-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}