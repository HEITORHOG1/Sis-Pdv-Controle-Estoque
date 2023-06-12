using Commands.ProdutoPedido.AdicionarProdutoPedido;
using Commands.ProdutoPedido.AlterarProdutoPedido;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Security.Cryptography;

namespace Sis_Pdv_Controle_Estoque_Form.Services.ProdutoPedido
{
    public class ProdutoPedidoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<ProdutoPedidoResponse> AdicionarProdutoPedido(ProdutoPedidoDto dto)
        {
            _client = new HttpClient();
            AdicionarProdutoPedidoRequest request = new AdicionarProdutoPedidoRequest()
            {
                codBarras = dto.codBarras,
                PedidoId = dto.PedidoId,
                ProdutoId = dto.ProdutoId,
                quantidadeItemPedido = dto.quantidadeItemPedido,
                totalProdutoPedido = dto.totalProdutoPedido
            };


            var response = await _client.PostAsJson($"{BasePath}/ProdutoPedido/AdicionarProdutoPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedido()
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedido");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedidoPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedidoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponseList> ListarProdutoPedidoPorCodBarras(string codBarras)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutoPedidoPorCodBarras/{codBarras}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponse> AlterarProdutoPedido(ProdutoPedidoDto dto)
        {
            _client = new HttpClient();

            AlterarProdutoPedidoRequest request = new AlterarProdutoPedidoRequest()
            {
                Id = dto.Id,
                codBarras = dto.codBarras,
                PedidoId = dto.PedidoId,
                ProdutoId = dto.ProdutoId,
                quantidadeItemPedido = dto.quantidadeItemPedido,
                totalProdutoPedido = dto.totalProdutoPedido
            };

            var response = await _client.PutAsJson($"{BasePath}/ProdutoPedido/AlterarProdutoPedido", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoPedidoResponse> RemoverProdutoPedido(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/ProdutoPedido/RemoverProdutoPedido/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProdutoPedidoResponseListGrid> ListarProdutosPorPedidoId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/ProdutoPedido/ListarProdutosPorPedidoId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoPedidoResponseListGrid>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
