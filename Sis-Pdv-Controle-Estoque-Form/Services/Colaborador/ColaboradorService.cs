using Commands.Colaborador.AdicionarColaborador;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Colaborador
{
    public class ColaboradorService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<ColaboradorResponse> AdicionarColaborador(ColaboradorDto dto)
        {
            _client = new HttpClient();
            AdicionarColaboradorRequest request = new AdicionarColaboradorRequest()
            {
                nomeColaborador = dto.nomeColaborador,
                cargoColaborador = dto.cargoColaborador,
                cpfColaborador = dto.cpfColaborador,
                emailCorporativo = dto.emailCorporativo,
                emailPessoalColaborador = dto.emailPessoalColaborador,
                telefoneColaborador = dto.telefoneColaborador,
                DepartamentoId = Guid.Parse(dto.departamentoId),
                Usuario = new Model.Usuario
                {
                    Senha = dto.senha,
                    Login = dto.login,
                    statusAtivo = dto.statusAtivo
                },
            };

            var response = await _client.PostAsJson($"{BasePath}/Colaborador/AdicionarColaborador", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ColaboradorResponseList> ListarColaborador()
        {

            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaborador");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ColaboradorResponseList> ListarColaboradorPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaboradorPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ColaboradorResponseList> ListarColaboradorPorNomeColaborador(string NomeColaborador)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaboradorPorNomeColaborador/{NomeColaborador}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ColaboradorResponse> AlterarColaborador(ColaboradorDto dto)
        {
            _client = new HttpClient();

            AdicionarColaboradorRequest request = new AdicionarColaboradorRequest()
            {
                Id = Guid.Parse(dto.id),
                nomeColaborador = dto.nomeColaborador,
                cargoColaborador = dto.cargoColaborador,
                cpfColaborador = dto.cpfColaborador,
                emailCorporativo = dto.emailCorporativo,
                emailPessoalColaborador = dto.emailPessoalColaborador,
                telefoneColaborador = dto.telefoneColaborador,
                DepartamentoId = Guid.Parse(dto.departamentoId),
                Usuario = new Model.Usuario
                {
                    Id = Guid.Parse(dto.idlogin),
                    Senha = dto.senha,
                    Login = dto.login,
                    statusAtivo = dto.statusAtivo
                },
            };

            var response = await _client.PutAsJson($"{BasePath}/Colaborador/AlterarColaborador", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ColaboradorResponse> RemoverColaborador(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Colaborador/RemoverColaborador/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<ColaboradorResponse> ValidarLogin(string Login, string Senha)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Colaborador/ValidarLogin/{Login}/{Senha}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ColaboradorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
