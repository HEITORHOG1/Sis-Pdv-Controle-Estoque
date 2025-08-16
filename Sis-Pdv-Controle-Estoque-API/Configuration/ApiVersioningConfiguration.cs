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
            
            // This will be handled by the API controllers with [ApiVersion] attributes
            
            // Version format
            options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            // Format version as 'v{major}.{minor}'
            options.GroupNameFormat = "'v'VVV";
            
            // Automatically substitute version in controller names
            options.SubstituteApiVersionInUrl = true;
            
            // Add version parameter to all endpoints
            options.AddApiVersionParametersWhenVersionNeutral = true;
        });
    }
}