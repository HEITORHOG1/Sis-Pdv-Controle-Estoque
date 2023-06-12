using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.AlterarCategoria;
using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Categoria
{
    public class CategoriaService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<CategoriaResponse> Adicionar(CategoriaDto dto)
        {
            _client = new HttpClient();
            AdicionarCategoriaRequest request = new AdicionarCategoriaRequest()
            {
                NomeCategoria = dto.NomeCategoria
            };

            var response = await _client.PostAsJson($"{BasePath}/Categoria/AdicionarCategoria", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<CategoriaResponseList> ListarCategoria()
        {
            _client = new HttpClient();
           
            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoria");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<CategoriaResponseList> ListarCategoriaPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoriaPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<CategoriaResponseList> ListarCategoriaPorNomeCategoria(string NomeCategoria)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Categoria/ListarCategoriaPorNomeCategoria/{NomeCategoria}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<CategoriaResponse> AlterarCategoria(CategoriaDto dto)
        {
            _client = new HttpClient();

            AlterarCategoriaRequest request = new AlterarCategoriaRequest()
            {
                Id = dto.id,
                NomeCategoria = dto.NomeCategoria
            };

            var response = await _client.PutAsJson($"{BasePath}/Categoria/AlterarCategoria", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<CategoriaResponse> RemoverCategoria(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Categoria/RemoverCategoria/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CategoriaResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
