using Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Fornecedor
{
    public partial class CadFornecedor : Form
    {
        private FornecedorService fornecedorService;
        private BindingList<FornecedorDto> fornecedoresList;
        private bool isLoading = false;

        public CadFornecedor()
        {
            InitializeComponent();
            fornecedorService = new FornecedorService();
            fornecedoresList = new BindingList<FornecedorDto>();
            
            FornecedorLogger.LogInfo("Formulário de cadastro de fornecedor inicializado", "FormLoad");
        }

        private async void CadFornecedor_Load(object sender, EventArgs e)
        {
            FornecedorLogger.LogOperation("CarregamentoFormulario");
            await Consultar();
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await CadastrarFornecedor();
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await AlterarFornecedor();
        }

        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            if (isLoading) return;

            try
            {
                var pesquisa = mskTxbCnpj.Text?.Trim();
                
                if (!string.IsNullOrEmpty(pesquisa))
                {
                    await ConsultarPorCnpj(pesquisa);
                }
                else
                {
                    await Consultar();
                }
            }
            catch (Exception ex)
            {
                FornecedorLogger.LogError("Erro ao consultar fornecedores", "Consulta", ex);
                MessageBox.Show($"Erro ao consultar fornecedores: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLocalizar_Click(object sender, EventArgs e)
        {
            if (isLoading) return;
            await BuscarCep();
        }

        private async void dgvFornecedor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < fornecedoresList.Count)
            {
                var fornecedor = fornecedoresList[e.RowIndex];
                PreencherCamposEdicao(fornecedor);
                
                FornecedorLogger.LogOperation("FornecedorSelecionado", fornecedor.Id, fornecedor.NomeFantasia);
            }
        }

        private async Task CadastrarFornecedor()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                var dto = new FornecedorDto()
                {
                    NomeFantasia = txbNomeFantasia.Text.Trim(),
                    Cnpj = mskTxbCnpj.Text.Trim(),
                    InscricaoEstadual = txbInscricaoEstadual.Text.Trim(),
                    CepFornecedor = int.Parse(txbCep.Text.Replace("-", "").Replace(".", "")),
                    Rua = txbRua.Text.Trim(),
                    Numero = txbNumero.Text.Trim(),
                    Complemento = txbComplemento.Text.Trim(),
                    Bairro = txbBairro.Text.Trim(),
                    Cidade = txbCidade.Text.Trim(),
                    Uf = txbEstado.Text.Trim(),
                    StatusAtivo = rbFornecedorAtivo.Checked ? 1 : 0
                };

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    FornecedorLogger.LogValidationError("Fornecedor", mensagem, dto.NomeFantasia);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FornecedorLogger.LogOperation("CadastroIniciado", value: dto.NomeFantasia);

                // Verifica se já existe um fornecedor com o mesmo CNPJ
                if (await FornecedorJaExiste(dto.Cnpj))
                {
                    FornecedorLogger.LogWarning($"Tentativa de cadastro de fornecedor duplicado: {dto.Cnpj}", "Cadastro");
                    MessageBox.Show("Já existe um fornecedor com este CNPJ.", "Fornecedor Duplicado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var response = await fornecedorService.AdicionarFornecedor(dto);

                sw.Stop();
                FornecedorLogger.LogApiCall("AdicionarFornecedor", "POST", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    await LimpaCampos();
                    var mensagem = $"Fornecedor '{response.data.NomeFantasia}' inserido com sucesso!";
                    
                    FornecedorLogger.LogOperation("CadastroRealizado", response.data.id, response.data.NomeFantasia);
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    FornecedorLogger.LogError($"Erro no cadastro: {erros}", "Cadastro");
                    MessageBox.Show($"Erro ao cadastrar fornecedor:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                FornecedorLogger.LogError("Erro inesperado ao cadastrar fornecedor", "Cadastro", ex);
                MessageBox.Show($"Erro inesperado ao cadastrar fornecedor: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task AlterarFornecedor()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarSelecao("alterar")) return;
                if (!ValidarCamposCadastro()) return;

                isLoading = true;
                SetLoadingState(true);

                var dto = new FornecedorDto()
                {
                    Id = txbId.Text,
                    NomeFantasia = txbNomeFantasia.Text.Trim(),
                    Cnpj = mskTxbCnpj.Text.Trim(),
                    InscricaoEstadual = txbInscricaoEstadual.Text.Trim(),
                    CepFornecedor = int.Parse(txbCep.Text.Replace("-", "").Replace(".", "")),
                    Rua = txbRua.Text.Trim(),
                    Numero = txbNumero.Text.Trim(),
                    Complemento = txbComplemento.Text.Trim(),
                    Bairro = txbBairro.Text.Trim(),
                    Cidade = txbCidade.Text.Trim(),
                    Uf = txbEstado.Text.Trim(),
                    StatusAtivo = rbFornecedorAtivo.Checked ? 1 : 0
                };

                // Normaliza os dados
                dto.NormalizarDados();

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    var mensagem = $"Erros de validação:\n• {string.Join("\n• ", errosValidacao)}";
                    FornecedorLogger.LogValidationError("Fornecedor", mensagem, dto.NomeFantasia);
                    MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FornecedorLogger.LogOperation("AlteracaoIniciada", dto.Id, dto.NomeFantasia);

                var response = await fornecedorService.AlterarFornecedor(dto);

                sw.Stop();
                FornecedorLogger.LogApiCall("AlterarFornecedor", "PUT", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    await LimpaCampos();
                    var mensagem = $"Fornecedor '{response.data.NomeFantasia}' alterado com sucesso!";
                    
                    FornecedorLogger.LogOperation("AlteracaoRealizada", response.data.id, response.data.NomeFantasia);
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                    btnAdicionar.Enabled = true;
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    FornecedorLogger.LogError($"Erro na alteração: {erros}", "Alteracao");
                    MessageBox.Show($"Erro ao alterar fornecedor:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                FornecedorLogger.LogError("Erro inesperado ao alterar fornecedor", "Alteracao", ex);
                MessageBox.Show($"Erro inesperado ao alterar fornecedor: {ex.Message}", "Erro", 
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

                FornecedorLogger.LogOperation("ListagemIniciada");

                var response = await fornecedorService.ListarFornecedor();
                
                sw.Stop();
                FornecedorLogger.LogApiCall("ListarFornecedor", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    fornecedoresList.Clear();
                    foreach (var forn in response.data)
                    {
                        fornecedoresList.Add(forn.ToDto());
                    }

                    dgvFornecedor.DataSource = null;
                    dgvFornecedor.DataSource = fornecedoresList;
                    await DefinirCabecalhos(new List<string>() { 
                        "Id", "Insc. Estadual", "Nome", "Estado", "Numero", "Complemento", 
                        "Bairro", "Cidade", "CEP", "Ativo", "CNPJ", "Rua" 
                    });
                    
                    // Oculta a coluna Id
                    if (dgvFornecedor.Columns["Id"] != null)
                        dgvFornecedor.Columns["Id"].Visible = false;

                    dgvFornecedor.Refresh();
                    
                    FornecedorLogger.LogInfo($"Listagem concluída com {fornecedoresList.Count} fornecedores", "Listagem");
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    FornecedorLogger.LogError($"Erro na listagem: {erros}", "Listagem");
                    MessageBox.Show($"Erro ao consultar fornecedores:\n• {erros}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                FornecedorLogger.LogError("Erro inesperado ao consultar fornecedores", "Listagem", ex);
                MessageBox.Show($"Erro inesperado ao consultar fornecedores: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task ConsultarPorCnpj(string cnpj)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                isLoading = true;
                SetLoadingState(true);

                FornecedorLogger.LogOperation("PesquisaIniciada", value: cnpj);

                var response = await fornecedorService.ListarFornecedorPorNomeFornecedor(cnpj);
                
                sw.Stop();
                FornecedorLogger.LogApiCall("ListarFornecedorPorCnpj", "GET", sw.Elapsed, response.IsValidResponse());

                if (response.IsValidResponse())
                {
                    fornecedoresList.Clear();
                    foreach (var forn in response.data)
                    {
                        fornecedoresList.Add(forn.ToDto());
                    }

                    dgvFornecedor.DataSource = null;
                    dgvFornecedor.DataSource = fornecedoresList;
                    await DefinirCabecalhos(new List<string>() { 
                        "Id", "Insc. Estadual", "Nome", "Estado", "Numero", "Complemento", 
                        "Bairro", "Cidade", "CEP", "Ativo", "CNPJ", "Rua" 
                    });

                    if (dgvFornecedor.Columns["Id"] != null)
                        dgvFornecedor.Columns["Id"].Visible = false;

                    if (fornecedoresList.Count == 0)
                    {
                        FornecedorLogger.LogInfo($"Nenhum fornecedor encontrado para: {cnpj}", "Pesquisa");
                        MessageBox.Show("Nenhum fornecedor encontrado com o CNPJ especificado.", "Pesquisa", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        FornecedorLogger.LogInfo($"Pesquisa concluída com {fornecedoresList.Count} resultados para: {cnpj}", "Pesquisa");
                    }
                }
                else
                {
                    var erros = response.GetErrorMessages().FormatErrorMessages();
                    FornecedorLogger.LogWarning($"Pesquisa sem resultados: {erros}", "Pesquisa");
                    MessageBox.Show($"Erro na pesquisa:\n• {erros}", "Pesquisa", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await Consultar();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                FornecedorLogger.LogError("Erro inesperado na pesquisa", "Pesquisa", ex);
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

        private async Task BuscarCep()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                string cep = txbCep.Text.Replace(".", "").Replace("-", "").Trim();
                
                if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                {
                    MessageBox.Show("Informe um CEP válido com 8 dígitos.", "CEP Inválido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                isLoading = true;
                SetLoadingState(true);

                FornecedorLogger.LogOperation("ConsultaCepIniciada", value: cep);

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10);
                    
                    var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                    
                    sw.Stop();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var cepData = JsonSerializer.Deserialize<ViaCepResponse>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (cepData?.erro == true)
                        {
                            FornecedorLogger.LogCepLookup(cep, false, "CEP não encontrado");
                            MessageBox.Show("CEP não encontrado.", "CEP Inválido", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txbCep.Focus();
                            return;
                        }

                        // Preenche os campos com os dados do CEP
                        txbRua.Text = cepData?.logradouro ?? "";
                        txbComplemento.Text = cepData?.complemento ?? "";
                        txbBairro.Text = cepData?.bairro ?? "";
                        txbCidade.Text = cepData?.localidade ?? "";
                        txbEstado.Text = cepData?.uf ?? "";

                        FornecedorLogger.LogCepLookup(cep, true, $"{cepData?.localidade}, {cepData?.uf}");
                        
                        // Move o foco para o próximo campo
                        txbNumero.Focus();
                    }
                    else
                    {
                        FornecedorLogger.LogCepLookup(cep, false, "Servidor indisponível");
                        MessageBox.Show("Serviço de CEP temporariamente indisponível. Tente novamente.", "Erro", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                sw.Stop();
                FornecedorLogger.LogError($"Erro de rede ao consultar CEP: {ex.Message}", "ConsultaCep");
                MessageBox.Show("Erro de conexão ao consultar CEP. Verifique sua internet.", "Erro de Conexão", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                sw.Stop();
                FornecedorLogger.LogError("Timeout ao consultar CEP", "ConsultaCep");
                MessageBox.Show("Timeout ao consultar CEP. Tente novamente.", "Timeout", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                sw.Stop();
                FornecedorLogger.LogError($"Erro inesperado ao consultar CEP: {ex.Message}", "ConsultaCep", ex);
                MessageBox.Show($"Erro inesperado ao consultar CEP: {ex.Message}", "Erro", 
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
            // Validação Nome Fantasia
            if (string.IsNullOrWhiteSpace(txbNomeFantasia.Text))
            {
                MessageBox.Show("Informe o nome fantasia do fornecedor.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNomeFantasia.Focus();
                return false;
            }

            // Validação CNPJ
            if (string.IsNullOrWhiteSpace(mskTxbCnpj.Text) || mskTxbCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Length != 14)
            {
                MessageBox.Show("Informe um CNPJ válido.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskTxbCnpj.Focus();
                return false;
            }

            // Validação Inscrição Estadual - OBRIGATÓRIA conforme validação do backend
            if (string.IsNullOrWhiteSpace(txbInscricaoEstadual.Text))
            {
                MessageBox.Show("Informe a inscrição estadual.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbInscricaoEstadual.Focus();
                return false;
            }

            if (txbInscricaoEstadual.Text.Length < 8 || txbInscricaoEstadual.Text.Length > 15)
            {
                MessageBox.Show("A inscrição estadual deve ter entre 8 e 15 dígitos.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbInscricaoEstadual.Focus();
                return false;
            }

            // Validação CEP
            if (string.IsNullOrWhiteSpace(txbCep.Text) || txbCep.Text.Replace("-", "").Length != 8)
            {
                MessageBox.Show("Informe um CEP válido com 8 dígitos.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbCep.Focus();
                return false;
            }

            // Validação Rua
            if (string.IsNullOrWhiteSpace(txbRua.Text))
            {
                MessageBox.Show("Informe o logradouro.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbRua.Focus();
                return false;
            }

            // Validação Número - OBRIGATÓRIO conforme validação do backend
            if (string.IsNullOrWhiteSpace(txbNumero.Text))
            {
                MessageBox.Show("Informe o número do endereço.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNumero.Focus();
                return false;
            }

            // Validação Bairro
            if (string.IsNullOrWhiteSpace(txbBairro.Text))
            {
                MessageBox.Show("Informe o bairro.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbBairro.Focus();
                return false;
            }

            // Validação Cidade
            if (string.IsNullOrWhiteSpace(txbCidade.Text))
            {
                MessageBox.Show("Informe a cidade.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbCidade.Focus();
                return false;
            }

            // Validação UF
            if (string.IsNullOrWhiteSpace(txbEstado.Text) || txbEstado.Text.Length != 2)
            {
                MessageBox.Show("Informe a UF com 2 caracteres.", "Campo Obrigatório", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbEstado.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarSelecao(string acao)
        {
            if (string.IsNullOrEmpty(txbId.Text))
            {
                FornecedorLogger.LogWarning($"Tentativa de {acao} sem seleção", "Validacao");
                MessageBox.Show($"Selecione um fornecedor para {acao}.", "Seleção Necessária", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task<bool> FornecedorJaExiste(string cnpj)
        {
            try
            {
                var response = await fornecedorService.ListarFornecedorPorNomeFornecedor(cnpj);
                return response.IsValidResponse() && response.data != null && response.data.Any();
            }
            catch (Exception ex)
            {
                FornecedorLogger.LogWarning($"Erro ao verificar duplicação: {ex.Message}", "VerificacaoDuplicacao");
                return false;
            }
        }

        private void PreencherCamposEdicao(FornecedorDto fornecedor)
        {
            txbId.Text = fornecedor.Id;
            txbNomeFantasia.Text = fornecedor.NomeFantasia;
            mskTxbCnpj.Text = fornecedor.Cnpj;
            txbInscricaoEstadual.Text = fornecedor.InscricaoEstadual;
            txbCep.Text = fornecedor.CepFornecedor.ToString("D8").Insert(5, "-");
            txbRua.Text = fornecedor.Rua;
            txbNumero.Text = fornecedor.Numero;
            txbComplemento.Text = fornecedor.Complemento;
            txbBairro.Text = fornecedor.Bairro;
            txbCidade.Text = fornecedor.Cidade;
            txbEstado.Text = fornecedor.Uf;
            
            if (fornecedor.StatusAtivo == 1)
                rbFornecedorAtivo.Checked = true;
            else
                rbFornecedorInativo.Checked = true;

            btnAdicionar.Enabled = false;
        }

        private async Task LimpaCampos()
        {
            txbId.Clear();
            txbNomeFantasia.Clear();
            mskTxbCnpj.Text = "";
            txbInscricaoEstadual.Clear();
            txbCep.Clear();
            txbRua.Clear();
            txbNumero.Clear();
            txbComplemento.Clear();
            txbBairro.Clear();
            txbCidade.Clear();
            txbEstado.Clear();
            rbFornecedorAtivo.Checked = true;
            btnAdicionar.Enabled = true;
        }

        private async Task DefinirCabecalhos(List<string> listaCabecalhos)
        {
            int index = 0;

            foreach (DataGridViewColumn coluna in dgvFornecedor.Columns)
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
            btnLocalizar.Enabled = !loading;
            dgvFornecedor.Enabled = !loading;

            // Desabilita campos durante carregamento
            txbNomeFantasia.Enabled = !loading;
            mskTxbCnpj.Enabled = !loading;
            txbInscricaoEstadual.Enabled = !loading;
            txbCep.Enabled = !loading;
            txbRua.Enabled = !loading;
            txbNumero.Enabled = !loading;
            txbComplemento.Enabled = !loading;
            txbBairro.Enabled = !loading;
            txbCidade.Enabled = !loading;
            txbEstado.Enabled = !loading;
            rbFornecedorAtivo.Enabled = !loading;
            rbFornecedorInativo.Enabled = !loading;

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
        private void txbInscricaoEstadual_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txbCep_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txbNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite números e algumas letras para complementos como "123A"
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CadFornecedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            FornecedorLogger.LogOperation("FormularioFechado");
        }
    }

    // Classe para deserialização da resposta do ViaCEP
    public class ViaCepResponse
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }
        public bool erro { get; set; }
    }
}
