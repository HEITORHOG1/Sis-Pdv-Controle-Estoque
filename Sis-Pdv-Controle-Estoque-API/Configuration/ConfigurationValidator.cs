using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public interface IConfigurationValidator
{
    Task<ValidationResult> ValidateAsync();
    ValidationResult ValidateOptions<T>(T options, string sectionName) where T : class;
}

public class ConfigurationValidator : IConfigurationValidator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConfigurationValidator> _logger;
    private readonly ISecureConfigurationService _secureConfigurationService;

    public ConfigurationValidator(
        IServiceProvider serviceProvider, 
        ILogger<ConfigurationValidator> logger,
        ISecureConfigurationService secureConfigurationService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _secureConfigurationService = secureConfigurationService;
    }

    public async Task<ValidationResult> ValidateAsync()
    {
        var result = new ValidationResult();
        
        try
        {
            _logger.LogInformation("Starting configuration validation...");

            // Validate basic configuration structure
            if (!_secureConfigurationService.ValidateConfiguration())
            {
                result.AddError("Secure configuration validation failed");
            }

            // Validate specific option sections
            ValidateSection<DatabaseOptions>(result, DatabaseOptions.SectionName);
            ValidateSection<AuthenticationOptions>(result, AuthenticationOptions.SectionName);
            ValidateSection<RabbitMQOptions>(result, RabbitMQOptions.SectionName);
            ValidateSection<CacheOptions>(result, CacheOptions.SectionName);
            ValidateSection<BackupOptions>(result, BackupOptions.SectionName);
            ValidateSection<HealthCheckOptions>(result, HealthCheckOptions.SectionName);

            // Validate connectivity
            await ValidateConnectivityAsync(result);

            if (result.IsValid)
            {
                _logger.LogInformation("Configuration validation completed successfully");
            }
            else
            {
                _logger.LogError("Configuration validation failed with {ErrorCount} errors", result.Errors.Count);
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Configuration error: {Error}", error);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Configuration validation failed with exception");
            result.AddError($"Configuration validation exception: {ex.Message}");
        }

        return result;
    }

    public ValidationResult ValidateOptions<T>(T options, string sectionName) where T : class
    {
        var result = new ValidationResult();
        
        if (options == null)
        {
            result.AddError($"Configuration section '{sectionName}' is null");
            return result;
        }

        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(options);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        
        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                result.AddError($"{sectionName}: {validationResult.ErrorMessage}");
            }
        }

        return result;
    }

    private void ValidateSection<T>(ValidationResult result, string sectionName) where T : class
    {
        try
        {
            var options = _serviceProvider.GetService<IOptions<T>>()?.Value;
            if (options == null)
            {
                result.AddError($"Configuration section '{sectionName}' could not be loaded");
                return;
            }

            var sectionResult = ValidateOptions(options, sectionName);
            result.Merge(sectionResult);
        }
        catch (Exception ex)
        {
            result.AddError($"Failed to validate section '{sectionName}': {ex.Message}");
        }
    }

    private async Task ValidateConnectivityAsync(ValidationResult result)
    {
        try
        {
            // Validate database connectivity
            await ValidateDatabaseConnectivityAsync(result);
            
            // Validate RabbitMQ connectivity
            await ValidateRabbitMQConnectivityAsync(result);
        }
        catch (Exception ex)
        {
            result.AddError($"Connectivity validation failed: {ex.Message}");
        }
    }

    private Task ValidateDatabaseConnectivityAsync(ValidationResult result)
    {
        try
        {
            var connectionString = _secureConfigurationService.GetSecureValue("ConnectionStrings:DefaultConnection");
            
            // Basic connection string validation
            if (string.IsNullOrEmpty(connectionString))
            {
                result.AddError("Database connection string is empty");
                return Task.CompletedTask;
            }

            if (!connectionString.Contains("Server=") || !connectionString.Contains("Database="))
            {
                result.AddError("Database connection string is missing required parameters");
                return Task.CompletedTask;
            }

            _logger.LogDebug("Database connection string validation passed");
        }
        catch (Exception ex)
        {
            result.AddError($"Database connectivity validation failed: {ex.Message}");
        }
        
        return Task.CompletedTask;
    }

    private Task ValidateRabbitMQConnectivityAsync(ValidationResult result)
    {
        try
        {
            var hostName = _secureConfigurationService.GetSecureValue("RabbitMQ:HostName");
            var userName = _secureConfigurationService.GetSecureValue("RabbitMQ:UserName");
            var password = _secureConfigurationService.GetSecureValue("RabbitMQ:Password");

            if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                result.AddError("RabbitMQ configuration is incomplete");
                return Task.CompletedTask;
            }

            _logger.LogDebug("RabbitMQ configuration validation passed");
        }
        catch (Exception ex)
        {
            result.AddError($"RabbitMQ connectivity validation failed: {ex.Message}");
        }
        
        return Task.CompletedTask;
    }
}

public class ValidationResult
{
    public List<string> Errors { get; } = new();
    public bool IsValid => !Errors.Any();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void Merge(ValidationResult other)
    {
        Errors.AddRange(other.Errors);
    }
}