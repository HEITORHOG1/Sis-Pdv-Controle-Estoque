using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repositories.Base;

namespace Sis_Pdv_Controle_Estoque.Tests.Infrastructure;

public class WebApplicationTestFixture : WebApplicationFactory<TestStartup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<PdvContext>));
            services.RemoveAll(typeof(PdvContext));

            // Add in-memory database for testing
            services.AddDbContext<PdvContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            });

            // Build service provider and ensure database is created
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
            context.Database.EnsureCreated();
        });

        builder.UseEnvironment("Testing");
    }
}