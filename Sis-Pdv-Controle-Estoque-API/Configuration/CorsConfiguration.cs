using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Sis_Pdv_Controle_Estoque_API.Configuration
{
    /// <summary>
    /// CORS configuration for different environments
    /// </summary>
    public static class CorsConfiguration
    {
        public const string DevelopmentPolicy = "DevelopmentPolicy";
        public const string ProductionPolicy = "ProductionPolicy";

        public static void ConfigureCors(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                if (environment.IsDevelopment() || environment.IsEnvironment("Docker"))
                {
                    // Development policy - more permissive for testing
                    options.AddPolicy(DevelopmentPolicy, policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                }
                else
                {
                    // Production policy - restrictive and secure
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                                       ?? new[] { "https://localhost:3000" };

                    options.AddPolicy(ProductionPolicy, policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                              .WithHeaders("Content-Type", "Authorization", "X-Requested-With", "Accept", "Origin")
                              .AllowCredentials()
                              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                    });
                }

                // API-specific policy for external integrations
                var apiAllowedOrigins = configuration.GetSection("Cors:ApiAllowedOrigins").Get<string[]>() 
                                      ?? Array.Empty<string>();

                if (apiAllowedOrigins.Any())
                {
                    options.AddPolicy("ApiPolicy", policy =>
                    {
                        policy.WithOrigins(apiAllowedOrigins)
                              .WithMethods("GET", "POST", "PUT", "DELETE")
                              .WithHeaders("Content-Type", "Authorization", "X-API-Key")
                              .SetPreflightMaxAge(TimeSpan.FromHours(1));
                    });
                }
            });
        }

        public static void UseCorsPolicy(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment() || environment.IsEnvironment("Docker"))
            {
                app.UseCors(DevelopmentPolicy);
            }
            else
            {
                app.UseCors(ProductionPolicy);
            }
        }
    }
}