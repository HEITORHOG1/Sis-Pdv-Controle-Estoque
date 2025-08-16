using Microsoft.OpenApi.Models;
using System.Reflection;
using Asp.Versioning.ApiExplorer;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Configuration for Swagger/OpenAPI documentation
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Configure Swagger services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="environment">Web host environment</param>
    public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddSwaggerGen(options =>
        {
            // Configure API information
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PDV Control System API",
                Version = "v1.0",
                Description = @"
# PDV Control System API

A comprehensive Point of Sale and Inventory Control System API built with .NET 8.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control
- **Product Management**: Complete CRUD operations for products with barcode support
- **Inventory Control**: Real-time stock tracking with movement history
- **Customer Management**: Customer registration and management
- **Sales Processing**: Complete sales workflow with payment processing
- **Reporting**: Comprehensive reports in PDF and Excel formats
- **User Management**: Multi-user support with roles and permissions
- **Backup & Recovery**: Automated backup system with restore capabilities

## Authentication

This API uses JWT Bearer tokens for authentication. To access protected endpoints:

1. Obtain a token by calling the `/api/v1/auth/login` endpoint
2. Include the token in the Authorization header: `Bearer {your-token}`

## Rate Limiting

API calls are rate-limited to prevent abuse. Current limits:
- 1000 requests per hour per IP address
- 100 requests per minute per authenticated user

## Support

For technical support, please contact the development team.
",
                Contact = new OpenApiContact
                {
                    Name = "PDV System Development Team",
                    Email = "dev@pdvsystem.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                },
                TermsOfService = new Uri("https://pdvsystem.com/terms")
            });

            // Configure JWT authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = @"
JWT Authorization header using the Bearer scheme.

Enter your token in the text input below.

Example: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`

**Note:** Do not include 'Bearer ' prefix - it will be added automatically."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Include XML documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }

            // Include domain XML documentation if available
            var domainXmlFile = "Sis-Pdv-Controle-Estoque-Domain.xml";
            var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
            if (File.Exists(domainXmlPath))
            {
                options.IncludeXmlComments(domainXmlPath);
            }

            // Configure schema generation
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
            
            // Add operation filters for better documentation
            options.OperationFilter<SwaggerOperationFilter>();
            options.SchemaFilter<SwaggerSchemaFilter>();
            
            // Configure examples and annotations (if Swashbuckle.AspNetCore.Annotations is available)
            // options.EnableAnnotations();
            
            // Group endpoints by tags
            options.TagActionsBy(api => new[] { GetControllerName(api) });
            options.DocInclusionPredicate((name, api) => true);
            
            // Configure servers
            if (environment.IsDevelopment())
            {
                options.AddServer(new OpenApiServer
                {
                    Url = "https://localhost:7001",
                    Description = "Development Server (HTTPS)"
                });
                options.AddServer(new OpenApiServer
                {
                    Url = "http://localhost:5001",
                    Description = "Development Server (HTTP)"
                });
            }
        });
    }

    /// <summary>
    /// Configure Swagger UI
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="environment">Web host environment</param>
    /// <param name="provider">API version description provider</param>
    public static void UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment environment, IApiVersionDescriptionProvider? provider = null)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "api-docs/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            // Configure Swagger UI
            options.SwaggerEndpoint("/api-docs/v1/swagger.json", "PDV Control System API v1.0");
            options.RoutePrefix = "api-docs";
            
            // UI Configuration
            options.DocumentTitle = "PDV Control System API Documentation";
            options.DefaultModelsExpandDepth(2);
            options.DefaultModelExpandDepth(2);
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
            options.EnableValidator();
            
            // Custom CSS for better appearance
            options.InjectStylesheet("/swagger-ui/custom.css");
            
            // Configure OAuth2 if needed
            options.OAuthClientId("pdv-swagger-ui");
            options.OAuthAppName("PDV Control System API");
            options.OAuthUsePkce();
            
            // Try it out configuration
            options.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
            options.ConfigObject.AdditionalItems["filter"] = true;
            options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
            {
                ["activated"] = true,
                ["theme"] = "agate"
            };
        });

        // Serve custom CSS
        app.UseStaticFiles();
    }

    private static string GetControllerName(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription api)
    {
        var controllerName = api.ActionDescriptor.RouteValues["controller"] ?? "Unknown";
        
        // Convert controller names to friendly display names
        return controllerName switch
        {
            "Auth" => "Authentication",
            "UserManagement" => "User Management",
            "Produto" => "Products",
            "Cliente" => "Customers",
            "Fornecedor" => "Suppliers",
            "Pedido" => "Orders",
            "Inventory" => "Inventory Management",
            "Reports" => "Reports & Analytics",
            "Backup" => "Backup & Recovery",
            "Health" => "System Health",
            "Cache" => "Cache Management",
            _ => controllerName
        };
    }
}