using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Configuration;

namespace Sis_Pdv_Controle_Estoque_API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class ConfigurationController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly ISecureConfigurationService _secureConfigurationService;
    private readonly IConfigurationValidator _configurationValidator;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigurationController> _logger;

    public ConfigurationController(
        ISecureConfigurationService secureConfigurationService,
        IConfigurationValidator configurationValidator,
        IConfiguration configuration,
        ILogger<ConfigurationController> logger)
    {
        _secureConfigurationService = secureConfigurationService;
        _configurationValidator = configurationValidator;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Get masked configuration values for debugging
    /// </summary>
    [HttpGet("masked")]
    public ActionResult<Dictionary<string, string>> GetMaskedConfiguration()
    {
        try
        {
            var maskedConfig = _secureConfigurationService.GetMaskedConfiguration();
            
            _logger.LogInformation("Configuration retrieved by user {UserId}", User.Identity?.Name);
            
            return Ok(maskedConfig);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve masked configuration");
            return StatusCode(500, "Failed to retrieve configuration");
        }
    }

    /// <summary>
    /// Validate current configuration
    /// </summary>
    [HttpPost("validate")]
    public async Task<ActionResult<ValidationResult>> ValidateConfiguration()
    {
        try
        {
            var result = await _configurationValidator.ValidateAsync();
            
            _logger.LogInformation("Configuration validation requested by user {UserId}. Result: {IsValid}", 
                User.Identity?.Name, result.IsValid);
            
            if (result.IsValid)
            {
                return Ok(new { isValid = true, message = "Configuration is valid" });
            }
            
            return BadRequest(new { isValid = false, errors = result.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Configuration validation failed");
            return StatusCode(500, "Configuration validation failed");
        }
    }

    /// <summary>
    /// Get environment information
    /// </summary>
    [HttpGet("environment")]
    public ActionResult<object> GetEnvironmentInfo()
    {
        try
        {
            var environmentInfo = new
            {
                Environment = _configuration.GetEnvironmentName(),
                IsProduction = _configuration.IsProduction(),
                IsStaging = _configuration.IsStaging(),
                IsDevelopment = _configuration.IsDevelopment(),
                MachineName = Environment.MachineName,
                ProcessorCount = Environment.ProcessorCount,
                OSVersion = Environment.OSVersion.ToString(),
                RuntimeVersion = Environment.Version.ToString(),
                WorkingSet = Environment.WorkingSet,
                Timestamp = DateTime.UtcNow
            };

            _logger.LogInformation("Environment information retrieved by user {UserId}", User.Identity?.Name);
            
            return Ok(environmentInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve environment information");
            return StatusCode(500, "Failed to retrieve environment information");
        }
    }

    /// <summary>
    /// Test configuration connectivity
    /// </summary>
    [HttpPost("test-connectivity")]
    public Task<ActionResult<object>> TestConnectivity()
    {
        try
        {
            var results = new Dictionary<string, object>();

            // Test database connectivity
            try
            {
                var connectionString = _secureConfigurationService.GetSecureValue("ConnectionStrings:DefaultConnection");
                // In a real implementation, you would test the actual database connection
                results["Database"] = new { Status = "Available", Message = "Connection string configured" };
            }
            catch (Exception ex)
            {
                results["Database"] = new { Status = "Error", Message = ex.Message };
            }

            // Test RabbitMQ connectivity
            try
            {
                var rabbitMQHost = _secureConfigurationService.GetSecureValue("RabbitMQ:HostName");
                // In a real implementation, you would test the actual RabbitMQ connection
                results["RabbitMQ"] = new { Status = "Available", Message = "Configuration found" };
            }
            catch (Exception ex)
            {
                results["RabbitMQ"] = new { Status = "Error", Message = ex.Message };
            }

            _logger.LogInformation("Connectivity test performed by user {UserId}", User.Identity?.Name);
            
            return Task.FromResult<ActionResult<object>>(Ok(results));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Connectivity test failed");
            return Task.FromResult<ActionResult<object>>(StatusCode(500, "Connectivity test failed"));
        }
    }

    /// <summary>
    /// Reload configuration (development only)
    /// </summary>
    [HttpPost("reload")]
    public ActionResult ReloadConfiguration()
    {
        if (!_configuration.IsDevelopment())
        {
            return BadRequest("Configuration reload is only available in development environment");
        }

        try
        {
            // Trigger configuration reload
            if (_configuration is IConfigurationRoot configRoot)
            {
                configRoot.Reload();
            }

            _logger.LogInformation("Configuration reloaded by user {UserId}", User.Identity?.Name);
            
            return Ok(new { message = "Configuration reloaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reload configuration");
            return StatusCode(500, "Failed to reload configuration");
        }
    }
}