namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    partial class CadCategoria
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
            pnGrid = new Panel();
            lstGridCategoria = new DataGridView();
            lblGridInfo = new Label();
            pnForm = new Panel();
            gbOperacoes = new GroupBox();
            pnButtons = new Panel();
            btnExcluir = new Button();
            btnAlterar = new Button();
            btnAtualizar = new Button();
            btnCadastrar = new Button();
            btnConsultar = new Button();
            btnLimpar = new Button();
            gbDados = new GroupBox();
            pnInput = new Panel();
            txtNomeCategoria = new TextBox();
            lblInputIcon = new Label();
            lblNomeCategoria = new Label();
            LblId = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            lblContador = new Label();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            pnGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lstGridCategoria).BeginInit();
            pnForm.SuspendLayout();
            gbOperacoes.SuspendLayout();
            pnButtons.SuspendLayout();
            gbDados.SuspendLayout();
            pnInput.SuspendLayout();
            pnFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(241, 196, 15);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnMinimizar);
            pnHeader.Controls.Add(btnClose);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(1000, 60);
            pnHeader.TabIndex = 0;
            pnHeader.MouseDown += pnHeader_MouseDown;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(293, 32);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "🏷️ Gerenciar Categorias";
            // 
            // btnMinimizar
            // 
            btnMinimizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimizar.BackColor = Color.Transparent;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatAppearance.MouseDownBackColor = Color.FromArgb(211, 171, 13);
            btnMinimizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 126, 34);
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnMinimizar.ForeColor = Color.White;
            btnMinimizar.Location = new Point(905, 10);
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
            btnClose.Location = new Point(950, 10);
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
            pnMain.Controls.Add(pnGrid);
            pnMain.Controls.Add(pnForm);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(20);
            pnMain.Size = new Size(1000, 540);
            pnMain.TabIndex = 1;
            // 
            // pnGrid
            // 
            pnGrid.Controls.Add(lstGridCategoria);
            pnGrid.Controls.Add(lblGridInfo);
            pnGrid.Dock = DockStyle.Fill;
            pnGrid.Location = new Point(20, 170);
            pnGrid.Name = "pnGrid";
            pnGrid.Size = new Size(960, 350);
            pnGrid.TabIndex = 1;
            // 
            // lstGridCategoria
            // 
            lstGridCategoria.AllowUserToAddRows = false;
            lstGridCategoria.AllowUserToDeleteRows = false;
            lstGridCategoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lstGridCategoria.BackgroundColor = Color.White;
            lstGridCategoria.BorderStyle = BorderStyle.None;
            lstGridCategoria.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            lstGridCategoria.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            lstGridCategoria.ColumnHeadersHeight = 40;
            lstGridCategoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            lstGridCategoria.Dock = DockStyle.Fill;
            lstGridCategoria.EnableHeadersVisualStyles = false;
            lstGridCategoria.GridColor = Color.FromArgb(224, 224, 224);
            lstGridCategoria.Location = new Point(0, 25);
            lstGridCategoria.MultiSelect = false;
            lstGridCategoria.Name = "lstGridCategoria";
            lstGridCategoria.ReadOnly = true;
            lstGridCategoria.RowHeadersVisible = false;
            lstGridCategoria.RowTemplate.Height = 35;
            lstGridCategoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lstGridCategoria.Size = new Size(960, 325);
            lstGridCategoria.TabIndex = 0;
            lstGridCategoria.CellDoubleClick += lstGridCategoria_CellDoubleClick;
            // 
            // lblGridInfo
            // 
            lblGridInfo.AutoSize = true;
            lblGridInfo.Dock = DockStyle.Top;
            lblGridInfo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGridInfo.ForeColor = Color.FromArgb(52, 73, 94);
            lblGridInfo.Location = new Point(0, 0);
            lblGridInfo.Name = "lblGridInfo";
            lblGridInfo.Padding = new Padding(0, 0, 0, 5);
            lblGridInfo.Size = new Size(192, 25);
            lblGridInfo.TabIndex = 1;
            lblGridInfo.Text = "📋 Lista de Categorias (0)";
            // 
            // pnForm
            // 
            pnForm.Controls.Add(gbOperacoes);
            pnForm.Controls.Add(gbDados);
            pnForm.Dock = DockStyle.Top;
            pnForm.Location = new Point(20, 20);
            pnForm.Name = "pnForm";
            pnForm.Size = new Size(960, 150);
            pnForm.TabIndex = 0;
            // 
            // gbOperacoes
            // 
            gbOperacoes.Controls.Add(pnButtons);
            gbOperacoes.Dock = DockStyle.Fill;
            gbOperacoes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbOperacoes.ForeColor = Color.FromArgb(52, 73, 94);
            gbOperacoes.Location = new Point(500, 0);
            gbOperacoes.Name = "gbOperacoes";
            gbOperacoes.Padding = new Padding(15);
            gbOperacoes.Size = new Size(460, 150);
            gbOperacoes.TabIndex = 1;
            gbOperacoes.TabStop = false;
            gbOperacoes.Text = "⚙️ Operações";
            // 
            // pnButtons
            // 
            pnButtons.Controls.Add(btnExcluir);
            pnButtons.Controls.Add(btnAlterar);
            pnButtons.Controls.Add(btnAtualizar);
            pnButtons.Controls.Add(btnCadastrar);
            pnButtons.Controls.Add(btnConsultar);
            pnButtons.Controls.Add(btnLimpar);
            pnButtons.Dock = DockStyle.Fill;
            pnButtons.Location = new Point(15, 33);
            pnButtons.Name = "pnButtons";
            pnButtons.Size = new Size(430, 102);
            pnButtons.TabIndex = 0;
            // 
            // btnExcluir
            // 
            btnExcluir.BackColor = Color.FromArgb(231, 76, 60);
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43);
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(290, 60);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(130, 40);
            btnExcluir.TabIndex = 5;
            btnExcluir.Text = "🗑️ Excluir";
            btnExcluir.UseVisualStyleBackColor = false;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnAlterar
            // 
            btnAlterar.BackColor = Color.FromArgb(230, 126, 34);
            btnAlterar.FlatAppearance.BorderSize = 0;
            btnAlterar.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 84, 0);
            btnAlterar.FlatStyle = FlatStyle.Flat;
            btnAlterar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAlterar.ForeColor = Color.White;
            btnAlterar.Location = new Point(150, 60);
            btnAlterar.Name = "btnAlterar";
            btnAlterar.Size = new Size(130, 40);
            btnAlterar.TabIndex = 4;
            btnAlterar.Text = "✏️ Alterar";
            btnAlterar.UseVisualStyleBackColor = false;
            btnAlterar.Click += btnAlterar_Click;
            // 
            // btnAtualizar
            // 
            btnAtualizar.BackColor = Color.FromArgb(52, 152, 219);
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btnAtualizar.FlatStyle = FlatStyle.Flat;
            btnAtualizar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(10, 60);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(130, 40);
            btnAtualizar.TabIndex = 3;
            btnAtualizar.Text = "🔄 Atualizar";
            btnAtualizar.UseVisualStyleBackColor = false;
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // btnCadastrar
            // 
            btnCadastrar.BackColor = Color.FromArgb(46, 204, 113);
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
            btnCadastrar.FlatStyle = FlatStyle.Flat;
            btnCadastrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCadastrar.ForeColor = Color.White;
            btnCadastrar.Location = new Point(10, 10);
            btnCadastrar.Name = "btnCadastrar";
            btnCadastrar.Size = new Size(130, 40);
            btnCadastrar.TabIndex = 0;
            btnCadastrar.Text = "➕ Cadastrar";
            btnCadastrar.UseVisualStyleBackColor = false;
            btnCadastrar.Click += btnCadastrar_Click;
            // 
            // btnConsultar
            // 
            btnConsultar.BackColor = Color.FromArgb(155, 89, 182);
            btnConsultar.FlatAppearance.BorderSize = 0;
            btnConsultar.FlatAppearance.MouseOverBackColor = Color.FromArgb(142, 68, 173);
            btnConsultar.FlatStyle = FlatStyle.Flat;
            btnConsultar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConsultar.ForeColor = Color.White;
            btnConsultar.Location = new Point(150, 10);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(130, 40);
            btnConsultar.TabIndex = 1;
            btnConsultar.Text = "🔍 Consultar";
            btnConsultar.UseVisualStyleBackColor = false;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // btnLimpar
            // 
            btnLimpar.BackColor = Color.FromArgb(149, 165, 166);
            btnLimpar.FlatAppearance.BorderSize = 0;
            btnLimpar.FlatAppearance.MouseOverBackColor = Color.FromArgb(127, 140, 141);
            btnLimpar.FlatStyle = FlatStyle.Flat;
            btnLimpar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLimpar.ForeColor = Color.White;
            btnLimpar.Location = new Point(290, 10);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(130, 40);
            btnLimpar.TabIndex = 2;
            btnLimpar.Text = "\U0001f9f9 Limpar";
            btnLimpar.UseVisualStyleBackColor = false;
            btnLimpar.Click += btnLimpar_Click;
            // 
            // gbDados
            // 
            gbDados.Controls.Add(pnInput);
            gbDados.Controls.Add(lblNomeCategoria);
            gbDados.Controls.Add(LblId);
            gbDados.Dock = DockStyle.Left;
            gbDados.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDados.ForeColor = Color.FromArgb(52, 73, 94);
            gbDados.Location = new Point(0, 0);
            gbDados.Name = "gbDados";
            gbDados.Padding = new Padding(15);
            gbDados.Size = new Size(500, 150);
            gbDados.TabIndex = 0;
            gbDados.TabStop = false;
            gbDados.Text = "📝 Dados da Categoria";
            // 
            // pnInput
            // 
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            pnInput.Controls.Add(txtNomeCategoria);
            pnInput.Controls.Add(lblInputIcon);
            pnInput.Location = new Point(18, 70);
            pnInput.Name = "pnInput";
            pnInput.Padding = new Padding(10);
            pnInput.Size = new Size(460, 45);
            pnInput.TabIndex = 2;
            // 
            // txtNomeCategoria
            // 
            txtNomeCategoria.BackColor = Color.FromArgb(236, 240, 241);
            txtNomeCategoria.BorderStyle = BorderStyle.None;
            txtNomeCategoria.Dock = DockStyle.Fill;
            txtNomeCategoria.Font = new Font("Segoe UI", 12F);
            txtNomeCategoria.ForeColor = Color.FromArgb(52, 73, 94);
            txtNomeCategoria.Location = new Point(45, 10);
            txtNomeCategoria.Name = "txtNomeCategoria";
            txtNomeCategoria.Size = new Size(405, 22);
            txtNomeCategoria.TabIndex = 0;
            txtNomeCategoria.TextChanged += txtNomeCategoria_TextChanged;
            txtNomeCategoria.Enter += txtNomeCategoria_Enter;
            txtNomeCategoria.Leave += txtNomeCategoria_Leave;
            // 
            // lblInputIcon
            // 
            lblInputIcon.Dock = DockStyle.Left;
            lblInputIcon.Font = new Font("Segoe UI", 14F);
            lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            lblInputIcon.Location = new Point(10, 10);
            lblInputIcon.Name = "lblInputIcon";
            lblInputIcon.Size = new Size(35, 25);
            lblInputIcon.TabIndex = 1;
            lblInputIcon.Text = "🏷️";
            lblInputIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNomeCategoria
            // 
            lblNomeCategoria.AutoSize = true;
            lblNomeCategoria.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNomeCategoria.ForeColor = Color.FromArgb(52, 73, 94);
            lblNomeCategoria.Location = new Point(18, 40);
            lblNomeCategoria.Name = "lblNomeCategoria";
            lblNomeCategoria.Size = new Size(174, 20);
            lblNomeCategoria.TabIndex = 1;
            lblNomeCategoria.Text = "🏷️ Nome da Categoria:";
            // 
            // LblId
            // 
            LblId.AutoSize = true;
            LblId.Location = new Point(18, 125);
            LblId.Name = "LblId";
            LblId.Size = new Size(88, 19);
            LblId.TabIndex = 3;
            LblId.Text = "IdCategoria";
            LblId.Visible = false;
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(lblContador);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 600);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(20, 10, 20, 10);
            pnFooter.Size = new Size(1000, 50);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(20, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(193, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "\U0001f7e2 Pronto para gerenciar categorias";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(300, 20);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(400, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // lblContador
            // 
            lblContador.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblContador.Font = new Font("Segoe UI", 9F);
            lblContador.ForeColor = Color.White;
            lblContador.Location = new Point(720, 18);
            lblContador.Name = "lblContador";
            lblContador.Size = new Size(260, 15);
            lblContador.TabIndex = 2;
            lblContador.Text = "📊 0 categorias cadastradas";
            lblContador.TextAlign = ContentAlignment.TopRight;
            // 
            // CadCategoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "CadCategoria";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gerenciar Categorias";
            WindowState = FormWindowState.Maximized;
            Load += CadCategoria_Load;
            KeyDown += CadCategoria_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnGrid.ResumeLayout(false);
            pnGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lstGridCategoria).EndInit();
            pnForm.ResumeLayout(false);
            gbOperacoes.ResumeLayout(false);
            pnButtons.ResumeLayout(false);
            gbDados.ResumeLayout(false);
            gbDados.PerformLayout();
            pnInput.ResumeLayout(false);
            pnInput.PerformLayout();
            pnFooter.ResumeLayout(false);
            pnFooter.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnGrid;
        private System.Windows.Forms.DataGridView lstGridCategoria;
        private System.Windows.Forms.Label lblGridInfo;
        private System.Windows.Forms.Panel pnForm;
        private System.Windows.Forms.GroupBox gbOperacoes;
        private System.Windows.Forms.Panel pnButtons;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnAlterar;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.GroupBox gbDados;
        private System.Windows.Forms.Panel pnInput;
        private System.Windows.Forms.TextBox txtNomeCategoria;
        private System.Windows.Forms.Label lblInputIcon;
        private System.Windows.Forms.Label lblNomeCategoria;
        private System.Windows.Forms.Label LblId;
        private System.Windows.Forms.Panel pnFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblContador;
    }
}