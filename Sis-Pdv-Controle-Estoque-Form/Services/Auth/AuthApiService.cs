using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net.Http.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Auth
{
    public class AuthApiService
    {
        private readonly string _basePath = BaseAppConfig.ReadSetting("Base");

        public async Task<AuthResult> LoginAsync(string login, string password)
        {
            var client = Services.Http.HttpClientManager.GetClient();
            var payload = new { login = login, password = password };
            var response = await client.PostAsJsonAsync($"{_basePath}/v1/users/login", payload);

            // Lê o body independente do status code (API retorna 400 para credenciais inválidas)
            var result = await response.Content.ReadFromJsonAsync<AuthResult>();

            if (result == null)
                throw new Exception("Resposta de autenticação inválida");
            if (!result.success)
                throw new Exception(result.message ?? "Credenciais inválidas");

            return result;
        }
    }

    public class AuthResult
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }
        public DateTime? expiresAt { get; set; }
        public UserInfo? user { get; set; }
    }

    public class UserInfo
    {
        public Guid id { get; set; }
        public string login { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string nome { get; set; } = string.Empty;
        public List<string> roles { get; set; } = new();
        public List<string> permissions { get; set; } = new();
    }
}
