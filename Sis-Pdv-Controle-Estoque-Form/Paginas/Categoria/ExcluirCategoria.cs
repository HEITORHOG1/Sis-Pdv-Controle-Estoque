using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class ExcluirCategoria : Form
    {
        #region Campos Privados
        
        private CategoriaService _categoriaService;
        private bool _isLoading = false;
        private bool _exclusaoConfirmada = false;
        private string _NomeCategoria = "";
        private string _categoriaId = "";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public ExcluirCategoria(string NomeCategoria, string id)
        {
            InitializeComponent();
            InicializarComponentesModernos(NomeCategoria, id);
        }
        
        private void InicializarComponentesModernos(string NomeCategoria, string id)
        {
            // Inicializa serviços
            _categoriaService = new CategoriaService();
            
            // Configura dados iniciais
            _NomeCategoria = NomeCategoria ?? "";
            _categoriaId = id ?? "";
            
            txtNomeCategoria.Text = _NomeCategoria;
            LblId.Text = _categoriaId;
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Log de inicialização
            ExcluirCategoriaLogger.LogInfo($"Formulário de exclusão inicializado - ID: {id}, Nome: {NomeCategoria}", "Startup");
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
        
        private void ExcluirCategoria_KeyDown(object sender, KeyEventArgs e)
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
        
        private void ExcluirCategoria_Load(object sender, EventArgs e)
        {
            try
            {
                // Foco no botão cancelar (mais seguro)
                btnCancelar.Focus();
                
                ExcluirCategoriaLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                ExcluirCategoriaLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
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
                
                ExcluirCategoriaLogger.LogInfo($"Iniciando exclusão da categoria: ID={_categoriaId}, Nome={_NomeCategoria}", "Delete");
                
                var response = await _categoriaService.RemoverCategoria(_categoriaId);
                
                sw.Stop();
                ExcluirCategoriaLogger.LogApiCall("RemoverCategoria", "DELETE", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    NomeExcluido = _NomeCategoria;
                    ExclusaoRealizada = true;
                    _exclusaoConfirmada = true;
                    
                    ExibirSucesso($"Categoria '{_NomeCategoria}' excluída com sucesso!");
                    
                    ExcluirCategoriaLogger.LogInfo($"Categoria excluída com sucesso: ID={_categoriaId}, Nome={_NomeCategoria}", "Delete");
                    
                    // Pequeno delay para feedback visual
                    await Task.Delay(1500);
                    
                    FecharFormulario(true);
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao excluir categoria.";
                    ExibirErro("Erro na Exclusão", erro);
                    ExcluirCategoriaLogger.LogWarning($"Falha na exclusão: {erro}", "Delete");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ExcluirCategoriaLogger.LogError($"Erro ao excluir categoria: {ex.Message}", "Delete", ex);
                ExibirErro("Erro Inesperado", $"Erro ao excluir categoria: {ex.Message}");
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
                $"Categoria: {_NomeCategoria}\n" +
                $"ID: {_categoriaId}\n\n" +
                $"⚠️ ATENÇÃO:\n" +
                $"• Esta ação é IRREVERSÍVEL\n" +
                $"• A categoria será excluída permanentemente\n" +
                $"• Todos os dados serão perdidos\n\n" +
                $"Você tem CERTEZA ABSOLUTA que deseja excluir esta categoria?",
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
                    $"Categoria '{_NomeCategoria}' será excluída PERMANENTEMENTE.\n\n" +
                    $"Continuar com a exclusão?",
                    "🔴 ÚLTIMA CHANCE",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2);
                
                return segundaConfirmacao == DialogResult.Yes;
            }
            
            return false;
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Excluindo categoria...";
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
                    lblStatus.Text = "✅ Categoria excluída com sucesso!";
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
                ExcluirCategoriaLogger.LogInfo($"Exclusão de categoria {acao}", "UserAction");
                
                this.DialogResult = sucesso ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                ExcluirCategoriaLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - EXCLUIR CATEGORIA\n\n" +
                       "🚨 OPERAÇÃO CRÍTICA:\n" +
                       "Esta é uma operação de exclusão permanente.\n" +
                       "Os dados não poderão ser recuperados após a confirmação.\n\n" +
                       "⚠️ PROCESSO DE CONFIRMAÇÃO:\n" +
                       "1. Clique em 'Confirmar Exclusão'\n" +
                       "2. Confirme na primeira mensagem\n" +
                       "3. Confirme novamente na segunda mensagem\n" +
                       "4. A categoria será excluída permanentemente\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• DEL/ENTER - Iniciar processo de exclusão\n" +
                       "• ESC - Cancelar e manter categoria\n" +
                       "• F1 - Esta ajuda\n\n" +
                       "🛡️ SEGURANÇA:\n" +
                       "• Dupla confirmação obrigatória\n" +
                       "• Todas as ações são registradas\n" +
                       "• Botão 'Manter' está em destaque\n" +
                       "• Foco inicial no botão seguro\n\n" +
                       "💡 RECOMENDAÇÃO:\n" +
                       "Verifique se não há produtos vinculados\n" +
                       "a esta categoria antes de excluir.";
            
            MessageBox.Show(ajuda, "Ajuda - Excluir Categoria",
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
        
        #endregion
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantido para compatibilidade com código existente
        private async Task Excluir()
        {
            await ProcessarExclusao();
        }
        
        private void ExcluirCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Validação de fechamento já é feita em FecharFormulario()
        }
        
        private void ExcluirCategoria_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Limpeza se necessária
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class ExcluirCategoriaLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [ExcluirCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [ExcluirCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [ExcluirCategoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [ExcluirCategoria-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}
