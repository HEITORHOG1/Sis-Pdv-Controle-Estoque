using Asp.Versioning;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Configuration for API versioning
/// </summary>
public static class ApiVersioningConfiguration
{
    /// <summary>
    /// Configure API versioning services
    /// </summary>
    /// <param name="services">Service collection</param>
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // Default version
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            
            // Version reading strategies
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),           // /api/v1/products
                new QueryStringApiVersionReader("version"), // ?version=1.0
                new HeaderApiVersionReader("X-Version"),    // X-Version: 1.0
                new MediaTypeApiVersionReader("ver")        // Accept: application/json;ver=1.0
            );
            
            // Select current implementation by default when unspecified
            options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            // Use simple group names 'v1', 'v2', ... to align with Swagger doc names
            options.GroupNameFormat = "'v'V"; // e.g., v1
            
            // Automatically substitute version in controller names
            options.SubstituteApiVersionInUrl = true;
            
            // Add version parameter to all endpoints when version-neutral
            options.AddApiVersionParametersWhenVersionNeutral = true;
        });
    }
}