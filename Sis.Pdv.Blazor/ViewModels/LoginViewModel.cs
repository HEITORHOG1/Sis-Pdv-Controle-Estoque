using Sis.Pdv.Blazor.Models.Auth;
using Sis.Pdv.Blazor.Services;
using Sis.Pdv.Blazor.ViewModels.Base;

namespace Sis.Pdv.Blazor.ViewModels;

/// <summary>
/// ViewModel da tela de login.
/// </summary>
public sealed class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginViewModel> _logger;

    private string _login = string.Empty;
    private string _senha = string.Empty;
    private bool _isLoading;
    private string? _mensagemErro;
    private bool _loginRealizado;
    private UserInfo? _usuario;

    public LoginViewModel(
        IAuthService authService,
        ILogger<LoginViewModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public string Senha
    {
        get => _senha;
        set => SetProperty(ref _senha, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public string? MensagemErro
    {
        get => _mensagemErro;
        private set => SetProperty(ref _mensagemErro, value);
    }

    public bool LoginRealizado
    {
        get => _loginRealizado;
        private set => SetProperty(ref _loginRealizado, value);
    }

    public UserInfo? Usuario
    {
        get => _usuario;
        private set => SetProperty(ref _usuario, value);
    }

    public bool PodeLogar => !IsLoading
        && !string.IsNullOrWhiteSpace(Login)
        && !string.IsNullOrWhiteSpace(Senha);

    public async Task RealizarLoginAsync(CancellationToken cancellationToken = default)
    {
        if (!PodeLogar) return;

        try
        {
            IsLoading = true;
            MensagemErro = null;

            var request = new LoginRequest
            {
                Login = Login,
                Password = Senha
            };

            var result = await _authService.LoginAsync(request, cancellationToken);

            if (!result.Success)
            {
                MensagemErro = result.Message ?? "Credenciais inválidas.";
                return;
            }

            Usuario = result.User;
            LoginRealizado = true;

            _logger.LogInformation("Login realizado: {Nome}", Usuario?.Nome);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao realizar login");
            MensagemErro = "Erro de conexão com o servidor. Verifique se a API está online.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado no login");
            MensagemErro = "Erro inesperado. Tente novamente.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
