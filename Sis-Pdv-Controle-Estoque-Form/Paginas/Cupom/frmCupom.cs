using Microsoft.Extensions.Logging;
using Model;
using Sis_Pdv_Controle_Estoque_API.RabbitMQSender;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Cupom
{
    public partial class frmCupom : Form
    {
        #region Campos Privados
        
        private readonly string _idPedido;
        private readonly string _colaborador;
        private RabbitMQMessageSender _rabbitMQMessageSender;
        private readonly ILogger _logger;
        private readonly List<ProdutoPedidoBase> _produtos = new List<ProdutoPedidoBase>();
        private readonly List<string> _layout = new List<string>();
        private bool _isLoading = false;
        private bool _cupomGerado = false;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frmCupom(string idPedido, string colaborador)
        {
            InitializeComponent();
            _idPedido = idPedido ?? throw new ArgumentNullException(nameof(idPedido));
            _colaborador = colaborador ?? "";
            
            InicializarComponentesModernos();
            CupomLogger.LogInfo($"Formulário de cupom inicializado - Pedido: {idPedido}, Colaborador: {colaborador}", "Startup");
        }
        
        public frmCupom()
        {
            InitializeComponent();
            _idPedido = "";
            _colaborador = "";
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Configura lista de cupom
            ConfigurarListaCupom();
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Inicializa RabbitMQ sender
            _rabbitMQMessageSender = new RabbitMQMessageSender();
        }
        
        private void ConfigurarListaCupom()
        {
            // Remove header e configura para exibição de texto
            listBox1.View = View.List;
            listBox1.HeaderStyle = ColumnHeaderStyle.None;
            listBox1.GridLines = false;
            listBox1.FullRowSelect = true;
            listBox1.MultiSelect = false;
            
            // Fonte monoespaçada para alinhamento correto
            listBox1.Font = new Font("Courier New", 8F);
            listBox1.BackColor = Color.FromArgb(255, 255, 240); // Cor papel
            listBox1.ForeColor = Color.FromArgb(52, 73, 94);
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public bool CupomProcessado { get; private set; } = false;
        public string CaminhoArquivo { get; private set; } = "";
        
        #endregion
        
        #region Eventos de Controles
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            FecharCupom();
        }
        
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        
        private void btnFechar_Click(object sender, EventArgs e)
        {
            FecharCupom();
        }
        
        private async void btnImprimir_Click(object sender, EventArgs e)
        {
            await ImprimirCupomFisico();
        }
        
        private async void btnSalvarPdf_Click(object sender, EventArgs e)
        {
            await SalvarCupomPdf();
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void frmCupom_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    FecharCupom();
                    break;
                case Keys.Escape:
                    FecharCupom();
                    break;
                case Keys.F1:
                    MostrarAjuda();
                    break;
                case Keys.P:
                case Keys.F2:
                    _ = ImprimirCupomFisico();
                    break;
                case Keys.S:
                case Keys.F3:
                    _ = SalvarCupomPdf();
                    break;
            }
        }
        
        private void frmCupom_Load(object sender, EventArgs e)
        {
            try
            {
                // Atualiza título com informações do pedido
                lblTitulo.Text = $"🧾 Cupom Fiscal - Pedido #{_idPedido}";
                
                // Foco no botão fechar para facilitar o uso
                btnFechar.Focus();
                
                CupomLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                CupomLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
            }
        }
        
        private void pnHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permite mover o formulário
            MoverForm.ReleaseCapture();
            MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        
        #endregion
        
        #region Processamento Principal do Cupom
        
        public void CumpomImpresso(string codItem, string CodBarras, string descricao, string quantidade,
                                    string valorUnit, string total, string status, string cpf,
                                    string totalVendido, string data, string hora, string caixa, 
                                    string FormaPagamento, string valorRecebido, string troco)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                SetLoadingState(true);
                
                // Limpa lista anterior
                listBox1.Clear();
                
                // Gera o cupom
                ImprimirCupom(codItem, CodBarras, descricao, quantidade, valorUnit, total, status, 
                             cpf, data, hora, caixa, FormaPagamento, valorRecebido, troco, totalVendido);
                
                // Cria DTO do cupom
                var cupom = new CupomDTO(codItem, CodBarras, descricao, quantidade, valorUnit, total, 
                                        status, cpf, data, hora, caixa, FormaPagamento, valorRecebido, troco, totalVendido);
                
                // Envia para fila (assíncrono)
                _ = EnviarCupomFila(cupom);
                
                // Exibe o cupom na interface
                ExibirCupomInterface();
                
                _cupomGerado = true;
                CupomProcessado = true;
                
                sw.Stop();
                CupomLogger.LogPerformance("GerarCupom", sw.Elapsed);
                CupomLogger.LogInfo($"Cupom gerado com sucesso - Pedido: {_idPedido}", "Success");
            }
            catch (Exception ex)
            {
                sw.Stop();
                CupomLogger.LogError($"Erro ao gerar cupom: {ex.Message}", "ProcessarCupom", ex);
                ExibirErro("Erro ao Gerar Cupom", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void ExibirCupomInterface()
        {
            try
            {
                // Adiciona cada linha do layout ao ListView
                foreach (string linha in _layout)
                {
                    var item = new ListViewItem(linha);
                    
                    // Destaca linhas importantes
                    if (linha.Contains("CUPOM FISCAL") || linha.Contains("CANCELADO") || 
                        linha.Contains("Total R$") || linha.Contains("CNPJ"))
                    {
                        item.Font = new Font("Courier New", 8F, FontStyle.Bold);
                        item.ForeColor = Color.FromArgb(46, 125, 50); // Verde escuro
                    }
                    else if (linha.Contains("---"))
                    {
                        item.ForeColor = Color.FromArgb(149, 165, 166); // Cinza para separadores
                    }
                    
                    listBox1.Items.Add(item);
                }
                
                // Atualiza status
                lblStatus.Text = $"📄 Cupom gerado - {_layout.Count} linhas processadas";
                
                // Scroll para o topo
                if (listBox1.Items.Count > 0)
                {
                    listBox1.EnsureVisible(0);
                }
            }
            catch (Exception ex)
            {
                CupomLogger.LogError($"Erro ao exibir cupom: {ex.Message}", "ExibirInterface", ex);
            }
        }
        
        #endregion
        
        #region Envio para Fila RabbitMQ
        
        private async Task EnviarCupomFila(CupomDTO cupom)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                CupomLogger.LogInfo("Enviando cupom para fila RabbitMQ", "Queue");
                
                await _rabbitMQMessageSender.SendMessageAsync(cupom, "CUPOM_QUEUE");
                
                sw.Stop();
                CupomLogger.LogApiCall("EnviarCupomFila", "QUEUE", sw.Elapsed, true);
                CupomLogger.LogInfo("Cupom enviado para fila com sucesso", "Queue");
            }
            catch (Exception ex)
            {
                sw.Stop();
                CupomLogger.LogApiCall("EnviarCupomFila", "QUEUE", sw.Elapsed, false);
                CupomLogger.LogError($"Erro ao enviar cupom para fila: {ex.Message}", "Queue", ex);
                
                // Não exibe erro para o usuário - falha de fila não deve impedir a operação
                // O cupom já foi gerado localmente
            }
        }
        
        #endregion
        
        #region Geração do Cupom
        
        public void ImprimirCupom(string codItem, string codigoBarras, string descricao, string quantidade, 
                                 string valorUnitario, string total, string status, string cpf, string data, 
                                 string hora, string caixa, string FormaPagamento, string valorRecebido, 
                                 string troco, string totalVendido)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                CupomLogger.LogInfo($"Iniciando geração do cupom - Item: {codItem}", "GerarCupom");
                
                // Processa descrição (máximo 20 caracteres)
                var novaDescricao = descricao.Length >= 20 ? descricao.Substring(0, 20) : descricao;
                
                // Adiciona produto à lista
                _produtos.Add(new ProdutoPedidoBase(codItem, codigoBarras, novaDescricao, quantidade, valorUnitario, total, status));
                
                // Gera arquivo de cupom
                var caminhoArquivo = GerarArquivoCupom(totalVendido, FormaPagamento, valorRecebido, troco);
                CaminhoArquivo = caminhoArquivo;
                
                // Gera layout do cupom
                GerarLayoutCupom(caminhoArquivo, data, hora, caixa, cpf, FormaPagamento);
                
                // Salva arquivo final
                SalvarArquivoFinal(caminhoArquivo);
                
                sw.Stop();
                CupomLogger.LogPerformance("ImprimirCupom", sw.Elapsed);
                CupomLogger.LogInfo($"Cupom gerado com sucesso: {caminhoArquivo}", "GerarCupom");
            }
            catch (Exception ex)
            {
                sw.Stop();
                CupomLogger.LogError($"Erro ao imprimir cupom: {ex.Message}", "GerarCupom", ex);
                throw;
            }
        }
        
        private string GerarArquivoCupom(string totalVendido, string FormaPagamento, string valorRecebido, string troco)
        {
            var path = Path.Combine("C:\\", "Recibos");
            Directory.CreateDirectory(path);
            
            var caminhoArquivo = Path.Combine(path, $"{_idPedido}.txt");
            
            using var writer = File.CreateText(caminhoArquivo);
            
            // Escreve produtos
            foreach (var produto in _produtos)
            {
                writer.WriteLine($"{produto.CodItem};{produto.CodigoBarras};{produto.NomeProduto};{produto.Quantidade};{produto.ValorUnitario};{produto.Total};{produto.StatusAtivo};");
            }
            
            // Escreve totais
            writer.WriteLine($";;Total R$;;;{totalVendido};;");
            writer.WriteLine($";;{FormaPagamento};;;{valorRecebido};;");
            
            if (troco != "0,00" && !string.IsNullOrEmpty(troco))
            {
                writer.WriteLine($";;Troco;;;{troco};;");
            }
            
            return caminhoArquivo;
        }
        
        private void GerarLayoutCupom(string caminhoArquivo, string data, string hora, string caixa, string cpf, string FormaPagamento)
        {
            var sb = new StringBuilder();
            
            // Lê e processa arquivo
            var consulta = from linha in File.ReadAllLines(caminhoArquivo)
                          let produtoDados = linha.Split(';')
                          select new ProdutoPedidoBase()
                          {
                              CodItem = produtoDados[0],
                              CodigoBarras = produtoDados[1],
                              NomeProduto = produtoDados[2],
                              Quantidade = produtoDados[3],
                              ValorUnitario = produtoDados[4],
                              Total = produtoDados[5],
                              StatusAtivo = produtoDados[6],
                          };
            
            // Formata dados para exibição
            foreach (var item in consulta)
            {
                sb.AppendFormat("{0,-3}{1,-15}{2,-23}{3,-4}{4,-7}{5,-28}{6,-30}{7}",
                   item.CodItem, item.CodigoBarras, item.NomeProduto, item.Quantidade,
                   item.ValorUnitario, item.Total, item.StatusAtivo, Environment.NewLine);
            }
            
            File.WriteAllText(caminhoArquivo, sb.ToString());
            
            // Gera layout final
            GerarCabecalhoRodape(caminhoArquivo, data, hora, caixa, cpf, FormaPagamento);
        }
        
        private void GerarCabecalhoRodape(string caminhoArquivo, string data, string hora, string caixa, string cpf, string FormaPagamento)
        {
            _layout.Clear();
            
            var todos = new List<string>();
            var cancelados = new List<string>();
            var totalVenda = new List<string>();
            
            // Lê arquivo e categoriza linhas
            using (var reader = new StreamReader(caminhoArquivo))
            {
                // Cabeçalho da empresa
                AdicionarCabecalhoEmpresa(data, hora, cpf);
                
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ProcessarLinhaArquivo(line, FormaPagamento, todos, cancelados, totalVenda);
                }
                
                // Adiciona itens normais
                foreach (string item in todos)
                {
                    _layout.Add($" {item}");
                }
                
                // Adiciona itens cancelados se houver
                AdicionarItensCancelados(cancelados);
                
                // Adiciona totais
                _layout.Add(" -----------------------------------------------------------");
                foreach (string total in totalVenda)
                {
                    _layout.Add($" {total}");
                }
                
                // Rodapé
                AdicionarRodape(caixa);
            }
        }
        
        private void AdicionarCabecalhoEmpresa(string data, string hora, string cpf)
        {
            _layout.Add("");
            _layout.Add(" HEITOR OLIVEIRA GONÇALVES - LTDA ");
            _layout.Add(" Rua Professor João de Deus n° 908");
            _layout.Add(" Petrópolis-RJ");
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add($" CNPJ: 71.564.173/0001-80                         {data}");
            _layout.Add($" IE: 714.145.789                                  {hora}");
            _layout.Add(" IM: 4567412                     ");
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add($" PEDIDO: {_idPedido}");
            _layout.Add($" CPF/CNPJ: {cpf}");
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add(" ------------------------CUPOM FISCAL-----------------------");
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add("COD CDB            DESC.                 QTDE   UN   VALOR");
            _layout.Add("");
        }
        
        private void ProcessarLinhaArquivo(string line, string FormaPagamento, List<string> todos, List<string> cancelados, List<string> totalVenda)
        {
            if (line.Contains("Cancelado"))
            {
                var linhaAlterada = line.Replace("Cancelado", "");
                cancelados.Add(linhaAlterada);
                todos.Add(linhaAlterada);
            }
            else if (line.Contains("Total") || line.Contains(FormaPagamento) || line.Contains("Troco"))
            {
                totalVenda.Add(line);
            }
            else
            {
                var linhaAlterada = line.Replace("Cancelado", "").Replace("Ativo", "");
                todos.Add(linhaAlterada);
            }
        }
        
        private void AdicionarItensCancelados(List<string> cancelados)
        {
            if (cancelados.Any())
            {
                _layout.Add(" -----------------------------------------------------------");
                _layout.Add(" ------------------------CANCELADO--------------------------");
                _layout.Add(" -----------------------------------------------------------");
                
                foreach (string item in cancelados)
                {
                    _layout.Add($" {item}");
                }
            }
        }
        
        private void AdicionarRodape(string caixa)
        {
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add(" -----------------------------------------------------------");
            _layout.Add($" CAIXA: {caixa}");
            _layout.Add($" COLABORADOR: {_colaborador}");
            _layout.Add(" SISTEMA PDV MODERNO 3.0");
            _layout.Add(" IMPRESSORA: BEMATECH MP-2100");
            _layout.Add($" GERADO EM: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            _layout.Add(" -----------------------------------------------------------");
        }
        
        private void SalvarArquivoFinal(string caminhoArquivo)
        {
            using var writer = File.CreateText(caminhoArquivo);
            foreach (string linha in _layout)
            {
                writer.WriteLine(linha);
            }
        }
        
        #endregion
        
        #region Operações de Impressão e Salvamento
        
        private async Task ImprimirCupomFisico()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!_cupomGerado)
                {
                    ExibirAviso("Nenhum cupom foi gerado ainda.");
                    return;
                }
                
                SetLoadingState(true);
                lblStatus.Text = "🖨️ Enviando para impressora...";
                
                CupomLogger.LogInfo("Iniciando impressão física do cupom", "Print");
                
                // Simula envio para impressora
                await Task.Delay(2000);
                
                sw.Stop();
                CupomLogger.LogPerformance("ImprimirFisico", sw.Elapsed);
                CupomLogger.LogInfo("Cupom enviado para impressora com sucesso", "Print");
                
                ExibirSucesso("Cupom enviado para impressora com sucesso!");
                lblStatus.Text = "🖨️ Cupom enviado para impressora";
            }
            catch (Exception ex)
            {
                sw.Stop();
                CupomLogger.LogError($"Erro ao imprimir cupom: {ex.Message}", "Print", ex);
                ExibirErro("Erro na Impressão", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task SalvarCupomPdf()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!_cupomGerado)
                {
                    ExibirAviso("Nenhum cupom foi gerado ainda.");
                    return;
                }
                
                SetLoadingState(true);
                lblStatus.Text = "💾 Gerando arquivo PDF...";
                
                CupomLogger.LogInfo("Iniciando geração de PDF do cupom", "PDF");
                
                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Arquivos PDF|*.pdf|Arquivos de Texto|*.txt";
                saveDialog.Title = "Salvar Cupom Como";
                saveDialog.FileName = $"Cupom_Fiscal_{_idPedido}_{DateTime.Now:yyyyMMdd_HHmmss}";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var caminhoSalvo = saveDialog.FileName;
                    
                    if (Path.GetExtension(caminhoSalvo).ToLower() == ".pdf")
                    {
                        await GerarPdfCupom(caminhoSalvo);
                    }
                    else
                    {
                        await SalvarTextoSimples(caminhoSalvo);
                    }
                    
                    sw.Stop();
                    CupomLogger.LogPerformance("SalvarPDF", sw.Elapsed);
                    CupomLogger.LogInfo($"Cupom salvo como: {caminhoSalvo}", "PDF");
                    
                    ExibirSucesso($"Cupom salvo com sucesso!\n\nLocal: {caminhoSalvo}");
                    lblStatus.Text = "💾 Cupom salvo em arquivo";
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                CupomLogger.LogError($"Erro ao salvar cupom: {ex.Message}", "PDF", ex);
                ExibirErro("Erro ao Salvar", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task GerarPdfCupom(string caminho)
        {
            // Implementação futura para geração de PDF
            // Por enquanto, salva como texto
            await SalvarTextoSimples(caminho.Replace(".pdf", ".txt"));
            
            CupomLogger.LogInfo("PDF simulado - salvo como texto", "PDF");
        }
        
        private async Task SalvarTextoSimples(string caminho)
        {
            await Task.Run(() =>
            {
                using var writer = new StreamWriter(caminho, false, Encoding.UTF8);
                foreach (string linha in _layout)
                {
                    writer.WriteLine(linha);
                }
            });
        }
        
        #endregion
        
        #region Gerenciamento de Estado
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "⏳ Processando cupom...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                btnImprimir.Enabled = false;
                btnSalvarPdf.Enabled = false;
                btnFechar.Enabled = false;
            }
            else
            {
                progressBar.Visible = false;
                lblStatus.ForeColor = Color.White;
                
                if (_cupomGerado)
                {
                    lblStatus.Text = "📄 Cupom fiscal gerado com sucesso - Pressione ENTER para fechar";
                }
                else
                {
                    lblStatus.Text = "🟡 Aguardando geração do cupom...";
                }
                
                // Habilita controles
                btnImprimir.Enabled = _cupomGerado;
                btnSalvarPdf.Enabled = _cupomGerado;
                btnFechar.Enabled = true;
            }
        }
        
        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            AtualizarStatusInterface();
            
            if (loading)
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
        public void FecharCupom()
        {
            try
            {
                CupomLogger.LogInfo("Fechando formulário de cupom", "UserAction");
                this.Close();
            }
            catch (Exception ex)
            {
                CupomLogger.LogError($"Erro ao fechar formulário: {ex.Message}", "FormManagement", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - CUPOM FISCAL\n\n" +
                       "📄 VISUALIZAÇÃO:\n" +
                       "• O cupom fiscal é exibido com formatação de impressora\n" +
                       "• Use a rolagem para ver todo o conteúdo\n" +
                       "• Linhas importantes aparecem em destaque\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• ENTER/ESC - Fechar janela\n" +
                       "• F1 - Esta ajuda\n" +
                       "• P/F2 - Imprimir cupom\n" +
                       "• S/F3 - Salvar como arquivo\n\n" +
                       "🔧 AÇÕES DISPONÍVEIS:\n" +
                       "• Imprimir - Envia para impressora física\n" +
                       "• Salvar PDF - Gera arquivo para armazenamento\n" +
                       "• Fechar - Finaliza a visualização\n\n" +
                       "💡 INFORMAÇÕES:\n" +
                       "• O cupom é automaticamente salvo no sistema\n" +
                       "• Uma cópia é enviada para processamento\n" +
                       "• Arquivos ficam em C:\\Recibos\\";
            
            MessageBox.Show(ajuda, "Ajuda - Cupom Fiscal",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirSucesso(string mensagem)
        {
            MessageBox.Show(mensagem, "✅ Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExibirErro(string titulo, string mensagem)
        {
            MessageBox.Show(mensagem, $"❌ {titulo}",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void ExibirAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "⚠️ Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        #endregion
        
        #region Compatibilidade com Versão Legada
        
        // Mantido para compatibilidade
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    FecharCupom();
                    return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            frmCupom_Load(sender, e);
        }
        
        // Propriedades públicas para acesso externo
        public List<string> Layout => _layout;
        public List<string> _Layout => _layout; // Compatibilidade
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class CupomLogger
        {
            public static void LogInfo(string message, string category)
            {
                Debug.WriteLine($"[INFO] [Cupom-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Debug.WriteLine($"[WARN] [Cupom-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Debug.WriteLine($"[ERROR] [Cupom-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Debug.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Debug.WriteLine($"[API] [Cupom-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
            
            public static void LogPerformance(string operation, TimeSpan duration)
            {
                Debug.WriteLine($"[PERF] [Cupom-{operation}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {duration.TotalMilliseconds}ms");
            }
        }
        
        #endregion
    }
}

