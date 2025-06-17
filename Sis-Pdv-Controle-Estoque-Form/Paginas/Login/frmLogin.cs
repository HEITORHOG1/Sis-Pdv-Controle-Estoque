using Sis_Pdv_Controle_Estoque_Form.Paginas.PDV;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    public partial class frmLogin : Form
    {
        ColaboradorService _colaboradorService;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void lineShape1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }


        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txbUsuario_Enter(object sender, EventArgs e)
        {
            if (txtLogin.Text == "USUARIO")
            {
                txtLogin.Text = "";
                txtLogin.ForeColor = Color.LightGray;
            }
        }

        private void txbUsuario_Leave(object sender, EventArgs e)
        {
            if (txtLogin.Text == "")
            {
                txtLogin.Text = "USUARIO";
                txtLogin.ForeColor = Color.DimGray;
            }
        }

        private void txbSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "SENHA")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = Color.LightGray;
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private void txbSenha_Leave(object sender, EventArgs e)
        {
            if (txtSenha.Text == "")
            {
                txtSenha.Text = "SENHA";
                txtSenha.ForeColor = Color.DimGray;
                txtSenha.UseSystemPasswordChar = false;
            }
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {

            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string cargo = "";
            _colaboradorService = new ColaboradorService();
            var response = await _colaboradorService.ValidarLogin(txtLogin.Text, txtSenha.Text);
            if (response.success == false)
                foreach (var error in response.notifications)
                {
                    MessageBox.Show(error.ToString());
                }
            else
            {
                cargo = response.data.cargoColaborador;

                if (cargo == "Admin")
                {
                    frmMenu frmMenu = new frmMenu(cargo);

                    frmMenu.Show();
                    frmMenu.FormClosed += LogOut;
                    this.Hide();
                }
                else
                {
                    frmTelaPdv frmPdv = new frmTelaPdv(response.data.nomeColaborador);

                    frmPdv.Show();
                    frmPdv.FormClosed += LogOut;
                    this.Hide();
                }
            }
        }

        private void LogOut(object sender, FormClosedEventArgs e)
        {
            txtSenha.Text = "SENHA";
            txtSenha.UseSystemPasswordChar = false;

            txtSenha.Text = "USUARIO";

            this.Show();
            txtLogin.Focus();

        }
    }
}
