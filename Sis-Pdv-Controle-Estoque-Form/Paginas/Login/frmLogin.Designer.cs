namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Login
{
    partial class frmLogin
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
            // Header Panel
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblTituloLogin = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            // Left Panel - Logo/Brand
            this.pnLogo = new System.Windows.Forms.Panel();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.lblSlogan = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblVersao = new System.Windows.Forms.Label();
            
            // Right Panel - Login Form
            this.pnLoginForm = new System.Windows.Forms.Panel();
            this.gbLogin = new System.Windows.Forms.GroupBox();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            
            // Login Fields
            this.lblUsuarioLabel = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblSenhaLabel = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.chkMostrarSenha = new System.Windows.Forms.CheckBox();
            this.chkLembrarLogin = new System.Windows.Forms.CheckBox();
            
            // Action Buttons
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            
            // Status Panel
            this.pnStatus = new System.Windows.Forms.Panel();
            this.lblStatusLogin = new System.Windows.Forms.Label();
            this.progressLogin = new System.Windows.Forms.ProgressBar();
            
            // Suspension of layout
            this.pnHeader.SuspendLayout();
            this.pnLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnLoginForm.SuspendLayout();
            this.gbLogin.SuspendLayout();
            this.pnStatus.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.pnHeader.Controls.Add(this.lblTituloLogin);
            this.pnHeader.Controls.Add(this.btnMinimizar);
            this.pnHeader.Controls.Add(this.btnClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(1000, 50);
            this.pnHeader.TabIndex = 0;
            this.pnHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            
            // 
            // lblTituloLogin
            // 
            this.lblTituloLogin.AutoSize = true;
            this.lblTituloLogin.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloLogin.ForeColor = System.Drawing.Color.White;
            this.lblTituloLogin.Location = new System.Drawing.Point(20, 12);
            this.lblTituloLogin.Name = "lblTituloLogin";
            this.lblTituloLogin.Size = new System.Drawing.Size(320, 30);
            this.lblTituloLogin.TabIndex = 0;
            this.lblTituloLogin.Text = "🔐 ACESSO AO SISTEMA PDV";
            
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(910, 10);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(40, 30);
            this.btnMinimizar.TabIndex = 1;
            this.btnMinimizar.Text = "─";
            this.btnMinimizar.UseVisualStyleBackColor = false;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMin_Click);
            
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(955, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // 
            // pnLogo
            // 
            this.pnLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnLogo.Controls.Add(this.lblEmpresa);
            this.pnLogo.Controls.Add(this.lblSlogan);
            this.pnLogo.Controls.Add(this.pictureBox1);
            this.pnLogo.Controls.Add(this.lblVersao);
            this.pnLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLogo.Location = new System.Drawing.Point(0, 50);
            this.pnLogo.Name = "pnLogo";
            this.pnLogo.Size = new System.Drawing.Size(500, 550);
            this.pnLogo.TabIndex = 1;
            
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblEmpresa.ForeColor = System.Drawing.Color.White;
            this.lblEmpresa.Location = new System.Drawing.Point(80, 180);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(340, 45);
            this.lblEmpresa.TabIndex = 0;
            this.lblEmpresa.Text = "🏪 SISTEMA PDV";
            this.lblEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // lblSlogan
            // 
            this.lblSlogan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSlogan.AutoSize = true;
            this.lblSlogan.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            this.lblSlogan.ForeColor = System.Drawing.Color.LightBlue;
            this.lblSlogan.Location = new System.Drawing.Point(120, 240);
            this.lblSlogan.Name = "lblSlogan";
            this.lblSlogan.Size = new System.Drawing.Size(260, 25);
            this.lblSlogan.TabIndex = 1;
            this.lblSlogan.Text = "Controle Total do seu Negócio";
            this.lblSlogan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(175, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            
            // 
            // lblVersao
            // 
            this.lblVersao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersao.AutoSize = true;
            this.lblVersao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVersao.ForeColor = System.Drawing.Color.LightGray;
            this.lblVersao.Location = new System.Drawing.Point(20, 520);
            this.lblVersao.Name = "lblVersao";
            this.lblVersao.Size = new System.Drawing.Size(180, 19);
            this.lblVersao.TabIndex = 3;
            this.lblVersao.Text = "📍 Versão 2.0 - Build 2024.08";
            
            // 
            // pnLoginForm
            // 
            this.pnLoginForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnLoginForm.Controls.Add(this.gbLogin);
            this.pnLoginForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLoginForm.Location = new System.Drawing.Point(500, 50);
            this.pnLoginForm.Name = "pnLoginForm";
            this.pnLoginForm.Padding = new System.Windows.Forms.Padding(40);
            this.pnLoginForm.Size = new System.Drawing.Size(500, 520);
            this.pnLoginForm.TabIndex = 2;
            
            // 
            // gbLogin
            // 
            this.gbLogin.Controls.Add(this.lblWelcome);
            this.gbLogin.Controls.Add(this.lblSubtitle);
            this.gbLogin.Controls.Add(this.lblUsuarioLabel);
            this.gbLogin.Controls.Add(this.txtLogin);
            this.gbLogin.Controls.Add(this.lblSenhaLabel);
            this.gbLogin.Controls.Add(this.txtSenha);
            this.gbLogin.Controls.Add(this.chkMostrarSenha);
            this.gbLogin.Controls.Add(this.chkLembrarLogin);
            this.gbLogin.Controls.Add(this.btnLogin);
            this.gbLogin.Controls.Add(this.btnLimpar);
            this.gbLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.gbLogin.Location = new System.Drawing.Point(40, 40);
            this.gbLogin.Name = "gbLogin";
            this.gbLogin.Size = new System.Drawing.Size(420, 440);
            this.gbLogin.TabIndex = 0;
            this.gbLogin.TabStop = false;
            this.gbLogin.Text = "🔑 AUTENTICAÇÃO";
            
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblWelcome.Location = new System.Drawing.Point(30, 40);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(200, 32);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "👋 Bem-vindo!";
            
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblSubtitle.Location = new System.Drawing.Point(30, 80);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(320, 20);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Digite suas credenciais para acessar o sistema";
            
            // 
            // lblUsuarioLabel
            // 
            this.lblUsuarioLabel.AutoSize = true;
            this.lblUsuarioLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsuarioLabel.Location = new System.Drawing.Point(30, 130);
            this.lblUsuarioLabel.Name = "lblUsuarioLabel";
            this.lblUsuarioLabel.Size = new System.Drawing.Size(95, 20);
            this.lblUsuarioLabel.TabIndex = 2;
            this.lblUsuarioLabel.Text = "👤 Usuário:";
            
            // 
            // txtLogin
            // 
            this.txtLogin.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtLogin.Location = new System.Drawing.Point(30, 160);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(360, 32);
            this.txtLogin.TabIndex = 3;
            this.txtLogin.Enter += new System.EventHandler(this.txtLogin_Enter);
            this.txtLogin.Leave += new System.EventHandler(this.txtLogin_Leave);
            
            // 
            // lblSenhaLabel
            // 
            this.lblSenhaLabel.AutoSize = true;
            this.lblSenhaLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSenhaLabel.Location = new System.Drawing.Point(30, 210);
            this.lblSenhaLabel.Name = "lblSenhaLabel";
            this.lblSenhaLabel.Size = new System.Drawing.Size(80, 20);
            this.lblSenhaLabel.TabIndex = 4;
            this.lblSenhaLabel.Text = "🔒 Senha:";
            
            // 
            // txtSenha
            // 
            this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtSenha.Location = new System.Drawing.Point(30, 240);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(360, 32);
            this.txtSenha.TabIndex = 5;
            this.txtSenha.UseSystemPasswordChar = true;
            this.txtSenha.Enter += new System.EventHandler(this.txtSenha_Enter);
            this.txtSenha.Leave += new System.EventHandler(this.txtSenha_Leave);
            this.txtSenha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSenha_KeyPress);
            
            // 
            // chkMostrarSenha
            // 
            this.chkMostrarSenha.AutoSize = true;
            this.chkMostrarSenha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkMostrarSenha.Location = new System.Drawing.Point(30, 285);
            this.chkMostrarSenha.Name = "chkMostrarSenha";
            this.chkMostrarSenha.Size = new System.Drawing.Size(108, 19);
            this.chkMostrarSenha.TabIndex = 6;
            this.chkMostrarSenha.Text = "👁️ Mostrar senha";
            this.chkMostrarSenha.UseVisualStyleBackColor = true;
            this.chkMostrarSenha.CheckedChanged += new System.EventHandler(this.chkMostrarSenha_CheckedChanged);
            
            // 
            // chkLembrarLogin
            // 
            this.chkLembrarLogin.AutoSize = true;
            this.chkLembrarLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkLembrarLogin.Location = new System.Drawing.Point(250, 285);
            this.chkLembrarLogin.Name = "chkLembrarLogin";
            this.chkLembrarLogin.Size = new System.Drawing.Size(103, 19);
            this.chkLembrarLogin.TabIndex = 7;
            this.chkLembrarLogin.Text = "💾 Lembrar login";
            this.chkLembrarLogin.UseVisualStyleBackColor = true;
            
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(30, 330);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(240, 50);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "🚀 ENTRAR";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            // 
            // btnLimpar
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLimpar.FlatAppearance.BorderSize = 0;
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLimpar.ForeColor = System.Drawing.Color.White;
            this.btnLimpar.Location = new System.Drawing.Point(280, 330);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(110, 50);
            this.btnLimpar.TabIndex = 9;
            this.btnLimpar.Text = "🧹 LIMPAR";
            this.btnLimpar.UseVisualStyleBackColor = false;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            
            // 
            // pnStatus
            // 
            this.pnStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnStatus.Controls.Add(this.lblStatusLogin);
            this.pnStatus.Controls.Add(this.progressLogin);
            this.pnStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnStatus.Location = new System.Drawing.Point(0, 570);
            this.pnStatus.Name = "pnStatus";
            this.pnStatus.Size = new System.Drawing.Size(1000, 30);
            this.pnStatus.TabIndex = 3;
            
            // 
            // lblStatusLogin
            // 
            this.lblStatusLogin.AutoSize = true;
            this.lblStatusLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatusLogin.ForeColor = System.Drawing.Color.White;
            this.lblStatusLogin.Location = new System.Drawing.Point(20, 6);
            this.lblStatusLogin.Name = "lblStatusLogin";
            this.lblStatusLogin.Size = new System.Drawing.Size(250, 19);
            this.lblStatusLogin.TabIndex = 0;
            this.lblStatusLogin.Text = "🟢 Sistema pronto para autenticação";
            
            // 
            // progressLogin
            // 
            this.progressLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressLogin.Location = new System.Drawing.Point(830, 8);
            this.progressLogin.Name = "progressLogin";
            this.progressLogin.Size = new System.Drawing.Size(150, 15);
            this.progressLogin.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressLogin.TabIndex = 1;
            this.progressLogin.Visible = false;
            
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pnLoginForm);
            this.Controls.Add(this.pnLogo);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.pnStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login - Sistema PDV";
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnLogo.ResumeLayout(false);
            this.pnLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnLoginForm.ResumeLayout(false);
            this.gbLogin.ResumeLayout(false);
            this.gbLogin.PerformLayout();
            this.pnStatus.ResumeLayout(false);
            this.pnStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // Header Components
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTituloLogin;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnClose;
        
        // Logo Panel Components
        private System.Windows.Forms.Panel pnLogo;
        private System.Windows.Forms.Label lblEmpresa;
        private System.Windows.Forms.Label lblSlogan;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblVersao;
        
        // Login Form Components
        private System.Windows.Forms.Panel pnLoginForm;
        private System.Windows.Forms.GroupBox gbLogin;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsuarioLabel;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblSenhaLabel;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.CheckBox chkMostrarSenha;
        private System.Windows.Forms.CheckBox chkLembrarLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLimpar;
        
        // Status Panel Components
        private System.Windows.Forms.Panel pnStatus;
        private System.Windows.Forms.Label lblStatusLogin;
        private System.Windows.Forms.ProgressBar progressLogin;
    }
}