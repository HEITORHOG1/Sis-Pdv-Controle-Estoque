using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Custom schema filter for Swagger documentation enhancement
/// </summary>
public class SwaggerSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Apply schema-specific documentation enhancements
    /// </summary>
    /// <param name="schema">OpenAPI schema</param>
    /// <param name="context">Schema filter context</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Add examples for common types
        AddExamples(schema, context);
        
        // Enhance property descriptions
        EnhancePropertyDescriptions(schema, context);
        
        // Add validation information
        AddValidationInfo(schema, context);
        
        // Mark required properties
        MarkRequiredProperties(schema, context);
    }

    private static void AddExamples(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Guid) || context.Type == typeof(Guid?))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("123e4567-e89b-12d3-a456-426614174000");
            schema.Format = "uuid";
        }
        else if (context.Type == typeof(DateTime) || context.Type == typeof(DateTime?))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("2024-01-15T10:30:00Z");
            schema.Format = "date-time";
        }
        else if (context.Type == typeof(decimal) || context.Type == typeof(decimal?))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiDouble(29.99);
            schema.Format = "decimal";
        }
        else if (context.Type == typeof(string))
        {
            // Add examples based on property name patterns
            if (context.MemberInfo?.Name?.ToLower().Contains("email") == true)
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString("user@example.com");
                schema.Format = "email";
            }
            else if (context.MemberInfo?.Name?.ToLower().Contains("phone") == true ||
                     context.MemberInfo?.Name?.ToLower().Contains("telefone") == true)
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString("(11) 99999-9999");
                schema.Pattern = @"^\(\d{2}\)\s\d{4,5}-\d{4}$";
            }
            else if (context.MemberInfo?.Name?.ToLower().Contains("cpf") == true)
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString("123.456.789-00");
                schema.Pattern = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$";
            }
            else if (context.MemberInfo?.Name?.ToLower().Contains("cnpj") == true)
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString("12.345.678/0001-90");
                schema.Pattern = @"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$";
            }
            else if (context.MemberInfo?.Name?.ToLower().Contains("barcode") == true ||
                     context.MemberInfo?.Name?.ToLower().Contains("codigobarras") == true)
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString("1234567890123");
                schema.Pattern = @"^\d{8,14}$";
            }
        }
    }

    private static void EnhancePropertyDescriptions(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo != null)
        {
            var propertyName = context.MemberInfo.Name.ToLower();
            
            // Add descriptions for common property patterns
            if (string.IsNullOrEmpty(schema.Description))
            {
                schema.Description = propertyName switch
                {
                    "id" => "Unique identifier for the resource",
                    "createdat" => "Timestamp when the resource was created",
                    "updatedat" => "Timestamp when the resource was last updated",
                    "createdby" => "ID of the user who created the resource",
                    "updatedby" => "ID of the user who last updated the resource",
                    "isdeleted" => "Indicates if the resource has been soft deleted",
                    "deletedat" => "Timestamp when the resource was deleted",
                    "deletedby" => "ID of the user who deleted the resource",
                    "isactive" => "Indicates if the resource is currently active",
                    "nome" => "Name of the resource",
                    "descricao" => "Description of the resource",
                    "preco" => "Price value in the system currency",
                    "quantidade" => "Quantity or amount",
                    "email" => "Email address",
                    "telefone" => "Phone number in Brazilian format",
                    "cpf" => "Brazilian individual taxpayer registry (CPF)",
                    "cnpj" => "Brazilian national registry of legal entities (CNPJ)",
                    "codigobarras" => "Product barcode (EAN-8, EAN-13, UPC-A, etc.)",
                    _ => schema.Description
                };
            }
        }
    }

    private static void AddValidationInfo(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo != null)
        {
            var validationAttributes = context.MemberInfo.GetCustomAttributes<ValidationAttribute>();
            
            foreach (var attribute in validationAttributes)
            {
                switch (attribute)
                {
                    case RequiredAttribute:
                        // This is handled by MarkRequiredProperties
                        break;
                    case StringLengthAttribute stringLength:
                        schema.MaxLength = stringLength.MaximumLength;
                        if (stringLength.MinimumLength > 0)
                            schema.MinLength = stringLength.MinimumLength;
                        break;
                    case MaxLengthAttribute maxLength:
                        schema.MaxLength = maxLength.Length;
                        break;
                    case MinLengthAttribute minLength:
                        schema.MinLength = minLength.Length;
                        break;
                    case RangeAttribute range:
                        if (range.Minimum is double min)
                            schema.Minimum = (decimal)min;
                        if (range.Maximum is double max)
                            schema.Maximum = (decimal)max;
                        break;
                    case RegularExpressionAttribute regex:
                        schema.Pattern = regex.Pattern;
                        break;
                    case EmailAddressAttribute:
                        schema.Format = "email";
                        break;
                    case PhoneAttribute:
                        schema.Format = "phone";
                        break;
                    case UrlAttribute:
                        schema.Format = "uri";
                        break;
                }
            }
        }
    }

    private static void MarkRequiredProperties(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsClass && schema.Properties != null)
        {
            var requiredProperties = new List<string>();
            
            foreach (var property in context.Type.GetProperties())
            {
                var isRequired = property.GetCustomAttribute<RequiredAttribute>() != null ||
                               (!property.PropertyType.IsGenericType || 
                                property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) &&
                               property.PropertyType.IsValueType;
                
                if (isRequired)
                {
                    var propertyName = char.ToLowerInvariant(property.Name[0]) + property.Name[1..];
                    requiredProperties.Add(propertyName);
                }
            }
            
            if (requiredProperties.Any())
            {
                schema.Required = new HashSet<string>(requiredProperties);
            }
        }
    }
}