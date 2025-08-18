using Sis_Pdv_Controle_Estoque_Form.Paginas.PDV;
using Sis_Pdv_Controle_Estoque_Form.Services.Auth;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    public partial class frmLogin : Form
    {
        AuthApiService _authApiService;
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
            try
            {
                _authApiService = new AuthApiService();
                var auth = await _authApiService.LoginAsync(txtLogin.Text, txtSenha.Text);

                // Set bearer token for subsequent requests
                Services.Http.HttpClientManager.SetBearerToken(auth.accessToken);

                var roles = auth.user?.roles ?? new List<string>();
                var nome = auth.user?.nome ?? auth.user?.login ?? "";

                // Route by role
                if (roles.Contains("Administrator") || roles.Contains("Manager"))
                {
                    frmMenu frmMenu = new frmMenu(nome);
                    frmMenu.Show();
                    frmMenu.FormClosed += LogOut;
                    this.Hide();
                }
                else if (roles.Contains("Cashier") || roles.Contains("CashSupervisor"))
                {
                    frmTelaPdv frmPdv = new frmTelaPdv(nome);
                    frmPdv.Show();
                    frmPdv.FormClosed += LogOut;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuário sem perfil de acesso válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao autenticar: {ex.Message}");
            }
        }

        private void LogOut(object sender, FormClosedEventArgs e)
        {
            txtSenha.Text = "SENHA";
            txtSenha.UseSystemPasswordChar = false;

            txtLogin.Text = "USUARIO";

            this.Show();
            txtLogin.Focus();
        }
    }
}
