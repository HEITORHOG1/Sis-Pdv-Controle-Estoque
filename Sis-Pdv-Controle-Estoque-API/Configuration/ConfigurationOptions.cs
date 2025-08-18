using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";
    
    [Required]
    public string DefaultConnection { get; set; } = string.Empty;
    
    [Required]
    public string ControleFluxoCaixaConnectionString { get; set; } = string.Empty;
    
    [Required]
    public string RabbitMQ { get; set; } = string.Empty;
}

public class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    [Required]
    [MinLength(32)]
    public string JwtSecret { get; set; } = string.Empty;
    
    [Required]
    public string Issuer { get; set; } = string.Empty;
    
    [Required]
    public string Audience { get; set; } = string.Empty;
    
    [Range(1, 1440)]
    public int TokenExpirationMinutes { get; set; } = 60;
    
    [Range(1, 30)]
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

public class RabbitMQOptions
{
    public const string SectionName = "RabbitMQ";
    
    [Required]
    public string HostName { get; set; } = string.Empty;
    
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
}



public class BackupOptions
{
    public const string SectionName = "Backup";
    
    [Required]
    public string BackupDirectory { get; set; } = string.Empty;
    
    [Range(1, 365)]
    public int RetentionDays { get; set; } = 30;
    
    public bool CompressBackups { get; set; } = true;
    public bool VerifyWithTestRestore { get; set; } = false;
}

public class HealthCheckOptions
{
    public const string SectionName = "HealthChecks";
    
    [Range(10, 300)]
    public int EvaluationTimeInSeconds { get; set; } = 30;
    
    [Range(10, 1000)]
    public int MaximumHistoryEntriesPerEndpoint { get; set; } = 50;
    
    public HealthCheckUIOptions UI { get; set; } = new();
}

public class HealthCheckUIOptions
{
    [Range(50, 1000)]
    public int MaximumExecutionHistoriesPerEndpoint { get; set; } = 100;
    
    public string ApiPath { get; set; } = "/health-ui-api";
    public string UIPath { get; set; } = "/health-ui";
}