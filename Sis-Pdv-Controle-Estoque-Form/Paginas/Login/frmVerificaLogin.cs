using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    public partial class frmVerificaLogin : Form
    {
        public frmVerificaLogin()
        {
            InitializeComponent();
        }

        ColaboradorService _colaboradorService;

        bool validadodor;

        public bool Validador { get { return validadodor; } set { validadodor = value; } }


        private async void btnOk_Click(object sender, EventArgs e)
        {
            _colaboradorService = new ColaboradorService();

            var response = await _colaboradorService.ValidarLogin(txbUsuario.Text, txbSenha.Text);

            string cargo = "";

            if (response.success == false)
            {
                MessageBox.Show("Login ou senha inválidos!");
                txbUsuario.Clear();
                txbSenha.Clear();
                txbUsuario.Focus();
            }
            else
            {
                cargo = response.data.cargoColaborador;

                if (cargo == "Admin")
                {
                    validadodor = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario não tem autorizacao para fazer esta ação!!");
                    txbUsuario.Clear();
                    txbSenha.Clear();
                    txbUsuario.Focus();

                }

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            validadodor = false;

            this.Close();
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
    }
}