using System.Diagnostics;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class ColaboradorLogger
    {
        private static readonly string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SisPdv", "Logs", "Colaborador");

        static ColaboradorLogger()
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

        public static void LogOperation(string operation, string colaboradorId = "", string colaboradorNome = "", string value = "")
        {
            var message = $"Operação: {operation}";
            if (!string.IsNullOrEmpty(colaboradorId))
                message += $" | ID: {colaboradorId}";
            if (!string.IsNullOrEmpty(colaboradorNome))
                message += $" | Nome: {colaboradorNome}";
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

        public static void LogLoginAttempt(string login, bool success, string result = "")
        {
            var message = $"Tentativa de Login - Login: {login}, Success: {success}";
            if (!string.IsNullOrEmpty(result))
                message += $", Resultado: {result}";
            
            WriteLog(success ? "LOGIN_SUCCESS" : "LOGIN_FAILED", message, "Login");
        }

        public static void LogCpfValidation(string cpf, bool isValid)
        {
            var maskedCpf = MaskCpf(cpf);
            var message = $"Validação CPF - CPF: {maskedCpf}, Válido: {isValid}";
            WriteLog(isValid ? "CPF_VALID" : "CPF_INVALID", message, "CpfValidation");
        }

        public static void LogDepartmentAssignment(string colaboradorNome, string departamentoNome)
        {
            var message = $"Atribuição de Departamento - Colaborador: {colaboradorNome}, Departamento: {departamentoNome}";
            WriteLog("DEPT_ASSIGNMENT", message, "DepartmentAssignment");
        }

        public static void LogPasswordChange(string colaboradorNome, bool success)
        {
            var message = $"Alteração de Senha - Colaborador: {colaboradorNome}, Success: {success}";
            WriteLog(success ? "PASSWORD_CHANGED" : "PASSWORD_CHANGE_FAILED", message, "PasswordChange");
        }

        public static void LogPermissionCheck(string colaboradorNome, string acao, bool autorizado)
        {
            var message = $"Verificação de Permissão - Colaborador: {colaboradorNome}, Ação: {acao}, Autorizado: {autorizado}";
            WriteLog(autorizado ? "PERMISSION_GRANTED" : "PERMISSION_DENIED", message, "PermissionCheck");
        }

        private static void WriteLog(string level, string message, string operation)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logFileName = $"colaborador_{DateTime.Now:yyyyMMdd}.log";
                var logFilePath = Path.Combine(LogPath, logFileName);

                var logEntry = $"[{timestamp}] [{level}] [{operation}] {message}{Environment.NewLine}";

                // Escreve no arquivo de log
                File.AppendAllText(logFilePath, logEntry);

                // Também escreve no Debug Output para desenvolvimento
                Debug.WriteLine($"[COLABORADOR] [{level}] {message}");

                // Limpa logs antigos (mantém apenas os últimos 30 dias)
                CleanOldLogs();
            }
            catch (Exception ex)
            {
                // Se falhar ao escrever o log, pelo menos tenta escrever no Debug
                Debug.WriteLine($"[COLABORADOR] [LOG_ERROR] Falha ao escrever log: {ex.Message}");
            }
        }

        private static void CleanOldLogs()
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-30);
                var logFiles = Directory.GetFiles(LogPath, "colaborador_*.log");

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

        private static string MaskCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return "***";

            var numbersOnly = new string(cpf.Where(char.IsDigit).ToArray());
            if (numbersOnly.Length == 11)
            {
                return $"***.***.{numbersOnly.Substring(6, 3)}-**";
            }

            return "***";
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
                var logFiles = Directory.GetFiles(LogPath, "colaborador_*.log")
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