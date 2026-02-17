using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class AltDepartamento : Form
    {
        #region Campos Privados
        
        private DepartamentoService _departamentoService;
        private bool _isLoading = false;
        private bool _isValid = false;
        private string _nomeOriginal = "";
        private string _departamentoId = "";
        private bool _dadosAlterados = false;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public AltDepartamento(string NomeDepartamento, string id)
        {
            InitializeComponent();
            InicializarComponentesModernos(NomeDepartamento, id);
        }
        
        private void InicializarComponentesModernos(string NomeDepartamento, string id)
        {
            // Inicializa serviços
            _departamentoService = new DepartamentoService();
            
            // Configura dados iniciais
            txtNomeDepartamento.Text = NomeDepartamento ?? "";
            LblId.Text = id ?? "";
            _nomeOriginal = NomeDepartamento ?? "";
            _departamentoId = id ?? "";
            
            // Configura estado inicial
            ValidarCampo();
            AtualizarStatusInterface();
            
            // Log de inicialização
            AltDepartamentoLogger.LogInfo($"Formulário de alteração inicializado - ID: {id}, Nome: {NomeDepartamento}", "Startup");
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
        
        private void txtNomeDepartamento_Enter(object sender, EventArgs e)
        {
            lblInputIcon.Text = "✏️";
            lblInputIcon.ForeColor = Color.FromArgb(52, 152, 219);
            pnInput.BackColor = Color.FromArgb(232, 245, 255); // Azul muito claro
        }
        
        private void txtNomeDepartamento_Leave(object sender, EventArgs e)
        {
            // Remove espaços extras
            txtNomeDepartamento.Text = txtNomeDepartamento.Text?.Trim() ?? "";
            
            ValidarCampo();
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            VerificarAlteracoes();
        }
        
        private void txtNomeDepartamento_TextChanged(object sender, EventArgs e)
        {
            ValidarCampo();
            VerificarAlteracoes();
            AtualizarStatusInterface();
        }
        
        private void txtNomeDepartamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionou Enter, confirma alteração
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                _ = ProcessarAlteracao();
                return;
            }
            
            // Permite backspace, delete e espaço
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete || e.KeyChar == ' ')
                return;
            
            // Permite apenas letras, números e alguns caracteres especiais para departamentos
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '_')
            {
                e.Handled = true;
                ExibirFeedbackEntradaInvalida();
            }
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void AltDepartamento_KeyDown(object sender, KeyEventArgs e)
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
        
        private void AltDepartamento_Load(object sender, EventArgs e)
        {
            try
            {
                // Foco no campo de entrada e seleciona todo o texto
                txtNomeDepartamento.Focus();
                txtNomeDepartamento.SelectAll();
                
                AltDepartamentoLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                AltDepartamentoLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
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
            var nome = txtNomeDepartamento.Text.Trim();
            
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
            else if (nome.Length > 150)
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
            var nomeAtual = txtNomeDepartamento.Text.Trim();
            _dadosAlterados = !string.Equals(nomeAtual, _nomeOriginal, StringComparison.OrdinalIgnoreCase);
        }
        
        private async Task ProcessarAlteracao()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados()) return;
                
                if (!await ConfirmarAlteracao()) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeDepartamento.Text.Trim();
                
                // Verifica se já existe outro departamento com o mesmo nome
                if (await DepartamentoJaExiste(nome))
                {
                    ExibirErro("Departamento Duplicado", "Já existe um departamento com este nome.");
                    return;
                }
                
                var dto = new DepartamentoDto
                {
                    Id = _departamentoId,
                    NomeDepartamento = nome
                };
                
                AltDepartamentoLogger.LogInfo($"Iniciando alteração do departamento: ID={_departamentoId}, Nome Antigo={_nomeOriginal}, Nome Novo={nome}", "Update");
                
                var response = await _departamentoService.AlterarDepartamento(dto);
                
                sw.Stop();
                AltDepartamentoLogger.LogApiCall("AlterarDepartamento", "PUT", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    NovoNome = response.data.NomeDepartamento;
                    AlteracaoRealizada = true;
                    
                    ExibirSucesso($"Departamento alterado para '{NovoNome}' com sucesso!");
                    
                    AltDepartamentoLogger.LogInfo($"Departamento alterado com sucesso: ID={response.data.id}, Novo Nome={NovoNome}", "Update");
                    
                    // Pequeno delay para feedback visual
                    await Task.Delay(1000);
                    
                    FecharFormulario(true);
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao alterar departamento.";
                    ExibirErro("Erro na Alteração", erro);
                    AltDepartamentoLogger.LogWarning($"Falha na alteração: {erro}", "Update");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                AltDepartamentoLogger.LogError($"Erro ao alterar departamento: {ex.Message}", "Update", ex);
                ExibirErro("Erro Inesperado", $"Erro ao alterar departamento: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ValidarDados()
        {
            var nome = txtNomeDepartamento.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nome))
            {
                ExibirAviso("Digite o nome do departamento.");
                txtNomeDepartamento.Focus();
                return false;
            }
            
            if (nome.Length < 2)
            {
                ExibirAviso("O nome do departamento deve ter pelo menos 2 caracteres.");
                txtNomeDepartamento.Focus();
                txtNomeDepartamento.SelectAll();
                return false;
            }
            
            if (nome.Length > 150)
            {
                ExibirAviso("O nome do departamento não pode ter mais de 150 caracteres.");
                txtNomeDepartamento.Focus();
                txtNomeDepartamento.SelectAll();
                return false;
            }
            
            if (!_dadosAlterados)
            {
                ExibirAviso("Nenhuma alteração foi feita no nome do departamento.");
                txtNomeDepartamento.Focus();
                txtNomeDepartamento.SelectAll();
                return false;
            }
            
            return true;
        }
        
        private async Task<bool> ConfirmarAlteracao()
        {
            var nomeNovo = txtNomeDepartamento.Text.Trim();
            
            var confirmacao = MessageBox.Show(
                $"💼 CONFIRMAR ALTERAÇÃO DE DEPARTAMENTO\n\n" +
                $"Nome Atual: {_nomeOriginal}\n" +
                $"Nome Novo: {nomeNovo}\n\n" +
                $"Deseja confirmar esta alteração?",
                "Confirmar Alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            return confirmacao == DialogResult.Yes;
        }
        
        private async Task<bool> DepartamentoJaExiste(string nome)
        {
            try
            {
                var response = await _departamentoService.ListarDepartamentoPorNomeDepartamento(nome);
                return response.success && response.data != null && 
                       response.data.Any(d => d.id != _departamentoId); // Exclui o próprio departamento
            }
            catch (Exception ex)
            {
                AltDepartamentoLogger.LogError($"Erro ao verificar duplicação: {ex.Message}", "Validation", ex);
                return false; // Em caso de erro, assume que não existe
            }
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
                lblStatus.Text = "🔄 Salvando alterações...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                txtNomeDepartamento.Enabled = false;
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
                    btnAlterar.BackColor = Color.FromArgb(52, 152, 219); // Azul padrão
                    btnAlterar.Text = "💾 Salvar (ENTER)";
                    btnAlterar.Enabled = true;
                }
                
                // Habilita controles
                txtNomeDepartamento.Enabled = true;
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
                AltDepartamentoLogger.LogInfo($"Alteração de departamento {acao}", "UserAction");
                
                this.DialogResult = sucesso ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                AltDepartamentoLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - ALTERAR DEPARTAMENTO\n\n" +
                       "📝 COMO USAR:\n" +
                       "• Modifique o nome do departamento no campo\n" +
                       "• Clique em 'Salvar' ou pressione ENTER\n" +
                       "• Use 'Cancelar' ou ESC para desistir\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER - Salvar alterações\n" +
                       "• ESC - Cancelar operação\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "📏 VALIDAÇÕES:\n" +
                       "• Nome deve ter entre 2 e 150 caracteres\n" +
                       "• Nome deve ser diferente do original\n" +
                       "• Não pode duplicar nome existente\n" +
                       "• Permite letras, números, hífen e sublinhado\n\n" +
                       "💡 DICAS:\n" +
                       "• O sistema verifica duplicação automaticamente\n" +
                       "• Ícones indicam o status da validação\n" +
                       "• O botão só fica ativo com dados válidos\n" +
                       "• Alterações são confirmadas antes de salvar";
            
            MessageBox.Show(ajuda, "Ajuda - Alterar Departamento",
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
        
        private void TxtNomeDepartamento_TextChanged(object sender, EventArgs e)
        {
            txtNomeDepartamento_TextChanged(sender, e);
        }
        
        private void ValidarFormulario()
        {
            ValidarCampo();
            VerificarAlteracoes();
            AtualizarStatusInterface();
        }
        
        private bool ValidarCampos()
        {
            return ValidarDados();
        }
        
        private void AltDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLoading)
            {
                e.Cancel = true;
                return;
            }
            
            // Validação de fechamento já é feita em FecharFormulario()
        }
        
        private void AltDepartamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Limpeza se necessária
        }
        
        private async Task AtualizarFormularioPrincipal()
        {
            try
            {
                var formPrincipal = Application.OpenForms.OfType<CadDepartamento>().FirstOrDefault();
                if (formPrincipal != null)
                {
                    await formPrincipal.Consultar();
                }
            }
            catch (Exception ex)
            {
                AltDepartamentoLogger.LogError($"Erro ao atualizar formulário principal: {ex.Message}", "Integration", ex);
            }
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class AltDepartamentoLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [AltDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [AltDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [AltDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [AltDepartamento-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}
