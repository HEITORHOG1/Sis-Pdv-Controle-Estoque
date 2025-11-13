namespace Sis_Pdv_Controle_Estoque_Form.Paginas
{
    partial class frmMenu
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
            
            // Header Panel
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblTituloSistema = new System.Windows.Forms.Label();
            this.lblUsuarioLogado = new System.Windows.Forms.Label();
            this.lblDataHora = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            // Sidebar Navigation
            this.pnMenuVertical = new System.Windows.Forms.Panel();
            this.pnMenuContainer = new System.Windows.Forms.Panel();
            
            // Logo Section
            this.pnLogo = new System.Windows.Forms.Panel();
            this.lblNomeEmpresa = new System.Windows.Forms.Label();
            this.pictBoxLogo = new System.Windows.Forms.PictureBox();
            this.btnToggleMenu = new System.Windows.Forms.Button();
            
            // Navigation Buttons
            this.btnHome = new System.Windows.Forms.Button();
            this.btnProdutos = new System.Windows.Forms.Button();
            this.btnColaboradores = new System.Windows.Forms.Button();
            this.btnFornecedores = new System.Windows.Forms.Button();
            this.btnCategorias = new System.Windows.Forms.Button();
            this.btnDepartamentos = new System.Windows.Forms.Button();
            this.btnPDV = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            
            // Content Area
            this.pnTopInfo = new System.Windows.Forms.Panel();
            this.lblModuloAtivo = new System.Windows.Forms.Label();
            this.lblDescricaoModulo = new System.Windows.Forms.Label();
            this.iconModuloAtivo = new System.Windows.Forms.Label();
            
            this.pnForm = new System.Windows.Forms.Panel();
            
            // Status Bar
            this.pnStatus = new System.Windows.Forms.Panel();
            this.lblStatusSistema = new System.Windows.Forms.Label();
            this.lblVersaoSistema = new System.Windows.Forms.Label();
            this.lblAutor = new System.Windows.Forms.LinkLabel();
            this.progressStatus = new System.Windows.Forms.ProgressBar();
            
            // Timer
            this.timerRelogio = new System.Windows.Forms.Timer(this.components);
            
            // Suspension of layout
            this.pnHeader.SuspendLayout();
            this.pnMenuVertical.SuspendLayout();
            this.pnMenuContainer.SuspendLayout();
            this.pnLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxLogo)).BeginInit();
            this.pnTopInfo.SuspendLayout();
            this.pnStatus.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.pnHeader.Controls.Add(this.lblTituloSistema);
            this.pnHeader.Controls.Add(this.lblUsuarioLogado);
            this.pnHeader.Controls.Add(this.lblDataHora);
            this.pnHeader.Controls.Add(this.btnMinimizar);
            this.pnHeader.Controls.Add(this.btnMaximizar);
            this.pnHeader.Controls.Add(this.btnClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(1400, 60);
            this.pnHeader.TabIndex = 0;
            this.pnHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnHeader_MouseDown);
            
            // 
            // lblTituloSistema
            // 
            this.lblTituloSistema.AutoSize = true;
            this.lblTituloSistema.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTituloSistema.ForeColor = System.Drawing.Color.White;
            this.lblTituloSistema.Location = new System.Drawing.Point(280, 15);
            this.lblTituloSistema.Name = "lblTituloSistema";
            this.lblTituloSistema.Size = new System.Drawing.Size(420, 32);
            this.lblTituloSistema.TabIndex = 0;
            this.lblTituloSistema.Text = "🏪 SISTEMA PDV - MENU PRINCIPAL";
            
            // 
            // lblUsuarioLogado
            // 
            this.lblUsuarioLogado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsuarioLogado.AutoSize = true;
            this.lblUsuarioLogado.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUsuarioLogado.ForeColor = System.Drawing.Color.LightBlue;
            this.lblUsuarioLogado.Location = new System.Drawing.Point(900, 8);
            this.lblUsuarioLogado.Name = "lblUsuarioLogado";
            this.lblUsuarioLogado.Size = new System.Drawing.Size(150, 21);
            this.lblUsuarioLogado.TabIndex = 1;
            this.lblUsuarioLogado.Text = "👤 Usuário Logado";
            
            // 
            // lblDataHora
            // 
            this.lblDataHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataHora.AutoSize = true;
            this.lblDataHora.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDataHora.ForeColor = System.Drawing.Color.LightGray;
            this.lblDataHora.Location = new System.Drawing.Point(900, 32);
            this.lblDataHora.Name = "lblDataHora";
            this.lblDataHora.Size = new System.Drawing.Size(160, 19);
            this.lblDataHora.TabIndex = 2;
            this.lblDataHora.Text = "📅 00/00/0000 - 00:00:00";
            
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(1280, 15);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(35, 30);
            this.btnMinimizar.TabIndex = 3;
            this.btnMinimizar.Text = "─";
            this.btnMinimizar.UseVisualStyleBackColor = false;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMin_Click);
            
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMaximizar.ForeColor = System.Drawing.Color.White;
            this.btnMaximizar.Location = new System.Drawing.Point(1320, 15);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(35, 30);
            this.btnMaximizar.TabIndex = 4;
            this.btnMaximizar.Text = "⬜";
            this.btnMaximizar.UseVisualStyleBackColor = false;
            this.btnMaximizar.Click += new System.EventHandler(this.btnMax_Click);
            
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1360, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // 
            // pnMenuVertical
            // 
            this.pnMenuVertical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnMenuVertical.Controls.Add(this.pnMenuContainer);
            this.pnMenuVertical.Controls.Add(this.pnLogo);
            this.pnMenuVertical.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnMenuVertical.Location = new System.Drawing.Point(0, 60);
            this.pnMenuVertical.Name = "pnMenuVertical";
            this.pnMenuVertical.Size = new System.Drawing.Size(260, 700);
            this.pnMenuVertical.TabIndex = 1;
            
            // 
            // pnLogo
            // 
            this.pnLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnLogo.Controls.Add(this.lblNomeEmpresa);
            this.pnLogo.Controls.Add(this.pictBoxLogo);
            this.pnLogo.Controls.Add(this.btnToggleMenu);
            this.pnLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnLogo.Location = new System.Drawing.Point(0, 0);
            this.pnLogo.Name = "pnLogo";
            this.pnLogo.Size = new System.Drawing.Size(260, 80);
            this.pnLogo.TabIndex = 0;
            
            // 
            // lblNomeEmpresa
            // 
            this.lblNomeEmpresa.AutoSize = true;
            this.lblNomeEmpresa.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblNomeEmpresa.ForeColor = System.Drawing.Color.White;
            this.lblNomeEmpresa.Location = new System.Drawing.Point(60, 25);
            this.lblNomeEmpresa.Name = "lblNomeEmpresa";
            this.lblNomeEmpresa.Size = new System.Drawing.Size(150, 25);
            this.lblNomeEmpresa.TabIndex = 0;
            this.lblNomeEmpresa.Text = "🏪 SISTEMA PDV";
            
            // 
            // pictBoxLogo
            // 
            this.pictBoxLogo.Location = new System.Drawing.Point(15, 15);
            this.pictBoxLogo.Name = "pictBoxLogo";
            this.pictBoxLogo.Size = new System.Drawing.Size(40, 40);
            this.pictBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxLogo.TabIndex = 1;
            this.pictBoxLogo.TabStop = false;
            
            // 
            // btnToggleMenu
            // 
            this.btnToggleMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnToggleMenu.FlatAppearance.BorderSize = 0;
            this.btnToggleMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleMenu.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnToggleMenu.ForeColor = System.Drawing.Color.White;
            this.btnToggleMenu.Location = new System.Drawing.Point(220, 25);
            this.btnToggleMenu.Name = "btnToggleMenu";
            this.btnToggleMenu.Size = new System.Drawing.Size(35, 30);
            this.btnToggleMenu.TabIndex = 2;
            this.btnToggleMenu.Text = "☰";
            this.btnToggleMenu.UseVisualStyleBackColor = false;
            this.btnToggleMenu.Click += new System.EventHandler(this.btnToggleMenu_Click);
            
            // 
            // pnMenuContainer
            // 
            this.pnMenuContainer.AutoScroll = true;
            this.pnMenuContainer.Controls.Add(this.btnHome);
            this.pnMenuContainer.Controls.Add(this.btnProdutos);
            this.pnMenuContainer.Controls.Add(this.btnColaboradores);
            this.pnMenuContainer.Controls.Add(this.btnFornecedores);
            this.pnMenuContainer.Controls.Add(this.btnCategorias);
            this.pnMenuContainer.Controls.Add(this.btnDepartamentos);
            this.pnMenuContainer.Controls.Add(this.btnPDV);
            this.pnMenuContainer.Controls.Add(this.btnRelatorios);
            this.pnMenuContainer.Controls.Add(this.btnConfiguracoes);
            this.pnMenuContainer.Controls.Add(this.btnLogout);
            this.pnMenuContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMenuContainer.Location = new System.Drawing.Point(0, 80);
            this.pnMenuContainer.Name = "pnMenuContainer";
            this.pnMenuContainer.Size = new System.Drawing.Size(260, 620);
            this.pnMenuContainer.TabIndex = 1;
            
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(0, 0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnHome.Size = new System.Drawing.Size(260, 50);
            this.btnHome.TabIndex = 0;
            this.btnHome.Text = "🏠 INÍCIO";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            
            // 
            // btnProdutos
            // 
            this.btnProdutos.BackColor = System.Drawing.Color.Transparent;
            this.btnProdutos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProdutos.FlatAppearance.BorderSize = 0;
            this.btnProdutos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProdutos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnProdutos.ForeColor = System.Drawing.Color.White;
            this.btnProdutos.Location = new System.Drawing.Point(0, 50);
            this.btnProdutos.Name = "btnProdutos";
            this.btnProdutos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnProdutos.Size = new System.Drawing.Size(260, 50);
            this.btnProdutos.TabIndex = 1;
            this.btnProdutos.Text = "📦 PRODUTOS";
            this.btnProdutos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProdutos.UseVisualStyleBackColor = false;
            this.btnProdutos.Click += new System.EventHandler(this.btnProdutos_Click);
            
            // 
            // btnColaboradores
            // 
            this.btnColaboradores.BackColor = System.Drawing.Color.Transparent;
            this.btnColaboradores.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnColaboradores.FlatAppearance.BorderSize = 0;
            this.btnColaboradores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColaboradores.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnColaboradores.ForeColor = System.Drawing.Color.White;
            this.btnColaboradores.Location = new System.Drawing.Point(0, 100);
            this.btnColaboradores.Name = "btnColaboradores";
            this.btnColaboradores.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnColaboradores.Size = new System.Drawing.Size(260, 50);
            this.btnColaboradores.TabIndex = 2;
            this.btnColaboradores.Text = "👥 COLABORADORES";
            this.btnColaboradores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnColaboradores.UseVisualStyleBackColor = false;
            this.btnColaboradores.Click += new System.EventHandler(this.btnColaboradores_Click);
            
            // 
            // btnFornecedores
            // 
            this.btnFornecedores.BackColor = System.Drawing.Color.Transparent;
            this.btnFornecedores.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFornecedores.FlatAppearance.BorderSize = 0;
            this.btnFornecedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFornecedores.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFornecedores.ForeColor = System.Drawing.Color.White;
            this.btnFornecedores.Location = new System.Drawing.Point(0, 150);
            this.btnFornecedores.Name = "btnFornecedores";
            this.btnFornecedores.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnFornecedores.Size = new System.Drawing.Size(260, 50);
            this.btnFornecedores.TabIndex = 3;
            this.btnFornecedores.Text = "🏭 FORNECEDORES";
            this.btnFornecedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFornecedores.UseVisualStyleBackColor = false;
            this.btnFornecedores.Click += new System.EventHandler(this.btnFornecedores_Click);
            
            // 
            // btnCategorias
            // 
            this.btnCategorias.BackColor = System.Drawing.Color.Transparent;
            this.btnCategorias.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCategorias.FlatAppearance.BorderSize = 0;
            this.btnCategorias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCategorias.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCategorias.ForeColor = System.Drawing.Color.White;
            this.btnCategorias.Location = new System.Drawing.Point(0, 200);
            this.btnCategorias.Name = "btnCategorias";
            this.btnCategorias.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnCategorias.Size = new System.Drawing.Size(260, 50);
            this.btnCategorias.TabIndex = 4;
            this.btnCategorias.Text = "🏷️ CATEGORIAS";
            this.btnCategorias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCategorias.UseVisualStyleBackColor = false;
            this.btnCategorias.Click += new System.EventHandler(this.btnCategorias_Click);
            
            // 
            // btnDepartamentos
            // 
            this.btnDepartamentos.BackColor = System.Drawing.Color.Transparent;
            this.btnDepartamentos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDepartamentos.FlatAppearance.BorderSize = 0;
            this.btnDepartamentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDepartamentos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDepartamentos.ForeColor = System.Drawing.Color.White;
            this.btnDepartamentos.Location = new System.Drawing.Point(0, 250);
            this.btnDepartamentos.Name = "btnDepartamentos";
            this.btnDepartamentos.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnDepartamentos.Size = new System.Drawing.Size(260, 50);
            this.btnDepartamentos.TabIndex = 5;
            this.btnDepartamentos.Text = "🏢 DEPARTAMENTOS";
            this.btnDepartamentos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDepartamentos.UseVisualStyleBackColor = false;
            this.btnDepartamentos.Click += new System.EventHandler(this.btnDepartamentos_Click);
            
            // 
            // btnPDV
            // 
            this.btnPDV.BackColor = System.Drawing.Color.Transparent;
            this.btnPDV.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPDV.FlatAppearance.BorderSize = 0;
            this.btnPDV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPDV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPDV.ForeColor = System.Drawing.Color.LightGreen;
            this.btnPDV.Location = new System.Drawing.Point(0, 300);
            this.btnPDV.Name = "btnPDV";
            this.btnPDV.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnPDV.Size = new System.Drawing.Size(260, 50);
            this.btnPDV.TabIndex = 6;
            this.btnPDV.Text = "🛒 PONTO DE VENDA";
            this.btnPDV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPDV.UseVisualStyleBackColor = false;
            this.btnPDV.Click += new System.EventHandler(this.btnPDV_Click);
            
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.BackColor = System.Drawing.Color.Transparent;
            this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRelatorios.FlatAppearance.BorderSize = 0;
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRelatorios.ForeColor = System.Drawing.Color.White;
            this.btnRelatorios.Location = new System.Drawing.Point(0, 350);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnRelatorios.Size = new System.Drawing.Size(260, 50);
            this.btnRelatorios.TabIndex = 7;
            this.btnRelatorios.Text = "📊 RELATÓRIOS";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.UseVisualStyleBackColor = false;
            this.btnRelatorios.Click += new System.EventHandler(this.btnRelatorios_Click);
            
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.BackColor = System.Drawing.Color.Transparent;
            this.btnConfiguracoes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConfiguracoes.FlatAppearance.BorderSize = 0;
            this.btnConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfiguracoes.ForeColor = System.Drawing.Color.White;
            this.btnConfiguracoes.Location = new System.Drawing.Point(0, 400);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnConfiguracoes.Size = new System.Drawing.Size(260, 50);
            this.btnConfiguracoes.TabIndex = 8;
            this.btnConfiguracoes.Text = "⚙️ CONFIGURAÇÕES";
            this.btnConfiguracoes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracoes.UseVisualStyleBackColor = false;
            this.btnConfiguracoes.Click += new System.EventHandler(this.btnConfiguracoes_Click);
            
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(0, 570);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(260, 50);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "🚪 SAIR";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            
            // 
            // pnTopInfo
            // 
            this.pnTopInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnTopInfo.Controls.Add(this.iconModuloAtivo);
            this.pnTopInfo.Controls.Add(this.lblModuloAtivo);
            this.pnTopInfo.Controls.Add(this.lblDescricaoModulo);
            this.pnTopInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopInfo.Location = new System.Drawing.Point(260, 60);
            this.pnTopInfo.Name = "pnTopInfo";
            this.pnTopInfo.Size = new System.Drawing.Size(1140, 50);
            this.pnTopInfo.TabIndex = 2;
            
            // 
            // iconModuloAtivo
            // 
            this.iconModuloAtivo.AutoSize = true;
            this.iconModuloAtivo.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.iconModuloAtivo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.iconModuloAtivo.Location = new System.Drawing.Point(20, 8);
            this.iconModuloAtivo.Name = "iconModuloAtivo";
            this.iconModuloAtivo.Size = new System.Drawing.Size(53, 45);
            this.iconModuloAtivo.TabIndex = 0;
            this.iconModuloAtivo.Text = "🏠";
            
            // 
            // lblModuloAtivo
            // 
            this.lblModuloAtivo.AutoSize = true;
            this.lblModuloAtivo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblModuloAtivo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblModuloAtivo.Location = new System.Drawing.Point(80, 8);
            this.lblModuloAtivo.Name = "lblModuloAtivo";
            this.lblModuloAtivo.Size = new System.Drawing.Size(80, 30);
            this.lblModuloAtivo.TabIndex = 1;
            this.lblModuloAtivo.Text = "INÍCIO";
            
            // 
            // lblDescricaoModulo
            // 
            this.lblDescricaoModulo.AutoSize = true;
            this.lblDescricaoModulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescricaoModulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblDescricaoModulo.Location = new System.Drawing.Point(80, 30);
            this.lblDescricaoModulo.Name = "lblDescricaoModulo";
            this.lblDescricaoModulo.Size = new System.Drawing.Size(300, 19);
            this.lblDescricaoModulo.TabIndex = 2;
            this.lblDescricaoModulo.Text = "Página inicial com resumo do sistema";
            
            // 
            // pnForm
            // 
            this.pnForm.BackColor = System.Drawing.Color.White;
            this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnForm.Location = new System.Drawing.Point(260, 110);
            this.pnForm.Name = "pnForm";
            this.pnForm.Padding = new System.Windows.Forms.Padding(10);
            this.pnForm.Size = new System.Drawing.Size(1140, 620);
            this.pnForm.TabIndex = 3;
            
            // 
            // pnStatus
            // 
            this.pnStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnStatus.Controls.Add(this.lblStatusSistema);
            this.pnStatus.Controls.Add(this.lblVersaoSistema);
            this.pnStatus.Controls.Add(this.lblAutor);
            this.pnStatus.Controls.Add(this.progressStatus);
            this.pnStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnStatus.Location = new System.Drawing.Point(0, 730);
            this.pnStatus.Name = "pnStatus";
            this.pnStatus.Size = new System.Drawing.Size(1400, 30);
            this.pnStatus.TabIndex = 4;
            
            // 
            // lblStatusSistema
            // 
            this.lblStatusSistema.AutoSize = true;
            this.lblStatusSistema.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatusSistema.ForeColor = System.Drawing.Color.White;
            this.lblStatusSistema.Location = new System.Drawing.Point(15, 6);
            this.lblStatusSistema.Name = "lblStatusSistema";
            this.lblStatusSistema.Size = new System.Drawing.Size(150, 19);
            this.lblStatusSistema.TabIndex = 0;
            this.lblStatusSistema.Text = "🟢 Sistema operacional";
            
            // 
            // lblVersaoSistema
            // 
            this.lblVersaoSistema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersaoSistema.AutoSize = true;
            this.lblVersaoSistema.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVersaoSistema.ForeColor = System.Drawing.Color.LightGray;
            this.lblVersaoSistema.Location = new System.Drawing.Point(1250, 8);
            this.lblVersaoSistema.Name = "lblVersaoSistema";
            this.lblVersaoSistema.Size = new System.Drawing.Size(140, 15);
            this.lblVersaoSistema.TabIndex = 1;
            this.lblVersaoSistema.Text = "📍 v2.0.0 - Build 2024.08";
            // 
            // lblAutor
            // 
            this.lblAutor.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.lblAutor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAutor.AutoSize = true;
            this.lblAutor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAutor.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblAutor.LinkColor = System.Drawing.Color.LightGray;
            this.lblAutor.Location = new System.Drawing.Point(980, 8);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(252, 15);
            this.lblAutor.TabIndex = 3;
            this.lblAutor.TabStop = true;
            this.lblAutor.Text = "Feito por Heitor Gonçalves · LinkedIn (clique)";
            this.lblAutor.VisitedLinkColor = System.Drawing.Color.Silver;
            this.lblAutor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAutor_LinkClicked);
            
            // 
            // progressStatus
            // 
            this.progressStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressStatus.Location = new System.Drawing.Point(1080, 8);
            this.progressStatus.Name = "progressStatus";
            this.progressStatus.Size = new System.Drawing.Size(150, 15);
            this.progressStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressStatus.TabIndex = 2;
            this.progressStatus.Visible = false;
            
            // 
            // timerRelogio
            // 
            this.timerRelogio.Enabled = true;
            this.timerRelogio.Interval = 1000;
            this.timerRelogio.Tick += new System.EventHandler(this.timerRelogio_Tick);
            
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 760);
            this.Controls.Add(this.pnForm);
            this.Controls.Add(this.pnTopInfo);
            this.Controls.Add(this.pnMenuVertical);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.pnStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "frmMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema PDV - Menu Principal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnMenuVertical.ResumeLayout(false);
            this.pnMenuContainer.ResumeLayout(false);
            this.pnLogo.ResumeLayout(false);
            this.pnLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxLogo)).EndInit();
            this.pnTopInfo.ResumeLayout(false);
            this.pnTopInfo.PerformLayout();
            this.pnStatus.ResumeLayout(false);
            this.pnStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // Header Components
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTituloSistema;
        private System.Windows.Forms.Label lblUsuarioLogado;
        private System.Windows.Forms.Label lblDataHora;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button btnClose;
        
        // Sidebar Components
        private System.Windows.Forms.Panel pnMenuVertical;
        private System.Windows.Forms.Panel pnLogo;
        private System.Windows.Forms.Label lblNomeEmpresa;
        private System.Windows.Forms.PictureBox pictBoxLogo;
        private System.Windows.Forms.Button btnToggleMenu;
        
        private System.Windows.Forms.Panel pnMenuContainer;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnProdutos;
        private System.Windows.Forms.Button btnColaboradores;
        private System.Windows.Forms.Button btnFornecedores;
        private System.Windows.Forms.Button btnCategorias;
        private System.Windows.Forms.Button btnDepartamentos;
        private System.Windows.Forms.Button btnPDV;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.Button btnLogout;
        
        // Content Area Components
        private System.Windows.Forms.Panel pnTopInfo;
        private System.Windows.Forms.Label iconModuloAtivo;
        private System.Windows.Forms.Label lblModuloAtivo;
        private System.Windows.Forms.Label lblDescricaoModulo;
        private System.Windows.Forms.Panel pnForm;
        
        // Status Components
        private System.Windows.Forms.Panel pnStatus;
        private System.Windows.Forms.Label lblStatusSistema;
        private System.Windows.Forms.Label lblVersaoSistema;
    private System.Windows.Forms.LinkLabel lblAutor;
        private System.Windows.Forms.ProgressBar progressStatus;
        
        // Timer
        private System.Windows.Forms.Timer timerRelogio;
    }
}