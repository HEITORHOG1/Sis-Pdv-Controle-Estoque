namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Cliente
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
        }
        public string Parametro
        {
            get { return msktCnpjCpf.Text; }
            set { msktCnpjCpf.Text = value; }
        }
        private void rbCpf_CheckedChanged(object sender, EventArgs e)
        {

            msktCnpjCpf.Text = "";
            msktCnpjCpf.Mask = "000,000,000-00";
            msktCnpjCpf.ReadOnly = false;
            btnAdicionar.Enabled = true;


        }

        private void rbCNPJ_CheckedChanged(object sender, EventArgs e)
        {
            msktCnpjCpf.Text = "";
            msktCnpjCpf.Mask = "00,000,000/0000-00";
            btnAdicionar.Enabled = true;


        }
        string cpf = "";
        string cnpj = "";
        private void btnAdicionar_Click(object sender, EventArgs e)
        {

            if (rbCpf.Checked)
            {
                cpf = msktCnpjCpf.Text.Replace(".", "").Replace("-", "");

            }
            else
            {
                cnpj = msktCnpjCpf.Text.Replace(".", "").Replace("-", "").Replace("/", ""); ;

            }


            if (msktCnpjCpf.Text != "   .   .   -" && msktCnpjCpf.Text != "  .   .   /    -")
            {
                if (cpf.Length == 11 || cnpj.Length == 14)
                {
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Digite um cpf/cnpj valido!!");
                }

            }
            else
            {
                MessageBox.Show("Por favor digite um valor!!");
            }

        }

        private void frmCliente_Load(object sender, EventArgs e)
        {
            btnAdicionar.Enabled = false;
            msktCnpjCpf.ReadOnly = true;

            msktCnpjCpf.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            msktCnpjCpf.Text = "";
            msktCnpjCpf.Mask = null;
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            switch (keyData)
            {

                case Keys.Enter:
                    btnAdicionar.Focus();
                    break;
                case Keys.Escape:
                    msktCnpjCpf.Text = "";
                    msktCnpjCpf.Mask = null;
                    this.Close();
                    break;
                case Keys.F1:

                    rbCpf.Focus();
                    msktCnpjCpf.ReadOnly = false;

                    msktCnpjCpf.Focus();


                    break;
                case Keys.F2:

                    rbCNPJ.Focus();
                    msktCnpjCpf.ReadOnly = false;

                    msktCnpjCpf.Focus();

                    break;

            }

            return base.ProcessCmdKey(ref msg, keyData);

        }


    }

}