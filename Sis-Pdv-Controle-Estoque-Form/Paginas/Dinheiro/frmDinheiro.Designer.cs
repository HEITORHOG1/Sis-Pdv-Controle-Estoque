namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Dinheiro
{
    partial class frmDinheiro
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
            pnSugestoes = new Panel();
            btnSugestao100 = new Button();
            btnSugestao50 = new Button();
            btnSugestao20 = new Button();
            btnSugestaoExato = new Button();
            lblSugestoes = new Label();
            gbCalculos = new GroupBox();
            pnTroco = new Panel();
            lblValorTroco = new Label();
            lblTroco = new Label();
            pnAReceber = new Panel();
            lblValorAReceber = new Label();
            lblAReceber = new Label();
            pnTotal = new Panel();
            lblSubTotalValor = new Label();
            lblTotal = new Label();
            pnValorRecebido = new Panel();
            pnInput = new Panel();
            txbValorRecebido = new TextBox();
            lblInputIcon = new Label();
            lblValorRecebido = new Label();
            lblInstrucoes = new Label();
            pnFooter = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            pnButtonContainer = new Panel();
            btnCancelar = new Button();
            btnOK = new Button();
            pnHeader.SuspendLayout();
            pnMain.SuspendLayout();
            pnSugestoes.SuspendLayout();
            gbCalculos.SuspendLayout();
            pnTroco.SuspendLayout();
            pnAReceber.SuspendLayout();
            pnTotal.SuspendLayout();
            pnValorRecebido.SuspendLayout();
            pnInput.SuspendLayout();
            pnFooter.SuspendLayout();
            pnButtonContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.FromArgb(46, 204, 113);
            pnHeader.Controls.Add(lblTitulo);
            pnHeader.Controls.Add(btnClose);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Size = new Size(480, 60);
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
            lblTitulo.Size = new Size(300, 30);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "💰 Pagamento em Dinheiro";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(39, 174, 96);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(435, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 40);
            btnClose.TabIndex = 6;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnMain
            // 
            pnMain.BackColor = Color.White;
            pnMain.Controls.Add(pnSugestoes);
            pnMain.Controls.Add(gbCalculos);
            pnMain.Controls.Add(pnValorRecebido);
            pnMain.Controls.Add(lblInstrucoes);
            pnMain.Dock = DockStyle.Fill;
            pnMain.Location = new Point(0, 60);
            pnMain.Name = "pnMain";
            pnMain.Padding = new Padding(25, 20, 25, 20);
            pnMain.Size = new Size(480, 420);
            pnMain.TabIndex = 1;
            // 
            // pnSugestoes
            // 
            pnSugestoes.Controls.Add(btnSugestao100);
            pnSugestoes.Controls.Add(btnSugestao50);
            pnSugestoes.Controls.Add(btnSugestao20);
            pnSugestoes.Controls.Add(btnSugestaoExato);
            pnSugestoes.Controls.Add(lblSugestoes);
            pnSugestoes.Dock = DockStyle.Fill;
            pnSugestoes.Location = new Point(25, 134);
            pnSugestoes.Name = "pnSugestoes";
            pnSugestoes.Size = new Size(430, 96);
            pnSugestoes.TabIndex = 9;
            // 
            // btnSugestao100
            // 
            btnSugestao100.BackColor = Color.FromArgb(155, 89, 182);
            btnSugestao100.FlatAppearance.BorderSize = 0;
            btnSugestao100.FlatStyle = FlatStyle.Flat;
            btnSugestao100.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSugestao100.ForeColor = Color.White;
            btnSugestao100.Location = new Point(322, 35);
            btnSugestao100.Name = "btnSugestao100";
            btnSugestao100.Size = new Size(100, 35);
            btnSugestao100.TabIndex = 4;
            btnSugestao100.Text = "💵 R$ 100";
            btnSugestao100.UseVisualStyleBackColor = false;
            btnSugestao100.Click += btnSugestao_Click;
            // 
            // btnSugestao50
            // 
            btnSugestao50.BackColor = Color.FromArgb(142, 68, 173);
            btnSugestao50.FlatAppearance.BorderSize = 0;
            btnSugestao50.FlatStyle = FlatStyle.Flat;
            btnSugestao50.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSugestao50.ForeColor = Color.White;
            btnSugestao50.Location = new Point(216, 35);
            btnSugestao50.Name = "btnSugestao50";
            btnSugestao50.Size = new Size(100, 35);
            btnSugestao50.TabIndex = 3;
            btnSugestao50.Text = "💵 R$ 50";
            btnSugestao50.UseVisualStyleBackColor = false;
            btnSugestao50.Click += btnSugestao_Click;
            // 
            // btnSugestao20
            // 
            btnSugestao20.BackColor = Color.FromArgb(230, 126, 34);
            btnSugestao20.FlatAppearance.BorderSize = 0;
            btnSugestao20.FlatStyle = FlatStyle.Flat;
            btnSugestao20.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSugestao20.ForeColor = Color.White;
            btnSugestao20.Location = new Point(110, 35);
            btnSugestao20.Name = "btnSugestao20";
            btnSugestao20.Size = new Size(100, 35);
            btnSugestao20.TabIndex = 2;
            btnSugestao20.Text = "💵 R$ 20";
            btnSugestao20.UseVisualStyleBackColor = false;
            btnSugestao20.Click += btnSugestao_Click;
            // 
            // btnSugestaoExato
            // 
            btnSugestaoExato.BackColor = Color.FromArgb(39, 174, 96);
            btnSugestaoExato.FlatAppearance.BorderSize = 0;
            btnSugestaoExato.FlatStyle = FlatStyle.Flat;
            btnSugestaoExato.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSugestaoExato.ForeColor = Color.White;
            btnSugestaoExato.Location = new Point(4, 35);
            btnSugestaoExato.Name = "btnSugestaoExato";
            btnSugestaoExato.Size = new Size(100, 35);
            btnSugestaoExato.TabIndex = 1;
            btnSugestaoExato.Text = "✓ Exato";
            btnSugestaoExato.UseVisualStyleBackColor = false;
            btnSugestaoExato.Click += btnSugestao_Click;
            // 
            // lblSugestoes
            // 
            lblSugestoes.AutoSize = true;
            lblSugestoes.Dock = DockStyle.Top;
            lblSugestoes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSugestoes.ForeColor = Color.FromArgb(52, 73, 94);
            lblSugestoes.Location = new Point(0, 0);
            lblSugestoes.Name = "lblSugestoes";
            lblSugestoes.Padding = new Padding(0, 0, 0, 15);
            lblSugestoes.Size = new Size(157, 34);
            lblSugestoes.TabIndex = 0;
            lblSugestoes.Text = "💡 Valores Sugeridos:";
            // 
            // gbCalculos
            // 
            gbCalculos.Controls.Add(pnTroco);
            gbCalculos.Controls.Add(pnAReceber);
            gbCalculos.Controls.Add(pnTotal);
            gbCalculos.Dock = DockStyle.Bottom;
            gbCalculos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbCalculos.ForeColor = Color.FromArgb(52, 73, 94);
            gbCalculos.Location = new Point(25, 230);
            gbCalculos.Name = "gbCalculos";
            gbCalculos.Padding = new Padding(15);
            gbCalculos.Size = new Size(430, 170);
            gbCalculos.TabIndex = 7;
            gbCalculos.TabStop = false;
            gbCalculos.Text = "📊 Resumo do Pagamento";
            // 
            // pnTroco
            // 
            pnTroco.BackColor = Color.FromArgb(236, 240, 241);
            pnTroco.Controls.Add(lblValorTroco);
            pnTroco.Controls.Add(lblTroco);
            pnTroco.Dock = DockStyle.Top;
            pnTroco.Location = new Point(15, 123);
            pnTroco.Name = "pnTroco";
            pnTroco.Padding = new Padding(15, 10, 15, 10);
            pnTroco.Size = new Size(400, 45);
            pnTroco.TabIndex = 2;
            // 
            // lblValorTroco
            // 
            lblValorTroco.Dock = DockStyle.Right;
            lblValorTroco.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblValorTroco.ForeColor = Color.FromArgb(46, 204, 113);
            lblValorTroco.Location = new Point(285, 10);
            lblValorTroco.Name = "lblValorTroco";
            lblValorTroco.Size = new Size(100, 25);
            lblValorTroco.TabIndex = 1;
            lblValorTroco.Text = "R$ 0,00";
            lblValorTroco.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTroco
            // 
            lblTroco.AutoSize = true;
            lblTroco.Dock = DockStyle.Left;
            lblTroco.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTroco.ForeColor = Color.FromArgb(52, 73, 94);
            lblTroco.Location = new Point(15, 10);
            lblTroco.Name = "lblTroco";
            lblTroco.Padding = new Padding(0, 5, 0, 0);
            lblTroco.Size = new Size(83, 26);
            lblTroco.TabIndex = 0;
            lblTroco.Text = "💸 Troco:";
            // 
            // pnAReceber
            // 
            pnAReceber.BackColor = Color.FromArgb(255, 248, 220);
            pnAReceber.Controls.Add(lblValorAReceber);
            pnAReceber.Controls.Add(lblAReceber);
            pnAReceber.Dock = DockStyle.Top;
            pnAReceber.Location = new Point(15, 78);
            pnAReceber.Name = "pnAReceber";
            pnAReceber.Padding = new Padding(15, 10, 15, 10);
            pnAReceber.Size = new Size(400, 45);
            pnAReceber.TabIndex = 1;
            // 
            // lblValorAReceber
            // 
            lblValorAReceber.Dock = DockStyle.Right;
            lblValorAReceber.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblValorAReceber.ForeColor = Color.FromArgb(230, 126, 34);
            lblValorAReceber.Location = new Point(285, 10);
            lblValorAReceber.Name = "lblValorAReceber";
            lblValorAReceber.Size = new Size(100, 25);
            lblValorAReceber.TabIndex = 1;
            lblValorAReceber.Text = "R$ 0,00";
            lblValorAReceber.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblAReceber
            // 
            lblAReceber.AutoSize = true;
            lblAReceber.Dock = DockStyle.Left;
            lblAReceber.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAReceber.ForeColor = Color.FromArgb(52, 73, 94);
            lblAReceber.Location = new Point(15, 10);
            lblAReceber.Name = "lblAReceber";
            lblAReceber.Padding = new Padding(0, 5, 0, 0);
            lblAReceber.Size = new Size(118, 26);
            lblAReceber.TabIndex = 0;
            lblAReceber.Text = "⏳ A Receber:";
            // 
            // pnTotal
            // 
            pnTotal.BackColor = Color.FromArgb(52, 152, 219);
            pnTotal.Controls.Add(lblSubTotalValor);
            pnTotal.Controls.Add(lblTotal);
            pnTotal.Dock = DockStyle.Top;
            pnTotal.Location = new Point(15, 33);
            pnTotal.Name = "pnTotal";
            pnTotal.Padding = new Padding(15, 10, 15, 10);
            pnTotal.Size = new Size(400, 45);
            pnTotal.TabIndex = 0;
            // 
            // lblSubTotalValor
            // 
            lblSubTotalValor.Dock = DockStyle.Right;
            lblSubTotalValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSubTotalValor.ForeColor = Color.White;
            lblSubTotalValor.Location = new Point(285, 10);
            lblSubTotalValor.Name = "lblSubTotalValor";
            lblSubTotalValor.Size = new Size(100, 25);
            lblSubTotalValor.TabIndex = 1;
            lblSubTotalValor.Text = "R$ 0,00";
            lblSubTotalValor.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Left;
            lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotal.ForeColor = Color.White;
            lblTotal.Location = new Point(15, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new Padding(0, 5, 0, 0);
            lblTotal.Size = new Size(154, 26);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "💰 Total da Venda:";
            // 
            // pnValorRecebido
            // 
            pnValorRecebido.Controls.Add(pnInput);
            pnValorRecebido.Controls.Add(lblValorRecebido);
            pnValorRecebido.Dock = DockStyle.Top;
            pnValorRecebido.Location = new Point(25, 64);
            pnValorRecebido.Name = "pnValorRecebido";
            pnValorRecebido.Size = new Size(430, 70);
            pnValorRecebido.TabIndex = 6;
            // 
            // pnInput
            // 
            pnInput.BackColor = Color.FromArgb(236, 240, 241);
            pnInput.Controls.Add(txbValorRecebido);
            pnInput.Controls.Add(lblInputIcon);
            pnInput.Dock = DockStyle.Top;
            pnInput.Location = new Point(0, 25);
            pnInput.Name = "pnInput";
            pnInput.Padding = new Padding(15, 10, 15, 10);
            pnInput.Size = new Size(430, 45);
            pnInput.TabIndex = 2;
            // 
            // txbValorRecebido
            // 
            txbValorRecebido.BackColor = Color.FromArgb(236, 240, 241);
            txbValorRecebido.BorderStyle = BorderStyle.None;
            txbValorRecebido.Dock = DockStyle.Fill;
            txbValorRecebido.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txbValorRecebido.ForeColor = Color.FromArgb(52, 73, 94);
            txbValorRecebido.Location = new Point(50, 10);
            txbValorRecebido.Name = "txbValorRecebido";
            txbValorRecebido.Size = new Size(365, 29);
            txbValorRecebido.TabIndex = 0;
            txbValorRecebido.Text = "R$ 0,00";
            txbValorRecebido.TextAlign = HorizontalAlignment.Center;
            txbValorRecebido.TextChanged += txbValorRecebido_TextChanged;
            txbValorRecebido.Enter += txbValorRecebido_Enter;
            txbValorRecebido.KeyPress += txbValorRecebido_KeyPress;
            txbValorRecebido.Leave += txbValorRecebido_Leave;
            // 
            // lblInputIcon
            // 
            lblInputIcon.Dock = DockStyle.Left;
            lblInputIcon.Font = new Font("Segoe UI", 16F);
            lblInputIcon.ForeColor = Color.FromArgb(149, 165, 166);
            lblInputIcon.Location = new Point(15, 10);
            lblInputIcon.Name = "lblInputIcon";
            lblInputIcon.Size = new Size(35, 25);
            lblInputIcon.TabIndex = 1;
            lblInputIcon.Text = "💵";
            lblInputIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblValorRecebido
            // 
            lblValorRecebido.AutoSize = true;
            lblValorRecebido.Dock = DockStyle.Top;
            lblValorRecebido.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblValorRecebido.ForeColor = Color.FromArgb(52, 73, 94);
            lblValorRecebido.Location = new Point(0, 0);
            lblValorRecebido.Name = "lblValorRecebido";
            lblValorRecebido.Padding = new Padding(0, 0, 0, 5);
            lblValorRecebido.Size = new Size(233, 25);
            lblValorRecebido.TabIndex = 1;
            lblValorRecebido.Text = "💴 Valor Recebido em Dinheiro:";
            // 
            // lblInstrucoes
            // 
            lblInstrucoes.AutoSize = true;
            lblInstrucoes.Dock = DockStyle.Top;
            lblInstrucoes.Font = new Font("Segoe UI", 10F);
            lblInstrucoes.ForeColor = Color.FromArgb(127, 140, 141);
            lblInstrucoes.Location = new Point(25, 20);
            lblInstrucoes.Name = "lblInstrucoes";
            lblInstrucoes.Padding = new Padding(0, 0, 0, 25);
            lblInstrucoes.Size = new Size(364, 44);
            lblInstrucoes.TabIndex = 8;
            lblInstrucoes.Text = "💡 Digite o valor recebido do cliente para calcular o troco.";
            // 
            // pnFooter
            // 
            pnFooter.BackColor = Color.FromArgb(52, 73, 94);
            pnFooter.Controls.Add(lblStatus);
            pnFooter.Controls.Add(progressBar);
            pnFooter.Controls.Add(pnButtonContainer);
            pnFooter.Dock = DockStyle.Bottom;
            pnFooter.Location = new Point(0, 480);
            pnFooter.Name = "pnFooter";
            pnFooter.Padding = new Padding(25, 20, 25, 20);
            pnFooter.Size = new Size(480, 90);
            pnFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(25, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(208, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "💰 Digite o valor recebido em dinheiro";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(25, 50);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(430, 6);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 5;
            progressBar.Visible = false;
            // 
            // pnButtonContainer
            // 
            pnButtonContainer.Controls.Add(btnCancelar);
            pnButtonContainer.Controls.Add(btnOK);
            pnButtonContainer.Dock = DockStyle.Top;
            pnButtonContainer.Location = new Point(25, 20);
            pnButtonContainer.Name = "pnButtonContainer";
            pnButtonContainer.Size = new Size(430, 40);
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
            btnCancelar.Location = new Point(215, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(215, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "🚫 Cancelar (ESC)";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.FromArgb(46, 204, 113);
            btnOK.Dock = DockStyle.Left;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(0, 0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(210, 40);
            btnOK.TabIndex = 3;
            btnOK.Text = "✓ Confirmar (ENTER)";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // frmDinheiro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 570);
            Controls.Add(pnMain);
            Controls.Add(pnFooter);
            Controls.Add(pnHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmDinheiro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pagamento em Dinheiro";
            Load += frmDinheiro_Load;
            KeyDown += frmDinheiro_KeyDown;
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            pnSugestoes.ResumeLayout(false);
            pnSugestoes.PerformLayout();
            gbCalculos.ResumeLayout(false);
            pnTroco.ResumeLayout(false);
            pnTroco.PerformLayout();
            pnAReceber.ResumeLayout(false);
            pnAReceber.PerformLayout();
            pnTotal.ResumeLayout(false);
            pnTotal.PerformLayout();
            pnValorRecebido.ResumeLayout(false);
            pnValorRecebido.PerformLayout();
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
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnValorRecebido;
        private System.Windows.Forms.Panel pnInput;
        private System.Windows.Forms.TextBox txbValorRecebido;
        private System.Windows.Forms.Label lblInputIcon;
        private System.Windows.Forms.Label lblValorRecebido;
        private System.Windows.Forms.Label lblInstrucoes;
        private System.Windows.Forms.GroupBox gbCalculos;
        private System.Windows.Forms.Panel pnTroco;
        private System.Windows.Forms.Label lblValorTroco;
        private System.Windows.Forms.Label lblTroco;
        private System.Windows.Forms.Panel pnAReceber;
        private System.Windows.Forms.Label lblValorAReceber;
        private System.Windows.Forms.Label lblAReceber;
        private System.Windows.Forms.Panel pnTotal;
        private System.Windows.Forms.Label lblSubTotalValor;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel pnSugestoes;
        private System.Windows.Forms.Button btnSugestao100;
        private System.Windows.Forms.Button btnSugestao50;
        private System.Windows.Forms.Button btnSugestao20;
        private System.Windows.Forms.Button btnSugestaoExato;
        private System.Windows.Forms.Label lblSugestoes;
    }
}