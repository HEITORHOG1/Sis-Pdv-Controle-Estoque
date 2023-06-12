using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    public partial class frmRelatorio : Form
    {
        private Form formFilho;
        public frmRelatorio()
        {
            InitializeComponent();
        }

        private void btnMenuRelatorioVendasPorData_Click(object sender, EventArgs e)
        {
            AbrirForm(new frnRelatorioVendasPorData());
        }
        private void AbrirForm(Form formFilho)
        {
            if (this.formFilho != null)
            {
                this.formFilho.Close();
            }
            this.formFilho = formFilho;
            formFilho.TopLevel = false;
            formFilho.FormBorderStyle = FormBorderStyle.None;
            formFilho.Dock = DockStyle.Fill;
            pnForm.Controls.Add(formFilho);
            formFilho.BringToFront();
            formFilho.Show();
        }
    }
}
