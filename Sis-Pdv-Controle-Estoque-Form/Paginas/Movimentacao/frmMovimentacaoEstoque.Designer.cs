namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Movimentacao
{
    partial class frmMovimentacaoEstoque
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
            this.tcMovimentacao = new System.Windows.Forms.TabControl();
            this.tpNovaMovimentacao = new System.Windows.Forms.TabPage();
            this.gbMovimentacao = new System.Windows.Forms.GroupBox();
            this.lblTipoMovimentacao = new System.Windows.Forms.Label();
            this.cmbTipoMovimentacao = new System.Windows.Forms.ComboBox();
            this.lblProduto = new System.Windows.Forms.Label();
            this.cmbProduto = new System.Windows.Forms.ComboBox();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.nudQuantidade = new System.Windows.Forms.NumericUpDown();
            this.lblObservacoes = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.gbDadosComplementares = new System.Windows.Forms.GroupBox();
            this.lblLote = new System.Windows.Forms.Label();
            this.txtLote = new System.Windows.Forms.TextBox();
            this.lblDataVencimento = new System.Windows.Forms.Label();
            this.dtpDataVencimento = new System.Windows.Forms.DateTimePicker();
            this.lblPrecoCompra = new System.Windows.Forms.Label();
            this.nudPrecoCompra = new System.Windows.Forms.NumericUpDown();
            this.lblFornecedor = new System.Windows.Forms.Label();
            this.cmbFornecedor = new System.Windows.Forms.ComboBox();
            this.lblNotaFiscal = new System.Windows.Forms.Label();
            this.txtNotaFiscal = new System.Windows.Forms.TextBox();
            this.lblDataOperacao = new System.Windows.Forms.Label();
            this.dtpDataOperacao = new System.Windows.Forms.DateTimePicker();
            this.gbValidacao = new System.Windows.Forms.GroupBox();
            this.lblEstoqueAtual = new System.Windows.Forms.Label();
            this.lblEstoqueAtualValor = new System.Windows.Forms.Label();
            this.lblNovoEstoque = new System.Windows.Forms.Label();
            this.lblNovoEstoqueValor = new System.Windows.Forms.Label();
            this.pnlBotoes = new System.Windows.Forms.Panel();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tpHistoricoMovimentacoes = new System.Windows.Forms.TabPage();
            this.gbFiltros = new System.Windows.Forms.GroupBox();
            this.lblFiltroDataInicio = new System.Windows.Forms.Label();
            this.dtpFiltroDataInicio = new System.Windows.Forms.DateTimePicker();
            this.lblFiltroDataFim = new System.Windows.Forms.Label();
            this.dtpFiltroDataFim = new System.Windows.Forms.DateTimePicker();
            this.lblFiltroProduto = new System.Windows.Forms.Label();
            this.cmbFiltroProduto = new System.Windows.Forms.ComboBox();
            this.lblFiltroTipoMovimentacao = new System.Windows.Forms.Label();
            this.cmbFiltroTipoMovimentacao = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnLimparFiltros = new System.Windows.Forms.Button();
            this.dgvMovimentacoes = new System.Windows.Forms.DataGridView();
            this.pnlPaginacao = new System.Windows.Forms.Panel();
            this.lblPaginaAtual = new System.Windows.Forms.Label();
            this.btnPaginaAnterior = new System.Windows.Forms.Button();
            this.btnProximaPagina = new System.Windows.Forms.Button();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.tpAlertasEstoque = new System.Windows.Forms.TabPage();
            this.dgvAlertas = new System.Windows.Forms.DataGridView();
            this.pnlAlertasBotoes = new System.Windows.Forms.Panel();
            this.btnAtualizarAlertas = new System.Windows.Forms.Button();
            this.tcMovimentacao.SuspendLayout();
            this.tpNovaMovimentacao.SuspendLayout();
            this.gbMovimentacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).BeginInit();
            this.gbDadosComplementares.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecoCompra)).BeginInit();
            this.gbValidacao.SuspendLayout();
            this.pnlBotoes.SuspendLayout();
            this.tpHistoricoMovimentacoes.SuspendLayout();
            this.gbFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimentacoes)).BeginInit();
            this.pnlPaginacao.SuspendLayout();
            this.tpAlertasEstoque.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlertas)).BeginInit();
            this.pnlAlertasBotoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMovimentacao
            // 
            this.tcMovimentacao.Controls.Add(this.tpNovaMovimentacao);
            this.tcMovimentacao.Controls.Add(this.tpHistoricoMovimentacoes);
            this.tcMovimentacao.Controls.Add(this.tpAlertasEstoque);
            this.tcMovimentacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMovimentacao.Location = new System.Drawing.Point(0, 0);
            this.tcMovimentacao.Name = "tcMovimentacao";
            this.tcMovimentacao.SelectedIndex = 0;
            this.tcMovimentacao.Size = new System.Drawing.Size(800, 600);
            this.tcMovimentacao.TabIndex = 0;
            // 
            // tpNovaMovimentacao
            // 
            this.tpNovaMovimentacao.Controls.Add(this.gbMovimentacao);
            this.tpNovaMovimentacao.Controls.Add(this.gbDadosComplementares);
            this.tpNovaMovimentacao.Controls.Add(this.gbValidacao);
            this.tpNovaMovimentacao.Controls.Add(this.pnlBotoes);
            this.tpNovaMovimentacao.Location = new System.Drawing.Point(4, 22);
            this.tpNovaMovimentacao.Name = "tpNovaMovimentacao";
            this.tpNovaMovimentacao.Padding = new System.Windows.Forms.Padding(3);
            this.tpNovaMovimentacao.Size = new System.Drawing.Size(792, 574);
            this.tpNovaMovimentacao.TabIndex = 0;
            this.tpNovaMovimentacao.Text = "Nova Movimentação";
            this.tpNovaMovimentacao.UseVisualStyleBackColor = true;
            // 
            // gbMovimentacao
            // 
            this.gbMovimentacao.Controls.Add(this.lblTipoMovimentacao);
            this.gbMovimentacao.Controls.Add(this.cmbTipoMovimentacao);
            this.gbMovimentacao.Controls.Add(this.lblProduto);
            this.gbMovimentacao.Controls.Add(this.cmbProduto);
            this.gbMovimentacao.Controls.Add(this.lblQuantidade);
            this.gbMovimentacao.Controls.Add(this.nudQuantidade);
            this.gbMovimentacao.Controls.Add(this.lblObservacoes);
            this.gbMovimentacao.Controls.Add(this.txtObservacoes);
            this.gbMovimentacao.Location = new System.Drawing.Point(8, 6);
            this.gbMovimentacao.Name = "gbMovimentacao";
            this.gbMovimentacao.Size = new System.Drawing.Size(380, 200);
            this.gbMovimentacao.TabIndex = 0;
            this.gbMovimentacao.TabStop = false;
            this.gbMovimentacao.Text = "Dados da Movimentação";
            // 
            // lblTipoMovimentacao
            // 
            this.lblTipoMovimentacao.AutoSize = true;
            this.lblTipoMovimentacao.Location = new System.Drawing.Point(15, 25);
            this.lblTipoMovimentacao.Name = "lblTipoMovimentacao";
            this.lblTipoMovimentacao.Size = new System.Drawing.Size(31, 13);
            this.lblTipoMovimentacao.TabIndex = 0;
            this.lblTipoMovimentacao.Text = "Tipo:";
            // 
            // cmbTipoMovimentacao
            // 
            this.cmbTipoMovimentacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoMovimentacao.FormattingEnabled = true;
            this.cmbTipoMovimentacao.Location = new System.Drawing.Point(15, 41);
            this.cmbTipoMovimentacao.Name = "cmbTipoMovimentacao";
            this.cmbTipoMovimentacao.Size = new System.Drawing.Size(150, 21);
            this.cmbTipoMovimentacao.TabIndex = 1;
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Location = new System.Drawing.Point(180, 25);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(47, 13);
            this.lblProduto.TabIndex = 2;
            this.lblProduto.Text = "Produto:";
            // 
            // cmbProduto
            // 
            this.cmbProduto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduto.FormattingEnabled = true;
            this.cmbProduto.Location = new System.Drawing.Point(180, 41);
            this.cmbProduto.Name = "cmbProduto";
            this.cmbProduto.Size = new System.Drawing.Size(180, 21);
            this.cmbProduto.TabIndex = 3;
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(15, 75);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(65, 13);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "Quantidade:";
            // 
            // nudQuantidade
            // 
            this.nudQuantidade.DecimalPlaces = 2;
            this.nudQuantidade.Location = new System.Drawing.Point(15, 91);
            this.nudQuantidade.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudQuantidade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudQuantidade.Name = "nudQuantidade";
            this.nudQuantidade.Size = new System.Drawing.Size(120, 20);
            this.nudQuantidade.TabIndex = 5;
            this.nudQuantidade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.AutoSize = true;
            this.lblObservacoes.Location = new System.Drawing.Point(15, 125);
            this.lblObservacoes.Name = "lblObservacoes";
            this.lblObservacoes.Size = new System.Drawing.Size(73, 13);
            this.lblObservacoes.TabIndex = 6;
            this.lblObservacoes.Text = "Observações:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.Location = new System.Drawing.Point(15, 141);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(345, 50);
            this.txtObservacoes.TabIndex = 7;
            // 
            // gbDadosComplementares
            // 
            this.gbDadosComplementares.Controls.Add(this.lblLote);
            this.gbDadosComplementares.Controls.Add(this.txtLote);
            this.gbDadosComplementares.Controls.Add(this.lblDataVencimento);
            this.gbDadosComplementares.Controls.Add(this.dtpDataVencimento);
            this.gbDadosComplementares.Controls.Add(this.lblPrecoCompra);
            this.gbDadosComplementares.Controls.Add(this.nudPrecoCompra);
            this.gbDadosComplementares.Controls.Add(this.lblFornecedor);
            this.gbDadosComplementares.Controls.Add(this.cmbFornecedor);
            this.gbDadosComplementares.Controls.Add(this.lblNotaFiscal);
            this.gbDadosComplementares.Controls.Add(this.txtNotaFiscal);
            this.gbDadosComplementares.Controls.Add(this.lblDataOperacao);
            this.gbDadosComplementares.Controls.Add(this.dtpDataOperacao);
            this.gbDadosComplementares.Location = new System.Drawing.Point(394, 6);
            this.gbDadosComplementares.Name = "gbDadosComplementares";
            this.gbDadosComplementares.Size = new System.Drawing.Size(390, 280);
            this.gbDadosComplementares.TabIndex = 1;
            this.gbDadosComplementares.TabStop = false;
            this.gbDadosComplementares.Text = "Dados Complementares";
            // 
            // lblLote
            // 
            this.lblLote.AutoSize = true;
            this.lblLote.Location = new System.Drawing.Point(15, 25);
            this.lblLote.Name = "lblLote";
            this.lblLote.Size = new System.Drawing.Size(31, 13);
            this.lblLote.TabIndex = 0;
            this.lblLote.Text = "Lote:";
            // 
            // txtLote
            // 
            this.txtLote.Location = new System.Drawing.Point(15, 41);
            this.txtLote.Name = "txtLote";
            this.txtLote.Size = new System.Drawing.Size(120, 20);
            this.txtLote.TabIndex = 1;
            // 
            // lblDataVencimento
            // 
            this.lblDataVencimento.AutoSize = true;
            this.lblDataVencimento.Location = new System.Drawing.Point(150, 25);
            this.lblDataVencimento.Name = "lblDataVencimento";
            this.lblDataVencimento.Size = new System.Drawing.Size(63, 13);
            this.lblDataVencimento.TabIndex = 2;
            this.lblDataVencimento.Text = "Vencimento:";
            // 
            // dtpDataVencimento
            // 
            this.dtpDataVencimento.Location = new System.Drawing.Point(150, 41);
            this.dtpDataVencimento.Name = "dtpDataVencimento";
            this.dtpDataVencimento.Size = new System.Drawing.Size(200, 20);
            this.dtpDataVencimento.TabIndex = 3;
            // 
            // lblPrecoCompra
            // 
            this.lblPrecoCompra.AutoSize = true;
            this.lblPrecoCompra.Location = new System.Drawing.Point(15, 75);
            this.lblPrecoCompra.Name = "lblPrecoCompra";
            this.lblPrecoCompra.Size = new System.Drawing.Size(79, 13);
            this.lblPrecoCompra.TabIndex = 4;
            this.lblPrecoCompra.Text = "Preço Compra:";
            // 
            // nudPrecoCompra
            // 
            this.nudPrecoCompra.DecimalPlaces = 2;
            this.nudPrecoCompra.Location = new System.Drawing.Point(15, 91);
            this.nudPrecoCompra.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudPrecoCompra.Name = "nudPrecoCompra";
            this.nudPrecoCompra.Size = new System.Drawing.Size(120, 20);
            this.nudPrecoCompra.TabIndex = 5;
            // 
            // lblFornecedor
            // 
            this.lblFornecedor.AutoSize = true;
            this.lblFornecedor.Location = new System.Drawing.Point(150, 75);
            this.lblFornecedor.Name = "lblFornecedor";
            this.lblFornecedor.Size = new System.Drawing.Size(64, 13);
            this.lblFornecedor.TabIndex = 6;
            this.lblFornecedor.Text = "Fornecedor:";
            // 
            // cmbFornecedor
            // 
            this.cmbFornecedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFornecedor.FormattingEnabled = true;
            this.cmbFornecedor.Location = new System.Drawing.Point(150, 91);
            this.cmbFornecedor.Name = "cmbFornecedor";
            this.cmbFornecedor.Size = new System.Drawing.Size(200, 21);
            this.cmbFornecedor.TabIndex = 7;
            // 
            // lblNotaFiscal
            // 
            this.lblNotaFiscal.AutoSize = true;
            this.lblNotaFiscal.Location = new System.Drawing.Point(15, 125);
            this.lblNotaFiscal.Name = "lblNotaFiscal";
            this.lblNotaFiscal.Size = new System.Drawing.Size(64, 13);
            this.lblNotaFiscal.TabIndex = 8;
            this.lblNotaFiscal.Text = "Nota Fiscal:";
            // 
            // txtNotaFiscal
            // 
            this.txtNotaFiscal.Location = new System.Drawing.Point(15, 141);
            this.txtNotaFiscal.Name = "txtNotaFiscal";
            this.txtNotaFiscal.Size = new System.Drawing.Size(150, 20);
            this.txtNotaFiscal.TabIndex = 9;
            // 
            // lblDataOperacao
            // 
            this.lblDataOperacao.AutoSize = true;
            this.lblDataOperacao.Location = new System.Drawing.Point(15, 175);
            this.lblDataOperacao.Name = "lblDataOperacao";
            this.lblDataOperacao.Size = new System.Drawing.Size(82, 13);
            this.lblDataOperacao.TabIndex = 10;
            this.lblDataOperacao.Text = "Data Operação:";
            // 
            // dtpDataOperacao
            // 
            this.dtpDataOperacao.Location = new System.Drawing.Point(15, 191);
            this.dtpDataOperacao.Name = "dtpDataOperacao";
            this.dtpDataOperacao.Size = new System.Drawing.Size(200, 20);
            this.dtpDataOperacao.TabIndex = 11;
            // 
            // gbValidacao
            // 
            this.gbValidacao.Controls.Add(this.lblEstoqueAtual);
            this.gbValidacao.Controls.Add(this.lblEstoqueAtualValor);
            this.gbValidacao.Controls.Add(this.lblNovoEstoque);
            this.gbValidacao.Controls.Add(this.lblNovoEstoqueValor);
            this.gbValidacao.Location = new System.Drawing.Point(8, 212);
            this.gbValidacao.Name = "gbValidacao";
            this.gbValidacao.Size = new System.Drawing.Size(380, 100);
            this.gbValidacao.TabIndex = 2;
            this.gbValidacao.TabStop = false;
            this.gbValidacao.Text = "Validação de Estoque";
            // 
            // lblEstoqueAtual
            // 
            this.lblEstoqueAtual.AutoSize = true;
            this.lblEstoqueAtual.Location = new System.Drawing.Point(15, 25);
            this.lblEstoqueAtual.Name = "lblEstoqueAtual";
            this.lblEstoqueAtual.Size = new System.Drawing.Size(77, 13);
            this.lblEstoqueAtual.TabIndex = 0;
            this.lblEstoqueAtual.Text = "Estoque Atual:";
            // 
            // lblEstoqueAtualValor
            // 
            this.lblEstoqueAtualValor.AutoSize = true;
            this.lblEstoqueAtualValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstoqueAtualValor.Location = new System.Drawing.Point(100, 25);
            this.lblEstoqueAtualValor.Name = "lblEstoqueAtualValor";
            this.lblEstoqueAtualValor.Size = new System.Drawing.Size(29, 13);
            this.lblEstoqueAtualValor.TabIndex = 1;
            this.lblEstoqueAtualValor.Text = "0,00";
            // 
            // lblNovoEstoque
            // 
            this.lblNovoEstoque.AutoSize = true;
            this.lblNovoEstoque.Location = new System.Drawing.Point(15, 50);
            this.lblNovoEstoque.Name = "lblNovoEstoque";
            this.lblNovoEstoque.Size = new System.Drawing.Size(80, 13);
            this.lblNovoEstoque.TabIndex = 2;
            this.lblNovoEstoque.Text = "Novo Estoque:";
            // 
            // lblNovoEstoqueValor
            // 
            this.lblNovoEstoqueValor.AutoSize = true;
            this.lblNovoEstoqueValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNovoEstoqueValor.Location = new System.Drawing.Point(100, 50);
            this.lblNovoEstoqueValor.Name = "lblNovoEstoqueValor";
            this.lblNovoEstoqueValor.Size = new System.Drawing.Size(29, 13);
            this.lblNovoEstoqueValor.TabIndex = 3;
            this.lblNovoEstoqueValor.Text = "0,00";
            // 
            // pnlBotoes
            // 
            this.pnlBotoes.Controls.Add(this.btnSalvar);
            this.pnlBotoes.Controls.Add(this.btnLimpar);
            this.pnlBotoes.Controls.Add(this.btnCancelar);
            this.pnlBotoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotoes.Location = new System.Drawing.Point(3, 531);
            this.pnlBotoes.Name = "pnlBotoes";
            this.pnlBotoes.Size = new System.Drawing.Size(786, 40);
            this.pnlBotoes.TabIndex = 3;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(538, 8);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(619, 8);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpar.TabIndex = 1;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(700, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // tpHistoricoMovimentacoes
            // 
            this.tpHistoricoMovimentacoes.Controls.Add(this.gbFiltros);
            this.tpHistoricoMovimentacoes.Controls.Add(this.dgvMovimentacoes);
            this.tpHistoricoMovimentacoes.Controls.Add(this.pnlPaginacao);
            this.tpHistoricoMovimentacoes.Location = new System.Drawing.Point(4, 22);
            this.tpHistoricoMovimentacoes.Name = "tpHistoricoMovimentacoes";
            this.tpHistoricoMovimentacoes.Padding = new System.Windows.Forms.Padding(3);
            this.tpHistoricoMovimentacoes.Size = new System.Drawing.Size(792, 574);
            this.tpHistoricoMovimentacoes.TabIndex = 1;
            this.tpHistoricoMovimentacoes.Text = "Histórico de Movimentações";
            this.tpHistoricoMovimentacoes.UseVisualStyleBackColor = true;
            // 
            // gbFiltros
            // 
            this.gbFiltros.Controls.Add(this.lblFiltroDataInicio);
            this.gbFiltros.Controls.Add(this.dtpFiltroDataInicio);
            this.gbFiltros.Controls.Add(this.lblFiltroDataFim);
            this.gbFiltros.Controls.Add(this.dtpFiltroDataFim);
            this.gbFiltros.Controls.Add(this.lblFiltroProduto);
            this.gbFiltros.Controls.Add(this.cmbFiltroProduto);
            this.gbFiltros.Controls.Add(this.lblFiltroTipoMovimentacao);
            this.gbFiltros.Controls.Add(this.cmbFiltroTipoMovimentacao);
            this.gbFiltros.Controls.Add(this.btnFiltrar);
            this.gbFiltros.Controls.Add(this.btnLimparFiltros);
            this.gbFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFiltros.Location = new System.Drawing.Point(3, 3);
            this.gbFiltros.Name = "gbFiltros";
            this.gbFiltros.Size = new System.Drawing.Size(786, 100);
            this.gbFiltros.TabIndex = 0;
            this.gbFiltros.TabStop = false;
            this.gbFiltros.Text = "Filtros";
            // 
            // lblFiltroDataInicio
            // 
            this.lblFiltroDataInicio.AutoSize = true;
            this.lblFiltroDataInicio.Location = new System.Drawing.Point(15, 25);
            this.lblFiltroDataInicio.Name = "lblFiltroDataInicio";
            this.lblFiltroDataInicio.Size = new System.Drawing.Size(64, 13);
            this.lblFiltroDataInicio.TabIndex = 0;
            this.lblFiltroDataInicio.Text = "Data Início:";
            // 
            // dtpFiltroDataInicio
            // 
            this.dtpFiltroDataInicio.Location = new System.Drawing.Point(15, 41);
            this.dtpFiltroDataInicio.Name = "dtpFiltroDataInicio";
            this.dtpFiltroDataInicio.Size = new System.Drawing.Size(150, 20);
            this.dtpFiltroDataInicio.TabIndex = 1;
            // 
            // lblFiltroDataFim
            // 
            this.lblFiltroDataFim.AutoSize = true;
            this.lblFiltroDataFim.Location = new System.Drawing.Point(180, 25);
            this.lblFiltroDataFim.Name = "lblFiltroDataFim";
            this.lblFiltroDataFim.Size = new System.Drawing.Size(55, 13);
            this.lblFiltroDataFim.TabIndex = 2;
            this.lblFiltroDataFim.Text = "Data Fim:";
            // 
            // dtpFiltroDataFim
            // 
            this.dtpFiltroDataFim.Location = new System.Drawing.Point(180, 41);
            this.dtpFiltroDataFim.Name = "dtpFiltroDataFim";
            this.dtpFiltroDataFim.Size = new System.Drawing.Size(150, 20);
            this.dtpFiltroDataFim.TabIndex = 3;
            // 
            // lblFiltroProduto
            // 
            this.lblFiltroProduto.AutoSize = true;
            this.lblFiltroProduto.Location = new System.Drawing.Point(345, 25);
            this.lblFiltroProduto.Name = "lblFiltroProduto";
            this.lblFiltroProduto.Size = new System.Drawing.Size(47, 13);
            this.lblFiltroProduto.TabIndex = 4;
            this.lblFiltroProduto.Text = "Produto:";
            // 
            // cmbFiltroProduto
            // 
            this.cmbFiltroProduto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroProduto.FormattingEnabled = true;
            this.cmbFiltroProduto.Location = new System.Drawing.Point(345, 41);
            this.cmbFiltroProduto.Name = "cmbFiltroProduto";
            this.cmbFiltroProduto.Size = new System.Drawing.Size(150, 21);
            this.cmbFiltroProduto.TabIndex = 5;
            // 
            // lblFiltroTipoMovimentacao
            // 
            this.lblFiltroTipoMovimentacao.AutoSize = true;
            this.lblFiltroTipoMovimentacao.Location = new System.Drawing.Point(510, 25);
            this.lblFiltroTipoMovimentacao.Name = "lblFiltroTipoMovimentacao";
            this.lblFiltroTipoMovimentacao.Size = new System.Drawing.Size(31, 13);
            this.lblFiltroTipoMovimentacao.TabIndex = 6;
            this.lblFiltroTipoMovimentacao.Text = "Tipo:";
            // 
            // cmbFiltroTipoMovimentacao
            // 
            this.cmbFiltroTipoMovimentacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroTipoMovimentacao.FormattingEnabled = true;
            this.cmbFiltroTipoMovimentacao.Location = new System.Drawing.Point(510, 41);
            this.cmbFiltroTipoMovimentacao.Name = "cmbFiltroTipoMovimentacao";
            this.cmbFiltroTipoMovimentacao.Size = new System.Drawing.Size(120, 21);
            this.cmbFiltroTipoMovimentacao.TabIndex = 7;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(650, 39);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(60, 23);
            this.btnFiltrar.TabIndex = 8;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.Location = new System.Drawing.Point(716, 39);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(60, 23);
            this.btnLimparFiltros.TabIndex = 9;
            this.btnLimparFiltros.Text = "Limpar";
            this.btnLimparFiltros.UseVisualStyleBackColor = true;
            // 
            // dgvMovimentacoes
            // 
            this.dgvMovimentacoes.AllowUserToAddRows = false;
            this.dgvMovimentacoes.AllowUserToDeleteRows = false;
            this.dgvMovimentacoes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMovimentacoes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimentacoes.Location = new System.Drawing.Point(3, 109);
            this.dgvMovimentacoes.Name = "dgvMovimentacoes";
            this.dgvMovimentacoes.ReadOnly = true;
            this.dgvMovimentacoes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMovimentacoes.Size = new System.Drawing.Size(786, 422);
            this.dgvMovimentacoes.TabIndex = 1;
            // 
            // pnlPaginacao
            // 
            this.pnlPaginacao.Controls.Add(this.lblPaginaAtual);
            this.pnlPaginacao.Controls.Add(this.btnPaginaAnterior);
            this.pnlPaginacao.Controls.Add(this.btnProximaPagina);
            this.pnlPaginacao.Controls.Add(this.lblTotalRegistros);
            this.pnlPaginacao.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPaginacao.Location = new System.Drawing.Point(3, 537);
            this.pnlPaginacao.Name = "pnlPaginacao";
            this.pnlPaginacao.Size = new System.Drawing.Size(786, 34);
            this.pnlPaginacao.TabIndex = 2;
            // 
            // lblPaginaAtual
            // 
            this.lblPaginaAtual.AutoSize = true;
            this.lblPaginaAtual.Location = new System.Drawing.Point(350, 10);
            this.lblPaginaAtual.Name = "lblPaginaAtual";
            this.lblPaginaAtual.Size = new System.Drawing.Size(59, 13);
            this.lblPaginaAtual.TabIndex = 0;
            this.lblPaginaAtual.Text = "Página 1/1";
            // 
            // btnPaginaAnterior
            // 
            this.btnPaginaAnterior.Location = new System.Drawing.Point(300, 6);
            this.btnPaginaAnterior.Name = "btnPaginaAnterior";
            this.btnPaginaAnterior.Size = new System.Drawing.Size(44, 23);
            this.btnPaginaAnterior.TabIndex = 1;
            this.btnPaginaAnterior.Text = "< Ant";
            this.btnPaginaAnterior.UseVisualStyleBackColor = true;
            // 
            // btnProximaPagina
            // 
            this.btnProximaPagina.Location = new System.Drawing.Point(415, 6);
            this.btnProximaPagina.Name = "btnProximaPagina";
            this.btnProximaPagina.Size = new System.Drawing.Size(44, 23);
            this.btnProximaPagina.TabIndex = 2;
            this.btnProximaPagina.Text = "Prox >";
            this.btnProximaPagina.UseVisualStyleBackColor = true;
            // 
            // lblTotalRegistros
            // 
            this.lblTotalRegistros.AutoSize = true;
            this.lblTotalRegistros.Location = new System.Drawing.Point(10, 10);
            this.lblTotalRegistros.Name = "lblTotalRegistros";
            this.lblTotalRegistros.Size = new System.Drawing.Size(88, 13);
            this.lblTotalRegistros.TabIndex = 3;
            this.lblTotalRegistros.Text = "Total: 0 registros";
            // 
            // tpAlertasEstoque
            // 
            this.tpAlertasEstoque.Controls.Add(this.dgvAlertas);
            this.tpAlertasEstoque.Controls.Add(this.pnlAlertasBotoes);
            this.tpAlertasEstoque.Location = new System.Drawing.Point(4, 22);
            this.tpAlertasEstoque.Name = "tpAlertasEstoque";
            this.tpAlertasEstoque.Size = new System.Drawing.Size(792, 574);
            this.tpAlertasEstoque.TabIndex = 2;
            this.tpAlertasEstoque.Text = "Alertas de Estoque";
            this.tpAlertasEstoque.UseVisualStyleBackColor = true;
            // 
            // dgvAlertas
            // 
            this.dgvAlertas.AllowUserToAddRows = false;
            this.dgvAlertas.AllowUserToDeleteRows = false;
            this.dgvAlertas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlertas.Location = new System.Drawing.Point(0, 0);
            this.dgvAlertas.Name = "dgvAlertas";
            this.dgvAlertas.ReadOnly = true;
            this.dgvAlertas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlertas.Size = new System.Drawing.Size(792, 534);
            this.dgvAlertas.TabIndex = 0;
            // 
            // pnlAlertasBotoes
            // 
            this.pnlAlertasBotoes.Controls.Add(this.btnAtualizarAlertas);
            this.pnlAlertasBotoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlertasBotoes.Location = new System.Drawing.Point(0, 534);
            this.pnlAlertasBotoes.Name = "pnlAlertasBotoes";
            this.pnlAlertasBotoes.Size = new System.Drawing.Size(792, 40);
            this.pnlAlertasBotoes.TabIndex = 1;
            // 
            // btnAtualizarAlertas
            // 
            this.btnAtualizarAlertas.Location = new System.Drawing.Point(10, 8);
            this.btnAtualizarAlertas.Name = "btnAtualizarAlertas";
            this.btnAtualizarAlertas.Size = new System.Drawing.Size(100, 23);
            this.btnAtualizarAlertas.TabIndex = 0;
            this.btnAtualizarAlertas.Text = "Atualizar Alertas";
            this.btnAtualizarAlertas.UseVisualStyleBackColor = true;
            // 
            // frmMovimentacaoEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tcMovimentacao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmMovimentacaoEstoque";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movimentação de Estoque";
            this.tcMovimentacao.ResumeLayout(false);
            this.tpNovaMovimentacao.ResumeLayout(false);
            this.gbMovimentacao.ResumeLayout(false);
            this.gbMovimentacao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).EndInit();
            this.gbDadosComplementares.ResumeLayout(false);
            this.gbDadosComplementares.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecoCompra)).EndInit();
            this.gbValidacao.ResumeLayout(false);
            this.gbValidacao.PerformLayout();
            this.pnlBotoes.ResumeLayout(false);
            this.tpHistoricoMovimentacoes.ResumeLayout(false);
            this.gbFiltros.ResumeLayout(false);
            this.gbFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimentacoes)).EndInit();
            this.pnlPaginacao.ResumeLayout(false);
            this.pnlPaginacao.PerformLayout();
            this.tpAlertasEstoque.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlertas)).EndInit();
            this.pnlAlertasBotoes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMovimentacao;
        private System.Windows.Forms.TabPage tpNovaMovimentacao;
        private System.Windows.Forms.GroupBox gbMovimentacao;
        private System.Windows.Forms.Label lblTipoMovimentacao;
        private System.Windows.Forms.ComboBox cmbTipoMovimentacao;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.ComboBox cmbProduto;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.NumericUpDown nudQuantidade;
        private System.Windows.Forms.Label lblObservacoes;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.GroupBox gbDadosComplementares;
        private System.Windows.Forms.Label lblLote;
        private System.Windows.Forms.TextBox txtLote;
        private System.Windows.Forms.Label lblDataVencimento;
        private System.Windows.Forms.DateTimePicker dtpDataVencimento;
        private System.Windows.Forms.Label lblPrecoCompra;
        private System.Windows.Forms.NumericUpDown nudPrecoCompra;
        private System.Windows.Forms.Label lblFornecedor;
    private System.Windows.Forms.ComboBox cmbFornecedor;
        private System.Windows.Forms.Label lblNotaFiscal;
        private System.Windows.Forms.TextBox txtNotaFiscal;
        private System.Windows.Forms.Label lblDataOperacao;
        private System.Windows.Forms.DateTimePicker dtpDataOperacao;
        private System.Windows.Forms.GroupBox gbValidacao;
        private System.Windows.Forms.Label lblEstoqueAtual;
        private System.Windows.Forms.Label lblEstoqueAtualValor;
        private System.Windows.Forms.Label lblNovoEstoque;
        private System.Windows.Forms.Label lblNovoEstoqueValor;
        private System.Windows.Forms.Panel pnlBotoes;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TabPage tpHistoricoMovimentacoes;
        private System.Windows.Forms.GroupBox gbFiltros;
        private System.Windows.Forms.Label lblFiltroDataInicio;
        private System.Windows.Forms.DateTimePicker dtpFiltroDataInicio;
        private System.Windows.Forms.Label lblFiltroDataFim;
        private System.Windows.Forms.DateTimePicker dtpFiltroDataFim;
        private System.Windows.Forms.Label lblFiltroProduto;
        private System.Windows.Forms.ComboBox cmbFiltroProduto;
        private System.Windows.Forms.Label lblFiltroTipoMovimentacao;
        private System.Windows.Forms.ComboBox cmbFiltroTipoMovimentacao;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnLimparFiltros;
        private System.Windows.Forms.DataGridView dgvMovimentacoes;
        private System.Windows.Forms.Panel pnlPaginacao;
        private System.Windows.Forms.Label lblPaginaAtual;
        private System.Windows.Forms.Button btnPaginaAnterior;
        private System.Windows.Forms.Button btnProximaPagina;
        private System.Windows.Forms.Label lblTotalRegistros;
        private System.Windows.Forms.TabPage tpAlertasEstoque;
        private System.Windows.Forms.DataGridView dgvAlertas;
        private System.Windows.Forms.Panel pnlAlertasBotoes;
        private System.Windows.Forms.Button btnAtualizarAlertas;
    }
}
