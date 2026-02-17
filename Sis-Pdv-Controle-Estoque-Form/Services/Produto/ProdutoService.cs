using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.AtualizarEstoque;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net;
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
                    throw new ArgumentException($"Dados inv�lidos: {string.Join(", ", errosValidacao)}");
                }

                _client = Services.Http.HttpClientManager.GetClient();

                AdicionarProdutoRequest request = new AdicionarProdutoRequest()
                {
                    CodBarras = dto.CodBarras.Trim(),
                    NomeProduto = dto.NomeProduto.Trim(),
                    DescricaoProduto = dto.DescricaoProduto?.Trim() ?? string.Empty,
                    PrecoCusto = dto.PrecoCusto,
                    PrecoVenda = dto.PrecoVenda,
                    MargemLucro = dto.MargemLucro,
                    DataFabricao = dto.DataFabricao,
                    DataVencimento = dto.DataVencimento,
                    QuantidadeEstoqueProduto = dto.QuantidadeEstoqueProduto,
                    FornecedorId = dto.FornecedorId,
                    CategoriaId = dto.CategoriaId,
                    StatusAtivo = dto.StatusAtivo
                };

                System.Diagnostics.Debug.WriteLine($"Tentando adicionar produto: {request.NomeProduto}");
                System.Diagnostics.Debug.WriteLine($"FornecedorId: {request.FornecedorId}");
                System.Diagnostics.Debug.WriteLine($"CategoriaId: {request.CategoriaId}");
                System.Diagnostics.Debug.WriteLine($"URL: {BasePath}/v1/produto/AdicionarProduto");

                var response = await _client.PostAsJson($"{BasePath}/v1/produto/AdicionarProduto", request);
                
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
                throw new Exception("Falha desconhecida ao adicionar produto.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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

                var response = await _client.GetAsync($"{BasePath}/v1/produto/ListarProduto");
                
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
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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
                    throw new ArgumentException("Id � obrigat�rio.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID v�lido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/v1/produto/ListarProdutoPorId/{id}");
                
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
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao processar dados JSON: {ex.Message}");
            }
        }

        public async Task<ProdutoResponseList> ListarProdutoPorCodBarras(string CodBarras)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CodBarras))
                    throw new ArgumentException("C�digo de barras � obrigat�rio.");

                // Remove espa�os e caracteres especiais
                CodBarras = CodBarras.Trim().Replace(" ", "").Replace("-", "");

                if (!System.Text.RegularExpressions.Regex.IsMatch(CodBarras, @"^[0-9]+$"))
                    throw new ArgumentException("C�digo de barras deve conter apenas n�meros.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/v1/produto/ListarProdutoPorCodBarras/{Uri.EscapeDataString(CodBarras)}");
                
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
                throw new Exception("Falha desconhecida ao consultar produto por c�digo de barras.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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
                    throw new ArgumentException("Id � obrigat�rio.");

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    throw new ArgumentException($"Dados inv�lidos: {string.Join(", ", errosValidacao)}");
                }

                _client = Services.Http.HttpClientManager.GetClient();

                AlterarProdutoRequest request = new AlterarProdutoRequest()
                {
                    Id = dto.Id,
                    CodBarras = dto.CodBarras.Trim(),
                    NomeProduto = dto.NomeProduto.Trim(),
                    DescricaoProduto = dto.DescricaoProduto?.Trim() ?? string.Empty,
                    PrecoCusto = dto.PrecoCusto,
                    PrecoVenda = dto.PrecoVenda,
                    MargemLucro = dto.MargemLucro,
                    DataFabricao = dto.DataFabricao,
                    DataVencimento = dto.DataVencimento,
                    QuantidadeEstoqueProduto = dto.QuantidadeEstoqueProduto,
                    FornecedorId = dto.FornecedorId,
                    CategoriaId = dto.CategoriaId,
                    StatusAtivo = dto.StatusAtivo
                };

                var response = await _client.PutAsJson($"{BasePath}/v1/produto/AlterarProduto", request);
                
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
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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

        public async Task<ProdutoResponse> AtualizarEstoque(ProdutoDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                
                if (dto.Id == Guid.Empty)
                    throw new ArgumentException("Id � obrigat�rio.");

                if (dto.QuantidadeEstoqueProduto < 0)
                    throw new ArgumentException("Quantidade n�o pode ser negativa.");

                _client = Services.Http.HttpClientManager.GetClient();

                AtualizarEstoqueRequest request = new AtualizarEstoqueRequest()
                {
                    Id = dto.Id,
                    QuantidadeEstoqueProduto = dto.QuantidadeEstoqueProduto
                };

                var response = await _client.PutAsJson($"{BasePath}/v1/produto/AtualizaEstoque", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ProdutoResponse>();
                        return result ?? new ProdutoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AtualizarEstoque: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AtualizarEstoque));
                throw new Exception("Falha desconhecida ao atualizar estoque.");
            }
            catch (ApplicationException appEx)
            {
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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
                    throw new ArgumentException("Id � obrigat�rio.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID v�lido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.DeleteAsync($"{BasePath}/v1/produto/RemoverProduto/{id}");
                
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
                throw new Exception($"Erro na comunica��o com API: {appEx.Message}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro de conex�o: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Timeout na requisi��o: {ex.Message}");
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
                content = "N�o foi poss�vel ler o conte�do da resposta";
            }

            var message = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Dados inv�lidos enviados para o servidor",
                HttpStatusCode.Unauthorized => "N�o autorizado. Verifique suas credenciais",
                HttpStatusCode.Forbidden => "Acesso negado",
                HttpStatusCode.NotFound => GetNotFoundMessage(methodName),
                HttpStatusCode.Conflict => GetConflictMessage(methodName),
                HttpStatusCode.UnprocessableEntity => "Dados inconsistentes",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.ServiceUnavailable => "Servi�o temporariamente indispon�vel",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            throw new Exception($"{methodName} falhou: {message}. Detalhes: {content}");
        }

        private string GetNotFoundMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AdicionarProduto) => "Erro ao cadastrar: Fornecedor ou Categoria n�o encontrados",
                nameof(AlterarProduto) => "Produto n�o encontrado para altera��o",
                nameof(RemoverProduto) => "Produto n�o encontrado para remo��o",
                nameof(ListarProdutoPorId) => "Produto n�o encontrado",
                nameof(ListarProdutoPorCodBarras) => "Produto com este c�digo de barras n�o foi encontrado",
                nameof(AtualizarEstoque) => "Produto n�o encontrado para atualiza��o de estoque",
                _ => "Recurso n�o encontrado"
            };
        }

        private string GetConflictMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AdicionarProduto) => "Produto j� existe com este c�digo de barras",
                nameof(AlterarProduto) => "Conflito ao alterar produto - c�digo de barras j� existe",
                _ => "Conflito de dados"
            };
        }
    }
}
