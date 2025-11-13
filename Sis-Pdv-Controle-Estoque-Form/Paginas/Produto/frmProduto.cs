using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Produto
{
    public partial class frmProduto : Form
    {
        private ProdutoService produtoService;
        private FornecedorService fornecedorService;
        private CategoriaService categoriaService;
        private BindingList<ProdutoDto> produtosList;
        private bool isLoading = false;

        public frmProduto()
        {
            InitializeComponent();
            produtoService = new ProdutoService();
            fornecedorService = new FornecedorService();
            categoriaService = new CategoriaService();
            produtosList = new BindingList<ProdutoDto>();
            
            ProdutoLogger.LogInfo("Formulário de cadastro de produto inicializado", "FormLoad");
        }

        private async void frmProduto_Load(object sender, EventArgs e)
        {
            ProdutoLogger.LogOperation("CarregamentoFormulario");
            
            rbPerecivel.Checked = true;
            rbProdutoAtivo.Checked = true;
            
            await CarregarDados();
        }

        private async Task CarregarDados()
        {
            await PreencherComboFornecedor();
            await PreencherComboCategoria();
            await Consultar();
        }

        // Classe auxiliar para binding em ComboBoxes
        private class ComboItem
        {
            public string Id { get; set; } = string.Empty;
            public string Nome { get; set; } = string.Empty;
        }

        private async Task PreencherComboCategoria()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await categoriaService.ListarCategoria();

                sw.Stop();
                ProdutoLogger.LogApiCall("ListarCategoria", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse() && response.data != null && response.data.Count > 0)
                {
                    // Criar uma lista com item padrão e dados
                    var lista = new List<ComboItem>
                    {
                        new ComboItem { Id = Guid.Empty.ToString(), Nome = "Selecione uma categoria" }
                    };
                    lista.AddRange(response.data.Select(c => new ComboItem { Id = c.id.ToString(), Nome = c.NomeCategoria }));

                    // Prevenir conflito com itens adicionados no Designer
                    cmbCategoria.DataSource = null;
                    cmbCategoria.Items.Clear();
                    // Definir membros antes do DataSource
                    cmbCategoria.DisplayMember = "Nome";
                    cmbCategoria.ValueMember = "Id";
                    cmbCategoria.DataSource = lista;
                    cmbCategoria.SelectedIndex = 0; // Seleciona o item padrão
                    
                    ProdutoLogger.LogInfo($"Combo categoria carregado com {response.data.Count} itens", "CarregarCategoria");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogError($"Erro ao carregar categorias: {erros}", "CarregarCategoria");
                    MessageBox.Show($"Erro ao carregar categorias:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError("Erro inesperado ao carregar categorias", "CarregarCategoria", ex);
                MessageBox.Show($"Erro inesperado ao carregar categorias: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task PreencherComboFornecedor()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await fornecedorService.ListarFornecedor();

                sw.Stop();
                ProdutoLogger.LogApiCall("ListarFornecedor", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse() && response.data != null && response.data.Count > 0)
                {
                    // Criar lista com item padrão e dados
                    var lista = new List<ComboItem>
                    {
                        new ComboItem { Id = Guid.Empty.ToString(), Nome = "Selecione um fornecedor" }
                    };
                    lista.AddRange(response.data.Select(f => new ComboItem { Id = f.id, Nome = f.nomeFantasia }));

                    // Prevenir conflito com itens adicionados no Designer
                    cmbFornecedor.DataSource = null;
                    cmbFornecedor.Items.Clear();
                    // Definir membros antes do DataSource
                    cmbFornecedor.DisplayMember = "Nome";
                    cmbFornecedor.ValueMember = "Id";
                    cmbFornecedor.DataSource = lista;
                    cmbFornecedor.SelectedIndex = 0; // Selecionar o item padrão
                    
                    ProdutoLogger.LogInfo($"Combo fornecedor carregado com {response.data.Count} itens", "CarregarFornecedor");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogError($"Erro ao carregar fornecedores: {erros}", "CarregarFornecedor");
                    MessageBox.Show($"Erro ao carregar fornecedores:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError("Erro inesperado ao carregar fornecedores", "CarregarFornecedor", ex);
                MessageBox.Show($"Erro inesperado ao carregar fornecedores: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task Consultar()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                isLoading = true;
                SetLoadingState(true);

                ProdutoLogger.LogOperation("ListagemIniciada");

                var response = await produtoService.ListarProduto();
                
                sw.Stop();
                ProdutoLogger.LogApiCall("ListarProduto", "GET", sw.Elapsed, response.IsValidResponse());
                ProdutoLogger.LogPerformance("ListarProdutos", sw.Elapsed, response.data?.Count ?? 0);

                if (response.IsValidResponse())
                {
                    produtosList.Clear();
                        if (response.data != null)
                        {
                    foreach (var produto in response.data)
                    {
                        produtosList.Add(produto.ToDto());
                    }
                        }

                    dgvProduto.DataSource = null;
                    dgvProduto.DataSource = produtosList;
                    DefinirCabecalhos(new List<string>() {
                        "ID", "Cód barras", "Nome", "Descrição", "P. Custo", "P. Venda",
                        "Margem", "Dt. Fabri.", "Dt. Venci.", "Qtd.", "Fornecedor", "Categoria", "Ativo"
                    });

                    // Oculta colunas desnecessárias
                    if (dgvProduto.Columns["Id"] != null)
                        dgvProduto.Columns["Id"].Visible = false;

                    // Formata colunas de preço
                    FormatarColunas();
                    
                    // Aplica cores baseadas em alertas
                    AplicarCoresAlertas();

                    dgvProduto.Refresh();
                    
                    ProdutoLogger.LogInfo($"Listagem concluída com {produtosList.Count} produtos", "Listagem");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogError($"Erro na listagem: {erros}", "Listagem");
                    MessageBox.Show($"Erro ao consultar produtos:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError("Erro inesperado ao consultar produtos", "Listagem", ex);
                MessageBox.Show($"Erro inesperado ao consultar produtos: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task ConsultarPorCodigodeBarras(string codigoBarras)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrWhiteSpace(codigoBarras))
                {
                    MessageBox.Show("Por favor, digite um código de barras válido.", "Campo Obrigatório", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                isLoading = true;
                SetLoadingState(true);

                ProdutoLogger.LogBuscaCodigoBarras(codigoBarras, false);

                var response = await produtoService.ListarProdutoPorCodBarras(codigoBarras);
                
                sw.Stop();
                ProdutoLogger.LogApiCall("ListarProdutoPorCodBarras", "GET", sw.Elapsed, response.IsValidResponse());
                
                var encontrado = response.IsValidResponse() && response.data?.Count > 0;
                ProdutoLogger.LogBuscaCodigoBarras(codigoBarras, encontrado, response.data?.Count ?? 0);

                if (encontrado)
                {
                    produtosList.Clear();
                    foreach (var produto in response.data)
                    {
                        produtosList.Add(produto.ToDto());
                    }

                    dgvProduto.DataSource = null;
                    dgvProduto.DataSource = produtosList;
                    DefinirCabecalhos(new List<string>() {
                        "ID", "Cód barras", "Nome", "Descrição", "P. Custo", "P. Venda",
                        "Margem", "Dt. Fabri.", "Dt. Venci.", "Qtd.", "Fornecedor", "Categoria", "Ativo"
                    });

                    FormatarColunas();
                    AplicarCoresAlertas();
                    
                    ProdutoLogger.LogInfo($"Busca por código encontrou {response.data.Count} produto(s)", "BuscaCodigo");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogWarning($"Produto não encontrado para código: {codigoBarras}", "BuscaCodigo");
                    MessageBox.Show($"Nenhum produto encontrado para o código: {codigoBarras}", "Produto Não Encontrado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError($"Erro ao buscar produto por código: {codigoBarras}", "BuscaCodigo", ex);
                MessageBox.Show($"Erro ao consultar produto por código de barras: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await CadastrarProduto();
        }

        private async Task CadastrarProduto()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                // Debug: Verificar valores dos combos antes da conversão
                ProdutoLogger.LogInfo($"Debug ComboBoxes - Fornecedor SelectedValue: {cmbFornecedor.SelectedValue}, " +
                                    $"Categoria SelectedValue: {cmbCategoria.SelectedValue}", "DebugCombos");

                // Converte e valida GUIDs selecionados
                if (!Guid.TryParse(cmbFornecedor.SelectedValue?.ToString(), out var fornecedorId) || fornecedorId == Guid.Empty)
                {
                    MessageBox.Show("Selecione um fornecedor válido.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbFornecedor.Focus();
                    return;
                }

                if (!Guid.TryParse(cmbCategoria.SelectedValue?.ToString(), out var categoriaId) || categoriaId == Guid.Empty)
                {
                    MessageBox.Show("Selecione uma categoria válida.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCategoria.Focus();
                    return;
                }

                var dto = new ProdutoDto()
                {
                    codBarras = txbCodigoBarras.Text.Trim(),
                    FornecedorId = fornecedorId,
                    CategoriaId = categoriaId,
                    nomeProduto = txbNome.Text.Trim(),
                    descricaoProduto = rtbDescricao.Text.Trim(),
                    precoCusto = decimal.Parse(txbPrecoCusto.Text),
                    precoVenda = decimal.Parse(txbPrecoDeVenda.Text),
                    margemLucro = decimal.Parse(txbMargemDeLucro.Text),
                    dataFabricao = GetDataFabricacao(),
                    dataVencimento = GetDataVencimento(),
                    quatidadeEstoqueProduto = int.Parse(txbQuantidadeEstoque.Text),
                    statusAtivo = rbProdutoAtivo.Checked ? 1 : 0
                };

                // Log dos dados antes do envio
                ProdutoLogger.LogInfo($"Dados do produto antes do envio - Nome: {dto.nomeProduto}, " +
                                    $"Código: {dto.codBarras}, FornecedorId: {dto.FornecedorId}, " +
                                    $"CategoriaId: {dto.CategoriaId}", "CadastroDebug");

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    ProdutoLogger.LogValidationError("Produto", mensagem, dto.nomeProduto);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProdutoLogger.LogOperation("CadastroIniciado", "", dto.nomeProduto);

                // Verifica se já existe produto com o mesmo código de barras
                if (await ProdutoJaExiste(dto.codBarras))
                {
                    ProdutoLogger.LogWarning($"Tentativa de cadastro de produto duplicado: {dto.codBarras}", "Cadastro");
                    MessageBox.Show("Já existe um produto com este código de barras.", "Produto Duplicado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var response = await produtoService.AdicionarProduto(dto);

                sw.Stop();
                ProdutoLogger.LogApiCall("AdicionarProduto", "POST", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    LimparCampos();
                    var mensagem = $"Produto '{response.data.nomeProduto}' inserido com sucesso!";
                    
                    ProdutoLogger.LogOperation("CadastroRealizado", response.data.Id.ToString(), response.data.nomeProduto, response.data.codBarras);
                    ProdutoLogger.LogEstoque(response.data.nomeProduto, 0, response.data.quatidadeEstoqueProduto, "Cadastro");
                    ProdutoLogger.LogMargemLucro(response.data.nomeProduto, response.data.precoCusto, response.data.precoVenda, response.data.margemLucro);
                    
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogError($"Erro no cadastro: {erros}", "Cadastro");
                    MessageBox.Show($"Erro ao cadastrar produto:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError("Erro inesperado ao cadastrar produto", "Cadastro", ex);
                
                // Log mais detalhado do erro
                var innerException = ex.InnerException?.Message ?? "Nenhuma";
                ProdutoLogger.LogError($"Detalhes do erro - Message: {ex.Message}, Inner: {innerException}, StackTrace: {ex.StackTrace}", "CadastroDetalhado");
                
                MessageBox.Show($"Erro inesperado ao cadastrar produto: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task AlterarProduto()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarSelecao("alterar")) return;
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                // Converte e valida GUIDs selecionados
                if (!Guid.TryParse(cmbFornecedor.SelectedValue?.ToString(), out var fornecedorIdAlt) || fornecedorIdAlt == Guid.Empty)
                {
                    MessageBox.Show("Selecione um fornecedor válido.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbFornecedor.Focus();
                    return;
                }

                if (!Guid.TryParse(cmbCategoria.SelectedValue?.ToString(), out var categoriaIdAlt) || categoriaIdAlt == Guid.Empty)
                {
                    MessageBox.Show("Selecione uma categoria válida.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbCategoria.Focus();
                    return;
                }

                var dto = new ProdutoDto()
                {
                    Id = Guid.Parse(txbId.Text),
                    codBarras = txbCodigoBarras.Text.Trim(),
                    FornecedorId = fornecedorIdAlt,
                    CategoriaId = categoriaIdAlt,
                    nomeProduto = txbNome.Text.Trim(),
                    descricaoProduto = rtbDescricao.Text.Trim(),
                    precoCusto = decimal.Parse(txbPrecoCusto.Text),
                    precoVenda = decimal.Parse(txbPrecoDeVenda.Text),
                    margemLucro = decimal.Parse(txbMargemDeLucro.Text),
                    dataFabricao = GetDataFabricacao(),
                    dataVencimento = GetDataVencimento(),
                    quatidadeEstoqueProduto = int.Parse(txbQuantidadeEstoque.Text),
                    statusAtivo = rbProdutoAtivo.Checked ? 1 : 0
                };

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    ProdutoLogger.LogValidationError("Produto", mensagem, dto.nomeProduto);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProdutoLogger.LogOperation("AlteracaoIniciada", dto.Id.ToString(), dto.nomeProduto, dto.codBarras);

                var response = await produtoService.AlterarProduto(dto);

                sw.Stop();
                ProdutoLogger.LogApiCall("AlterarProduto", "PUT", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    LimparCampos();
                    var mensagem = $"Produto '{response.data.nomeProduto}' alterado com sucesso!";
                    
                    ProdutoLogger.LogOperation("AlteracaoRealizada", response.data.Id.ToString(), response.data.nomeProduto, response.data.codBarras);
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    ProdutoLogger.LogError($"Erro na alteração: {erros}", "Alteracao");
                    MessageBox.Show($"Erro ao alterar produto:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                ProdutoLogger.LogError("Erro inesperado ao alterar produto", "Alteracao", ex);
                MessageBox.Show($"Erro inesperado ao alterar produto: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private bool ValidarCamposCadastro()
        {
            // Validação Código de Barras
            if (string.IsNullOrWhiteSpace(txbCodigoBarras.Text))
            {
                MessageBox.Show("Informe o código de barras do produto.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbCodigoBarras.Focus();
                return false;
            }

            var codigo = txbCodigoBarras.Text.Trim();
            if (!Regex.IsMatch(codigo, @"^[0-9]{8,20}$"))
            {
                MessageBox.Show("Código de barras deve conter apenas números e ter entre 8 e 20 dígitos.",
                    "Código Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbCodigoBarras.Focus();
                return false;
            }

            // Validação Fornecedor
            if (cmbFornecedor.SelectedValue == null || 
                cmbFornecedor.SelectedValue.ToString() == Guid.Empty.ToString())
            {
                MessageBox.Show("Selecione um fornecedor.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbFornecedor.Focus();
                return false;
            }

            // Validação Categoria
            if (cmbCategoria.SelectedValue == null || 
                cmbCategoria.SelectedValue.ToString() == Guid.Empty.ToString())
            {
                MessageBox.Show("Selecione uma categoria.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return false;
            }

            // Validação Nome
            if (string.IsNullOrWhiteSpace(txbNome.Text))
            {
                MessageBox.Show("Informe o nome do produto.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNome.Focus();
                return false;
            }

            // Validação Preço de Custo
            if (!decimal.TryParse(txbPrecoCusto.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precoCusto) || precoCusto <= 0)
            {
                MessageBox.Show("Informe um preço de custo válido maior que zero.", "Valor Inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPrecoCusto.Focus();
                return false;
            }

            // Validação Preço de Venda
            if (!decimal.TryParse(txbPrecoDeVenda.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precoVenda) || precoVenda <= 0)
            {
                MessageBox.Show("Informe um preço de venda válido maior que zero.", "Valor Inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPrecoDeVenda.Focus();
                return false;
            }

            // Validação: Preço de venda deve ser maior que custo
            if (precoVenda <= precoCusto)
            {
                MessageBox.Show("O preço de venda deve ser maior que o preço de custo.", "Validação de Negócio",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPrecoDeVenda.Focus();
                return false;
            }

            // Validação Quantidade
            if (!int.TryParse(txbQuantidadeEstoque.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int quantidade) || quantidade < 0)
            {
                MessageBox.Show("Informe uma quantidade em estoque válida (maior ou igual a zero).", "Valor Inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbQuantidadeEstoque.Focus();
                return false;
            }

            // Validação Datas para produtos perecíveis
            if (rbPerecivel.Checked)
            {
                var formats = new[] { "dd/MM/yyyy", "d/M/yyyy" };
                var culture = CultureInfo.GetCultureInfo("pt-BR");

                if (!DateTime.TryParseExact(msktDataFabricacao.Text.Trim(), formats, culture, DateTimeStyles.None, out var dataFab))
                {
                    MessageBox.Show("Informe uma data de fabricação válida (dd/MM/aaaa) para produtos perecíveis.", "Data Inválida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    msktDataFabricacao.Focus();
                    return false;
                }

                if (!DateTime.TryParseExact(msktDataVencimento.Text.Trim(), formats, culture, DateTimeStyles.None, out var dataVenc))
                {
                    MessageBox.Show("Informe uma data de vencimento válida para produtos perecíveis.", "Data Inválida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    msktDataVencimento.Focus();
                    return false;
                }

                // Valida regras de negócio
                if (dataVenc <= dataFab)
                {
                    MessageBox.Show("A data de vencimento deve ser posterior à data de fabricação.", "Validação de Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    msktDataVencimento.Focus();
                    return false;
                }

                if (dataVenc <= DateTime.Today)
                {
                    MessageBox.Show("Não é possível cadastrar produto com data de vencimento expirada.", "Produto Vencido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    msktDataVencimento.Focus();
                    return false;
                }

                // Opcional: fabricação não pode ser futura
                if (dataFab > DateTime.Today)
                {
                    MessageBox.Show("A data de fabricação não pode ser futura.", "Data Inválida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    msktDataFabricacao.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidarSelecao(string acao)
        {
            if (string.IsNullOrEmpty(txbId.Text) || txbId.Text == "IdProduto")
            {
                ProdutoLogger.LogWarning($"Tentativa de {acao} sem seleção", "Validacao");
                MessageBox.Show($"Selecione um produto para {acao}.", "Seleção Necessária", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task<bool> ProdutoJaExiste(string codigoBarras)
        {
            try
            {
                var response = await produtoService.ListarProdutoPorCodBarras(codigoBarras);
                return response.IsValidResponse() && response.data?.Count > 0;
            }
            catch (Exception ex)
            {
                ProdutoLogger.LogWarning($"Erro ao verificar duplicação de código: {ex.Message}", "VerificacaoDuplicacao");
                return false;
            }
        }

        private DateTime GetDataFabricacao()
        {
            if (rbNaoPerecivel.Checked || string.IsNullOrWhiteSpace(msktDataFabricacao.Text))
                return DateTime.MinValue;

            var formats = new[] { "dd/MM/yyyy", "d/M/yyyy" };
            var culture = CultureInfo.GetCultureInfo("pt-BR");
            
            return DateTime.TryParseExact(msktDataFabricacao.Text.Trim(), formats, culture, DateTimeStyles.None, out DateTime data) ? data : DateTime.MinValue;
        }

        private DateTime GetDataVencimento()
        {
            if (rbNaoPerecivel.Checked || string.IsNullOrWhiteSpace(msktDataVencimento.Text))
                return DateTime.MinValue;

            var formats = new[] { "dd/MM/yyyy", "d/M/yyyy" };
            var culture = CultureInfo.GetCultureInfo("pt-BR");
            
            return DateTime.TryParseExact(msktDataVencimento.Text.Trim(), formats, culture, DateTimeStyles.None, out DateTime data) ? data : DateTime.MinValue;
        }

        private void FormatarColunas()
        {
            // Formatar colunas de preço
            if (dgvProduto.Columns["precoCusto"] != null)
                dgvProduto.Columns["precoCusto"].DefaultCellStyle.Format = "C2";
            
            if (dgvProduto.Columns["precoVenda"] != null)
                dgvProduto.Columns["precoVenda"].DefaultCellStyle.Format = "C2";
            
            if (dgvProduto.Columns["margemLucro"] != null)
                dgvProduto.Columns["margemLucro"].DefaultCellStyle.Format = "F2";

            // Formatar datas
            if (dgvProduto.Columns["dataFabricao"] != null)
                dgvProduto.Columns["dataFabricao"].DefaultCellStyle.Format = "dd/MM/yyyy";
            
            if (dgvProduto.Columns["dataVencimento"] != null)
                dgvProduto.Columns["dataVencimento"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void AplicarCoresAlertas()
        {
            foreach (DataGridViewRow row in dgvProduto.Rows)
            {
                if (row.DataBoundItem is ProdutoDto produto)
                {
                    var cor = produto.GetCorAlerta();
                    
                    // Aplica cor apenas se não for verde (normal)
                    if (cor != System.Drawing.Color.Green)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, cor);
                        row.DefaultCellStyle.SelectionBackColor = cor;
                    }
                }
            }
        }

        private async void txbPrecoDeVenda_TextChanged(object sender, EventArgs e)
        {
            await ValidarValores();
        }

        private async void txbPrecoCusto_TextChanged(object sender, EventArgs e)
        {
            await ValidarValores();
        }

        private async void dgvProduto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < produtosList.Count)
                {
                    var produto = produtosList[e.RowIndex];
                    PreencherCamposEdicao(produto);
                    
                    ProdutoLogger.LogOperation("ProdutoSelecionado", produto.Id.ToString(), produto.nomeProduto, produto.codBarras);
                }
            }
            catch (Exception ex)
            {
                ProdutoLogger.LogError("Erro ao carregar dados do produto selecionado", "SelecaoProduto", ex);
                MessageBox.Show($"Erro ao carregar dados do produto: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencherCamposEdicao(ProdutoDto produto)
        {
            txbId.Text = produto.Id.ToString();
            txbCodigoBarras.Text = produto.codBarras;
            txbNome.Text = produto.nomeProduto;
            rtbDescricao.Text = produto.descricaoProduto;
            txbPrecoCusto.Text = produto.precoCusto.ToString("F2");
            txbPrecoDeVenda.Text = produto.precoVenda.ToString("F2");
            txbMargemDeLucro.Text = produto.margemLucro.ToString("F2");
            txbQuantidadeEstoque.Text = produto.quatidadeEstoqueProduto.ToString();

            // Seleciona fornecedor
            if (produto.FornecedorId != Guid.Empty)
            {
                cmbFornecedor.SelectedValue = produto.FornecedorId.ToString();
            }

            // Seleciona categoria
            if (produto.CategoriaId != Guid.Empty)
            {
                cmbCategoria.SelectedValue = produto.CategoriaId.ToString();
            }

            // Define datas
            if (produto.dataFabricao > DateTime.MinValue)
            {
                msktDataFabricacao.Text = produto.dataFabricao.ToString("dd/MM/yyyy");
                rbPerecivel.Checked = true;
            }
            else
            {
                msktDataFabricacao.Text = "";
                rbNaoPerecivel.Checked = true;
            }

            if (produto.dataVencimento > DateTime.MinValue)
            {
                msktDataVencimento.Text = produto.dataVencimento.ToString("dd/MM/yyyy");
            }
            else
            {
                msktDataVencimento.Text = "";
            }

            // Define status
            rbProdutoAtivo.Checked = produto.statusAtivo == 1;
            rbProdutoInativo.Checked = produto.statusAtivo != 1;

            btnAdicionar.Enabled = false;

            // Log alertas do produto
            var alertas = produto.GetAlertas();
            if (alertas.Any())
            {
                ProdutoLogger.LogWarning($"Produto selecionado possui alertas: {string.Join(", ", alertas)}", "AlertaProduto");
            }
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await AlterarProduto();
        }

        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            
            string codigoBarras = txbCodigoBarras.Text.Trim();

            if (!string.IsNullOrEmpty(codigoBarras))
            {
                await ConsultarPorCodigodeBarras(codigoBarras);
            }
            else
            {
                LimparCampos();
                await Consultar();
            }
        }

        private void rbPerecivel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPerecivel.Checked)
            {
                msktDataFabricacao.Enabled = true;
                msktDataVencimento.Enabled = true;
                
                // Define data de fabricação padrão como hoje se estiver vazia
                if (string.IsNullOrWhiteSpace(msktDataFabricacao.Text))
                {
                    msktDataFabricacao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        private void rbNaoPerecivel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNaoPerecivel.Checked)
            {
                msktDataFabricacao.Enabled = false;
                msktDataVencimento.Enabled = false;
                msktDataFabricacao.Text = "";
                msktDataVencimento.Text = "";
            }
        }

        private async void txbCodigoBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void txbPrecoCusto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite números, vírgula e backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permite apenas uma vírgula
            if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private async void txbPrecoDeVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite números, vírgula e backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Permite apenas uma vírgula
            if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void DefinirCabecalhos(List<string> listaCabecalhos)
        {
            int index = 0;

            foreach (DataGridViewColumn coluna in dgvProduto.Columns)
            {
                if (coluna.Visible && index < listaCabecalhos.Count)
                {
                    coluna.HeaderText = listaCabecalhos[index];
                    index++;
                }
            }
        }

        private async Task ValidarValores()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbPrecoDeVenda.Text) || 
                    string.IsNullOrWhiteSpace(txbPrecoCusto.Text) ||
                    txbPrecoCusto.Text == "0" || txbPrecoDeVenda.Text == "0")
                {
                    return;
                }

                if (decimal.TryParse(txbPrecoDeVenda.Text, out decimal precoVenda) &&
                    decimal.TryParse(txbPrecoCusto.Text, out decimal precoCusto) &&
                    precoCusto > 0)
                {
                    decimal margem = ((precoVenda / precoCusto) - 1) * 100;
                    txbMargemDeLucro.Text = margem.ToString("F2");

                    // Log da margem calculada
                    ProdutoLogger.LogMargemLucro(txbNome.Text, precoCusto, precoVenda, margem);

                    // Alerta para margem muito baixa
                    if (margem < 10)
                    {
                        txbMargemDeLucro.BackColor = System.Drawing.Color.LightCoral;
                    }
                    else if (margem < 20)
                    {
                        txbMargemDeLucro.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        txbMargemDeLucro.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                ProdutoLogger.LogError("Erro ao calcular margem de lucro", "CalculoMargem", ex);
            }
        }

        private void LimparCampos()
        {
            txbId.Text = "IdProduto";
            txbCodigoBarras.Clear();
            txbMargemDeLucro.Clear();
            txbNome.Clear();
            txbPrecoDeVenda.Clear();
            txbPrecoCusto.Clear();
            txbQuantidadeEstoque.Clear();
            rtbDescricao.Clear();
            msktDataFabricacao.Text = "";
            msktDataVencimento.Text = "";

            if (cmbFornecedor.Items.Count > 0)
                cmbFornecedor.SelectedIndex = 0;

            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;

            rbProdutoAtivo.Checked = true;
            rbPerecivel.Checked = true;
            btnAdicionar.Enabled = true;

            // Reset cor da margem
            txbMargemDeLucro.BackColor = System.Drawing.SystemColors.Window;
        }

        private void SetLoadingState(bool loading)
        {
            btnAdicionar.Enabled = !loading;
            btnAlterar.Enabled = !loading;
            btnConsulta.Enabled = !loading;
            dgvProduto.Enabled = !loading;

            // Desabilita campos durante carregamento
            txbCodigoBarras.Enabled = !loading;
            txbNome.Enabled = !loading;
            rtbDescricao.Enabled = !loading;
            txbPrecoCusto.Enabled = !loading;
            txbPrecoDeVenda.Enabled = !loading;
            txbQuantidadeEstoque.Enabled = !loading;
            cmbFornecedor.Enabled = !loading;
            cmbCategoria.Enabled = !loading;
            rbProdutoAtivo.Enabled = !loading;
            rbProdutoInativo.Enabled = !loading;
            rbPerecivel.Enabled = !loading;
            rbNaoPerecivel.Enabled = !loading;
            msktDataFabricacao.Enabled = !loading && rbPerecivel.Checked;
            msktDataVencimento.Enabled = !loading && rbPerecivel.Checked;

            if (loading)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Event handlers para sugestões automáticas
        private void txbNome_Leave(object sender, EventArgs e)
        {
            // Gera código de barras se estiver vazio
            if (string.IsNullOrWhiteSpace(txbCodigoBarras.Text) && !string.IsNullOrWhiteSpace(txbNome.Text))
            {
                var dto = new ProdutoDto { nomeProduto = txbNome.Text };
                var categoria = cmbCategoria.Text;
                txbCodigoBarras.Text = dto.GerarCodigoBarrasSugerido(categoria);
                
                ProdutoLogger.LogInfo($"Código de barras gerado automaticamente: {txbCodigoBarras.Text} para produto: {txbNome.Text}", "GeracaoCodigo");
            }
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Atualiza código de barras se necessário
            if (!string.IsNullOrWhiteSpace(txbNome.Text) && string.IsNullOrWhiteSpace(txbCodigoBarras.Text))
            {
                var dto = new ProdutoDto { nomeProduto = txbNome.Text };
                var categoria = cmbCategoria.Text;
                txbCodigoBarras.Text = dto.GerarCodigoBarrasSugerido(categoria);
            }
        }

        private void txbCodigoBarras_Leave(object sender, EventArgs e)
        {
            // Valida código de barras em tempo real
            if (!string.IsNullOrWhiteSpace(txbCodigoBarras.Text))
            {
                var codigo = txbCodigoBarras.Text.Trim();
                var valido = System.Text.RegularExpressions.Regex.IsMatch(codigo, @"^[0-9]{8,20}$");
                
                ProdutoLogger.LogCodigoBarras(codigo, valido, txbNome.Text);
                
                if (!valido)
                {
                    MessageBox.Show("Código de barras deve conter apenas números e ter entre 8 e 20 dígitos.", 
                        "Código Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txbCodigoBarras.Focus();
                }
            }
        }

        private void msktDataVencimento_Leave(object sender, EventArgs e)
        {
            // Verifica proximidade do vencimento
            var formats = new[] { "dd/MM/yyyy", "d/M/yyyy" };
            var culture = CultureInfo.GetCultureInfo("pt-BR");
            
            if (DateTime.TryParseExact(msktDataVencimento.Text.Trim(), formats, culture, DateTimeStyles.None, out DateTime dataVenc))
            {
                var diasRestantes = (dataVenc - DateTime.Now).Days;
                
                ProdutoLogger.LogVencimento(txbNome.Text, dataVenc, diasRestantes);
                
                if (diasRestantes <= 0)
                {
                    MessageBox.Show("ATENÇÃO: Este produto já está vencido!", 
                        "Produto Vencido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (diasRestantes <= 7)
                {
                    MessageBox.Show($"ATENÇÃO: Este produto vence em {diasRestantes} dias!", 
                        "Vencimento Próximo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void frmProduto_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProdutoLogger.LogOperation("FormularioFechado");
        }

        // Métodos auxiliares para relatórios e exportação
        public void ExportarProdutos()
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx",
                    Title = "Exportar Lista de Produtos"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var produtos = produtosList.ToList();
                    var arquivo = saveDialog.FileName;
                    var extensao = Path.GetExtension(arquivo).ToLower();

                    if (extensao == ".csv")
                    {
                        ExportarCSV(produtos, arquivo);
                    }
                    else
                    {
                        // Implementar exportação Excel se necessário
                        MessageBox.Show("Exportação Excel ainda não implementada.", "Info", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ProdutoLogger.LogError("Erro ao exportar produtos", "Exportacao", ex);
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarCSV(List<ProdutoDto> produtos, string arquivo)
        {
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Codigo,Nome,Descricao,PrecoCusto,PrecoVenda,Margem,Estoque,Categoria,Fornecedor,Status");

            foreach (var produto in produtos)
            {
                csv.AppendLine($"{produto.codBarras},{produto.nomeProduto},{produto.descricaoProduto}," +
                              $"{produto.precoCusto:F2},{produto.precoVenda:F2},{produto.margemLucro:F2}," +
                              $"{produto.quatidadeEstoqueProduto},{produto.CategoriaId},{produto.FornecedorId}," +
                              $"{produto.GetStatusFormatado()}");
            }

            File.WriteAllText(arquivo, csv.ToString(), System.Text.Encoding.UTF8);
            
            ProdutoLogger.LogExportacao(arquivo, produtos.Count, "CSV");
            MessageBox.Show($"Produtos exportados com sucesso!\nArquivo: {arquivo}", "Exportação Concluída", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void GerarRelatorioEstoqueBaixo(int limiteMinimo = 10)
        {
            try
            {
                var produtosBaixoEstoque = produtosList.Where(p => p.EstoqueAbaixoMinimo(limiteMinimo)).ToList();
                
                ProdutoLogger.LogRelatorio("EstoqueBaixo", produtosBaixoEstoque.Count, $"Limite: {limiteMinimo}");
                
                if (produtosBaixoEstoque.Any())
                {
                    var relatorio = "RELATÓRIO - PRODUTOS COM ESTOQUE BAIXO\n";
                    relatorio += $"Data: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
                    relatorio += $"Limite mínimo: {limiteMinimo} unidades\n\n";
                    
                    foreach (var produto in produtosBaixoEstoque.OrderBy(p => p.quatidadeEstoqueProduto))
                    {
                        relatorio += $"• {produto.nomeProduto} - Código: {produto.codBarras} - Estoque: {produto.quatidadeEstoqueProduto}\n";
                    }
                    
                    MessageBox.Show(relatorio, "Relatório de Estoque Baixo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Nenhum produto com estoque abaixo do limite encontrado.", "Relatório", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ProdutoLogger.LogError("Erro ao gerar relatório de estoque baixo", "Relatorio", ex);
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
