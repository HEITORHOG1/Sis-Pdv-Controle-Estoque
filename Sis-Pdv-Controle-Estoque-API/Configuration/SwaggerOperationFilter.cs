using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Custom operation filter for Swagger documentation enhancement
/// </summary>
public class SwaggerOperationFilter : IOperationFilter
{
    /// <summary>
    /// Apply operation-specific documentation enhancements
    /// </summary>
    /// <param name="operation">OpenAPI operation</param>
    /// <param name="context">Operation filter context</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common response codes
        AddCommonResponses(operation, context);
        
        // Add operation tags based on controller
        AddOperationTags(operation, context);
        
        // Enhance parameter descriptions
        EnhanceParameterDescriptions(operation, context);
        
        // Add examples for request/response
        AddExamples(operation, context);
        
        // Add deprecation warnings if applicable
        CheckDeprecation(operation, context);
    }

    private static void AddCommonResponses(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add 401 Unauthorized for protected endpoints
        if (context.MethodInfo.GetCustomAttributes<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any() ||
            context.MethodInfo.DeclaringType?.GetCustomAttributes<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any() == true)
        {
            if (!operation.Responses.ContainsKey("401"))
            {
                var schema = context.SchemaGenerator.GenerateSchema(typeof(Sis_Pdv_Controle_Estoque_API.Models.ApiResponse), context.SchemaRepository);
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized - Invalid or missing authentication token",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = schema
                        }
                    }
                });
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                var schema = context.SchemaGenerator.GenerateSchema(typeof(Sis_Pdv_Controle_Estoque_API.Models.ApiResponse), context.SchemaRepository);
                operation.Responses.Add("403", new OpenApiResponse
                {
                    Description = "Forbidden - Insufficient permissions to access this resource",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = schema
                        }
                    }
                });
            }
        }

        // Add 400 Bad Request for POST/PUT operations
        if (context.ApiDescription.HttpMethod?.ToUpper() is "POST" or "PUT" or "PATCH")
        {
            if (!operation.Responses.ContainsKey("400"))
            {
                var schema = context.SchemaGenerator.GenerateSchema(typeof(Sis_Pdv_Controle_Estoque_API.Models.ApiResponse), context.SchemaRepository);
                operation.Responses.Add("400", new OpenApiResponse
                {
                    Description = "Bad Request - Invalid input data or validation errors",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = schema
                        }
                    }
                });
            }
        }

        // Add 500 Internal Server Error
        if (!operation.Responses.ContainsKey("500"))
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(Sis_Pdv_Controle_Estoque_API.Models.ApiResponse), context.SchemaRepository);
            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Internal Server Error - An unexpected error occurred",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema
                    }
                }
            });
        }
    }

    private static void AddOperationTags(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerName = context.MethodInfo.DeclaringType?.Name.Replace("Controller", "") ?? "Unknown";
        
        // Add additional tags based on operation type
        var httpMethod = context.ApiDescription.HttpMethod?.ToUpper();
        var operationName = context.MethodInfo.Name;

        var additionalTags = new List<string>();

        // Add CRUD operation tags
        if (operationName.StartsWith("Get") || operationName.StartsWith("Listar"))
            additionalTags.Add("Read Operations");
        else if (operationName.StartsWith("Post") || operationName.StartsWith("Add") || operationName.StartsWith("Create"))
            additionalTags.Add("Create Operations");
        else if (operationName.StartsWith("Put") || operationName.StartsWith("Update") || operationName.StartsWith("Alterar"))
            additionalTags.Add("Update Operations");
        else if (operationName.StartsWith("Delete") || operationName.StartsWith("Remove") || operationName.StartsWith("Remover"))
            additionalTags.Add("Delete Operations");

        // Add security-related tags
        if (controllerName.Contains("Auth") || controllerName.Contains("User"))
            additionalTags.Add("Security");

        // Add business operation tags
        if (controllerName.Contains("Inventory") || controllerName.Contains("Stock"))
            additionalTags.Add("Inventory Management");
        else if (controllerName.Contains("Report"))
            additionalTags.Add("Reporting");
        else if (controllerName.Contains("Backup"))
            additionalTags.Add("System Administration");

        foreach (var tag in additionalTags)
        {
            if (operation.Tags?.Any(t => t.Name == tag) != true)
            {
                operation.Tags ??= new List<OpenApiTag>();
                operation.Tags.Add(new OpenApiTag { Name = tag });
            }
        }
    }

    private static void EnhanceParameterDescriptions(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters ?? Enumerable.Empty<OpenApiParameter>())
        {
            // Enhance common parameter descriptions
            switch (parameter.Name.ToLower())
            {
                case "id":
                    parameter.Description ??= "Unique identifier for the resource";
                    parameter.Example = new Microsoft.OpenApi.Any.OpenApiString("123e4567-e89b-12d3-a456-426614174000");
                    break;
                case "page":
                    parameter.Description ??= "Page number for pagination (1-based)";
                    parameter.Example = new Microsoft.OpenApi.Any.OpenApiInteger(1);
                    break;
                case "pagesize":
                    parameter.Description ??= "Number of items per page (max 100)";
                    parameter.Example = new Microsoft.OpenApi.Any.OpenApiInteger(20);
                    break;
                case "search":
                    parameter.Description ??= "Search term to filter results";
                    parameter.Example = new Microsoft.OpenApi.Any.OpenApiString("search term");
                    break;
            }
        }
    }

    private static void AddExamples(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add request examples for POST/PUT operations
        if (context.ApiDescription.HttpMethod?.ToUpper() is "POST" or "PUT" or "PATCH")
        {
            var requestBody = operation.RequestBody;
            if (requestBody?.Content?.ContainsKey("application/json") == true)
            {
                var jsonContent = requestBody.Content["application/json"];
                if (jsonContent.Example == null && jsonContent.Examples?.Any() != true)
                {
                    // Add generic example based on operation
                    var operationName = context.MethodInfo.Name.ToLower();
                    if (operationName.Contains("product") || operationName.Contains("produto"))
                    {
                        jsonContent.Example = new Microsoft.OpenApi.Any.OpenApiObject
                        {
                            ["nome"] = new Microsoft.OpenApi.Any.OpenApiString("Product Name"),
                            ["descricao"] = new Microsoft.OpenApi.Any.OpenApiString("Product Description"),
                            ["preco"] = new Microsoft.OpenApi.Any.OpenApiDouble(29.99),
                            ["codigoBarras"] = new Microsoft.OpenApi.Any.OpenApiString("1234567890123")
                        };
                    }
                }
            }
        }
    }

    private static void CheckDeprecation(OpenApiOperation operation, OperationFilterContext context)
    {
        var obsoleteAttribute = context.MethodInfo.GetCustomAttribute<ObsoleteAttribute>();
        if (obsoleteAttribute != null)
        {
            operation.Deprecated = true;
            operation.Description = $"⚠️ **DEPRECATED**: {obsoleteAttribute.Message}\n\n{operation.Description}";
        }
    }
}