using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    public partial class frnRelatorioVendasPorData : Form
    {
        PedidoService _pedidoService;
        public frnRelatorioVendasPorData()
        {
            InitializeComponent();
        }
        public List<string> listaCarrinho;

        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            _pedidoService = new PedidoService();

            var response = await _pedidoService.ListarVendaPedidoPorData(Convert.ToDateTime(dtpDataInicial.Text), Convert.ToDateTime(dtpDataFinal.Text));

            dgvRelatorio.DataSource = response.data;
            DefinirCabecalhos(new List<string>() { "Data ", "Pagamento",  "Total" ,"ID" });

            decimal valor = 0;

            for (int i = 0; i < dgvRelatorio.Rows.Count; i++)
            {
                valor += Convert.ToInt32(dgvRelatorio.Rows[i].Cells[2].Value);

            }
            lblTotal.Text = valor.ToString("F2");


        }
        private void DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in dgvRelatorio.Columns)
            {
                if (coluna.Visible)
                {
                    coluna.HeaderText = ListaCabecalhos[_index];
                    _index++;
                }
            }
        }

        private void dgvRelatorio_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string id;

            using (var carrinho = new frmCarrinho())
            {

                id = dgvRelatorio.CurrentRow.Cells[3].Value.ToString();
                carrinho.PreencheListaProdutos(id);


                carrinho.ShowDialog();

            }

        }
    }
}
