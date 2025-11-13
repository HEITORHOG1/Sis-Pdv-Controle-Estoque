namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Departamento
{
    partial class AltDepartamento
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
            gbDados = new GroupBox();
            pnInput = new Panel();
            txtNomeDepartamento = new TextBox();
            lblInputIcon = new Label();
            lblNomeCategoria = new Label();
            lblInstrucoes = new Label();
            LblId = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnButtonContainer = new Panel();
            btnCancelar = new Button();
            btnAlterar = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            gbDados.SuspendLayout();
            pnInput.SuspendLayout();
            pnFooter.SuspendLayout();
            pnButtonContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(52, 152, 219);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnClose);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(500, 60);
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
            lblTitulo.Size = new Size(280, 30);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "✏️ Alterar Departamento";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(455, 10);
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
            pnMain.Controls.Add(gbDados);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Controls.Add(LblId);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(30, 25, 30, 25);
            pnMain.Size = new Size(500, 190);
            pnMain.TabIndex = 1;
            // 
            // gbDados
            // 
            gbDados.Controls.Add(pnInput);
            gbDados.Controls.Add(lblNomeCategoria);
            gbDados.Dock = DockStyle.Fill;
            gbDados.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDados.ForeColor = Color.FromArgb(52, 73, 94);
            gbDados.Location = new Point(30, 69);
            gbDados.Name = "gbDados";
            gbDados.Padding = new Padding(15);
            gbDados.Size = new Size(440, 96);
            gbDados.TabIndex = 1;
            gbDados.TabStop = false;
            gbDados.Text = "📝 Dados do Departamento";
            // 
            // pnInput
            // 
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            pnInput.Controls.Add(txtNomeDepartamento);
            pnInput.Controls.Add(lblInputIcon);
            pnInput.Dock = DockStyle.Top;
            pnInput.Location = new Point(15, 58);
            pnInput.Name = "pnInput";
            pnInput.Padding = new Padding(15, 10, 15, 10);
            pnInput.Size = new Size(410, 45);
            pnInput.TabIndex = 2;
            // 
            // txtNomeDepartamento
            // 
            txtNomeDepartamento.BackColor = Color.FromArgb(236, 240, 241);
            txtNomeDepartamento.BorderStyle = BorderStyle.None;
            txtNomeDepartamento.Dock = DockStyle.Fill;
            txtNomeDepartamento.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtNomeDepartamento.ForeColor = Color.FromArgb(52, 73, 94);
            txtNomeDepartamento.Location = new Point(50, 10);
            txtNomeDepartamento.MaxLength = 150;
            txtNomeDepartamento.Name = "txtNomeDepartamento";
            txtNomeDepartamento.Size = new Size(345, 22);
            txtNomeDepartamento.TabIndex = 0;
            txtNomeDepartamento.TextChanged += txtNomeDepartamento_TextChanged;
            txtNomeDepartamento.Enter += txtNomeDepartamento_Enter;
            txtNomeDepartamento.KeyPress += txtNomeDepartamento_KeyPress;
            txtNomeDepartamento.Leave += txtNomeDepartamento_Leave;
            // 
            // lblInputIcon
            // 
            lblInputIcon.Dock = DockStyle.Left;
            lblInputIcon.Font = new Font("Segoe UI", 14F);
            lblInputIcon.ForeColor = Color.FromArgb(52, 152, 219);
            lblInputIcon.Location = new Point(15, 10);
            lblInputIcon.Name = "lblInputIcon";
            lblInputIcon.Size = new Size(35, 25);
            lblInputIcon.TabIndex = 1;
            lblInputIcon.Text = "✏️";
            lblInputIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNomeCategoria
            // 
            lblNomeCategoria.AutoSize = true;
            lblNomeCategoria.Dock = DockStyle.Top;
            lblNomeCategoria.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNomeCategoria.ForeColor = Color.FromArgb(52, 73, 94);
            lblNomeCategoria.Location = new Point(15, 33);
            lblNomeCategoria.Name = "lblNomeCategoria";
            lblNomeCategoria.Padding = new Padding(0, 0, 0, 5);
            lblNomeCategoria.Size = new Size(210, 25);
            lblNomeCategoria.TabIndex = 1;
            lblNomeCategoria.Text = "🏢 Nome do Departamento:";
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
            lblInstrucoes.Size = new Size(397, 44);
            lblInstrucoes.TabIndex = 2;
            lblInstrucoes.Text = "💡 Modifique o nome do departamento e confirme a alteração.";
            // 
            // LblId
            // 
            LblId.AutoSize = true;
            LblId.Location = new Point(30, 170);
            LblId.Name = "LblId";
            LblId.Size = new Size(93, 15);
            LblId.TabIndex = 3;
            LblId.Text = "IdDepartamento";
            LblId.Visible = false;
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnButtonContainer);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 250);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(30, 20, 30, 20);
            pnFooter.Size = new Size(500, 90);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(30, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(225, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "✏️ Modifique o nome e clique em 'Salvar'";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(30, 50);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(440, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 5;
            progressBar.Visible = false;
            // 
            // pnButtonContainer
            // 
            pnButtonContainer.Controls.Add(btnCancelar);
            pnButtonContainer.Controls.Add(btnAlterar);
            pnButtonContainer.Dock = DockStyle.Top;
            pnButtonContainer.Location = new Point(30, 20);
            pnButtonContainer.Name = "pnButtonContainer";
            pnButtonContainer.Size = new Size(440, 40);
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
            btnCancelar.Location = new Point(220, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(220, 40);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "🚫 Cancelar (ESC)";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnAlterar
            // 
            btnAlterar.BackColor = Color.FromArgb(52, 152, 219);
            btnAlterar.Dock = DockStyle.Left;
            btnAlterar.FlatAppearance.BorderSize = 0;
            btnAlterar.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btnAlterar.FlatStyle = FlatStyle.Flat;
            btnAlterar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAlterar.ForeColor = Color.White;
            btnAlterar.Location = new Point(0, 0);
            btnAlterar.Name = "btnAlterar";
            btnAlterar.Size = new Size(215, 40);
            btnAlterar.TabIndex = 2;
            btnAlterar.Text = "💾 Salvar (ENTER)";
            btnAlterar.UseVisualStyleBackColor = false;
            btnAlterar.Click += btnAlterar_Click;
            // 
            // AltDepartamento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 340);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "AltDepartamento";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alterar Departamento";
            Load += AltDepartamento_Load;
            KeyDown += AltDepartamento_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
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
        private System.Windows.Forms.Button btnAlterar;
        private System.Windows.Forms.GroupBox gbDados;
        private System.Windows.Forms.Panel pnInput;
        private System.Windows.Forms.TextBox txtNomeDepartamento;
        private System.Windows.Forms.Label lblInputIcon;
        private System.Windows.Forms.Label lblNomeCategoria;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label LblId;
    }
}