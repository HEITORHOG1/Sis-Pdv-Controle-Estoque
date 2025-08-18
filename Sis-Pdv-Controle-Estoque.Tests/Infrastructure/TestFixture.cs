using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MediatR;
using Interfaces;
using Repositories.Base;
using Repositories;

namespace Sis_Pdv_Controle_Estoque.Tests.Infrastructure;

public class TestFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }

    public TestFixture()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", "Server=localhost;Database=PDVTest;Uid=test;Pwd=test;"},
                {"Authentication:JwtSecret", "test-secret-key-for-testing-purposes-only"},
                {"Authentication:TokenExpirationMinutes", "60"},
                {"Authentication:RefreshTokenExpirationDays", "7"}
            })
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Commands.Cliente.AdicionarCliente.AdicionarClienteRequest).Assembly));

        // Logging
        services.AddLogging(builder => builder.AddConsole());

        // Test services configuration
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}