namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    partial class frmRelatorio
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnMenuProduto = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMenuRelatorioVendasPorData = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMenuPedido = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMenuContasPagar = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMenuContasReceber = new System.Windows.Forms.ToolStripMenuItem();
            this.pnForm = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMenuProduto,
            this.btnMenuPedido,
            this.btnMenuContasPagar,
            this.btnMenuContasReceber});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnMenuProduto
            // 
            this.btnMenuProduto.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMenuRelatorioVendasPorData});
            this.btnMenuProduto.Name = "btnMenuProduto";
            this.btnMenuProduto.Size = new System.Drawing.Size(62, 20);
            this.btnMenuProduto.Text = "Produto";
            // 
            // btnMenuRelatorioVendasPorData
            // 
            this.btnMenuRelatorioVendasPorData.Name = "btnMenuRelatorioVendasPorData";
            this.btnMenuRelatorioVendasPorData.Size = new System.Drawing.Size(225, 22);
            this.btnMenuRelatorioVendasPorData.Text = "Relatorio de Vendas Por Data";
            this.btnMenuRelatorioVendasPorData.Click += new System.EventHandler(this.btnMenuRelatorioVendasPorData_Click);
            // 
            // btnMenuPedido
            // 
            this.btnMenuPedido.Name = "btnMenuPedido";
            this.btnMenuPedido.Size = new System.Drawing.Size(56, 20);
            this.btnMenuPedido.Text = "Pedido";
            // 
            // btnMenuContasPagar
            // 
            this.btnMenuContasPagar.Name = "btnMenuContasPagar";
            this.btnMenuContasPagar.Size = new System.Drawing.Size(98, 20);
            this.btnMenuContasPagar.Text = "Contas a Pagar";
            // 
            // btnMenuContasReceber
            // 
            this.btnMenuContasReceber.Name = "btnMenuContasReceber";
            this.btnMenuContasReceber.Size = new System.Drawing.Size(110, 20);
            this.btnMenuContasReceber.Text = "Contas a Receber";
            // 
            // pnForm
            // 
            this.pnForm.BackColor = System.Drawing.Color.Black;
            this.pnForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnForm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnForm.Location = new System.Drawing.Point(0, 24);
            this.pnForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnForm.Name = "pnForm";
            this.pnForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnForm.Size = new System.Drawing.Size(800, 426);
            this.pnForm.TabIndex = 9;
            // 
            // frmRelatorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnForm);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmRelatorio";
            this.Text = "frmRelatorio";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem btnMenuProduto;
        private ToolStripMenuItem btnMenuPedido;
        private ToolStripMenuItem btnMenuContasPagar;
        private ToolStripMenuItem btnMenuContasReceber;
        private ToolStripMenuItem btnMenuRelatorioVendasPorData;
        private Panel pnForm;
    }
}