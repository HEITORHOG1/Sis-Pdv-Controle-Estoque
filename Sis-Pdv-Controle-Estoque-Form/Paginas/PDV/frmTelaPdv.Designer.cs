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
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.lblValorUnitario = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblCodigoProduto = new System.Windows.Forms.Label();
            this.txbTotalRecebido = new System.Windows.Forms.TextBox();
            this.txbPrecoUnit = new System.Windows.Forms.TextBox();
            this.txbQuantidade = new System.Windows.Forms.TextBox();
            this.txbCodBarras = new System.Windows.Forms.TextBox();
            this.txbDescricao = new System.Windows.Forms.TextBox();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.pnInfos = new System.Windows.Forms.Panel();
            this.lblCaixa = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblNomeOperador = new System.Windows.Forms.Label();
            this.lblNomeHora = new System.Windows.Forms.Label();
            this.lblOperador = new System.Windows.Forms.Label();
            this.lblNomeData = new System.Windows.Forms.Label();
            this.lblPdvEcf = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblAtalhos = new System.Windows.Forms.Label();
            this.btnLimpaCampos = new System.Windows.Forms.Button();
            this.btnFinalizarVendas = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btnAjuda = new System.Windows.Forms.Button();
            this.pnTitulo = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblNomeCaixa = new System.Windows.Forms.Label();
            this.timerData = new System.Windows.Forms.Timer(this.components);
            this.dgvCarrinho = new System.Windows.Forms.DataGridView();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblNomeFormaPagamento = new System.Windows.Forms.Label();
            this.lblFormaPagamento = new System.Windows.Forms.Label();
            this.lblNomeTroco = new System.Windows.Forms.Label();
            this.lblTroco = new System.Windows.Forms.Label();
            this.lblNomeValorPago = new System.Windows.Forms.Label();
            this.lblValorAReceber = new System.Windows.Forms.Label();
            this.pnFormaPagamento = new System.Windows.Forms.Panel();
            this.pnInfos.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrinho)).BeginInit();
            this.pnFormaPagamento.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.AutoSize = true;
            this.lblValorTotal.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblValorTotal.Location = new System.Drawing.Point(1396, 444);
            this.lblValorTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(249, 32);
            this.lblValorTotal.TabIndex = 59;
            this.lblValorTotal.Text = "Valor Total(R$)";
            this.lblValorTotal.Visible = false;
            // 
            // lblValorUnitario
            // 
            this.lblValorUnitario.AutoSize = true;
            this.lblValorUnitario.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblValorUnitario.Location = new System.Drawing.Point(1394, 256);
            this.lblValorUnitario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorUnitario.Name = "lblValorUnitario";
            this.lblValorUnitario.Size = new System.Drawing.Size(291, 32);
            this.lblValorUnitario.TabIndex = 58;
            this.lblValorUnitario.Text = "Valor unitário(R$)";
            this.lblValorUnitario.Visible = false;
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblQuantidade.Location = new System.Drawing.Point(1394, 359);
            this.lblQuantidade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(188, 32);
            this.lblQuantidade.TabIndex = 57;
            this.lblQuantidade.Text = "Quantidade";
            this.lblQuantidade.Visible = false;
            // 
            // lblCodigoProduto
            // 
            this.lblCodigoProduto.AutoSize = true;
            this.lblCodigoProduto.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCodigoProduto.Location = new System.Drawing.Point(1394, 143);
            this.lblCodigoProduto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigoProduto.Name = "lblCodigoProduto";
            this.lblCodigoProduto.Size = new System.Drawing.Size(295, 32);
            this.lblCodigoProduto.TabIndex = 56;
            this.lblCodigoProduto.Text = "Código do produto";
            // 
            // txbTotalRecebido
            // 
            this.txbTotalRecebido.Enabled = false;
            this.txbTotalRecebido.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txbTotalRecebido.Location = new System.Drawing.Point(1404, 485);
            this.txbTotalRecebido.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbTotalRecebido.Multiline = true;
            this.txbTotalRecebido.Name = "txbTotalRecebido";
            this.txbTotalRecebido.Size = new System.Drawing.Size(251, 43);
            this.txbTotalRecebido.TabIndex = 54;
            this.txbTotalRecebido.Visible = false;
            // 
            // txbPrecoUnit
            // 
            this.txbPrecoUnit.Enabled = false;
            this.txbPrecoUnit.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txbPrecoUnit.Location = new System.Drawing.Point(1401, 306);
            this.txbPrecoUnit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbPrecoUnit.Name = "txbPrecoUnit";
            this.txbPrecoUnit.Size = new System.Drawing.Size(321, 39);
            this.txbPrecoUnit.TabIndex = 53;
            this.txbPrecoUnit.Visible = false;
            // 
            // txbQuantidade
            // 
            this.txbQuantidade.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txbQuantidade.Location = new System.Drawing.Point(1401, 399);
            this.txbQuantidade.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbQuantidade.Name = "txbQuantidade";
            this.txbQuantidade.Size = new System.Drawing.Size(251, 39);
            this.txbQuantidade.TabIndex = 52;
            this.txbQuantidade.Visible = false;
            this.txbQuantidade.TextChanged += new System.EventHandler(this.txbQuantidade_TextChanged);
            this.txbQuantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbQuantidade_KeyPress);
            // 
            // txbCodBarras
            // 
            this.txbCodBarras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCodBarras.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txbCodBarras.Location = new System.Drawing.Point(1404, 194);
            this.txbCodBarras.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbCodBarras.Name = "txbCodBarras";
            this.txbCodBarras.Size = new System.Drawing.Size(384, 39);
            this.txbCodBarras.TabIndex = 51;
            this.txbCodBarras.TextChanged += new System.EventHandler(this.txbCodBarras_TextChanged);
            this.txbCodBarras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbCodBarras_KeyPress);
            // 
            // txbDescricao
            // 
            this.txbDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDescricao.Enabled = false;
            this.txbDescricao.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txbDescricao.Location = new System.Drawing.Point(14, 85);
            this.txbDescricao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbDescricao.Multiline = true;
            this.txbDescricao.Name = "txbDescricao";
            this.txbDescricao.Size = new System.Drawing.Size(1765, 41);
            this.txbDescricao.TabIndex = 50;
            this.txbDescricao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDescricao.Location = new System.Drawing.Point(10, 50);
            this.lblDescricao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(193, 32);
            this.lblDescricao.TabIndex = 49;
            this.lblDescricao.Text = "DESCRIÇÃO";
            // 
            // pnInfos
            // 
            this.pnInfos.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnInfos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnInfos.Controls.Add(this.lblCaixa);
            this.pnInfos.Controls.Add(this.lblHora);
            this.pnInfos.Controls.Add(this.lblData);
            this.pnInfos.Controls.Add(this.lblNomeOperador);
            this.pnInfos.Controls.Add(this.lblNomeHora);
            this.pnInfos.Controls.Add(this.lblOperador);
            this.pnInfos.Controls.Add(this.lblNomeData);
            this.pnInfos.Controls.Add(this.lblPdvEcf);
            this.pnInfos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnInfos.Location = new System.Drawing.Point(0, 795);
            this.pnInfos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnInfos.Name = "pnInfos";
            this.pnInfos.Size = new System.Drawing.Size(1794, 46);
            this.pnInfos.TabIndex = 48;
            // 
            // lblCaixa
            // 
            this.lblCaixa.AutoSize = true;
            this.lblCaixa.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCaixa.Location = new System.Drawing.Point(166, 6);
            this.lblCaixa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCaixa.Name = "lblCaixa";
            this.lblCaixa.Size = new System.Drawing.Size(92, 25);
            this.lblCaixa.TabIndex = 71;
            this.lblCaixa.Text = "Codigo";
            // 
            // lblHora
            // 
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHora.Location = new System.Drawing.Point(1441, 6);
            this.lblHora.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(68, 25);
            this.lblHora.TabIndex = 6;
            this.lblHora.Text = "Hora";
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblData.Location = new System.Drawing.Point(1122, 6);
            this.lblData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(67, 25);
            this.lblData.TabIndex = 5;
            this.lblData.Text = "Data";
            // 
            // lblNomeOperador
            // 
            this.lblNomeOperador.AutoSize = true;
            this.lblNomeOperador.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeOperador.Location = new System.Drawing.Point(659, 6);
            this.lblNomeOperador.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeOperador.Name = "lblNomeOperador";
            this.lblNomeOperador.Size = new System.Drawing.Size(80, 25);
            this.lblNomeOperador.TabIndex = 4;
            this.lblNomeOperador.Text = "Nome";
            // 
            // lblNomeHora
            // 
            this.lblNomeHora.AutoSize = true;
            this.lblNomeHora.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeHora.Location = new System.Drawing.Point(1331, 6);
            this.lblNomeHora.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeHora.Name = "lblNomeHora";
            this.lblNomeHora.Size = new System.Drawing.Size(88, 25);
            this.lblNomeHora.TabIndex = 3;
            this.lblNomeHora.Text = "HORA:";
            // 
            // lblOperador
            // 
            this.lblOperador.AutoSize = true;
            this.lblOperador.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblOperador.Location = new System.Drawing.Point(477, 6);
            this.lblOperador.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOperador.Name = "lblOperador";
            this.lblOperador.Size = new System.Drawing.Size(150, 25);
            this.lblOperador.TabIndex = 2;
            this.lblOperador.Text = "OPERADOR:";
            // 
            // lblNomeData
            // 
            this.lblNomeData.AutoSize = true;
            this.lblNomeData.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeData.Location = new System.Drawing.Point(1027, 6);
            this.lblNomeData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeData.Name = "lblNomeData";
            this.lblNomeData.Size = new System.Drawing.Size(83, 25);
            this.lblNomeData.TabIndex = 1;
            this.lblNomeData.Text = "DATA:";
            // 
            // lblPdvEcf
            // 
            this.lblPdvEcf.AutoSize = true;
            this.lblPdvEcf.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPdvEcf.Location = new System.Drawing.Point(13, 6);
            this.lblPdvEcf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPdvEcf.Name = "lblPdvEcf";
            this.lblPdvEcf.Size = new System.Drawing.Size(125, 25);
            this.lblPdvEcf.TabIndex = 0;
            this.lblPdvEcf.Text = "PDV/ECF:";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.lblAtalhos);
            this.panel3.Controls.Add(this.btnLimpaCampos);
            this.panel3.Controls.Add(this.btnFinalizarVendas);
            this.panel3.Controls.Add(this.btnAdicionar);
            this.panel3.Controls.Add(this.btnAjuda);
            this.panel3.Location = new System.Drawing.Point(1455, 582);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(248, 165);
            this.panel3.TabIndex = 47;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Info;
            this.button2.Location = new System.Drawing.Point(124, 119);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 40);
            this.button2.TabIndex = 70;
            this.button2.Text = "Forma Pgmt.(D,C)";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Info;
            this.button1.Location = new System.Drawing.Point(4, 119);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 40);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancelar Venda(F8)";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // lblAtalhos
            // 
            this.lblAtalhos.AutoSize = true;
            this.lblAtalhos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAtalhos.Location = new System.Drawing.Point(4, 3);
            this.lblAtalhos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAtalhos.Name = "lblAtalhos";
            this.lblAtalhos.Size = new System.Drawing.Size(53, 13);
            this.lblAtalhos.TabIndex = 6;
            this.lblAtalhos.Text = "Atalhos:";
            // 
            // btnLimpaCampos
            // 
            this.btnLimpaCampos.BackColor = System.Drawing.SystemColors.Info;
            this.btnLimpaCampos.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimpaCampos.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLimpaCampos.Location = new System.Drawing.Point(4, 70);
            this.btnLimpaCampos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLimpaCampos.Name = "btnLimpaCampos";
            this.btnLimpaCampos.Size = new System.Drawing.Size(117, 42);
            this.btnLimpaCampos.TabIndex = 4;
            this.btnLimpaCampos.Text = "Limpa campos (F5)";
            this.btnLimpaCampos.UseVisualStyleBackColor = false;
            // 
            // btnFinalizarVendas
            // 
            this.btnFinalizarVendas.BackColor = System.Drawing.SystemColors.Info;
            this.btnFinalizarVendas.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFinalizarVendas.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFinalizarVendas.Location = new System.Drawing.Point(124, 22);
            this.btnFinalizarVendas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFinalizarVendas.Name = "btnFinalizarVendas";
            this.btnFinalizarVendas.Size = new System.Drawing.Size(117, 42);
            this.btnFinalizarVendas.TabIndex = 2;
            this.btnFinalizarVendas.Text = "Finalizar Venda (F2)";
            this.btnFinalizarVendas.UseVisualStyleBackColor = false;
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.SystemColors.Info;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdicionar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAdicionar.Location = new System.Drawing.Point(124, 70);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(117, 42);
            this.btnAdicionar.TabIndex = 1;
            this.btnAdicionar.Text = "Adicionar (A)";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Visible = false;
            // 
            // btnAjuda
            // 
            this.btnAjuda.BackColor = System.Drawing.SystemColors.Info;
            this.btnAjuda.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAjuda.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAjuda.Location = new System.Drawing.Point(4, 22);
            this.btnAjuda.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAjuda.Name = "btnAjuda";
            this.btnAjuda.Size = new System.Drawing.Size(117, 42);
            this.btnAjuda.TabIndex = 0;
            this.btnAjuda.Text = "Cancelar item(I)";
            this.btnAjuda.UseVisualStyleBackColor = false;
            // 
            // pnTitulo
            // 
            this.pnTitulo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnTitulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnTitulo.Controls.Add(this.btnClose);
            this.pnTitulo.Controls.Add(this.lblNomeCaixa);
            this.pnTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnTitulo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnTitulo.Name = "pnTitulo";
            this.pnTitulo.Size = new System.Drawing.Size(1794, 46);
            this.pnTitulo.TabIndex = 43;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1751, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(28, 27);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblNomeCaixa
            // 
            this.lblNomeCaixa.AutoSize = true;
            this.lblNomeCaixa.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNomeCaixa.Location = new System.Drawing.Point(727, 0);
            this.lblNomeCaixa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeCaixa.Name = "lblNomeCaixa";
            this.lblNomeCaixa.Size = new System.Drawing.Size(189, 32);
            this.lblNomeCaixa.TabIndex = 0;
            this.lblNomeCaixa.Text = "CAIXA LIVRE";
            // 
            // timerData
            // 
            this.timerData.Tick += new System.EventHandler(this.timerData_Tick);
            // 
            // dgvCarrinho
            // 
            this.dgvCarrinho.AllowUserToAddRows = false;
            this.dgvCarrinho.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCarrinho.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCarrinho.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCarrinho.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvCarrinho.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCarrinho.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCarrinho.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCarrinho.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCarrinho.ColumnHeadersHeight = 30;
            this.dgvCarrinho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCarrinho.EnableHeadersVisualStyles = false;
            this.dgvCarrinho.GridColor = System.Drawing.Color.SandyBrown;
            this.dgvCarrinho.Location = new System.Drawing.Point(14, 143);
            this.dgvCarrinho.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvCarrinho.Name = "dgvCarrinho";
            this.dgvCarrinho.ReadOnly = true;
            this.dgvCarrinho.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCarrinho.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCarrinho.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dgvCarrinho.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCarrinho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarrinho.Size = new System.Drawing.Size(1220, 592);
            this.dgvCarrinho.TabIndex = 60;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubTotal.Location = new System.Drawing.Point(-2, 5);
            this.lblSubTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(184, 32);
            this.lblSubTotal.TabIndex = 61;
            this.lblSubTotal.Text = "SUBTOTAL:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotal.Location = new System.Drawing.Point(211, 7);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(33, 32);
            this.lblTotal.TabIndex = 62;
            this.lblTotal.Text = "0";
            // 
            // lblNomeFormaPagamento
            // 
            this.lblNomeFormaPagamento.AutoSize = true;
            this.lblNomeFormaPagamento.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeFormaPagamento.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblNomeFormaPagamento.Location = new System.Drawing.Point(362, 6);
            this.lblNomeFormaPagamento.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeFormaPagamento.Name = "lblNomeFormaPagamento";
            this.lblNomeFormaPagamento.Size = new System.Drawing.Size(268, 29);
            this.lblNomeFormaPagamento.TabIndex = 63;
            this.lblNomeFormaPagamento.Text = "Forma Pagamento:";
            this.lblNomeFormaPagamento.Visible = false;
            // 
            // lblFormaPagamento
            // 
            this.lblFormaPagamento.AutoSize = true;
            this.lblFormaPagamento.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFormaPagamento.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblFormaPagamento.Location = new System.Drawing.Point(673, 7);
            this.lblFormaPagamento.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFormaPagamento.Name = "lblFormaPagamento";
            this.lblFormaPagamento.Size = new System.Drawing.Size(30, 29);
            this.lblFormaPagamento.TabIndex = 64;
            this.lblFormaPagamento.Text = "0";
            this.lblFormaPagamento.Visible = false;
            // 
            // lblNomeTroco
            // 
            this.lblNomeTroco.AutoSize = true;
            this.lblNomeTroco.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeTroco.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblNomeTroco.Location = new System.Drawing.Point(1269, 7);
            this.lblNomeTroco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeTroco.Name = "lblNomeTroco";
            this.lblNomeTroco.Size = new System.Drawing.Size(97, 29);
            this.lblNomeTroco.TabIndex = 65;
            this.lblNomeTroco.Text = "Troco:";
            this.lblNomeTroco.Visible = false;
            // 
            // lblTroco
            // 
            this.lblTroco.AutoSize = true;
            this.lblTroco.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTroco.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblTroco.Location = new System.Drawing.Point(1394, 7);
            this.lblTroco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTroco.Name = "lblTroco";
            this.lblTroco.Size = new System.Drawing.Size(30, 29);
            this.lblTroco.TabIndex = 66;
            this.lblTroco.Text = "0";
            this.lblTroco.Visible = false;
            // 
            // lblNomeValorPago
            // 
            this.lblNomeValorPago.AutoSize = true;
            this.lblNomeValorPago.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNomeValorPago.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblNomeValorPago.Location = new System.Drawing.Point(847, 6);
            this.lblNomeValorPago.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomeValorPago.Name = "lblNomeValorPago";
            this.lblNomeValorPago.Size = new System.Drawing.Size(217, 29);
            this.lblNomeValorPago.TabIndex = 67;
            this.lblNomeValorPago.Text = "Valor recebido:";
            this.lblNomeValorPago.Visible = false;
            // 
            // lblValorAReceber
            // 
            this.lblValorAReceber.AutoSize = true;
            this.lblValorAReceber.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblValorAReceber.ForeColor = System.Drawing.Color.Aquamarine;
            this.lblValorAReceber.Location = new System.Drawing.Point(1107, 7);
            this.lblValorAReceber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorAReceber.Name = "lblValorAReceber";
            this.lblValorAReceber.Size = new System.Drawing.Size(30, 29);
            this.lblValorAReceber.TabIndex = 68;
            this.lblValorAReceber.Text = "0";
            this.lblValorAReceber.Visible = false;
            // 
            // pnFormaPagamento
            // 
            this.pnFormaPagamento.BackColor = System.Drawing.Color.Transparent;
            this.pnFormaPagamento.Controls.Add(this.lblNomeFormaPagamento);
            this.pnFormaPagamento.Controls.Add(this.lblValorAReceber);
            this.pnFormaPagamento.Controls.Add(this.lblSubTotal);
            this.pnFormaPagamento.Controls.Add(this.lblNomeValorPago);
            this.pnFormaPagamento.Controls.Add(this.lblTotal);
            this.pnFormaPagamento.Controls.Add(this.lblTroco);
            this.pnFormaPagamento.Controls.Add(this.lblFormaPagamento);
            this.pnFormaPagamento.Controls.Add(this.lblNomeTroco);
            this.pnFormaPagamento.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnFormaPagamento.Location = new System.Drawing.Point(0, 753);
            this.pnFormaPagamento.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnFormaPagamento.Name = "pnFormaPagamento";
            this.pnFormaPagamento.Size = new System.Drawing.Size(1794, 42);
            this.pnFormaPagamento.TabIndex = 69;
            // 
            // frmTelaPdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1794, 841);
            this.Controls.Add(this.pnFormaPagamento);
            this.Controls.Add(this.dgvCarrinho);
            this.Controls.Add(this.lblValorTotal);
            this.Controls.Add(this.lblValorUnitario);
            this.Controls.Add(this.lblQuantidade);
            this.Controls.Add(this.lblCodigoProduto);
            this.Controls.Add(this.txbTotalRecebido);
            this.Controls.Add(this.txbPrecoUnit);
            this.Controls.Add(this.txbQuantidade);
            this.Controls.Add(this.txbCodBarras);
            this.Controls.Add(this.txbDescricao);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.pnInfos);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnTitulo);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmTelaPdv";
            this.Text = "frmTelaPdv";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTelaPdv_Load);
            this.pnInfos.ResumeLayout(false);
            this.pnInfos.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnTitulo.ResumeLayout(false);
            this.pnTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrinho)).EndInit();
            this.pnFormaPagamento.ResumeLayout(false);
            this.pnFormaPagamento.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.Label lblValorUnitario;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.Label lblCodigoProduto;
        private System.Windows.Forms.TextBox txbTotalRecebido;
        private System.Windows.Forms.TextBox txbPrecoUnit;
        private System.Windows.Forms.TextBox txbQuantidade;
        private System.Windows.Forms.TextBox txbCodBarras;
        private System.Windows.Forms.TextBox txbDescricao;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Panel pnInfos;
        private System.Windows.Forms.Label lblNomeHora;
        private System.Windows.Forms.Label lblOperador;
        private System.Windows.Forms.Label lblNomeData;
        private System.Windows.Forms.Label lblPdvEcf;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblAtalhos;
        private System.Windows.Forms.Button btnLimpaCampos;
        private System.Windows.Forms.Button btnFinalizarVendas;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Button btnAjuda;
        private System.Windows.Forms.Panel pnTitulo;
        private System.Windows.Forms.Label lblNomeCaixa;
        private System.Windows.Forms.Label lblNomeOperador;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Timer timerData;
        private System.Windows.Forms.DataGridView dgvCarrinho;
        private System.Windows.Forms.Label lblCaixa;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblNomeFormaPagamento;
        private System.Windows.Forms.Label lblFormaPagamento;
        private System.Windows.Forms.Label lblNomeTroco;
        private System.Windows.Forms.Label lblTroco;
        private System.Windows.Forms.Label lblNomeValorPago;
        private System.Windows.Forms.Label lblValorAReceber;
        private System.Windows.Forms.Panel pnFormaPagamento;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnClose;
    }
}