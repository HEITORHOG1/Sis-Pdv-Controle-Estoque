using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Produto
{
    public partial class frmProduto : Form
    {
        ProdutoService produtoService;
        FornecedorService fornecedorService;
        CategoriaService categoriaService;


        readonly string _perecivel = "";
        int _ativo = 0;

        public frmProduto()
        {
            InitializeComponent();
        }
        private async void frmProduto_Load(object sender, EventArgs e)
        {
            await Consultar();

            rbPerecivel.Checked = true;
            rbProdutoAtivo.Checked = true;
            await PreencherComboFornecedor();
            await PreencherComboCategoria();

        }
        private async Task PreencherComboCategoria()
        {
            categoriaService = new CategoriaService();

            try
            {
                var responsecategoria = await categoriaService.ListarCategoria();

                if (responsecategoria.data != null && responsecategoria.data.Count > 0)
                {
                    cmbCategoria.DataSource = responsecategoria.data;
                    cmbCategoria.DisplayMember = "nomeCategoria";
                    cmbCategoria.ValueMember = "id";
                }
                else
                {
                    string errorMsg = responsecategoria?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao obter a lista de categorias";
                    MessageBox.Show(errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao preencher o combo de categorias: {ex.Message}");
            }
        }

        private async Task PreencherComboFornecedor()
        {
            fornecedorService = new FornecedorService();

            try
            {
                var responseFornecedor = await fornecedorService.ListarFornecedor();

                if (responseFornecedor.data != null && responseFornecedor.data.Count > 0)
                {
                    cmbFornecedor.DataSource = responseFornecedor.data;
                    cmbFornecedor.DisplayMember = "nomeFantasia";
                    cmbFornecedor.ValueMember = "id";
                }
                else
                {
                    string errorMsg = responseFornecedor?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao obter a lista de fornecedores";
                    MessageBox.Show(errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao preencher o combo de fornecedores: {ex.Message}");
            }
        }

        private async Task Consultar()
        {
            produtoService = new ProdutoService();

            try
            {
                var response = await produtoService.ListarProduto();

                if (response != null && response.success == true)
                {
                    dgvProduto.DataSource = response.data;
                    await DefinirCabecalhos(new List<string>() { "ID",
                                                        "Cód barras",
                                                        "Nome",
                                                        "Descrição",
                                                        "P. Custo",
                                                        "P. Venda",
                                                        "Margem",
                                                        "Dt. Fabri.",
                                                        "Dt. Venci.",
                                                        "Qtd.",
                                                        "Fornecedor",
                                                        "Categoria",
                                                        "Ativo" });
                }
                else
                {
                    string errorMsg = response?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao listar os produtos";
                    MessageBox.Show(errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao consultar os produtos: {ex.Message}");
            }
        }

        private async Task ConsultarPorCodigodeBarras(string codigoBarras)
        {
            if (string.IsNullOrEmpty(codigoBarras))
            {
                MessageBox.Show("Por favor, digite um código de barras válido.");
                return;
            }

            produtoService = new ProdutoService();

            try
            {
                var response = await produtoService.ListarProdutoPorCodBarras(codigoBarras);

                if (response != null && response.success == true)
                {
                    dgvProduto.DataSource = response.data;
                    await DefinirCabecalhos(new List<string>() { "ID",
                                                        "Cód barras",
                                                        "Nome",
                                                        "Descrição",
                                                        "P. Custo",
                                                        "P. Venda",
                                                        "Margem",
                                                        "Dt. Fabri.",
                                                        "Dt. Venci.",
                                                        "Qtd.",
                                                        "Fornecedor",
                                                        "Categoria",
                                                        "Ativo" });
                }
                else
                {
                    string errorMsg = response?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao consultar os produtos por código de barras";
                    MessageBox.Show(errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao consultar os produtos por código de barras: {ex.Message}");
            }
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            await Adicionar();
        }
        private async Task Adicionar()
        {
            produtoService = new ProdutoService();

            await InseriValorRb();

            if (ValidarCampos())
            {
                try
                {
                    ProdutoDto produtoDto = new ProdutoDto
                    {
                        codBarras = txbCodigoBarras.Text,
                        FornecedorId = Guid.Parse(cmbFornecedor.SelectedValue.ToString()),
                        CategoriaId = Guid.Parse(cmbCategoria.SelectedValue.ToString()),
                        nomeProduto = txbNome.Text,
                        descricaoProduto = rtbDescricao.Text,
                        precoCusto = decimal.Parse(txbPrecoCusto.Text),
                        precoVenda = decimal.Parse(txbPrecoDeVenda.Text),
                        margemLucro = decimal.Parse(txbMargemDeLucro.Text),
                        dataFabricao = DateTime.Parse(msktDataFabricacao.Text),
                        dataVencimento = DateTime.Parse(msktDataVencimento.Text),
                        quatidadeEstoqueProduto = Int32.Parse(txbQuantidadeEstoque.Text),
                        statusAtivo = _ativo
                    };

                    var request = await produtoService.AdicionarProduto(produtoDto);
                    if (request != null && request.success == true)
                    {
                        await LimpaCampos();
                        await Consultar();
                        MessageBox.Show($"Produto {request.data.nomeProduto} inserido com sucesso");
                    }
                    else
                    {
                        string errorMsg = request?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao adicionar o produto";
                        MessageBox.Show(errorMsg);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ocorreu um erro ao adicionar o produto: O formato de data fornecido é inválido.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocorreu um erro ao adicionar o produto: {ex.Message}");
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txbCodigoBarras.Text))
            {
                MessageBox.Show("O campo Código de Barras é obrigatório.");
                return false;
            }

            if (cmbFornecedor.SelectedValue == null || cmbFornecedor.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Selecione um fornecedor válido.");
                return false;
            }

            if (cmbCategoria.SelectedValue == null || cmbCategoria.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Selecione uma categoria válida.");
                return false;
            }

            if (string.IsNullOrEmpty(txbNome.Text))
            {
                MessageBox.Show("O campo Nome é obrigatório.");
                return false;
            }

            decimal precoCusto;
            if (!decimal.TryParse(txbPrecoCusto.Text, out precoCusto))
            {
                MessageBox.Show("O campo Preço de Custo possui um valor inválido.");
                return false;
            }

            decimal precoVenda;
            if (!decimal.TryParse(txbPrecoDeVenda.Text, out precoVenda))
            {
                MessageBox.Show("O campo Preço de Venda possui um valor inválido.");
                return false;
            }

            decimal margemLucro;
            if (!decimal.TryParse(txbMargemDeLucro.Text, out margemLucro))
            {
                MessageBox.Show("O campo Margem de Lucro possui um valor inválido.");
                return false;
            }

            int quantidadeEstoque;
            if (!int.TryParse(txbQuantidadeEstoque.Text, out quantidadeEstoque))
            {
                MessageBox.Show("O campo Quantidade em Estoque possui um valor inválido.");
                return false;
            }

            DateTime dataFabricacao;
            if (string.IsNullOrWhiteSpace(msktDataFabricacao.Text) || !DateTime.TryParse(msktDataFabricacao.Text, out dataFabricacao))
            {
                msktDataFabricacao.Text = DateTime.MinValue.ToString();
            }

            DateTime dataVencimento;
            if (string.IsNullOrWhiteSpace(msktDataVencimento.Text) || !DateTime.TryParse(msktDataVencimento.Text, out dataVencimento))
            {
                msktDataVencimento.Text = DateTime.MinValue.ToString();
            }

            return true;
        }


        private async Task Alterar()
        {
            produtoService = new ProdutoService();

            if (!string.IsNullOrEmpty(txbId.Text))
            {
                await InseriValorRb();

                if (ValidarCampos())
                {
                    try
                    {
                        ProdutoDto produtoDto = new ProdutoDto
                        {
                            Id = Guid.Parse(txbId.Text),
                            codBarras = txbCodigoBarras.Text,
                            FornecedorId = Guid.Parse(cmbFornecedor.SelectedValue.ToString()),
                            CategoriaId = Guid.Parse(cmbCategoria.SelectedValue.ToString()),
                            nomeProduto = txbNome.Text,
                            descricaoProduto = rtbDescricao.Text,
                            precoCusto = decimal.Parse(txbPrecoCusto.Text),
                            precoVenda = decimal.Parse(txbPrecoDeVenda.Text),
                            margemLucro = decimal.Parse(txbMargemDeLucro.Text),
                            dataFabricao = DateTime.Parse(msktDataFabricacao.Text),
                            dataVencimento = DateTime.Parse(msktDataVencimento.Text),
                            quatidadeEstoqueProduto = Int32.Parse(txbQuantidadeEstoque.Text),
                            statusAtivo = _ativo
                        };

                        var request = await produtoService.AlterarProduto(produtoDto);
                        if (request != null && request.success == true)
                        {
                            await LimpaCampos();
                            await Consultar();
                            MessageBox.Show($"Produto {request.data.nomeProduto} alterado com sucesso");
                        }
                        else
                        {
                            string errorMsg = request?.notifications?.FirstOrDefault()?.ToString() ?? "Erro desconhecido ao alterar o produto";
                            MessageBox.Show(errorMsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao alterar o produto: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("ID do produto inválido.");
            }
        }



        private async void txbPrecoDeVenda_TextChanged(object sender, EventArgs e)
        {
            //await ValidaMoeda(txbPrecoDeVenda);
            await ValidarValores();
        }
        private async void dgvProduto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string _tempAtivo;

                txbId.Text = this.dgvProduto.CurrentRow.Cells[0].Value?.ToString();
                txbCodigoBarras.Text = this.dgvProduto.CurrentRow.Cells[1].Value?.ToString();
                txbNome.Text = this.dgvProduto.CurrentRow.Cells[2].Value?.ToString();
                rtbDescricao.Text = this.dgvProduto.CurrentRow.Cells[3].Value?.ToString();
                txbPrecoCusto.Text = this.dgvProduto.CurrentRow.Cells[4].Value?.ToString();
                txbPrecoDeVenda.Text = this.dgvProduto.CurrentRow.Cells[5].Value?.ToString();
                txbMargemDeLucro.Text = this.dgvProduto.CurrentRow.Cells[6].Value?.ToString();
                msktDataFabricacao.Text = this.dgvProduto.CurrentRow.Cells[7].Value?.ToString();
                msktDataVencimento.Text = this.dgvProduto.CurrentRow.Cells[8].Value?.ToString();
                txbQuantidadeEstoque.Text = this.dgvProduto.CurrentRow.Cells[9].Value?.ToString();
                cmbFornecedor.Text = this.dgvProduto.CurrentRow.Cells[10].Value?.ToString();
                cmbCategoria.Text = this.dgvProduto.CurrentRow.Cells[11].Value?.ToString();
                _tempAtivo = this.dgvProduto.CurrentRow.Cells[12].Value?.ToString();

                if (_tempAtivo == "1")
                    rbProdutoAtivo.Checked = true;
                else
                    rbProdutoInativo.Checked = true;

                btnAdicionar.Enabled = false;
            }
            catch (Exception ex)
            {
                // Aqui você pode tratar o erro de acordo com a sua lógica
                // Por exemplo, exibir uma mensagem de erro, registrar o erro em um log, etc.
                MessageBox.Show("Ocorreu um erro ao carregar os dados do produto: " + ex.Message);
            }
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await Alterar();
        }
        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            string codigoBarras = txbCodigoBarras.Text;

            if (!string.IsNullOrEmpty(codigoBarras))
            {
                await ConsultarPorCodigodeBarras(codigoBarras);
            }
            else
            {
                await LimpaCampos();
                await Consultar();

            }
        }

        private async void rbPerecivel_CheckedChanged(object sender, EventArgs e)
        {
            msktDataFabricacao.Enabled = true;
            msktDataVencimento.Enabled = true;

        }
        private async void rbNaoPerecivel_CheckedChanged(object sender, EventArgs e)
        {
            msktDataFabricacao.Enabled = false;
            msktDataVencimento.Enabled = false;

            msktDataFabricacao.Text = "";
            msktDataVencimento.Text = "";
        }
        private async void txbPrecoCusto_TextChanged(object sender, EventArgs e)
        {
            //await ValidaMoeda(txbPrecoCusto);
            await ValidarValores();
        }
        private async void txbCodigoBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        private async void txbPrecoCusto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)44 && e.KeyChar != (char)1)
            {
                e.Handled = true;
            }
        }
        private async void txbPrecoDeVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)44 && e.KeyChar != (char)1)
            {
                e.Handled = true;
            }
        }
        private async Task DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in dgvProduto.Columns)
            {
                if (coluna.Visible)
                {
                    coluna.HeaderText = ListaCabecalhos[_index];
                    _index++;
                }
            }
        }
        private async Task ValidaMoeda(TextBox txt)
        {
            string m = string.Empty;
            double v = 0;
            try
            {
                m = txt.Text.Replace(",", "").Replace(".", "");
                if (m.Equals(""))
                    m = "";
                m = m.PadLeft(3, '0');
                if (m.Length > 3 & m.Substring(0, 1) == "0")
                    m = m.Substring(1, m.Length - 1);
                v = Convert.ToDouble(m) / 100;
                txt.Text = string.Format("{0:N}", v);
                txt.SelectionStart = txt.Text.Length;
            }
            catch (Exception)
            {

            }

        }
        private async Task ValidarValores()
        {
            decimal precoVenda = 0;


            decimal precoCusto = 0;
            decimal total = 0;

            if (txbPrecoDeVenda.Text != "" && txbPrecoCusto.Text != "" && txbPrecoCusto.Text != "0,00" && txbPrecoDeVenda.Text != "0,00")
            {
                precoVenda = decimal.Parse(txbPrecoDeVenda.Text);
                precoCusto = decimal.Parse(txbPrecoCusto.Text);
                total = (((precoVenda / precoCusto) - 1)) * 100;
                txbMargemDeLucro.Text = total.ToString("F2");
            }
        }
        private async Task InseriValorRb()
        {
            //if (rbPerecivel.Checked == true)
            //{
            //    _perecivel = rbPerecivel.Text;
            //}
            //else
            //{
            //    _perecivel = rbNaoPerecivel.Text;
            //}
            if (rbProdutoAtivo.Checked == true)
            {
                _ativo = 1;

            }
            else
            {
                _ativo = 0;
            }
        }
        private async Task LimpaCampos()
        {
            txbId.Clear();
            txbCodigoBarras.Clear();
            txbMargemDeLucro.Clear();
            txbNome.Clear();
            txbPrecoDeVenda.Clear();
            txbPrecoCusto.Clear();
            txbQuantidadeEstoque.Clear();
            if (cmbFornecedor.Text.Equals(" "))
            {
                cmbFornecedor.SelectedIndex = 0;
            }
            if (cmbCategoria.Text.Equals(" "))
            {
                cmbCategoria.SelectedIndex = 0;
            }
            rbProdutoAtivo.Checked = true;
            rbPerecivel.Checked = true;
            rtbDescricao.Clear();
            msktDataFabricacao.Text = "";
            msktDataVencimento.Text = "";
        }
    }
}
