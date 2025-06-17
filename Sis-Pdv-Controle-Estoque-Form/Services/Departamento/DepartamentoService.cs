using Commands.Departamento.AdicionarDepartamento;
using Commands.Departamento.AlterarDepartamento;
using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Departamento
{
    public class DepartamentoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<DepartamentoResponse> AdicionarDepartamento(DepartamentoDto dto)
        {
            _client = new HttpClient();
            AdicionarDepartamentoRequest request = new AdicionarDepartamentoRequest()
            {
                NomeDepartamento = dto.NomeDepartamento
            };

            var response = await _client.PostAsJson($"{BasePath}/Departamento/AdicionarDepartamento", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<DepartamentoResponseList> ListarDepartamento()
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamento");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<DepartamentoResponseList> ListarDepartamentoPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamentoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<DepartamentoResponseList> ListarDepartamentoPorNomeDepartamento(string NomeDepartamento)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamentoPorNomeDepartamento/{NomeDepartamento}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<DepartamentoResponse> AlterarDepartamento(DepartamentoDto dto)
        {
            _client = new HttpClient();

            AlterarDepartamentoRequest request = new AlterarDepartamentoRequest()
            {
                Id = Guid.Parse(dto.Id),
                NomeDepartamento = dto.NomeDepartamento
            };

            var response = await _client.PutAsJson($"{BasePath}/Departamento/AlterarDepartamento", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<DepartamentoResponse> RemoverDepartamento(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Departamento/RemoverDepartamento/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<DepartamentoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
