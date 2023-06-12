using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    public partial class frmCancelarItem : Form
    {
        public frmCancelarItem()
        {
            InitializeComponent();
        }
        public string Parametro
        {
            get { return txbItemCancelar.Text; }
            set { txbItemCancelar.Text = value; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txbItemCancelar.Text != "")

                this.Close();

            else
                MessageBox.Show("Por favor digite um valor!!");

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void txbItemCancelar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    btnOk.Focus();
                    break;
                case Keys.Escape:
                    btnCancelar.Focus();
                    break;

            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void frmCancelarItem_Load(object sender, EventArgs e)
        {
            txbItemCancelar.Focus();
        }
    }
}