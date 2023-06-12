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
    public partial class AltDepartamento : Form
    {
        DepartamentoService departamentoService;
        public AltDepartamento(string _NomeCategoria, string Id)
        {
            InitializeComponent();
            txtNomeDepartamento.Text = _NomeCategoria;
            LblId.Text = Id;    
        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await Alterar();
        }
        private async Task Alterar()
        {
            departamentoService = new DepartamentoService();

            DepartamentoDto dto = new DepartamentoDto()
            {
                Id = LblId.Text,
                NomeDepartamento = txtNomeDepartamento.Text
            };
            DepartamentoResponse response = await departamentoService.AlterarDepartamento(dto);

            if (response.success == true)
            {
                txtNomeDepartamento.Text = "";
                LblId.Text = "";
                MessageBox.Show("Alterado com sucesso");

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

        private async void AltDepartamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide(); 
            var qrForm = from frm in Application.OpenForms.Cast<Form>()
                                      where frm is CadDepartamento
                                      select frm;

            if (qrForm != null && qrForm.Count() > 0)
            {
              await  ((CadDepartamento)qrForm.First()).Consultar();
            }
            this.Close();
        }

        private void AltDepartamento_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
