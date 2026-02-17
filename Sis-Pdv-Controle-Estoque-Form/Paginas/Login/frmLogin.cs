using Sis_Pdv_Controle_Estoque_Form.Paginas.PDV;
using Sis_Pdv_Controle_Estoque_Form.Services.Auth;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    public partial class frmLogin : Form
    {
        #region Campos Privados
        
        private AuthApiService _authApiService;
        private bool _isLoading = false;
        private const string PLACEHOLDER_USER = "Digite seu usuário...";
        private const string PLACEHOLDER_PASS = "Digite sua senha...";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmLogin()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Configura placeholders iniciais
            ConfigurarPlaceholders();
            
            // Configura eventos de teclado
            this.KeyPreview = true;
            this.KeyDown += FrmLogin_KeyDown;
            
            // Foco inicial no campo usuário
            txtLogin.Focus();
            
            // Log de inicialização
            LoginLogger.LogInfo("Sistema de login inicializado", "Startup");
        }
        
        private void ConfigurarPlaceholders()
        {
            // Usuário
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                txtLogin.Text = PLACEHOLDER_USER;
                txtLogin.ForeColor = Color.Gray;
            }
            
            // Senha - não usa placeholder por questões de segurança
            txtSenha.UseSystemPasswordChar = true;
        }
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                
            if (resultado == DialogResult.Yes)
            {
                LoginLogger.LogInfo("Aplicação encerrada pelo usuário", "Shutdown");
                Application.Exit();
            }
        }
        
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        
        #endregion
        
        #region Eventos de Entrada/Saída dos Campos
        
        private void txtLogin_Enter(object sender, EventArgs e)
        {
            if (txtLogin.Text == PLACEHOLDER_USER)
            {
                txtLogin.Text = "";
                txtLogin.ForeColor = Color.FromArgb(52, 73, 94);
            }
        }
        
        private void txtLogin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                txtLogin.Text = PLACEHOLDER_USER;
                txtLogin.ForeColor = Color.Gray;
            }
        }
        
        private void txtSenha_Enter(object sender, EventArgs e)
        {
            // A senha sempre mantém UseSystemPasswordChar = true
            txtSenha.ForeColor = Color.FromArgb(52, 73, 94);
        }
        
        private void txtSenha_Leave(object sender, EventArgs e)
        {
            // Mantém a cor padrão quando sai do campo
        }
        
        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter no campo senha = fazer login
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                RealizarLogin();
            }
        }
        
        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }
        
        #endregion
        
        #region Eventos de Teclado
        
        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    RealizarLogin();
                    break;
                case Keys.Escape:
                    btnClose_Click(sender, e);
                    break;
                case Keys.F5:
                    LimparCampos();
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
            }
        }
        
        #endregion
        
        #region Autenticação
        
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            await RealizarLogin();
        }
        
        private async Task RealizarLogin()
        {
            if (_isLoading) return;
            
            var sw = Stopwatch.StartNew();
            try
            {
                // Validações básicas
                if (!ValidarCampos()) return;
                
                SetLoadingState(true);
                
                var usuario = txtLogin.Text.Trim();
                var senha = txtSenha.Text;
                
                LoginLogger.LogInfo($"Tentativa de login para usuário: {usuario}", "Authentication");
                
                _authApiService = new AuthApiService();
                var auth = await _authApiService.LoginAsync(usuario, senha);
                
                sw.Stop();
                LoginLogger.LogApiCall("LoginAsync", "POST", sw.Elapsed, auth != null);
                
                if (auth?.accessToken != null)
                {
                    // Salva token para requisições subsequentes
                    Services.Http.HttpClientManager.SetBearerToken(auth.accessToken);
                    
                    // Salva login se solicitado
                    if (chkLembrarLogin.Checked)
                    {
                        SalvarLoginLembrado(usuario);
                    }
                    
                    var roles = auth.user?.roles ?? new List<string>();
                    var nome = auth.user?.nome ?? auth.user?.login ?? usuario;
                    
                    LoginLogger.LogInfo($"Login realizado com sucesso para: {nome}", "Authentication");
                    LoginLogger.LogInfo($"Roles do usuário: {string.Join(", ", roles)}", "Authorization");
                    
                    // Roteamento baseado no perfil
                    RedirecionarUsuario(roles, nome);
                }
                else
                {
                    LoginLogger.LogWarning($"Falha na autenticação para usuário: {usuario}", "Authentication");
                    ExibirErroLogin("Usuário ou senha inválidos.");
                }
            }
            catch (HttpRequestException ex)
            {
                sw.Stop();
                LoginLogger.LogError($"Erro de conexão durante autenticação: {ex.Message}", "Authentication", ex);
                ExibirErroLogin($"Erro ao conectar com o servidor:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                sw.Stop();
                LoginLogger.LogError($"Erro durante autenticação: {ex.Message}", "Authentication", ex);
                ExibirErroLogin($"Erro de Autenticação:\n{ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarCampos()
        {
            // Valida usuário
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || txtLogin.Text == PLACEHOLDER_USER)
            {
                MessageBox.Show("Por favor, digite seu usuário.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return false;
            }
            
            // Valida senha
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, digite sua senha.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return false;
            }
            
            // Validações de formato
            if (txtLogin.Text.Length < 3)
            {
                MessageBox.Show("O usuário deve ter pelo menos 3 caracteres.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return false;
            }
            
            if (txtSenha.Text.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return false;
            }
            
            return true;
        }
        
        private void RedirecionarUsuario(List<string> roles, string nome)
        {
            try
            {
                if (roles.Contains("Administrator") || roles.Contains("Manager"))
                {
                    // Administradores e gerentes vão para o menu principal
                    var frmMenu = new frmMenu(nome);
                    frmMenu.Show();
                    frmMenu.FormClosed += LogOut;
                    this.Hide();
                    
                    LoginLogger.LogInfo($"Usuário redirecionado para Menu Principal: {nome}", "Navigation");
                }
                else if (roles.Contains("Cashier") || roles.Contains("CashSupervisor"))
                {
                    // Operadores de caixa vão direto para o PDV
                    var frmPdv = new frmTelaPdv(nome);
                    frmPdv.Show();
                    frmPdv.FormClosed += LogOut;
                    this.Hide();
                    
                    LoginLogger.LogInfo($"Usuário redirecionado para PDV: {nome}", "Navigation");
                }
                else
                {
                    LoginLogger.LogWarning($"Usuário sem perfil válido: {nome} - Roles: {string.Join(", ", roles)}", "Authorization");
                    MessageBox.Show(
                        "Usuário sem perfil de acesso válido.\n\n" +
                        "Entre em contato com o administrador do sistema.",
                        "Acesso Negado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LoginLogger.LogError($"Erro ao redirecionar usuário: {ex.Message}", "Navigation", ex);
                MessageBox.Show($"Erro ao abrir o sistema: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ExibirErroLogin(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro de Autenticação",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            // Limpa apenas a senha por segurança
            txtSenha.Clear();
            txtSenha.Focus();
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            
            if (loading)
            {
                lblStatusLogin.Text = "🔄 Autenticando...";
                lblStatusLogin.ForeColor = Color.Orange;
                progressLogin.Visible = true;
                progressLogin.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txtLogin.Enabled = false;
                txtSenha.Enabled = false;
                btnLogin.Enabled = false;
                btnLimpar.Enabled = false;
                chkMostrarSenha.Enabled = false;
                chkLembrarLogin.Enabled = false;
                
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                lblStatusLogin.Text = "🟢 Sistema pronto para autenticação";
                lblStatusLogin.ForeColor = Color.White;
                progressLogin.Visible = false;
                
                // Habilita controles
                txtLogin.Enabled = true;
                txtSenha.Enabled = true;
                btnLogin.Enabled = true;
                btnLimpar.Enabled = true;
                chkMostrarSenha.Enabled = true;
                chkLembrarLogin.Enabled = true;
                
                Cursor = Cursors.Default;
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        private void LimparCampos()
        {
            txtLogin.Clear();
            txtSenha.Clear();
            chkMostrarSenha.Checked = false;
            chkLembrarLogin.Checked = false;
            
            ConfigurarPlaceholders();
            txtLogin.Focus();
            
            LoginLogger.LogInfo("Campos de login limpos", "UserAction");
        }
        
        private void SalvarLoginLembrado(string usuario)
        {
            try
            {
                // Por enquanto apenas log - implementar persistência futuramente
                LoginLogger.LogInfo($"Login lembrado solicitado para: {usuario}", "UserPreference");
            }
            catch (Exception ex)
            {
                LoginLogger.LogWarning($"Erro ao salvar login lembrado: {ex.Message}", "UserPreference");
            }
        }
        
        private void CarregarLoginLembrado()
        {
            try
            {
                // Por enquanto apenas log - implementar carregamento futuramente
                LoginLogger.LogInfo("Tentativa de carregar login lembrado", "UserPreference");
            }
            catch (Exception ex)
            {
                LoginLogger.LogWarning($"Erro ao carregar login lembrado: {ex.Message}", "UserPreference");
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🔑 AJUDA DO SISTEMA DE LOGIN\n\n" +
                       "💡 ATALHOS DE TECLADO:\n" +
                       "Enter - Fazer login\n" +
                       "ESC - Sair do sistema\n" +
                       "F1 - Esta ajuda\n" +
                       "F5 - Limpar campos\n\n" +
                       "👤 PERFIS DE ACESSO:\n" +
                       "• Administrator/Manager → Menu Principal\n" +
                       "• Cashier/CashSupervisor → PDV Direto\n\n" +
                       "🔐 DICAS DE SEGURANÇA:\n" +
                       "• Use senhas com pelo menos 6 caracteres\n" +
                       "• Não compartilhe suas credenciais\n" +
                       "• Sempre faça logout ao sair";
            
            MessageBox.Show(ajuda, "Ajuda - Sistema de Login", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion
        
        #region Eventos de Form
        
        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o form clicando no header
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        private void LogOut(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Limpa token de autenticação se método existir
                try
                {
                    Services.Http.HttpClientManager.SetBearerToken("");
                }
                catch
                {
                    // Método não existe, ignorar
                }
                
                // Limpa campos sensíveis
                txtSenha.Clear();
                chkMostrarSenha.Checked = false;
                
                // Reexibe form de login
                this.Show();
                
                // Foco no campo apropriado
                if (chkLembrarLogin.Checked && !string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    txtSenha.Focus();
                }
                else
                {
                    txtLogin.Focus();
                }
                
                LoginLogger.LogInfo("Logout realizado, retornando à tela de login", "Authentication");
            }
            catch (Exception ex)
            {
                LoginLogger.LogError($"Erro durante logout: {ex.Message}", "Authentication", ex);
            }
        }
        
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Carrega login lembrado se existir
            CarregarLoginLembrado();
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class LoginLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}
