using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Fornecedor
{
    public class FornecedorService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        
        public FornecedorService()
        {
            _client = Services.Http.HttpClientManager.GetClient();
        }

        public async Task<FornecedorResponse> AdicionarFornecedor(FornecedorDto dto)
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

                AdicionarFornecedorRequest request = new AdicionarFornecedorRequest()
                {
                    InscricaoEstadual = dto.InscricaoEstadual?.Trim() ?? string.Empty,
                    NomeFantasia = dto.NomeFantasia.Trim(),
                    Uf = dto.Uf.Trim().ToUpper(),
                    Numero = dto.Numero?.Trim() ?? string.Empty,
                    Complemento = dto.Complemento?.Trim() ?? string.Empty,
                    Bairro = dto.Bairro.Trim(),
                    Cidade = dto.Cidade.Trim(),
                    CepFornecedor = dto.CepFornecedor,
                    StatusAtivo = dto.StatusAtivo,
                    Cnpj = dto.Cnpj.Trim(),
                    Rua = dto.Rua.Trim()
                };

                System.Diagnostics.Debug.WriteLine($"Tentando adicionar fornecedor: {request.NomeFantasia}");

                var response = await _client.PostAsJson($"{BasePath}/Fornecedor/AdicionarFornecedor", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponse>();
                        return result ?? new FornecedorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AdicionarFornecedor: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AdicionarFornecedor));
                throw new Exception("Falha desconhecida ao adicionar fornecedor.");
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

        public async Task<FornecedorResponseList> ListarFornecedor()
        {
            try
            {
                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedor");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponseList>();
                        return result ?? new FornecedorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarFornecedor: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarFornecedor));
                throw new Exception("Falha desconhecida ao listar fornecedores.");
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

        public async Task<FornecedorResponseList> ListarFornecedorPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedorPorId/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponseList>();
                        return result ?? new FornecedorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarFornecedorPorId: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarFornecedorPorId));
                throw new Exception("Falha desconhecida ao consultar fornecedor por Id.");
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

        public async Task<FornecedorResponseList> ListarFornecedorPorNomeFornecedor(string nomeOuCnpj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeOuCnpj))
                    throw new ArgumentException("Nome ou CNPJ é obrigatório.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Fornecedor/ListarFornecedorPorNomeFornecedor/{Uri.EscapeDataString(nomeOuCnpj)}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponseList>();
                        return result ?? new FornecedorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarFornecedorPorNome: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarFornecedorPorNomeFornecedor));
                throw new Exception("Falha desconhecida ao consultar fornecedor por nome/CNPJ.");
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

        public async Task<FornecedorResponse> AlterarFornecedor(FornecedorDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                
                if (string.IsNullOrWhiteSpace(dto.Id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(dto.Id, out Guid guidId))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    throw new ArgumentException($"Dados inválidos: {string.Join(", ", errosValidacao)}");
                }

                _client = Services.Http.HttpClientManager.GetClient();

                AlterarFornecedorRequest request = new AlterarFornecedorRequest()
                {
                    Id = guidId,
                    InscricaoEstadual = dto.InscricaoEstadual?.Trim() ?? string.Empty,
                    NomeFantasia = dto.NomeFantasia.Trim(),
                    Uf = dto.Uf.Trim().ToUpper(),
                    Numero = dto.Numero?.Trim() ?? string.Empty,
                    Complemento = dto.Complemento?.Trim() ?? string.Empty,
                    Bairro = dto.Bairro.Trim(),
                    Cidade = dto.Cidade.Trim(),
                    CepFornecedor = dto.CepFornecedor,
                    StatusAtivo = dto.StatusAtivo,
                    Cnpj = dto.Cnpj.Trim(),
                    Rua = dto.Rua.Trim()
                };

                var response = await _client.PutAsJson($"{BasePath}/Fornecedor/AlterarFornecedor", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponse>();
                        return result ?? new FornecedorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AlterarFornecedor: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AlterarFornecedor));
                throw new Exception("Falha desconhecida ao alterar fornecedor.");
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

        public async Task<FornecedorResponse> RemoverFornecedor(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.DeleteAsync($"{BasePath}/Fornecedor/RemoverFornecedor/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<FornecedorResponse>();
                        return result ?? new FornecedorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de RemoverFornecedor: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(RemoverFornecedor));
                throw new Exception("Falha desconhecida ao remover fornecedor.");
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
                HttpStatusCode.NotFound => "Fornecedor não encontrado",
                HttpStatusCode.Conflict => "Conflito - fornecedor já existe",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.ServiceUnavailable => "Serviço temporariamente indisponível",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            throw new Exception($"{methodName} falhou: {message}. Detalhes: {content}");
        }
    }
}
