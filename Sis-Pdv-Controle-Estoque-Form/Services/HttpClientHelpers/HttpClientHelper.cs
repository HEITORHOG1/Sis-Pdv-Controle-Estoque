using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Sis_Pdv_Controle_Estoque_Form.Services.Exceptions;
using System.Net.Http.Json;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_Form.Services.HttpClientHelpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _client;
        private readonly ILogger<HttpClientHelper> _logger;

        public HttpClientHelper(HttpClient client, ILogger<HttpClientHelper> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> SendRequest<T>(string url, HttpMethod method, object content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                request.Content = new JsonContent(content);
            }

            var policy = Policy.Handle<Exception>().RetryAsync(3);
            HttpResponseMessage response = await policy.ExecuteAsync(() => _client.SendAsync(request));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                _logger.LogError($"Erro ao chamar a API. URL: {url}, StatusCode: {response.StatusCode}");
                throw new ApiException(response);
            }
        }
    }

}
public class JsonContent : StringContent
{
    public JsonContent(object obj) :
        base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
    { }
}