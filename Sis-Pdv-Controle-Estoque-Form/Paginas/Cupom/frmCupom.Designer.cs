namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Cupom
{
    partial class frmCupom
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
            btnMinimizar = new Button();
            btnClose = new Button();
            pnMain = new Panel();
            gbCupom = new GroupBox();
            listBox1 = new ListView();
            lblInstrucoes = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnActions = new Panel();
            btnSalvarPdf = new Button();
            btnImprimir = new Button();
            btnFechar = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            gbCupom.SuspendLayout();
            pnFooter.SuspendLayout();
            pnActions.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(46, 125, 50);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnMinimizar);
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
            lblTitulo.Size = new Size(186, 30);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "\U0001f9fe Cupom Fiscal";
            // 
            // btnMinimizar
            // 
            btnMinimizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimizar.BackColor = Color.Transparent;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 105, 43);
            btnMinimizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 111, 45);
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnMinimizar.ForeColor = Color.White;
            btnMinimizar.Location = new Point(410, 10);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(40, 40);
            btnMinimizar.TabIndex = 3;
            btnMinimizar.Text = "—";
            btnMinimizar.UseVisualStyleBackColor = false;
            btnMinimizar.Click += btnMinimizar_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 76, 60);
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
            pnMain.Controls.Add(gbCupom);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(20);
            pnMain.Size = new Size(500, 590);
            pnMain.TabIndex = 1;
            // 
            // gbCupom
            // 
            gbCupom.Controls.Add(listBox1);
            gbCupom.Dock = DockStyle.Fill;
            gbCupom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbCupom.ForeColor = Color.FromArgb(52, 73, 94);
            gbCupom.Location = new Point(20, 64);
            gbCupom.Name = "gbCupom";
            gbCupom.Padding = new Padding(15);
            gbCupom.Size = new Size(460, 506);
            gbCupom.TabIndex = 1;
            gbCupom.TabStop = false;
            gbCupom.Text = "📄 Visualização do Cupom Fiscal";
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(255, 255, 240);
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Courier New", 8F);
            listBox1.ForeColor = Color.FromArgb(52, 73, 94);
            listBox1.FullRowSelect = true;
            listBox1.HeaderStyle = ColumnHeaderStyle.None;
            listBox1.Location = new Point(15, 33);
            listBox1.MultiSelect = false;
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(430, 458);
            listBox1.TabIndex = 0;
            listBox1.UseCompatibleStateImageBehavior = false;
            listBox1.View = View.List;
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Dock = DockStyle.Top;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(20, 20);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Padding = new Padding(0, 0, 0, 25);
            lblInstrucoes.Size = new Size(453, 44);
            lblInstrucoes.TabIndex = 0;
            lblInstrucoes.Text = "\U0001f9fe Visualize o cupom fiscal gerado para a venda. Use ENTER para fechar.";
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnActions);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 650);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(20);
            pnFooter.Size = new Size(500, 100);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(20, 75);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(364, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "📄 Cupom fiscal gerado com sucesso - Pressione ENTER para fechar";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(20, 60);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(460, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // pnActions
            // 
            pnActions.Controls.Add(btnSalvarPdf);
            pnActions.Controls.Add(btnImprimir);
            pnActions.Controls.Add(btnFechar);
            pnActions.Dock = DockStyle.Top;
            pnActions.Location = new Point(20, 20);
            pnActions.Name = "pnActions";
            pnActions.Size = new Size(460, 40);
            pnActions.TabIndex = 0;
            // 
            // btnSalvarPdf
            // 
            btnSalvarPdf.BackColor = Color.FromArgb(155, 89, 182);
            btnSalvarPdf.Dock = DockStyle.Left;
            btnSalvarPdf.FlatAppearance.BorderSize = 0;
            btnSalvarPdf.FlatAppearance.MouseOverBackColor = Color.FromArgb(142, 68, 173);
            btnSalvarPdf.FlatStyle = FlatStyle.Flat;
            btnSalvarPdf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalvarPdf.ForeColor = Color.White;
            btnSalvarPdf.Location = new Point(145, 0);
            btnSalvarPdf.Name = "btnSalvarPdf";
            btnSalvarPdf.Size = new Size(145, 40);
            btnSalvarPdf.TabIndex = 0;
            btnSalvarPdf.Text = "💾 Salvar PDF";
            btnSalvarPdf.UseVisualStyleBackColor = false;
            btnSalvarPdf.Click += btnSalvarPdf_Click;
            // 
            // btnImprimir
            // 
            btnImprimir.BackColor = Color.FromArgb(52, 152, 219);
            btnImprimir.Dock = DockStyle.Left;
            btnImprimir.FlatAppearance.BorderSize = 0;
            btnImprimir.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.Location = new Point(0, 0);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(145, 40);
            btnImprimir.TabIndex = 1;
            btnImprimir.Text = "🖨️ Imprimir";
            btnImprimir.UseVisualStyleBackColor = false;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnFechar
            // 
            btnFechar.BackColor = Color.FromArgb(46, 125, 50);
            btnFechar.Dock = DockStyle.Right;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatAppearance.MouseOverBackColor = Color.FromArgb(56, 142, 60);
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(315, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(145, 40);
            btnFechar.TabIndex = 2;
            btnFechar.Text = "✅ Fechar (ENTER)";
            btnFechar.UseVisualStyleBackColor = false;
            btnFechar.Click += btnFechar_Click;
            // 
            // frmCupom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 750);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmCupom";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cupom Fiscal";
            Load += frmCupom_Load;
            KeyDown += frmCupom_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            gbCupom.ResumeLayout(false);
            pnFooter.ResumeLayout(false);
            pnFooter.PerformLayout();
            pnActions.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.GroupBox gbCupom;
        private System.Windows.Forms.ListView listBox1;
        private System.Windows.Forms.Panel pnFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel pnActions;
        private System.Windows.Forms.Button btnSalvarPdf;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Label lblInstrucoes;
    }
}