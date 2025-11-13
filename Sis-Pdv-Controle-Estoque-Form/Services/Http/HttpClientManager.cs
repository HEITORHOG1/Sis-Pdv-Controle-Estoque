using System.Net.Http.Headers;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Http
{
    public static class HttpClientManager
    {
        private static readonly HttpClient _client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
        private static string? _bearerToken;

        static HttpClientManager()
        {
            _client.Timeout = TimeSpan.FromSeconds(100);
        }

        public static void SetBearerToken(string? token)
        {
            _bearerToken = token;
        }

        public static HttpClient GetClient()
        {
            if (!string.IsNullOrWhiteSpace(_bearerToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = null;
            }
            return _client;
        }
    }
}
