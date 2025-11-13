namespace Sis_Pdv_Controle_Estoque_API.Configuration;

/// <summary>
/// Configuration for scheduled backups
/// </summary>
public class BackupScheduleConfiguration
{
    /// <summary>
    /// Whether scheduled backups are enabled
    /// </summary>
    public bool EnableScheduledBackups { get; set; } = true;
    
    /// <summary>
    /// Database backup schedule settings
    /// </summary>
    public ScheduleSettings? DatabaseBackupSchedule { get; set; }
    
    /// <summary>
    /// File backup schedule settings
    /// </summary>
    public ScheduleSettings? FileBackupSchedule { get; set; }
    
    /// <summary>
    /// Full backup schedule settings
    /// </summary>
    public ScheduleSettings? FullBackupSchedule { get; set; }
    
    /// <summary>
    /// Retention days for database backups
    /// </summary>
    public int DatabaseRetentionDays { get; set; } = 30;
    
    /// <summary>
    /// Retention days for file backups
    /// </summary>
    public int FileRetentionDays { get; set; } = 7;
    
    /// <summary>
    /// Retention days for full backups
    /// </summary>
    public int FullBackupRetentionDays { get; set; } = 90;
    
    /// <summary>
    /// Paths to include in file backups
    /// </summary>
    public List<string> FileBackupPaths { get; set; } = new();
}

/// <summary>
/// Individual schedule settings for a backup type
/// </summary>
public class ScheduleSettings
{
    /// <summary>
    /// Whether this specific backup schedule is enabled
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Type of backup
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Frequency of backup (Daily, Weekly, Monthly)
    /// </summary>
    public string Frequency { get; set; } = "Daily";
    
    /// <summary>
    /// Interval for the frequency
    /// </summary>
    public int Interval { get; set; } = 1;
    
    /// <summary>
    /// Preferred time to run the backup (HH:mm:ss format)
    /// </summary>
    public TimeSpan PreferredTime { get; set; }
}
