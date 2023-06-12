using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Security.Cryptography;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor
{
    public class FornecedorService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<FornecedorResponse> AdicionarFornecedor(FornecedorDto dto)
        {
            _client = new HttpClient();
            AdicionarFornecedorRequest request = new AdicionarFornecedorRequest()
            {
                inscricaoEstadual = dto.inscricaoEstadual,
                nomeFantasia = dto.nomeFantasia,
                Uf = dto.Uf,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                cepFornecedor = dto.cepFornecedor,
                statusAtivo = dto.statusAtivo,
                Cnpj = dto.Cnpj,
                Rua = dto.Rua
            };

            var response = await _client.PostAsJson($"{BasePath}/Fornecedor/AdicionarFornecedor", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<FornecedorResponseList> ListarFornecedor()
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedor");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<FornecedorResponseList> ListarFornecedorPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedorPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<FornecedorResponseList> ListarFornecedorPorNomeFornecedor(string Cnpj)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedorPorNomeFornecedor/{Cnpj}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<FornecedorResponse> AlterarFornecedor(FornecedorDto dto)
        {
            _client = new HttpClient();

            AlterarFornecedorRequest request = new AlterarFornecedorRequest()
            {
                Id = Guid.Parse(dto.Id),
                inscricaoEstadual = dto.inscricaoEstadual,
                nomeFantasia = dto.nomeFantasia,
                Uf = dto.Uf,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                cepFornecedor = dto.cepFornecedor,
                statusAtivo = dto.statusAtivo,
                Cnpj = dto.Cnpj,
                Rua = dto.Rua
            };

            var response = await _client.PutAsJson($"{BasePath}/Fornecedor/AlterarFornecedor", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<FornecedorResponse> RemoverFornecedor(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Fornecedor/RemoverFornecedor/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<FornecedorResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
