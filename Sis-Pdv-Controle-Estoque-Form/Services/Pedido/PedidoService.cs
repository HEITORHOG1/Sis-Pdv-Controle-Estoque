using Commands.Pedidos.AdicionarPedido;
using Commands.Pedidos.AlterarPedido;
using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Pedido
{
    public class PedidoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");

        private HttpClient GetClient() => Services.Http.HttpClientManager.GetClient();
        public async Task<PedidoResponse> AdicionarPedido(PedidoDto dto)
        {
            _client = GetClient();
            AdicionarPedidoRequest request = new AdicionarPedidoRequest()
            {
                ColaboradorId = dto.ColaboradorId,
                ClienteId = dto.ClienteId,
                Status = dto.Status,
                DataDoPedido = dto.DataDoPedido,
                FormaPagamento = dto.FormaPagamento,
                TotalPedido = dto.TotalPedido
            };

            var response = await _client.PostAsJson($"{BasePath}/Pedido/AdicionarPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            
            // Le o corpo do erro para diagnostico
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar pedido (HTTP {(int)response.StatusCode}): {errorBody}");
        }
        public async Task<PedidoResponseList> ListarPedido()
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedido");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponseList> ListarPedidoPorId(string id)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedidoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponseList> ListarPedidoPorNomePedido(string Cnpj)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedidoPorCnpj/{Cnpj}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponse> AlterarPedido(PedidoDto dto)
        {
            _client = GetClient();

            AlterarPedidoRequest request = new AlterarPedidoRequest()
            {
                Id = dto.Id,
                ColaboradorId = dto.ColaboradorId,
                ClienteId = dto.ClienteId,
                Status = dto.Status,
                DataDoPedido = dto.DataDoPedido,
                FormaPagamento = dto.FormaPagamento,
                TotalPedido = dto.TotalPedido
            };

            var response = await _client.PutAsJson($"{BasePath}/Pedido/AlterarPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao alterar pedido (HTTP {(int)response.StatusCode}): {errorBody}");
        }
        public async Task<PedidoResponse> RemoverPedido(string id)
        {
            _client = GetClient();

            var response = await _client.DeleteAsync($"{BasePath}/Pedido/RemoverPedido/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<PedidoResponseListGrid> ListarVendaPedidoPorData(DateTime DataInicio, DateTime DataFim)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarVendaPedidoPorData/{DataInicio.Date.ToString("yyyy-MM-dd")}/{DataFim.Date.ToString("yyyy-MM-dd")}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseListGrid>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
