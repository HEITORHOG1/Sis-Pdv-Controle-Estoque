using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class CadCategoria : Form
    {
        CategoriaService categoriaService;
        public CadCategoria()
        {
            InitializeComponent();
        }

        private async void btnCadastrar_Click_1(object sender, EventArgs e)
        {
            await CadastrarCategoria();
            await Consultar();
        }
        private async void CadCategoria_Load(object sender, EventArgs e)
        {
            await Consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
            {
                await ConsultarPorNomeCategoria(txtNomeCategoria.Text);
            }
            else
            {
                await Consultar();
            }
        }
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
            {
                AltCategoria form = new AltCategoria(txtNomeCategoria.Text, LblId.Text);
                form.ShowDialog();
            }
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
            {
                ExcluirCategoria form = new ExcluirCategoria(txtNomeCategoria.Text, LblId.Text);
                form.ShowDialog();
            }
        }
        private async Task CadastrarCategoria()
        {
            CategoriaDto dto = new CategoriaDto()
            {
                NomeCategoria = txtNomeCategoria.Text
            };
            txtNomeCategoria.Enabled = true;
            var response = await categoriaService.Adicionar(dto);

            if (response.success == true)
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                MessageBox.Show(String.Format($"Categoria {response.data.NomeCategoria} Inserida com sucesso"));
            }
            else
            {
                var resp = response.notifications.FirstOrDefault();

                MessageBox.Show(resp.ToString());
            }
        }



        public async Task Consultar()
        {
            txtNomeCategoria.Enabled = true;
            txtNomeCategoria.Text = "";
            categoriaService = new CategoriaService();
            var response = await categoriaService.ListarCategoria();
            lstGridCategoria.DataSource = response.data;
            DefinirCabecalhos(new List<string>() { "NomeCategoria", "id" });
        }
        private async Task ConsultarPorNomeCategoria(string NomeCategoria)
        {
            txtNomeCategoria.Enabled = true;
            categoriaService = new CategoriaService();
            var response = await categoriaService.ListarCategoriaPorNomeCategoria(NomeCategoria);
            if (response.success == true)
            {
                lstGridCategoria.DataSource = response.data;
                DefinirCabecalhos(new List<string>() { "NomeCategoria", "id" });
            }
            else
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                var resp = response.notifications.FirstOrDefault();
                await Consultar();
                MessageBox.Show(resp.ToString());
            }
        }

        private void DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in lstGridCategoria.Columns)
            {
                if (coluna.Visible)
                {
                    coluna.HeaderText = ListaCabecalhos[_index];
                    _index++;
                }
            }
        }

        private void lstGridCategoria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNomeCategoria.Enabled = false;
            txtNomeCategoria.Text = this.lstGridCategoria.CurrentRow.Cells[0].Value.ToString();
            LblId.Text = this.lstGridCategoria.CurrentRow.Cells[1].Value.ToString();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}




