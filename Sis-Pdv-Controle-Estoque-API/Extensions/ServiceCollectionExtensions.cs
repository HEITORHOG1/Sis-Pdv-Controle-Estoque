using Microsoft.AspNetCore.Mvc;
using Repositories.Base;
using Sis_Pdv_Controle_Estoque_API.Filters;
using Sis_Pdv_Controle_Estoque_API.Services.Metrics;
using Sis_Pdv_Controle_Estoque_API.Services.Validation;

namespace Sis_Pdv_Controle_Estoque_API.Extensions
{
    /// <summary>
    /// Extension methods for service registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds enhanced API services including validation, metrics, and error handling
        /// </summary>
        public static IServiceCollection AddEnhancedApiServices(this IServiceCollection services)
        {
            // Add validation services
            services.AddScoped<IModelValidationService, ModelValidationService>();

            // Add metrics services
            services.AddSingleton<IApiMetricsService, ApiMetricsService>();

            // Add global exception filter
            services.AddScoped<GlobalExceptionFilter>();

            // Configure MVC with global filters
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            return services;
        }

        /// <summary>
        /// Adds structured logging configuration
        /// </summary>
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                });

                // Add file logging if configured
                var logPath = configuration.GetValue<string>("Logging:FilePath");
                if (!string.IsNullOrEmpty(logPath))
                {
                    // Note: This would require a file logging provider like Serilog or NLog
                    // For now, we'll use console logging
                }

                // Configure log levels
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
                builder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
            });

            return services;
        }

        /// <summary>
        /// Adds API versioning and documentation
        /// </summary>
        public static IServiceCollection AddApiVersioningAndDocumentation(this IServiceCollection services)
        {
            // API Versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
                    new Asp.Versioning.UrlSegmentApiVersionReader(),
                    new Asp.Versioning.QueryStringApiVersionReader("version"),
                    new Asp.Versioning.HeaderApiVersionReader("X-Version")
                );
            });

            // TODO: Fix versioned API explorer dependency
            // services.AddVersionedApiExplorer(options =>
            // {
            //     options.GroupNameFormat = "'v'VVV";
            //     options.SubstituteApiVersionInUrl = true;
            // });

            return services;
        }

        /// <summary>
        /// Adds enhanced security headers and policies
        /// </summary>
        public static IServiceCollection AddSecurityPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Product management policies
                options.AddPolicy("ProductView", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("permission", "product.view"));

                options.AddPolicy("ProductManagement", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("permission", "product.manage"));

                // Inventory management policies
                options.AddPolicy("InventoryView", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("permission", "inventory.view"));

                options.AddPolicy("InventoryManagement", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("permission", "inventory.manage"));
            });

            return services;
        }

        /// <summary>
        /// Adds CORS policies for API access
        /// </summary>
        public static IServiceCollection AddApiCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ApiCorsPolicy", builder =>
                {
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
                    
                    if (allowedOrigins.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins);
                    }
                    else
                    {
                        builder.AllowAnyOrigin();
                    }

                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Correlation-ID", "X-Total-Count");
                });
            });

            return services;
        }

        /// <summary>
        /// Adds health checks for API monitoring
        /// </summary>
        public static IServiceCollection AddApiHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
                // TODO: Add database health check
                // .AddDbContext<PdvContext>("database");

            return services;
        }
    }
}