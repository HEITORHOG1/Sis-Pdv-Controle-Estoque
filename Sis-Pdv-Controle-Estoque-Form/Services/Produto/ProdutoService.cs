using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using Sis_Pdv_Controle_Estoque_Form.IServices;
using Sis_Pdv_Controle_Estoque_Form.Services.Base;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Produto
{
    /// <summary>
    /// Serviço para operações de dados mestres de produtos
    /// (Para operações de estoque use IInventoryService)
    /// </summary>
    public class ProdutoService : BaseApiService, IProdutoService
    {
        public ProdutoService() : base(BaseAppConfig.ReadSetting("Base"))
        {
        }

        public async Task<ProdutoResponse> AdicionarProduto(ProdutoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Valida o DTO
            var errosValidacao = dto.Validar();
            if (errosValidacao.Any())
            {
                throw new ArgumentException($"Dados inválidos: {string.Join(", ", errosValidacao)}");
            }

            var request = new AdicionarProdutoRequest()
            {
                codBarras = dto.codBarras.Trim(),
                nomeProduto = dto.nomeProduto.Trim(),
                descricaoProduto = dto.descricaoProduto?.Trim() ?? string.Empty,
                isPerecivel = dto.isPerecivel,
                FornecedorId = dto.FornecedorId,
                CategoriaId = dto.CategoriaId,
                statusAtivo = dto.statusAtivo
            };

            System.Diagnostics.Debug.WriteLine($"Tentando adicionar produto: {request.nomeProduto}");
            System.Diagnostics.Debug.WriteLine($"FornecedorId: {request.FornecedorId}");
            System.Diagnostics.Debug.WriteLine($"CategoriaId: {request.CategoriaId}");

            return await ExecuteWithRetry<ProdutoResponse>(
                () => _client.PostAsJson($"{_basePath}/Produto/AdicionarProduto", request),
                nameof(AdicionarProduto)
            );
        }

        public async Task<ProdutoResponseList> ListarProduto()
        {
            return await ExecuteWithRetry<ProdutoResponseList>(
                () => _client.GetAsync($"{_basePath}/Produto/ListarProduto"),
                nameof(ListarProduto)
            );
        }

        public async Task<ProdutoResponseList> ListarProdutoPorId(string id)
        {
            ValidateGuid(id, nameof(id));

            return await ExecuteWithRetry<ProdutoResponseList>(
                () => _client.GetAsync($"{_basePath}/Produto/ListarProdutoPorId/{id}"),
                nameof(ListarProdutoPorId)
            );
        }

        public async Task<ProdutoResponseList> ListarProdutoPorCodBarras(string codBarras)
        {
            ValidateRequired(codBarras, nameof(codBarras));

            // Remove espaços e caracteres especiais
            codBarras = codBarras.Trim().Replace(" ", "").Replace("-", "");

            if (!System.Text.RegularExpressions.Regex.IsMatch(codBarras, @"^[0-9]+$"))
                throw new ArgumentException("Código de barras deve conter apenas números.");

            return await ExecuteWithRetry<ProdutoResponseList>(
                () => _client.GetAsync($"{_basePath}/Produto/ListarProdutoPorCodBarras/{Uri.EscapeDataString(codBarras)}"),
                nameof(ListarProdutoPorCodBarras)
            );
        }

        public async Task<ProdutoResponse> AlterarProduto(ProdutoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if (dto.Id == Guid.Empty)
                throw new ArgumentException("Id é obrigatório.");

            // Valida o DTO
            var errosValidacao = dto.Validar();
            if (errosValidacao.Any())
            {
                throw new ArgumentException($"Dados inválidos: {string.Join(", ", errosValidacao)}");
            }

            var request = new AlterarProdutoRequest()
            {
                Id = dto.Id,
                codBarras = dto.codBarras.Trim(),
                nomeProduto = dto.nomeProduto.Trim(),
                descricaoProduto = dto.descricaoProduto?.Trim() ?? string.Empty,
                isPerecivel = dto.isPerecivel,
                FornecedorId = dto.FornecedorId,
                CategoriaId = dto.CategoriaId,
                statusAtivo = dto.statusAtivo
            };

            return await ExecuteWithRetry<ProdutoResponse>(
                () => _client.PutAsJson($"{_basePath}/Produto/AlterarProduto", request),
                nameof(AlterarProduto)
            );
        }

        public async Task<ProdutoResponse> RemoverProduto(string id)
        {
            ValidateGuid(id, nameof(id));

            return await ExecuteWithRetry<ProdutoResponse>(
                () => _client.DeleteAsync($"{_basePath}/Produto/RemoverProduto/{id}"),
                nameof(RemoverProduto)
            );
        }

        protected override string GetNotFoundMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AdicionarProduto) => "Erro ao cadastrar: Fornecedor ou Categoria não encontrados",
                nameof(AlterarProduto) => "Produto não encontrado para alteração",
                nameof(RemoverProduto) => "Produto não encontrado para remoção",
                nameof(ListarProdutoPorId) => "Produto não encontrado",
                nameof(ListarProdutoPorCodBarras) => "Produto com este código de barras não foi encontrado",
                _ => "Recurso não encontrado"
            };
        }

        protected override string GetConflictMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AdicionarProduto) => "Produto já existe com este código de barras",
                nameof(AlterarProduto) => "Conflito ao alterar produto - código de barras já existe",
                _ => "Conflito de dados"
            };
        }
    }
}
