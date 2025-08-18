using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class ProdutoLogger
    {
        private static readonly string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SisPdv", "Logs", "Produto");

        static ProdutoLogger()
        {
            // Garante que o diretório de logs existe
            Directory.CreateDirectory(LogPath);
        }

        public static void LogInfo(string message, string operation = "")
        {
            WriteLog("INFO", message, operation);
        }

        public static void LogWarning(string message, string operation = "")
        {
            WriteLog("WARNING", message, operation);
        }

        public static void LogError(string message, string operation = "", Exception ex = null)
        {
            var fullMessage = message;
            if (ex != null)
            {
                fullMessage += $"\nException: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
            WriteLog("ERROR", fullMessage, operation);
        }

        public static void LogOperation(string operation, string produtoId = "", string produtoNome = "", string codigoBarras = "")
        {
            var message = $"Operação: {operation}";
            if (!string.IsNullOrEmpty(produtoId))
                message += $" | ID: {produtoId}";
            if (!string.IsNullOrEmpty(produtoNome))
                message += $" | Nome: {produtoNome}";
            if (!string.IsNullOrEmpty(codigoBarras))
                message += $" | Código: {codigoBarras}";
            
            WriteLog("OPERATION", message, operation);
        }

        public static void LogValidationError(string field, string error, string value = "")
        {
            var message = $"Erro de validação - Campo: {field}, Erro: {error}";
            if (!string.IsNullOrEmpty(value))
                message += $", Valor: {value}";
            
            WriteLog("VALIDATION", message, "Validation");
        }

        public static void LogApiCall(string endpoint, string method, TimeSpan duration, bool success)
        {
            var message = $"API Call - Endpoint: {endpoint}, Method: {method}, Duration: {duration.TotalMilliseconds}ms, Success: {success}";
            WriteLog(success ? "API_SUCCESS" : "API_ERROR", message, "ApiCall");
        }

        public static void LogEstoque(string produtoNome, int quantidadeAnterior, int quantidadeNova, string operacao)
        {
            var message = $"Atualização Estoque - Produto: {produtoNome}, Operação: {operacao}, " +
                         $"Anterior: {quantidadeAnterior}, Nova: {quantidadeNova}, Diferença: {quantidadeNova - quantidadeAnterior}";
            WriteLog("ESTOQUE", message, "EstoqueUpdate");
        }

        public static void LogPreco(string produtoNome, decimal precoAnterior, decimal precoNovo, string tipo)
        {
            var message = $"Atualização Preço - Produto: {produtoNome}, Tipo: {tipo}, " +
                         $"Anterior: R$ {precoAnterior:F2}, Novo: R$ {precoNovo:F2}, " +
                         $"Diferença: R$ {precoNovo - precoAnterior:F2}";
            WriteLog("PRECO", message, "PrecoUpdate");
        }

        public static void LogCodigoBarras(string codigoBarras, bool isValido, string produtoNome = "")
        {
            var message = $"Validação Código Barras - Código: {codigoBarras}, Válido: {isValido}";
            if (!string.IsNullOrEmpty(produtoNome))
                message += $", Produto: {produtoNome}";
            
            WriteLog(isValido ? "CODIGO_VALID" : "CODIGO_INVALID", message, "CodigoValidation");
        }

        public static void LogVencimento(string produtoNome, DateTime dataVencimento, int diasRestantes)
        {
            var status = diasRestantes <= 0 ? "VENCIDO" : 
                        diasRestantes <= 7 ? "CRITICO" : 
                        diasRestantes <= 30 ? "ALERTA" : "OK";
            
            var message = $"Controle Vencimento - Produto: {produtoNome}, " +
                         $"Vencimento: {dataVencimento:dd/MM/yyyy}, Dias Restantes: {diasRestantes}, Status: {status}";
            
            WriteLog($"VENCIMENTO_{status}", message, "VencimentoCheck");
        }

        public static void LogMargemLucro(string produtoNome, decimal precoCusto, decimal precoVenda, decimal margem)
        {
            var status = margem < 10 ? "BAIXA" : margem > 100 ? "ALTA" : "NORMAL";
            
            var message = $"Análise Margem - Produto: {produtoNome}, " +
                         $"Custo: R$ {precoCusto:F2}, Venda: R$ {precoVenda:F2}, Margem: {margem:F2}%, Status: {status}";
            
            WriteLog($"MARGEM_{status}", message, "MargemAnalysis");
        }

        public static void LogCategorizacao(string produtoNome, string categoriaAnterior, string categoriaNova)
        {
            var message = $"Alteração Categoria - Produto: {produtoNome}, " +
                         $"Anterior: {categoriaAnterior}, Nova: {categoriaNova}";
            WriteLog("CATEGORIA", message, "CategoriaChange");
        }

        public static void LogFornecedor(string produtoNome, string fornecedorAnterior, string fornecedorNovo)
        {
            var message = $"Alteração Fornecedor - Produto: {produtoNome}, " +
                         $"Anterior: {fornecedorAnterior}, Novo: {fornecedorNovo}";
            WriteLog("FORNECEDOR", message, "FornecedorChange");
        }

        public static void LogBuscaCodigoBarras(string codigoBarras, bool encontrado, int quantidadeResultados = 0)
        {
            var message = $"Busca por Código - Código: {codigoBarras}, Encontrado: {encontrado}";
            if (encontrado)
                message += $", Resultados: {quantidadeResultados}";
            
            WriteLog(encontrado ? "SEARCH_SUCCESS" : "SEARCH_NOT_FOUND", message, "SearchCodigo");
        }

        public static void LogPerformance(string operacao, TimeSpan duracao, int quantidadeItens = 0)
        {
            var message = $"Performance - Operação: {operacao}, Duração: {duracao.TotalMilliseconds}ms";
            if (quantidadeItens > 0)
                message += $", Itens: {quantidadeItens}, Média: {duracao.TotalMilliseconds / quantidadeItens:F2}ms/item";
            
            var status = duracao.TotalMilliseconds > 5000 ? "LENTA" : 
                        duracao.TotalMilliseconds > 2000 ? "MODERADA" : "RAPIDA";
            
            WriteLog($"PERFORMANCE_{status}", message, "Performance");
        }

        private static void WriteLog(string level, string message, string operation)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logFileName = $"produto_{DateTime.Now:yyyyMMdd}.log";
                var logFilePath = Path.Combine(LogPath, logFileName);

                var logEntry = $"[{timestamp}] [{level}] [{operation}] {message}{Environment.NewLine}";

                // Escreve no arquivo de log
                File.AppendAllText(logFilePath, logEntry);

                // Também escreve no Debug Output para desenvolvimento
                Debug.WriteLine($"[PRODUTO] [{level}] {message}");

                // Limpa logs antigos (mantém apenas os últimos 30 dias)
                CleanOldLogs();
            }
            catch (Exception ex)
            {
                // Se falhar ao escrever o log, pelo menos tenta escrever no Debug
                Debug.WriteLine($"[PRODUTO] [LOG_ERROR] Falha ao escrever log: {ex.Message}");
            }
        }

        private static void CleanOldLogs()
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-30);
                var logFiles = Directory.GetFiles(LogPath, "produto_*.log");

                foreach (var logFile in logFiles)
                {
                    var fileInfo = new FileInfo(logFile);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch
            {
                // Ignora erros na limpeza de logs
            }
        }

        public static string GetLogDirectory()
        {
            return LogPath;
        }

        public static List<string> GetRecentLogs(int days = 7)
        {
            try
            {
                var logs = new List<string>();
                var cutoffDate = DateTime.Now.AddDays(-days);
                var logFiles = Directory.GetFiles(LogPath, "produto_*.log")
                    .Where(f => new FileInfo(f).CreationTime >= cutoffDate)
                    .OrderByDescending(f => new FileInfo(f).CreationTime);

                foreach (var logFile in logFiles)
                {
                    var content = File.ReadAllText(logFile);
                    logs.Add($"=== {Path.GetFileName(logFile)} ===\n{content}\n");
                }

                return logs;
            }
            catch
            {
                return new List<string> { "Erro ao ler logs" };
            }
        }

        public static void LogRelatorio(string tipoRelatorio, int quantidadeItens, string filtros = "")
        {
            var message = $"Geração Relatório - Tipo: {tipoRelatorio}, Itens: {quantidadeItens}";
            if (!string.IsNullOrEmpty(filtros))
                message += $", Filtros: {filtros}";
            
            WriteLog("RELATORIO", message, "RelatorioGeneration");
        }

        public static void LogImportacao(string arquivo, int totalItens, int sucessos, int erros)
        {
            var message = $"Importação - Arquivo: {Path.GetFileName(arquivo)}, " +
                         $"Total: {totalItens}, Sucessos: {sucessos}, Erros: {erros}, " +
                         $"Taxa Sucesso: {(sucessos * 100.0 / totalItens):F1}%";
            
            var status = erros == 0 ? "COMPLETO" : 
                        erros < totalItens / 2 ? "PARCIAL" : "FALHOU";
            
            WriteLog($"IMPORT_{status}", message, "Import");
        }

        public static void LogExportacao(string arquivo, int quantidadeItens, string formato)
        {
            var message = $"Exportação - Arquivo: {Path.GetFileName(arquivo)}, " +
                         $"Itens: {quantidadeItens}, Formato: {formato}";
            WriteLog("EXPORT", message, "Export");
        }
    }
}