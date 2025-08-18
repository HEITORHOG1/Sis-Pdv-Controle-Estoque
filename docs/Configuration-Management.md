# Configuration Management Guide

## Overview

The PDV System implements a comprehensive configuration management system that supports multiple environments, secure handling of sensitive data, and automatic validation.

## Environment Support

The system supports three environments:

- **Development**: Local development with relaxed security
- **Staging**: Pre-production testing environment
- **Production**: Live production environment with enhanced security

## Configuration Files

### Environment-Specific Files

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Staging.json` - Staging configuration
- `appsettings.Production.json` - Production configuration

### Environment Variables Files

- `.env.development` - Development environment variables
- `.env.staging` - Staging environment variables  
- `.env.production` - Production environment variables
- `.env.example` - Template for environment files

## Setup Instructions

### 1. Initial Environment Setup

Run the setup script to create environment-specific configuration:

```powershell
# Development environment
.\scripts\setup-environment.ps1 -Environment Development

# Staging environment
.\scripts\setup-environment.ps1 -Environment Staging

# Production environment
.\scripts\setup-environment.ps1 -Environment Production
```

### 2. Customize Configuration

Edit the generated `.env.{environment}` file to match your specific requirements:

```bash
# Database Configuration
DB_PASSWORD=your_secure_password
MYSQL_ROOT_PASSWORD=your_mysql_root_password

# RabbitMQ Configuration
RABBITMQ_PASSWORD=your_rabbitmq_password

# Authentication Configuration
JWT_SECRET=your_jwt_secret_minimum_32_characters

# Security Configuration
PDV_ENCRYPTION_KEY=your_32_character_encryption_key
```

### 3. Deploy Application

Use the deployment script:

```powershell
# Deploy to development
.\scripts\deploy.ps1 -Environment Development

# Deploy to staging
.\scripts\deploy.ps1 -Environment Staging

# Deploy to production
.\scripts\deploy.ps1 -Environment Production -Force
```

## Configuration Sections

### Database Options

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_SERVER};Database=${DB_NAME};...",
    "ControleFluxoCaixaConnectionString": "...",
    "RabbitMQ": "amqp://${RABBITMQ_USER}:${RABBITMQ_PASSWORD}@${RABBITMQ_HOST}/"
  }
}
```

### Authentication Options

```json
{
  "Authentication": {
    "JwtSecret": "${JWT_SECRET}",
    "Issuer": "PDV-System",
    "Audience": "PDV-Users",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Performance Options

```json
{
  "Performance": {
    "DefaultTimeoutMinutes": 30,
    "QueryTimeoutMinutes": 10,
    "EnableOptimizations": true,
    "MaxConnections": 1000
  }
}
```

## Environment Variable Substitution

The system supports environment variable substitution using the `${VARIABLE_NAME}` syntax:

- `${VARIABLE_NAME}` - Required environment variable
- `${VARIABLE_NAME:default_value}` - Optional with default value

Example:
```json
{
  "ConnectionString": "Server=${DB_SERVER:localhost};Database=${DB_NAME};Uid=${DB_USER:root};Pwd=${DB_PASSWORD};"
}
```

## Security Features

### Secure Configuration Service

The `ISecureConfigurationService` provides:

- Encrypted storage of sensitive values
- Masked configuration display
- Secure value retrieval
- Configuration validation

### Configuration Validation

Automatic validation includes:

- Required configuration presence
- Data annotation validation
- Connectivity testing
- Security compliance checks

### File Permissions

Environment files are automatically secured with restricted permissions to prevent unauthorized access.

## Docker Configuration

### Development

```bash
docker-compose -f docker-compose.development.yml up
```

### Staging

```bash
docker-compose -f docker-compose.staging.yml up
```

### Production

```bash
docker-compose -f docker-compose.yml up
```

## API Endpoints

### Configuration Management

- `GET /api/v1/configuration/masked` - Get masked configuration
- `POST /api/v1/configuration/validate` - Validate configuration
- `GET /api/v1/configuration/environment` - Get environment info
- `POST /api/v1/configuration/test-connectivity` - Test connectivity
- `POST /api/v1/configuration/reload` - Reload configuration (dev only)

## Best Practices

### Development

1. Use the development environment file for local development
2. Never commit environment files to version control
3. Use the configuration reload endpoint for testing changes
4. Validate configuration regularly during development

### Staging

1. Use production-like configuration with test data
2. Test all configuration changes in staging first
3. Verify connectivity to all external services
4. Run full validation before promoting to production

### Production

1. Use strong, unique passwords and secrets
2. Enable all security features
3. Monitor configuration validation results
4. Implement proper backup and recovery procedures
5. Use external secret management systems when possible

## Troubleshooting

### Configuration Validation Errors

1. Check the validation endpoint: `POST /api/v1/configuration/validate`
2. Review application logs for detailed error messages
3. Verify all required environment variables are set
4. Test connectivity to external services

### Environment Variable Issues

1. Verify environment file exists and has correct permissions
2. Check variable names match exactly (case-sensitive)
3. Ensure no extra spaces or special characters
4. Validate environment variable substitution syntax

### Docker Deployment Issues

1. Verify environment file is loaded correctly
2. Check container logs for configuration errors
3. Ensure all required services are healthy
4. Validate network connectivity between containers

## Migration Guide

### From Previous Configuration

1. Backup existing configuration files
2. Run the setup script for your environment
3. Migrate custom settings to new format
4. Test configuration validation
5. Deploy and verify functionality

### Environment Promotion

1. Export configuration from source environment
2. Run setup script for target environment
3. Import and adapt configuration values
4. Validate configuration in target environment
5. Deploy and test thoroughly

## Security Considerations

### Secrets Management

- Never store secrets in configuration files
- Use environment variables for all sensitive data
- Consider external secret management systems for production
- Rotate secrets regularly

### Access Control

- Restrict access to configuration endpoints
- Use proper authentication and authorization
- Monitor configuration access and changes
- Implement audit logging for configuration operations

### Network Security

- Use HTTPS for all communications
- Implement proper firewall rules
- Use VPNs or private networks when possible
- Monitor network traffic for anomalies