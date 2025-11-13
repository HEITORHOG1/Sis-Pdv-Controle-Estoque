using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public class EnvironmentVariableConfigurationProvider : ConfigurationProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnvironmentVariableConfigurationProvider> _logger;

    public EnvironmentVariableConfigurationProvider(IConfiguration configuration, ILogger<EnvironmentVariableConfigurationProvider> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public override void Load()
    {
        Data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        
        // Load all configuration values and substitute environment variables
        LoadConfigurationRecursively(_configuration.AsEnumerable());
    }

    private void LoadConfigurationRecursively(IEnumerable<KeyValuePair<string, string?>> configItems)
    {
        foreach (var item in configItems)
        {
            if (item.Value != null)
            {
                var substitutedValue = SubstituteEnvironmentVariables(item.Value);
                Data[item.Key] = substitutedValue;
            }
        }
    }

    private string SubstituteEnvironmentVariables(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Pattern to match ${VARIABLE_NAME} or ${VARIABLE_NAME:default_value}
        var pattern = @"\$\{([^}:]+)(?::([^}]*))?\}";
        
        return Regex.Replace(value, pattern, match =>
        {
            var variableName = match.Groups[1].Value;
            var defaultValue = match.Groups[2].Success ? match.Groups[2].Value : string.Empty;
            
            var envValue = Environment.GetEnvironmentVariable(variableName);
            
            if (string.IsNullOrEmpty(envValue))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    _logger.LogInformation("Environment variable {VariableName} not found, using default value", variableName);
                    return defaultValue;
                }
                
                _logger.LogWarning("Environment variable {VariableName} not found and no default value provided", variableName);
                return match.Value; // Return original placeholder if no env var and no default
            }
            
            _logger.LogDebug("Substituted environment variable {VariableName}", variableName);
            return envValue;
        });
    }
}

public class EnvironmentVariableConfigurationSource : IConfigurationSource
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnvironmentVariableConfigurationProvider> _logger;

    public EnvironmentVariableConfigurationSource(IConfiguration configuration, ILogger<EnvironmentVariableConfigurationProvider> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EnvironmentVariableConfigurationProvider(_configuration, _logger);
    }
}