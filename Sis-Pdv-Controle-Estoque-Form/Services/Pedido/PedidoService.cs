using Commands.Pedido.AdicionarPedido;
using Commands.Pedido.AlterarPedido;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Pedido
{
    public class PedidoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<PedidoResponse> AdicionarPedido(PedidoDto dto)
        {
            _client = new HttpClient();
            AdicionarPedidoRequest request = new AdicionarPedidoRequest()
            {
                ColaboradorId = dto.ColaboradorId,
                ClienteId = dto.ClienteId,
                Status = dto.Status,
                dataDoPedido = dto.dataDoPedido,
                formaPagamento = dto.formaPagamento,
                totalPedido = dto.totalPedido
            };

            var response = await _client.PostAsJson($"{BasePath}/Pedido/AdicionarPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponseList> ListarPedido()
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedido");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponseList> ListarPedidoPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedidoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponseList> ListarPedidoPorNomePedido(string Cnpj)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarPedidoPorNomePedido/{Cnpj}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponse> AlterarPedido(PedidoDto dto)
        {
            _client = new HttpClient();

            AlterarPedidoRequest request = new AlterarPedidoRequest()
            {
                Id = dto.Id,
                ColaboradorId = dto.ColaboradorId,
                ClienteId = dto.ClienteId,
                Status = dto.Status,
                dataDoPedido = dto.dataDoPedido,
                formaPagamento = dto.formaPagamento,
                totalPedido = dto.totalPedido
            };

            var response = await _client.PutAsJson($"{BasePath}/Pedido/AlterarPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<PedidoResponse> RemoverPedido(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Pedido/RemoverPedido/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<PedidoResponseListGrid> ListarVendaPedidoPorData(DateTime DataInicio,DateTime DataFim)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Pedido/ListarVendaPedidoPorData/{DataInicio.Date.ToString("yyyy-MM-dd")}/{DataFim.Date.ToString("yyyy-MM-dd")}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<PedidoResponseListGrid>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
