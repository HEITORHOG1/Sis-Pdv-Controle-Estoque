using MediatR;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor;
using System.Net;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Fornecedor
{
    public partial class CadFornecedor : Form
    {
        FornecedorService fornecedorService;
        string _ativo;
        public CadFornecedor()
        {
            InitializeComponent();
        }

        private async void btnLocalizar_Click(object sender, EventArgs e)
        {
            await BuscarCep();
        }
        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            await CadastrarFornecedor();
            await Consultar();
        }
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await AlterarFornecedor();
            await LimpaCampos();
            btnAdicionar.Enabled = true;
        }
        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            await ConsultarPorCnpj(mskTxbCnpj.Text);
        }
        private async void txbInscricaoEstadual_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsLetter(e.KeyChar)))
                e.Handled = true;
        }
        private async void txbCep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsLetter(e.KeyChar)))
                e.Handled = true;
        }
        private async void dgvFornecedor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _temp;
            //"Id",
            //"Insc. Estadual" ,
            //"Nome",
            //"Estado",
            //"Numero",
            //"Complemnto" ,
            //"Bairro",
            //"Cidade",
            //"CEP",
            //"Ativo",
            //"CNPJ",
            //"Rua"
           

            txbId.Text = this.dgvFornecedor.CurrentRow.Cells[0].Value.ToString();
            txbInscricaoEstadual.Text = this.dgvFornecedor.CurrentRow.Cells[1].Value.ToString();
            txbNomeFantasia.Text = this.dgvFornecedor.CurrentRow.Cells[2].Value.ToString();
            txbEstado.Text = this.dgvFornecedor.CurrentRow.Cells[3].Value.ToString();
            txbNumero.Text = this.dgvFornecedor.CurrentRow.Cells[4].Value.ToString();
            txbComplemento.Text = this.dgvFornecedor.CurrentRow.Cells[5].Value.ToString();
            txbBairro.Text = this.dgvFornecedor.CurrentRow.Cells[6].Value.ToString();
            txbCidade.Text = this.dgvFornecedor.CurrentRow.Cells[7].Value.ToString();
            txbCep.Text = this.dgvFornecedor.CurrentRow.Cells[8].Value.ToString();
            _temp = this.dgvFornecedor.CurrentRow.Cells[9].Value.ToString();
            int _temp2 = int.Parse(_temp);
            if (_temp2 == 1)
                rbFornecedorAtivo.Checked = true;
            else
                rbFornecedorInativo.Checked = true;
            mskTxbCnpj.Text = this.dgvFornecedor.CurrentRow.Cells[10].Value.ToString();
            txbRua.Text = this.dgvFornecedor.CurrentRow.Cells[11].Value.ToString();
            btnAdicionar.Enabled = false;
        }
        private async void CadFornecedor_Load(object sender, EventArgs e)
        {
            await Consultar();

        }
        private async Task AtribuirValorRb()
        {
            if (rbFornecedorAtivo.Checked == true)
            {
                _ativo = "1";
            }
            else
            {
                _ativo = "0";
            }
        }
        private async Task DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in dgvFornecedor.Columns)
            {
                if (coluna.Visible)
                {
                    coluna.HeaderText = ListaCabecalhos[_index];
                    _index++;
                }
            }
        }
        private async Task BuscarCep()
        {
            try
            {
                string temp = "";
                temp = txbCep.Text.Replace(".", "").Replace("-", "");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + temp + "/json/");
                request.AllowAutoRedirect = false;
                HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();
                if (ChecaServidor.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Servidor indisponível");
                    return; // Sai da rotina
                }
                using (Stream webStream = ChecaServidor.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            response = Regex.Replace(response, "[{},]", string.Empty);
                            response = response.Replace("\"", "");

                            String[] substrings = response.Split('\n');

                            int cont = 0;
                            foreach (var substring in substrings)
                            {
                                if (cont == 1)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    if (valor[0] == "  erro")
                                    {
                                        MessageBox.Show("CEP não encontrado");
                                        txbCep.Focus();
                                        return;
                                    }
                                }

                                //Logradouro
                                if (cont == 2)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txbRua.Text = valor[1];
                                }

                                //Complemento
                                if (cont == 3)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txbComplemento.Text = valor[1];
                                }

                                //Bairro
                                if (cont == 4)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txbBairro.Text = valor[1];
                                }

                                //Localidade (Cidade)
                                if (cont == 5)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txbCidade.Text = valor[1];
                                }

                                //Estado (UF)
                                if (cont == 6)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txbEstado.Text = valor[1];
                                }

                                cont++;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task ConsultarPorCnpj(string cnpj)
        {
            fornecedorService = new FornecedorService();
            if (txbNomeFantasia.Text != "")
            {
                dgvFornecedor.DataSource = await fornecedorService.ListarFornecedorPorNomeFornecedor(cnpj);
                await DefinirCabecalhos(new List<string>() { "Id", "Insc. Estadual", "Nome", "Estado", "Numero", "Complemnto", "Bairro", "Cidade", "CEP", "CNPJ", "Rua" });
            }
            //else
            //{
            //    if (ckbInativo.Checked)
            //    {
            //        dgvFornecedor.DataSource = controllerFornecedor.ListarTodosFornecedores();
            //        DefinirCabecalhos(new List<string>() { "Id", "CNPJ", "Nome", "Insc. Estadual", "CEP", "Rua", "Estado", "Numero", "Complemnto", "Bairro", "Cidade", "Ativo" });


            //    }
            //    else
            //    {
            //        Listar();
            //    }
            //}
        }
        private async Task Consultar()
        {
            fornecedorService = new FornecedorService();
            var request = await fornecedorService.ListarFornecedor();

            dgvFornecedor.DataSource = request.data;
            await DefinirCabecalhos(new List<string>() { "Id", "Insc. Estadual" ,"Nome","Estado","Numero", "Complemnto" , "Bairro", "Cidade","CEP", "Ativo", "CNPJ", "Rua" });
        }
        private async Task CadastrarFornecedor()
        {
            fornecedorService = new FornecedorService();
            FornecedorDto dto = new FornecedorDto()
            {
                inscricaoEstadual = txbInscricaoEstadual.Text,
                nomeFantasia = txbNomeFantasia.Text,
                Uf = txbEstado.Text,
                Numero = txbNumero.Text,
                Complemento = txbComplemento.Text,
                Bairro = txbBairro.Text,
                Cidade = txbCidade.Text,
                cepFornecedor = Convert.ToInt32(txbCep.Text),
                statusAtivo = 1,
                Cnpj = mskTxbCnpj.Text,
                Rua = txbRua.Text
            };

            var response = await fornecedorService.AdicionarFornecedor(dto);

            if (response.success == true)
            {
                await LimpaCampos();
                MessageBox.Show(String.Format($"Fornecedor {response.data.nomeFantasia} Inserido com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        private async Task AlterarFornecedor()
        {
            await AtribuirValorRb();
            fornecedorService = new FornecedorService();
            FornecedorDto dto = new FornecedorDto()
            {
                Id = txbId.Text,
                inscricaoEstadual = txbInscricaoEstadual.Text,
                nomeFantasia = txbNomeFantasia.Text,
                Uf = txbEstado.Text,
                Numero = txbNumero.Text,
                Complemento = txbComplemento.Text,
                Bairro = txbBairro.Text,
                Cidade = txbCidade.Text,
                cepFornecedor = Convert.ToInt32(txbCep.Text),
                statusAtivo = Convert.ToInt32(_ativo),
                Cnpj = mskTxbCnpj.Text,
                Rua = txbRua.Text
            };

            var response = await fornecedorService.AlterarFornecedor(dto);

            if (response.success == true)
            {
                await LimpaCampos();
                await Consultar();
                MessageBox.Show(String.Format($"Fornecedor {response.data.nomeFantasia} Alterado com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        private async Task LimpaCampos()
        {
            rbFornecedorAtivo.Checked = true;
            txbBairro.Clear();
            txbCep.Clear();
            txbCidade.Clear();
            txbComplemento.Clear();
            txbEstado.Clear();
            txbId.Clear();
            txbInscricaoEstadual.Clear();

            txbNomeFantasia.Clear();
            txbNumero.Clear();
            txbRua.Clear();
            mskTxbCnpj.Text = "";
        }
    }
}
