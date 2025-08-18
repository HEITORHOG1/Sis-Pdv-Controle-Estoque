using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class ExcluirDepartamento : Form
    {
        private DepartamentoService departamentoService;
        private string departamentoId;
        private string nomeDepartamento;
        private bool isLoading = false;

        public ExcluirDepartamento(string nomeDept, string id)
        {
            InitializeComponent();
            departamentoService = new DepartamentoService();
            departamentoId = id;
            nomeDepartamento = nomeDept;
            
            txtNomeDepartamento.Text = nomeDept;
            txtNomeDepartamento.ReadOnly = true; // Campo readonly para exclusão
            LblId.Text = id;
            
            // Configura o foco no botão de exclusão
            btnExcluir.Focus();
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await Excluir();
        }

        private async Task Excluir()
        {
            try
            {
                // Confirmação dupla para exclusão
                var result = MessageBox.Show(
                    $"Tem certeza que deseja excluir o departamento '{nomeDepartamento}'?\n\n" +
                    "ATENÇÃO: Esta ação não pode ser desfeita!",
                    "Confirmar Exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2); // Padrão é "Não"

                if (result != DialogResult.Yes) return;

                // Segunda confirmação
                result = MessageBox.Show(
                    "Confirma definitivamente a exclusão?\n\nEsta é sua última chance de cancelar!",
                    "Confirmação Final",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes) return;

                isLoading = true;
                SetLoadingState(true);

                // Verifica se o departamento pode ser excluído (não tem colaboradores vinculados)
                if (!await PodeExcluirDepartamento())
                {
                    MessageBox.Show(
                        "Este departamento não pode ser excluído pois possui colaboradores vinculados.\n\n" +
                        "Para excluir este departamento, primeiro remova ou transfira todos os colaboradores para outros departamentos.",
                        "Departamento em Uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var response = await departamentoService.RemoverDepartamento(departamentoId);

                if (response.success)
                {
                    MessageBox.Show(
                        $"Departamento '{nomeDepartamento}' excluído com sucesso!",
                        "Exclusão Realizada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    
                    await AtualizarFormularioPrincipal();
                    this.Close();
                }
                else
                {
                    var erros = string.Join("\n", response.notifications?.Select(n => n.ToString()) ?? 
                        new[] { "Erro desconhecido" });
                    
                    MessageBox.Show(
                        $"Erro ao excluir departamento:\n{erros}",
                        "Erro na Exclusão",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro inesperado ao excluir departamento: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void ExcluirDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoading)
            {
                e.Cancel = true;
                MessageBox.Show(
                    "Aguarde a conclusão da operação de exclusão.",
                    "Operação em Andamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            await AtualizarFormularioPrincipal();
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
                // Log do erro, mas não mostra para o usuário
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar formulário principal: {ex.Message}");
            }
        }

        private async Task<bool> PodeExcluirDepartamento()
        {
            try
            {
                // TODO: Implementar verificação se o departamento tem colaboradores vinculados
                // Por enquanto, retorna true. Em uma implementação real, você faria uma consulta
                // para verificar se existem colaboradores vinculados a este departamento
                
                // Exemplo de implementação:
                // var colaboradorService = new ColaboradorService();
                // var colaboradores = await colaboradorService.ListarColaboradoresPorDepartamento(departamentoId);
                // return !colaboradores.Any();
                
                return true;
            }
            catch
            {
                // Em caso de erro na verificação, permite a exclusão
                // A API fará a validação final
                return true;
            }
        }

        private void SetLoadingState(bool loading)
        {
            btnExcluir.Enabled = !loading;
            
            if (loading)
            {
                this.Cursor = Cursors.WaitCursor;
                btnExcluir.Text = "Excluindo...";
            }
            else
            {
                this.Cursor = Cursors.Default;
                btnExcluir.Text = "Excluir";
            }
        }

        // Permite fechar com ESC
        private void ExcluirDepartamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        // Configura o formulário no load
        private void ExcluirDepartamento_Load(object sender, EventArgs e)
        {
            // Centraliza o formulário
            this.CenterToParent();
            
            // Define o foco no botão de exclusão
            btnExcluir.Focus();
            
            // Adiciona informações visuais sobre a exclusão
            this.Text = $"Excluir Departamento - {nomeDepartamento}";
        }

        // Adiciona botão Cancelar para melhor UX
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
