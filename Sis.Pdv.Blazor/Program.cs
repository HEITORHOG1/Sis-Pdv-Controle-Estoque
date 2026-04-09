using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Components;
using Sis.Pdv.Blazor.Data;
using Sis.Pdv.Blazor.Extensions;
using Radzen;

namespace Sis.Pdv.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddRadzenComponents();

        builder.Services
            .AddPdvConfiguration(builder.Configuration)
            .AddPdvData(builder.Configuration)
            .AddPdvHttpClient(builder.Configuration)
            .AddPdvMessaging(builder.Configuration)
            .AddPdvRepositories()
            .AddPdvServices()
            .AddPdvViewModels();

        var app = builder.Build();

        // Auto-migrate local database
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PdvDbContext>();
            db.Database.Migrate();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
