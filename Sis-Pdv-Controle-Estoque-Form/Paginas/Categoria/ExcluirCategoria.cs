using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using System.Data;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class ExcluirCategoria : Form
    {
        CategoriaService categoriaService;
        public ExcluirCategoria(string _NomeCategoria, string _id)
        {
            InitializeComponent();
            txtNomeCategoria.Text = _NomeCategoria;
            LblId.Text = _id;
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            await Excluir();
        }

        private async Task Excluir()
        {
            categoriaService = new CategoriaService();
            CategoriaResponse response = await categoriaService.RemoverCategoria(LblId.Text);

            if (response.success == true)
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                MessageBox.Show("Excluido com sucesso");
                var qrForm = from frm in Application.OpenForms.Cast<Form>()
                             where frm is CadCategoria
                             select frm;

                if (qrForm != null && qrForm.Count() > 0)
                {
                    ((CadCategoria)qrForm.First()).Consultar();
                }

                this.Close();
            }
        }

        private void ExcluirCategoria_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private async void ExcluirCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var qrForm = from frm in Application.OpenForms.Cast<Form>()
                         where frm is CadCategoria
                         select frm;

            if (qrForm != null && qrForm.Count() > 0)
            {
                ((CadCategoria)qrForm.First()).Consultar();
            }
        }
    }
}
