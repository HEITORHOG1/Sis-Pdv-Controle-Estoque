using Microsoft.Extensions.Options;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigurationManagement(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration options with validation
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));
        services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.SectionName));
        services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));
        services.Configure<BackupOptions>(configuration.GetSection(BackupOptions.SectionName));
        services.Configure<HealthCheckOptions>(configuration.GetSection(HealthCheckOptions.SectionName));

        // Add validation for all options
        services.AddSingleton<IValidateOptions<DatabaseOptions>, ValidateOptionsService<DatabaseOptions>>();
        services.AddSingleton<IValidateOptions<AuthenticationOptions>, ValidateOptionsService<AuthenticationOptions>>();
        services.AddSingleton<IValidateOptions<RabbitMQOptions>, ValidateOptionsService<RabbitMQOptions>>();
        services.AddSingleton<IValidateOptions<CacheOptions>, ValidateOptionsService<CacheOptions>>();
        services.AddSingleton<IValidateOptions<BackupOptions>, ValidateOptionsService<BackupOptions>>();
        services.AddSingleton<IValidateOptions<HealthCheckOptions>, ValidateOptionsService<HealthCheckOptions>>();

        // Register configuration services
        services.AddSingleton<ISecureConfigurationService, SecureConfigurationService>();
        services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();

        return services;
    }

    public static IConfigurationBuilder AddEnvironmentVariableSubstitution(this IConfigurationBuilder builder)
    {
        // This will be called after the configuration is built
        return builder;
    }

    public static async Task ValidateConfigurationAsync(this IServiceProvider serviceProvider)
    {
        var validator = serviceProvider.GetRequiredService<IConfigurationValidator>();
        var result = await validator.ValidateAsync();
        
        if (!result.IsValid)
        {
            var errors = string.Join(Environment.NewLine, result.Errors);
            throw new InvalidOperationException($"Configuration validation failed:{Environment.NewLine}{errors}");
        }
    }

    public static T GetValidatedOptions<T>(this IServiceProvider serviceProvider) where T : class
    {
        var options = serviceProvider.GetRequiredService<IOptions<T>>();
        return options.Value;
    }

    public static string GetEnvironmentName(this IConfiguration configuration)
    {
        return configuration["ASPNETCORE_ENVIRONMENT"] ?? 
               Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? 
               "Development";
    }

    public static bool IsProduction(this IConfiguration configuration)
    {
        return configuration.GetEnvironmentName().Equals("Production", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsStaging(this IConfiguration configuration)
    {
        return configuration.GetEnvironmentName().Equals("Staging", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsDevelopment(this IConfiguration configuration)
    {
        return configuration.GetEnvironmentName().Equals("Development", StringComparison.OrdinalIgnoreCase);
    }
}

public class ValidateOptionsService<T> : IValidateOptions<T> where T : class
{
    public ValidateOptionsResult Validate(string? name, T options)
    {
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(options);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        
        if (System.ComponentModel.DataAnnotations.Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            return ValidateOptionsResult.Success;
        }

        var errors = validationResults.Select(r => r.ErrorMessage ?? "Unknown validation error").ToList();
        return ValidateOptionsResult.Fail(errors);
    }
}