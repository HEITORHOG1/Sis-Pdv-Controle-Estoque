# PDV System Deployment Guide

## Overview

This guide provides comprehensive instructions for deploying the PDV (Point of Sale) Control System in different environments.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Environment Setup](#environment-setup)
3. [Database Setup](#database-setup)
4. [Application Configuration](#application-configuration)
5. [Docker Deployment](#docker-deployment)
6. [Manual Deployment](#manual-deployment)
7. [Security Configuration](#security-configuration)
8. [Monitoring Setup](#monitoring-setup)
9. [Troubleshooting](#troubleshooting)

## Prerequisites

### System Requirements

- **Operating System**: Linux (Ubuntu 20.04+ recommended), Windows Server 2019+, or macOS
- **Runtime**: .NET 8.0 Runtime
- **Database**: MySQL 8.0+ or MariaDB 10.5+
- **Message Queue**: RabbitMQ 3.8+
- **Memory**: Minimum 2GB RAM (4GB+ recommended)
- **Storage**: Minimum 10GB free space (50GB+ recommended for production)
- **Network**: HTTPS support, firewall configuration

### Software Dependencies

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y dotnet-runtime-8.0 mysql-server rabbitmq-server nginx

# CentOS/RHEL
sudo yum install -y dotnet-runtime-8.0 mysql-server rabbitmq-server nginx

# Windows (using Chocolatey)
choco install dotnet-8.0-runtime mysql rabbitmq nginx
```

## Environment Setup

### Development Environment

```bash
# Clone the repository
git clone <repository-url>
cd Sis-Pdv-Controle-Estoque

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run database migrations
dotnet ef database update --project Sis-Pdv-Controle-Estoque-Infra --startup-project Sis-Pdv-Controle-Estoque-API
```

### Production Environment

```bash
# Create application directory
sudo mkdir -p /opt/pdv-system
sudo chown -R www-data:www-data /opt/pdv-system

# Create logs directory
sudo mkdir -p /var/log/pdv-system
sudo chown -R www-data:www-data /var/log/pdv-system

# Create backup directory
sudo mkdir -p /var/backups/pdv-system
sudo chown -R www-data:www-data /var/backups/pdv-system
```

## Database Setup

### MySQL Configuration

```sql
-- Create database and user
CREATE DATABASE PDV_Production CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'pdv_user'@'localhost' IDENTIFIED BY 'secure_password_here';
GRANT ALL PRIVILEGES ON PDV_Production.* TO 'pdv_user'@'localhost';
FLUSH PRIVILEGES;

-- Apply performance optimizations
SET GLOBAL innodb_buffer_pool_size = 2147483648; -- 2GB
SET GLOBAL innodb_log_file_size = 268435456; -- 256MB
SET GLOBAL max_connections = 200;
```

### Database Optimization

Run the database optimization script:

```bash
mysql -u root -p PDV_Production < database-optimization.sql
```

### Connection String Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDV_Production;Uid=pdv_user;Pwd=secure_password_here;SslMode=Required;AllowPublicKeyRetrieval=false;"
  }
}
```

## Application Configuration

### Production Configuration (appsettings.Production.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDV_Production;Uid=pdv_user;Pwd=${DB_PASSWORD};SslMode=Required;"
  },
  "Authentication": {
    "JwtSecret": "${JWT_SECRET}",
    "Issuer": "PDV-System-Production",
    "Audience": "PDV-Users-Production",
    "TokenExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 7
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Sis_Pdv_Controle_Estoque": "Information"
      }
    }
  },
  "RateLimit": {
    "MaxRequests": 60,
    "WindowSizeInMinutes": 1
  },
  "Cors": {
    "AllowedOrigins": [
      "https://your-frontend-domain.com"
    ]
  },
  "Sefaz": {
    "Environment": "Producao",
    "Certificado": {
      "Arquivo": "/opt/pdv-system/certificates/certificado.pfx",
      "Senha": "${SEFAZ_CERT_PASSWORD}"
    }
  }
}
```

### Environment Variables

Create `/opt/pdv-system/.env`:

```bash
# Database
DB_PASSWORD=your_secure_database_password

# JWT
JWT_SECRET=your_super_secure_jwt_secret_key_minimum_32_characters

# SEFAZ
SEFAZ_CERT_PASSWORD=your_certificate_password

# Application
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://localhost:5000
```

## Docker Deployment

### Docker Compose Setup

```yaml
# docker-compose.production.yml
version: '3.8'

services:
  pdv-api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=mysql;Database=PDV_Production;Uid=pdv_user;Pwd=${DB_PASSWORD}
    depends_on:
      - mysql
      - rabbitmq
    volumes:
      - ./logs:/app/logs
      - ./backups:/app/backups
      - ./certificates:/app/certificates
    restart: unless-stopped

  mysql:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
      MYSQL_DATABASE: PDV_Production
      MYSQL_USER: pdv_user
      MYSQL_PASSWORD: ${DB_PASSWORD}
    volumes:
      - mysql_data:/var/lib/mysql
      - ./database-optimization.sql:/docker-entrypoint-initdb.d/optimization.sql
    ports:
      - "3306:3306"
    restart: unless-stopped

  rabbitmq:
    image: rabbitmq:3.8-management
    environment:
      RABBITMQ_DEFAULT_USER: ${RABBITMQ_USER}
      RABBITMQ_DEFAULT_PASS: ${RABBITMQ_PASSWORD}
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    restart: unless-stopped

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx/nginx.conf:/etc/nginx/nginx.conf
      - ./certificates:/etc/nginx/certificates
    depends_on:
      - pdv-api
    restart: unless-stopped

volumes:
  mysql_data:
  rabbitmq_data:
```

### Deployment Commands

```bash
# Create environment file
cp .env.example .env
# Edit .env with production values

# Deploy with Docker Compose
docker-compose -f docker-compose.production.yml up -d

# Check logs
docker-compose -f docker-compose.production.yml logs -f pdv-api

# Run database migrations
docker-compose -f docker-compose.production.yml exec pdv-api dotnet ef database update
```

## Manual Deployment

### Build and Publish

```bash
# Build for production
dotnet publish Sis-Pdv-Controle-Estoque-API/Sis-Pdv-Controle-Estoque-API.csproj \
  -c Release \
  -o /opt/pdv-system/app \
  --self-contained false \
  --runtime linux-x64

# Set permissions
sudo chown -R www-data:www-data /opt/pdv-system
sudo chmod +x /opt/pdv-system/app/Sis-Pdv-Controle-Estoque-API
```

### Systemd Service

Create `/etc/systemd/system/pdv-system.service`:

```ini
[Unit]
Description=PDV Control System API
After=network.target mysql.service rabbitmq-server.service

[Service]
Type=notify
User=www-data
Group=www-data
WorkingDirectory=/opt/pdv-system/app
ExecStart=/usr/bin/dotnet /opt/pdv-system/app/Sis-Pdv-Controle-Estoque-API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=pdv-system
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
EnvironmentFile=/opt/pdv-system/.env

[Install]
WantedBy=multi-user.target
```

### Service Management

```bash
# Enable and start service
sudo systemctl enable pdv-system
sudo systemctl start pdv-system

# Check status
sudo systemctl status pdv-system

# View logs
sudo journalctl -u pdv-system -f
```

## Security Configuration

### SSL/TLS Setup

```bash
# Generate SSL certificate (Let's Encrypt)
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com

# Or use existing certificate
sudo cp your-certificate.crt /etc/ssl/certs/pdv-system.crt
sudo cp your-private-key.key /etc/ssl/private/pdv-system.key
sudo chmod 600 /etc/ssl/private/pdv-system.key
```

### Nginx Configuration

```nginx
# /etc/nginx/sites-available/pdv-system
server {
    listen 80;
    server_name your-domain.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name your-domain.com;

    ssl_certificate /etc/ssl/certs/pdv-system.crt;
    ssl_certificate_key /etc/ssl/private/pdv-system.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-RSA-AES256-GCM-SHA512:DHE-RSA-AES256-GCM-SHA512;
    ssl_prefer_server_ciphers off;

    # Security headers
    add_header Strict-Transport-Security "max-age=63072000; includeSubDomains; preload";
    add_header X-Content-Type-Options nosniff;
    add_header X-Frame-Options DENY;
    add_header X-XSS-Protection "1; mode=block";

    # Rate limiting
    limit_req_zone $binary_remote_addr zone=api:10m rate=10r/s;
    limit_req zone=api burst=20 nodelay;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header Upgrade $http_upgrade;
    }
}
```

### Firewall Configuration

```bash
# UFW (Ubuntu)
sudo ufw allow ssh
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw --force enable

# iptables (CentOS/RHEL)
sudo firewall-cmd --permanent --add-service=ssh
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --permanent --add-service=https
sudo firewall-cmd --reload
```

## Monitoring Setup

### Health Checks

The application includes built-in health checks accessible at:
- `/health` - Basic health check
- `/health-ui` - Detailed health dashboard

### Log Monitoring

```bash
# Install log monitoring tools
sudo apt install logrotate rsyslog

# Configure log rotation
sudo tee /etc/logrotate.d/pdv-system << EOF
/var/log/pdv-system/*.log {
    daily
    missingok
    rotate 30
    compress
    delaycompress
    notifempty
    create 644 www-data www-data
    postrotate
        systemctl reload pdv-system
    endscript
}
EOF
```

### Performance Monitoring

```bash
# Install monitoring tools
sudo apt install htop iotop nethogs

# Monitor application performance
htop
sudo iotop -o
sudo nethogs
```

## Backup and Recovery

### Automated Backups

The system includes automated backup functionality:

```json
{
  "BackupSchedule": {
    "EnableScheduledBackups": true,
    "DatabaseBackupSchedule": {
      "Enabled": true,
      "Frequency": "Daily",
      "PreferredTime": "02:00:00"
    }
  }
}
```

### Manual Backup

```bash
# Database backup
mysqldump -u pdv_user -p PDV_Production > backup_$(date +%Y%m%d_%H%M%S).sql

# Application backup
tar -czf pdv_app_backup_$(date +%Y%m%d_%H%M%S).tar.gz /opt/pdv-system/app

# Configuration backup
tar -czf pdv_config_backup_$(date +%Y%m%d_%H%M%S).tar.gz /opt/pdv-system/.env /opt/pdv-system/appsettings.Production.json
```

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   ```bash
   # Check MySQL status
   sudo systemctl status mysql
   
   # Test connection
   mysql -u pdv_user -p -h localhost PDV_Production
   ```

2. **Application Won't Start**
   ```bash
   # Check logs
   sudo journalctl -u pdv-system -n 50
   
   # Check configuration
   dotnet /opt/pdv-system/app/Sis-Pdv-Controle-Estoque-API.dll --environment Production
   ```

3. **Performance Issues**
   ```bash
   # Check system resources
   htop
   df -h
   free -m
   
   # Check database performance
   mysql -u root -p -e "SHOW PROCESSLIST;"
   ```

### Log Analysis

```bash
# Application logs
tail -f /var/log/pdv-system/pdv-api-*.log

# System logs
sudo journalctl -u pdv-system -f

# Nginx logs
sudo tail -f /var/log/nginx/access.log
sudo tail -f /var/log/nginx/error.log
```

### Performance Tuning

1. **Database Optimization**
   - Run `database-optimization.sql`
   - Monitor slow queries
   - Adjust buffer pool size

2. **Application Tuning**
   - Adjust performance settings
   - Configure connection pooling
   - Monitor memory usage

3. **Web Server Optimization**
   - Enable gzip compression
   - Configure caching headers
   - Optimize SSL settings

## Maintenance

### Regular Tasks

1. **Daily**
   - Check application logs
   - Verify backup completion
   - Monitor system resources

2. **Weekly**
   - Update system packages
   - Review security logs
   - Check disk space

3. **Monthly**
   - Update application dependencies
   - Review performance metrics
   - Test backup restoration

### Update Procedure

```bash
# 1. Backup current version
sudo systemctl stop pdv-system
cp -r /opt/pdv-system/app /opt/pdv-system/app.backup

# 2. Deploy new version
dotnet publish -c Release -o /opt/pdv-system/app.new

# 3. Run database migrations
dotnet ef database update --project /opt/pdv-system/app.new

# 4. Switch to new version
mv /opt/pdv-system/app /opt/pdv-system/app.old
mv /opt/pdv-system/app.new /opt/pdv-system/app

# 5. Start application
sudo systemctl start pdv-system

# 6. Verify deployment
curl -k https://localhost/health
```

## Support

For technical support and additional documentation:
- Check application logs first
- Review this deployment guide
- Contact system administrator
- Refer to API documentation at `/swagger`