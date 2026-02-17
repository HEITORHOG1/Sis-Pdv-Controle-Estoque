using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    /// <summary>
    /// Sistema de logs espec�fico para opera��es do PDV
    /// </summary>
    public static class PdvLogger
    {
        private static string LogDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SisPdv", "Logs", "PDV");

        static PdvLogger()
        {
            Directory.CreateDirectory(LogDirectory);
        }

        /// <summary>
        /// Log de abertura de caixa
        /// </summary>
        public static void LogAberturaCaixa(string operador, string caixa, decimal valorInicial = 0)
        {
            var message = $"ABERTURA DE CAIXA - Operador: {operador}, Caixa: {caixa}, Valor Inicial: {valorInicial:C2}";
            WriteLog("INFO", "ABERTURA_CAIXA", message);
        }

        /// <summary>
        /// Log de fechamento de caixa
        /// </summary>
        public static void LogFechamentoCaixa(string operador, string caixa, decimal valorFinal, int totalVendas)
        {
            var message = $"FECHAMENTO DE CAIXA - Operador: {operador}, Caixa: {caixa}, Valor Final: {valorFinal:C2}, Total Vendas: {totalVendas}";
            WriteLog("INFO", "FECHAMENTO_CAIXA", message);
        }

        /// <summary>
        /// Log de in�cio de venda
        /// </summary>
        public static void LogInicioVenda(Guid vendaId, string operador)
        {
            var message = $"IN�CIO VENDA - ID: {vendaId}, Operador: {operador}";
            WriteLog("INFO", "INICIO_VENDA", message);
        }

        /// <summary>
        /// Log de adi��o de item ao carrinho
        /// </summary>
        public static void LogAdicionarItem(string codigoBarras, string produto, int quantidade, decimal preco, decimal total)
        {
            var message = $"ITEM ADICIONADO - C�digo: {codigoBarras}, Produto: {produto}, Qtd: {quantidade}, Pre�o: {preco:C2}, Total: {total:C2}";
            WriteLog("INFO", "ADICIONAR_ITEM", message);
        }

        /// <summary>
        /// Log de cancelamento de item
        /// </summary>
        public static void LogCancelarItem(int codigo, string codigoBarras, string produto, string operador, string motivo = "")
        {
            var message = $"ITEM CANCELADO - C�digo: {codigo}, Produto: {produto} ({codigoBarras}), Operador: {operador}";
            if (!string.IsNullOrEmpty(motivo))
                message += $", Motivo: {motivo}";
            WriteLog("WARNING", "CANCELAR_ITEM", message);
        }

        /// <summary>
        /// Log de finaliza��o de venda
        /// </summary>
        public static void LogFinalizarVenda(Guid vendaId, string FormaPagamento, decimal valorTotal, decimal valorRecebido, decimal troco, int itens)
        {
            var message = $"VENDA FINALIZADA - ID: {vendaId}, Forma: {FormaPagamento}, Total: {valorTotal:C2}, Recebido: {valorRecebido:C2}, Troco: {troco:C2}, Itens: {itens}";
            WriteLog("INFO", "FINALIZAR_VENDA", message);
        }

        /// <summary>
        /// Log de cancelamento de venda
        /// </summary>
        public static void LogCancelarVenda(Guid vendaId, string operador, string motivo = "")
        {
            var message = $"VENDA CANCELADA - ID: {vendaId}, Operador: {operador}";
            if (!string.IsNullOrEmpty(motivo))
                message += $", Motivo: {motivo}";
            WriteLog("WARNING", "CANCELAR_VENDA", message);
        }

        /// <summary>
        /// Log de busca de produto
        /// </summary>
        public static void LogBuscarProduto(string codigoBarras, bool encontrado, string NomeProduto = "")
        {
            var status = encontrado ? "ENCONTRADO" : "N�O ENCONTRADO";
            var message = $"BUSCA PRODUTO - C�digo: {codigoBarras}, Status: {status}";
            if (!string.IsNullOrEmpty(NomeProduto))
                message += $", Produto: {NomeProduto}";
            
            WriteLog(encontrado ? "INFO" : "WARNING", "BUSCAR_PRODUTO", message);
        }

        /// <summary>
        /// Log de altera��o de quantidade
        /// </summary>
        public static void LogAlterarQuantidade(string codigoBarras, int quantidadeAnterior, int quantidadeNova, string operador)
        {
            var message = $"QUANTIDADE ALTERADA - C�digo: {codigoBarras}, De: {quantidadeAnterior} Para: {quantidadeNova}, Operador: {operador}";
            WriteLog("INFO", "ALTERAR_QUANTIDADE", message);
        }

        /// <summary>
        /// Log de forma de pagamento
        /// </summary>
        public static void LogFormaPagamento(string forma, decimal valor, string detalhes = "")
        {
            var message = $"FORMA PAGAMENTO - Tipo: {forma}, Valor: {valor:C2}";
            if (!string.IsNullOrEmpty(detalhes))
                message += $", Detalhes: {detalhes}";
            WriteLog("INFO", "FORMA_PAGAMENTO", message);
        }

        /// <summary>
        /// Log de impress�o de cupom
        /// </summary>
        public static void LogImpressaoCupom(Guid vendaId, string tipo, bool sucesso, string erro = "")
        {
            var status = sucesso ? "SUCESSO" : "ERRO";
            var message = $"IMPRESS�O CUPOM - ID: {vendaId}, Tipo: {tipo}, Status: {status}";
            if (!string.IsNullOrEmpty(erro))
                message += $", Erro: {erro}";
            
            WriteLog(sucesso ? "INFO" : "ERROR", "IMPRESSAO_CUPOM", message);
        }

        /// <summary>
        /// Log de alertas de estoque
        /// </summary>
        public static void LogAlertaEstoque(string codigoBarras, string produto, int estoqueAtual, int quantidadeSolicitada)
        {
            var message = $"ALERTA ESTOQUE - Produto: {produto} ({codigoBarras}), Estoque: {estoqueAtual}, Solicitado: {quantidadeSolicitada}";
            WriteLog("WARNING", "ALERTA_ESTOQUE", message);
        }

        /// <summary>
        /// Log de alertas de vencimento
        /// </summary>
        public static void LogAlertaVencimento(string codigoBarras, string produto, DateTime DataVencimento, int diasRestantes)
        {
            var status = diasRestantes <= 0 ? "VENCIDO" : $"VENCE EM {diasRestantes} DIAS";
            var message = $"ALERTA VENCIMENTO - Produto: {produto} ({codigoBarras}), Vencimento: {DataVencimento:dd/MM/yyyy}, Status: {status}";
            WriteLog("WARNING", "ALERTA_VENCIMENTO", message);
        }

        /// <summary>
        /// Log de autentica��o de operador
        /// </summary>
        public static void LogAutenticacao(string operador, bool sucesso, string ip = "")
        {
            var status = sucesso ? "SUCESSO" : "FALHA";
            var message = $"AUTENTICA��O - Operador: {operador}, Status: {status}";
            if (!string.IsNullOrEmpty(ip))
                message += $", IP: {ip}";
            
            WriteLog(sucesso ? "INFO" : "WARNING", "AUTENTICACAO", message);
        }

        /// <summary>
        /// Log de descontos aplicados
        /// </summary>
        public static void LogDesconto(decimal valorOriginal, decimal valorDesconto, decimal percentual, string operador, string motivo = "")
        {
            var message = $"DESCONTO APLICADO - Valor Original: {valorOriginal:C2}, Desconto: {valorDesconto:C2} ({percentual:F2}%), Operador: {operador}";
            if (!string.IsNullOrEmpty(motivo))
                message += $", Motivo: {motivo}";
            WriteLog("INFO", "DESCONTO", message);
        }

        /// <summary>
        /// Log de erros do sistema
        /// </summary>
        public static void LogError(string operacao, string mensagem, Exception ex = null)
        {
            var message = $"ERRO - Opera��o: {operacao}, Mensagem: {mensagem}";
            if (ex != null)
            {
                message += $", Exception: {ex.Message}";
                if (ex.InnerException != null)
                    message += $", Inner: {ex.InnerException.Message}";
            }
            WriteLog("ERROR", "SISTEMA", message);
        }

        /// <summary>
        /// Log de opera��es da API
        /// </summary>
        public static void LogApiCall(string endpoint, string metodo, TimeSpan duracao, bool sucesso, string detalhes = "")
        {
            var status = sucesso ? "SUCESSO" : "ERRO";
            var message = $"API CALL - {metodo} {endpoint}, Dura��o: {duracao.TotalMilliseconds}ms, Status: {status}";
            if (!string.IsNullOrEmpty(detalhes))
                message += $", Detalhes: {detalhes}";
            
            WriteLog(sucesso ? "INFO" : "ERROR", "API", message);
        }

        /// <summary>
        /// Log de performance do sistema
        /// </summary>
        public static void LogPerformance(string operacao, TimeSpan duracao, int quantidade = 0)
        {
            var classificacao = duracao.TotalMilliseconds switch
            {
                < 100 => "R�PIDA",
                < 1000 => "MODERADA",
                _ => "LENTA"
            };
            
            var message = $"PERFORMANCE - Opera��o: {operacao}, Dura��o: {duracao.TotalMilliseconds}ms, Classifica��o: {classificacao}";
            if (quantidade > 0)
                message += $", Quantidade: {quantidade}";
            
            WriteLog("INFO", "PERFORMANCE", message);
        }

        /// <summary>
        /// Log de integra��o com SEFAZ
        /// </summary>
        public static void LogSefaz(string operacao, string chaveNfe, bool sucesso, string protocolo = "", string erro = "")
        {
            var status = sucesso ? "SUCESSO" : "ERRO";
            var message = $"SEFAZ - Opera��o: {operacao}, Chave NFe: {chaveNfe}, Status: {status}";
            if (!string.IsNullOrEmpty(protocolo))
                message += $", Protocolo: {protocolo}";
            if (!string.IsNullOrEmpty(erro))
                message += $", Erro: {erro}";
            
            WriteLog(sucesso ? "INFO" : "ERROR", "SEFAZ", message);
        }

        /// <summary>
        /// Log de backup e sincroniza��o
        /// </summary>
        public static void LogBackup(string tipo, bool sucesso, int registros = 0, string erro = "")
        {
            var status = sucesso ? "SUCESSO" : "ERRO";
            var message = $"BACKUP - Tipo: {tipo}, Status: {status}";
            if (registros > 0)
                message += $", Registros: {registros}";
            if (!string.IsNullOrEmpty(erro))
                message += $", Erro: {erro}";
            
            WriteLog(sucesso ? "INFO" : "ERROR", "BACKUP", message);
        }

        /// <summary>
        /// Escreve log no arquivo
        /// </summary>
        private static void WriteLog(string level, string category, string message)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logEntry = $"[{timestamp}] [{level}] [{category}] {message}";
                
                var fileName = $"PDV_{DateTime.Now:yyyy-MM-dd}.log";
                var filePath = Path.Combine(LogDirectory, fileName);
                
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
                
                // Tamb�m escreve no Debug Output para desenvolvimento
                Debug.WriteLine($"PDV_LOG: {logEntry}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao escrever log: {ex.Message}");
            }
        }

        /// <summary>
        /// Limpa logs antigos (mant�m apenas �ltimos 30 dias)
        /// </summary>
        public static void LimparLogsAntigos()
        {
            try
            {
                var files = Directory.GetFiles(LogDirectory, "PDV_*.log");
                var dataLimite = DateTime.Now.AddDays(-30);
                
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < dataLimite)
                    {
                        File.Delete(file);
                        Debug.WriteLine($"Log removido: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao limpar logs antigos: {ex.Message}");
            }
        }

        /// <summary>
        /// Obt�m estat�sticas dos logs
        /// </summary>
        public static Dictionary<string, int> GetEstatisticasLogs(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var stats = new Dictionary<string, int>();
            
            try
            {
                dataInicio ??= DateTime.Today;
                dataFim ??= DateTime.Today.AddDays(1);
                
                var files = Directory.GetFiles(LogDirectory, "PDV_*.log");
                
                foreach (var file in files)
                {
                    var fileDate = ExtractDateFromFileName(file);
                    if (fileDate >= dataInicio && fileDate < dataFim)
                    {
                        var lines = File.ReadAllLines(file);
                        
                        foreach (var line in lines)
                        {
                            if (line.Contains("[INFO]")) stats["INFO"] = stats.GetValueOrDefault("INFO", 0) + 1;
                            else if (line.Contains("[WARNING]")) stats["WARNING"] = stats.GetValueOrDefault("WARNING", 0) + 1;
                            else if (line.Contains("[ERROR]")) stats["ERROR"] = stats.GetValueOrDefault("ERROR", 0) + 1;
                            
                            if (line.Contains("[FINALIZAR_VENDA]")) stats["VENDAS"] = stats.GetValueOrDefault("VENDAS", 0) + 1;
                            else if (line.Contains("[CANCELAR_VENDA]")) stats["CANCELAMENTOS"] = stats.GetValueOrDefault("CANCELAMENTOS", 0) + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao obter estat�sticas: {ex.Message}");
            }
            
            return stats;
        }

        private static DateTime ExtractDateFromFileName(string filePath)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var datePart = fileName.Replace("PDV_", "");
                return DateTime.ParseExact(datePart, "yyyy-MM-dd", null);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}