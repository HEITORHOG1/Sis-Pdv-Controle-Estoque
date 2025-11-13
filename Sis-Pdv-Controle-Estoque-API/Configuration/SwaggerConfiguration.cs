using Microsoft.OpenApi.Models;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Bearer token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }

            var domainXmlFile = "Sis-Pdv-Controle-Estoque-Domain.xml";
            var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
            if (File.Exists(domainXmlPath))
            {
                options.IncludeXmlComments(domainXmlPath);
            }

            options.CustomSchemaIds(type =>
            {
                static string Clean(string name) => name.Replace('+', '.').Replace('`', '_').Replace('.', '_');
                if (type.IsGenericType)
                {
                    var genericTypeName = type.Name.Split('`')[0];
                    var genericArgs = type.GetGenericArguments().Select(t => Clean(t.FullName ?? t.Name));
                    var ns = Clean(type.Namespace ?? "Global");
                    return $"{ns}_{genericTypeName}_{string.Join("_", genericArgs)}";
                }
                return Clean(type.FullName ?? type.Name);
            });
            options.SupportNonNullableReferenceTypes();
            
            // Temporarily disable custom filters to rule out runtime issues causing blank UI
            // options.OperationFilter<SwaggerOperationFilter>();
            // options.SchemaFilter<SwaggerSchemaFilter>();
            
            // Default grouping
            // options.TagActionsBy(api => new[] { GetControllerName(api) });
            // options.DocInclusionPredicate((name, api) => true);
            
            if (environment.IsDevelopment())
            {
                options.AddServer(new OpenApiServer { Url = "http://localhost:7003", Description = "Development Server (HTTP)" });
            }
        });
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment environment, IApiVersionDescriptionProvider? provider = null)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var routePrefix = configuration["Swagger:RoutePrefix"] ?? "api-docs";
        
        app.UseSwagger(options =>
        {
            options.RouteTemplate = $"{routePrefix}/{{documentName}}/swagger.json";
        });

        provider ??= app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

        app.UseSwaggerUI(options =>
        {
            var added = false;
            if (provider != null && provider.ApiVersionDescriptions?.Count > 0)
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var name = description.GroupName;
                    options.SwaggerEndpoint($"/{routePrefix}/{name}/swagger.json", $"PDV API {name.ToUpper()}");
                    added = true;
                }
            }
            if (!added)
            {
                options.SwaggerEndpoint($"/{routePrefix}/v1/swagger.json", "PDV API v1");
            }

            options.RoutePrefix = routePrefix;
            options.DocumentTitle = "PDV Control System API";
        });

        app.UseStaticFiles();
    }

    private static string GetControllerName(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription api)
    {
        var controllerName = api.ActionDescriptor.RouteValues["controller"] ?? "Unknown";
        return controllerName;
    }
}