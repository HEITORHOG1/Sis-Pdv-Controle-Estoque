using Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    public partial class frmCarrinho : Form
    {
        ProdutoPedidoService _produtoPedidoService;

        public frmCarrinho()
        {
            InitializeComponent();
        }
        private void frmCarrinho_Load(object sender, EventArgs e)
        {
            dgvListaProdutos.ClearSelection();
            DefinirCabecalhos(new List<string>() { "Quantidade", "Nome", "Preço", "Total" });


        }
        public async void PreencheListaProdutos(string id)
        {
            _produtoPedidoService = new ProdutoPedidoService();
            var response = await _produtoPedidoService.ListarProdutosPorPedidoId(id);

            dgvListaProdutos.DataSource = response.data;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            var colunas = new Dictionary<string, (string Header, int Width)>
            {
                ["QuantidadeItemPedido"] = ("Quantidade", 90),
                ["NomeProduto"]          = ("Nome",      200),
                ["PrecoVenda"]           = ("Preco",     100),
                ["TotalProdutoPedido"]   = ("Total",     100),
                ["CodBarras"]            = ("Cod. Barras", 120),
            };

            foreach (DataGridViewColumn coluna in dgvListaProdutos.Columns)
            {
                if (colunas.TryGetValue(coluna.Name, out var config))
                {
                    coluna.HeaderText = config.Header;
                    coluna.Width = config.Width;
                    coluna.Visible = true;
                }
                else
                {
                    coluna.Visible = false;
                }
            }
        }
    }
}
