using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class CadDepartamento : Form
    {
        private DepartamentoService departamentoService;
        private BindingList<DepartamentoDto> departamentosList;
        private bool isLoading = false;

        public CadDepartamento()
        {
            InitializeComponent();
            departamentoService = new DepartamentoService();
            departamentosList = new BindingList<DepartamentoDto>();
            
            DepartamentoLogger.LogInfo("Formulário de cadastro de departamento inicializado", "FormLoad");
        }

        private async void CadDepartamento_Load(object sender, EventArgs e)
        {
            DepartamentoLogger.LogOperation("CarregamentoFormulario");
            await Consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;

            try
            {
                var nomePesquisa = txtNomeDepartamento.Text?.Trim();
                
                DepartamentoLogger.LogOperation("ConsultaIniciada", value: nomePesquisa ?? "TODOS");
                
                if (!string.IsNullOrEmpty(nomePesquisa))
                {
                    await ConsultarPorNomeDepartamento(nomePesquisa);
                }
                else
                {
                    await Consultar();
                }
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError("Erro ao consultar departamentos", "Consulta", ex);
                MessageBox.Show($"Erro ao consultar departamentos: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await CadastrarDepartamento();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (!ValidarSelecao("excluir")) return;

            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir o departamento '{txtNomeDepartamento.Text}'?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DepartamentoLogger.LogOperation("ExclusaoIniciada", LblId.Text, txtNomeDepartamento.Text);
                ExcluirDepartamento form = new ExcluirDepartamento(txtNomeDepartamento.Text, LblId.Text);
                form.ShowDialog();
                _ = Task.Run(async () => await Consultar());
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (!ValidarSelecao("alterar")) return;

            DepartamentoLogger.LogOperation("AlteracaoIniciada", LblId.Text, txtNomeDepartamento.Text);
            AltDepartamento form = new AltDepartamento(txtNomeDepartamento.Text, LblId.Text);
            form.ShowDialog();
            _ = Task.Run(async () => await Consultar());
        }

        private async Task CadastrarDepartamento()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                var dto = new DepartamentoDto()
                {
                    NomeDepartamento = txtNomeDepartamento.Text.Trim()
                };

                // Normaliza o nome
                dto.NormalizarNome();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    DepartamentoLogger.LogValidationError("NomeDepartamento", mensagem, dto.NomeDepartamento);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DepartamentoLogger.LogOperation("CadastroIniciado", value: dto.NomeDepartamento);

                // Verifica se já existe um departamento com o mesmo nome
                if (await DepartamentoJaExiste(dto.NomeDepartamento))
                {
                    DepartamentoLogger.LogWarning($"Tentativa de cadastro de departamento duplicado: {dto.NomeDepartamento}", "Cadastro");
                    MessageBox.Show("Já existe um departamento com este nome.", "Departamento Duplicado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var response = await departamentoService.AdicionarDepartamento(dto);

                sw.Stop();
                DepartamentoLogger.LogApiCall("AdicionarDepartamento", "POST", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    LimparCampos();
                    var mensagem = $"Departamento '{response.data.nomeDepartamento}' inserido com sucesso!";
                    
                    DepartamentoLogger.LogOperation("CadastroRealizado", response.data.id, response.data.nomeDepartamento);
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    DepartamentoLogger.LogError($"Erro no cadastro: {erros}", "Cadastro");
                    MessageBox.Show($"Erro ao cadastrar departamento:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError("Erro inesperado ao cadastrar departamento", "Cadastro", ex);
                MessageBox.Show($"Erro inesperado ao cadastrar departamento: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        public async Task Consultar()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                isLoading = true;
                SetLoadingState(true);

                DepartamentoLogger.LogOperation("ListagemIniciada");

                var response = await departamentoService.ListarDepartamento();
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("ListarDepartamento", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    departamentosList.Clear();
                    foreach (var dept in response.data)
                    {
                        departamentosList.Add(dept.ToDto());
                    }

                    lstGrid.DataSource = null;
                    lstGrid.DataSource = departamentosList;
                    DefinirCabecalhos(new List<string>() { "NomeDepartamento", "Id" });
                    
                    // Oculta a coluna Id
                    if (lstGrid.Columns["Id"] != null)
                        lstGrid.Columns["Id"].Visible = false;

                    lstGrid.Refresh();
                    
                    DepartamentoLogger.LogInfo($"Listagem concluída com {departamentosList.Count} departamentos", "Listagem");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    DepartamentoLogger.LogError($"Erro na listagem: {erros}", "Listagem");
                    MessageBox.Show($"Erro ao consultar departamentos:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError("Erro inesperado ao consultar departamentos", "Listagem", ex);
                MessageBox.Show($"Erro inesperado ao consultar departamentos: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
                LimparCampos();
            }
        }

        private async Task ConsultarPorNomeDepartamento(string nomeDepartamento)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                isLoading = true;
                SetLoadingState(true);

                DepartamentoLogger.LogOperation("PesquisaIniciada", value: nomeDepartamento);

                var response = await departamentoService.ListarDepartamentoPorNomeDepartamento(nomeDepartamento);
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("ListarDepartamentoPorNome", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    departamentosList.Clear();
                    foreach (var dept in response.data)
                    {
                        departamentosList.Add(dept.ToDto());
                    }

                    lstGrid.DataSource = null;
                    lstGrid.DataSource = departamentosList;
                    DefinirCabecalhos(new List<string>() { "NomeDepartamento", "Id" });
                    
                    if (lstGrid.Columns["Id"] != null)
                        lstGrid.Columns["Id"].Visible = false;

                    if (departamentosList.Count == 0)
                    {
                        DepartamentoLogger.LogInfo($"Nenhum departamento encontrado para: {nomeDepartamento}", "Pesquisa");
                        MessageBox.Show("Nenhum departamento encontrado com o nome especificado.", "Pesquisa", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DepartamentoLogger.LogInfo($"Pesquisa concluída com {departamentosList.Count} resultados para: {nomeDepartamento}", "Pesquisa");
                    }
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    DepartamentoLogger.LogWarning($"Pesquisa sem resultados: {erros}", "Pesquisa");
                    MessageBox.Show($"Erro na pesquisa:\n• {erros}", "Pesquisa", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError("Erro inesperado na pesquisa", "Pesquisa", ex);
                MessageBox.Show($"Erro inesperado na pesquisa: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Consultar();
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private void DefinirCabecalhos(List<string> listaCabecalhos)
        {
            int index = 0;

            foreach (DataGridViewColumn coluna in lstGrid.Columns)
            {
                if (coluna.Visible && index < listaCabecalhos.Count)
                {
                    coluna.HeaderText = listaCabecalhos[index];
                    index++;
                }
            }
        }

        private void lstGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < departamentosList.Count)
            {
                var departamento = departamentosList[e.RowIndex];
                txtNomeDepartamento.Text = departamento.NomeDepartamento;
                LblId.Text = departamento.Id;
                
                DepartamentoLogger.LogOperation("DepartamentoSelecionado", departamento.Id, departamento.NomeDepartamento);
            }
        }

        private bool ValidarCamposCadastro()
        {
            var nome = txtNomeDepartamento.Text?.Trim();

            if (string.IsNullOrEmpty(nome))
            {
                DepartamentoLogger.LogValidationError("NomeDepartamento", "Campo obrigatório vazio");
                MessageBox.Show("Informe o nome do departamento.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            if (nome.Length < 2)
            {
                DepartamentoLogger.LogValidationError("NomeDepartamento", "Muito curto", nome);
                MessageBox.Show("O nome do departamento deve ter pelo menos 2 caracteres.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            if (nome.Length > 150)
            {
                DepartamentoLogger.LogValidationError("NomeDepartamento", "Muito longo", nome);
                MessageBox.Show("O nome do departamento não pode ter mais de 150 caracteres.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeDepartamento.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarSelecao(string acao)
        {
            if (string.IsNullOrEmpty(txtNomeDepartamento.Text?.Trim()) || string.IsNullOrEmpty(LblId.Text))
            {
                DepartamentoLogger.LogWarning($"Tentativa de {acao} sem seleção", "Validacao");
                MessageBox.Show($"Selecione um departamento para {acao}.", "Seleção Necessária", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task<bool> DepartamentoJaExiste(string nome)
        {
            try
            {
                var response = await departamentoService.ListarDepartamentoPorNomeDepartamento(nome);
                return response.IsValidResponse() && response.data != null && response.data.Any();
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogWarning($"Erro ao verificar duplicação: {ex.Message}", "VerificacaoDuplicacao");
                return false;
            }
        }

        private void LimparCampos()
        {
            txtNomeDepartamento.Text = "";
            LblId.Text = "";
            txtNomeDepartamento.Focus();
        }

        private void SetLoadingState(bool loading)
        {
            btnCadastrar.Enabled = !loading;
            btnConsultar.Enabled = !loading;
            btnAlterar.Enabled = !loading;
            btnExcluir.Enabled = !loading;
            txtNomeDepartamento.Enabled = !loading;
            lstGrid.Enabled = !loading;

            if (loading)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtNomeDepartamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete || e.KeyChar == ' ')
                return;

            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '_' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtNomeDepartamento_Leave(object sender, EventArgs e)
        {
            txtNomeDepartamento.Text = txtNomeDepartamento.Text?.Trim();
        }

        private void CadDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            DepartamentoLogger.LogOperation("FormularioFechado");
        }
    }
}
