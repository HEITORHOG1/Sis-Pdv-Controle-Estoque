using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class AltCategoria : Form
    {
        #region Campos Privados
        
        private CategoriaService _categoriaService;
        private bool _isLoading = false;
        private bool _isValid = false;
        private string _nomeOriginal = "";
        private bool _dadosAlterados = false;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public AltCategoria(string nomeCategoria, string id)
        {
            InitializeComponent();
            InicializarComponentesModernos(nomeCategoria, id);
        }
        
        private void InicializarComponentesModernos(string nomeCategoria, string id)
        {
            // Inicializa serviços
            _categoriaService = new CategoriaService();
            
            // Configura dados iniciais
            txtNomeCategoria.Text = nomeCategoria ?? "";
            LblId.Text = id ?? "";
            _nomeOriginal = nomeCategoria ?? "";
            
            // Configura estado inicial
            ValidarCampo();
            AtualizarStatusInterface();
            
            // Log de inicialização
            AltCategoriaLogger.LogInfo($"Formulário de alteração inicializado - ID: {id}, Nome: {nomeCategoria}", "Startup");
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public bool AlteracaoRealizada { get; private set; } = false;
        public string NovoNome { get; private set; } = "";
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await ProcessarAlteracao();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        #endregion
        
        #region Eventos do Campo de Entrada
        
        private void txtNomeCategoria_Enter(object sender, EventArgs e)
        {
            lblInputIcon.Text = "✏️";
            lblInputIcon.ForeColor = Color.FromArgb(230, 126, 34);
            pnInput.BackColor = Color.FromArgb(255, 244, 220); // Laranja muito claro
        }
        
        private void txtNomeCategoria_Leave(object sender, EventArgs e)
        {
            ValidarCampo();
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            VerificarAlteracoes();
        }
        
        private void txtNomeCategoria_TextChanged(object sender, EventArgs e)
        {
            ValidarCampo();
            VerificarAlteracoes();
            AtualizarStatusInterface();
        }
        
        private void txtNomeCategoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionou Enter, confirma alteração
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                _ = ProcessarAlteracao();
            }
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void AltCategoria_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    _ = ProcessarAlteracao();
                    break;
                case Keys.Escape:
                    FecharFormulario(false);
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
            }
        }
        
        private void AltCategoria_Load(object sender, EventArgs e)
        {
            try
            {
                // Foco no campo de entrada e seleciona todo o texto
                txtNomeCategoria.Focus();
                txtNomeCategoria.SelectAll();
                
                AltCategoriaLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                AltCategoriaLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
            }
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Validação e Processamento
        
        private void ValidarCampo()
        {
            var nome = txtNomeCategoria.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nome))
            {
                _isValid = false;
                lblInputIcon.Text = "⚠️";
                lblInputIcon.ForeColor = Color.FromArgb(231, 76, 60);
            }
            else if (nome.Length < 2)
            {
                _isValid = false;
                lblInputIcon.Text = "⚠️";
                lblInputIcon.ForeColor = Color.FromArgb(230, 126, 34);
            }
            else if (nome.Length > 100)
            {
                _isValid = false;
                lblInputIcon.Text = "❌";
                lblInputIcon.ForeColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                _isValid = true;
                lblInputIcon.Text = "✅";
                lblInputIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
        }
        
        private void VerificarAlteracoes()
        {
            var nomeAtual = txtNomeCategoria.Text.Trim();
            _dadosAlterados = !string.Equals(nomeAtual, _nomeOriginal, StringComparison.OrdinalIgnoreCase);
        }
        
        private async Task ProcessarAlteracao()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados()) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeCategoria.Text.Trim();
                var dto = new CategoriaDto
                {
                    id = Guid.Parse(LblId.Text),
                    NomeCategoria = nome
                };
                
                AltCategoriaLogger.LogInfo($"Iniciando alteração da categoria: ID={LblId.Text}, Nome Antigo={_nomeOriginal}, Nome Novo={nome}", "Update");
                
                var response = await _categoriaService.AlterarCategoria(dto);
                
                sw.Stop();
                AltCategoriaLogger.LogApiCall("AlterarCategoria", "PUT", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    NovoNome = response.data.NomeCategoria;
                    AlteracaoRealizada = true;
                    
                    ExibirSucesso($"Categoria alterada para '{NovoNome}' com sucesso!");
                    
                    AltCategoriaLogger.LogInfo($"Categoria alterada com sucesso: ID={response.data.id}, Novo Nome={NovoNome}", "Update");
                    
                    // Pequeno delay para feedback visual
                    await Task.Delay(1000);
                    
                    FecharFormulario(true);
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao alterar categoria.";
                    ExibirErro("Erro na Alteração", erro);
                    AltCategoriaLogger.LogWarning($"Falha na alteração: {erro}", "Update");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                AltCategoriaLogger.LogError($"Erro ao alterar categoria: {ex.Message}", "Update", ex);
                ExibirErro("Erro Inesperado", $"Erro ao alterar categoria: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarDados()
        {
            var nome = txtNomeCategoria.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nome))
            {
                ExibirAviso("Digite o nome da categoria.");
                txtNomeCategoria.Focus();
                return false;
            }
            
            if (nome.Length < 2)
            {
                ExibirAviso("O nome da categoria deve ter pelo menos 2 caracteres.");
                txtNomeCategoria.Focus();
                txtNomeCategoria.SelectAll();
                return false;
            }
            
            if (nome.Length > 100)
            {
                ExibirAviso("O nome da categoria não pode ter mais de 100 caracteres.");
                txtNomeCategoria.Focus();
                txtNomeCategoria.SelectAll();
                return false;
            }
            
            if (!_dadosAlterados)
            {
                ExibirAviso("Nenhuma alteração foi feita no nome da categoria.");
                txtNomeCategoria.Focus();
                txtNomeCategoria.SelectAll();
                return false;
            }
            
            return true;
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Salvando alterações...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txtNomeCategoria.Enabled = false;
                btnAlterar.Enabled = false;
                btnCancelar.Enabled = false;
            }
            else
            {
                progressBar.Visible = false;
                lblStatus.ForeColor = Color.White;
                
                if (_isValid && _dadosAlterados)
                {
                    lblStatus.Text = "✅ Pronto para salvar as alterações";
                    btnAlterar.BackColor = Color.FromArgb(46, 204, 113); // Verde para salvar
                    btnAlterar.Text = "💾 Salvar (ENTER)";
                    btnAlterar.Enabled = true;
                }
                else if (!_isValid)
                {
                    lblStatus.Text = "⚠️ Nome inválido - Verifique o campo";
                    btnAlterar.BackColor = Color.FromArgb(149, 165, 166); // Cinza desabilitado
                    btnAlterar.Text = "💾 Salvar (ENTER)";
                    btnAlterar.Enabled = false;
                }
                else if (!_dadosAlterados)
                {
                    lblStatus.Text = "📝 Modifique o nome para habilitar a alteração";
                    btnAlterar.BackColor = Color.FromArgb(149, 165, 166); // Cinza desabilitado
                    btnAlterar.Text = "💾 Salvar (ENTER)";
                    btnAlterar.Enabled = false;
                }
                else
                {
                    lblStatus.Text = "✏️ Modifique o nome e clique em 'Salvar'";
                    btnAlterar.BackColor = Color.FromArgb(230, 126, 34); // Laranja padrão
                    btnAlterar.Text = "💾 Salvar (ENTER)";
                    btnAlterar.Enabled = true;
                }
                
                // Habilita controles
                txtNomeCategoria.Enabled = true;
                btnCancelar.Enabled = true;
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
        
        private void FecharFormulario(bool sucesso)
        {
            try
            {
                if (!sucesso && _dadosAlterados)
                {
                    var resultado = MessageBox.Show(
                        "Há alterações não salvas.\n\nDeseja realmente cancelar?",
                        "Confirmação",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    if (resultado == DialogResult.No) return;
                }
                
                var acao = sucesso ? "confirmada" : "cancelada";
                AltCategoriaLogger.LogInfo($"Alteração de categoria {acao}", "UserAction");
                
                this.DialogResult = sucesso ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                AltCategoriaLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - ALTERAR CATEGORIA\n\n" +
                       "📝 COMO USAR:\n" +
                       "• Modifique o nome da categoria no campo\n" +
                       "• Clique em 'Salvar' ou pressione ENTER\n" +
                       "• Use 'Cancelar' ou ESC para desistir\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER - Salvar alterações\n" +
                       "• ESC - Cancelar operação\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "📏 VALIDAÇÕES:\n" +
                       "• Nome deve ter entre 2 e 100 caracteres\n" +
                       "• Nome deve ser diferente do original\n" +
                       "• Campo é obrigatório\n\n" +
                       "💡 DICAS:\n" +
                       "• O sistema confirma se há alterações não salvas\n" +
                       "• Ícones indicam o status da validação\n" +
                       "• O botão só fica ativo com dados válidos";
            
            MessageBox.Show(ajuda, "Ajuda - Alterar Categoria",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirSucesso(string mensagem)
        {
            MessageBox.Show(mensagem, "✅ Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirErro(string titulo, string mensagem)
        {
            MessageBox.Show(mensagem, $"❌ {titulo}",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void ExibirAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "⚠️ Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        #endregion
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantido para compatibilidade com código existente
        private async Task Alterar()
        {
            await ProcessarAlteracao();
        }
        
        private void AltCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Validação de fechamento já é feita em FecharFormulario()
        }
        
        private void AltCategoria_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Limpeza se necessária
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class AltCategoriaLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [AltCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Console.WriteLine($"[WARN] [AltCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [AltCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Console.WriteLine($"[API] [AltCategoria-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}
