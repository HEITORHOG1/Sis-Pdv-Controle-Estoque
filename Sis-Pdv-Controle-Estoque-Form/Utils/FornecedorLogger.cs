using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class FornecedorLogger
    {
        private static readonly string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SisPdv", "Logs", "Fornecedor");

        static FornecedorLogger()
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

        public static void LogOperation(string operation, string fornecedorId = "", string fornecedorNome = "", string value = "")
        {
            var message = $"Operação: {operation}";
            if (!string.IsNullOrEmpty(fornecedorId))
                message += $" | ID: {fornecedorId}";
            if (!string.IsNullOrEmpty(fornecedorNome))
                message += $" | Nome: {fornecedorNome}";
            if (!string.IsNullOrEmpty(value))
                message += $" | Valor: {value}";
            
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

        public static void LogCepLookup(string cep, bool success, string result = "")
        {
            var message = $"Consulta CEP - CEP: {cep}, Success: {success}";
            if (!string.IsNullOrEmpty(result))
                message += $", Resultado: {result}";
            
            WriteLog(success ? "CEP_SUCCESS" : "CEP_ERROR", message, "CepLookup");
        }

        public static void LogCnpjValidation(string cnpj, bool isValid)
        {
            var message = $"Validação CNPJ - CNPJ: {cnpj}, Válido: {isValid}";
            WriteLog(isValid ? "CNPJ_VALID" : "CNPJ_INVALID", message, "CnpjValidation");
        }

        private static void WriteLog(string level, string message, string operation)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logFileName = $"fornecedor_{DateTime.Now:yyyyMMdd}.log";
                var logFilePath = Path.Combine(LogPath, logFileName);

                var logEntry = $"[{timestamp}] [{level}] [{operation}] {message}{Environment.NewLine}";

                // Escreve no arquivo de log
                File.AppendAllText(logFilePath, logEntry);

                // Também escreve no Debug Output para desenvolvimento
                Debug.WriteLine($"[FORNECEDOR] [{level}] {message}");

                // Limpa logs antigos (mantém apenas os últimos 30 dias)
                CleanOldLogs();
            }
            catch (Exception ex)
            {
                // Se falhar ao escrever o log, pelo menos tenta escrever no Debug
                Debug.WriteLine($"[FORNECEDOR] [LOG_ERROR] Falha ao escrever log: {ex.Message}");
            }
        }

        private static void CleanOldLogs()
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-30);
                var logFiles = Directory.GetFiles(LogPath, "fornecedor_*.log");

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
                var logFiles = Directory.GetFiles(LogPath, "fornecedor_*.log")
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
    }
}