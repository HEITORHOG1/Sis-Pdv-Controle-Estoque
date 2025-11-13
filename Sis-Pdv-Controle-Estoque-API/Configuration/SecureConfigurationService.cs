using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public interface ISecureConfigurationService
{
    string GetSecureValue(string key);
    void SetSecureValue(string key, string value);
    bool ValidateConfiguration();
    Dictionary<string, string> GetMaskedConfiguration();
}

public class SecureConfigurationService : ISecureConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SecureConfigurationService> _logger;
    private readonly Dictionary<string, string> _secureValues;
    private readonly string _encryptionKey;

    public SecureConfigurationService(IConfiguration configuration, ILogger<SecureConfigurationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _secureValues = new Dictionary<string, string>();
        _encryptionKey = GetOrCreateEncryptionKey();
        LoadSecureValues();
    }

    public string GetSecureValue(string key)
    {
        if (_secureValues.TryGetValue(key, out var encryptedValue))
        {
            try
            {
                return DecryptValue(encryptedValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to decrypt secure value for key {Key}", key);
                throw;
            }
        }

        // Fallback to regular configuration
        var value = _configuration[key];
        if (!string.IsNullOrEmpty(value))
        {
            _logger.LogDebug("Retrieved configuration value for key {Key} from regular configuration", key);
            return value;
        }

        throw new KeyNotFoundException($"Configuration key '{key}' not found");
    }

    public void SetSecureValue(string key, string value)
    {
        try
        {
            var encryptedValue = EncryptValue(value);
            _secureValues[key] = encryptedValue;
            _logger.LogInformation("Secure value set for key {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set secure value for key {Key}", key);
            throw;
        }
    }

    public bool ValidateConfiguration()
    {
        var isValid = true;
        var requiredKeys = new[]
        {
            "ConnectionStrings:DefaultConnection",
            "Authentication:JwtSecret",
            "RabbitMQ:HostName",
            "RabbitMQ:UserName",
            "RabbitMQ:Password"
        };

        foreach (var key in requiredKeys)
        {
            try
            {
                var value = GetSecureValue(key);
                if (string.IsNullOrEmpty(value))
                {
                    _logger.LogError("Required configuration key {Key} is missing or empty", key);
                    isValid = false;
                }
                else
                {
                    _logger.LogDebug("Configuration key {Key} is valid", key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate configuration key {Key}", key);
                isValid = false;
            }
        }

        return isValid;
    }

    public Dictionary<string, string> GetMaskedConfiguration()
    {
        var maskedConfig = new Dictionary<string, string>();
        var sensitiveKeys = new[]
        {
            "password", "secret", "key", "token", "connectionstring"
        };

        foreach (var item in _configuration.AsEnumerable())
        {
            if (item.Value != null)
            {
                var isSensitive = sensitiveKeys.Any(sensitive => 
                    item.Key.Contains(sensitive, StringComparison.OrdinalIgnoreCase));

                maskedConfig[item.Key] = isSensitive ? MaskValue(item.Value) : item.Value;
            }
        }

        return maskedConfig;
    }

    private string GetOrCreateEncryptionKey()
    {
        var key = Environment.GetEnvironmentVariable("PDV_ENCRYPTION_KEY");
        if (string.IsNullOrEmpty(key))
        {
            // In production, this should come from a secure key management service
            key = _configuration["Security:EncryptionKey"] ?? GenerateDefaultKey();
            _logger.LogWarning("Using default encryption key. In production, set PDV_ENCRYPTION_KEY environment variable");
        }
        return key;
    }

    private string GenerateDefaultKey()
    {
        // This is for development only - in production use proper key management
        return "PDV-Default-Encryption-Key-32-Chars!";
    }

    private void LoadSecureValues()
    {
        // In a real implementation, this would load from a secure store
        // For now, we'll just initialize empty
        _logger.LogInformation("Secure configuration service initialized");
    }

    private string EncryptValue(string value)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var encryptedBytes = encryptor.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
        
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
        Array.Copy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
        
        return Convert.ToBase64String(result);
    }

    private string DecryptValue(string encryptedValue)
    {
        var data = Convert.FromBase64String(encryptedValue);
        
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
        
        var iv = new byte[16];
        var encrypted = new byte[data.Length - 16];
        Array.Copy(data, 0, iv, 0, 16);
        Array.Copy(data, 16, encrypted, 0, encrypted.Length);
        
        aes.IV = iv;
        
        using var decryptor = aes.CreateDecryptor();
        var decryptedBytes = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
        
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private static string MaskValue(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= 4)
            return "****";
        
        return value.Substring(0, 2) + new string('*', value.Length - 4) + value.Substring(value.Length - 2);
    }
}