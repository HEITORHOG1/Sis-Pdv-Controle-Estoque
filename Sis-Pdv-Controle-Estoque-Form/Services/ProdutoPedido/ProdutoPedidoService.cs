using Commands.ProdutoPedido.AdicionarProdutoPedido;
using Commands.ProdutoPedido.AlterarProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido
{
    public class ProdutoPedidoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");

        private HttpClient GetClient() => Services.Http.HttpClientManager.GetClient();
        public async Task<ProdutoPedidoResponse> AdicionarProdutoPedido(ProdutoPedidoDto dto)
        {
            _client = GetClient();
            AdicionarProdutoPedidoRequest request = new AdicionarProdutoPedidoRequest()
            {
                CodBarras = dto.CodBarras,
                PedidoId = dto.PedidoId,
                ProdutoId = dto.ProdutoId,
                QuantidadeItemPedido = dto.QuantidadeItemPedido,
                TotalProdutoPedido = dto.TotalProdutoPedido
            };


            var response = await _client.PostAsJson($"{BasePath}/ProdutoPedido/AdicionarProdutoPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao adicionar produto ao pedido (HTTP {(int)response.StatusCode}): {errorBody}");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedido()
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedido");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedidoPorId(string id)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedidoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedidoPorCodBarras(string CodBarras)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedidoPorCodigoDeBarras/{CodBarras}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponse> AlterarProdutoPedido(ProdutoPedidoDto dto)
        {
            _client = GetClient();

            AlterarProdutoPedidoRequest request = new AlterarProdutoPedidoRequest()
            {
                Id = dto.Id,
                CodBarras = dto.CodBarras,
                PedidoId = dto.PedidoId,
                ProdutoId = dto.ProdutoId,
                QuantidadeItemPedido = dto.QuantidadeItemPedido,
                TotalProdutoPedido = dto.TotalProdutoPedido
            };

            var response = await _client.PutAsJson($"{BasePath}/ProdutoPedido/AlterarProdutoPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponse> RemoverProdutoPedido(string id)
        {
            _client = GetClient();

            var response = await _client.DeleteAsync($"{BasePath}/ProdutoPedido/RemoverProdutoPedido/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProdutoPedidoResponseListGrid> ListarProdutosPorPedidoId(string id)
        {
            _client = GetClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutosPorPedidoId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseListGrid>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
