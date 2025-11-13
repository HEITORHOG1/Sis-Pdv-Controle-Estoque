using Commands.Departamento.AdicionarDepartamento;
using Commands.Departamento.AlterarDepartamento;
using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Departamento
{
    public class DepartamentoService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        
        public DepartamentoService()
        {
            _client = Services.Http.HttpClientManager.GetClient();
        }

        public async Task<DepartamentoResponse> AdicionarDepartamento(DepartamentoDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                
                if (string.IsNullOrWhiteSpace(dto.NomeDepartamento))
                    throw new ArgumentException("Nome do departamento é obrigatório.");

                _client = Services.Http.HttpClientManager.GetClient();

                AdicionarDepartamentoRequest request = new AdicionarDepartamentoRequest()
                {
                    NomeDepartamento = dto.NomeDepartamento.Trim()
                };

                System.Diagnostics.Debug.WriteLine($"Tentando adicionar departamento: {request.NomeDepartamento}");

                var response = await _client.PostAsJson($"{BasePath}/Departamento/AdicionarDepartamento", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponse>();
                        return result ?? new DepartamentoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AdicionarDepartamento: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AdicionarDepartamento));
                throw new Exception("Falha desconhecida ao adicionar departamento.");
            }
            catch (ApplicationException appEx)
            {
                // Erros já tratados pelas extensões HTTP
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

        public async Task<DepartamentoResponseList> ListarDepartamento()
        {
            try
            {
                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamento");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponseList>();
                        return result ?? new DepartamentoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarDepartamento: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarDepartamento));
                throw new Exception("Falha desconhecida ao listar departamentos.");
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

        public async Task<DepartamentoResponseList> ListarDepartamentoPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamentoPorId/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponseList>();
                        return result ?? new DepartamentoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarDepartamentoPorId: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarDepartamentoPorId));
                throw new Exception("Falha desconhecida ao consultar departamento por Id.");
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

        public async Task<DepartamentoResponseList> ListarDepartamentoPorNomeDepartamento(string nomeDepartamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeDepartamento))
                    throw new ArgumentException("Nome do departamento é obrigatório.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Departamento/ListarDepartamentoPorNomeDepartamento/{Uri.EscapeDataString(nomeDepartamento)}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponseList>();
                        return result ?? new DepartamentoResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarDepartamentoPorNome: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarDepartamentoPorNomeDepartamento));
                throw new Exception("Falha desconhecida ao consultar departamento por nome.");
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

        public async Task<DepartamentoResponse> AlterarDepartamento(DepartamentoDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                
                if (string.IsNullOrWhiteSpace(dto.Id))
                    throw new ArgumentException("Id é obrigatório.");
                
                if (string.IsNullOrWhiteSpace(dto.NomeDepartamento))
                    throw new ArgumentException("Nome do departamento é obrigatório.");

                if (!Guid.TryParse(dto.Id, out Guid guidId))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                AlterarDepartamentoRequest request = new AlterarDepartamentoRequest()
                {
                    Id = guidId,
                    NomeDepartamento = dto.NomeDepartamento.Trim()
                };

                var response = await _client.PutAsJson($"{BasePath}/Departamento/AlterarDepartamento", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponse>();
                        return result ?? new DepartamentoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AlterarDepartamento: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AlterarDepartamento));
                throw new Exception("Falha desconhecida ao alterar departamento.");
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

        public async Task<DepartamentoResponse> RemoverDepartamento(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.DeleteAsync($"{BasePath}/Departamento/RemoverDepartamento/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<DepartamentoResponse>();
                        return result ?? new DepartamentoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de RemoverDepartamento: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(RemoverDepartamento));
                throw new Exception("Falha desconhecida ao remover departamento.");
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
                HttpStatusCode.NotFound => "Departamento não encontrado",
                HttpStatusCode.Conflict => "Conflito - departamento já existe",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.ServiceUnavailable => "Serviço temporariamente indisponível",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            throw new Exception($"{methodName} falhou: {message}. Detalhes: {content}");
        }
    }
}
