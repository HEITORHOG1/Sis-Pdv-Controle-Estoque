using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.AlterarCategoria;
using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Categoria
{
    public class CategoriaService
    {
        private System.Net.Http.HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");

        public async Task<CategoriaResponse> Adicionar(CategoriaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.NomeCategoria))
                throw new ArgumentException("Nome da categoria é obrigatório.");
            if (dto.NomeCategoria.Length < 2 || dto.NomeCategoria.Length > 100)
                throw new ArgumentException("Nome da categoria deve ter entre 2 e 100 caracteres.");

            _client = Services.Http.HttpClientManager.GetClient();
            var request = new AdicionarCategoriaRequest { NomeCategoria = dto.NomeCategoria.Trim() };

            var response = await _client.PostAsJsonAsync($"{BasePath}/Categoria/AdicionarCategoria", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponse>()
                       ?? new CategoriaResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
            }
            await ThrowDetailedException(response, nameof(Adicionar));
            throw new Exception("Falha desconhecida ao adicionar categoria.");
        }

        public async Task<CategoriaResponseList> ListarCategoria()
        {
            _client = Services.Http.HttpClientManager.GetClient();

            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoria");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponseList>()
                       ?? new CategoriaResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<CategoriaDto>() };
            }
            await ThrowDetailedException(response, nameof(ListarCategoria));
            throw new Exception("Falha desconhecida ao listar categorias.");
        }

        public async Task<CategoriaResponseList> ListarCategoriaPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id é obrigatório.");

            _client = Services.Http.HttpClientManager.GetClient();

            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoriaPorId/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponseList>()
                       ?? new CategoriaResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<CategoriaDto>() };
            }
            await ThrowDetailedException(response, nameof(ListarCategoriaPorId));
            throw new Exception("Falha desconhecida ao consultar categoria por Id.");
        }

        public async Task<CategoriaResponseList> ListarCategoriaPorNomeCategoria(string NomeCategoria)
        {
            if (string.IsNullOrWhiteSpace(NomeCategoria)) throw new ArgumentException("Nome da categoria é obrigatório.");

            _client = Services.Http.HttpClientManager.GetClient();

            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoriaPorNomeCategoria/{Uri.EscapeDataString(NomeCategoria)}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponseList>()
                       ?? new CategoriaResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<CategoriaDto>() };
            }
            await ThrowDetailedException(response, nameof(ListarCategoriaPorNomeCategoria));
            throw new Exception("Falha desconhecida ao consultar categoria por nome.");
        }

        public async Task<CategoriaResponse> AlterarCategoria(CategoriaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.id == Guid.Empty) throw new ArgumentException("Id é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.NomeCategoria))
                throw new ArgumentException("Nome da categoria é obrigatório.");

            _client = Services.Http.HttpClientManager.GetClient();

            var request = new AlterarCategoriaRequest { Id = dto.id, NomeCategoria = dto.NomeCategoria.Trim() };

            var response = await _client.PutAsJsonAsync($"{BasePath}/Categoria/AlterarCategoria", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponse>()
                       ?? new CategoriaResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
            }
            await ThrowDetailedException(response, nameof(AlterarCategoria));
            throw new Exception("Falha desconhecida ao alterar categoria.");
        }

        public async Task<CategoriaResponse> RemoverCategoria(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id é obrigatório.");

            _client = Services.Http.HttpClientManager.GetClient();

            var response = await _client.DeleteAsync($"{BasePath}/Categoria/RemoverCategoria/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoriaResponse>()
                       ?? new CategoriaResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
            }
            await ThrowDetailedException(response, nameof(RemoverCategoria));
            throw new Exception("Falha desconhecida ao remover categoria.");
        }

        private static async Task ThrowDetailedException(HttpResponseMessage response, string operation)
        {
            var status = (int)response.StatusCode;
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"CategoriaService.{operation} - Status: {status}, Body: {body}");

            // Tenta desserializar um envelope conhecido
            try
            {
                var apiError = System.Text.Json.JsonSerializer.Deserialize<ApiErrorEnvelope>(body);
                if (apiError != null)
                {
                    var msg = apiError.message ?? $"Erro na API ({status})";
                    throw new Exception($"{msg}");
                }
            }
            catch { }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new Exception("Não autorizado (401). Faça login novamente.");
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new Exception("Acesso negado (403). Permissões insuficientes.");

            throw new Exception($"Erro na API ({status}): {body}");
        }

        private class ApiErrorEnvelope
        {
            public bool success { get; set; }
            public string? message { get; set; }
            public object? data { get; set; }
        }
    }
}
