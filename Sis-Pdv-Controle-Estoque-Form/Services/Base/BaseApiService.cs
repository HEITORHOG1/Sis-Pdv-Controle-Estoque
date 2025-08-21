using System.Net;
using System.Text.Json;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Base
{
    /// <summary>
    /// Configurações para retry logic
    /// </summary>
    public class RetryConfig
    {
        public int MaxRetries { get; set; } = 3;
        public TimeSpan BaseDelay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);
        public double BackoffMultiplier { get; set; } = 2.0;
        public bool EnableExponentialBackoff { get; set; } = true;
    }

    /// <summary>
    /// Exceção personalizada para erros de API
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ResponseContent { get; }
        public string? ErrorCode { get; }

        public ApiException(HttpStatusCode statusCode, string message, string? responseContent = null, string? errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Classe base para serviços com tratamento robusto de erros, retry logic e timeouts
    /// </summary>
    public abstract class BaseApiService
    {
        protected readonly HttpClient _client;
        protected readonly string _basePath;
        protected readonly RetryConfig _retryConfig;

        protected BaseApiService(string basePath, RetryConfig? retryConfig = null)
        {
            _client = Services.Http.HttpClientManager.GetClient();
            _basePath = basePath;
            _retryConfig = retryConfig ?? new RetryConfig();
        }

        /// <summary>
        /// Executa uma requisição HTTP com retry logic e tratamento de erros
        /// </summary>
        protected async Task<T> ExecuteWithRetry<T>(Func<Task<HttpResponseMessage>> httpCall, string operationName)
        {
            Exception? lastException = null;
            
            for (int attempt = 0; attempt <= _retryConfig.MaxRetries; attempt++)
            {
                try
                {
                    using var response = await httpCall();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        return await DeserializeResponse<T>(response);
                    }

                    // Se não for um erro que deve ser retentado, lança exceção imediatamente
                    if (!ShouldRetry(response.StatusCode, attempt))
                    {
                        await ThrowDetailedException(response, operationName);
                    }

                    lastException = new ApiException(response.StatusCode, $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    lastException = new Exception($"Timeout na requisição {operationName} (tentativa {attempt + 1})", ex);
                    
                    if (attempt == _retryConfig.MaxRetries)
                    {
                        throw new Exception($"Timeout após {_retryConfig.MaxRetries + 1} tentativas para {operationName}", ex);
                    }
                }
                catch (HttpRequestException ex)
                {
                    lastException = new Exception($"Erro de conexão em {operationName} (tentativa {attempt + 1}): {ex.Message}", ex);
                    
                    if (attempt == _retryConfig.MaxRetries)
                    {
                        throw new Exception($"Falha de conexão após {_retryConfig.MaxRetries + 1} tentativas para {operationName}", ex);
                    }
                }
                catch (JsonException ex)
                {
                    // Erros de JSON não devem ser retentados
                    throw new Exception($"Erro ao processar resposta JSON em {operationName}: {ex.Message}", ex);
                }
                catch (ApiException)
                {
                    // ApiExceptions não devem ser retentadas se já foram processadas
                    throw;
                }
                catch (Exception ex) when (!(ex is ArgumentException || ex is ArgumentNullException))
                {
                    lastException = ex;
                    
                    if (attempt == _retryConfig.MaxRetries)
                    {
                        throw new Exception($"Erro inesperado após {_retryConfig.MaxRetries + 1} tentativas para {operationName}: {ex.Message}", ex);
                    }
                }

                // Aguarda antes da próxima tentativa
                if (attempt < _retryConfig.MaxRetries)
                {
                    await Task.Delay(CalculateDelay(attempt));
                }
            }

            throw lastException ?? new Exception($"Falha após {_retryConfig.MaxRetries + 1} tentativas para {operationName}");
        }

        /// <summary>
        /// Executa uma requisição HTTP simples com tratamento de erros (sem retry)
        /// </summary>
        protected async Task<T> ExecuteRequest<T>(Func<Task<HttpResponseMessage>> httpCall, string operationName)
        {
            try
            {
                using var response = await httpCall();
                
                if (response.IsSuccessStatusCode)
                {
                    return await DeserializeResponse<T>(response);
                }

                await ThrowDetailedException(response, operationName);
                throw new Exception($"Falha desconhecida em {operationName}");
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new Exception($"Timeout na requisição {operationName}: {ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão em {operationName}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON em {operationName}: {ex.Message}", ex);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex) when (!(ex is ArgumentException || ex is ArgumentNullException))
            {
                throw new Exception($"Erro inesperado em {operationName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deserializa a resposta HTTP
        /// </summary>
        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            try
            {
                var result = await response.ReadContentAs<T>();
                return result ?? throw new Exception("Resposta vazia da API");
            }
            catch (JsonException ex)
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta: {ex.Message}. Content: {content}");
                throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
            }
        }

        /// <summary>
        /// Determina se uma requisição deve ser retentada baseada no status code
        /// </summary>
        private bool ShouldRetry(HttpStatusCode statusCode, int attempt)
        {
            if (attempt >= _retryConfig.MaxRetries)
                return false;

            return statusCode switch
            {
                // Erros de servidor que podem ser transitórios
                HttpStatusCode.InternalServerError => true,
                HttpStatusCode.BadGateway => true,
                HttpStatusCode.ServiceUnavailable => true,
                HttpStatusCode.GatewayTimeout => true,
                HttpStatusCode.RequestTimeout => true,
                
                // Too Many Requests - pode ser retentado
                HttpStatusCode.TooManyRequests => true,
                
                // Erros de cliente não devem ser retentados
                HttpStatusCode.BadRequest => false,
                HttpStatusCode.Unauthorized => false,
                HttpStatusCode.Forbidden => false,
                HttpStatusCode.NotFound => false,
                HttpStatusCode.Conflict => false,
                HttpStatusCode.UnprocessableEntity => false,
                
                _ => false
            };
        }

        /// <summary>
        /// Calcula o delay para a próxima tentativa
        /// </summary>
        private TimeSpan CalculateDelay(int attempt)
        {
            if (!_retryConfig.EnableExponentialBackoff)
                return _retryConfig.BaseDelay;

            var delay = TimeSpan.FromMilliseconds(
                _retryConfig.BaseDelay.TotalMilliseconds * Math.Pow(_retryConfig.BackoffMultiplier, attempt)
            );

            return delay > _retryConfig.MaxDelay ? _retryConfig.MaxDelay : delay;
        }

        /// <summary>
        /// Lança exceção detalhada baseada no status code
        /// </summary>
        protected async Task ThrowDetailedException(HttpResponseMessage response, string methodName)
        {
            var content = "";
            try
            {
                content = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                content = "Não foi possível ler o conteúdo da resposta";
            }

            var message = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Dados inválidos enviados para o servidor",
                HttpStatusCode.Unauthorized => "Não autorizado. Verifique suas credenciais",
                HttpStatusCode.Forbidden => "Acesso negado",
                HttpStatusCode.NotFound => GetNotFoundMessage(methodName),
                HttpStatusCode.Conflict => GetConflictMessage(methodName),
                HttpStatusCode.UnprocessableEntity => "Dados inconsistentes",
                HttpStatusCode.TooManyRequests => "Muitas requisições. Tente novamente em alguns momentos",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.BadGateway => "Erro no gateway do servidor",
                HttpStatusCode.ServiceUnavailable => "Serviço temporariamente indisponível",
                HttpStatusCode.GatewayTimeout => "Timeout no gateway do servidor",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            var errorCode = TryExtractErrorCode(content);
            throw new ApiException(response.StatusCode, $"{methodName} falhou: {message}", content, errorCode);
        }

        /// <summary>
        /// Tenta extrair código de erro do conteúdo da resposta
        /// </summary>
        private string? TryExtractErrorCode(string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                    return null;

                using var document = JsonDocument.Parse(content);
                
                // Tenta encontrar propriedades comuns de código de erro
                if (document.RootElement.TryGetProperty("errorCode", out var errorCodeElement))
                    return errorCodeElement.GetString();
                
                if (document.RootElement.TryGetProperty("code", out var codeElement))
                    return codeElement.GetString();
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtém mensagem personalizada para erro 404
        /// </summary>
        protected virtual string GetNotFoundMessage(string methodName) => "Recurso não encontrado";

        /// <summary>
        /// Obtém mensagem personalizada para erro 409 (Conflict)
        /// </summary>
        protected virtual string GetConflictMessage(string methodName) => "Conflito de dados";

        /// <summary>
        /// Valida se uma string é um GUID válido
        /// </summary>
        protected void ValidateGuid(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{parameterName} é obrigatório.");

            if (!Guid.TryParse(value, out _))
                throw new ArgumentException($"{parameterName} deve ser um GUID válido.");
        }

        /// <summary>
        /// Valida se uma string não é nula ou vazia
        /// </summary>
        protected void ValidateRequired(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{parameterName} é obrigatório.");
        }

        /// <summary>
        /// Valida se um valor é positivo
        /// </summary>
        protected void ValidatePositive(int value, string parameterName)
        {
            if (value < 0)
                throw new ArgumentException($"{parameterName} não pode ser negativo.");
        }
    }
}
