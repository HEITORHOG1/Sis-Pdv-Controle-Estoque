namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    partial class frmVerificaLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnHeader = new Panel();
            lblTitulo = new Label();
            btnClose = new Button();
            pnMain = new Panel();
            pnSenhaContainer = new Panel();
            pnSenha = new Panel();
            txbSenha = new TextBox();
            lblSenhaIcon = new Label();
            lblSenha = new Label();
            pnUsuarioContainer = new Panel();
            pnUsuario = new Panel();
            txbUsuario = new TextBox();
            lblUsuarioIcon = new Label();
            lblUsuario = new Label();
            lblInstrucoes = new Label();
            chkMostrarSenha = new CheckBox();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnButtonContainer = new Panel();
            btnCancelar = new Button();
            btnOk = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            pnSenhaContainer.SuspendLayout();
            pnSenha.SuspendLayout();
            pnUsuarioContainer.SuspendLayout();
            pnUsuario.SuspendLayout();
            pnFooter.SuspendLayout();
            pnButtonContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(192, 57, 43);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnClose);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(516, 60);
            pnHeader.TabIndex = 0;
            pnHeader.MouseDown += pnHeader_MouseDown;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(273, 30);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "🔐 Verificação de Acesso";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(169, 50, 38);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 84, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(471, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 40);
            btnClose.TabIndex = 5;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnMain
            // 
            pnMain.BackColor = Color.White;
            pnMain.Controls.Add(pnSenhaContainer);
            pnMain.Controls.Add(pnUsuarioContainer);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Controls.Add(chkMostrarSenha);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(30, 25, 30, 25);
            pnMain.Size = new Size(516, 260);
            pnMain.TabIndex = 1;
            // 
            // pnSenhaContainer
            // 
            pnSenhaContainer.Controls.Add(pnSenha);
            pnSenhaContainer.Controls.Add(lblSenha);
            pnSenhaContainer.Dock = DockStyle.Top;
            pnSenhaContainer.Location = new Point(30, 163);
            pnSenhaContainer.Name = "pnSenhaContainer";
            pnSenhaContainer.Size = new Size(456, 70);
            pnSenhaContainer.TabIndex = 5;
            // 
            // pnSenha
            // 
            pnSenha.BackColor = Color.FromArgb(236, 240, 241);
            pnSenha.Controls.Add(txbSenha);
            pnSenha.Controls.Add(lblSenhaIcon);
            pnSenha.Dock = DockStyle.Top;
            pnSenha.Location = new Point(0, 25);
            pnSenha.Name = "pnSenha";
            pnSenha.Padding = new Padding(15, 10, 15, 10);
            pnSenha.Size = new Size(456, 45);
            pnSenha.TabIndex = 3;
            // 
            // txbSenha
            // 
            txbSenha.BackColor = Color.FromArgb(236, 240, 241);
            txbSenha.BorderStyle = BorderStyle.None;
            txbSenha.Dock = DockStyle.Fill;
            txbSenha.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            txbSenha.ForeColor = Color.FromArgb(52, 73, 94);
            txbSenha.Location = new Point(50, 10);
            txbSenha.Name = "txbSenha";
            txbSenha.PasswordChar = '●';
            txbSenha.Size = new Size(391, 24);
            txbSenha.TabIndex = 2;
            txbSenha.TextChanged += txbSenha_TextChanged;
            txbSenha.Enter += txbSenha_Enter;
            txbSenha.KeyPress += txbSenha_KeyPress;
            txbSenha.Leave += txbSenha_Leave;
            // 
            // lblSenhaIcon
            // 
            lblSenhaIcon.Dock = DockStyle.Left;
            lblSenhaIcon.Font = new Font("Segoe UI", 14F);
            lblSenhaIcon.ForeColor = Color.FromArgb(149, 165, 166);
            lblSenhaIcon.Location = new Point(15, 10);
            lblSenhaIcon.Name = "lblSenhaIcon";
            lblSenhaIcon.Size = new Size(35, 25);
            lblSenhaIcon.TabIndex = 1;
            lblSenhaIcon.Text = "🔒";
            lblSenhaIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Dock = DockStyle.Top;
            lblSenha.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSenha.ForeColor = Color.FromArgb(52, 73, 94);
            lblSenha.Location = new Point(0, 0);
            lblSenha.Name = "lblSenha";
            lblSenha.Padding = new Padding(0, 0, 0, 5);
            lblSenha.Size = new Size(155, 25);
            lblSenha.TabIndex = 2;
            lblSenha.Text = "🛡️ Senha de Acesso:";
            // 
            // pnUsuarioContainer
            // 
            pnUsuarioContainer.Controls.Add(pnUsuario);
            pnUsuarioContainer.Controls.Add(lblUsuario);
            pnUsuarioContainer.Dock = DockStyle.Top;
            pnUsuarioContainer.Location = new Point(30, 93);
            pnUsuarioContainer.Name = "pnUsuarioContainer";
            pnUsuarioContainer.Size = new Size(456, 70);
            pnUsuarioContainer.TabIndex = 4;
            // 
            // pnUsuario
            // 
            pnUsuario.BackColor = Color.FromArgb(236, 240, 241);
            pnUsuario.Controls.Add(txbUsuario);
            pnUsuario.Controls.Add(lblUsuarioIcon);
            pnUsuario.Dock = DockStyle.Top;
            pnUsuario.Location = new Point(0, 25);
            pnUsuario.Name = "pnUsuario";
            pnUsuario.Padding = new Padding(15, 10, 15, 10);
            pnUsuario.Size = new Size(456, 45);
            pnUsuario.TabIndex = 2;
            // 
            // txbUsuario
            // 
            txbUsuario.BackColor = Color.FromArgb(236, 240, 241);
            txbUsuario.BorderStyle = BorderStyle.None;
            txbUsuario.Dock = DockStyle.Fill;
            txbUsuario.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            txbUsuario.ForeColor = Color.FromArgb(52, 73, 94);
            txbUsuario.Location = new Point(50, 10);
            txbUsuario.Name = "txbUsuario";
            txbUsuario.Size = new Size(391, 24);
            txbUsuario.TabIndex = 1;
            txbUsuario.TextChanged += txbUsuario_TextChanged;
            txbUsuario.Enter += txbUsuario_Enter;
            txbUsuario.Leave += txbUsuario_Leave;
            // 
            // lblUsuarioIcon
            // 
            lblUsuarioIcon.Dock = DockStyle.Left;
            lblUsuarioIcon.Font = new Font("Segoe UI", 14F);
            lblUsuarioIcon.ForeColor = Color.FromArgb(149, 165, 166);
            lblUsuarioIcon.Location = new Point(15, 10);
            lblUsuarioIcon.Name = "lblUsuarioIcon";
            lblUsuarioIcon.Size = new Size(35, 25);
            lblUsuarioIcon.TabIndex = 1;
            lblUsuarioIcon.Text = "👤";
            lblUsuarioIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Dock = DockStyle.Top;
            lblUsuario.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(52, 73, 94);
            lblUsuario.Location = new Point(0, 0);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Padding = new Padding(0, 0, 0, 5);
            lblUsuario.Size = new Size(202, 25);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "👨‍💼 Usuário Administrativo:";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Dock = DockStyle.Top;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(30, 49);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Padding = new Padding(0, 0, 0, 25);
            lblInstrucoes.Size = new Size(412, 44);
            lblInstrucoes.TabIndex = 6;
            lblInstrucoes.Text = "🔐 Esta operação requer credenciais de administrador do sistema.";
            // 
            // chkMostrarSenha
            // 
            chkMostrarSenha.AutoSize = true;
            chkMostrarSenha.Dock = DockStyle.Top;
            chkMostrarSenha.Font = new Font("Segoe UI", 9F);
            chkMostrarSenha.ForeColor = Color.FromArgb(127, 140, 141);
            chkMostrarSenha.Location = new Point(30, 25);
            chkMostrarSenha.Name = "chkMostrarSenha";
            chkMostrarSenha.Padding = new Padding(0, 5, 0, 0);
            chkMostrarSenha.Size = new Size(456, 24);
            chkMostrarSenha.TabIndex = 7;
            chkMostrarSenha.Text = "👁️ Mostrar senha";
            chkMostrarSenha.UseVisualStyleBackColor = true;
            chkMostrarSenha.CheckedChanged += chkMostrarSenha_CheckedChanged;
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnButtonContainer);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 320);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(30, 20, 30, 20);
            pnFooter.Size = new Size(516, 90);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(30, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(284, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "🔓 Digite suas credenciais para acesso administrativo";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(30, 50);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(390, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 5;
            progressBar.Visible = false;
            // 
            // pnButtonContainer
            // 
            pnButtonContainer.Controls.Add(btnCancelar);
            pnButtonContainer.Controls.Add(btnOk);
            pnButtonContainer.Dock = DockStyle.Top;
            pnButtonContainer.Location = new Point(30, 20);
            pnButtonContainer.Name = "pnButtonContainer";
            pnButtonContainer.Size = new Size(456, 40);
            pnButtonContainer.TabIndex = 4;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(149, 165, 166);
            btnCancelar.Dock = DockStyle.Right;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(127, 140, 141);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(261, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(195, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "🚫 Cancelar (ESC)";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(46, 204, 113);
            btnOk.Dock = DockStyle.Left;
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(0, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(190, 40);
            btnOk.TabIndex = 3;
            btnOk.Text = "✓ Verificar (ENTER)";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // frmVerificaLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(516, 410);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmVerificaLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Verificação de Acesso";
            Load += frmVerificaLogin_Load;
            KeyDown += frmVerificaLogin_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            pnSenhaContainer.ResumeLayout(false);
            pnSenhaContainer.PerformLayout();
            pnSenha.ResumeLayout(false);
            pnSenha.PerformLayout();
            pnUsuarioContainer.ResumeLayout(false);
            pnUsuarioContainer.PerformLayout();
            pnUsuario.ResumeLayout(false);
            pnUsuario.PerformLayout();
            pnFooter.ResumeLayout(false);
            pnFooter.PerformLayout();
            pnButtonContainer.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnFooter;
        private System.Windows.Forms.Panel pnButtonContainer;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnSenhaContainer;
        private System.Windows.Forms.Panel pnSenha;
        private System.Windows.Forms.TextBox txbSenha;
        private System.Windows.Forms.Label lblSenhaIcon;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Panel pnUsuarioContainer;
        private System.Windows.Forms.Panel pnUsuario;
        private System.Windows.Forms.TextBox txbUsuario;
        private System.Windows.Forms.Label lblUsuarioIcon;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox chkMostrarSenha;
    }
}