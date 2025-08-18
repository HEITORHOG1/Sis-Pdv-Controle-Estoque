using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class AltDepartamento : Form
    {
        private DepartamentoService departamentoService;
        private string departamentoId;
        private string nomeOriginal;
        private bool isLoading = false;

        public AltDepartamento(string nomeDepartamento, string id)
        {
            InitializeComponent();
            departamentoService = new DepartamentoService();
            departamentoId = id;
            nomeOriginal = nomeDepartamento;
            
            txtNomeDepartamento.Text = nomeDepartamento;
            LblId.Text = id;
            
            // Configura validação em tempo real
            txtNomeDepartamento.TextChanged += TxtNomeDepartamento_TextChanged;
        }

        private void TxtNomeDepartamento_TextChanged(object sender, EventArgs e)
        {
            ValidarFormulario();
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await Alterar();
        }

        private async Task Alterar()
        {
            try
            {
                if (!ValidarCampos()) return;

                var novoNome = txtNomeDepartamento.Text.Trim();
                
                // Verifica se houve mudança
                if (novoNome.Equals(nomeOriginal, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Nenhuma alteração foi detectada.", "Informação", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Confirma a alteração do departamento?\n\nDe: {nomeOriginal}\nPara: {novoNome}",
                    "Confirmar Alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                isLoading = true;
                SetLoadingState(true);

                // Verifica se já existe outro departamento com o mesmo nome
                if (await DepartamentoJaExiste(novoNome))
                {
                    MessageBox.Show("Já existe um departamento com este nome.", "Departamento Duplicado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DepartamentoDto dto = new DepartamentoDto()
                {
                    Id = departamentoId,
                    NomeDepartamento = novoNome
                };

                DepartamentoResponse response = await departamentoService.AlterarDepartamento(dto);

                if (response.success)
                {
                    MessageBox.Show("Departamento alterado com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    await AtualizarFormularioPrincipal();
                    this.Close();
                }
                else
                {
                    var erros = string.Join("\n", response.notifications?.Select(n => n.ToString()) ?? 
                        new[] { "Erro desconhecido" });
                    MessageBox.Show($"Erro ao alterar departamento:\n{erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado ao alterar departamento: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void AltDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoading)
            {
                e.Cancel = true;
                return;
            }

            // Verifica se houve alterações não salvas
            var nomeAtual = txtNomeDepartamento.Text?.Trim();
            if (!string.IsNullOrEmpty(nomeAtual) && !nomeAtual.Equals(nomeOriginal, StringComparison.OrdinalIgnoreCase))
            {
                var result = MessageBox.Show(
                    "Existem alterações não salvas. Deseja sair sem salvar?",
                    "Alterações Não Salvas",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            await AtualizarFormularioPrincipal();
        }

        private void AltDepartamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Método necessário para o event handler do Designer
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

        private bool ValidarCampos()
        {
            var nome = txtNomeDepartamento.Text?.Trim();

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Informe o nome do departamento.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            if (nome.Length < 2)
            {
                MessageBox.Show("O nome do departamento deve ter pelo menos 2 caracteres.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            if (nome.Length > 150)
            {
                MessageBox.Show("O nome do departamento não pode ter mais de 150 caracteres.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            return true;
        }

        private void ValidarFormulario()
        {
            var nome = txtNomeDepartamento.Text?.Trim();
            btnAlterar.Enabled = !string.IsNullOrEmpty(nome) && nome.Length >= 2 && nome.Length <= 150;
        }

        private async Task<bool> DepartamentoJaExiste(string nome)
        {
            try
            {
                var response = await departamentoService.ListarDepartamentoPorNomeDepartamento(nome);
                return response.success && response.data != null && 
                       response.data.Any(d => d.id != departamentoId); // Exclui o próprio departamento
            }
            catch
            {
                return false; // Em caso de erro, assume que não existe
            }
        }

        private void SetLoadingState(bool loading)
        {
            btnAlterar.Enabled = !loading;
            txtNomeDepartamento.Enabled = !loading;

            if (loading)
            {
                this.Cursor = Cursors.WaitCursor;
                btnAlterar.Text = "Alterando...";
            }
            else
            {
                this.Cursor = Cursors.Default;
                btnAlterar.Text = "Alterar";
                ValidarFormulario(); // Revalida o formulário
            }
        }

        // Método para validar entrada de texto (apenas letras, números e espaços)
        private void txtNomeDepartamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite backspace, delete e espaço
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete || e.KeyChar == ' ')
                return;

            // Permite apenas letras e números
            if (!char.IsLetterOrDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Valida quando o usuário sai do campo
        private void txtNomeDepartamento_Leave(object sender, EventArgs e)
        {
            txtNomeDepartamento.Text = txtNomeDepartamento.Text?.Trim();
        }

        // Permite fechar com ESC
        private void AltDepartamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void AltDepartamento_Load(object sender, EventArgs e)
        {
            txtNomeDepartamento.Focus();
            txtNomeDepartamento.SelectAll();
            ValidarFormulario();
        }
    }
}
