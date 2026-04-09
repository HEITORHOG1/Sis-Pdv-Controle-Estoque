using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Configuration;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Repositories;
using Sis.Pdv.Blazor.Services;
using Sis.Pdv.Blazor.ViewModels;

namespace Sis.Pdv.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    private const string PdvSettingsSection = "PdvSettings";
    private const string ApiSettingsSection = "ApiSettings";

    /// <summary>
    /// Registra configurações do PDV usando padrão IOptions.
    /// </summary>
    public static IServiceCollection AddPdvConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PdvSettings>(
            configuration.GetSection(PdvSettingsSection));

        services.Configure<ApiSettings>(
            configuration.GetSection(ApiSettingsSection));

        return services;
    }

    /// <summary>
    /// Configura HttpClient nomeado para chamadas de API.
    /// </summary>
    public static IServiceCollection AddPdvHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var apiSettings = configuration
            .GetSection(ApiSettingsSection)
            .Get<ApiSettings>();

        var baseUrl = apiSettings?.BaseUrl
            ?? throw new InvalidOperationException("API BaseUrl não configurada.");

        services.AddHttpClient(ApiSettings.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    /// <summary>
    /// Registra o DbContext e acesso a dados (MySQL + EF Core).
    /// </summary>
    public static IServiceCollection AddPdvData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PdvDatabase");
        
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ConnectionString 'PdvDatabase' não encontrada.");

        services.AddDbContext<PdvDbContext>(options =>
        {
            options.UseMySql(
                connectionString, 
                ServerVersion.AutoDetect(connectionString));
        });

        return services;
    }

    /// <summary>
    /// Registra os repositórios de acesso ao banco local do PDV.
    /// </summary>
    public static IServiceCollection AddPdvRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IVendaRepository, VendaRepository>();
        services.AddScoped<IVendaRascunhoRepository, VendaRascunhoRepository>();

        return services;
    }

    /// <summary>
    /// Registra os serviços de acesso à API (Scoped — um por circuito Blazor).
    /// </summary>
    public static IServiceCollection AddPdvServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IKeyboardService, KeyboardService>();
        services.AddScoped<IVendaApiService, VendaApiService>();
        services.AddScoped<IItemVendaApiService, ItemVendaApiService>();

        return services;
    }

    /// <summary>
    /// Configura mensageria para sincronização offline (MassTransit + RabbitMQ).
    /// </summary>
    public static IServiceCollection AddPdvMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            // Sincronização down: Recebe atualizações do ERP
            x.AddConsumer<Sis.Pdv.Blazor.Messaging.Consumers.ProdutoAlteradoConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                // Configura host do container RabbitMQ
                var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
                var rabbitUser = configuration["RabbitMQ:Username"] ?? "user";
                var rabbitPass = configuration["RabbitMQ:Password"] ?? "password";

                cfg.Host(rabbitHost, "/", h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPass);
                });

                // Cria exchange e fila automaticamente para o consumer
                cfg.ConfigureEndpoints(context);
            });
        });

        // Worker de Upload (Sincronização Up)
        services.AddHostedService<SalesUploaderWorker>();

        return services;
    }

    /// <summary>
    /// Registra os ViewModels do MVVM.
    /// </summary>
    public static IServiceCollection AddPdvViewModels(
        this IServiceCollection services)
    {
        services.AddScoped<LoginViewModel>();
        services.AddScoped<PdvViewModel>();

        return services;
    }
}
