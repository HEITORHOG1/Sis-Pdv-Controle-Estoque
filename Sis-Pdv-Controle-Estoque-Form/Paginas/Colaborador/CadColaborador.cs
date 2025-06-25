using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Colaborador
{

    public partial class CadColaborador : Form
    {
        ColaboradorService colaboradorService;
        DepartamentoService departamentoService;
        bool _ativo = false;
        public CadColaborador()
        {
            InitializeComponent();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            await Consultar();
        }
        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            await Cadastrar();
            await Consultar();
        }
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (LblId.Text != "IdColaborador")
            {
                ColaboradorDto dto = new ColaboradorDto();
                dto.id = LblId.Text;
                if (!String.IsNullOrEmpty(dto.id))
                {
                    await Alterar();
                    await Consultar();
                }
            }
        }
        private async void CadColaborador_Load(object sender, EventArgs e)
        {
            await Consultar();
        }
        private async void btnExcluir_Click(object sender, EventArgs e)
        {

            if (LblId.Text != "IdColaborador")
            {
                ColaboradorDto dto = new ColaboradorDto();
                dto.id = LblId.Text;
                if (!String.IsNullOrEmpty(dto.id))
                {
                    await Excluir(dto);
                    await Consultar();
                }
            }
        }
        private async Task Cadastrar()
        {
            colaboradorService = new ColaboradorService();
            ColaboradorDto colaborador = new()
            {
                nomeColaborador = txtNomeColaborador.Text,
                cargoColaborador = txtCargo.Text,
                cpfColaborador = txtCPF.Text,
                emailCorporativo = txtEmail.Text,
                emailPessoalColaborador = txtEmailCorp.Text,
                telefoneColaborador = txtTelefone.Text,
                senha = txtSenha.Text,
                login = txtLogin.Text,
                departamentoId = cbxDepartamento.SelectedValue.ToString(),
                idlogin = txtLogin.Text,
                statusAtivo = true
            };
            InseriValorRb();

            var response = await colaboradorService.AdicionarColaborador(colaborador);

            if (response.success == true)
            {
                LimparCampos();
                MessageBox.Show(String.Format($"Colaborador {response.data.nomeColaborador} Inserida com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        private async Task Alterar()
        {
            colaboradorService = new ColaboradorService();
            ColaboradorDto colaborador = new()
            {
                id = LblId.Text,
                nomeColaborador = txtNomeColaborador.Text,
                cargoColaborador = txtCargo.Text,
                cpfColaborador = txtCPF.Text,
                emailCorporativo = txtEmail.Text,
                emailPessoalColaborador = txtEmailCorp.Text,
                telefoneColaborador = txtTelefone.Text,
                senha = txtSenha.Text,
                login = txtLogin.Text,
                departamentoId = cbxDepartamento.SelectedValue.ToString(),
                idlogin = lblIdLogin.Text,
                statusAtivo = _ativo
            };
            InseriValorRb();

            var response = await colaboradorService.AlterarColaborador(colaborador);

            if (response.success == true)
            {
                LimparCampos();
                MessageBox.Show(String.Format($"Colaborador {response.data.nomeColaborador} Inserida com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        private void InseriValorRb()
        {
            if (rbAtivo.Checked == true)
            {
                _ativo = true;
            }
            else
            {
                _ativo = false;
            }
        }
        private async Task Excluir(ColaboradorDto dto)
        {
            colaboradorService = new ColaboradorService();
            var response = await colaboradorService.RemoverColaborador(dto.id);

            if (response.success == true)
            {
                LimparCampos();
                MessageBox.Show(String.Format($"Colaborador {response.data.nomeColaborador} Inserida com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        private async Task Consultar()
        {
            List<ColaboradorDto> lista = new();
            colaboradorService = new ColaboradorService();
            departamentoService = new DepartamentoService();
            var response = await colaboradorService.ListarColaborador();
            foreach (var item in response.data)
            {
                ColaboradorDto colaborador = new()
                {
                    nomeColaborador = item.nomeColaborador,
                    cargoColaborador = item.cargoColaborador,
                    cpfColaborador = item.cpfColaborador,
                    emailCorporativo = item.emailCorporativo,
                    emailPessoalColaborador = item.emailPessoalColaborador,
                    telefoneColaborador = item.telefoneColaborador,
                    senha = item.usuario.senha,
                    login = item.usuario.login,
                    statusAtivo = item.usuario.statusAtivo,
                    departamentoId = item.Departamento.NomeDepartamento,
                    idlogin = item.usuario.id,
                    id = item.id
                };
                lista.Add(colaborador);
            }


            lstGrid.DataSource = lista;
            DefinirCabecalhos(new List<string>() {
                 "id",
                 "Nome",
                 "Departamento",
                 "Cpf",
                 "Cargo",
                 "Telefone",
                "E_MAIL Pessoal",
                "E_MAIL Colab",
                "idlogin",
                "Login",
                "Senha",
                "StatusAtivo",
               });

            var retornoDepartamento = await departamentoService.ListarDepartamento();
            cbxDepartamento.DataSource = retornoDepartamento.data;

            cbxDepartamento.DisplayMember = "NomeDepartamento";
            cbxDepartamento.ValueMember = "Id";
        }
        public void LimparCampos()
        {
            LblId.Text = "";
            txtNomeColaborador.Text = "";
            if (cbxDepartamento.Text.Equals(" "))
            {
                cbxDepartamento.SelectedIndex = 0;
            }
            rbAtivo.Checked = false;
            rbAtivo.Checked = false;
            txtCPF.Text = "";
            txtCargo.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            txtEmailCorp.Text = "";
            lblIdLogin.Text = "";
            txtLogin.Text = "";
            txtSenha.Text = "";
            btnCadastrar.Enabled = true;
        }
        private void DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in lstGrid.Columns)
            {
                if (coluna.Visible)
                {
                    coluna.HeaderText = ListaCabecalhos[_index];
                    _index++;
                }
            }
        }
        private void lstGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _tempAtivo;
            LblId.Text = this.lstGrid.CurrentRow.Cells[0].Value.ToString();
            txtNomeColaborador.Text = this.lstGrid.CurrentRow.Cells[1].Value.ToString();
            cbxDepartamento.Text = this.lstGrid.CurrentRow.Cells[2].Value.ToString();
            txtCPF.Text = this.lstGrid.CurrentRow.Cells[3].Value.ToString();
            txtCargo.Text = this.lstGrid.CurrentRow.Cells[4].Value.ToString();
            txtTelefone.Text = this.lstGrid.CurrentRow.Cells[5].Value.ToString();
            txtEmail.Text = this.lstGrid.CurrentRow.Cells[6].Value.ToString();
            txtEmailCorp.Text = this.lstGrid.CurrentRow.Cells[7].Value.ToString();
            lblIdLogin.Text = this.lstGrid.CurrentRow.Cells[8].Value.ToString();
            txtLogin.Text = this.lstGrid.CurrentRow.Cells[9].Value.ToString();
            txtSenha.Text = this.lstGrid.CurrentRow.Cells[10].Value.ToString();
            _tempAtivo = this.lstGrid.CurrentRow.Cells[11].Value.ToString();

            bool _tempAtivoV = bool.Parse(_tempAtivo);

            if (_tempAtivoV == true)
            {
                rbAtivo.Checked = true;
                rbInativo.Checked = false;
                _ativo = true;
            }

            else
            {
                rbInativo.Checked = true;
                rbAtivo.Checked = false;
                _ativo = false;
            }




            btnCadastrar.Enabled = false;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }


    }
}
