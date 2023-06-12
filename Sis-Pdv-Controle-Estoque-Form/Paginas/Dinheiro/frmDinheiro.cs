using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Dinheiro
{
    public partial class frmDinheiro : Form
    {
        public frmDinheiro()
        {
            InitializeComponent();
        }

        decimal total;
        decimal recebido;
        decimal troco;

        public string ValorRecibido
        {
            get { return txbValorRecebido.Text; }
            set { txbValorRecebido.Text = value; }
        }
        public string Troco
        {
            get { return lblValorTroco.Text; }
            set { lblValorTroco.Text = value; }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lblValorAReceber.Text == "0.00")
            {
                this.Close();

            }
            else
            {
                MessageBox.Show("O valor a receber tem que ser igual  a 0!!");
            }

        }

        private void txbValorRecebido_TextChanged(object sender, EventArgs e)
        {

            Moeda(ref txbValorRecebido);

            recebido = decimal.Parse(txbValorRecebido.Text);

            if (total > recebido)
            {
                troco = total - recebido;

                lblValorAReceber.Text = troco.ToString();
                lblValorTroco.Text = "0.00";

            }
            else
            {
                troco = recebido - total;
                lblValorTroco.Text = troco.ToString();
                lblValorAReceber.Text = "0.00";

            }

        }

        private void frmDinheiro_Load(object sender, EventArgs e)
        {
            lblValorAReceber.Text = lblSubTotalValor.Text;
            txbValorRecebido.Focus();
        }

        public static void Moeda(ref TextBox txt)
        {
            String n = string.Empty;
            double v = 0;
            try
            {
                n = txt.Text.Replace(",", "").Replace(".", "");
                if (n.Equals(""))
                    n = "";
                n = n.PadLeft(3, '0');
                if (n.Length > 3 & n.Substring(0, 1) == "0")

                    n = n.Substring(1, n.Length - 1);
                v = Convert.ToDouble(n) / 100;
                txt.Text = String.Format("{0:N}", v);
                txt.SelectionStart = txt.Text.Length;
            }
            catch (Exception)
            {

            }
        }

        public void ReceberValor(string total)
        {
            lblSubTotalValor.Text = total;
            this.total = decimal.Parse(total);
        }



        private void txbValorRecebido_Leave(object sender, EventArgs e)
        {
            if (txbValorRecebido.Text == "")
            {
                txbValorRecebido.Text = "0";
                txbValorRecebido.ForeColor = Color.DimGray;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    btnOK.Focus();
                    break;
                case Keys.Escape:
                    txbValorRecebido.Text = "0";
                    this.Close();
                    break;

            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txbValorRecebido.Text = "0";
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}