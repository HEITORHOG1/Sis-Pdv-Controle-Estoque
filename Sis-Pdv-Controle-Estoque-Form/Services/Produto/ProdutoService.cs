using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.AtualizarEstoque;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Security.Cryptography;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Produto
{
    public class ProdutoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        public async Task<ProdutoResponse> AdicionarProduto(ProdutoDto dto)
        {
            _client = new HttpClient();
            AdicionarProdutoRequest request = new AdicionarProdutoRequest()
            {
                codBarras = dto.codBarras,
                nomeProduto = dto.nomeProduto,
                descricaoProduto = dto.descricaoProduto,
                precoCusto = dto.precoCusto,
                precoVenda = dto.precoVenda,
                margemLucro = dto.margemLucro,
                dataFabricao = dto.dataFabricao,
                dataVencimento = dto.dataVencimento,
                quatidadeEstoqueProduto = dto.quatidadeEstoqueProduto,
                FornecedorId = dto.FornecedorId,
                CategoriaId = dto.CategoriaId,
                statusAtivo = dto.statusAtivo
            };


            var response = await _client.PostAsJson($"{BasePath}/Produto/AdicionarProduto", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoResponseList> ListarProduto()
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Produto/ListarProduto");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoResponseList> ListarProdutoPorId(string id)
        {
            _client = new HttpClient();

            var response = await _client.GetAsync($"{BasePath}/Produto/ListarProdutoPorId/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponseList>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoResponseList> ListarProdutoPorCodBarras(string codBarras)
        {
            try
            {
                _client = new HttpClient();

                var response = await _client.GetAsync($"{BasePath}/Produto/ListarProdutoPorCodBarras/{codBarras}");

                if (response.IsSuccessStatusCode)
                    return await response.ReadContentAs<ProdutoResponseList>();
                else
                    throw new Exception("Something went wrong when calling API");
            }
            catch (Exception ex)
            {
                // Aqui você pode lidar com a exceção de acordo com a lógica do seu aplicativo
                // Por exemplo, exibir uma mensagem de erro, registrar o erro em um log, etc.
                Console.WriteLine("Error occurred while calling the API: " + ex.Message);
                // Ou, se preferir, você pode lançar uma exceção personalizada com uma mensagem mais específica
                throw new Exception("Failed to retrieve product information. Please try again later.", ex);
            }
        }

        public async Task<ProdutoResponse> AlterarProduto(ProdutoDto dto)
        {
            _client = new HttpClient();

            AlterarProdutoRequest request = new AlterarProdutoRequest()
            {
                Id = dto.Id,
                codBarras = dto.codBarras,
                nomeProduto = dto.nomeProduto,
                descricaoProduto = dto.descricaoProduto,
                precoCusto = dto.precoCusto,
                precoVenda = dto.precoVenda,
                margemLucro = dto.margemLucro,
                dataFabricao = dto.dataFabricao,
                dataVencimento = dto.dataVencimento,
                quatidadeEstoqueProduto = dto.quatidadeEstoqueProduto,
                FornecedorId = dto.FornecedorId,
                CategoriaId = dto.CategoriaId,
                statusAtivo = dto.statusAtivo
            };

            var response = await _client.PutAsJson($"{BasePath}/Produto/AlterarProduto", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<ProdutoResponse> AtualizarEstoque(ProdutoDto dto)
        {
            _client = new HttpClient();

            AtualizarEstoqueRequest request = new AtualizarEstoqueRequest()
            {
                Id = dto.Id,
                quatidadeEstoqueProduto = dto.quatidadeEstoqueProduto
            };

            var response = await _client.PutAsJson($"{BasePath}/Produto/AtualizaEstoque", request);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProdutoResponse> RemoverProduto(string id)
        {
            _client = new HttpClient();

            var response = await _client.DeleteAsync($"{BasePath}/Produto/RemoverProduto/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProdutoResponse>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
