namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    partial class frnRelatorioVendasPorData
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnMain = new Panel();
            pnGrid = new Panel();
            dgvRelatorio = new DataGridView();
            lblGridInfo = new Label();
            pnForm = new Panel();
            gbTotais = new GroupBox();
            pnTotaisContainer = new Panel();
            lblTotalIcon = new Label();
            lblTotal = new Label();
            lblTotalLabel = new Label();
            gbFiltros = new GroupBox();
            pnExportacao = new Panel();
            btnExportarExcel = new Button();
            btnExportarPdf = new Button();
            pnAcoes = new Panel();
            btnConsulta = new Button();
            btnLimpar = new Button();
            pnDatas = new Panel();
            pnDataFinal = new Panel();
            dtpDataFinal = new DateTimePicker();
            lblDataFinalIcon = new Label();
            lblDataFinal = new Label();
            pnDataInicial = new Panel();
            dtpDataInicial = new DateTimePicker();
            lblDataInicialIcon = new Label();
            lblDataInicial = new Label();
            pnHeader = new Panel();
            lblTitulo = new Label();
            lblInstrucoes = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            pnMain.SuspendLayout();
            pnGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRelatorio).BeginInit();
            pnForm.SuspendLayout();
            gbTotais.SuspendLayout();
            pnTotaisContainer.SuspendLayout();
            gbFiltros.SuspendLayout();
            pnExportacao.SuspendLayout();
            pnAcoes.SuspendLayout();
            pnDatas.SuspendLayout();
            pnDataFinal.SuspendLayout();
            pnDataInicial.SuspendLayout();
            pnHeader.SuspendLayout();
            pnFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnMain
            // 
            pnMain.BackColor = Color.White;
            pnMain.Controls.Add(pnGrid);
            pnMain.Controls.Add(pnForm);
            pnMain.Controls.Add(pnHeader);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 0);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(20);
            pnMain.Size = new Size(1200, 700);
            pnMain.TabIndex = 0;
            // 
            // pnGrid
            // 
            pnGrid.Controls.Add(dgvRelatorio);
            pnGrid.Controls.Add(lblGridInfo);
            pnGrid.Dock = DockStyle.Fill;
            pnGrid.Location = new Point(20, 305);
            pnGrid.Name = "pnGrid";
            pnGrid.Size = new Size(1160, 375);
            pnGrid.TabIndex = 2;
            // 
            // dgvRelatorio
            // 
            dgvRelatorio.AllowUserToAddRows = false;
            dgvRelatorio.AllowUserToDeleteRows = false;
            dgvRelatorio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelatorio.BackgroundColor = Color.White;
            dgvRelatorio.BorderStyle = BorderStyle.None;
            dgvRelatorio.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRelatorio.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(155, 89, 182);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(142, 68, 173);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvRelatorio.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvRelatorio.ColumnHeadersHeight = 40;
            dgvRelatorio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRelatorio.Dock = DockStyle.Fill;
            dgvRelatorio.EnableHeadersVisualStyles = false;
            dgvRelatorio.GridColor = Color.FromArgb(224, 224, 224);
            dgvRelatorio.Location = new Point(0, 25);
            dgvRelatorio.MultiSelect = false;
            dgvRelatorio.Name = "dgvRelatorio";
            dgvRelatorio.ReadOnly = true;
            dgvRelatorio.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(155, 89, 182);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dgvRelatorio.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvRelatorio.RowTemplate.Height = 35;
            dgvRelatorio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelatorio.Size = new Size(1160, 350);
            dgvRelatorio.TabIndex = 0;
            dgvRelatorio.CellDoubleClick += dgvRelatorio_CellDoubleClick;
            dgvRelatorio.CellFormatting += dgvRelatorio_CellFormatting;
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
            lblGridInfo.Size = new Size(223, 25);
            lblGridInfo.TabIndex = 1;
            lblGridInfo.Text = "📈 Resultados da Consulta (0)";
            // 
            // pnForm
            // 
            pnForm.Controls.Add(gbTotais);
            pnForm.Controls.Add(gbFiltros);
            pnForm.Dock = DockStyle.Top;
            pnForm.Location = new Point(20, 85);
            pnForm.Name = "pnForm";
            pnForm.Size = new Size(1160, 220);
            pnForm.TabIndex = 1;
            // 
            // gbTotais
            // 
            gbTotais.Controls.Add(pnTotaisContainer);
            gbTotais.Dock = DockStyle.Right;
            gbTotais.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbTotais.ForeColor = Color.FromArgb(52, 73, 94);
            gbTotais.Location = new Point(830, 0);
            gbTotais.Name = "gbTotais";
            gbTotais.Padding = new Padding(15);
            gbTotais.Size = new Size(330, 220);
            gbTotais.TabIndex = 1;
            gbTotais.TabStop = false;
            gbTotais.Text = "💰 Totalizadores";
            // 
            // pnTotaisContainer
            // 
            pnTotaisContainer.Controls.Add(lblTotalIcon);
            pnTotaisContainer.Controls.Add(lblTotal);
            pnTotaisContainer.Controls.Add(lblTotalLabel);
            pnTotaisContainer.Dock = DockStyle.Fill;
            pnTotaisContainer.Location = new Point(15, 33);
            pnTotaisContainer.Name = "pnTotaisContainer";
            pnTotaisContainer.Size = new Size(300, 172);
            pnTotaisContainer.TabIndex = 0;
            // 
            // lblTotalIcon
            // 
            lblTotalIcon.Dock = DockStyle.Top;
            lblTotalIcon.Font = new Font("Segoe UI", 32F);
            lblTotalIcon.ForeColor = Color.FromArgb(155, 89, 182);
            lblTotalIcon.Location = new Point(0, 20);
            lblTotalIcon.Name = "lblTotalIcon";
            lblTotalIcon.Size = new Size(300, 60);
            lblTotalIcon.TabIndex = 0;
            lblTotalIcon.Text = "💰";
            lblTotalIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(155, 89, 182);
            lblTotal.Location = new Point(0, 20);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(300, 152);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "R$ 0,00";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalLabel
            // 
            lblTotalLabel.Dock = DockStyle.Top;
            lblTotalLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalLabel.ForeColor = Color.FromArgb(52, 73, 94);
            lblTotalLabel.Location = new Point(0, 0);
            lblTotalLabel.Name = "lblTotalLabel";
            lblTotalLabel.Size = new Size(300, 20);
            lblTotalLabel.TabIndex = 1;
            lblTotalLabel.Text = "Total de Vendas";
            lblTotalLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbFiltros
            // 
            gbFiltros.Controls.Add(pnExportacao);
            gbFiltros.Controls.Add(pnAcoes);
            gbFiltros.Controls.Add(pnDatas);
            gbFiltros.Dock = DockStyle.Fill;
            gbFiltros.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbFiltros.ForeColor = Color.FromArgb(52, 73, 94);
            gbFiltros.Location = new Point(0, 0);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Padding = new Padding(15);
            gbFiltros.Size = new Size(1160, 220);
            gbFiltros.TabIndex = 0;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "🔍 Filtros de Consulta";
            // 
            // pnExportacao
            // 
            pnExportacao.Controls.Add(btnExportarExcel);
            pnExportacao.Controls.Add(btnExportarPdf);
            pnExportacao.Dock = DockStyle.Top;
            pnExportacao.Location = new Point(15, 163);
            pnExportacao.Name = "pnExportacao";
            pnExportacao.Padding = new Padding(0, 10, 0, 10);
            pnExportacao.Size = new Size(1130, 60);
            pnExportacao.TabIndex = 2;
            // 
            // btnExportarExcel
            // 
            btnExportarExcel.BackColor = Color.FromArgb(76, 175, 80);
            btnExportarExcel.FlatAppearance.BorderSize = 0;
            btnExportarExcel.FlatAppearance.MouseOverBackColor = Color.FromArgb(67, 160, 71);
            btnExportarExcel.FlatStyle = FlatStyle.Flat;
            btnExportarExcel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnExportarExcel.ForeColor = Color.White;
            btnExportarExcel.Location = new Point(410, 10);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(180, 40);
            btnExportarExcel.TabIndex = 1;
            btnExportarExcel.Text = "📊 Exportar Excel";
            btnExportarExcel.UseVisualStyleBackColor = false;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // btnExportarPdf
            // 
            btnExportarPdf.BackColor = Color.FromArgb(244, 67, 54);
            btnExportarPdf.FlatAppearance.BorderSize = 0;
            btnExportarPdf.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 57, 53);
            btnExportarPdf.FlatStyle = FlatStyle.Flat;
            btnExportarPdf.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnExportarPdf.ForeColor = Color.White;
            btnExportarPdf.Location = new Point(210, 10);
            btnExportarPdf.Name = "btnExportarPdf";
            btnExportarPdf.Size = new Size(180, 40);
            btnExportarPdf.TabIndex = 0;
            btnExportarPdf.Text = "📄 Exportar PDF";
            btnExportarPdf.UseVisualStyleBackColor = false;
            btnExportarPdf.Click += btnExportarPdf_Click;
            // 
            // pnAcoes
            // 
            pnAcoes.Controls.Add(btnConsulta);
            pnAcoes.Controls.Add(btnLimpar);
            pnAcoes.Dock = DockStyle.Top;
            pnAcoes.Location = new Point(15, 113);
            pnAcoes.Name = "pnAcoes";
            pnAcoes.Padding = new Padding(0, 10, 0, 10);
            pnAcoes.Size = new Size(1130, 50);
            pnAcoes.TabIndex = 1;
            // 
            // btnConsulta
            // 
            btnConsulta.BackColor = Color.FromArgb(155, 89, 182);
            btnConsulta.FlatAppearance.BorderSize = 0;
            btnConsulta.FlatAppearance.MouseOverBackColor = Color.FromArgb(142, 68, 173);
            btnConsulta.FlatStyle = FlatStyle.Flat;
            btnConsulta.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnConsulta.ForeColor = Color.White;
            btnConsulta.Location = new Point(210, 10);
            btnConsulta.Name = "btnConsulta";
            btnConsulta.Size = new Size(180, 40);
            btnConsulta.TabIndex = 0;
            btnConsulta.Text = "🔍 Consultar";
            btnConsulta.UseVisualStyleBackColor = false;
            btnConsulta.Click += btnConsulta_Click;
            // 
            // btnLimpar
            // 
            btnLimpar.BackColor = Color.FromArgb(149, 165, 166);
            btnLimpar.FlatAppearance.BorderSize = 0;
            btnLimpar.FlatAppearance.MouseOverBackColor = Color.FromArgb(127, 140, 141);
            btnLimpar.FlatStyle = FlatStyle.Flat;
            btnLimpar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLimpar.ForeColor = Color.White;
            btnLimpar.Location = new Point(410, 10);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(180, 40);
            btnLimpar.TabIndex = 1;
            btnLimpar.Text = "\U0001f9f9 Limpar";
            btnLimpar.UseVisualStyleBackColor = false;
            btnLimpar.Click += btnLimpar_Click;
            // 
            // pnDatas
            // 
            pnDatas.Controls.Add(pnDataFinal);
            pnDatas.Controls.Add(pnDataInicial);
            pnDatas.Dock = DockStyle.Top;
            pnDatas.Location = new Point(15, 33);
            pnDatas.Name = "pnDatas";
            pnDatas.Padding = new Padding(0, 10, 0, 10);
            pnDatas.Size = new Size(1130, 80);
            pnDatas.TabIndex = 0;
            // 
            // pnDataFinal
            // 
            pnDataFinal.Controls.Add(dtpDataFinal);
            pnDataFinal.Controls.Add(lblDataFinalIcon);
            pnDataFinal.Controls.Add(lblDataFinal);
            pnDataFinal.Location = new Point(410, 10);
            pnDataFinal.Name = "pnDataFinal";
            pnDataFinal.Size = new Size(280, 60);
            pnDataFinal.TabIndex = 1;
            // 
            // dtpDataFinal
            // 
            dtpDataFinal.CalendarFont = new Font("Segoe UI", 10F);
            dtpDataFinal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dtpDataFinal.Format = DateTimePickerFormat.Short;
            dtpDataFinal.Location = new Point(40, 25);
            dtpDataFinal.Name = "dtpDataFinal";
            dtpDataFinal.Size = new Size(220, 29);
            dtpDataFinal.TabIndex = 1;
            dtpDataFinal.ValueChanged += dtpDataFinal_ValueChanged;
            // 
            // lblDataFinalIcon
            // 
            lblDataFinalIcon.Font = new Font("Segoe UI", 16F);
            lblDataFinalIcon.ForeColor = Color.FromArgb(155, 89, 182);
            lblDataFinalIcon.Location = new Point(0, 25);
            lblDataFinalIcon.Name = "lblDataFinalIcon";
            lblDataFinalIcon.Size = new Size(35, 29);
            lblDataFinalIcon.TabIndex = 2;
            lblDataFinalIcon.Text = "📅";
            lblDataFinalIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDataFinal
            // 
            lblDataFinal.AutoSize = true;
            lblDataFinal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDataFinal.ForeColor = Color.FromArgb(52, 73, 94);
            lblDataFinal.Location = new Point(0, 5);
            lblDataFinal.Name = "lblDataFinal";
            lblDataFinal.Size = new Size(109, 20);
            lblDataFinal.TabIndex = 0;
            lblDataFinal.Text = "📅 Data Final:";
            // 
            // pnDataInicial
            // 
            pnDataInicial.Controls.Add(dtpDataInicial);
            pnDataInicial.Controls.Add(lblDataInicialIcon);
            pnDataInicial.Controls.Add(lblDataInicial);
            pnDataInicial.Location = new Point(10, 10);
            pnDataInicial.Name = "pnDataInicial";
            pnDataInicial.Size = new Size(280, 60);
            pnDataInicial.TabIndex = 0;
            // 
            // dtpDataInicial
            // 
            dtpDataInicial.CalendarFont = new Font("Segoe UI", 10F);
            dtpDataInicial.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dtpDataInicial.Format = DateTimePickerFormat.Short;
            dtpDataInicial.Location = new Point(40, 25);
            dtpDataInicial.Name = "dtpDataInicial";
            dtpDataInicial.Size = new Size(220, 29);
            dtpDataInicial.TabIndex = 0;
            dtpDataInicial.ValueChanged += dtpDataInicial_ValueChanged;
            // 
            // lblDataInicialIcon
            // 
            lblDataInicialIcon.Font = new Font("Segoe UI", 16F);
            lblDataInicialIcon.ForeColor = Color.FromArgb(155, 89, 182);
            lblDataInicialIcon.Location = new Point(0, 25);
            lblDataInicialIcon.Name = "lblDataInicialIcon";
            lblDataInicialIcon.Size = new Size(35, 29);
            lblDataInicialIcon.TabIndex = 2;
            lblDataInicialIcon.Text = "📅";
            lblDataInicialIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDataInicial
            // 
            lblDataInicial.AutoSize = true;
            lblDataInicial.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDataInicial.ForeColor = Color.FromArgb(52, 73, 94);
            lblDataInicial.Location = new Point(0, 5);
            lblDataInicial.Name = "lblDataInicial";
            lblDataInicial.Size = new Size(117, 20);
            lblDataInicial.TabIndex = 0;
            lblDataInicial.Text = "📅 Data Inicial:";
            // 
            // pnHeader
            // 
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(lblInstrucoes);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(20, 20);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(1160, 65);
            pnHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(155, 89, 182);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(389, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📊 Relatório de Vendas por Data";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(0, 40);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Size = new Size(611, 19);
            lblInstrucoes.TabIndex = 1;
            lblInstrucoes.Text = "📊 Analise as vendas por período. Selecione as datas e clique em 'Consultar' para gerar o relatório.";
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 700);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(20, 10, 20, 10);
            pnFooter.Size = new Size(1200, 50);
            pnFooter.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(20, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(311, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "📊 Selecione o período e clique em 'Consultar' para iniciar";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(400, 20);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(780, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // frnRelatorioVendasPorData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 750);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frnRelatorioVendasPorData";
            Text = "Relatório de Vendas por Data";
            Load += frnRelatorioVendasPorData_Load;
            KeyDown += frnRelatorioVendasPorData_KeyDown;
            pnMain.ResumeLayout(false);
            pnGrid.ResumeLayout(false);
            pnGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRelatorio).EndInit();
            pnForm.ResumeLayout(false);
            gbTotais.ResumeLayout(false);
            pnTotaisContainer.ResumeLayout(false);
            gbFiltros.ResumeLayout(false);
            pnExportacao.ResumeLayout(false);
            pnAcoes.ResumeLayout(false);
            pnDatas.ResumeLayout(false);
            pnDataFinal.ResumeLayout(false);
            pnDataFinal.PerformLayout();
            pnDataInicial.ResumeLayout(false);
            pnDataInicial.PerformLayout();
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnFooter.ResumeLayout(false);
            pnFooter.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnGrid;
        private System.Windows.Forms.DataGridView dgvRelatorio;
        private System.Windows.Forms.Label lblGridInfo;
        private System.Windows.Forms.Panel pnForm;
        private System.Windows.Forms.GroupBox gbTotais;
        private System.Windows.Forms.Panel pnTotaisContainer;
        private System.Windows.Forms.Label lblTotalIcon;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.GroupBox gbFiltros;
        private System.Windows.Forms.Panel pnExportacao;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Panel pnAcoes;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Panel pnDatas;
        private System.Windows.Forms.Panel pnDataFinal;
        private System.Windows.Forms.DateTimePicker dtpDataFinal;
        private System.Windows.Forms.Label lblDataFinalIcon;
        private System.Windows.Forms.Label lblDataFinal;
        private System.Windows.Forms.Panel pnDataInicial;
        private System.Windows.Forms.DateTimePicker dtpDataInicial;
        private System.Windows.Forms.Label lblDataInicialIcon;
        private System.Windows.Forms.Label lblDataInicial;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.Panel pnFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer timer1;
    }
}