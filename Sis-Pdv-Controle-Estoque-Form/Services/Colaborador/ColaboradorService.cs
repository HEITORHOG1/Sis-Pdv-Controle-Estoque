using Commands.Colaborador.AdicionarColaborador;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;
using Sis_Pdv_Controle_Estoque_Form.Utils;
using System.Net;
using System.Text.Json;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Colaborador
{
    public class ColaboradorService
    {
        private HttpClient _client;
        public string BasePath = BaseAppConfig.ReadSetting("Base");
        
        public ColaboradorService()
        {
            _client = Services.Http.HttpClientManager.GetClient();
        }

        public async Task<ColaboradorResponse> AdicionarColaborador(ColaboradorDto dto)
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

                AdicionarColaboradorRequest request = new AdicionarColaboradorRequest()
                {
                    nomeColaborador = dto.nomeColaborador.Trim(),
                    cargoColaborador = dto.cargoColaborador.Trim(),
                    cpfColaborador = dto.cpfColaborador.Replace(".", "").Replace("-", ""),
                    emailCorporativo = dto.emailCorporativo.Trim(),
                    emailPessoalColaborador = dto.emailPessoalColaborador.Trim(),
                    telefoneColaborador = dto.telefoneColaborador.Trim(),
                    DepartamentoId = Guid.Parse(dto.departamentoId),
                    Usuario = new Model.Usuario
                    {
                        Senha = dto.senha.Trim(),
                        Login = dto.login.Trim(),
                        StatusAtivo = dto.statusAtivo
                    },
                };

                System.Diagnostics.Debug.WriteLine($"Tentando adicionar colaborador: {request.nomeColaborador}");

                var response = await _client.PostAsJson($"{BasePath}/Colaborador/AdicionarColaborador", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponse>();
                        return result ?? new ColaboradorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AdicionarColaborador: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AdicionarColaborador));
                throw new Exception("Falha desconhecida ao adicionar colaborador.");
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

        public async Task<ColaboradorResponseList> ListarColaborador()
        {
            try
            {
                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaborador");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponseList>();
                        return result ?? new ColaboradorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarColaborador: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarColaborador));
                throw new Exception("Falha desconhecida ao listar colaboradores.");
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

        public async Task<ColaboradorResponseList> ListarColaboradorPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaboradorPorId/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponseList>();
                        return result ?? new ColaboradorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarColaboradorPorId: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarColaboradorPorId));
                throw new Exception("Falha desconhecida ao consultar colaborador por Id.");
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

        public async Task<ColaboradorResponseList> ListarColaboradorPorNomeColaborador(string nomeColaborador)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeColaborador))
                    throw new ArgumentException("Nome do colaborador é obrigatório.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Colaborador/ListarColaboradorPorNomeColaborador/{Uri.EscapeDataString(nomeColaborador)}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponseList>();
                        return result ?? new ColaboradorResponseList { success = false, notifications = new List<object> { "Resposta vazia da API" }, data = new List<Data>() };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ListarColaboradorPorNome: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ListarColaboradorPorNomeColaborador));
                throw new Exception("Falha desconhecida ao consultar colaborador por nome.");
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

        public async Task<ColaboradorResponse> AlterarColaborador(ColaboradorDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                
                if (string.IsNullOrWhiteSpace(dto.id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(dto.id, out Guid guidId))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                // Valida o DTO
                var errosValidacao = dto.Validar();
                if (errosValidacao.Any())
                {
                    throw new ArgumentException($"Dados inválidos: {string.Join(", ", errosValidacao)}");
                }

                _client = Services.Http.HttpClientManager.GetClient();

                AdicionarColaboradorRequest request = new AdicionarColaboradorRequest()
                {
                    Id = guidId,
                    nomeColaborador = dto.nomeColaborador.Trim(),
                    cargoColaborador = dto.cargoColaborador.Trim(),
                    cpfColaborador = dto.cpfColaborador.Replace(".", "").Replace("-", ""),
                    emailCorporativo = dto.emailCorporativo.Trim(),
                    emailPessoalColaborador = dto.emailPessoalColaborador.Trim(),
                    telefoneColaborador = dto.telefoneColaborador.Trim(),
                    DepartamentoId = Guid.Parse(dto.departamentoId),
                    Usuario = new Model.Usuario
                    {
                        Id = string.IsNullOrWhiteSpace(dto.idlogin) ? Guid.NewGuid() : Guid.Parse(dto.idlogin),
                        Senha = dto.senha.Trim(),
                        Login = dto.login.Trim(),
                        StatusAtivo = dto.statusAtivo
                    },
                };

                var response = await _client.PutAsJson($"{BasePath}/Colaborador/AlterarColaborador", request);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponse>();
                        return result ?? new ColaboradorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de AlterarColaborador: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(AlterarColaborador));
                throw new Exception("Falha desconhecida ao alterar colaborador.");
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

        public async Task<ColaboradorResponse> RemoverColaborador(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Id é obrigatório.");

                if (!Guid.TryParse(id, out _))
                    throw new ArgumentException("Id deve ser um GUID válido.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.DeleteAsync($"{BasePath}/Colaborador/RemoverColaborador/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponse>();
                        return result ?? new ColaboradorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de RemoverColaborador: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(RemoverColaborador));
                throw new Exception("Falha desconhecida ao remover colaborador.");
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

        public async Task<ColaboradorResponse> ValidarLogin(string login, string senha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login))
                    throw new ArgumentException("Login é obrigatório.");

                if (string.IsNullOrWhiteSpace(senha))
                    throw new ArgumentException("Senha é obrigatória.");

                _client = Services.Http.HttpClientManager.GetClient();

                var response = await _client.GetAsync($"{BasePath}/Colaborador/ValidarLogin/{Uri.EscapeDataString(login)}/{Uri.EscapeDataString(senha)}");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.ReadContentAs<ColaboradorResponse>();
                        return result ?? new ColaboradorResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta de ValidarLogin: {ex.Message}");
                        throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
                    }
                }

                await ThrowDetailedException(response, nameof(ValidarLogin));
                throw new Exception("Falha desconhecida ao validar login.");
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
                HttpStatusCode.NotFound => "Colaborador não encontrado",
                HttpStatusCode.Conflict => "Conflito - colaborador já existe",
                HttpStatusCode.InternalServerError => "Erro interno do servidor",
                HttpStatusCode.ServiceUnavailable => "Serviço temporariamente indisponível",
                _ => $"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };

            throw new Exception($"{methodName} falhou: {message}. Detalhes: {content}");
        }
    }
}
