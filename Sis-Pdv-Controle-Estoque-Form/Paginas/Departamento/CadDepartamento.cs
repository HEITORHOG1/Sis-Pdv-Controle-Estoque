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
        #region Campos Privados
        
        private DepartamentoService _departamentoService;
        private BindingList<DepartamentoDto> _departamentosList;
        private bool _isLoading = false;
        private bool _modoEdicao = false;
        private string _departamentoIdEdicao = "";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public CadDepartamento()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Inicializa serviços e coleções
            _departamentoService = new DepartamentoService();
            _departamentosList = new BindingList<DepartamentoDto>();
            
            // Configura DataGridView
            ConfigurarDataGridView();
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Log de inicialização
            DepartamentoLogger.LogInfo("Formulário de gerenciamento de departamentos inicializado", "Startup");
        }
        
        private void ConfigurarDataGridView()
        {
            lstGrid.DataSource = _departamentosList;
            lstGrid.AutoGenerateColumns = true;
            
            // Configurações de estilo moderno
            lstGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            lstGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            lstGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lstGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            lstGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            lstGrid.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            lstGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            lstGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            
            lstGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharFormulario();
        }
        
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        
        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (_modoEdicao)
            {
                await AlterarDepartamento();
            }
            else
            {
                await CadastrarDepartamento();
            }
        }
        
        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            await ConsultarDepartamentos();
        }
        
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            AtivarModoEdicao();
        }
        
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            await ExcluirDepartamento();
        }
        
        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await AtualizarLista();
        }
        
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
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
            ValidarCampo();
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
        }
        
        private void txtNomeDepartamento_TextChanged(object sender, EventArgs e)
        {
            ValidarCampo();
            AtualizarStatusInterface();
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void CadDepartamento_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    MostrarAjuda();
                    break;
                case Keys.F2:
                    btnCadastrar.PerformClick();
                    break;
                case Keys.F3:
                    btnConsultar.PerformClick();
                    break;
                case Keys.F4:
                    btnAlterar.PerformClick();
                    break;
                case Keys.F5:
                    btnAtualizar.PerformClick();
                    break;
                case Keys.Delete:
                    btnExcluir.PerformClick();
                    break;
                case Keys.Escape:
                    if (_modoEdicao)
                    {
                        DesativarModoEdicao();
                    }
                    else
                    {
                        LimparCampos();
                    }
                    break;
                case Keys.Enter:
                    if (txtNomeDepartamento.Focused)
                    {
                        btnCadastrar.PerformClick();
                    }
                    break;
            }
        }
        
        private async void CadDepartamento_Load(object sender, EventArgs e)
        {
            try
            {
                SetLoadingState(true);
                await AtualizarLista();
                txtNomeDepartamento.Focus();
                
                DepartamentoLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
                ExibirErro("Erro ao carregar formulário", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Operações CRUD
        
        private async Task CadastrarDepartamento()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados()) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeDepartamento.Text.Trim();
                var dto = new DepartamentoDto { NomeDepartamento = nome };
                
                // Normaliza o nome
                dto.NormalizarNome();
                
                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    DepartamentoLogger.LogValidationError("NomeDepartamento", mensagem, dto.NomeDepartamento);
                    ExibirErro("Validação", mensagem);
                    return;
                }
                
                DepartamentoLogger.LogInfo($"Iniciando cadastro do departamento: {nome}", "Create");
                
                // Verifica se já existe um departamento com o mesmo nome
                if (await DepartamentoJaExiste(dto.NomeDepartamento))
                {
                    DepartamentoLogger.LogWarning($"Tentativa de cadastro de departamento duplicado: {dto.NomeDepartamento}", "Create");
                    ExibirAviso("Já existe um departamento com este nome.");
                    return;
                }
                
                var response = await _departamentoService.AdicionarDepartamento(dto);
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("AdicionarDepartamento", "POST", sw.Elapsed, response.IsValidResponse());
                
                if (response.IsValidResponse())
                {
                    ExibirSucesso($"Departamento '{response.data.nomeDepartamento}' cadastrado com sucesso!");
                    LimparCampos();
                    await AtualizarLista();
                    
                    DepartamentoLogger.LogInfo($"Departamento cadastrado: ID={response.data.id}, Nome={response.data.nomeDepartamento}", "Create");
                }
                else
                {
                    var erro = response.GetErrorMessages().FormatErrorMessages();
                    ExibirErro("Erro no Cadastro", erro);
                    DepartamentoLogger.LogWarning($"Falha no cadastro: {erro}", "Create");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError($"Erro ao cadastrar departamento: {ex.Message}", "Create", ex);
                ExibirErro("Erro Inesperado", $"Erro ao cadastrar departamento: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task AlterarDepartamento()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados() || string.IsNullOrEmpty(_departamentoIdEdicao)) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeDepartamento.Text.Trim();
                var dto = new DepartamentoDto 
                { 
                    Id = _departamentoIdEdicao,
                    NomeDepartamento = nome 
                };
                
                DepartamentoLogger.LogInfo($"Iniciando alteração do departamento: ID={_departamentoIdEdicao}, Nome={nome}", "Update");
                
                var response = await _departamentoService.AlterarDepartamento(dto);
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("AlterarDepartamento", "PUT", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    ExibirSucesso($"Departamento alterado para '{response.data.nomeDepartamento}' com sucesso!");
                    DesativarModoEdicao();
                    await AtualizarLista();
                    
                    DepartamentoLogger.LogInfo($"Departamento alterado: ID={response.data.id}, Nome={response.data.nomeDepartamento}", "Update");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao alterar departamento.";
                    ExibirErro("Erro na Alteração", erro);
                    DepartamentoLogger.LogWarning($"Falha na alteração: {erro}", "Update");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError($"Erro ao alterar departamento: {ex.Message}", "Update", ex);
                ExibirErro("Erro Inesperado", $"Erro ao alterar departamento: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task ExcluirDepartamento()
        {
            try
            {
                if (string.IsNullOrEmpty(LblId.Text) || LblId.Text == "IdDepartamento")
                {
                    ExibirAviso("Selecione um departamento para excluir.");
                    return;
                }
                
                var nomeDepartamento = txtNomeDepartamento.Text.Trim();
                var confirmacao = MessageBox.Show(
                    $"⚠️ ATENÇÃO - Exclusão de Departamento\n\n" +
                    $"Departamento: {nomeDepartamento}\n\n" +
                    $"Esta ação não pode ser desfeita.\n" +
                    $"Tem certeza que deseja excluir este departamento?",
                    "Confirmar Exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (confirmacao == DialogResult.No) return;
                
                SetLoadingState(true);
                
                DepartamentoLogger.LogInfo($"Iniciando exclusão do departamento: ID={LblId.Text}, Nome={nomeDepartamento}", "Delete");
                
                var response = await _departamentoService.RemoverDepartamento(LblId.Text);
                
                if (response?.success == true)
                {
                    ExibirSucesso($"Departamento '{nomeDepartamento}' excluído com sucesso!");
                    LimparCampos();
                    await AtualizarLista();
                    
                    DepartamentoLogger.LogInfo($"Departamento excluído: ID={LblId.Text}, Nome={nomeDepartamento}", "Delete");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao excluir departamento.";
                    ExibirErro("Erro na Exclusão", erro);
                    DepartamentoLogger.LogWarning($"Falha na exclusão: {erro}", "Delete");
                }
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro ao excluir departamento: {ex.Message}", "Delete", ex);
                ExibirErro("Erro Inesperado", $"Erro ao excluir departamento: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task ConsultarDepartamentos()
        {
            try
            {
                var termoBusca = txtNomeDepartamento.Text.Trim();
                
                if (string.IsNullOrEmpty(termoBusca))
                {
                    await AtualizarLista();
                }
                else
                {
                    await ConsultarPorNome(termoBusca);
                }
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro na consulta: {ex.Message}", "Search", ex);
                ExibirErro("Erro na Consulta", ex.Message);
            }
        }
        
        private async Task ConsultarPorNome(string nome)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                DepartamentoLogger.LogInfo($"Consultando departamentos por nome: {nome}", "Search");
                
                var response = await _departamentoService.ListarDepartamentoPorNomeDepartamento(nome);
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("ListarDepartamentoPorNome", "GET", sw.Elapsed, response.IsValidResponse());
                
                if (response.IsValidResponse())
                {
                    _departamentosList.Clear();
                    foreach (var departamento in response.data)
                    {
                        _departamentosList.Add(departamento.ToDto());
                    }
                    
                    FormatarGrid();
                    AtualizarContadores();
                    
                    var quantidade = response.data.Count();
                    lblStatus.Text = $"🔍 Encontrado(s) {quantidade} departamento(s) com '{nome}'";
                    
                    DepartamentoLogger.LogInfo($"Consulta concluída: {quantidade} departamento(s) encontrado(s)", "Search");
                }
                else
                {
                    _departamentosList.Clear();
                    FormatarGrid();
                    AtualizarContadores();
                    
                    ExibirAviso($"Nenhum departamento encontrado com o nome '{nome}'.");
                    DepartamentoLogger.LogInfo($"Nenhum departamento encontrado com o nome: {nome}", "Search");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError($"Erro na consulta por nome: {ex.Message}", "Search", ex);
                ExibirErro("Erro na Consulta", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task AtualizarLista()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                DepartamentoLogger.LogInfo("Atualizando lista de departamentos", "Read");
                
                var response = await _departamentoService.ListarDepartamento();
                
                sw.Stop();
                DepartamentoLogger.LogApiCall("ListarDepartamento", "GET", sw.Elapsed, response.IsValidResponse());
                
                if (response.IsValidResponse())
                {
                    _departamentosList.Clear();
                    foreach (var departamento in response.data)
                    {
                        _departamentosList.Add(departamento.ToDto());
                    }
                    
                    FormatarGrid();
                    AtualizarContadores();
                    
                    var quantidade = response.data.Count();
                    lblStatus.Text = $"✅ Lista atualizada com {quantidade} departamento(s)";
                    
                    DepartamentoLogger.LogInfo($"Lista atualizada com {quantidade} departamento(s)", "Read");
                }
                else
                {
                    var erro = response.GetErrorMessages().FormatErrorMessages();
                    ExibirErro("Erro na Consulta", erro);
                    DepartamentoLogger.LogWarning($"Falha na consulta: {erro}", "Read");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                DepartamentoLogger.LogError($"Erro ao atualizar lista: {ex.Message}", "Read", ex);
                ExibirErro("Erro Inesperado", $"Erro ao carregar departamentos: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        #endregion
        
        #region Formatação e Interface
        
        private void FormatarGrid()
        {
            try
            {
                if (lstGrid.Columns.Count == 0) return;
                
                // Configura cabeçalhos
                var cabecalhos = new Dictionary<string, string>
                {
                    ["NomeDepartamento"] = "🏢 Nome do Departamento",
                    ["Id"] = "🔑 ID"
                };
                
                foreach (DataGridViewColumn coluna in lstGrid.Columns)
                {
                    if (cabecalhos.ContainsKey(coluna.Name))
                    {
                        coluna.HeaderText = cabecalhos[coluna.Name];
                        coluna.Visible = true;
                        
                        // Configurações específicas por coluna
                        switch (coluna.Name)
                        {
                            case "NomeDepartamento":
                                coluna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                coluna.MinimumWidth = 200;
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                break;
                            case "Id":
                                coluna.Width = 100;
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                coluna.DefaultCellStyle.Font = new Font("Consolas", 9F);
                                coluna.Visible = false; // Oculta ID por padrão
                                break;
                        }
                    }
                    else
                    {
                        coluna.Visible = false;
                    }
                }
                
                lstGrid.Refresh();
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro ao formatar grid: {ex.Message}", "UI", ex);
            }
        }
        
        private void AtualizarContadores()
        {
            var total = _departamentosList.Count;
            lblGridInfo.Text = $"📋 Lista de Departamentos ({total})";
            lblContador.Text = $"📊 {total} departamento(s) cadastrado(s)";
        }
        
        #endregion
        
        #region Validação e Estado
        
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
                return false;
            }
            
            if (nome.Length > 150)
            {
                ExibirAviso("O nome do departamento não pode ter mais de 150 caracteres.");
                txtNomeDepartamento.Focus();
                return false;
            }
            
            return true;
        }
        
        private void ValidarCampo()
        {
            var nome = txtNomeDepartamento.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nome))
            {
                lblInputIcon.Text = "🏢";
                lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            }
            else if (nome.Length < 2)
            {
                lblInputIcon.Text = "⚠️";
                lblInputIcon.ForeColor = Color.FromArgb(230, 126, 34);
            }
            else
            {
                lblInputIcon.Text = "✅";
                lblInputIcon.ForeColor = Color.FromArgb(46, 204, 113);
            }
        }
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Processando...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                DesabilitarControles(false);
            }
            else
            {
                progressBar.Visible = false;
                lblStatus.ForeColor = Color.White;
                
                if (_modoEdicao)
                {
                    lblStatus.Text = "✏️ Modo de edição - Altere os dados e clique em 'Salvar'";
                }
                else
                {
                    lblStatus.Text = "🟢 Pronto para gerenciar departamentos";
                }
                
                // Habilita controles
                DesabilitarControles(true);
                
                // Atualiza botões baseado no modo
                AtualizarBotoes();
            }
        }
        
        private void AtualizarBotoes()
        {
            var temTexto = !string.IsNullOrWhiteSpace(txtNomeDepartamento.Text);
            var temSelecao = !string.IsNullOrEmpty(LblId.Text) && LblId.Text != "IdDepartamento";
            
            if (_modoEdicao)
            {
                btnCadastrar.Text = "💾 Salvar";
                btnCadastrar.BackColor = Color.FromArgb(52, 152, 219);
                btnCadastrar.Enabled = temTexto;
                
                btnAlterar.Text = "🚫 Cancelar";
                btnAlterar.BackColor = Color.FromArgb(149, 165, 166);
                
                btnExcluir.Enabled = false;
                btnConsultar.Enabled = false;
            }
            else
            {
                btnCadastrar.Text = "➕ Cadastrar";
                btnCadastrar.BackColor = Color.FromArgb(46, 204, 113);
                btnCadastrar.Enabled = temTexto;
                
                btnAlterar.Text = "✏️ Alterar";
                btnAlterar.BackColor = Color.FromArgb(230, 126, 34);
                btnAlterar.Enabled = temSelecao;
                
                btnExcluir.Enabled = temSelecao;
                btnConsultar.Enabled = true;
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
        
        private void DesabilitarControles(bool habilitar)
        {
            txtNomeDepartamento.Enabled = habilitar;
            pnButtons.Enabled = habilitar;
            lstGrid.Enabled = habilitar;
        }
        
        #endregion
        
        #region Modos de Operação
        
        private void AtivarModoEdicao()
        {
            if (string.IsNullOrEmpty(LblId.Text) || LblId.Text == "IdDepartamento")
            {
                ExibirAviso("Selecione um departamento para alterar.");
                return;
            }
            
            _modoEdicao = true;
            _departamentoIdEdicao = LblId.Text;
            
            txtNomeDepartamento.Enabled = true;
            txtNomeDepartamento.Focus();
            txtNomeDepartamento.SelectAll();
            
            AtualizarStatusInterface();
            
            DepartamentoLogger.LogInfo($"Modo de edição ativado para departamento ID: {_departamentoIdEdicao}", "UI");
        }
        
        private void DesativarModoEdicao()
        {
            _modoEdicao = false;
            _departamentoIdEdicao = "";
            
            LimparCampos();
            AtualizarStatusInterface();
            
            DepartamentoLogger.LogInfo("Modo de edição desativado", "UI");
        }
        
        #endregion
        
        #region Eventos do Grid
        
        private void lstGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= _departamentosList.Count) return;
                
                var departamento = _departamentosList[e.RowIndex];
                
                txtNomeDepartamento.Text = departamento.NomeDepartamento;
                LblId.Text = departamento.Id;
                txtNomeDepartamento.Enabled = false;
                
                AtualizarStatusInterface();
                
                DepartamentoLogger.LogInfo($"Departamento selecionado: ID={departamento.Id}, Nome={departamento.NomeDepartamento}", "Selection");
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro ao selecionar departamento: {ex.Message}", "Selection", ex);
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        private void LimparCampos()
        {
            txtNomeDepartamento.Clear();
            txtNomeDepartamento.Enabled = true;
            LblId.Text = "IdDepartamento";
            
            ValidarCampo();
            AtualizarStatusInterface();
            
            txtNomeDepartamento.Focus();
            
            DepartamentoLogger.LogInfo("Campos limpos", "UI");
        }
        
        private void FecharFormulario()
        {
            try
            {
                if (_modoEdicao)
                {
                    var resultado = MessageBox.Show(
                        "Há alterações não salvas. Deseja realmente sair?",
                        "Confirmação",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    if (resultado == DialogResult.No) return;
                }
                
                DepartamentoLogger.LogInfo("Formulário de departamentos fechado", "Shutdown");
                this.Close();
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "Shutdown", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - GERENCIAR DEPARTAMENTOS\n\n" +
                       "📝 OPERAÇÕES:\n" +
                       "• Cadastrar - Adiciona novo departamento\n" +
                       "• Consultar - Busca por nome (deixe vazio para listar todos)\n" +
                       "• Alterar - Edita departamento selecionado\n" +
                       "• Excluir - Remove departamento selecionado\n" +
                       "• Atualizar - Recarrega a lista\n" +
                       "• Limpar - Limpa os campos\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• F1 - Esta ajuda\n" +
                       "• F2 - Cadastrar/Salvar\n" +
                       "• F3 - Consultar\n" +
                       "• F4 - Alterar\n" +
                       "• F5 - Atualizar\n" +
                       "• DEL - Excluir\n" +
                       "• ESC - Cancelar/Limpar\n" +
                       "• ENTER - Executar ação no campo\n\n" +
                       "💡 DICAS:\n" +
                       "• Clique duplo na lista para selecionar\n" +
                       "• Nome deve ter entre 2 e 150 caracteres\n" +
                       "• Use a busca para encontrar departamentos específicos\n" +
                       "• Permite letras, números, hífen, sublinhado e ponto";
            
            MessageBox.Show(ajuda, "Ajuda - Gerenciar Departamentos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private async Task<bool> DepartamentoJaExiste(string nome)
        {
            try
            {
                var response = await _departamentoService.ListarDepartamentoPorNomeDepartamento(nome);
                return response.IsValidResponse() && response.data != null && response.data.Any();
            }
            catch (Exception ex)
            {
                DepartamentoLogger.LogWarning($"Erro ao verificar duplicação: {ex.Message}", "VerificacaoDuplicacao");
                return false;
            }
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
        
        #region Métodos de Validação de Entrada
        
        private void txtNomeDepartamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionou Enter, executa cadastro
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnCadastrar.PerformClick();
                return;
            }
            
            // Permite backspace, delete e espaço
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete || e.KeyChar == ' ')
                return;
            
            // Permite apenas letras, números e alguns caracteres especiais para departamentos
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '_' && e.KeyChar != '.')
            {
                e.Handled = true;
                ExibirFeedbackEntradaInvalida();
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
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantidos para compatibilidade com código existente
        public async Task Consultar()
        {
            await AtualizarLista();
        }
        
        private async Task ConsultarPorNomeDepartamento(string nomeDepartamento)
        {
            await ConsultarPorNome(nomeDepartamento);
        }
        
        private void DefinirCabecalhos(List<string> listaCabecalhos)
        {
            // Método legado - nova implementação usa FormatarGrid()
            FormatarGrid();
        }
        
        private bool ValidarCamposCadastro()
        {
            return ValidarDados();
        }
        
        private bool ValidarSelecao(string acao)
        {
            if (string.IsNullOrEmpty(txtNomeDepartamento.Text?.Trim()) || string.IsNullOrEmpty(LblId.Text))
            {
                DepartamentoLogger.LogWarning($"Tentativa de {acao} sem seleção", "Validacao");
                ExibirAviso($"Selecione um departamento para {acao}.");
                return false;
            }
            
            return true;
        }
        
        private void CadDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            DepartamentoLogger.LogOperation("FormularioFechado");
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class DepartamentoLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [Departamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Console.WriteLine($"[WARN] [Departamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [Departamento-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Console.WriteLine($"[API] [Departamento-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
            
            public static void LogOperation(string operation, string id = null, string value = null)
            {
                var details = string.Empty;
                if (!string.IsNullOrEmpty(id)) details += $" ID={id}";
                if (!string.IsNullOrEmpty(value)) details += $" Value={value}";
                Console.WriteLine($"[OPERATION] [Departamento-{operation}] {DateTime.Now:yyyy-MM-dd HH:mm:ss}{details}");
            }
            
            public static void LogValidationError(string field, string message, string value = null)
            {
                var details = !string.IsNullOrEmpty(value) ? $" Value={value}" : "";
                Console.WriteLine($"[VALIDATION] [Departamento-{field}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{details}");
            }
        }
        
        #endregion
    }
}
