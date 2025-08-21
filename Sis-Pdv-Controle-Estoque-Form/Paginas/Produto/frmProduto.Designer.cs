namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Produto
{
    partial class frmProduto
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblCodBarras = new System.Windows.Forms.Label();
            this.txbCodigoBarras = new System.Windows.Forms.TextBox();
            this.lblFornecedor = new System.Windows.Forms.Label();
            this.cmbFornecedor = new System.Windows.Forms.ComboBox();
            this.gpDescricaoNome = new System.Windows.Forms.GroupBox();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.rtbDescricao = new System.Windows.Forms.RichTextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.txbNome = new System.Windows.Forms.TextBox();
            this.gpCategoria = new System.Windows.Forms.GroupBox();
            this.rbNaoPerecivel = new System.Windows.Forms.RadioButton();
            this.rbPerecivel = new System.Windows.Forms.RadioButton();
            this.btnAlterar = new FontAwesome.Sharp.IconButton();
            this.btnAdicionar = new FontAwesome.Sharp.IconButton();
            this.rbProdutoInativo = new System.Windows.Forms.RadioButton();
            this.rbProdutoAtivo = new System.Windows.Forms.RadioButton();
            this.dgvProduto = new System.Windows.Forms.DataGridView();
            this.gpAtivo = new System.Windows.Forms.GroupBox();
            this.ckbInativo = new System.Windows.Forms.CheckBox();
            this.btnConsulta = new FontAwesome.Sharp.IconButton();
            this.txbId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gpDescricaoNome.SuspendLayout();
            this.gpCategoria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduto)).BeginInit();
            this.gpAtivo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCodBarras
            // 
            this.lblCodBarras.AutoSize = true;
            this.lblCodBarras.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCodBarras.ForeColor = System.Drawing.Color.Black;
            this.lblCodBarras.Location = new System.Drawing.Point(104, 21);
            this.lblCodBarras.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodBarras.Name = "lblCodBarras";
            this.lblCodBarras.Size = new System.Drawing.Size(118, 15);
            this.lblCodBarras.TabIndex = 0;
            this.lblCodBarras.Text = "Código de barras";
            // 
            // txbCodigoBarras
            // 
            this.txbCodigoBarras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCodigoBarras.Location = new System.Drawing.Point(107, 42);
            this.txbCodigoBarras.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbCodigoBarras.Name = "txbCodigoBarras";
            this.txbCodigoBarras.Size = new System.Drawing.Size(307, 23);
            this.txbCodigoBarras.TabIndex = 1;
            this.txbCodigoBarras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbCodigoBarras_KeyPress);
            // 
            // lblFornecedor
            // 
            this.lblFornecedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFornecedor.AutoSize = true;
            this.lblFornecedor.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFornecedor.ForeColor = System.Drawing.Color.Black;
            this.lblFornecedor.Location = new System.Drawing.Point(439, 2);
            this.lblFornecedor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFornecedor.Name = "lblFornecedor";
            this.lblFornecedor.Size = new System.Drawing.Size(83, 15);
            this.lblFornecedor.TabIndex = 3;
            this.lblFornecedor.Text = "Fornecedor";
            // 
            // cmbFornecedor
            // 
            this.cmbFornecedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFornecedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFornecedor.FormattingEnabled = true;
            this.cmbFornecedor.Items.AddRange(new object[] {
            "Selecione"});
            this.cmbFornecedor.Location = new System.Drawing.Point(442, 23);
            this.cmbFornecedor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbFornecedor.Name = "cmbFornecedor";
            this.cmbFornecedor.Size = new System.Drawing.Size(227, 23);
            this.cmbFornecedor.TabIndex = 2;
            // 
            // gpDescricaoNome
            // 
            this.gpDescricaoNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpDescricaoNome.Controls.Add(this.lblDescricao);
            this.gpDescricaoNome.Controls.Add(this.rtbDescricao);
            this.gpDescricaoNome.Controls.Add(this.lblNome);
            this.gpDescricaoNome.Controls.Add(this.txbNome);
            this.gpDescricaoNome.Location = new System.Drawing.Point(28, 91);
            this.gpDescricaoNome.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpDescricaoNome.Name = "gpDescricaoNome";
            this.gpDescricaoNome.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpDescricaoNome.Size = new System.Drawing.Size(632, 200);
            this.gpDescricaoNome.TabIndex = 7;
            this.gpDescricaoNome.TabStop = false;
            // 
            // lblDescricao
            // 
            this.lblDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDescricao.ForeColor = System.Drawing.Color.Black;
            this.lblDescricao.Location = new System.Drawing.Point(7, 66);
            this.lblDescricao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(74, 15);
            this.lblDescricao.TabIndex = 13;
            this.lblDescricao.Text = "Descrição";
            // 
            // rtbDescricao
            // 
            this.rtbDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDescricao.Location = new System.Drawing.Point(7, 87);
            this.rtbDescricao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtbDescricao.Name = "rtbDescricao";
            this.rtbDescricao.Size = new System.Drawing.Size(605, 106);
            this.rtbDescricao.TabIndex = 4;
            this.rtbDescricao.Text = "";
            // 
            // lblNome
            // 
            this.lblNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNome.ForeColor = System.Drawing.Color.Black;
            this.lblNome.Location = new System.Drawing.Point(7, 18);
            this.lblNome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(45, 15);
            this.lblNome.TabIndex = 12;
            this.lblNome.Text = "Nome";
            // 
            // txbNome
            // 
            this.txbNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbNome.Location = new System.Drawing.Point(7, 39);
            this.txbNome.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbNome.Name = "txbNome";
            this.txbNome.Size = new System.Drawing.Size(605, 23);
            this.txbNome.TabIndex = 3;
            // 
            // gpCategoria
            // 
            this.gpCategoria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gpCategoria.Controls.Add(this.rbNaoPerecivel);
            this.gpCategoria.Controls.Add(this.rbPerecivel);
            this.gpCategoria.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gpCategoria.Location = new System.Drawing.Point(677, 5);
            this.gpCategoria.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpCategoria.Name = "gpCategoria";
            this.gpCategoria.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpCategoria.Size = new System.Drawing.Size(146, 80);
            this.gpCategoria.TabIndex = 10;
            this.gpCategoria.TabStop = false;
            this.gpCategoria.Text = "Tipo de Produto";
            // 
            // rbNaoPerecivel
            // 
            this.rbNaoPerecivel.AutoSize = true;
            this.rbNaoPerecivel.Location = new System.Drawing.Point(8, 46);
            this.rbNaoPerecivel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbNaoPerecivel.Name = "rbNaoPerecivel";
            this.rbNaoPerecivel.Size = new System.Drawing.Size(116, 19);
            this.rbNaoPerecivel.TabIndex = 1;
            this.rbNaoPerecivel.TabStop = true;
            this.rbNaoPerecivel.Text = "Não Perecível";
            this.rbNaoPerecivel.UseVisualStyleBackColor = true;
            this.rbNaoPerecivel.CheckedChanged += new System.EventHandler(this.rbNaoPerecivel_CheckedChanged);
            // 
            // rbPerecivel
            // 
            this.rbPerecivel.AutoSize = true;
            this.rbPerecivel.Location = new System.Drawing.Point(7, 18);
            this.rbPerecivel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbPerecivel.Name = "rbPerecivel";
            this.rbPerecivel.Size = new System.Drawing.Size(87, 19);
            this.rbPerecivel.TabIndex = 0;
            this.rbPerecivel.TabStop = true;
            this.rbPerecivel.Text = "Perecível";
            this.rbPerecivel.UseVisualStyleBackColor = true;
            this.rbPerecivel.CheckedChanged += new System.EventHandler(this.rbPerecivel_CheckedChanged);
            // 
            // btnAlterar
            // 
            // btnAlterar
            // 
            this.btnAlterar.BackColor = System.Drawing.Color.Azure;
            this.btnAlterar.FlatAppearance.BorderSize = 0;
            this.btnAlterar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlterar.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnAlterar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAlterar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAlterar.IconChar = FontAwesome.Sharp.IconChar.SyncAlt;
            this.btnAlterar.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAlterar.IconSize = 32;
            this.btnAlterar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAlterar.Location = new System.Drawing.Point(195, 368);
            this.btnAlterar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAlterar.Name = "btnAlterar";
            this.btnAlterar.Rotation = 0D;
            this.btnAlterar.Size = new System.Drawing.Size(148, 40);
            this.btnAlterar.TabIndex = 20;
            this.btnAlterar.Text = "Alterar";
            this.btnAlterar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAlterar.UseVisualStyleBackColor = false;
            this.btnAlterar.Click += new System.EventHandler(this.btnAlterar_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.Color.Azure;
            this.btnAdicionar.FlatAppearance.BorderSize = 0;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnAdicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAdicionar.ForeColor = System.Drawing.Color.Green;
            this.btnAdicionar.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.btnAdicionar.IconColor = System.Drawing.Color.Green;
            this.btnAdicionar.IconSize = 32;
            this.btnAdicionar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdicionar.Location = new System.Drawing.Point(28, 368);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Rotation = 0D;
            this.btnAdicionar.Size = new System.Drawing.Size(148, 40);
            this.btnAdicionar.TabIndex = 19;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // rbProdutoInativo
            // 
            this.rbProdutoInativo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbProdutoInativo.AutoSize = true;
            this.rbProdutoInativo.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rbProdutoInativo.Location = new System.Drawing.Point(39, 46);
            this.rbProdutoInativo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbProdutoInativo.Name = "rbProdutoInativo";
            this.rbProdutoInativo.Size = new System.Drawing.Size(124, 19);
            this.rbProdutoInativo.TabIndex = 23;
            this.rbProdutoInativo.TabStop = true;
            this.rbProdutoInativo.Text = "Produto inativo";
            this.rbProdutoInativo.UseVisualStyleBackColor = true;
            // 
            // rbProdutoAtivo
            // 
            this.rbProdutoAtivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbProdutoAtivo.AutoSize = true;
            this.rbProdutoAtivo.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rbProdutoAtivo.Location = new System.Drawing.Point(35, 16);
            this.rbProdutoAtivo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbProdutoAtivo.Name = "rbProdutoAtivo";
            this.rbProdutoAtivo.Size = new System.Drawing.Size(113, 19);
            this.rbProdutoAtivo.TabIndex = 22;
            this.rbProdutoAtivo.TabStop = true;
            this.rbProdutoAtivo.Text = "Produto Ativo";
            this.rbProdutoAtivo.UseVisualStyleBackColor = true;
            // 
            // dgvProduto
            // 
            this.dgvProduto.AllowUserToAddRows = false;
            this.dgvProduto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProduto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProduto.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvProduto.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvProduto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProduto.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProduto.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProduto.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProduto.ColumnHeadersHeight = 30;
            this.dgvProduto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduto.EnableHeadersVisualStyles = false;
            this.dgvProduto.GridColor = System.Drawing.Color.SandyBrown;
            this.dgvProduto.Location = new System.Drawing.Point(23, 415);
            this.dgvProduto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvProduto.Name = "dgvProduto";
            this.dgvProduto.ReadOnly = true;
            this.dgvProduto.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProduto.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProduto.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dgvProduto.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProduto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduto.Size = new System.Drawing.Size(975, 177);
            this.dgvProduto.TabIndex = 24;
            this.dgvProduto.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduto_CellDoubleClick);
            // 
            // gpAtivo
            // 
            this.gpAtivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gpAtivo.Controls.Add(this.rbProdutoAtivo);
            this.gpAtivo.Controls.Add(this.rbProdutoInativo);
            this.gpAtivo.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gpAtivo.Location = new System.Drawing.Point(830, 5);
            this.gpAtivo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpAtivo.Name = "gpAtivo";
            this.gpAtivo.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpAtivo.Size = new System.Drawing.Size(169, 80);
            this.gpAtivo.TabIndex = 11;
            this.gpAtivo.TabStop = false;
            // 
            // ckbInativo
            // 
            this.ckbInativo.AutoSize = true;
            this.ckbInativo.Location = new System.Drawing.Point(533, 380);
            this.ckbInativo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ckbInativo.Name = "ckbInativo";
            this.ckbInativo.Size = new System.Drawing.Size(62, 19);
            this.ckbInativo.TabIndex = 28;
            this.ckbInativo.Text = "Inativo";
            this.ckbInativo.UseVisualStyleBackColor = true;
            // 
            // btnConsulta
            // 
            this.btnConsulta.BackColor = System.Drawing.Color.Azure;
            this.btnConsulta.FlatAppearance.BorderSize = 0;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnConsulta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConsulta.ForeColor = System.Drawing.Color.Green;
            this.btnConsulta.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.btnConsulta.IconColor = System.Drawing.Color.Green;
            this.btnConsulta.IconSize = 32;
            this.btnConsulta.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConsulta.Location = new System.Drawing.Point(364, 368);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Rotation = 0D;
            this.btnConsulta.Size = new System.Drawing.Size(148, 40);
            this.btnConsulta.TabIndex = 29;
            this.btnConsulta.Text = "Consulta";
            this.btnConsulta.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConsulta.UseVisualStyleBackColor = false;
            this.btnConsulta.Click += new System.EventHandler(this.btnConsulta_Click);
            // 
            // txbId
            // 
            this.txbId.Enabled = false;
            this.txbId.Location = new System.Drawing.Point(22, 42);
            this.txbId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbId.Name = "txbId";
            this.txbId.Size = new System.Drawing.Size(62, 23);
            this.txbId.TabIndex = 45;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblId.ForeColor = System.Drawing.Color.Black;
            this.lblId.Location = new System.Drawing.Point(20, 21);
            this.lblId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(21, 15);
            this.lblId.TabIndex = 44;
            this.lblId.Text = "ID";
            // 
            // cmbCategoria
            // 
            this.cmbCategoria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.FormattingEnabled = true;
            this.cmbCategoria.Items.AddRange(new object[] {
            "Selecione"});
            this.cmbCategoria.Location = new System.Drawing.Point(442, 71);
            this.cmbCategoria.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(227, 23);
            this.cmbCategoria.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(439, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 47;
            this.label1.Text = "Categoria";
            // 
            // frmProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1013, 602);
            this.Controls.Add(this.cmbCategoria);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbId);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.ckbInativo);
            this.Controls.Add(this.gpAtivo);
            this.Controls.Add(this.dgvProduto);
            this.Controls.Add(this.btnAlterar);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.gpCategoria);
            this.Controls.Add(this.gpDescricaoNome);
            this.Controls.Add(this.cmbFornecedor);
            this.Controls.Add(this.lblFornecedor);
            this.Controls.Add(this.txbCodigoBarras);
            this.Controls.Add(this.lblCodBarras);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmProduto";
            this.Text = "frmProduto";
            this.Load += new System.EventHandler(this.frmProduto_Load);
            this.gpDescricaoNome.ResumeLayout(false);
            this.gpDescricaoNome.PerformLayout();
            this.gpCategoria.ResumeLayout(false);
            this.gpCategoria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduto)).EndInit();
            this.gpAtivo.ResumeLayout(false);
            this.gpAtivo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCodBarras;
        private System.Windows.Forms.TextBox txbCodigoBarras;
        private System.Windows.Forms.Label lblFornecedor;
        private System.Windows.Forms.ComboBox cmbFornecedor;
        private System.Windows.Forms.GroupBox gpDescricaoNome;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.RichTextBox rtbDescricao;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txbNome;
        private System.Windows.Forms.GroupBox gpCategoria;
        private System.Windows.Forms.RadioButton rbNaoPerecivel;
        private System.Windows.Forms.RadioButton rbPerecivel;
        private System.Windows.Forms.GroupBox gpValores;
        private System.Windows.Forms.TextBox txbMargemDeLucro;
        private System.Windows.Forms.Label lblMargem;
        private System.Windows.Forms.TextBox txbPrecoDeVenda;
        private System.Windows.Forms.Label lblPrecoVenda;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.TextBox txbQuantidadeEstoque;
        private System.Windows.Forms.TextBox txbPrecoCusto;
        private System.Windows.Forms.Label lblPrecoCusto;
        private System.Windows.Forms.Label lblDataFabricacao;
        private System.Windows.Forms.Label lblDataVencimento;
        private FontAwesome.Sharp.IconButton btnAdicionar;
        private FontAwesome.Sharp.IconButton btnAlterar;
        private System.Windows.Forms.RadioButton rbProdutoInativo;
        private System.Windows.Forms.RadioButton rbProdutoAtivo;
        private System.Windows.Forms.DataGridView dgvProduto;
        private System.Windows.Forms.GroupBox gpAtivo;
        private System.Windows.Forms.CheckBox ckbInativo;
        private FontAwesome.Sharp.IconButton btnConsulta;
        private System.Windows.Forms.MaskedTextBox msktDataFabricacao;
        private System.Windows.Forms.MaskedTextBox msktDataVencimento;
        private System.Windows.Forms.TextBox txbId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label label1;
    }
}