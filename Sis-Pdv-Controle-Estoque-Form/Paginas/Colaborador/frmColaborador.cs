using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Colaborador
{
    public partial class frmColaborador : Form
    {
        private ColaboradorService colaboradorService;
        private DepartamentoService departamentoService;
        private BindingList<ColaboradorDto> colaboradoresList;
        private bool isLoading = false;

        public frmColaborador()
        {
            InitializeComponent();
            colaboradorService = new ColaboradorService();
            departamentoService = new DepartamentoService();
            colaboradoresList = new BindingList<ColaboradorDto>();
            
            ColaboradorLogger.LogInfo("Formulário de cadastro de colaborador inicializado", "FormLoad");
        }

        private async void frmColaborador_Load(object sender, EventArgs e)
        {
            ColaboradorLogger.LogOperation("CarregamentoFormulario");
            await Consultar();
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await CadastrarColaborador();
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await AlterarColaborador();
        }

        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await Consultar();
        }

        private async void dgvColaborador_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < colaboradoresList.Count)
            {
                var colaborador = colaboradoresList[e.RowIndex];
                PreencherCamposEdicao(colaborador);
                
                ColaboradorLogger.LogOperation("ColaboradorSelecionado", colaborador.id, colaborador.nomeColaborador);
            }
        }

        private async Task CadastrarColaborador()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                var dto = new ColaboradorDto()
                {
                    nomeColaborador = txtNomeColaborador.Text.Trim(),
                    cargoColaborador = txtCargo.Text.Trim(),
                    cpfColaborador = txtCPF.Text.Trim(),
                    emailCorporativo = txtEmail.Text.Trim(),
                    emailPessoalColaborador = txtEmailCorp.Text.Trim(),
                    telefoneColaborador = txtTelefone.Text.Trim(),
                    senha = txtSenha.Text.Trim(),
                    login = txtLogin.Text.Trim(),
                    departamentoId = cbxDepartamento.SelectedValue?.ToString() ?? string.Empty,
                    statusAtivo = rbAtivo.Checked
                };

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    ColaboradorLogger.LogValidationError("Colaborador", mensagem, dto.nomeColaborador);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ColaboradorLogger.LogOperation("CadastroIniciado", value: dto.nomeColaborador);

                var response = await colaboradorService.AdicionarColaborador(dto);

                sw.Stop();
                ColaboradorLogger.LogApiCall("AdicionarColaborador", "POST", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    LimparCampos();
                    var mensagem = $"Colaborador '{response.data.nomeColaborador}' inserido com sucesso!";
                    
                    ColaboradorLogger.LogOperation("CadastroRealizado", response.data.id, response.data.nomeColaborador);
                    
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ColaboradorLogger.LogError($"Erro no cadastro: {erros}", "Cadastro");
                    MessageBox.Show($"Erro ao cadastrar colaborador:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ColaboradorLogger.LogError("Erro inesperado ao cadastrar colaborador", "Cadastro", ex);
                MessageBox.Show($"Erro inesperado ao cadastrar colaborador: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task AlterarColaborador()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarSelecao("alterar")) return;
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                var dto = new ColaboradorDto()
                {
                    id = lblId.Text,
                    nomeColaborador = txtNomeColaborador.Text.Trim(),
                    cargoColaborador = txtCargo.Text.Trim(),
                    cpfColaborador = txtCPF.Text.Trim(),
                    emailCorporativo = txtEmail.Text.Trim(),
                    emailPessoalColaborador = txtEmailCorp.Text.Trim(),
                    telefoneColaborador = txtTelefone.Text.Trim(),
                    senha = txtSenha.Text.Trim(),
                    login = txtLogin.Text.Trim(),
                    departamentoId = cbxDepartamento.SelectedValue?.ToString() ?? string.Empty,
                    idlogin = lblIdLogin.Text,
                    statusAtivo = rbAtivo.Checked
                };

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    ColaboradorLogger.LogValidationError("Colaborador", mensagem, dto.nomeColaborador);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ColaboradorLogger.LogOperation("AlteracaoIniciada", dto.id, dto.nomeColaborador);

                var response = await colaboradorService.AlterarColaborador(dto);

                sw.Stop();
                ColaboradorLogger.LogApiCall("AlterarColaborador", "PUT", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    LimparCampos();
                    var mensagem = $"Colaborador '{response.data.nomeColaborador}' alterado com sucesso!";
                    
                    ColaboradorLogger.LogOperation("AlteracaoRealizada", response.data.id, response.data.nomeColaborador);
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ColaboradorLogger.LogError($"Erro na alteração: {erros}", "Alteracao");
                    MessageBox.Show($"Erro ao alterar colaborador:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ColaboradorLogger.LogError("Erro inesperado ao alterar colaborador", "Alteracao", ex);
                MessageBox.Show($"Erro inesperado ao alterar colaborador: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task Consultar()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                isLoading = true;
                SetLoadingState(true);

                ColaboradorLogger.LogOperation("ListagemIniciada");

                var response = await colaboradorService.ListarColaborador();
                
                sw.Stop();
                ColaboradorLogger.LogApiCall("ListarColaborador", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    colaboradoresList.Clear();
                    foreach (var colab in response.data)
                    {
                        colaboradoresList.Add(colab.ToDto());
                    }

                    lstGrid.DataSource = null;
                    lstGrid.DataSource = colaboradoresList;
                    DefinirCabecalhos(new List<string>() {
                        "ID", "Nome", "Departamento", "CPF", "Cargo", "Telefone",
                        "E-mail Pessoal", "E-mail Corp", "IdLogin", "Login", "Senha", "Status"
                    });

                    // Oculta colunas sensíveis
                    if (lstGrid.Columns["id"] != null)
                        lstGrid.Columns["id"].Visible = false;
                    if (lstGrid.Columns["idlogin"] != null)
                        lstGrid.Columns["idlogin"].Visible = false;
                    if (lstGrid.Columns["senha"] != null)
                        lstGrid.Columns["senha"].Visible = false;

                    lstGrid.Refresh();
                    
                    ColaboradorLogger.LogInfo($"Listagem concluída com {colaboradoresList.Count} colaboradores", "Listagem");

                    // Carrega departamentos no combo
                    await CarregarDepartamentos();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ColaboradorLogger.LogError($"Erro na listagem: {erros}", "Listagem");
                    MessageBox.Show($"Erro ao consultar colaboradores:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ColaboradorLogger.LogError("Erro inesperado ao consultar colaboradores", "Listagem", ex);
                MessageBox.Show($"Erro inesperado ao consultar colaboradores: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task CarregarDepartamentos()
        {
            try
            {
                var response = await departamentoService.ListarDepartamento();
                if (response.IsValidResponse())
                {
                    cbxDepartamento.DataSource = response.data;
                    cbxDepartamento.DisplayMember = "nomeDepartamento";
                    cbxDepartamento.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                ColaboradorLogger.LogError($"Erro ao carregar departamentos: {ex.Message}", "CarregarDepartamentos", ex);
            }
        }

        private bool ValidarCamposCadastro()
        {
            // Validação Nome
            if (string.IsNullOrWhiteSpace(txtNomeColaborador.Text))
            {
                MessageBox.Show("Informe o nome do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeColaborador.Focus();
                return false;
            }

            // Validação CPF
            if (string.IsNullOrWhiteSpace(txtCPF.Text))
            {
                MessageBox.Show("Informe o CPF do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCPF.Focus();
                return false;
            }

            // Validação Departamento
            if (cbxDepartamento.SelectedValue == null)
            {
                MessageBox.Show("Selecione um departamento.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxDepartamento.Focus();
                return false;
            }

            // Validação Cargo
            if (string.IsNullOrWhiteSpace(txtCargo.Text))
            {
                MessageBox.Show("Informe o cargo do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCargo.Focus();
                return false;
            }

            // Validação Telefone
            if (string.IsNullOrWhiteSpace(txtTelefone.Text))
            {
                MessageBox.Show("Informe o telefone do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefone.Focus();
                return false;
            }

            // Validação Email Pessoal
            if (string.IsNullOrWhiteSpace(txtEmailCorp.Text))
            {
                MessageBox.Show("Informe o email pessoal do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmailCorp.Focus();
                return false;
            }

            // Validação Email Corporativo
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Informe o email corporativo do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // NOVA VALIDAÇÃO: Emails devem ser diferentes
            if (string.Equals(txtEmailCorp.Text.Trim(), txtEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("O email corporativo deve ser diferente do email pessoal.\n\n" +
                    "Sugestão: Use um domínio corporativo para o email corporativo\n" +
                    "(ex: nome@empresa.com.br)", "Emails Iguais", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validação Login
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Informe o login do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return false;
            }

            // Validação Senha
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Informe a senha do colaborador.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return false;
            }

            if (txtSenha.Text.Length < 6)
            {
                MessageBox.Show("A senha deve ter pelo menos 6 caracteres.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarSelecao(string acao)
        {
            if (string.IsNullOrEmpty(lblId.Text) || lblId.Text == "IdColaborador")
            {
                ColaboradorLogger.LogWarning($"Tentativa de {acao} sem seleção", "Validacao");
                MessageBox.Show($"Selecione um colaborador para {acao}.", "Seleção Necessária", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void PreencherCamposEdicao(ColaboradorDto colaborador)
        {
            lblId.Text = colaborador.id;
            txtNomeColaborador.Text = colaborador.nomeColaborador;
            txtCPF.Text = colaborador.cpfColaborador;
            txtCargo.Text = colaborador.cargoColaborador;
            txtTelefone.Text = colaborador.telefoneColaborador;
            txtEmailCorp.Text = colaborador.emailPessoalColaborador;
            txtEmail.Text = colaborador.emailCorporativo;
            txtLogin.Text = colaborador.login;
            txtSenha.Text = colaborador.senha;
            lblIdLogin.Text = colaborador.idlogin;

            // Seleciona o departamento
            if (!string.IsNullOrEmpty(colaborador.departamentoId))
            {
                cbxDepartamento.SelectedValue = colaborador.departamentoId;
            }

            // Define status
            rbAtivo.Checked = colaborador.statusAtivo;
            rbInativo.Checked = !colaborador.statusAtivo;

            btnAdicionar.Enabled = false;
        }

        private void LimparCampos()
        {
            lblId.Text = "IdColaborador";
            txtNomeColaborador.Clear();
            txtCPF.Clear();
            txtCargo.Text = "";
            txtTelefone.Clear();
            txtEmailCorp.Clear();
            txtEmail.Clear();
            txtLogin.Clear();
            txtSenha.Clear();
            lblIdLogin.Text = "IdLogin";

            cbxDepartamento.Text = "";

            if (cbxDepartamento.Items.Count > 0)
                cbxDepartamento.SelectedIndex = 0;

            rbAtivo.Checked = true;
            rbInativo.Checked = false;
            btnAdicionar.Enabled = true;
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

        private void SetLoadingState(bool loading)
        {
            btnAdicionar.Enabled = !loading;
            btnAlterar.Enabled = !loading;
            btnConsulta.Enabled = !loading;
            lstGrid.Enabled = !loading;

            // Desabilita campos durante carregamento
            txtNomeColaborador.Enabled = !loading;
            txtCPF.Enabled = !loading;
            txtCargo.Enabled = !loading;
            txtTelefone.Enabled = !loading;
            txtEmailCorp.Enabled = !loading;
            txtEmail.Enabled = !loading;
            txtLogin.Enabled = !loading;
            txtSenha.Enabled = !loading;
            cbxDepartamento.Enabled = !loading;
            rbAtivo.Enabled = !loading;
            rbInativo.Enabled = !loading;

            if (loading)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Event handlers para validação de entrada
        private void txtCPF_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNomeColaborador_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas letras e espaços
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas letras, números, pontos e underscores
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && 
                e.KeyChar != '.' && e.KeyChar != '_')
            {
                e.Handled = true;
            }
        }

        // Novo event handler para sugerir login baseado no nome
        private void txtNomeColaborador_Leave(object sender, EventArgs e)
        {
            // Gera sugestão de login se o campo estiver vazio
            if (string.IsNullOrWhiteSpace(txtLogin.Text) && !string.IsNullOrWhiteSpace(txtNomeColaborador.Text))
            {
                var dto = new ColaboradorDto { nomeColaborador = txtNomeColaborador.Text };
                txtLogin.Text = dto.GerarLoginSugerido();
            }

            // Gera sugestão de email corporativo se o campo estiver vazio
            if (string.IsNullOrWhiteSpace(txtEmail.Text) && !string.IsNullOrWhiteSpace(txtNomeColaborador.Text))
            {
                var dto = new ColaboradorDto { nomeColaborador = txtNomeColaborador.Text };
                txtEmail.Text = dto.GerarEmailCorporativoSugerido();
            }
        }

        // Event handler para validar emails diferentes em tempo real
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            ValidarEmailsDiferentes();
        }

        private void txtEmailCorp_Leave(object sender, EventArgs e)
        {
            ValidarEmailsDiferentes();
        }

        private void ValidarEmailsDiferentes()
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && 
                !string.IsNullOrWhiteSpace(txtEmailCorp.Text) &&
                string.Equals(txtEmail.Text.Trim(), txtEmailCorp.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var resultado = MessageBox.Show(
                    "Os emails pessoal e corporativo são iguais.\n\n" +
                    "Deseja gerar automaticamente um email corporativo diferente?",
                    "Emails Iguais", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    var dto = new ColaboradorDto { nomeColaborador = txtNomeColaborador.Text };
                    txtEmail.Text = dto.GerarEmailCorporativoSugerido();
                }
            }
        }

        private void frmColaborador_FormClosing(object sender, FormClosingEventArgs e)
        {
            ColaboradorLogger.LogOperation("FormularioFechado");
        }
    }
}
