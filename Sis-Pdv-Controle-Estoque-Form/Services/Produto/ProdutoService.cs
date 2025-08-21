using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.AtualizarEstoque;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Produto
{
    public class ProdutoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        
        public ProdutoService()
        {
            _client = Services.Http.HttpClientManager.GetClient();
        }

        public async Task<ProdutoResponse> AdicionarProduto(ProdutoDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    throw new ArgumentException($"Dados inválidos: {string.Join(", ", errosValidacao)}");
                }

                _client = Services.Http.HttpClientManager.GetClient();

                AdicionarProdutoRequest request = new AdicionarProdutoRequest()
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
                System.Diagnostics.Debug.WriteLine($"URL: {BasePath}/Produto/AdicionarProduto");

                var response = await _client.PostAsJson($"{BasePath}/Produto/AdicionarProduto", request);
                
                System.Diagnostics.Debug.WriteLine($"Status Code: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponse>();
                        return result ?? new ProdutoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AdicionarProduto: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                // Log detalhado do erro
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Erro na API - Status: {response.StatusCode}, Content: {errorContent}");

                await ThrowDetailedException(response, nameof(AdicionarProduto));
                throw new Exception("Falha desconheida ao adicionar produto.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException || ex is ArgumentNullException))
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<ProdutoResponseList> ListarProduto()
        {
            try
            {
                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Produto/ListarProduto");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponseList>();
                        return result ?? new ProdutoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarProduto: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarProduto));
                throw new Exception("Falha desconhecida ao listar produtos.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
        }

        public async Task<ProdutoResponseList> ListarProdutoPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Produto/ListarProdutoPorId/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponseList>();
                        return result ?? new ProdutoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarProdutoPorId: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarProdutoPorId));
                throw new Exception("Falha desconhecida ao consultar produto por Id.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
        }

        public async Task<ProdutoResponseList> ListarProdutoPorCodBarras(string codBarras)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codBarras))
                    throw new ArgumentException("Código de barras é obrigatório.");

                // Remove espaços e caracteres especiais
                codBarras = codBarras.Trim().Replace(" ", "").Replace("-", "");

                if (!System.Text.RegularExpressions.Regex.IsMatch(codBarras, @"^[0-9]+$"))
                    throw new ArgumentException("Código de barras deve conter apenas números.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Produto/ListarProdutoPorCodBarras/{Uri.EscapeDataString(codBarras)}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponseList>();
                        return result ?? new ProdutoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarProdutoPorCodBarras: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarProdutoPorCodBarras));
                throw new Exception("Falha desconhecida ao consultar produto por código de barras.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }
        }

        public async Task<ProdutoResponse> AlterarProduto(ProdutoDto dto)
        {
            try
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

                _client = Services.Http.HttpClientManager.GetClient();

                AlterarProdutoRequest request = new AlterarProdutoRequest()
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

                var response = await _client.PutAsJson($"{BasePath}/Produto/AlterarProduto", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponse>();
                        return result ?? new ProdutoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AlterarProduto: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AlterarProduto));
                throw new Exception("Falha desconhecida ao alterar produto.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException || ex is ArgumentNullException))
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }
        }



        public async Task<ProdutoResponse> RemoverProduto(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.DeleteAsync($"{BasePath}/Produto/RemoverProduto/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponse>();
                        return result ?? new ProdutoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de RemoverProduto: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(RemoverProduto));
                throw new Exception("Falha desconhecida ao remover produto.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunicação com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza o estoque de um produto
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidade">Nova quantidade em estoque</param>
        /// <returns>Resposta da API</returns>
        public async Task<ProdutoResponse> AtualizarEstoque(string produtoId, int quantidade)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(produtoId))
                    throw new ArgumentException("ID do produto é obrigatório", nameof(produtoId));

                if (quantidade < 0)
                    throw new ArgumentException("Quantidade não pode ser negativa", nameof(quantidade));

                var comando = new { ProdutoId = produtoId, QuantidadeEstoque = quantidade };

                var jsonContent = JsonSerializer.Serialize(comando);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"api/Produto/estoque/{produtoId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ProdutoResponse>(responseContent);
                    return result ?? new ProdutoResponse { success = false };
                }

                await ThrowDetailedException(response, nameof(AtualizarEstoque));
                return new ProdutoResponse { success = false };
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisição: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new Exception($"Erro inesperado: {ex.Message}");
            }
        }

        private async Task ThrowDetailedException(HttpResponseMessage response, string methodName)
        {
            var content = "";
            try
            {
                content = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                content = "Não foi possível ler o conteúdo da resposta";
            }

            var message = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Dados inválidos enviados para o servidor",
                HttpStatusCode.Unauthorized => "Não autorizado. Verifique suas credenciais",
                HttpStatusCode.Forbidden => "Acesso negado",
                HttpStatusCode.NotFound => GetNotFoundMessage(methodName),
                HttpStatusCode.Conflict => GetConflictMessage(methodName),
                HttpStatusCode.UnprocessableEntity => "Dados inconsistentes",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.ServiceUnavailable => "Serviço temporariamente indisponível",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            throw new Exception($"{methodName} falhou: {message}. Detalhes: {content}");
        }

        private string GetNotFoundMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AdicionarProduto) => "Erro ao cadastrar: Fornecedor ou Categoria não encontrados",
                nameof(AlterarProduto) => "Produto não encontrado para alteração",
                nameof(RemoverProduto) => "Produto não encontrado para remoção",
                nameof(ListarProdutoPorId) => "Produto não encontrado",
                nameof(ListarProdutoPorCodBarras) => "Produto com este código de barras não foi encontrado",
                nameof(AtualizarEstoque) => "Produto não encontrado para atualização de estoque",
                _ => "Recurso não encontrado"
            };
        }

        private string GetConflictMessage(string methodName)
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
