# PDV System Administration Guide

## Overview

This guide provides comprehensive instructions for system administrators managing the PDV (Point of Sale) Control System.

## Table of Contents

1. [System Architecture](#system-architecture)
2. [User Management](#user-management)
3. [Security Administration](#security-administration)
4. [Database Administration](#database-administration)
5. [Backup and Recovery](#backup-and-recovery)
6. [Monitoring and Alerting](#monitoring-and-alerting)
7. [Performance Optimization](#performance-optimization)
8. [Troubleshooting](#troubleshooting)
9. [Maintenance Procedures](#maintenance-procedures)

## System Architecture

### Component Overview

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Web Client    │    │  Mobile Client  │    │  Desktop App    │
└─────────┬───────┘    └─────────┬───────┘    └─────────┬───────┘
          │                      │                      │
          └──────────────────────┼──────────────────────┘
                                 │
                    ┌─────────────┴───────────┐
                    │      Load Balancer      │
                    │       (Nginx)           │
                    └─────────────┬───────────┘
                                 │
                    ┌─────────────┴───────────┐
                    │      PDV API            │
                    │   (.NET 8 Web API)      │
                    └─────────────┬───────────┘
                                 │
          ┌──────────────────────┼──────────────────────┐
          │                      │                      │
┌─────────┴───────┐    ┌─────────┴───────┐    ┌─────────┴───────┐
│     MySQL       │    │    RabbitMQ     │    │   File System   │
│   Database      │    │  Message Queue  │    │   (Backups)     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### Service Dependencies

- **PDV API**: Main application service
- **MySQL**: Primary database
- **RabbitMQ**: Message queue for async processing
- **Nginx**: Reverse proxy and load balancer
- **File System**: Backup storage and logs

## User Management

### User Roles and Permissions

#### Default Roles

1. **SuperAdmin**
   - Full system access
   - User and role management
   - System configuration
   - Backup and restore

2. **Admin**
   - User management (except SuperAdmin)
   - Business configuration
   - Reports access
   - Inventory management

3. **Manager**
   - Sales management
   - Inventory view/edit
   - Reports (limited)
   - Employee management

4. **Cashier**
   - Sales operations
   - Customer management
   - Basic inventory view

5. **Viewer**
   - Read-only access
   - Basic reports

#### Permission Matrix

| Permission | SuperAdmin | Admin | Manager | Cashier | Viewer |
|------------|------------|-------|---------|---------|--------|
| user.manage | ✓ | ✓ | - | - | - |
| role.manage | ✓ | - | - | - | - |
| inventory.manage | ✓ | ✓ | ✓ | - | - |
| inventory.write | ✓ | ✓ | ✓ | - | - |
| inventory.read | ✓ | ✓ | ✓ | ✓ | ✓ |
| sales.manage | ✓ | ✓ | ✓ | - | - |
| sales.write | ✓ | ✓ | ✓ | ✓ | - |
| sales.read | ✓ | ✓ | ✓ | ✓ | ✓ |
| reports.generate | ✓ | ✓ | ✓ | - | - |
| reports.read | ✓ | ✓ | ✓ | - | ✓ |
| backup.manage | ✓ | - | - | - | - |
| system.admin | ✓ | - | - | - | - |

### User Management Commands

#### Creating Users

```bash
# Using API endpoint
curl -X POST "https://your-domain.com/api/v1/users" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "new_user",
    "email": "user@company.com",
    "password": "SecurePassword123!",
    "roles": ["Cashier"]
  }'
```

#### Managing User Sessions

```sql
-- View active sessions
SELECT u.Username, us.CreatedAt, us.ExpiresAt, us.IsActive
FROM UserSession us
JOIN Usuario u ON us.UserId = u.Id
WHERE us.IsActive = 1;

-- Revoke user sessions
UPDATE UserSession 
SET IsActive = 0 
WHERE UserId = 'user-guid-here';
```

#### Password Policies

Configure in `appsettings.json`:

```json
{
  "PasswordPolicy": {
    "MinimumLength": 8,
    "RequireUppercase": true,
    "RequireLowercase": true,
    "RequireDigit": true,
    "RequireSpecialCharacter": true,
    "MaxAge": 90,
    "PreventReuse": 5
  }
}
```

## Security Administration

### SSL/TLS Management

#### Certificate Renewal

```bash
# Let's Encrypt renewal
sudo certbot renew --dry-run
sudo certbot renew

# Manual certificate update
sudo cp new-certificate.crt /etc/ssl/certs/pdv-system.crt
sudo cp new-private-key.key /etc/ssl/private/pdv-system.key
sudo systemctl reload nginx
```

#### Security Headers Verification

```bash
# Test security headers
curl -I https://your-domain.com/health

# Expected headers:
# Strict-Transport-Security: max-age=31536000; includeSubDomains
# X-Content-Type-Options: nosniff
# X-Frame-Options: DENY
# X-XSS-Protection: 1; mode=block
```

### Rate Limiting Configuration

```json
{
  "RateLimit": {
    "MaxRequests": 100,
    "WindowSizeInMinutes": 1,
    "EnableIpWhitelist": true,
    "WhitelistedIps": [
      "192.168.1.0/24",
      "10.0.0.0/8"
    ]
  }
}
```

### Audit Log Management

#### Viewing Audit Logs

```sql
-- Recent user actions
SELECT al.*, u.Username
FROM AuditLog al
LEFT JOIN Usuario u ON al.UserId = u.Id
WHERE al.Timestamp >= DATE_SUB(NOW(), INTERVAL 24 HOUR)
ORDER BY al.Timestamp DESC;

-- Failed login attempts
SELECT *
FROM AuditLog
WHERE Action = 'LoginFailed'
AND Timestamp >= DATE_SUB(NOW(), INTERVAL 1 HOUR)
ORDER BY Timestamp DESC;
```

#### Audit Log Cleanup

```sql
-- Clean logs older than 6 months
CALL CleanupOldAuditLogs();

-- Manual cleanup
DELETE FROM AuditLog 
WHERE Timestamp < DATE_SUB(NOW(), INTERVAL 6 MONTH);
```

## Database Administration

### Database Maintenance

#### Daily Tasks

```sql
-- Check database size
SELECT 
    table_schema AS 'Database',
    ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'Size (MB)'
FROM information_schema.tables 
WHERE table_schema = 'PDV_Production'
GROUP BY table_schema;

-- Check table statistics
SELECT 
    TABLE_NAME,
    TABLE_ROWS,
    ROUND(((DATA_LENGTH + INDEX_LENGTH) / 1024 / 1024), 2) AS 'Size (MB)'
FROM information_schema.TABLES 
WHERE TABLE_SCHEMA = 'PDV_Production'
ORDER BY (DATA_LENGTH + INDEX_LENGTH) DESC;
```

#### Performance Monitoring

```sql
-- Slow queries
SELECT * FROM v_slow_queries LIMIT 10;

-- Index usage
SELECT * FROM v_index_usage WHERE times_used = 0;

-- Table access patterns
SELECT * FROM v_table_access ORDER BY read_operations DESC;
```

#### Index Optimization

```sql
-- Find missing indexes
SELECT 
    CONCAT('CREATE INDEX IX_', TABLE_NAME, '_', COLUMN_NAME, ' ON ', TABLE_NAME, '(', COLUMN_NAME, ');') AS suggested_index
FROM information_schema.COLUMNS c
WHERE TABLE_SCHEMA = 'PDV_Production'
AND COLUMN_NAME IN ('CreatedAt', 'UpdatedAt', 'IsDeleted')
AND NOT EXISTS (
    SELECT 1 FROM information_schema.STATISTICS s
    WHERE s.TABLE_SCHEMA = c.TABLE_SCHEMA
    AND s.TABLE_NAME = c.TABLE_NAME
    AND s.COLUMN_NAME = c.COLUMN_NAME
);
```

### Database Backup Verification

```bash
# Test backup integrity
mysql -u pdv_user -p PDV_Test < latest_backup.sql

# Verify backup completeness
mysql -u pdv_user -p -e "
SELECT 
    COUNT(*) as table_count,
    SUM(TABLE_ROWS) as total_rows
FROM information_schema.TABLES 
WHERE TABLE_SCHEMA = 'PDV_Test';"
```

## Backup and Recovery

### Backup Strategy

#### Automated Backups

The system performs three types of backups:

1. **Database Backups** (Daily at 2:00 AM)
2. **File Backups** (Weekly at 3:00 AM)
3. **Full System Backups** (Monthly at 1:00 AM)

#### Manual Backup Commands

```bash
# Database backup
mysqldump -u pdv_user -p \
  --single-transaction \
  --routines \
  --triggers \
  PDV_Production > backup_$(date +%Y%m%d_%H%M%S).sql

# Application files backup
tar -czf app_backup_$(date +%Y%m%d_%H%M%S).tar.gz \
  /opt/pdv-system/app \
  /opt/pdv-system/.env \
  /opt/pdv-system/appsettings.Production.json

# Logs backup
tar -czf logs_backup_$(date +%Y%m%d_%H%M%S).tar.gz \
  /var/log/pdv-system/
```

### Recovery Procedures

#### Database Recovery

```bash
# 1. Stop the application
sudo systemctl stop pdv-system

# 2. Create backup of current database
mysqldump -u pdv_user -p PDV_Production > current_backup.sql

# 3. Restore from backup
mysql -u pdv_user -p PDV_Production < backup_file.sql

# 4. Verify restoration
mysql -u pdv_user -p -e "SELECT COUNT(*) FROM PDV_Production.Usuario;"

# 5. Start the application
sudo systemctl start pdv-system
```

#### Application Recovery

```bash
# 1. Stop services
sudo systemctl stop pdv-system nginx

# 2. Restore application files
sudo rm -rf /opt/pdv-system/app
sudo tar -xzf app_backup.tar.gz -C /

# 3. Set permissions
sudo chown -R www-data:www-data /opt/pdv-system

# 4. Start services
sudo systemctl start pdv-system nginx
```

### Disaster Recovery Plan

#### RTO/RPO Targets

- **Recovery Time Objective (RTO)**: 4 hours
- **Recovery Point Objective (RPO)**: 24 hours

#### Recovery Steps

1. **Assess the situation** (15 minutes)
2. **Prepare recovery environment** (1 hour)
3. **Restore database** (1 hour)
4. **Restore application** (30 minutes)
5. **Verify system functionality** (1 hour)
6. **Update DNS/routing** (30 minutes)

## Monitoring and Alerting

### Health Check Endpoints

- `/health` - Basic application health
- `/health-ui` - Detailed health dashboard
- `/health/ready` - Readiness probe
- `/health/live` - Liveness probe

### Key Metrics to Monitor

#### Application Metrics

```bash
# CPU and Memory usage
ps aux | grep dotnet
htop

# Application logs
tail -f /var/log/pdv-system/pdv-api-*.log | grep -E "(ERROR|WARN)"

# Request metrics
curl -s https://your-domain.com/health | jq '.status'
```

#### Database Metrics

```sql
-- Connection count
SHOW STATUS LIKE 'Threads_connected';

-- Query performance
SHOW STATUS LIKE 'Slow_queries';

-- Buffer pool usage
SHOW STATUS LIKE 'Innodb_buffer_pool_pages_%';
```

#### System Metrics

```bash
# Disk usage
df -h

# Memory usage
free -m

# Network connections
netstat -tulpn | grep :5000
```

### Alerting Rules

#### Critical Alerts

1. **Application Down** - HTTP 500 responses > 5%
2. **Database Connection Failed** - Cannot connect to MySQL
3. **Disk Space Low** - < 10% free space
4. **Memory Usage High** - > 90% memory usage
5. **SSL Certificate Expiring** - < 30 days until expiration

#### Warning Alerts

1. **High Response Time** - Average response time > 2 seconds
2. **Failed Login Attempts** - > 10 failed attempts in 5 minutes
3. **Backup Failed** - Scheduled backup did not complete
4. **Queue Length High** - RabbitMQ queue > 1000 messages

### Log Analysis

#### Important Log Patterns

```bash
# Error patterns
grep -E "(ERROR|FATAL)" /var/log/pdv-system/*.log

# Security events
grep -E "(Authentication|Authorization|Failed)" /var/log/pdv-system/*.log

# Performance issues
grep -E "(timeout|slow|performance)" /var/log/pdv-system/*.log
```

## Performance Optimization

### Application Performance

#### Memory Optimization

```json
{
  "Performance": {
    "DefaultTimeoutMinutes": 15,
    "QueryTimeoutMinutes": 5,
    "MaxConnections": 500
  }
}
```

#### Connection Pool Tuning

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDV_Production;Uid=pdv_user;Pwd=password;Pooling=true;MinPoolSize=5;MaxPoolSize=100;ConnectionTimeout=30;"
  }
}
```

### Database Performance

#### Query Optimization

```sql
-- Enable slow query log
SET GLOBAL slow_query_log = 'ON';
SET GLOBAL long_query_time = 2;

-- Analyze slow queries
SELECT * FROM mysql.slow_log ORDER BY start_time DESC LIMIT 10;
```

#### Buffer Pool Optimization

```sql
-- Check buffer pool status
SHOW STATUS LIKE 'Innodb_buffer_pool_%';

-- Recommended settings (adjust based on available RAM)
SET GLOBAL innodb_buffer_pool_size = 2147483648; -- 2GB
```

### Web Server Performance

#### Nginx Optimization

```nginx
# /etc/nginx/nginx.conf
worker_processes auto;
worker_connections 1024;

gzip on;
gzip_vary on;
gzip_min_length 1024;
gzip_types text/plain text/css application/json application/javascript;

# Caching
location ~* \.(css|js|png|jpg|jpeg|gif|ico|svg)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
}
```

## Troubleshooting

### Common Issues

#### Application Won't Start

```bash
# Check service status
sudo systemctl status pdv-system

# Check logs
sudo journalctl -u pdv-system -n 50

# Check configuration
dotnet /opt/pdv-system/app/Sis-Pdv-Controle-Estoque-API.dll --environment Production --urls http://localhost:5000
```

#### Database Connection Issues

```bash
# Test database connection
mysql -u pdv_user -p -h localhost PDV_Production

# Check MySQL status
sudo systemctl status mysql

# Check connection limits
mysql -u root -p -e "SHOW STATUS LIKE 'Max_used_connections';"
```

#### High Memory Usage

```bash
# Check memory usage by process
ps aux --sort=-%mem | head -10

# Check for memory leaks
dotnet-dump collect -p $(pgrep -f "Sis-Pdv-Controle-Estoque-API")
```

#### SSL Certificate Issues

```bash
# Check certificate validity
openssl x509 -in /etc/ssl/certs/pdv-system.crt -text -noout

# Test SSL configuration
openssl s_client -connect your-domain.com:443 -servername your-domain.com
```

### Performance Issues

#### Slow Database Queries

```sql
-- Find slow queries
SELECT * FROM performance_schema.events_statements_summary_by_digest
WHERE AVG_TIMER_WAIT > 1000000000
ORDER BY AVG_TIMER_WAIT DESC
LIMIT 10;

-- Check for missing indexes
SELECT * FROM sys.statements_with_runtimes_in_95th_percentile;
```

#### High CPU Usage

```bash
# Check CPU usage by process
top -p $(pgrep -f "Sis-Pdv-Controle-Estoque-API")

# Profile application
dotnet-trace collect -p $(pgrep -f "Sis-Pdv-Controle-Estoque-API") --duration 00:00:30
```

## Maintenance Procedures

### Regular Maintenance Tasks

#### Daily Tasks

1. **Check system health**
   ```bash
   curl -s https://your-domain.com/health
   ```

2. **Review error logs**
   ```bash
   grep -E "(ERROR|FATAL)" /var/log/pdv-system/*.log | tail -20
   ```

3. **Check disk space**
   ```bash
   df -h
   ```

4. **Verify backup completion**
   ```bash
   ls -la /var/backups/pdv-system/ | tail -5
   ```

#### Weekly Tasks

1. **Update system packages**
   ```bash
   sudo apt update && sudo apt upgrade
   ```

2. **Analyze database performance**
   ```sql
   CALL UpdateTableStatistics();
   ```

3. **Review security logs**
   ```bash
   grep -E "(Failed|Authentication)" /var/log/pdv-system/*.log | tail -50
   ```

4. **Check SSL certificate expiration**
   ```bash
   openssl x509 -in /etc/ssl/certs/pdv-system.crt -noout -dates
   ```

#### Monthly Tasks

1. **Update application dependencies**
   ```bash
   # Check for updates
   dotnet list package --outdated
   ```

2. **Review and optimize database**
   ```sql
   -- Run optimization script
   source database-optimization.sql
   ```

3. **Test backup restoration**
   ```bash
   # Test restore to staging environment
   mysql -u pdv_user -p PDV_Staging < latest_backup.sql
   ```

4. **Security audit**
   ```bash
   # Check for security updates
   sudo apt list --upgradable | grep -i security
   ```

### Update Procedures

#### Application Updates

1. **Preparation**
   ```bash
   # Create backup
   sudo systemctl stop pdv-system
   cp -r /opt/pdv-system/app /opt/pdv-system/app.backup.$(date +%Y%m%d)
   ```

2. **Deploy new version**
   ```bash
   # Build and publish new version
   dotnet publish -c Release -o /opt/pdv-system/app.new
   
   # Run database migrations
   dotnet ef database update --project /opt/pdv-system/app.new
   ```

3. **Switch versions**
   ```bash
   mv /opt/pdv-system/app /opt/pdv-system/app.old
   mv /opt/pdv-system/app.new /opt/pdv-system/app
   sudo chown -R www-data:www-data /opt/pdv-system/app
   ```

4. **Verification**
   ```bash
   sudo systemctl start pdv-system
   curl -s https://your-domain.com/health
   ```

#### Rollback Procedure

```bash
# Stop current version
sudo systemctl stop pdv-system

# Restore previous version
mv /opt/pdv-system/app /opt/pdv-system/app.failed
mv /opt/pdv-system/app.old /opt/pdv-system/app

# Restore database if needed
mysql -u pdv_user -p PDV_Production < database_backup_before_update.sql

# Start service
sudo systemctl start pdv-system
```

## Emergency Procedures

### System Recovery Checklist

1. **Assess the situation** (5 minutes)
   - Identify the scope of the problem
   - Determine if it's a partial or complete outage
   - Check if data is at risk

2. **Immediate actions** (15 minutes)
   - Stop affected services
   - Secure any data at risk
   - Notify stakeholders

3. **Recovery actions** (varies)
   - Follow appropriate recovery procedure
   - Monitor progress
   - Test functionality

4. **Post-recovery** (30 minutes)
   - Verify all systems are operational
   - Document the incident
   - Plan preventive measures

### Contact Information

- **System Administrator**: admin@company.com
- **Database Administrator**: dba@company.com
- **Security Team**: security@company.com
- **Emergency Hotline**: +1-XXX-XXX-XXXX

### Escalation Matrix

1. **Level 1**: System Administrator (0-2 hours)
2. **Level 2**: Senior Administrator + DBA (2-4 hours)
3. **Level 3**: Management + External Support (4+ hours)

---

*This guide should be reviewed and updated quarterly to ensure accuracy and completeness.*

---

Author: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/