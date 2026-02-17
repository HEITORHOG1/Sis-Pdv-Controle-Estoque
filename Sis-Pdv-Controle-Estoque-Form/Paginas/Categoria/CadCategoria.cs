using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class CadCategoria : Form
    {
        #region Campos Privados
        
        private CategoriaService _categoriaService;
        private BindingList<CategoriaDto> _categoriasList;
        private bool _isLoading = false;
        private bool _modoEdicao = false;
        private string _categoriaIdEdicao = "";
        
        #endregion
        
        #region Construtor e Inicialização
        
        public CadCategoria()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Inicializa serviços e coleções
            _categoriaService = new CategoriaService();
            _categoriasList = new BindingList<CategoriaDto>();
            
            // Configura DataGridView
            ConfigurarDataGridView();
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Log de inicialização
            CategoriaLogger.LogInfo("Formulário de gerenciamento de categorias inicializado", "Startup");
        }
        
        private void ConfigurarDataGridView()
        {
            lstGridCategoria.DataSource = _categoriasList;
            lstGridCategoria.AutoGenerateColumns = true;
            
            // Configurações de estilo moderno
            lstGridCategoria.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 196, 15);
            lstGridCategoria.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            lstGridCategoria.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lstGridCategoria.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            lstGridCategoria.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            lstGridCategoria.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            lstGridCategoria.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 196, 15);
            lstGridCategoria.DefaultCellStyle.SelectionForeColor = Color.White;
            
            lstGridCategoria.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
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
                await AlterarCategoria();
            }
            else
            {
                await CadastrarCategoria();
            }
        }
        
        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            await ConsultarCategorias();
        }
        
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            AtivarModoEdicao();
        }
        
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            await ExcluirCategoria();
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
        
        private void txtNomeCategoria_Enter(object sender, EventArgs e)
        {
            lblInputIcon.Text = "✏️";
            lblInputIcon.ForeColor = Color.FromArgb(241, 196, 15);
            pnInput.BackColor = Color.FromArgb(255, 251, 230); // Amarelo muito claro
        }
        
        private void txtNomeCategoria_Leave(object sender, EventArgs e)
        {
            ValidarCampo();
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
        }
        
        private void txtNomeCategoria_TextChanged(object sender, EventArgs e)
        {
            ValidarCampo();
            AtualizarStatusInterface();
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void CadCategoria_KeyDown(object sender, KeyEventArgs e)
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
                    if (txtNomeCategoria.Focused)
                    {
                        btnCadastrar.PerformClick();
                    }
                    break;
            }
        }
        
        private async void CadCategoria_Load(object sender, EventArgs e)
        {
            try
            {
                SetLoadingState(true);
                await AtualizarLista();
                txtNomeCategoria.Focus();
                
                CategoriaLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                CategoriaLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
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
        
        private async Task CadastrarCategoria()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados()) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeCategoria.Text.Trim();
                var dto = new CategoriaDto { NomeCategoria = nome };
                
                CategoriaLogger.LogInfo($"Iniciando cadastro da categoria: {nome}", "Create");
                
                var response = await _categoriaService.Adicionar(dto);
                
                sw.Stop();
                CategoriaLogger.LogApiCall("Adicionar", "POST", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    ExibirSucesso($"Categoria '{response.data.NomeCategoria}' cadastrada com sucesso!");
                    LimparCampos();
                    await AtualizarLista();
                    
                    CategoriaLogger.LogInfo($"Categoria cadastrada: ID={response.data.id}, Nome={response.data.NomeCategoria}", "Create");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao cadastrar categoria.";
                    ExibirErro("Erro no Cadastro", erro);
                    CategoriaLogger.LogWarning($"Falha no cadastro: {erro}", "Create");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                CategoriaLogger.LogError($"Erro ao cadastrar categoria: {ex.Message}", "Create", ex);
                ExibirErro("Erro Inesperado", $"Erro ao cadastrar categoria: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task AlterarCategoria()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarDados() || string.IsNullOrEmpty(_categoriaIdEdicao)) return;
                
                SetLoadingState(true);
                
                var nome = txtNomeCategoria.Text.Trim();
                var dto = new CategoriaDto 
                { 
                    id = Guid.Parse(_categoriaIdEdicao),
                    NomeCategoria = nome 
                };
                
                CategoriaLogger.LogInfo($"Iniciando alteração da categoria: ID={_categoriaIdEdicao}, Nome={nome}", "Update");
                
                var response = await _categoriaService.AlterarCategoria(dto);
                
                sw.Stop();
                CategoriaLogger.LogApiCall("AlterarCategoria", "PUT", sw.Elapsed, response?.success == true);
                
                if (response?.success == true)
                {
                    ExibirSucesso($"Categoria alterada para '{response.data.NomeCategoria}' com sucesso!");
                    DesativarModoEdicao();
                    await AtualizarLista();
                    
                    CategoriaLogger.LogInfo($"Categoria alterada: ID={response.data.id}, Nome={response.data.NomeCategoria}", "Update");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao alterar categoria.";
                    ExibirErro("Erro na Alteração", erro);
                    CategoriaLogger.LogWarning($"Falha na alteração: {erro}", "Update");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                CategoriaLogger.LogError($"Erro ao alterar categoria: {ex.Message}", "Update", ex);
                ExibirErro("Erro Inesperado", $"Erro ao alterar categoria: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task ExcluirCategoria()
        {
            try
            {
                if (string.IsNullOrEmpty(LblId.Text) || LblId.Text == "IdCategoria")
                {
                    ExibirAviso("Selecione uma categoria para excluir.");
                    return;
                }
                
                var NomeCategoria = txtNomeCategoria.Text.Trim();
                var confirmacao = MessageBox.Show(
                    $"⚠️ ATENÇÃO - Exclusão de Categoria\n\n" +
                    $"Categoria: {NomeCategoria}\n\n" +
                    $"Esta ação não pode ser desfeita.\n" +
                    $"Tem certeza que deseja excluir esta categoria?",
                    "Confirmar Exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (confirmacao == DialogResult.No) return;
                
                SetLoadingState(true);
                
                CategoriaLogger.LogInfo($"Iniciando exclusão da categoria: ID={LblId.Text}, Nome={NomeCategoria}", "Delete");
                
                var response = await _categoriaService.RemoverCategoria(LblId.Text);
                
                if (response?.success == true)
                {
                    ExibirSucesso($"Categoria '{NomeCategoria}' excluída com sucesso!");
                    LimparCampos();
                    await AtualizarLista();
                    
                    CategoriaLogger.LogInfo($"Categoria excluída: ID={LblId.Text}, Nome={NomeCategoria}", "Delete");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao excluir categoria.";
                    ExibirErro("Erro na Exclusão", erro);
                    CategoriaLogger.LogWarning($"Falha na exclusão: {erro}", "Delete");
                }
            }
            catch (Exception ex)
            {
                CategoriaLogger.LogError($"Erro ao excluir categoria: {ex.Message}", "Delete", ex);
                ExibirErro("Erro Inesperado", $"Erro ao excluir categoria: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task ConsultarCategorias()
        {
            try
            {
                var termoBusca = txtNomeCategoria.Text.Trim();
                
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
                CategoriaLogger.LogError($"Erro na consulta: {ex.Message}", "Search", ex);
                ExibirErro("Erro na Consulta", ex.Message);
            }
        }
        
        private async Task ConsultarPorNome(string nome)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                CategoriaLogger.LogInfo($"Consultando categorias por nome: {nome}", "Search");
                
                var response = await _categoriaService.ListarCategoriaPorNomeCategoria(nome);
                
                sw.Stop();
                CategoriaLogger.LogApiCall("ListarCategoriaPorNome", "GET", sw.Elapsed, response?.success == true);
                
                if (response?.success == true && response.data != null)
                {
                    _categoriasList.Clear();
                    foreach (var categoria in response.data)
                    {
                        _categoriasList.Add(categoria);
                    }
                    
                    FormatarGrid();
                    AtualizarContadores();
                    
                    var quantidade = response.data.Count();
                    lblStatus.Text = $"🔍 Encontrada(s) {quantidade} categoria(s) com '{nome}'";
                    
                    CategoriaLogger.LogInfo($"Consulta concluída: {quantidade} categoria(s) encontrada(s)", "Search");
                }
                else
                {
                    _categoriasList.Clear();
                    FormatarGrid();
                    AtualizarContadores();
                    
                    ExibirAviso($"Nenhuma categoria encontrada com o nome '{nome}'.");
                    CategoriaLogger.LogInfo($"Nenhuma categoria encontrada com o nome: {nome}", "Search");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                CategoriaLogger.LogError($"Erro na consulta por nome: {ex.Message}", "Search", ex);
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
                
                CategoriaLogger.LogInfo("Atualizando lista de categorias", "Read");
                
                var response = await _categoriaService.ListarCategoria();
                
                sw.Stop();
                CategoriaLogger.LogApiCall("ListarCategoria", "GET", sw.Elapsed, response?.success == true);
                
                if (response?.success == true && response.data != null)
                {
                    _categoriasList.Clear();
                    foreach (var categoria in response.data)
                    {
                        _categoriasList.Add(categoria);
                    }
                    
                    FormatarGrid();
                    AtualizarContadores();
                    
                    var quantidade = response.data.Count();
                    lblStatus.Text = $"✅ Lista atualizada com {quantidade} categoria(s)";
                    
                    CategoriaLogger.LogInfo($"Lista atualizada com {quantidade} categoria(s)", "Read");
                }
                else
                {
                    var erro = response?.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao consultar categorias.";
                    ExibirErro("Erro na Consulta", erro);
                    CategoriaLogger.LogWarning($"Falha na consulta: {erro}", "Read");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                CategoriaLogger.LogError($"Erro ao atualizar lista: {ex.Message}", "Read", ex);
                ExibirErro("Erro Inesperado", $"Erro ao carregar categorias: {ex.Message}");
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
                if (lstGridCategoria.Columns.Count == 0) return;
                
                // Configura cabeçalhos
                var cabecalhos = new Dictionary<string, string>
                {
                    ["NomeCategoria"] = "🏷️ Nome da Categoria",
                    ["id"] = "🔑 ID"
                };
                
                foreach (DataGridViewColumn coluna in lstGridCategoria.Columns)
                {
                    if (cabecalhos.ContainsKey(coluna.Name))
                    {
                        coluna.HeaderText = cabecalhos[coluna.Name];
                        coluna.Visible = true;
                        
                        // Configurações específicas por coluna
                        switch (coluna.Name)
                        {
                            case "NomeCategoria":
                                coluna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                coluna.MinimumWidth = 200;
                                coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                break;
                            case "id":
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
                
                lstGridCategoria.Refresh();
            }
            catch (Exception ex)
            {
                CategoriaLogger.LogError($"Erro ao formatar grid: {ex.Message}", "UI", ex);
            }
        }
        
        private void AtualizarContadores()
        {
            var total = _categoriasList.Count;
            lblGridInfo.Text = $"📋 Lista de Categorias ({total})";
            lblContador.Text = $"📊 {total} categoria(s) cadastrada(s)";
        }
        
        #endregion
        
        #region Validação e Estado
        
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
                return false;
            }
            
            if (nome.Length > 100)
            {
                ExibirAviso("O nome da categoria não pode ter mais de 100 caracteres.");
                txtNomeCategoria.Focus();
                return false;
            }
            
            return true;
        }
        
        private void ValidarCampo()
        {
            var nome = txtNomeCategoria.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(nome))
            {
                lblInputIcon.Text = "🏷️";
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
                    lblStatus.Text = "🟢 Pronto para gerenciar categorias";
                }
                
                // Habilita controles
                DesabilitarControles(true);
                
                // Atualiza botões baseado no modo
                AtualizarBotoes();
            }
        }
        
        private void AtualizarBotoes()
        {
            var temTexto = !string.IsNullOrWhiteSpace(txtNomeCategoria.Text);
            var temSelecao = !string.IsNullOrEmpty(LblId.Text) && LblId.Text != "IdCategoria";
            
            if (_modoEdicao)
            {
                btnCadastrar.Text = "💾 Salvar";
                btnCadastrar.BackColor = Color.FromArgb(241, 196, 15);
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
            txtNomeCategoria.Enabled = habilitar;
            pnButtons.Enabled = habilitar;
            lstGridCategoria.Enabled = habilitar;
        }
        
        #endregion
        
        #region Modos de Operação
        
        private void AtivarModoEdicao()
        {
            if (string.IsNullOrEmpty(LblId.Text) || LblId.Text == "IdCategoria")
            {
                ExibirAviso("Selecione uma categoria para alterar.");
                return;
            }
            
            _modoEdicao = true;
            _categoriaIdEdicao = LblId.Text;
            
            txtNomeCategoria.Enabled = true;
            txtNomeCategoria.Focus();
            txtNomeCategoria.SelectAll();
            
            AtualizarStatusInterface();
            
            CategoriaLogger.LogInfo($"Modo de edição ativado para categoria ID: {_categoriaIdEdicao}", "UI");
        }
        
        private void DesativarModoEdicao()
        {
            _modoEdicao = false;
            _categoriaIdEdicao = "";
            
            LimparCampos();
            AtualizarStatusInterface();
            
            CategoriaLogger.LogInfo("Modo de edição desativado", "UI");
        }
        
        #endregion
        
        #region Eventos do Grid
        
        private void lstGridCategoria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= _categoriasList.Count) return;
                
                var categoria = _categoriasList[e.RowIndex];
                
                txtNomeCategoria.Text = categoria.NomeCategoria;
                LblId.Text = categoria.id.ToString();
                txtNomeCategoria.Enabled = false;
                
                AtualizarStatusInterface();
                
                CategoriaLogger.LogInfo($"Categoria selecionada: ID={categoria.id}, Nome={categoria.NomeCategoria}", "Selection");
            }
            catch (Exception ex)
            {
                CategoriaLogger.LogError($"Erro ao selecionar categoria: {ex.Message}", "Selection", ex);
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        private void LimparCampos()
        {
            txtNomeCategoria.Clear();
            txtNomeCategoria.Enabled = true;
            LblId.Text = "IdCategoria";
            
            ValidarCampo();
            AtualizarStatusInterface();
            
            txtNomeCategoria.Focus();
            
            CategoriaLogger.LogInfo("Campos limpos", "UI");
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
                
                CategoriaLogger.LogInfo("Formulário de categorias fechado", "Shutdown");
                this.Close();
            }
            catch (Exception ex)
            {
                CategoriaLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "Shutdown", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - GERENCIAR CATEGORIAS\n\n" +
                       "📝 OPERAÇÕES:\n" +
                       "• Cadastrar - Adiciona nova categoria\n" +
                       "• Consultar - Busca por nome (deixe vazio para listar todas)\n" +
                       "• Alterar - Edita categoria selecionada\n" +
                       "• Excluir - Remove categoria selecionada\n" +
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
                       "• Nome deve ter entre 2 e 100 caracteres\n" +
                       "• Use a busca para encontrar categorias específicas";
            
            MessageBox.Show(ajuda, "Ajuda - Gerenciar Categorias",
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
        
        // Mantidos para compatibilidade com código existente
        private async void btnCadastrar_Click_1(object sender, EventArgs e)
        {
            await CadastrarCategoria();
        }
        
        private async void btnAtualziar_Click(object sender, EventArgs e)
        {
            await AtualizarLista();
        }
        
        public async Task Consultar()
        {
            await AtualizarLista();
        }
        
        private async Task ConsultarPorNomeCategoria(string NomeCategoria)
        {
            await ConsultarPorNome(NomeCategoria);
        }
        
        private void DefinirCabecalhos(List<string> listaCabecalhos)
        {
            // Método legado - nova implementação usa FormatarGrid()
            FormatarGrid();
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class CategoriaLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [Categoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [Categoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [Categoria-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [Categoria-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
        }
        
        #endregion
    }
}







