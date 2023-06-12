using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class AltCategoria : Form
    {
        CategoriaService categoriaService;
        public AltCategoria(string _NomeCategoria, string Id)
        {
            InitializeComponent();

            txtNomeCategoria.Text = _NomeCategoria;
            LblId.Text = Id;
        }

        private void AltCategoria_Load(object sender, EventArgs e)
        {

        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await Alterar();
        }

        private async Task Alterar()
        {
            categoriaService = new CategoriaService();

            CategoriaDto dto = new CategoriaDto()
            {
                id = Guid.Parse(LblId.Text),
                NomeCategoria = txtNomeCategoria.Text
            };
            CategoriaResponse response = await categoriaService.AlterarCategoria(dto);

            if (response.success == true)
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                MessageBox.Show("Alterado com sucesso");

                var qrForm = from frm in Application.OpenForms.Cast<Form>()
                             where frm is CadCategoria
                             select frm;

                if (qrForm != null && qrForm.Count() > 0)
                {
                    await ((CadCategoria)qrForm.First()).Consultar();
                }
                this.Close();
            }
        }

        private async void AltCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var qrForm = from frm in Application.OpenForms.Cast<Form>()
                         where frm is CadCategoria
                         select frm;

            if (qrForm != null && qrForm.Count() > 0)
            {
                await((CadCategoria)qrForm.First()).Consultar();
            }
        }

        private void AltCategoria_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
