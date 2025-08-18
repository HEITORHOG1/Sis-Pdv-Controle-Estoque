namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    partial class frmTelaPdv
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            
            // Header Panel
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblTituloPdv = new System.Windows.Forms.Label();
            this.lblStatusCaixa = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            // Top Info Panel
            this.pnTopInfo = new System.Windows.Forms.Panel();
            this.lblOperadorLabel = new System.Windows.Forms.Label();
            this.lblNomeOperador = new System.Windows.Forms.Label();
            this.lblCaixaLabel = new System.Windows.Forms.Label();
            this.lblCaixa = new System.Windows.Forms.Label();
            this.lblDataLabel = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblHoraLabel = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            
            // Left Panel - Product Input
            this.pnLeftInput = new System.Windows.Forms.Panel();
            this.gbProdutoInfo = new System.Windows.Forms.GroupBox();
            this.lblCodigoProduto = new System.Windows.Forms.Label();
            this.txbCodBarras = new System.Windows.Forms.TextBox();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.txbDescricao = new System.Windows.Forms.TextBox();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.txbQuantidade = new System.Windows.Forms.TextBox();
            this.lblPrecoUnit = new System.Windows.Forms.Label();
            this.txbPrecoUnit = new System.Windows.Forms.TextBox();
            this.lblTotalItem = new System.Windows.Forms.Label();
            this.txbTotalRecebido = new System.Windows.Forms.TextBox();
            
            // Controls Panel
            this.pnControles = new System.Windows.Forms.Panel();
            this.gbAcoes = new System.Windows.Forms.GroupBox();
            this.btnPagamentoDinheiro = new System.Windows.Forms.Button();
            this.btnPagamentoCartao = new System.Windows.Forms.Button();
            this.btnPagamentoPix = new System.Windows.Forms.Button();
            this.btnCancelarItem = new System.Windows.Forms.Button();
            this.btnCancelarVenda = new System.Windows.Forms.Button();
            this.btnFinalizarVenda = new System.Windows.Forms.Button();
            this.btnLimparCampos = new System.Windows.Forms.Button();
            this.btnAjuda = new System.Windows.Forms.Button();
            
            // Center Panel - Cart
            this.pnCenter = new System.Windows.Forms.Panel();
            this.gbCarrinho = new System.Windows.Forms.GroupBox();
            this.dgvCarrinho = new System.Windows.Forms.DataGridView();
            
            // Bottom Panel - Totals and Payment
            this.pnBottom = new System.Windows.Forms.Panel();
            this.gbTotais = new System.Windows.Forms.GroupBox();
            this.lblSubTotalLabel = new System.Windows.Forms.Label();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblDescontoLabel = new System.Windows.Forms.Label();
            this.lblDesconto = new System.Windows.Forms.Label();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            
            this.gbPagamento = new System.Windows.Forms.GroupBox();
            this.lblFormaPagamentoLabel = new System.Windows.Forms.Label();
            this.lblFormaPagamento = new System.Windows.Forms.Label();
            this.lblValorRecebidoLabel = new System.Windows.Forms.Label();
            this.lblValorAReceber = new System.Windows.Forms.Label();
            this.lblTrocoLabel = new System.Windows.Forms.Label();
            this.lblTroco = new System.Windows.Forms.Label();
            
            // Status Panel
            this.pnStatus = new System.Windows.Forms.Panel();
            this.lblStatusOperacao = new System.Windows.Forms.Label();
            this.lblNomeCaixa = new System.Windows.Forms.Label();
            this.progressOperacao = new System.Windows.Forms.ProgressBar();
            
            // Timer
            this.timerData = new System.Windows.Forms.Timer(this.components);
            
            // Suspension of layout
            this.pnHeader.SuspendLayout();
            this.pnTopInfo.SuspendLayout();
            this.pnLeftInput.SuspendLayout();
            this.gbProdutoInfo.SuspendLayout();
            this.pnControles.SuspendLayout();
            this.gbAcoes.SuspendLayout();
            this.pnCenter.SuspendLayout();
            this.gbCarrinho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrinho)).BeginInit();
            this.pnBottom.SuspendLayout();
            this.gbTotais.SuspendLayout();
            this.gbPagamento.SuspendLayout();
            this.pnStatus.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.pnHeader.Controls.Add(this.lblTituloPdv);
            this.pnHeader.Controls.Add(this.lblStatusCaixa);
            this.pnHeader.Controls.Add(this.btnMinimizar);
            this.pnHeader.Controls.Add(this.btnClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(1920, 50);
            this.pnHeader.TabIndex = 0;
            
            // 
            // lblTituloPdv
            // 
            this.lblTituloPdv.AutoSize = true;
            this.lblTituloPdv.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloPdv.ForeColor = System.Drawing.Color.White;
            this.lblTituloPdv.Location = new System.Drawing.Point(20, 12);
            this.lblTituloPdv.Name = "lblTituloPdv";
            this.lblTituloPdv.Size = new System.Drawing.Size(270, 30);
            this.lblTituloPdv.TabIndex = 0;
            this.lblTituloPdv.Text = "🏪 SISTEMA PDV - MODERNO";
            
            // 
            // lblStatusCaixa
            // 
            this.lblStatusCaixa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatusCaixa.AutoSize = true;
            this.lblStatusCaixa.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblStatusCaixa.ForeColor = System.Drawing.Color.LightGreen;
            this.lblStatusCaixa.Location = new System.Drawing.Point(850, 15);
            this.lblStatusCaixa.Name = "lblStatusCaixa";
            this.lblStatusCaixa.Size = new System.Drawing.Size(220, 25);
            this.lblStatusCaixa.TabIndex = 1;
            this.lblStatusCaixa.Text = "🟢 CAIXA ABERTO";
            
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(1820, 10);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(40, 30);
            this.btnMinimizar.TabIndex = 2;
            this.btnMinimizar.Text = "─";
            this.btnMinimizar.UseVisualStyleBackColor = false;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1870, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // 
            // pnTopInfo
            // 
            this.pnTopInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnTopInfo.Controls.Add(this.lblOperadorLabel);
            this.pnTopInfo.Controls.Add(this.lblNomeOperador);
            this.pnTopInfo.Controls.Add(this.lblCaixaLabel);
            this.pnTopInfo.Controls.Add(this.lblCaixa);
            this.pnTopInfo.Controls.Add(this.lblDataLabel);
            this.pnTopInfo.Controls.Add(this.lblData);
            this.pnTopInfo.Controls.Add(this.lblHoraLabel);
            this.pnTopInfo.Controls.Add(this.lblHora);
            this.pnTopInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopInfo.Location = new System.Drawing.Point(0, 50);
            this.pnTopInfo.Name = "pnTopInfo";
            this.pnTopInfo.Size = new System.Drawing.Size(1920, 40);
            this.pnTopInfo.TabIndex = 1;
            
            // 
            // lblOperadorLabel
            // 
            this.lblOperadorLabel.AutoSize = true;
            this.lblOperadorLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOperadorLabel.ForeColor = System.Drawing.Color.White;
            this.lblOperadorLabel.Location = new System.Drawing.Point(20, 10);
            this.lblOperadorLabel.Name = "lblOperadorLabel";
            this.lblOperadorLabel.Size = new System.Drawing.Size(103, 19);
            this.lblOperadorLabel.TabIndex = 0;
            this.lblOperadorLabel.Text = "👤 Operador:";
            
            // 
            // lblNomeOperador
            // 
            this.lblNomeOperador.AutoSize = true;
            this.lblNomeOperador.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNomeOperador.ForeColor = System.Drawing.Color.LightBlue;
            this.lblNomeOperador.Location = new System.Drawing.Point(130, 10);
            this.lblNomeOperador.Name = "lblNomeOperador";
            this.lblNomeOperador.Size = new System.Drawing.Size(100, 19);
            this.lblNomeOperador.TabIndex = 1;
            this.lblNomeOperador.Text = "Nome Operador";
            
            // 
            // lblCaixaLabel
            // 
            this.lblCaixaLabel.AutoSize = true;
            this.lblCaixaLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCaixaLabel.ForeColor = System.Drawing.Color.White;
            this.lblCaixaLabel.Location = new System.Drawing.Point(400, 10);
            this.lblCaixaLabel.Name = "lblCaixaLabel";
            this.lblCaixaLabel.Size = new System.Drawing.Size(72, 19);
            this.lblCaixaLabel.TabIndex = 2;
            this.lblCaixaLabel.Text = "🏪 Caixa:";
            
            // 
            // lblCaixa
            // 
            this.lblCaixa.AutoSize = true;
            this.lblCaixa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCaixa.ForeColor = System.Drawing.Color.LightGreen;
            this.lblCaixa.Location = new System.Drawing.Point(480, 10);
            this.lblCaixa.Name = "lblCaixa";
            this.lblCaixa.Size = new System.Drawing.Size(73, 19);
            this.lblCaixa.TabIndex = 3;
            this.lblCaixa.Text = "CAIXA-001";
            
            // 
            // lblDataLabel
            // 
            this.lblDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataLabel.AutoSize = true;
            this.lblDataLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDataLabel.ForeColor = System.Drawing.Color.White;
            this.lblDataLabel.Location = new System.Drawing.Point(1650, 10);
            this.lblDataLabel.Name = "lblDataLabel";
            this.lblDataLabel.Size = new System.Drawing.Size(62, 19);
            this.lblDataLabel.TabIndex = 4;
            this.lblDataLabel.Text = "📅 Data:";
            
            // 
            // lblData
            // 
            this.lblData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblData.ForeColor = System.Drawing.Color.LightYellow;
            this.lblData.Location = new System.Drawing.Point(1720, 10);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(74, 19);
            this.lblData.TabIndex = 5;
            this.lblData.Text = "00/00/0000";
            
            // 
            // lblHoraLabel
            // 
            this.lblHoraLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHoraLabel.AutoSize = true;
            this.lblHoraLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHoraLabel.ForeColor = System.Drawing.Color.White;
            this.lblHoraLabel.Location = new System.Drawing.Point(1810, 10);
            this.lblHoraLabel.Name = "lblHoraLabel";
            this.lblHoraLabel.Size = new System.Drawing.Size(62, 19);
            this.lblHoraLabel.TabIndex = 6;
            this.lblHoraLabel.Text = "🕐 Hora:";
            
            // 
            // lblHora
            // 
            this.lblHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHora.ForeColor = System.Drawing.Color.LightYellow;
            this.lblHora.Location = new System.Drawing.Point(1880, 10);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(57, 19);
            this.lblHora.TabIndex = 7;
            this.lblHora.Text = "00:00:00";
            
            // 
            // pnLeftInput
            // 
            this.pnLeftInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnLeftInput.Controls.Add(this.gbProdutoInfo);
            this.pnLeftInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftInput.Location = new System.Drawing.Point(0, 90);
            this.pnLeftInput.Name = "pnLeftInput";
            this.pnLeftInput.Padding = new System.Windows.Forms.Padding(10);
            this.pnLeftInput.Size = new System.Drawing.Size(400, 700);
            this.pnLeftInput.TabIndex = 2;
            
            // 
            // gbProdutoInfo
            // 
            this.gbProdutoInfo.Controls.Add(this.lblCodigoProduto);
            this.gbProdutoInfo.Controls.Add(this.txbCodBarras);
            this.gbProdutoInfo.Controls.Add(this.lblDescricao);
            this.gbProdutoInfo.Controls.Add(this.txbDescricao);
            this.gbProdutoInfo.Controls.Add(this.lblQuantidade);
            this.gbProdutoInfo.Controls.Add(this.txbQuantidade);
            this.gbProdutoInfo.Controls.Add(this.lblPrecoUnit);
            this.gbProdutoInfo.Controls.Add(this.txbPrecoUnit);
            this.gbProdutoInfo.Controls.Add(this.lblTotalItem);
            this.gbProdutoInfo.Controls.Add(this.txbTotalRecebido);
            this.gbProdutoInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProdutoInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbProdutoInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.gbProdutoInfo.Location = new System.Drawing.Point(10, 10);
            this.gbProdutoInfo.Name = "gbProdutoInfo";
            this.gbProdutoInfo.Size = new System.Drawing.Size(380, 680);
            this.gbProdutoInfo.TabIndex = 0;
            this.gbProdutoInfo.TabStop = false;
            this.gbProdutoInfo.Text = "📦 INFORMAÇÕES DO PRODUTO";
            
            // 
            // lblCodigoProduto
            // 
            this.lblCodigoProduto.AutoSize = true;
            this.lblCodigoProduto.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCodigoProduto.Location = new System.Drawing.Point(20, 40);
            this.lblCodigoProduto.Name = "lblCodigoProduto";
            this.lblCodigoProduto.Size = new System.Drawing.Size(199, 20);
            this.lblCodigoProduto.TabIndex = 0;
            this.lblCodigoProduto.Text = "🔍 Código de Barras (F2):";
            
            // 
            // txbCodBarras
            // 
            this.txbCodBarras.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.txbCodBarras.Location = new System.Drawing.Point(20, 70);
            this.txbCodBarras.Name = "txbCodBarras";
            this.txbCodBarras.Size = new System.Drawing.Size(340, 32);
            this.txbCodBarras.TabIndex = 1;
            this.txbCodBarras.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbCodBarras.TextChanged += new System.EventHandler(this.txbCodBarras_TextChanged);
            this.txbCodBarras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbCodBarras_KeyPress);
            
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDescricao.Location = new System.Drawing.Point(20, 120);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(144, 20);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "📝 Descrição:";
            
            // 
            // txbDescricao
            // 
            this.txbDescricao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txbDescricao.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txbDescricao.Location = new System.Drawing.Point(20, 150);
            this.txbDescricao.Multiline = true;
            this.txbDescricao.Name = "txbDescricao";
            this.txbDescricao.ReadOnly = true;
            this.txbDescricao.Size = new System.Drawing.Size(340, 80);
            this.txbDescricao.TabIndex = 3;
            this.txbDescricao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblQuantidade.Location = new System.Drawing.Point(20, 250);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(127, 20);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "🔢 Quantidade:";
            
            // 
            // txbQuantidade
            // 
            this.txbQuantidade.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.txbQuantidade.Location = new System.Drawing.Point(20, 280);
            this.txbQuantidade.Name = "txbQuantidade";
            this.txbQuantidade.Size = new System.Drawing.Size(150, 32);
            this.txbQuantidade.TabIndex = 5;
            this.txbQuantidade.Text = "1";
            this.txbQuantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbQuantidade.TextChanged += new System.EventHandler(this.txbQuantidade_TextChanged);
            this.txbQuantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbQuantidade_KeyPress);
            
            // 
            // lblPrecoUnit
            // 
            this.lblPrecoUnit.AutoSize = true;
            this.lblPrecoUnit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPrecoUnit.Location = new System.Drawing.Point(200, 250);
            this.lblPrecoUnit.Name = "lblPrecoUnit";
            this.lblPrecoUnit.Size = new System.Drawing.Size(147, 20);
            this.lblPrecoUnit.TabIndex = 6;
            this.lblPrecoUnit.Text = "💰 Preço Unitário:";
            
            // 
            // txbPrecoUnit
            // 
            this.txbPrecoUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txbPrecoUnit.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.txbPrecoUnit.ForeColor = System.Drawing.Color.Green;
            this.txbPrecoUnit.Location = new System.Drawing.Point(200, 280);
            this.txbPrecoUnit.Name = "txbPrecoUnit";
            this.txbPrecoUnit.ReadOnly = true;
            this.txbPrecoUnit.Size = new System.Drawing.Size(160, 32);
            this.txbPrecoUnit.TabIndex = 7;
            this.txbPrecoUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            
            // 
            // lblTotalItem
            // 
            this.lblTotalItem.AutoSize = true;
            this.lblTotalItem.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalItem.Location = new System.Drawing.Point(20, 330);
            this.lblTotalItem.Name = "lblTotalItem";
            this.lblTotalItem.Size = new System.Drawing.Size(106, 20);
            this.lblTotalItem.TabIndex = 8;
            this.lblTotalItem.Text = "💵 Total Item:";
            
            // 
            // txbTotalRecebido
            // 
            this.txbTotalRecebido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.txbTotalRecebido.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.txbTotalRecebido.ForeColor = System.Drawing.Color.White;
            this.txbTotalRecebido.Location = new System.Drawing.Point(20, 360);
            this.txbTotalRecebido.Name = "txbTotalRecebido";
            this.txbTotalRecebido.ReadOnly = true;
            this.txbTotalRecebido.Size = new System.Drawing.Size(340, 36);
            this.txbTotalRecebido.TabIndex = 9;
            this.txbTotalRecebido.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            
            // 
            // pnControles
            // 
            this.pnControles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.pnControles.Controls.Add(this.gbAcoes);
            this.pnControles.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnControles.Location = new System.Drawing.Point(1520, 90);
            this.pnControles.Name = "pnControles";
            this.pnControles.Padding = new System.Windows.Forms.Padding(10);
            this.pnControles.Size = new System.Drawing.Size(400, 700);
            this.pnControles.TabIndex = 3;
            
            // 
            // gbAcoes
            // 
            this.gbAcoes.Controls.Add(this.btnPagamentoDinheiro);
            this.gbAcoes.Controls.Add(this.btnPagamentoCartao);
            this.gbAcoes.Controls.Add(this.btnPagamentoPix);
            this.gbAcoes.Controls.Add(this.btnCancelarItem);
            this.gbAcoes.Controls.Add(this.btnCancelarVenda);
            this.gbAcoes.Controls.Add(this.btnFinalizarVenda);
            this.gbAcoes.Controls.Add(this.btnLimparCampos);
            this.gbAcoes.Controls.Add(this.btnAjuda);
            this.gbAcoes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAcoes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbAcoes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.gbAcoes.Location = new System.Drawing.Point(10, 10);
            this.gbAcoes.Name = "gbAcoes";
            this.gbAcoes.Size = new System.Drawing.Size(380, 680);
            this.gbAcoes.TabIndex = 0;
            this.gbAcoes.TabStop = false;
            this.gbAcoes.Text = "⚡ AÇÕES E PAGAMENTOS";
            
            // 
            // btnPagamentoDinheiro
            // 
            this.btnPagamentoDinheiro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnPagamentoDinheiro.FlatAppearance.BorderSize = 0;
            this.btnPagamentoDinheiro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamentoDinheiro.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnPagamentoDinheiro.ForeColor = System.Drawing.Color.White;
            this.btnPagamentoDinheiro.Location = new System.Drawing.Point(20, 40);
            this.btnPagamentoDinheiro.Name = "btnPagamentoDinheiro";
            this.btnPagamentoDinheiro.Size = new System.Drawing.Size(340, 60);
            this.btnPagamentoDinheiro.TabIndex = 0;
            this.btnPagamentoDinheiro.Text = "💰 DINHEIRO (D)";
            this.btnPagamentoDinheiro.UseVisualStyleBackColor = false;
            this.btnPagamentoDinheiro.Click += new System.EventHandler(this.btnPagamentoDinheiro_Click);
            
            // 
            // btnPagamentoCartao
            // 
            this.btnPagamentoCartao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPagamentoCartao.FlatAppearance.BorderSize = 0;
            this.btnPagamentoCartao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamentoCartao.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnPagamentoCartao.ForeColor = System.Drawing.Color.White;
            this.btnPagamentoCartao.Location = new System.Drawing.Point(20, 110);
            this.btnPagamentoCartao.Name = "btnPagamentoCartao";
            this.btnPagamentoCartao.Size = new System.Drawing.Size(340, 60);
            this.btnPagamentoCartao.TabIndex = 1;
            this.btnPagamentoCartao.Text = "💳 CARTÃO (C)";
            this.btnPagamentoCartao.UseVisualStyleBackColor = false;
            this.btnPagamentoCartao.Click += new System.EventHandler(this.btnPagamentoCartao_Click);
            
            // 
            // btnPagamentoPix
            // 
            this.btnPagamentoPix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnPagamentoPix.FlatAppearance.BorderSize = 0;
            this.btnPagamentoPix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamentoPix.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnPagamentoPix.ForeColor = System.Drawing.Color.White;
            this.btnPagamentoPix.Location = new System.Drawing.Point(20, 180);
            this.btnPagamentoPix.Name = "btnPagamentoPix";
            this.btnPagamentoPix.Size = new System.Drawing.Size(340, 60);
            this.btnPagamentoPix.TabIndex = 2;
            this.btnPagamentoPix.Text = "📱 PIX (P)";
            this.btnPagamentoPix.UseVisualStyleBackColor = false;
            this.btnPagamentoPix.Click += new System.EventHandler(this.btnPagamentoPix_Click);
            
            // 
            // btnCancelarItem
            // 
            this.btnCancelarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnCancelarItem.FlatAppearance.BorderSize = 0;
            this.btnCancelarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarItem.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCancelarItem.ForeColor = System.Drawing.Color.White;
            this.btnCancelarItem.Location = new System.Drawing.Point(20, 250);
            this.btnCancelarItem.Name = "btnCancelarItem";
            this.btnCancelarItem.Size = new System.Drawing.Size(340, 60);
            this.btnCancelarItem.TabIndex = 3;
            this.btnCancelarItem.Text = "❌ CANCELAR ITEM (F2)";
            this.btnCancelarItem.UseVisualStyleBackColor = false;
            this.btnCancelarItem.Click += new System.EventHandler(this.btnCancelarItem_Click);
            
            // 
            // btnCancelarVenda
            // 
            this.btnCancelarVenda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancelarVenda.FlatAppearance.BorderSize = 0;
            this.btnCancelarVenda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarVenda.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCancelarVenda.ForeColor = System.Drawing.Color.White;
            this.btnCancelarVenda.Location = new System.Drawing.Point(20, 320);
            this.btnCancelarVenda.Name = "btnCancelarVenda";
            this.btnCancelarVenda.Size = new System.Drawing.Size(340, 60);
            this.btnCancelarVenda.TabIndex = 4;
            this.btnCancelarVenda.Text = "🛑 CANCELAR VENDA (F6)";
            this.btnCancelarVenda.UseVisualStyleBackColor = false;
            this.btnCancelarVenda.Click += new System.EventHandler(this.btnCancelarVenda_Click);
            
            // 
            // btnFinalizarVenda
            // 
            this.btnFinalizarVenda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnFinalizarVenda.FlatAppearance.BorderSize = 0;
            this.btnFinalizarVenda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizarVenda.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnFinalizarVenda.ForeColor = System.Drawing.Color.White;
            this.btnFinalizarVenda.Location = new System.Drawing.Point(20, 390);
            this.btnFinalizarVenda.Name = "btnFinalizarVenda";
            this.btnFinalizarVenda.Size = new System.Drawing.Size(340, 60);
            this.btnFinalizarVenda.TabIndex = 5;
            this.btnFinalizarVenda.Text = "✔️ FINALIZAR VENDA (F5)";
            this.btnFinalizarVenda.UseVisualStyleBackColor = false;
            this.btnFinalizarVenda.Click += new System.EventHandler(this.btnFinalizarVenda_Click);
            
            // 
            // btnLimparCampos
            // 
            this.btnLimparCampos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnLimparCampos.FlatAppearance.BorderSize = 0;
            this.btnLimparCampos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimparCampos.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnLimparCampos.ForeColor = System.Drawing.Color.White;
            this.btnLimparCampos.Location = new System.Drawing.Point(20, 460);
            this.btnLimparCampos.Name = "btnLimparCampos";
            this.btnLimparCampos.Size = new System.Drawing.Size(340, 60);
            this.btnLimparCampos.TabIndex = 6;
            this.btnLimparCampos.Text = "🧹 LIMPAR CAMPOS";
            this.btnLimparCampos.UseVisualStyleBackColor = false;
            this.btnLimparCampos.Click += new System.EventHandler(this.btnLimparCampos_Click);
            
            // 
            // btnAjuda
            // 
            this.btnAjuda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAjuda.FlatAppearance.BorderSize = 0;
            this.btnAjuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjuda.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnAjuda.ForeColor = System.Drawing.Color.White;
            this.btnAjuda.Location = new System.Drawing.Point(20, 530);
            this.btnAjuda.Name = "btnAjuda";
            this.btnAjuda.Size = new System.Drawing.Size(340, 60);
            this.btnAjuda.TabIndex = 7;
            this.btnAjuda.Text = "❓ AJUDA";
            this.btnAjuda.UseVisualStyleBackColor = false;
            this.btnAjuda.Click += new System.EventHandler(this.btnAjuda_Click);
            
            // 
            // pnCenter
            // 
            this.pnCenter.BackColor = System.Drawing.Color.White;
            this.pnCenter.Controls.Add(this.gbCarrinho);
            this.pnCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnCenter.Location = new System.Drawing.Point(400, 90);
            this.pnCenter.Name = "pnCenter";
            this.pnCenter.Padding = new System.Windows.Forms.Padding(10);
            this.pnCenter.Size = new System.Drawing.Size(1120, 560);
            this.pnCenter.TabIndex = 4;
            
            // 
            // gbCarrinho
            // 
            this.gbCarrinho.Controls.Add(this.dgvCarrinho);
            this.gbCarrinho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCarrinho.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbCarrinho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.gbCarrinho.Location = new System.Drawing.Point(10, 10);
            this.gbCarrinho.Name = "gbCarrinho";
            this.gbCarrinho.Size = new System.Drawing.Size(1100, 540);
            this.gbCarrinho.TabIndex = 0;
            this.gbCarrinho.TabStop = false;
            this.gbCarrinho.Text = "🛒 CARRINHO DE COMPRAS";
            
            // 
            // dgvCarrinho
            // 
            this.dgvCarrinho.AllowUserToAddRows = false;
            this.dgvCarrinho.AllowUserToDeleteRows = false;
            this.dgvCarrinho.AllowUserToResizeRows = false;
            this.dgvCarrinho.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCarrinho.BackgroundColor = System.Drawing.Color.White;
            this.dgvCarrinho.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCarrinho.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCarrinho.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCarrinho.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCarrinho.ColumnHeadersHeight = 50;
            this.dgvCarrinho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCarrinho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCarrinho.EnableHeadersVisualStyles = false;
            this.dgvCarrinho.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.dgvCarrinho.Location = new System.Drawing.Point(3, 25);
            this.dgvCarrinho.MultiSelect = false;
            this.dgvCarrinho.Name = "dgvCarrinho";
            this.dgvCarrinho.ReadOnly = true;
            this.dgvCarrinho.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgvCarrinho.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCarrinho.RowTemplate.Height = 40;
            this.dgvCarrinho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarrinho.Size = new System.Drawing.Size(1094, 512);
            this.dgvCarrinho.TabIndex = 0;
            
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnBottom.Controls.Add(this.gbPagamento);
            this.pnBottom.Controls.Add(this.gbTotais);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(400, 650);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Padding = new System.Windows.Forms.Padding(10);
            this.pnBottom.Size = new System.Drawing.Size(1120, 140);
            this.pnBottom.TabIndex = 5;
            
            // 
            // gbTotais
            // 
            this.gbTotais.Controls.Add(this.lblSubTotalLabel);
            this.gbTotais.Controls.Add(this.lblSubTotal);
            this.gbTotais.Controls.Add(this.lblDescontoLabel);
            this.gbTotais.Controls.Add(this.lblDesconto);
            this.gbTotais.Controls.Add(this.lblTotalLabel);
            this.gbTotais.Controls.Add(this.lblTotal);
            this.gbTotais.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbTotais.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbTotais.ForeColor = System.Drawing.Color.White;
            this.gbTotais.Location = new System.Drawing.Point(10, 10);
            this.gbTotais.Name = "gbTotais";
            this.gbTotais.Size = new System.Drawing.Size(400, 120);
            this.gbTotais.TabIndex = 0;
            this.gbTotais.TabStop = false;
            this.gbTotais.Text = "💰 TOTAIS DA VENDA";
            
            // 
            // lblSubTotalLabel
            // 
            this.lblSubTotalLabel.AutoSize = true;
            this.lblSubTotalLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubTotalLabel.Location = new System.Drawing.Point(20, 30);
            this.lblSubTotalLabel.Name = "lblSubTotalLabel";
            this.lblSubTotalLabel.Size = new System.Drawing.Size(88, 21);
            this.lblSubTotalLabel.TabIndex = 0;
            this.lblSubTotalLabel.Text = "Sub-Total:";
            
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.lblSubTotal.ForeColor = System.Drawing.Color.LightGreen;
            this.lblSubTotal.Location = new System.Drawing.Point(120, 28);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(80, 22);
            this.lblSubTotal.TabIndex = 1;
            this.lblSubTotal.Text = "R$ 0,00";
            
            // 
            // lblDescontoLabel
            // 
            this.lblDescontoLabel.AutoSize = true;
            this.lblDescontoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDescontoLabel.Location = new System.Drawing.Point(20, 55);
            this.lblDescontoLabel.Name = "lblDescontoLabel";
            this.lblDescontoLabel.Size = new System.Drawing.Size(85, 21);
            this.lblDescontoLabel.TabIndex = 2;
            this.lblDescontoLabel.Text = "Desconto:";
            
            // 
            // lblDesconto
            // 
            this.lblDesconto.AutoSize = true;
            this.lblDesconto.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.lblDesconto.ForeColor = System.Drawing.Color.Orange;
            this.lblDesconto.Location = new System.Drawing.Point(120, 53);
            this.lblDesconto.Name = "lblDesconto";
            this.lblDesconto.Size = new System.Drawing.Size(80, 22);
            this.lblDesconto.TabIndex = 3;
            this.lblDesconto.Text = "R$ 0,00";
            
            // 
            // lblTotalLabel
            // 
            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.Location = new System.Drawing.Point(20, 80);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(60, 25);
            this.lblTotalLabel.TabIndex = 4;
            this.lblTotalLabel.Text = "TOTAL:";
            
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.Yellow;
            this.lblTotal.Location = new System.Drawing.Point(120, 78);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(116, 28);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "R$ 0,00";
            
            // 
            // gbPagamento
            // 
            this.gbPagamento.Controls.Add(this.lblFormaPagamentoLabel);
            this.gbPagamento.Controls.Add(this.lblFormaPagamento);
            this.gbPagamento.Controls.Add(this.lblValorRecebidoLabel);
            this.gbPagamento.Controls.Add(this.lblValorAReceber);
            this.gbPagamento.Controls.Add(this.lblTrocoLabel);
            this.gbPagamento.Controls.Add(this.lblTroco);
            this.gbPagamento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPagamento.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gbPagamento.ForeColor = System.Drawing.Color.White;
            this.gbPagamento.Location = new System.Drawing.Point(410, 10);
            this.gbPagamento.Name = "gbPagamento";
            this.gbPagamento.Size = new System.Drawing.Size(700, 120);
            this.gbPagamento.TabIndex = 1;
            this.gbPagamento.TabStop = false;
            this.gbPagamento.Text = "💳 INFORMAÇÕES DE PAGAMENTO";
            
            // 
            // lblFormaPagamentoLabel
            // 
            this.lblFormaPagamentoLabel.AutoSize = true;
            this.lblFormaPagamentoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFormaPagamentoLabel.Location = new System.Drawing.Point(20, 30);
            this.lblFormaPagamentoLabel.Name = "lblFormaPagamentoLabel";
            this.lblFormaPagamentoLabel.Size = new System.Drawing.Size(149, 21);
            this.lblFormaPagamentoLabel.TabIndex = 0;
            this.lblFormaPagamentoLabel.Text = "Forma Pagamento:";
            this.lblFormaPagamentoLabel.Visible = false;
            
            // 
            // lblFormaPagamento
            // 
            this.lblFormaPagamento.AutoSize = true;
            this.lblFormaPagamento.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.lblFormaPagamento.ForeColor = System.Drawing.Color.Cyan;
            this.lblFormaPagamento.Location = new System.Drawing.Point(180, 28);
            this.lblFormaPagamento.Name = "lblFormaPagamento";
            this.lblFormaPagamento.Size = new System.Drawing.Size(30, 22);
            this.lblFormaPagamento.TabIndex = 1;
            this.lblFormaPagamento.Text = "---";
            this.lblFormaPagamento.Visible = false;
            
            // 
            // lblValorRecebidoLabel
            // 
            this.lblValorRecebidoLabel.AutoSize = true;
            this.lblValorRecebidoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblValorRecebidoLabel.Location = new System.Drawing.Point(350, 30);
            this.lblValorRecebidoLabel.Name = "lblValorRecebidoLabel";
            this.lblValorRecebidoLabel.Size = new System.Drawing.Size(126, 21);
            this.lblValorRecebidoLabel.TabIndex = 2;
            this.lblValorRecebidoLabel.Text = "Valor Recebido:";
            this.lblValorRecebidoLabel.Visible = false;
            
            // 
            // lblValorAReceber
            // 
            this.lblValorAReceber.AutoSize = true;
            this.lblValorAReceber.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.lblValorAReceber.ForeColor = System.Drawing.Color.LightGreen;
            this.lblValorAReceber.Location = new System.Drawing.Point(490, 28);
            this.lblValorAReceber.Name = "lblValorAReceber";
            this.lblValorAReceber.Size = new System.Drawing.Size(80, 22);
            this.lblValorAReceber.TabIndex = 3;
            this.lblValorAReceber.Text = "R$ 0,00";
            this.lblValorAReceber.Visible = false;
            
            // 
            // lblTrocoLabel
            // 
            this.lblTrocoLabel.AutoSize = true;
            this.lblTrocoLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTrocoLabel.Location = new System.Drawing.Point(180, 60);
            this.lblTrocoLabel.Name = "lblTrocoLabel";
            this.lblTrocoLabel.Size = new System.Drawing.Size(65, 25);
            this.lblTrocoLabel.TabIndex = 4;
            this.lblTrocoLabel.Text = "TROCO:";
            this.lblTrocoLabel.Visible = false;
            
            // 
            // lblTroco
            // 
            this.lblTroco.AutoSize = true;
            this.lblTroco.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.lblTroco.ForeColor = System.Drawing.Color.Yellow;
            this.lblTroco.Location = new System.Drawing.Point(260, 58);
            this.lblTroco.Name = "lblTroco";
            this.lblTroco.Size = new System.Drawing.Size(116, 28);
            this.lblTroco.TabIndex = 5;
            this.lblTroco.Text = "R$ 0,00";
            this.lblTroco.Visible = false;
            
            // 
            // pnStatus
            // 
            this.pnStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnStatus.Controls.Add(this.lblStatusOperacao);
            this.pnStatus.Controls.Add(this.lblNomeCaixa);
            this.pnStatus.Controls.Add(this.progressOperacao);
            this.pnStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnStatus.Location = new System.Drawing.Point(0, 790);
            this.pnStatus.Name = "pnStatus";
            this.pnStatus.Size = new System.Drawing.Size(1920, 30);
            this.pnStatus.TabIndex = 6;
            
            // 
            // lblStatusOperacao
            // 
            this.lblStatusOperacao.AutoSize = true;
            this.lblStatusOperacao.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatusOperacao.ForeColor = System.Drawing.Color.White;
            this.lblStatusOperacao.Location = new System.Drawing.Point(20, 6);
            this.lblStatusOperacao.Name = "lblStatusOperacao";
            this.lblStatusOperacao.Size = new System.Drawing.Size(181, 19);
            this.lblStatusOperacao.TabIndex = 0;
            this.lblStatusOperacao.Text = "🟢 Sistema PDV - Pronto";
            
            // 
            // lblNomeCaixa
            // 
            this.lblNomeCaixa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNomeCaixa.AutoSize = true;
            this.lblNomeCaixa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNomeCaixa.ForeColor = System.Drawing.Color.LightGreen;
            this.lblNomeCaixa.Location = new System.Drawing.Point(860, 6);
            this.lblNomeCaixa.Name = "lblNomeCaixa";
            this.lblNomeCaixa.Size = new System.Drawing.Size(200, 19);
            this.lblNomeCaixa.TabIndex = 1;
            this.lblNomeCaixa.Text = "CAIXA LIVRE - Aguardando...";
            
            // 
            // progressOperacao
            // 
            this.progressOperacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressOperacao.Location = new System.Drawing.Point(1750, 8);
            this.progressOperacao.Name = "progressOperacao";
            this.progressOperacao.Size = new System.Drawing.Size(150, 15);
            this.progressOperacao.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressOperacao.TabIndex = 2;
            this.progressOperacao.Visible = false;
            
            // 
            // timerData
            // 
            this.timerData.Enabled = true;
            this.timerData.Interval = 1000;
            this.timerData.Tick += new System.EventHandler(this.timerData_Tick);
            
            // 
            // frmTelaPdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1920, 820);
            this.Controls.Add(this.pnCenter);
            this.Controls.Add(this.pnBottom);
            this.Controls.Add(this.pnControles);
            this.Controls.Add(this.pnLeftInput);
            this.Controls.Add(this.pnTopInfo);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.pnStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmTelaPdv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema PDV - Moderno";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTelaPdv_FormClosing);
            this.Load += new System.EventHandler(this.frmTelaPdv_Load);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnTopInfo.ResumeLayout(false);
            this.pnTopInfo.PerformLayout();
            this.pnLeftInput.ResumeLayout(false);
            this.gbProdutoInfo.ResumeLayout(false);
            this.gbProdutoInfo.PerformLayout();
            this.pnControles.ResumeLayout(false);
            this.gbAcoes.ResumeLayout(false);
            this.pnCenter.ResumeLayout(false);
            this.gbCarrinho.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrinho)).EndInit();
            this.pnBottom.ResumeLayout(false);
            this.gbTotais.ResumeLayout(false);
            this.gbTotais.PerformLayout();
            this.gbPagamento.ResumeLayout(false);
            this.gbPagamento.PerformLayout();
            this.pnStatus.ResumeLayout(false);
            this.pnStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // Header Components
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTituloPdv;
        private System.Windows.Forms.Label lblStatusCaixa;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnClose;
        
        // Top Info Components  
        private System.Windows.Forms.Panel pnTopInfo;
        private System.Windows.Forms.Label lblOperadorLabel;
        private System.Windows.Forms.Label lblNomeOperador;
        private System.Windows.Forms.Label lblCaixaLabel;
        private System.Windows.Forms.Label lblCaixa;
        private System.Windows.Forms.Label lblDataLabel;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblHoraLabel;
        private System.Windows.Forms.Label lblHora;
        
        // Left Input Panel Components
        private System.Windows.Forms.Panel pnLeftInput;
        private System.Windows.Forms.GroupBox gbProdutoInfo;
        private System.Windows.Forms.Label lblCodigoProduto;
        private System.Windows.Forms.TextBox txbCodBarras;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.TextBox txbDescricao;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.TextBox txbQuantidade;
        private System.Windows.Forms.Label lblPrecoUnit;
        private System.Windows.Forms.TextBox txbPrecoUnit;
        private System.Windows.Forms.Label lblTotalItem;
        private System.Windows.Forms.TextBox txbTotalRecebido;
        
        // Controls Panel Components
        private System.Windows.Forms.Panel pnControles;
        private System.Windows.Forms.GroupBox gbAcoes;
        private System.Windows.Forms.Button btnPagamentoDinheiro;
        private System.Windows.Forms.Button btnPagamentoCartao;
        private System.Windows.Forms.Button btnPagamentoPix;
        private System.Windows.Forms.Button btnCancelarItem;
        private System.Windows.Forms.Button btnCancelarVenda;
        private System.Windows.Forms.Button btnFinalizarVenda;
        private System.Windows.Forms.Button btnLimparCampos;
        private System.Windows.Forms.Button btnAjuda;
        
        // Center Panel Components
        private System.Windows.Forms.Panel pnCenter;
        private System.Windows.Forms.GroupBox gbCarrinho;
        private System.Windows.Forms.DataGridView dgvCarrinho;
        
        // Bottom Panel Components
        private System.Windows.Forms.Panel pnBottom;
        private System.Windows.Forms.GroupBox gbTotais;
        private System.Windows.Forms.Label lblSubTotalLabel;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblDescontoLabel;
        private System.Windows.Forms.Label lblDesconto;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.Label lblTotal;
        
        private System.Windows.Forms.GroupBox gbPagamento;
        private System.Windows.Forms.Label lblFormaPagamentoLabel;
        private System.Windows.Forms.Label lblFormaPagamento;
        private System.Windows.Forms.Label lblValorRecebidoLabel;
        private System.Windows.Forms.Label lblValorAReceber;
        private System.Windows.Forms.Label lblTrocoLabel;
        private System.Windows.Forms.Label lblTroco;
        
        // Status Panel Components
        private System.Windows.Forms.Panel pnStatus;
        private System.Windows.Forms.Label lblStatusOperacao;
        private System.Windows.Forms.Label lblNomeCaixa;
        private System.Windows.Forms.ProgressBar progressOperacao;
        
        // Timer
        private System.Windows.Forms.Timer timerData;
    }
}