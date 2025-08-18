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
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            btnExcluir = new Button();
            btnAlterar = new Button();
            LblId = new Label();
            btnConsultar = new Button();
            btnCadastrar = new Button();
            txtNomeCategoria = new TextBox();
            lblNomeCategoria = new Label();
            lstGridCategoria = new DataGridView();
            btnAtualziar = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lstGridCategoria).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(groupBox1);
            panel1.Location = new Point(3, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(779, 73);
            panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnAtualziar);
            groupBox1.Controls.Add(btnExcluir);
            groupBox1.Controls.Add(btnAlterar);
            groupBox1.Controls.Add(LblId);
            groupBox1.Controls.Add(btnConsultar);
            groupBox1.Controls.Add(btnCadastrar);
            groupBox1.Controls.Add(txtNomeCategoria);
            groupBox1.Controls.Add(lblNomeCategoria);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(770, 67);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Cadastrar Categoria";
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(605, 12);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(75, 49);
            btnExcluir.TabIndex = 6;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnAlterar
            // 
            btnAlterar.Location = new Point(686, 12);
            btnAlterar.Name = "btnAlterar";
            btnAlterar.Size = new Size(75, 49);
            btnAlterar.TabIndex = 5;
            btnAlterar.Text = "Alterar";
            btnAlterar.UseVisualStyleBackColor = true;
            btnAlterar.Click += btnAlterar_Click;
            // 
            // LblId
            // 
            LblId.AutoSize = true;
            LblId.Location = new Point(70, 52);
            LblId.Name = "LblId";
            LblId.Size = new Size(68, 15);
            LblId.TabIndex = 4;
            LblId.Text = "IdCategoria";
            LblId.Visible = false;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(263, 12);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(75, 49);
            btnConsultar.TabIndex = 3;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // btnCadastrar
            // 
            btnCadastrar.Location = new Point(344, 12);
            btnCadastrar.Name = "btnCadastrar";
            btnCadastrar.Size = new Size(75, 49);
            btnCadastrar.TabIndex = 2;
            btnCadastrar.Text = "Cadastrar";
            btnCadastrar.UseVisualStyleBackColor = true;
            btnCadastrar.Click += btnCadastrar_Click_1;
            // 
            // txtNomeCategoria
            // 
            txtNomeCategoria.Location = new Point(70, 29);
            txtNomeCategoria.Name = "txtNomeCategoria";
            txtNomeCategoria.Size = new Size(187, 23);
            txtNomeCategoria.TabIndex = 1;
            // 
            // lblNomeCategoria
            // 
            lblNomeCategoria.AutoSize = true;
            lblNomeCategoria.Location = new Point(6, 32);
            lblNomeCategoria.Name = "lblNomeCategoria";
            lblNomeCategoria.Size = new Size(58, 15);
            lblNomeCategoria.TabIndex = 0;
            lblNomeCategoria.Text = "Categoria";
            // 
            // lstGridCategoria
            // 
            lstGridCategoria.AllowUserToAddRows = false;
            lstGridCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstGridCategoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lstGridCategoria.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lstGridCategoria.BackgroundColor = Color.FromArgb(224, 224, 224);
            lstGridCategoria.BorderStyle = BorderStyle.None;
            lstGridCategoria.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            lstGridCategoria.ColumnHeadersHeight = 30;
            lstGridCategoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            lstGridCategoria.Location = new Point(6, 91);
            lstGridCategoria.Name = "lstGridCategoria";
            lstGridCategoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lstGridCategoria.Size = new Size(776, 347);
            lstGridCategoria.TabIndex = 0;
            lstGridCategoria.CellDoubleClick += lstGridCategoria_CellDoubleClick;
            // 
            // btnAtualziar
            // 
            btnAtualziar.Location = new Point(425, 12);
            btnAtualziar.Name = "btnAtualziar";
            btnAtualziar.Size = new Size(75, 49);
            btnAtualziar.TabIndex = 7;
            btnAtualziar.Text = "Atualizar";
            btnAtualziar.UseVisualStyleBackColor = true;
            btnAtualziar.Click += btnAtualziar_Click;
            // 
            // CadCategoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 450);
            Controls.Add(lstGridCategoria);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CadCategoria";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastro de Categoria";
            Load += CadCategoria_Load;
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lstGridCategoria).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox1;
        private Button btnCadastrar;
        private TextBox txtNomeCategoria;
        private Label lblNomeCategoria;
        private Button btnConsultar;
        private Label LblId;
        private Button btnExcluir;
        private Button btnAlterar;
        private DataGridView lstGridCategoria;
        private Button btnAtualziar;
    }
}