namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    partial class ExcluirCategoria
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcluirCategoria));
            pnHeader = new Panel();
            lblTitulo = new Label();
            btnClose = new Button();
            pnMain = new Panel();
            gbAviso = new GroupBox();
            lblAvisoDetalhado = new Label();
            lblAvisoIcon = new Label();
            gbDados = new GroupBox();
            pnInput = new Panel();
            txtNomeCategoria = new TextBox();
            lblInputIcon = new Label();
            lblNomeCategoria = new Label();
            lblInstrucoes = new Label();
            LblId = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnButtonContainer = new Panel();
            btnCancelar = new Button();
            btnExcluir = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            gbAviso.SuspendLayout();
            gbDados.SuspendLayout();
            pnInput.SuspendLayout();
            pnFooter.SuspendLayout();
            pnButtonContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(231, 76, 60);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnClose);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(550, 60);
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
            lblTitulo.Size = new Size(226, 30);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "🗑️ Excluir Categoria";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(169, 50, 38);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(505, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 40);
            btnClose.TabIndex = 4;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnMain
            // 
            pnMain.BackColor = Color.White;
            pnMain.Controls.Add(gbAviso);
            pnMain.Controls.Add(gbDados);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Controls.Add(LblId);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(30, 25, 30, 25);
            pnMain.Size = new Size(550, 290);
            pnMain.TabIndex = 1;
            // 
            // gbAviso
            // 
            gbAviso.Controls.Add(lblAvisoDetalhado);
            gbAviso.Controls.Add(lblAvisoIcon);
            gbAviso.Dock = DockStyle.Fill;
            gbAviso.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbAviso.ForeColor = Color.FromArgb(231, 76, 60);
            gbAviso.Location = new Point(30, 159);
            gbAviso.Name = "gbAviso";
            gbAviso.Padding = new Padding(15);
            gbAviso.Size = new Size(490, 106);
            gbAviso.TabIndex = 3;
            gbAviso.TabStop = false;
            gbAviso.Text = "⚠️ AVISO IMPORTANTE";
            // 
            // lblAvisoDetalhado
            // 
            lblAvisoDetalhado.Dock = DockStyle.Fill;
            lblAvisoDetalhado.Font = new Font("Segoe UI", 10F);
            lblAvisoDetalhado.ForeColor = Color.FromArgb(52, 73, 94);
            lblAvisoDetalhado.Location = new Point(65, 33);
            lblAvisoDetalhado.Name = "lblAvisoDetalhado";
            lblAvisoDetalhado.Size = new Size(410, 58);
            lblAvisoDetalhado.TabIndex = 1;
            lblAvisoDetalhado.Text = resources.GetString("lblAvisoDetalhado.Text");
            lblAvisoDetalhado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAvisoIcon
            // 
            lblAvisoIcon.Dock = DockStyle.Left;
            lblAvisoIcon.Font = new Font("Segoe UI", 32F);
            lblAvisoIcon.ForeColor = Color.FromArgb(231, 76, 60);
            lblAvisoIcon.Location = new Point(15, 33);
            lblAvisoIcon.Name = "lblAvisoIcon";
            lblAvisoIcon.Size = new Size(50, 58);
            lblAvisoIcon.TabIndex = 0;
            lblAvisoIcon.Text = "⚠️";
            lblAvisoIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbDados
            // 
            gbDados.Controls.Add(pnInput);
            gbDados.Controls.Add(lblNomeCategoria);
            gbDados.Dock = DockStyle.Top;
            gbDados.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDados.ForeColor = Color.FromArgb(52, 73, 94);
            gbDados.Location = new Point(30, 69);
            gbDados.Name = "gbDados";
            gbDados.Padding = new Padding(15);
            gbDados.Size = new Size(490, 90);
            gbDados.TabIndex = 1;
            gbDados.TabStop = false;
            gbDados.Text = "📋 Categoria a ser Excluída";
            // 
            // pnInput
            // 
            pnInput.BackColor = Color.FromArgb(255, 235, 235);
            pnInput.Controls.Add(txtNomeCategoria);
            pnInput.Controls.Add(lblInputIcon);
            pnInput.Dock = DockStyle.Top;
            pnInput.Location = new Point(15, 58);
            pnInput.Name = "pnInput";
            pnInput.Padding = new Padding(15, 10, 15, 10);
            pnInput.Size = new Size(460, 35);
            pnInput.TabIndex = 2;
            // 
            // txtNomeCategoria
            // 
            txtNomeCategoria.BackColor = Color.FromArgb(255, 235, 235);
            txtNomeCategoria.BorderStyle = BorderStyle.None;
            txtNomeCategoria.Dock = DockStyle.Fill;
            txtNomeCategoria.Enabled = false;
            txtNomeCategoria.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtNomeCategoria.ForeColor = Color.FromArgb(231, 76, 60);
            txtNomeCategoria.Location = new Point(50, 10);
            txtNomeCategoria.Name = "txtNomeCategoria";
            txtNomeCategoria.ReadOnly = true;
            txtNomeCategoria.Size = new Size(395, 22);
            txtNomeCategoria.TabIndex = 0;
            txtNomeCategoria.TextAlign = HorizontalAlignment.Center;
            // 
            // lblInputIcon
            // 
            lblInputIcon.Dock = DockStyle.Left;
            lblInputIcon.Font = new Font("Segoe UI", 14F);
            lblInputIcon.ForeColor = Color.FromArgb(231, 76, 60);
            lblInputIcon.Location = new Point(15, 10);
            lblInputIcon.Name = "lblInputIcon";
            lblInputIcon.Size = new Size(35, 15);
            lblInputIcon.TabIndex = 1;
            lblInputIcon.Text = "🗑️";
            lblInputIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNomeCategoria
            // 
            lblNomeCategoria.AutoSize = true;
            lblNomeCategoria.Dock = DockStyle.Top;
            lblNomeCategoria.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNomeCategoria.ForeColor = Color.FromArgb(231, 76, 60);
            lblNomeCategoria.Location = new Point(15, 33);
            lblNomeCategoria.Name = "lblNomeCategoria";
            lblNomeCategoria.Padding = new Padding(0, 0, 0, 5);
            lblNomeCategoria.Size = new Size(174, 25);
            lblNomeCategoria.TabIndex = 1;
            lblNomeCategoria.Text = "⚠️ Nome da Categoria:";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Dock = DockStyle.Top;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(30, 25);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Padding = new Padding(0, 0, 0, 25);
            lblInstrucoes.Size = new Size(388, 44);
            lblInstrucoes.TabIndex = 2;
            lblInstrucoes.Text = "🚨 Confirme a exclusão permanente da categoria selecionada.";
            // 
            // LblId
            // 
            LblId.AutoSize = true;
            LblId.Location = new Point(30, 270);
            LblId.Name = "LblId";
            LblId.Size = new Size(68, 15);
            LblId.TabIndex = 4;
            LblId.Text = "IdCategoria";
            LblId.Visible = false;
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnButtonContainer);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 350);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(30, 20, 30, 20);
            pnFooter.Size = new Size(550, 90);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(30, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(249, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "⚠️ ATENÇÃO: Esta ação não pode ser desfeita!";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(30, 50);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(490, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 5;
            progressBar.Visible = false;
            // 
            // pnButtonContainer
            // 
            pnButtonContainer.Controls.Add(btnCancelar);
            pnButtonContainer.Controls.Add(btnExcluir);
            pnButtonContainer.Dock = DockStyle.Top;
            pnButtonContainer.Location = new Point(30, 20);
            pnButtonContainer.Name = "pnButtonContainer";
            pnButtonContainer.Size = new Size(490, 40);
            pnButtonContainer.TabIndex = 4;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(46, 204, 113);
            btnCancelar.Dock = DockStyle.Right;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(245, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(245, 40);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "✅ Manter (ESC)";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.BackColor = Color.FromArgb(231, 76, 60);
            btnExcluir.Dock = DockStyle.Left;
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43);
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(0, 0);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(240, 40);
            btnExcluir.TabIndex = 2;
            btnExcluir.Text = "🗑️ Confirmar Exclusão";
            btnExcluir.UseVisualStyleBackColor = false;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // ExcluirCategoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 440);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "ExcluirCategoria";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Excluir Categoria";
            Load += ExcluirCategoria_Load;
            KeyDown += ExcluirCategoria_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            gbAviso.ResumeLayout(false);
            gbDados.ResumeLayout(false);
            gbDados.PerformLayout();
            pnInput.ResumeLayout(false);
            pnInput.PerformLayout();
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
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.GroupBox gbDados;
        private System.Windows.Forms.Panel pnInput;
        private System.Windows.Forms.TextBox txtNomeCategoria;
        private System.Windows.Forms.Label lblInputIcon;
        private System.Windows.Forms.Label lblNomeCategoria;
        private System.Windows.Forms.GroupBox gbAviso;
        private System.Windows.Forms.Label lblAvisoDetalhado;
        private System.Windows.Forms.Label lblAvisoIcon;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label LblId;
    }
}