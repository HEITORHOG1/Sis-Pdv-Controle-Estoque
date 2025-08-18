using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Adds a Swagger document per discovered API version.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // Register a Swagger document for each discovered API version
        var descriptions = _provider.ApiVersionDescriptions;
        if (descriptions is null || descriptions.Count == 0)
        {
            // Fallback to v1 if no versions discovered
            options.SwaggerDoc("v1", CreateInfoForApiVersion("v1", new OpenApiInfo
            {
                Title = "PDV Control System API",
                Version = "v1",
                Description = "PDV Control System API documentation"
            }));
            return;
        }

        foreach (var description in descriptions)
        {
            var groupName = description.GroupName; // e.g., v1
            var info = new OpenApiInfo
            {
                Title = "PDV Control System API",
                Version = groupName,
                Description = "PDV Control System API documentation",
                Contact = new OpenApiContact
                {
                    Name = "PDV System Development Team",
                    Email = "dev@pdvsystem.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " - This API version has been deprecated.";
            }

            options.SwaggerDoc(groupName, CreateInfoForApiVersion(groupName, info));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(string groupName, OpenApiInfo info)
    {
        // Ensure Version field matches group
        info.Version = groupName;
        return info;
    }
}
