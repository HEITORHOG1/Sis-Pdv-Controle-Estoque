﻿using Microsoft.Extensions.Logging;
using Serilog.Events;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_API.RabbitMQSender;
using System.Data;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Cupom
{
    public partial class frmCupom : Form
    {
        private string _IdPedido;
        private string _Colaborador;
        RabbitMQMessageSender _rabbitMQMessageSender;
        readonly ILogger _logger;

        public frmCupom(string IdPedido,string colaborador)
        {
            InitializeComponent();
            _IdPedido = IdPedido;
            _Colaborador = colaborador;
        }

        public frmCupom()
        {
        }

        List<ProdutoPedidoBase> produtos = new List<ProdutoPedidoBase>();
        public List<string> _Layout = new List<string>();

        public void CumpomImpresso(string codItem, string codBarras, string descricao, string quantidade,
                                    string valorUnit, string Total, string Status, string cpf,
                                    string totalVendido, string data,
                                    string hora, string caixa, string formaPagamento,
                                    string valorRecebido, string troco)
        {
            listBox1.Clear();

            ImprimirCupom(codItem, codBarras, descricao, quantidade, valorUnit, Total, Status, cpf, data, hora, caixa, formaPagamento, valorRecebido, troco, totalVendido);


            CupomDTO cupom = new(codItem, codBarras, descricao, quantidade, valorUnit, Total, Status, cpf, data, hora, caixa, formaPagamento, valorRecebido, troco, totalVendido);

            EnviarCupomFila(cupom);

            foreach (string obj in _Layout)
            {
                listBox1.Items.Add(obj);
            }
        }

        private void EnviarCupomFila(CupomDTO cupom)
        {
            try
            { 
                _rabbitMQMessageSender = new RabbitMQMessageSender();

                _rabbitMQMessageSender.SendMessage(cupom, "CUPOM_QUEUE");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    FecharCupom();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharCupom();
        }

        public void FecharCupom()
        {
            this.Close();
        }

        public void ImprimirCupom(string codItem, string codigoBarras, string Descricao, string quantidade, string valorUnitario,
        string total, string Status, string cpf, string data, string hora, string caixa, string formaPagamento, string valorRecebido,
        string troco, string totalVendido)
        {
            try
            {
                var novaDescricao = "";

                if (Descricao.Length >= 20)
                {
                    novaDescricao = Descricao.Substring(0, 20);
                }
                else
                {
                    novaDescricao = Descricao;
                }

                produtos.Add(new ProdutoPedidoBase(codItem, codigoBarras, novaDescricao, quantidade, valorUnitario, total, Status));

                StreamWriter x;

                string path = "C:\\Recibos\\";
                Directory.CreateDirectory(path);

                string caminho = path + _IdPedido + ".txt";
                x = File.CreateText(caminho);

                foreach (ProdutoPedidoBase obj in produtos)
                {
                    x.WriteLine(obj.codItem + ";" + obj.codigoBarras + ";" + obj.NomeProduto + ";" + obj.quantidade + ";" + obj.valorUnitario + ";" + obj.total + ";" + obj.StatusAtivo + ";");
                }
                x.WriteLine("" + ";" + "" + ";" + "Total R$" + ";" + "" + ";" + "" + ";" + totalVendido + ";" + "" + ";");
                x.WriteLine("" + ";" + "" + ";" + formaPagamento + ";" + "" + ";" + "" + ";" + valorRecebido + ";" + "" + ";");
                if (troco != "0,00")
                {
                    x.WriteLine("" + ";" + "" + ";" + "Troco" + ";" + "" + ";" + "" + ";" + troco + ";" + "" + ";");

                }

                x.Close();

                StringBuilder sb = new StringBuilder();

                string caminhoArquivo = path + _IdPedido + ".txt";

                var consulta = from linha in File.ReadAllLines(caminhoArquivo)
                               let ProdutoDados = linha.Split(';')
                               select new ProdutoPedidoBase()
                               {
                                   codItem = ProdutoDados[0],
                                   codigoBarras = ProdutoDados[1],
                                   NomeProduto = ProdutoDados[2],
                                   quantidade = ProdutoDados[3],
                                   valorUnitario = ProdutoDados[4],
                                   total = ProdutoDados[5],
                                   StatusAtivo = ProdutoDados[6],
                               };


                foreach (var item in consulta)
                {
                    sb.AppendFormat("{0,-3}{1,-15}{2,-23}{3,-4}{4,-7}{5,-28}{6,-30}{7}",
                       item.codItem,
                       item.codigoBarras,
                       item.NomeProduto,
                       item.quantidade,
                       item.valorUnitario,
                       item.total,
                       item.StatusAtivo,

                       Environment.NewLine);
                }
                File.WriteAllText(path + _IdPedido + ".txt", sb.ToString());
                _Layout.Clear();

                using (StreamReader reader = new StreamReader(path + _IdPedido + ".txt"))
                {
                    _Layout.Add("");
                    _Layout.Add(" HEITOR OLIVEIRA GONÇALVES - LTDA ");
                    _Layout.Add(" Rua Professor joão de Deus n° 908");
                    _Layout.Add(" Petrópolis-RJ");
                    _Layout.Add(" -----------------------------------------------------------");
                    _Layout.Add(" CNPJ: 71.564.173/0001-80                         " + data);
                    _Layout.Add(" IE: 714.145.789                                  " + hora);
                    _Layout.Add(" IM: 4567412                     ");
                    _Layout.Add(" -----------------------------------------------------------");
                    _Layout.Add(" CODIGO:" + _IdPedido);

                    _Layout.Add(" CPF/CNPJ:" + cpf);

                    _Layout.Add(" -----------------------------------------------------------");
                    _Layout.Add(" ------------------------CUPOM FISCAL-----------------------");
                    _Layout.Add(" -----------------------------------------------------------");

                    _Layout.Add("COD CDB            DESC.                 QTDE   UN   VALOR");

                    _Layout.Add("");


                    List<string> Todos = new List<string>();
                    List<string> cancelado = new List<string>();
                    List<string> totalVenda = new List<string>();

                    string line;


                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Cancelado"))
                        {
                            string linhaAlterada;
                            linhaAlterada = line.Replace("Cancelado", "");
                            cancelado.Add(linhaAlterada);
                            Todos.Add(linhaAlterada);
                        }
                        else if (line.Contains("Total"))
                        {
                            totalVenda.Add(line);
                        }
                        else if (line.Contains(formaPagamento))
                        {
                            totalVenda.Add(line);
                        }
                        else if (line.Contains("Troco"))
                        {
                            totalVenda.Add(line);
                        }
                        else
                        {
                            string linhaAlterada;
                            linhaAlterada = line.Replace("Cancelado", "").Replace("Ativo", "");
                            Todos.Add(linhaAlterada);
                        }
                    }

                    foreach (string obj in Todos)
                    {
                        _Layout.Add(" " + obj);
                    }
                    bool vericadora = false;


                    foreach (string obj in cancelado)
                    {
                        if (vericadora == false)
                        {
                            _Layout.Add(" -----------------------------------------------------------");
                            _Layout.Add(" ------------------------CANCELADO--------------------------");
                            _Layout.Add(" -----------------------------------------------------------");
                            vericadora = true;

                        }
                        _Layout.Add(" " + obj);
                    }
                    _Layout.Add(" -----------------------------------------------------------");

                    foreach (string obj in totalVenda)
                    {
                        _Layout.Add(" " + obj);
                    }

                    _Layout.Add(" -----------------------------------------------------------");
                    _Layout.Add(" -----------------------------------------------------------");

                    _Layout.Add(" CAIXA:" + caixa);
                    _Layout.Add(" COLABORAR:" + _Colaborador);
                    _Layout.Add(" PDVR 2.0.3");
                    _Layout.Add(" BEMATECH MP -2100");
                    _Layout.Add(" -----------------------------------------------------------");


                }
                x = File.CreateText(caminho);


                foreach (string obj in _Layout)
                {
                    x.WriteLine(obj);
                }

                x.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

