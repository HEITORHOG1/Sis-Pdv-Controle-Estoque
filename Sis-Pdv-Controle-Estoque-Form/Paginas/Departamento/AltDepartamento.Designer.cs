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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAlterar = new System.Windows.Forms.Button();
            this.LblId = new System.Windows.Forms.Label();
            this.txtNomeDepartamento = new System.Windows.Forms.TextBox();
            this.lblNomeCategoria = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(2, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 100);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAlterar);
            this.groupBox1.Controls.Add(this.LblId);
            this.groupBox1.Controls.Add(this.txtNomeDepartamento);
            this.groupBox1.Controls.Add(this.lblNomeCategoria);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alterar Departamento";
            // 
            // btnAlterar
            // 
            this.btnAlterar.Location = new System.Drawing.Point(405, 28);
            this.btnAlterar.Name = "btnAlterar";
            this.btnAlterar.Size = new System.Drawing.Size(75, 49);
            this.btnAlterar.TabIndex = 8;
            this.btnAlterar.Text = "Alterar";
            this.btnAlterar.UseVisualStyleBackColor = true;
            this.btnAlterar.Click += new System.EventHandler(this.btnAlterar_Click);
            // 
            // LblId
            // 
            this.LblId.AutoSize = true;
            this.LblId.Location = new System.Drawing.Point(69, 76);
            this.LblId.Name = "LblId";
            this.LblId.Size = new System.Drawing.Size(93, 15);
            this.LblId.TabIndex = 7;
            this.LblId.Text = "IdDepartamento";
            this.LblId.Visible = false;
            // 
            // txtNomeDepartamento
            // 
            this.txtNomeDepartamento.Location = new System.Drawing.Point(94, 42);
            this.txtNomeDepartamento.Name = "txtNomeDepartamento";
            this.txtNomeDepartamento.Size = new System.Drawing.Size(187, 23);
            this.txtNomeDepartamento.TabIndex = 6;
            // 
            // lblNomeCategoria
            // 
            this.lblNomeCategoria.AutoSize = true;
            this.lblNomeCategoria.Location = new System.Drawing.Point(5, 45);
            this.lblNomeCategoria.Name = "lblNomeCategoria";
            this.lblNomeCategoria.Size = new System.Drawing.Size(83, 15);
            this.lblNomeCategoria.TabIndex = 5;
            this.lblNomeCategoria.Text = "Departamento";
            // 
            // AltDepartamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 110);
            this.Controls.Add(this.panel1);
            this.Name = "AltDepartamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alterar Departamento";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AltDepartamento_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AltDepartamento_FormClosed);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox1;
        private Button btnAlterar;
        private Label LblId;
        private TextBox txtNomeDepartamento;
        private Label lblNomeCategoria;
    }
}