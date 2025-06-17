using Sis_Pdv_Controle_Estoque_Form.Dto.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Cupom;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Dinheiro;
using Sis_Pdv_Controle_Estoque_Form.Paginas.Login;
using Sis_Pdv_Controle_Estoque_Form.Services.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Services.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Services.Produto;
using Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.PDV
{
    public partial class frmTelaPdv : Form
    {
        PedidoService _pedidoService;
        ClienteService _clienteService;
        ColaboradorService _colaboradorService;
        ProdutoService _produtoService;
        ProdutoPedidoService _produtoPedidoService;

        bool verificadorTecla = false;
        string codItem;
        bool i = false;
        readonly string cpfCnpjCliente;
        bool verificador = false;
        int codigo = 1;
        string codBarras;
        string quantidade;
        string totalProduto;
        string preco;
        string descricao;
        readonly string Nome;
        ColaboradorResponseList colaboradorResponseList = new ColaboradorResponseList();
        public frmTelaPdv(string nome)
        {
            InitializeComponent();
            Nome = nome;
        }
        private async void frmTelaPdv_Load(object sender, EventArgs e)
        {
            _colaboradorService = new ColaboradorService();
            colaboradorResponseList = await _colaboradorService.ListarColaboradorPorNomeColaborador(Nome);

            if (colaboradorResponseList != null && colaboradorResponseList.data != null)
            {
                foreach (var item in colaboradorResponseList.data)
                {
                    lblNomeOperador.Text = item.nomeColaborador;
                }
            }

            timerData.Start();
        }

        private async void txbCodBarras_TextChanged(object sender, EventArgs e)
        {
            _produtoService = new ProdutoService();
            string descricao = "";
            string barras = "";
            if (txbCodBarras.Text != "")
            {
                var response = await _produtoService.ListarProdutoPorCodBarras(txbCodBarras.Text);
                if (response.success != false)
                {
                    foreach (var item in response.data)
                    {
                        descricao = item.descricaoProduto;
                        barras = item.codBarras;
                        txbDescricao.Text = item.descricaoProduto;
                        txbPrecoUnit.Text = item.precoVenda.ToString();
                    }

                    await CalculoValorQauntidade();
                    Adicionar(barras, descricao);
                    await CalculaTotal();
                    await LimpaCampos();
                }
                else
                {
                    var resp = response.notifications.FirstOrDefault();
                    MessageBox.Show(resp.ToString());
                }
            }
        }
        private void txbQuantidade_TextChanged(object sender, EventArgs e)
        {
            //CalculoValorQauntidade();
        }
        private async Task CalculoValorQauntidade()
        {
            decimal quantidade = 1;
            decimal preco = 0;
            decimal total = 0;
            if (txbPrecoUnit.Text != "")
            {
                quantidade = 1;//decimal.Parse(txbQuantidade.Text);
                preco = decimal.Parse(txbPrecoUnit.Text);
            }
            total = quantidade * preco;
            txbTotalRecebido.Text = total.ToString();
            txbCodBarras.Text = "";
        }
        private void txbCodBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }

        }
        private void timerData_Tick(object sender, EventArgs e)
        {
            lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void txbQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    FinalizarVendas();

                    break;
                case Keys.D:
                    PagamentoDinheiro();

                    break;
                case Keys.C:
                    PagamentoCartao();
                    break;

                case Keys.I:
                    if (lblTotal.Text != "0")
                    {
                        CancelarItem();
                    }
                    break;
                case Keys.F8:
                    CancelarVenda();

                    break;
                case Keys.F5:

                    LimpaCampos();
                    break;

                case Keys.Escape:
                    frmCupom cupom = new frmCupom();
                    cupom.FecharCupom();
                    break;

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void CancelarVenda()
        {
            const string message = "Deseja cancelar a venda?";
            const string caption = "Atenção";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                LimpaDgv();
                txbCodBarras.Focus();

            }
        }
        private void PagamentoCartao()
        {
            if (lblTotal.Text != "0")
            {
                lblFormaPagamento.Text = "Cartão";
                lblTroco.Text = "0,00";
                lblValorAReceber.Text = lblTotal.Text;
                lblNomeFormaPagamento.Visible = true;
                lblFormaPagamento.Visible = true;
                lblNomeTroco.Visible = true;
                lblTroco.Visible = true;
                lblNomeValorPago.Visible = true;
                lblValorAReceber.Visible = true;

                verificadorTecla = true;
            }
            else
            {
                MessageBox.Show("Por favor insira um produto!!");
            }
        }
        private void PagamentoDinheiro()
        {
            if (lblTotal.Text != "0")
            {
                using (var dinheiro = new frmDinheiro())
                {

                    dinheiro.ReceberValor(lblTotal.Text);
                    dinheiro.ShowDialog();
                    lblValorAReceber.Text = dinheiro.ValorRecibido;
                    lblTroco.Text = dinheiro.Troco;

                }

                if (lblValorAReceber.Text != "0,00")
                {
                    lblFormaPagamento.Text = "Dinheiro";
                    lblNomeFormaPagamento.Visible = true;
                    lblFormaPagamento.Visible = true;
                    lblNomeTroco.Visible = true;
                    lblTroco.Visible = true;
                    lblNomeValorPago.Visible = true;
                    lblValorAReceber.Visible = true;
                }

                // controllerPedido.AtualizaFormaPagamento(btnDinheiro.Text);
                verificadorTecla = true;
            }
            else
            {
                MessageBox.Show("Por favor insira um produto!!");
            }
        }
        private void FinalizarVendas()
        {
            decimal valorTotal;
            decimal valorRecibido;
            if (verificadorTecla == true)
            {
                valorTotal = decimal.Parse(lblTotal.Text);
                valorRecibido = decimal.Parse(lblValorAReceber.Text);
                if (valorRecibido >= valorTotal)
                {
                    FinalizarVenda();

                    lblNomeFormaPagamento.Visible = false;
                    lblFormaPagamento.Visible = false;
                    lblNomeTroco.Visible = false;
                    lblTroco.Visible = false;
                    lblNomeValorPago.Visible = false;
                    lblValorAReceber.Visible = false;
                }
                else
                {
                    MessageBox.Show("Valor rebido menor que o total");
                }

            }
            else
            {
                MessageBox.Show("Por favor selecione a forma de pagamento!");
            }
        }
        private async void FinalizarVenda()
        {
            _pedidoService = new PedidoService();
            _produtoPedidoService = new ProdutoPedidoService();
            _clienteService = new ClienteService();


            ClienteDto clienteDto = new ClienteDto();

            if (cpfCnpjCliente != null && cpfCnpjCliente != "")
            {
                ClienteDto Dto = new ClienteDto
                {
                    CpfCnpj = cpfCnpjCliente,

                };
                clienteDto = await _clienteService.Adicionar(Dto);

            }


            int cont = 0;
            int idex = 0;
            int codItem = 1;
            PedidoDto pedidoDto = new PedidoDto
            {
                ClienteId = clienteDto.id,
                ColaboradorId = Guid.Parse(colaboradorResponseList.data[0].id),
                dataDoPedido = Convert.ToDateTime(lblData.Text),
                totalPedido = Convert.ToDecimal(lblTotal.Text),
                formaPagamento = lblFormaPagamento.Text
            };

            var response = await _pedidoService.AdicionarPedido(pedidoDto);
            // var responsesProdutoPedido = await _produtoPedidoService.ListarProdutoPedidoPorId(response.data.Id.ToString());


            frmCupom cupom = new frmCupom(response.data.Id.ToString(), lblNomeOperador.Text);


            foreach (DataGridViewRow coluna in dgvCarrinho.Rows)
            {
                if (coluna.Visible)
                {
                    if (coluna.DefaultCellStyle.BackColor == Color.Red)
                    {
                        string cancelado = "Cancelado";
                        codBarras = this.dgvCarrinho.Rows[idex].Cells[1].Value.ToString();
                        descricao = this.dgvCarrinho.Rows[idex].Cells[2].Value.ToString();
                        preco = this.dgvCarrinho.Rows[idex].Cells[3].Value.ToString();
                        quantidade = this.dgvCarrinho.Rows[idex].Cells[4].Value.ToString();
                        totalProduto = this.dgvCarrinho.Rows[idex].Cells[5].Value.ToString();
                        cupom.CumpomImpresso(codItem.ToString(), codBarras, descricao, quantidade, preco, totalProduto, cancelado, cpfCnpjCliente, lblTotal.Text, lblData.Text, lblHora.Text, lblCaixa.Text, lblFormaPagamento.Text, lblValorAReceber.Text, lblTroco.Text);


                        cont++;
                    }
                    else
                    {
                        string ativo = "Ativo";
                        if (dgvCarrinho.Rows[idex].Cells[cont].Value != null)
                        {
                            var responseProduto = await _produtoService.ListarProdutoPorCodBarras(this.dgvCarrinho.Rows[idex].Cells[1].Value.ToString());
                            codBarras = this.dgvCarrinho.Rows[idex].Cells[1].Value.ToString();
                            descricao = this.dgvCarrinho.Rows[idex].Cells[2].Value.ToString();
                            preco = this.dgvCarrinho.Rows[idex].Cells[3].Value.ToString();
                            quantidade = this.dgvCarrinho.Rows[idex].Cells[4].Value.ToString();
                            totalProduto = this.dgvCarrinho.Rows[idex].Cells[5].Value.ToString();

                            ProdutoPedidoDto produtoPedidoDto = new()
                            {
                                codBarras = codBarras,
                                ProdutoId = responseProduto.data[0].Id,
                                PedidoId = response.data.Id,
                                quantidadeItemPedido = Convert.ToInt32(quantidade),
                                totalProdutoPedido = Convert.ToDecimal(totalProduto)
                            };
                            await _produtoPedidoService.AdicionarProdutoPedido(produtoPedidoDto);

                            ProdutoDto produtoAtualizaEstoque = new ProdutoDto
                            {
                                Id = responseProduto.data[0].Id,
                                quatidadeEstoqueProduto = Convert.ToInt32(quantidade)
                            };

                            await _produtoService.AtualizarEstoque(produtoAtualizaEstoque);

                            cupom.CumpomImpresso(codItem.ToString(), codBarras, descricao,
                                                quantidade, preco, totalProduto,
                                                ativo, cpfCnpjCliente, lblTotal.Text,
                                                lblData.Text, lblHora.Text, lblCaixa.Text,
                                                lblFormaPagamento.Text, lblValorAReceber.Text, lblTroco.Text);
                        }
                    }
                    codItem++;

                    idex++;

                }

            }

            cupom.ShowDialog();
            lblNomeCaixa.Text = "CAIXA LIVRE";
            LimpaDgv();
            i = false;
            txbDescricao.Clear();
        }
        private void LimpaDgv()
        {
            i = false;
            verificador = false;
            codigo = 1;
            dgvCarrinho.Rows.Clear();
            dgvCarrinho.Columns.Clear();
            lblTotal.Text = "0";
        }
        private void Adicionar(string codigoBarras, string descricao)
        {
            if (i == false)
            {
                dgvCarrinho.Columns.Add("codVenda", "Cód.");
                dgvCarrinho.Columns.Add("CodBarras", "Cod. barras");
                dgvCarrinho.Columns.Add("Nome", "Produto");
                dgvCarrinho.Columns.Add("ProdutoVenda", "P. Venda");
                dgvCarrinho.Columns.Add("Quantidade", "Quant.");
                dgvCarrinho.Columns.Add("Total", "Total");
                i = true;
                verificador = true;
            }

            decimal valorprecomercadoria = decimal.Parse(txbPrecoUnit.Text);
            decimal valorTotalRecebido = decimal.Parse(txbPrecoUnit.Text);

            string outputDecimal = valorprecomercadoria.ToString("#,##0.00");
            string outputDecimalTotalRecebido = valorTotalRecebido.ToString("#,##0.00");

            dgvCarrinho.Rows.Add(
                                    codigo,
                                    codigoBarras,
                                    descricao,
                                    outputDecimal,
                                    1,
                                    outputDecimalTotalRecebido);
            codigo++;
            dgvCarrinho.ClearSelection();
            lblNomeCaixa.Text = "PROCESSANDO";
        }
        private async Task CalculaTotal()
        {
            int idex = 0;


            decimal calc = 0;


            foreach (DataGridViewRow coluna in dgvCarrinho.Rows)
            {

                if (coluna.Visible)
                {
                    if (coluna.DefaultCellStyle.BackColor == Color.Red)
                    {

                    }
                    else
                    {
                        calc += Convert.ToDecimal(dgvCarrinho.Rows[idex].Cells[5].Value);


                    }
                    lblTotal.Text = calc.ToString();


                    idex++;

                }

            }

        }
        private async Task LimpaCampos()
        {
            txbCodBarras.Clear();
            txbPrecoUnit.Clear();
            //txbDescricao.Clear();
            txbQuantidade.Clear();
            txbTotalRecebido.Clear();
            txbCodBarras.Focus();
        }
        private void CancelarItem()
        {
            int idex = 0;

            using (var verificaLogin = new frmVerificaLogin())
            {

                verificaLogin.ShowDialog();

                if (verificaLogin.Validador == true)
                {
                    using (var cancelarItem = new frmCancelarItem())
                    {

                        cancelarItem.ShowDialog();
                        codItem = cancelarItem.Parametro;

                    }

                }

            }

            if (codItem != "")
            {
                foreach (DataGridViewRow linha in dgvCarrinho.Rows)
                {

                    if (linha.Visible)
                    {
                        if (codItem == dgvCarrinho.Rows[idex].Cells[0].Value.ToString())
                        {
                            dgvCarrinho.Rows[idex].DefaultCellStyle.BackColor = Color.Red;

                        }

                    }
                    idex++;
                }
                CalculaTotal();
            }

        }
        private bool VerificaVazio()
        {
            if (txbCodBarras.Text == "" || txbDescricao.Text == "" || txbPrecoUnit.Text == "" || txbQuantidade.Text == "" || txbTotalRecebido.Text == "")
            {
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
