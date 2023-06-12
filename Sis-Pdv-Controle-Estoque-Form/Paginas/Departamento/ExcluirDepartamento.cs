using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Departamento;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    public partial class ExcluirDepartamento : Form
    {
        DepartamentoService departamentoService;
        public ExcluirDepartamento(string _NomeCategoria, string Id)
        {
            InitializeComponent();
            txtNomeDepartamento.Text = _NomeCategoria;
            LblId.Text = Id;
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            await Excluir();
        }
        private async Task Excluir()
        {
            departamentoService = new DepartamentoService();
            DepartamentoResponse response = await departamentoService.RemoverDepartamento(LblId.Text);

            if (response.success == true)
            {
                txtNomeDepartamento.Text = "";
                LblId.Text = "";
                MessageBox.Show("Excluido com sucesso");
                var qrForm = from frm in Application.OpenForms.Cast<Form>()
                             where frm is CadDepartamento
                             select frm;

                if (qrForm != null && qrForm.Count() > 0)
                {
                  await  ((CadDepartamento)qrForm.First()).Consultar();
                }

                this.Close();
            }
        }

        private async void ExcluirDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var qrForm = from frm in Application.OpenForms.Cast<Form>()
                         where frm is CadDepartamento
                         select frm;

            if (qrForm != null && qrForm.Count() > 0)
            {
                await ((CadDepartamento)qrForm.First()).Consultar();
            }

            this.Close();
        }
    }
}
