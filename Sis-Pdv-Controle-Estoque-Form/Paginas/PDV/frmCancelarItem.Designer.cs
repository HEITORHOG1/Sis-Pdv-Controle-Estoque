namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    partial class frmCancelarItem
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
            pnInputContainer = new Panel();
            pnInput = new Panel();
            txbItemCancelar = new TextBox();
            lblInputIcon = new Label();
            lblDescricao = new Label();
            lblInstrucoes = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnButtonContainer = new Panel();
            btnCancelar = new Button();
            btnOk = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            pnInputContainer.SuspendLayout();
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
            pnHeader.Size = new Size(496, 50);
            pnHeader.TabIndex = 0;
            pnHeader.MouseDown += pnHeader_MouseDown;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(15, 13);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(160, 25);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "🗑️ Cancelar Item";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 84, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(456, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(35, 35);
            btnClose.TabIndex = 4;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnMain
            // 
            pnMain.BackColor = Color.White;
            pnMain.Controls.Add(pnInputContainer);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 50);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(20);
            pnMain.Size = new Size(496, 184);
            pnMain.TabIndex = 1;
            // 
            // pnInputContainer
            // 
            pnInputContainer.Controls.Add(pnInput);
            pnInputContainer.Controls.Add(lblDescricao);
            pnInputContainer.Dock = DockStyle.Fill;
            pnInputContainer.Location = new Point(20, 59);
            pnInputContainer.Name = "pnInputContainer";
            pnInputContainer.Size = new Size(456, 105);
            pnInputContainer.TabIndex = 2;
            // 
            // pnInput
            // 
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            pnInput.Controls.Add(txbItemCancelar);
            pnInput.Controls.Add(lblInputIcon);
            pnInput.Dock = DockStyle.Top;
            pnInput.Location = new Point(0, 25);
            pnInput.Name = "pnInput";
            pnInput.Padding = new Padding(10, 8, 10, 8);
            pnInput.Size = new Size(456, 45);
            pnInput.TabIndex = 2;
            // 
            // txbItemCancelar
            // 
            txbItemCancelar.BackColor = Color.FromArgb(236, 240, 241);
            txbItemCancelar.BorderStyle = BorderStyle.None;
            txbItemCancelar.Dock = DockStyle.Fill;
            txbItemCancelar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txbItemCancelar.ForeColor = Color.FromArgb(52, 73, 94);
            txbItemCancelar.Location = new Point(45, 8);
            txbItemCancelar.Name = "txbItemCancelar";
            txbItemCancelar.Size = new Size(401, 25);
            txbItemCancelar.TabIndex = 0;
            txbItemCancelar.TextAlign = HorizontalAlignment.Center;
            txbItemCancelar.TextChanged += txbItemCancelar_TextChanged;
            txbItemCancelar.Enter += txbItemCancelar_Enter;
            txbItemCancelar.KeyPress += txbItemCancelar_KeyPress;
            txbItemCancelar.Leave += txbItemCancelar_Leave;
            // 
            // lblInputIcon
            // 
            lblInputIcon.Dock = DockStyle.Left;
            lblInputIcon.Font = new Font("Segoe UI", 14F);
            lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            lblInputIcon.Location = new Point(10, 8);
            lblInputIcon.Name = "lblInputIcon";
            lblInputIcon.Size = new Size(35, 29);
            lblInputIcon.TabIndex = 1;
            lblInputIcon.Text = "🔢";
            lblInputIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDescricao
            // 
            lblDescricao.AutoSize = true;
            lblDescricao.Dock = DockStyle.Top;
            lblDescricao.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDescricao.ForeColor = Color.FromArgb(52, 73, 94);
            lblDescricao.Location = new Point(0, 0);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Padding = new Padding(0, 0, 0, 5);
            lblDescricao.Size = new Size(252, 25);
            lblDescricao.TabIndex = 1;
            lblDescricao.Text = "📝 Número do item para cancelar:";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Dock = DockStyle.Top;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(20, 20);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Padding = new Padding(0, 0, 0, 20);
            lblInstrucoes.Size = new Size(449, 39);
            lblInstrucoes.TabIndex = 3;
            lblInstrucoes.Text = "💡 Digite o número sequencial do item no carrinho que deseja cancelar.";
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnButtonContainer);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 234);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(20, 15, 20, 15);
            pnFooter.Size = new Size(496, 80);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(20, 55);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(158, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "\U0001f7e2 Pronto para cancelar item";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(20, 40);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(380, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 3;
            progressBar.Visible = false;
            // 
            // pnButtonContainer
            // 
            pnButtonContainer.Controls.Add(btnCancelar);
            pnButtonContainer.Controls.Add(btnOk);
            pnButtonContainer.Dock = DockStyle.Top;
            pnButtonContainer.Location = new Point(20, 15);
            pnButtonContainer.Name = "pnButtonContainer";
            pnButtonContainer.Size = new Size(456, 35);
            pnButtonContainer.TabIndex = 2;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(149, 165, 166);
            btnCancelar.Dock = DockStyle.Right;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(127, 140, 141);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(266, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(190, 35);
            btnCancelar.TabIndex = 3;
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
            btnOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(0, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(185, 35);
            btnOk.TabIndex = 2;
            btnOk.Text = "✓ Confirmar (ENTER)";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // frmCancelarItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(496, 314);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmCancelarItem";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cancelar Item - PDV";
            Load += frmCancelarItem_Load;
            KeyDown += frmCancelarItem_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            pnInputContainer.ResumeLayout(false);
            pnInputContainer.PerformLayout();
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
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnInputContainer;
        private System.Windows.Forms.Panel pnInput;
        private System.Windows.Forms.TextBox txbItemCancelar;
        private System.Windows.Forms.Label lblInputIcon;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}