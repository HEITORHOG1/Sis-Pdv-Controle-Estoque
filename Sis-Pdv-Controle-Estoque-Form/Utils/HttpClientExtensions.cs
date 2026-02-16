using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class HttpClientExtensions
    {
        private static readonly MediaTypeHeaderValue contentType
            = new MediaTypeHeaderValue("application/json");

        private static readonly JsonSerializerOptions defaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ApplicationException(
                        $"API call failed: {response.StatusCode} - {response.ReasonPhrase}. " +
                        $"Content: {errorContent}");
                }

                var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                if (string.IsNullOrWhiteSpace(dataAsString))
                {
                    throw new ApplicationException("API returned empty response");
                }

                try
                {
                    return JsonSerializer.Deserialize<T>(dataAsString, defaultJsonOptions);
                }
                catch (JsonException jsonEx)
                {
                    // Log do conteúdo que causou erro para debugging
                    System.Diagnostics.Debug.WriteLine($"JSON Deserialization Error: {jsonEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"JSON Content: {dataAsString}");
                    
                    throw new ApplicationException(
                        $"Erro ao processar resposta da API. Dados inválidos recebidos. " +
                        $"Erro: {jsonEx.Message}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw new ApplicationException($"Erro de rede: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                throw new ApplicationException($"Timeout na requisição: {tcEx.Message}");
            }
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            try
            {
                var dataAsString = JsonSerializer.Serialize(data, defaultJsonOptions);
                var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = contentType;
                
                // Log para debugging
                System.Diagnostics.Debug.WriteLine($"POST to {url}: {dataAsString}");
                
                return httpClient.PostAsync(url, content);
            }
            catch (JsonException jsonEx)
            {
                throw new ApplicationException($"Erro ao serializar dados para envio: {jsonEx.Message}");
            }
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            try
            {
                var dataAsString = JsonSerializer.Serialize(data, defaultJsonOptions);
                var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = contentType;
                
                // Log para debugging
                System.Diagnostics.Debug.WriteLine($"PUT to {url}: {dataAsString}");
                
                return httpClient.PutAsync(url, content);
            }
            catch (JsonException jsonEx)
            {
                throw new ApplicationException($"Erro ao serializar dados para envio: {jsonEx.Message}");
            }
        }
    }
}
