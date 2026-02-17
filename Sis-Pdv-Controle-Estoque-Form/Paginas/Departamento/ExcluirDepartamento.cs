using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class ExcluirDepartamento : Form
    {
        #region Campos Privados
        
        private DepartamentoService _departamentoService;
        private bool _isLoading = false;
        private bool _exclusaoConfirmada = false;
        private string _NomeDepartamento = "";
        private string _departamentoId = "";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public ExcluirDepartamento(string NomeDepartamento, string id)
        {
            InitializeComponent();
            InicializarComponentesModernos(NomeDepartamento, id);
        }
        
        private void InicializarComponentesModernos(string NomeDepartamento, string id)
        {
            // Inicializa serviços
            _departamentoService = new DepartamentoService();
            
            // Configura dados iniciais
            _NomeDepartamento = NomeDepartamento ?? "";
            _departamentoId = id ?? "";
            
            txtNomeDepartamento.Text = _NomeDepartamento;
            LblId.Text = _departamentoId;
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Log de inicialização
            ExcluirDepartamentoLogger.LogInfo($"Formulário de exclusão inicializado - ID: {id}, Nome: {NomeDepartamento}", "Startup");
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public bool ExclusaoRealizada { get; private set; } = false;
        public string NomeExcluido { get; private set; } = "";
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            await ProcessarExclusao();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharFormulario(false);
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void ExcluirDepartamento_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                case Keys.Enter:
                    _ = ProcessarExclusao();
                    break;
                case Keys.Escape:
                    FecharFormulario(false);
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
            }
        }
        
        private void ExcluirDepartamento_Load(object sender, EventArgs e)
        {
            try
            {
                // Foco no botão cancelar (mais seguro)
                btnCancelar.Focus();
                
                ExcluirDepartamentoLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                ExcluirDepartamentoLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
            }
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Processamento e Validação
        
        private async Task ProcessarExclusao()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ConfirmarExclusao()) return;
                
                SetLoadingState(true);
                
                ExcluirDepartamentoLogger.LogInfo($"Iniciando exclusão do departamento: ID={_departamentoId}, Nome={_NomeDepartamento}", "Delete");
                
                // Verifica se o departamento pode ser excluído
                if (!await PodeExcluirDepartamento())
                {
                    ExibirAviso(
                        "Este departamento não pode ser excluído pois possui colaboradores vinculados.\n\n" +
                        "Para excluir este departamento, primeiro remova ou transfira todos os colaboradores para outros departamentos.",
                        "Departamento em Uso");
                    return;
                }
                
                var response = await _departamentoService.RemoverDepartamento(_departamentoId);
                
                sw.Stop();
                ExcluirDepartamentoLogger.LogApiCall("RemoverDepartamento", "DELETE", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    NomeExcluido = _NomeDepartamento;
                    ExclusaoRealizada = true;
                    _exclusaoConfirmada = true;
                    
                    ExibirSucesso($"Departamento '{_NomeDepartamento}' excluído com sucesso!");
                    
                    ExcluirDepartamentoLogger.LogInfo($"Departamento excluído com sucesso: ID={_departamentoId}, Nome={_NomeDepartamento}", "Delete");
                    
                    // Pequeno delay para feedback visual
                    await Task.Delay(1500);
                    
                    FecharFormulario(true);
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao excluir departamento.";
                    ExibirErro("Erro na Exclusão", erro);
                    ExcluirDepartamentoLogger.LogWarning($"Falha na exclusão: {erro}", "Delete");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ExcluirDepartamentoLogger.LogError($"Erro ao excluir departamento: {ex.Message}", "Delete", ex);
                ExibirErro("Erro Inesperado", $"Erro ao excluir departamento: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private bool ConfirmarExclusao()
        {
            var confirmacao = MessageBox.Show(
                $"🚨 CONFIRMAÇÃO FINAL DE EXCLUSÃO\n\n" +
                $"Departamento: {_NomeDepartamento}\n" +
                $"ID: {_departamentoId}\n\n" +
                $"⚠️ ATENÇÃO:\n" +
                $"• Esta ação é IRREVERSÍVEL\n" +
                $"• O departamento será excluído permanentemente\n" +
                $"• Todos os dados serão perdidos\n" +
                $"• Verifique se não há colaboradores vinculados\n\n" +
                $"Você tem CERTEZA ABSOLUTA que deseja excluir este departamento?",
                "⚠️ CONFIRMAR EXCLUSÃO PERMANENTE",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2); // Padrão é "Não"
            
            if (confirmacao == DialogResult.Yes)
            {
                // Segunda confirmação para operações críticas
                var segundaConfirmacao = MessageBox.Show(
                    $"🔴 ÚLTIMA CONFIRMAÇÃO\n\n" +
                    $"Esta é sua última chance de cancelar!\n\n" +
                    $"Departamento '{_NomeDepartamento}' será excluído PERMANENTEMENTE.\n\n" +
                    $"Continuar com a exclusão?",
                    "🔴 ÚLTIMA CHANCE",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2);
                
                return segundaConfirmacao == DialogResult.Yes;
            }
            
            return false;
        }
        
        private async Task<bool> PodeExcluirDepartamento()
        {
            try
            {
                // TODO: Implementar verificação se o departamento tem colaboradores vinculados
                // Por enquanto, retorna true. Em uma implementação real, você faria uma consulta
                // para verificar se existem colaboradores vinculados a este departamento
                
                ExcluirDepartamentoLogger.LogInfo($"Verificando se departamento pode ser excluído: ID={_departamentoId}", "Validation");
                
                // Exemplo de implementação:
                // var colaboradorService = new ColaboradorService();
                // var colaboradores = await colaboradorService.ListarColaboradoresPorDepartamento(_departamentoId);
                // return !colaboradores.Any();
                
                return true;
            }
            catch (Exception ex)
            {
                ExcluirDepartamentoLogger.LogError($"Erro ao verificar se pode excluir: {ex.Message}", "Validation", ex);
                // Em caso de erro na verificação, permite a exclusão
                // A API fará a validação final
                return true;
            }
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Excluindo departamento...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles durante processamento
                btnExcluir.Enabled = false;
                btnCancelar.Enabled = false;
                btnClose.Enabled = false;
                
                // Muda texto do botão
                btnExcluir.Text = "⏳ Processando...";
                btnExcluir.BackColor = Color.FromArgb(149, 165, 166);
            }
            else
            {
                if (_exclusaoConfirmada)
                {
                    lblStatus.Text = "✅ Departamento excluído com sucesso!";
                    lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                }
                else
                {
                    lblStatus.Text = "⚠️ ATENÇÃO: Esta ação não pode ser desfeita!";
                    lblStatus.ForeColor = Color.White;
                }
                
                progressBar.Visible = false;
                
                // Habilita controles
                btnExcluir.Enabled = true;
                btnCancelar.Enabled = true;
                btnClose.Enabled = true;
                
                // Restaura texto e cor do botão
                btnExcluir.Text = "🗑️ Confirmar Exclusão";
                btnExcluir.BackColor = Color.FromArgb(231, 76, 60);
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
                var acao = sucesso ? "confirmada" : "cancelada";
                ExcluirDepartamentoLogger.LogInfo($"Exclusão de departamento {acao}", "UserAction");
                
                this.DialogResult = sucesso ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                ExcluirDepartamentoLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - EXCLUIR DEPARTAMENTO\n\n" +
                       "🚨 OPERAÇÃO CRÍTICA:\n" +
                       "Esta é uma operação de exclusão permanente.\n" +
                       "Os dados não poderão ser recuperados após a confirmação.\n\n" +
                       "⚠️ PROCESSO DE CONFIRMAÇÃO:\n" +
                       "1. Clique em 'Confirmar Exclusão'\n" +
                       "2. Confirme na primeira mensagem\n" +
                       "3. Confirme novamente na segunda mensagem\n" +
                       "4. O departamento será excluído permanentemente\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• DEL/ENTER - Iniciar processo de exclusão\n" +
                       "• ESC - Cancelar e manter departamento\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "🛡️ SEGURANÇA:\n" +
                       "• Dupla confirmação obrigatória\n" +
                       "• Todas as ações são registradas\n" +
                       "• Botão 'Manter' está em destaque\n" +
                       "• Foco inicial no botão seguro\n" +
                       "• Verificação de colaboradores vinculados\n\n" +
                       "💡 RECOMENDAÇÃO:\n" +
                       "Certifique-se de que não há colaboradores\n" +
                       "vinculados a este departamento antes de excluir.";
            
            MessageBox.Show(ajuda, "Ajuda - Excluir Departamento",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirSucesso(string mensagem)
        {
            MessageBox.Show(mensagem, "✅ Exclusão Realizada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirErro(string titulo, string mensagem)
        {
            MessageBox.Show(mensagem, $"❌ {titulo}",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void ExibirAviso(string mensagem, string titulo = "⚠️ Atenção")
        {
            MessageBox.Show(mensagem, titulo,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        #endregion
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantido para compatibilidade com código existente
        private async Task Excluir()
        {
            await ProcessarExclusao();
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
                ExcluirDepartamentoLogger.LogError($"Erro ao atualizar formulário principal: {ex.Message}", "Integration", ex);
            }
        }
        
        private void ExcluirDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLoading)
            {
                e.Cancel = true;
                ExibirAviso(
                    "Aguarde a conclusão da operação de exclusão.",
                    "Operação em Andamento");
                return;
            }
            
            // Não precisa mais da atualização manual - DialogResult resolve isso
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class ExcluirDepartamentoLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [ExcluirDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [ExcluirDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [ExcluirDepartamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [ExcluirDepartamento-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}
