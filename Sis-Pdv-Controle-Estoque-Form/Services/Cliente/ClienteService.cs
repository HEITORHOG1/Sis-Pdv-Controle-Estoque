using Microsoft.Extensions.Logging;
using Sis_Pdv_Controle_Estoque_Form.Dto.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Cliente
{
    public class ClienteService
    {
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        private readonly HttpClient _client;
        private readonly ILogger<CategoriaService> _logger;

        public ClienteService(HttpClient client, ILogger<CategoriaService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ClienteService()
        {
        }

        public async Task<ClienteResponse> Adicionar(ClienteDto dto)
        {
            AdicionarClienteRequest request = new AdicionarClienteRequest()
            {
                CpfCnpj = dto.CpfCnpj,
                TipoCliente = dto.tipoCliente
            };

            _logger.LogInformation("Enviando requisição para Adicionar Cliente.");

            var response = await _client.PostAsJson($"{BasePath}/Cliente/AdicionarCliente", request);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Requisição para Adicionar Cliente foi bem sucedida.");
                return await response.ReadContentAs<ClienteResponse>();
            }
            else
            {
                _logger.LogError("Algo deu errado ao chamar a API para Adicionar Cliente.");
                throw new Exception("Algo deu errado ao chamar a API");
            }
        }


        public async Task<ClienteResponseList> ListarClientePorNomeCliente(string NomeCliente)
        {
            var response = await _client.GetAsync($"{BasePath}/Cliente/ListarClientePorNomeCliente/{NomeCliente}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ClienteResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
