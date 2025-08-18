using FontAwesome.Sharp;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Paginas.PDV;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Produto;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas
{
    public partial class frmMenu : Form
    {
        #region Campos Privados
        
        private readonly string _nomeUsuario;
        private Form _formFilhoAtivo;
        private Button _botaoAtivo;
        private bool _menuCollapsed = false;
        private bool _isLoading = false;
        
        // Configurações do menu
        private const int MENU_WIDTH_EXPANDED = 260;
        private const int MENU_WIDTH_COLLAPSED = 70;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmMenu(string nomeUsuario)
        {
            InitializeComponent();
            _nomeUsuario = nomeUsuario ?? "Usuário";
            
            InicializarInterfaceModerna();
            MenuLogger.LogInfo($"Menu principal inicializado para usuário: {nomeUsuario}", "Startup");
        }
        
        private void InicializarInterfaceModerna()
        {
            // Remove bordas e configura form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            
            // Configura usuário logado
            lblUsuarioLogado.Text = $"👤 {_nomeUsuario}";
            
            // Inicia timer de relógio
            timerRelogio.Start();
            AtualizarDataHora();
            
            // Carrega página inicial
            AbrirModulo(new frmHome(), "🏠", "INÍCIO", "Página inicial com resumo do sistema");
            
            // Ativa botão home
            AtivarBotao(btnHome);
            
            MenuLogger.LogInfo("Interface moderna inicializada", "UI");
        }
        
        #endregion
        
        #region Gerenciamento de Módulos
        
        private void AbrirModulo(Form novoForm, string icone, string titulo, string descricao)
        {
            if (_isLoading) return;
            
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                // Fecha form anterior
                if (_formFilhoAtivo != null)
                {
                    _formFilhoAtivo.Close();
                    _formFilhoAtivo.Dispose();
                }
                
                // Configura novo form
                _formFilhoAtivo = novoForm;
                novoForm.TopLevel = false;
                novoForm.FormBorderStyle = FormBorderStyle.None;
                novoForm.Dock = DockStyle.Fill;
                
                // Limpa e adiciona ao painel
                pnForm.Controls.Clear();
                pnForm.Controls.Add(novoForm);
                novoForm.BringToFront();
                novoForm.Show();
                
                // Atualiza informações do módulo
                AtualizarInfoModulo(icone, titulo, descricao);
                
                sw.Stop();
                MenuLogger.LogPerformance($"Módulo '{titulo}' carregado", sw.Elapsed);
            }
            catch (Exception ex)
            {
                sw.Stop();
                MenuLogger.LogError($"Erro ao abrir módulo '{titulo}': {ex.Message}", "Navigation", ex);
                MessageBox.Show($"Erro ao abrir módulo: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void AtualizarInfoModulo(string icone, string titulo, string descricao)
        {
            iconModuloAtivo.Text = icone;
            lblModuloAtivo.Text = titulo;
            lblDescricaoModulo.Text = descricao;
            
            // Atualiza título da janela
            this.Text = $"Sistema PDV - {titulo}";
        }
        
        #endregion
        
        #region Ativação de Botões
        
        private void AtivarBotao(Button botao)
        {
            // Desativa botão anterior
            if (_botaoAtivo != null)
            {
                DesativarBotao(_botaoAtivo);
            }
            
            // Ativa novo botão
            _botaoAtivo = botao;
            
            if (botao != null)
            {
                // Aplica estilo ativo
                botao.BackColor = GetCorModulo(botao);
                botao.ForeColor = Color.White;
                botao.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                
                // Adiciona indicador visual lateral
                AdicionarIndicadorLateral(botao);
                
                MenuLogger.LogInfo($"Botão ativado: {botao.Text}", "Navigation");
            }
        }
        
        private void DesativarBotao(Button botao)
        {
            if (botao == null) return;
            
            botao.BackColor = Color.Transparent;
            botao.ForeColor = Color.White;
            botao.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        }
        
        private void AdicionarIndicadorLateral(Button botao)
        {
            // Remove indicadores anteriores
            foreach (Control control in pnMenuContainer.Controls)
            {
                if (control is Panel panel && panel.Name == "indicadorAtivo")
                {
                    pnMenuContainer.Controls.Remove(panel);
                    panel.Dispose();
                    break;
                }
            }
            
            // Cria novo indicador
            var indicador = new Panel
            {
                Name = "indicadorAtivo",
                BackColor = Color.White,
                Size = new Size(5, botao.Height),
                Location = new Point(0, botao.Location.Y)
            };
            
            pnMenuContainer.Controls.Add(indicador);
            indicador.BringToFront();
        }
        
        private Color GetCorModulo(Button botao)
        {
            // Cores modernas para cada módulo
            if (botao == btnHome) return Color.FromArgb(46, 204, 113);          // Verde
            if (botao == btnProdutos) return Color.FromArgb(52, 152, 219);      // Azul
            if (botao == btnColaboradores) return Color.FromArgb(155, 89, 182);  // Roxo
            if (botao == btnFornecedores) return Color.FromArgb(230, 126, 34);   // Laranja
            if (botao == btnCategorias) return Color.FromArgb(241, 196, 15);     // Amarelo
            if (botao == btnDepartamentos) return Color.FromArgb(149, 165, 166); // Cinza
            if (botao == btnPDV) return Color.FromArgb(231, 76, 60);            // Vermelho
            if (botao == btnRelatorios) return Color.FromArgb(52, 73, 94);       // Azul Escuro
            if (botao == btnConfiguracoes) return Color.FromArgb(127, 140, 141); // Cinza Escuro
            
            return Color.FromArgb(52, 152, 219); // Padrão azul
        }
        
        #endregion
        
        #region Eventos de Navegação
        
        private void btnHome_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnHome);
            AbrirModulo(new frmHome(), "🏠", "INÍCIO", "Página inicial com resumo do sistema");
        }
        
        private void btnProdutos_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnProdutos);
            AbrirModulo(new frmProduto(), "📦", "PRODUTOS", "Gerenciamento de produtos e estoque");
        }
        
        private void btnColaboradores_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnColaboradores);
            AbrirModulo(new frmColaborador(), "👥", "COLABORADORES", "Cadastro e gestão de colaboradores");
        }
        
        private void btnFornecedores_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnFornecedores);
            AbrirModulo(new CadFornecedor(), "🏭", "FORNECEDORES", "Cadastro e gestão de fornecedores");
        }
        
        private void btnCategorias_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnCategorias);
            AbrirModulo(new CadCategoria(), "🏷️", "CATEGORIAS", "Organização de produtos por categoria");
        }
        
        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnDepartamentos);
            AbrirModulo(new CadDepartamento(), "🏢", "DEPARTAMENTOS", "Estrutura organizacional da empresa");
        }
        
        private void btnPDV_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmacao = MessageBox.Show(
                    "Deseja abrir o Ponto de Venda?\n\nO menu principal será minimizado.",
                    "Abrir PDV",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (confirmacao == DialogResult.Yes)
                {
                    var frmPdv = new frmTelaPdv(_nomeUsuario);
                    frmPdv.Show();
                    frmPdv.FormClosed += PdvFormClosed;
                    this.Hide();
                    
                    MenuLogger.LogInfo($"PDV aberto para usuário: {_nomeUsuario}", "PDV");
                }
            }
            catch (Exception ex)
            {
                MenuLogger.LogError($"Erro ao abrir PDV: {ex.Message}", "PDV", ex);
                MessageBox.Show($"Erro ao abrir PDV: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnRelatorios);
            AbrirModulo(new frmRelatorio(), "📊", "RELATÓRIOS", "Análises e relatórios gerenciais");
        }
        
        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            AtivarBotao(btnConfiguracoes);
            MessageBox.Show("Módulo de Configurações em desenvolvimento", "Em Breve",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void PdvFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            MenuLogger.LogInfo("Retorno do PDV para menu principal", "Navigation");
        }
        
        #endregion
        
        #region Toggle Menu (Expandir/Colapsar)
        
        private void btnToggleMenu_Click(object sender, EventArgs e)
        {
            ToggleMenu();
        }
        
        private void ToggleMenu()
        {
            _menuCollapsed = !_menuCollapsed;
            
            if (_menuCollapsed)
            {
                // Colapsa menu
                pnMenuVertical.Width = MENU_WIDTH_COLLAPSED;
                lblNomeEmpresa.Visible = false;
                btnToggleMenu.Text = "☰";
                
                // Oculta textos dos botões
                OcultarTextoBotoes();
            }
            else
            {
                // Expande menu
                pnMenuVertical.Width = MENU_WIDTH_EXPANDED;
                lblNomeEmpresa.Visible = true;
                btnToggleMenu.Text = "✕";
                
                // Mostra textos dos botões
                MostrarTextoBotoes();
            }
            
            MenuLogger.LogInfo($"Menu {(_menuCollapsed ? "colapsado" : "expandido")}", "UI");
        }
        
        private void OcultarTextoBotoes()
        {
            var botoes = new[] {
                (btnHome, "🏠"),
                (btnProdutos, "📦"),
                (btnColaboradores, "👥"),
                (btnFornecedores, "🏭"),
                (btnCategorias, "🏷️"),
                (btnDepartamentos, "🏢"),
                (btnPDV, "🛒"),
                (btnRelatorios, "📊"),
                (btnConfiguracoes, "⚙️"),
                (btnLogout, "🚪")
            };
            
            foreach (var (botao, icone) in botoes)
            {
                botao.Text = icone;
                botao.TextAlign = ContentAlignment.MiddleCenter;
                botao.Padding = new Padding(0);
            }
        }
        
        private void MostrarTextoBotoes()
        {
            var botoes = new[] {
                (btnHome, "🏠 INÍCIO"),
                (btnProdutos, "📦 PRODUTOS"),
                (btnColaboradores, "👥 COLABORADORES"),
                (btnFornecedores, "🏭 FORNECEDORES"),
                (btnCategorias, "🏷️ CATEGORIAS"),
                (btnDepartamentos, "🏢 DEPARTAMENTOS"),
                (btnPDV, "🛒 PONTO DE VENDA"),
                (btnRelatorios, "📊 RELATÓRIOS"),
                (btnConfiguracoes, "⚙️ CONFIGURAÇÕES"),
                (btnLogout, "🚪 SAIR")
            };
            
            foreach (var (botao, texto) in botoes)
            {
                botao.Text = texto;
                botao.TextAlign = ContentAlignment.MiddleLeft;
                botao.Padding = new Padding(15, 0, 0, 0);
            }
        }
        
        #endregion
        
        #region Controles da Janela
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            if (confirmacao == DialogResult.Yes)
            {
                MenuLogger.LogInfo($"Sistema encerrado pelo usuário: {_nomeUsuario}", "Shutdown");
                Application.Exit();
            }
        }
        
        private void btnMax_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                btnMaximizar.Text = "🗗";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                btnMaximizar.Text = "⬜";
            }
        }
        
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show(
                $"Deseja fazer logout?\n\nUsuário: {_nomeUsuario}",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            if (confirmacao == DialogResult.Yes)
            {
                MenuLogger.LogInfo($"Logout realizado para usuário: {_nomeUsuario}", "Authentication");
                this.Close();
            }
        }
        
        #endregion
        
        #region Movimentação da Janela
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Timer e Status
        
        private void timerRelogio_Tick(object sender, EventArgs e)
        {
            AtualizarDataHora();
        }
        
        private void AtualizarDataHora()
        {
            var agora = DateTime.Now;
            lblDataHora.Text = $"📅 {agora:dd/MM/yyyy} - {agora:HH:mm:ss}";
        }
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            
            if (loading)
            {
                lblStatusSistema.Text = "🔄 Carregando módulo...";
                lblStatusSistema.ForeColor = Color.Orange;
                progressStatus.Visible = true;
                progressStatus.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                lblStatusSistema.Text = "🟢 Sistema operacional";
                lblStatusSistema.ForeColor = Color.White;
                progressStatus.Visible = false;
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
                    case Keys.F1:
                        btnHome_Click(null, null);
                        return true;
                    case Keys.F2:
                        btnProdutos_Click(null, null);
                        return true;
                    case Keys.F3:
                        btnColaboradores_Click(null, null);
                        return true;
                    case Keys.F4:
                        btnFornecedores_Click(null, null);
                        return true;
                    case Keys.F5:
                        btnPDV_Click(null, null);
                        return true;
                    case Keys.F9:
                        btnRelatorios_Click(null, null);
                        return true;
                    case Keys.F11:
                        ToggleMenu();
                        return true;
                    case Keys.F12:
                        btnMax_Click(null, null);
                        return true;
                    case Keys.Escape:
                        btnLogout_Click(null, null);
                        return true;
                    case Keys.Alt | Keys.F4:
                        btnClose_Click(null, null);
                        return true;
                }
            }
            catch (Exception ex)
            {
                MenuLogger.LogError($"Erro em atalho de teclado: {ex.Message}", "Keyboard", ex);
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        #endregion
        
        #region Limpeza de Recursos
        
        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Para timer
                timerRelogio?.Stop();
                timerRelogio?.Dispose();
                
                // Fecha form filho se existir
                if (_formFilhoAtivo != null)
                {
                    _formFilhoAtivo.Close();
                    _formFilhoAtivo.Dispose();
                }
                
                MenuLogger.LogInfo($"Menu principal fechado para usuário: {_nomeUsuario}", "Shutdown");
            }
            catch (Exception ex)
            {
                MenuLogger.LogError($"Erro ao fechar menu: {ex.Message}", "Shutdown", ex);
            }
        }
        
        #endregion
        
        #region Métodos de Compatibilidade (Manter para não quebrar código existente)
        
        // Eventos mantidos para compatibilidade
        private void btnDashbord_Click(object sender, EventArgs e) => btnHome_Click(sender, e);
        private void btnProduto_Click_1(object sender, EventArgs e) => btnProdutos_Click(sender, e);
        private void btnColaborador_Click(object sender, EventArgs e) => btnColaboradores_Click(sender, e);
        private void btnFornecedor_Click(object sender, EventArgs e) => btnFornecedores_Click(sender, e);
        private void iconButton1_Click(object sender, EventArgs e) => btnCategorias_Click(sender, e);
        private void imgDepartamento_Click(object sender, EventArgs e) => btnDepartamentos_Click(sender, e);
        private void iconButton2_Click(object sender, EventArgs e) => btnPDV_Click(sender, e);
        private void btnRelatorios_Click_1(object sender, EventArgs e) => btnRelatorios_Click(sender, e);
        private void btnLogOut_Click(object sender, EventArgs e) => btnLogout_Click(sender, e);
        
        // Métodos legados vazios para compatibilidade
        private void btnLogo_Click(object sender, EventArgs e) { /* Compatibilidade */ }
        private void iconMenu_Click(object sender, EventArgs e) { ToggleMenu(); }
        private void pnForm_Paint(object sender, PaintEventArgs e) { /* Compatibilidade */ }
        private void frmMenu_Paint(object sender, PaintEventArgs e) { /* Compatibilidade */ }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class MenuLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [Menu-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [Menu-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogPerformance(string operation, TimeSpan duration)
            {
                Console.WriteLine($"[PERF] [Menu] {operation} - {duration.TotalMilliseconds}ms");
            }
        }
        
        #endregion
    }
}