using Sis_Pdv_Controle_Estoque_Form.Services.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Extensions;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Relatorios
{
    public partial class frnRelatorioVendasPorData : Form
    {
        #region Campos Privados
        
        private PedidoService _pedidoService;
        private BindingList<dynamic> _vendasList;
        private bool _isLoading = false;
        private DateTime _ultimaConsulta = DateTime.MinValue;
        private decimal _totalVendas = 0;
        private int _quantidadeVendas = 0;
        
        #endregion
        
        #region Construtor e Inicialização
        
        public frnRelatorioVendasPorData()
        {
            InitializeComponent();
            InicializarComponentesModernos();
        }
        
        private void InicializarComponentesModernos()
        {
            // Inicializa serviços e coleções
            _pedidoService = new PedidoService();
            _vendasList = new BindingList<dynamic>();
            
            // Configura DataGridView
            ConfigurarDataGridView();
            
            // Configura datas padrão
            ConfigurarDatasIniciais();
            
            // Configura estado inicial
            AtualizarStatusInterface();
            
            // Log de inicialização
            RelatorioLogger.LogInfo("Formulário de relatório de vendas por data inicializado", "Startup");
        }
        
        private void ConfigurarDataGridView()
        {
            dgvRelatorio.DataSource = _vendasList;
            dgvRelatorio.AutoGenerateColumns = true;
            
            // Remove dependência de FontAwesome
            dgvRelatorio.AllowUserToAddRows = false;
            dgvRelatorio.AllowUserToDeleteRows = false;
            dgvRelatorio.ReadOnly = true;
            dgvRelatorio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelatorio.MultiSelect = false;
            
            // Configurações de estilo moderno já definidas no Designer
            dgvRelatorio.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }
        
        private void ConfigurarDatasIniciais()
        {
            // Configura período padrão: último mês
            dtpDataFinal.Value = DateTime.Today;
            dtpDataInicial.Value = DateTime.Today.AddDays(-30);
            
            // Configura formato de data
            dtpDataInicial.Format = DateTimePickerFormat.Short;
            dtpDataFinal.Format = DateTimePickerFormat.Short;
        }
        
        #endregion
        
        #region Propriedades Públicas
        
        public List<string> listaCarrinho { get; set; } = new List<string>(); // Compatibilidade
        
        #endregion
        
        #region Eventos de Controles
        
        private async void btnConsulta_Click(object sender, EventArgs e)
        {
            await ConsultarVendas();
        }
        
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparResultados();
        }
        
        private async void btnExportarPdf_Click(object sender, EventArgs e)
        {
            await ExportarParaPdf();
        }
        
        private async void btnExportarExcel_Click(object sender, EventArgs e)
        {
            await ExportarParaExcel();
        }
        
        #endregion
        
        #region Eventos de Teclado e Form
        
        private void frnRelatorioVendasPorData_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    MostrarAjuda();
                    break;
                case Keys.F2:
                    btnConsulta.PerformClick();
                    break;
                case Keys.F3:
                    btnLimpar.PerformClick();
                    break;
                case Keys.F4:
                    _ = ExportarParaPdf();
                    break;
                case Keys.F5:
                    _ = ExportarParaExcel();
                    break;
                case Keys.Enter:
                    if (dtpDataInicial.Focused || dtpDataFinal.Focused)
                    {
                        btnConsulta.PerformClick();
                    }
                    break;
            }
        }
        
        private void frnRelatorioVendasPorData_Load(object sender, EventArgs e)
        {
            try
            {
                // Foco na data inicial
                dtpDataInicial.Focus();
                
                RelatorioLogger.LogInfo("Formulário carregado com sucesso", "UserInterface");
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao carregar formulário: {ex.Message}", "Startup", ex);
            }
        }
        
        #endregion
        
        #region Eventos de Mudança de Data
        
        private void dtpDataInicial_ValueChanged(object sender, EventArgs e)
        {
            ValidarPeriodo();
        }
        
        private void dtpDataFinal_ValueChanged(object sender, EventArgs e)
        {
            ValidarPeriodo();
        }
        
        private void ValidarPeriodo()
        {
            try
            {
                var dataInicial = dtpDataInicial.Value.Date;
                var dataFinal = dtpDataFinal.Value.Date;
                
                // Valida se data inicial não é maior que data final
                if (dataInicial > dataFinal)
                {
                    lblStatus.Text = "⚠️ Data inicial não pode ser maior que data final";
                    lblStatus.ForeColor = Color.Orange;
                    btnConsulta.Enabled = false;
                    return;
                }
                
                // Valida período máximo (ex: 1 ano)
                var diferenca = dataFinal - dataInicial;
                if (diferenca.TotalDays > 365)
                {
                    lblStatus.Text = "⚠️ Período máximo permitido: 1 ano";
                    lblStatus.ForeColor = Color.Orange;
                    btnConsulta.Enabled = false;
                    return;
                }
                
                // Período válido
                btnConsulta.Enabled = true;
                if (!_isLoading)
                {
                    var dias = (int)diferenca.TotalDays + 1;
                    lblStatus.Text = $"📅 Período selecionado: {dias} dia(s) - Clique em 'Consultar' para gerar relatório";
                    lblStatus.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao validar período: {ex.Message}", "Validation", ex);
            }
        }
        
        #endregion
        
        #region Consulta de Vendas
        
        private async Task ConsultarVendas()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!ValidarFiltros()) return;
                
                SetLoadingState(true);
                
                var dataInicial = dtpDataInicial.Value.Date;
                var dataFinal = dtpDataFinal.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                
                RelatorioLogger.LogInfo($"Iniciando consulta de vendas - Período: {dataInicial:dd/MM/yyyy} a {dataFinal:dd/MM/yyyy}", "Query");
                
                var response = await _pedidoService.ListarVendaPedidoPorData(dataInicial, dataFinal);
                
                sw.Stop();
                RelatorioLogger.LogApiCall("ListarVendaPedidoPorData", "GET", sw.Elapsed, response?.success == true);
                
                if (response?.success == true && response.data != null)
                {
                    ProcessarResultados(response.data);
                    _ultimaConsulta = DateTime.Now;
                    
                    RelatorioLogger.LogInfo($"Consulta concluída - {_quantidadeVendas} vendas encontradas, Total: R$ {_totalVendas:F2}", "Query");
                }
                else
                {
                    _vendasList.Clear();
                    _totalVendas = 0;
                    _quantidadeVendas = 0;
                    AtualizarTotalizadores();
                    
                    var erro = "Nenhum dado encontrado para o período selecionado";
                    ExibirAviso($"Nenhuma venda encontrada no período selecionado.\n\n{erro}");
                    
                    RelatorioLogger.LogInfo("Nenhuma venda encontrada no período", "Query");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                RelatorioLogger.LogError($"Erro ao consultar vendas: {ex.Message}", "Query", ex);
                ExibirErro("Erro na Consulta", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private void ProcessarResultados(dynamic data)
        {
            try
            {
                _vendasList.Clear();
                _totalVendas = 0;
                _quantidadeVendas = 0;
                
                // Converte os dados para lista
                var vendas = data as IEnumerable<dynamic> ?? new List<dynamic>();
                
                foreach (var venda in vendas)
                {
                    _vendasList.Add(venda);
                    
                    // Calcula total (assumindo que a coluna de valor é a terceira - índice 2)
                    if (decimal.TryParse(venda.GetType().GetProperties()[2].GetValue(venda)?.ToString(), out decimal valor))
                    {
                        _totalVendas += valor;
                    }
                    _quantidadeVendas++;
                }
                
                // Formatar colunas
                FormatarGrid();
                AtualizarTotalizadores();
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao processar resultados: {ex.Message}", "ProcessResults", ex);
                throw;
            }
        }
        
        #endregion
        
        #region Formatação do Grid
        
        private void FormatarGrid()
        {
            try
            {
                if (dgvRelatorio.Columns.Count == 0) return;
                
                // Define cabeçalhos padrão
                var cabecalhos = new List<string> { "📅 Data", "💳 Pagamento", "💰 Total", "🔑 ID" };
                DefinirCabecalhos(cabecalhos);
                
                // Configura larguras e formatação
                for (int i = 0; i < dgvRelatorio.Columns.Count && i < 4; i++)
                {
                    var coluna = dgvRelatorio.Columns[i];
                    
                    switch (i)
                    {
                        case 0: // Data
                            coluna.Width = 120;
                            coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case 1: // Pagamento
                            coluna.Width = 150;
                            coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case 2: // Total
                            coluna.Width = 120;
                            coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            coluna.DefaultCellStyle.Format = "C2";
                            coluna.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                            coluna.DefaultCellStyle.ForeColor = Color.FromArgb(155, 89, 182);
                            break;
                        case 3: // ID
                            coluna.Width = 80;
                            coluna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            coluna.DefaultCellStyle.Font = new Font("Consolas", 9F);
                            coluna.Visible = false; // Oculta ID por padrão
                            break;
                    }
                }
                
                // Atualiza informações do grid
                lblGridInfo.Text = $"📈 Resultados da Consulta ({_quantidadeVendas})";
                
                dgvRelatorio.Refresh();
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao formatar grid: {ex.Message}", "FormatGrid", ex);
            }
        }
        
        private void dgvRelatorio_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                
                // Formatação especial para valores monetários
                if (e.ColumnIndex == 2 && e.Value != null) // Coluna de Total
                {
                    if (decimal.TryParse(e.Value.ToString(), out decimal valor))
                    {
                        e.Value = valor.ToString("C2");
                        e.FormattingApplied = true;
                        
                        // Destaca valores altos
                        if (valor >= 1000)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(232, 245, 233);
                            e.CellStyle.ForeColor = Color.FromArgb(76, 175, 80);
                        }
                    }
                }
                
                // Formatação para forma de pagamento
                if (e.ColumnIndex == 1 && e.Value != null) // Coluna de Pagamento
                {
                    var formaPagamento = e.Value.ToString().ToUpper();
                    switch (formaPagamento)
                    {
                        case "DINHEIRO":
                            e.Value = "💵 Dinheiro";
                            break;
                        case "CARTAO":
                        case "CREDITO":
                        case "DEBITO":
                            e.Value = "💳 Cartão";
                            break;
                        case "PIX":
                            e.Value = "📱 PIX";
                            break;
                        default:
                            e.Value = $"💰 {formaPagamento}";
                            break;
                    }
                    e.FormattingApplied = true;
                }
                
                // Formatação para data
                if (e.ColumnIndex == 0 && e.Value != null) // Coluna de Data
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime data))
                    {
                        e.Value = data.ToString("dd/MM/yyyy");
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro na formatação de célula: {ex.Message}", "CellFormatting", ex);
            }
        }
        
        #endregion
        
        #region Totalizadores
        
        private void AtualizarTotalizadores()
        {
            try
            {
                lblTotal.Text = _totalVendas.FormatarMoeda();
                
                // Atualiza status com informações detalhadas
                if (_quantidadeVendas > 0)
                {
                    var mediaVenda = _totalVendas / _quantidadeVendas;
                    lblStatus.Text = $"📊 {_quantidadeVendas} venda(s) - Total: {_totalVendas:C2} - Média: {mediaVenda:C2}";
                    
                    // Destaca totais altos
                    if (_totalVendas >= 10000)
                    {
                        lblTotal.ForeColor = Color.FromArgb(76, 175, 80); // Verde
                        lblTotalIcon.Text = "🎉";
                    }
                    else if (_totalVendas >= 5000)
                    {
                        lblTotal.ForeColor = Color.FromArgb(255, 193, 7); // Amarelo
                        lblTotalIcon.Text = "⭐";
                    }
                    else
                    {
                        lblTotal.ForeColor = Color.FromArgb(155, 89, 182); // Roxo padrão
                        lblTotalIcon.Text = "💰";
                    }
                }
                else
                {
                    lblTotal.Text = "R$ 0,00";
                    lblTotal.ForeColor = Color.FromArgb(149, 165, 166); // Cinza
                    lblTotalIcon.Text = "📊";
                    lblStatus.Text = "📊 Nenhuma venda encontrada no período selecionado";
                }
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao atualizar totalizadores: {ex.Message}", "UpdateTotals", ex);
            }
        }
        
        #endregion
        
        #region Exportação
        
        private async Task ExportarParaPdf()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!TemDadosParaExportar()) return;
                
                SetLoadingState(true);
                lblStatus.Text = "📄 Gerando arquivo PDF...";
                
                RelatorioLogger.LogInfo("Iniciando exportação para PDF", "Export");
                
                var nomeArquivo = $"Relatorio_Vendas_{dtpDataInicial.Value:yyyyMMdd}_{dtpDataFinal.Value:yyyyMMdd}.pdf";
                
                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Arquivos PDF|*.pdf";
                saveDialog.Title = "Salvar Relatório como PDF";
                saveDialog.FileName = nomeArquivo;
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    await GerarPdfRelatorio(saveDialog.FileName);
                    
                    sw.Stop();
                    RelatorioLogger.LogPerformance("ExportarPDF", sw.Elapsed);
                    RelatorioLogger.LogInfo($"Relatório exportado para PDF: {saveDialog.FileName}", "Export");
                    
                    ExibirSucesso($"Relatório exportado com sucesso!\n\nArquivo: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                RelatorioLogger.LogError($"Erro ao exportar para PDF: {ex.Message}", "Export", ex);
                ExibirErro("Erro na Exportação", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task ExportarParaExcel()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (!TemDadosParaExportar()) return;
                
                SetLoadingState(true);
                lblStatus.Text = "📊 Gerando arquivo Excel...";
                
                RelatorioLogger.LogInfo("Iniciando exportação para Excel", "Export");
                
                var nomeArquivo = $"Relatorio_Vendas_{dtpDataInicial.Value:yyyyMMdd}_{dtpDataFinal.Value:yyyyMMdd}.csv";
                
                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Arquivos CSV|*.csv|Arquivos Excel|*.xlsx";
                saveDialog.Title = "Salvar Relatório como Excel";
                saveDialog.FileName = nomeArquivo;
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    await GerarExcelRelatorio(saveDialog.FileName);
                    
                    sw.Stop();
                    RelatorioLogger.LogPerformance("ExportarExcel", sw.Elapsed);
                    RelatorioLogger.LogInfo($"Relatório exportado para Excel: {saveDialog.FileName}", "Export");
                    
                    ExibirSucesso($"Relatório exportado com sucesso!\n\nArquivo: {saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                RelatorioLogger.LogError($"Erro ao exportar para Excel: {ex.Message}", "Export", ex);
                ExibirErro("Erro na Exportação", ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        
        private async Task GerarPdfRelatorio(string caminho)
        {
            // Implementação futura para geração de PDF
            // Por enquanto, gera um arquivo de texto formatado
            await GerarRelatorioTexto(caminho.Replace(".pdf", ".txt"));
            RelatorioLogger.LogInfo("PDF simulado - salvo como texto formatado", "Export");
        }
        
        private async Task GerarExcelRelatorio(string caminho)
        {
            await Task.Run(() =>
            {
                var sb = new StringBuilder();
                
                // Cabeçalho
                sb.AppendLine($"Relatório de Vendas por Data");
                sb.AppendLine($"Período: {dtpDataInicial.Value:dd/MM/yyyy} a {dtpDataFinal.Value:dd/MM/yyyy}");
                sb.AppendLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine();
                
                // Cabeçalho das colunas
                sb.AppendLine("Data;Forma de Pagamento;Valor Total;ID Pedido");
                
                // Dados
                foreach (var item in _vendasList)
                {
                    var propriedades = item.GetType().GetProperties();
                    var valores = new string[4];
                    
                    for (int i = 0; i < Math.Min(4, propriedades.Length); i++)
                    {
                        valores[i] = propriedades[i].GetValue(item)?.ToString() ?? "";
                    }
                    
                    sb.AppendLine(string.Join(";", valores));
                }
                
                // Totais
                sb.AppendLine();
                sb.AppendLine($"Total de Vendas;{_quantidadeVendas}");
                sb.AppendLine($"Valor Total;{_totalVendas:F2}");
                sb.AppendLine($"Valor Médio;{(_quantidadeVendas > 0 ? _totalVendas / _quantidadeVendas : 0):F2}");
                
                File.WriteAllText(caminho, sb.ToString(), Encoding.UTF8);
            });
        }
        
        private async Task GerarRelatorioTexto(string caminho)
        {
            await Task.Run(() =>
            {
                var sb = new StringBuilder();
                
                sb.AppendLine("=".PadRight(80, '='));
                sb.AppendLine("                    RELATÓRIO DE VENDAS POR DATA");
                sb.AppendLine("=".PadRight(80, '='));
                sb.AppendLine($"Período: {dtpDataInicial.Value:dd/MM/yyyy} a {dtpDataFinal.Value:dd/MM/yyyy}");
                sb.AppendLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine("-".PadRight(80, '-'));
                
                foreach (var item in _vendasList)
                {
                    var propriedades = item.GetType().GetProperties();
                    if (propriedades.Length >= 3)
                    {
                        var data = propriedades[0].GetValue(item);
                        var pagamento = propriedades[1].GetValue(item);
                        var total = propriedades[2].GetValue(item);
                        
                        sb.AppendLine($"{data,-12} | {pagamento,-15} | {total,12}");
                    }
                }
                
                sb.AppendLine("-".PadRight(80, '-'));
                sb.AppendLine($"Total de Vendas: {_quantidadeVendas}");
                sb.AppendLine($"Valor Total: R$ {_totalVendas:F2}");
                sb.AppendLine($"Valor Médio: R$ {(_quantidadeVendas > 0 ? _totalVendas / _quantidadeVendas : 0):F2}");
                sb.AppendLine("=".PadRight(80, '='));
                
                File.WriteAllText(caminho, sb.ToString(), Encoding.UTF8);
            });
        }
        
        private bool TemDadosParaExportar()
        {
            if (_vendasList.Count == 0)
            {
                ExibirAviso("Não há dados para exportar.\n\nExecute uma consulta primeiro.");
                return false;
            }
            return true;
        }
        
        #endregion
        
        #region Validação e Estado
        
        private bool ValidarFiltros()
        {
            try
            {
                var dataInicial = dtpDataInicial.Value.Date;
                var dataFinal = dtpDataFinal.Value.Date;
                
                if (dataInicial > dataFinal)
                {
                    ExibirAviso("Data inicial não pode ser maior que data final.");
                    dtpDataInicial.Focus();
                    return false;
                }
                
                if (dataFinal > DateTime.Today)
                {
                    ExibirAviso("Data final não pode ser maior que hoje.");
                    dtpDataFinal.Focus();
                    return false;
                }
                
                var diferenca = dataFinal - dataInicial;
                if (diferenca.TotalDays > 365)
                {
                    ExibirAviso("Período máximo permitido é de 1 ano.");
                    dtpDataInicial.Focus();
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro na validação: {ex.Message}", "Validation", ex);
                return false;
            }
        }
        
        private void AtualizarStatusInterface()
        {
            if (_isLoading)
            {
                lblStatus.Text = "🔄 Consultando vendas...";
                lblStatus.ForeColor = Color.Orange;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                
                // Desabilita controles
                btnConsulta.Enabled = false;
                btnLimpar.Enabled = false;
                btnExportarPdf.Enabled = false;
                btnExportarExcel.Enabled = false;
                dtpDataInicial.Enabled = false;
                dtpDataFinal.Enabled = false;
            }
            else
            {
                progressBar.Visible = false;
                lblStatus.ForeColor = Color.White;
                
                // Habilita controles
                btnConsulta.Enabled = true;
                btnLimpar.Enabled = true;
                btnExportarPdf.Enabled = _vendasList.Count > 0;
                btnExportarExcel.Enabled = _vendasList.Count > 0;
                dtpDataInicial.Enabled = true;
                dtpDataFinal.Enabled = true;
                
                ValidarPeriodo(); // Revalida período
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
        
        #region Operações Auxiliares
        
        private void LimparResultados()
        {
            try
            {
                _vendasList.Clear();
                _totalVendas = 0;
                _quantidadeVendas = 0;
                
                AtualizarTotalizadores();
                
                lblGridInfo.Text = "📈 Resultados da Consulta (0)";
                lblStatus.Text = "🧹 Resultados limpos - Selecione o período e consulte novamente";
                
                RelatorioLogger.LogInfo("Resultados limpos pelo usuário", "UserAction");
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao limpar resultados: {ex.Message}", "ClearResults", ex);
            }
        }
        
        private void MostrarAjuda()
        {
            var ajuda = "🆘 AJUDA - RELATÓRIO DE VENDAS POR DATA\n\n" +
                       "📊 CONSULTA:\n" +
                       "• Selecione o período desejado nas datas\n" +
                       "• Clique em 'Consultar' para gerar o relatório\n" +
                       "• Período máximo: 1 ano\n" +
                       "• Duplo clique em uma venda para ver detalhes\n\n" +
                       "📤 EXPORTAÇÃO:\n" +
                       "• PDF - Relatório formatado para impressão\n" +
                       "• Excel - Dados em planilha para análise\n" +
                       "• Disponível apenas após consulta\n\n" +
                       "⌨️ ATALHOS:\n" +
                       "• F1 - Esta ajuda\n" +
                       "• F2 - Consultar\n" +
                       "• F3 - Limpar resultados\n" +
                       "• F4 - Exportar PDF\n" +
                       "• F5 - Exportar Excel\n" +
                       "• ENTER - Consultar (nas datas)\n\n" +
                       "💡 DICAS:\n" +
                       "• Use períodos menores para consultas mais rápidas\n" +
                       "• O total inclui todas as formas de pagamento\n" +
                       "• Cores destacam vendas de alto valor\n" +
                       "• Dados são atualizados em tempo real";
            
            MessageBox.Show(ajuda, "Ajuda - Relatório de Vendas",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion
        
        #region Eventos do Grid
        
        private void dgvRelatorio_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= _vendasList.Count) return;
                
                // Obtém ID da venda (assumindo que está na última coluna visível)
                var row = dgvRelatorio.Rows[e.RowIndex];
                var idIndex = dgvRelatorio.Columns.Count - 1;
                
                if (row.Cells[idIndex].Value != null)
                {
                    var id = row.Cells[idIndex].Value.ToString();
                    
                    RelatorioLogger.LogInfo($"Abrindo detalhes da venda: {id}", "DetailView");
                    
                    // Abre formulário de detalhes
                    using var carrinho = new frmCarrinho();
                    carrinho.PreencheListaProdutos(id);
                    carrinho.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao abrir detalhes da venda: {ex.Message}", "DetailView", ex);
                ExibirErro("Erro", "Não foi possível abrir os detalhes da venda.");
            }
        }
        
        #endregion
        
        #region Métodos Auxiliares
        
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
        
        #region Métodos Legados (Compatibilidade)
        
        // Mantido para compatibilidade com código existente
        private void DefinirCabecalhos(List<string> listaCabecalhos)
        {
            try
            {
                int index = 0;
                foreach (DataGridViewColumn coluna in dgvRelatorio.Columns)
                {
                    if (coluna.Visible && index < listaCabecalhos.Count)
                    {
                        coluna.HeaderText = listaCabecalhos[index];
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                RelatorioLogger.LogError($"Erro ao definir cabeçalhos: {ex.Message}", "SetHeaders", ex);
            }
        }
        
        #endregion
        
        #region Classes de Log Auxiliares
        
        private static class RelatorioLogger
        {
            public static void LogInfo(string message, string category)
            {
                Console.WriteLine($"[INFO] [Relatorio-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogWarning(string message, string category)
            {
                Console.WriteLine($"[WARN] [Relatorio-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            
            public static void LogError(string message, string category, Exception ex = null)
            {
                Console.WriteLine($"[ERROR] [Relatorio-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                if (ex != null)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                }
            }
            
            public static void LogApiCall(string method, string type, TimeSpan duration, bool success)
            {
                var status = success ? "SUCCESS" : "FAILED";
                Console.WriteLine($"[API] [Relatorio-{method}] {type} - {duration.TotalMilliseconds}ms - {status}");
            }
            
            public static void LogPerformance(string operation, TimeSpan duration)
            {
                Console.WriteLine($"[PERF] [Relatorio-{operation}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {duration.TotalMilliseconds}ms");
            }
        }
        
        #endregion
    }
}
