using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class CadDepartamento : Form
    {
        DepartamentoService departamentoService;
        public CadDepartamento()
        {
            InitializeComponent();
        }

        private async void CadDepartamento_Load(object sender, EventArgs e)
        {
            await Consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeDepartamento.Text))
            {
                await ConsultarPorNomeDepartamento(txtNomeDepartamento.Text);
            }
            else
            {
                await Consultar();
            }
        }

        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            await CadastrarDepartamento();
            await Consultar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeDepartamento.Text))
            {
                ExcluirDepartamento form = new ExcluirDepartamento(txtNomeDepartamento.Text, LblId.Text);
                form.ShowDialog();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeDepartamento.Text))
            {
                AltDepartamento form = new AltDepartamento(txtNomeDepartamento.Text, LblId.Text);
                form.ShowDialog();
            }
        }
        private async Task CadastrarDepartamento()
        {
            DepartamentoDto dto = new DepartamentoDto()
            {
                NomeDepartamento = txtNomeDepartamento.Text
            };

            var response = await departamentoService.AdicionarDepartamento(dto);

            if (response.success == true)
            {
                txtNomeDepartamento.Text = "";
                LblId.Text = "";
                MessageBox.Show(String.Format($"Departamento {response.data.nomeDepartamento} Inserida com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }
        public async Task Consultar()
        {
            txtNomeDepartamento.Text = "";
            departamentoService = new DepartamentoService();
            var response = await departamentoService.ListarDepartamento();
            lstGrid.DataSource = response.data;
            DefinirCabecalhos(new List<string>() { "NomeDepartamento", "id" });
        }
        private async Task ConsultarPorNomeDepartamento(string NomeCategoria)
        {

            departamentoService = new DepartamentoService();
            var response = await departamentoService.ListarDepartamentoPorNomeDepartamento(NomeCategoria);
            if (response.success == true)
            {
                lstGrid.DataSource = response.data;
                DefinirCabecalhos(new List<string>() { "NomeDepartamento", "id" });
            }
            else
            {
                txtNomeDepartamento.Text = "";
                LblId.Text = "";
                var resp = response.notifications.FirstOrDefault();
                await Consultar();
                MessageBox.Show(resp.ToString());
            }
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
            txtNomeDepartamento.Text = this.lstGrid.CurrentRow.Cells[0].Value.ToString();
            LblId.Text = this.lstGrid.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
