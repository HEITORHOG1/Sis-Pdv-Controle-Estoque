using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Configuration;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Data.Entities;
using Sis.Pdv.Blazor.Models.Auth;

namespace Sis.Pdv.Blazor.Services;

/// <summary>
/// Autenticação via API REST com Fallback Offline.
/// POST /v1/users/login -> Salva Hash -> Recupera Hash sem internet
/// </summary>
public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AuthService(
        IHttpClientFactory httpClientFactory,
        ILogger<AuthService> logger,
        IServiceProvider serviceProvider)
    {
        _httpClient = httpClientFactory.CreateClient(ApiSettings.HttpClientName);
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<AuthResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Tentativa de login: {Login}", request.Login);

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/v1/users/login",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthResult>(
                cancellationToken: cancellationToken);

            if (result?.Success == true && result.User != null)
            {
                await SincronizarUsuarioOfflineAsync(request.Login, request.Password, result.User, cancellationToken);
                return result;
            }

            return result ?? throw new InvalidOperationException("Resposta nula da API.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao acessar API. Tentando Login OFFLINE para {Login}", request.Login);
            return await TentarLoginOfflineAsync(request.Login, request.Password, cancellationToken);
        }
    }

    private static string GerarHash(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }

    private async Task SincronizarUsuarioOfflineAsync(string login, string senha, UserInfo userInfo, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PdvDbContext>();
            
            var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
            if (user == null)
            {
                user = new UsuarioLocalEntity { Login = login };
                db.Usuarios.Add(user);
            }
            
            user.Id = userInfo.Id;
            user.Nome = userInfo.Nome;
            user.PasswordHashOffline = GerarHash(senha);
            user.Roles = string.Join(",", userInfo.Roles);
            user.Permissions = string.Join(",", userInfo.Permissions);
            user.DataUltimoLogin = DateTime.UtcNow;
            
            await db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cache de usuário {Login} atualizado localmente.", login);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar cache de usuário offline.");
        }
    }

    private async Task<AuthResult> TentarLoginOfflineAsync(string login, string senha, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PdvDbContext>();
            
            var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
            
            if (user != null && user.PasswordHashOffline == GerarHash(senha))
            {
                _logger.LogInformation("Login OFFLINE efetuado com sucesso para {Login}", login);
                
                return new AuthResult
                {
                    Success = true,
                    Message = "Login efetuado localmente (Modo Offline).",
                    AccessToken = "OFFLINE_TOKEN", 
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Nome = user.Nome,
                        Roles = string.IsNullOrEmpty(user.Roles) ? [] : user.Roles.Split(',').ToList(),
                        Permissions = string.IsNullOrEmpty(user.Permissions) ? [] : user.Permissions.Split(',').ToList()
                    }
                };
            }
            
            return new AuthResult { Success = false, Message = "Credenciais inválidas (Offline) ou usuário não sincronizado." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no banco ao validar acesso offline.");
            return new AuthResult { Success = false, Message = "Inconsistência local ao validar operador." };
        }
    }
}
