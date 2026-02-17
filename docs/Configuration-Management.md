# Gerenciamento de Configuração

## Visão Geral

O sistema PDV utiliza o pipeline de configuração padrão do ASP.NET Core 8.0, com suporte a múltiplos ambientes, substituição por variáveis de ambiente e User Secrets para desenvolvimento local.

## Ambientes Suportados

| Ambiente       | Arquivo                          | Propósito                                     |
|----------------|----------------------------------|-----------------------------------------------|
| Base           | `appsettings.json`               | Configurações padrão compartilhadas           |
| Desenvolvimento| `appsettings.Development.json`   | Overrides para desenvolvimento local          |
| Staging        | `appsettings.Staging.json`       | Configurações de pré-produção                 |
| Produção       | `appsettings.Production.json`    | Configurações com segurança reforçada         |

O ambiente é definido pela variável `ASPNETCORE_ENVIRONMENT`.

## Hierarquia de Configuração (Ordem de Precedência)

```
1. Variáveis de ambiente         ← maior precedência
2. User Secrets (somente Development)
3. appsettings.{Environment}.json
4. appsettings.json              ← menor precedência
```

## Seções de Configuração

### ConnectionStrings

O projeto mantém duas connection strings que apontam para o mesmo banco MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDV_02;Uid=root;Pwd=root;...",
    "ControleFluxoCaixaConnectionString": "Server=localhost;Database=PDV_02;Uid=root;Pwd=root;...",
    "RabbitMQ": "amqp://guest:guest@localhost:5672/"
  }
}
```

> **Nota:** Em produção, as credenciais **DEVEM** ser substituídas por variáveis de ambiente ou User Secrets. Nunca commite senhas reais no repositório.

**Parâmetros de connection pooling configurados:**
- `Pooling=true` — habilita o pool de conexões
- `MinPoolSize=5` / `MaxPoolSize=50` — limites do pool
- `ConnectionTimeout=30` — timeout de obtenção de conexão (segundos)
- `ConnectionLifeTime=300` — vida máxima de uma conexão no pool (segundos)
- `ConnectionIdleTimeout=180` — timeout de inatividade (segundos)

### Autenticação JWT

```json
{
  "Authentication": {
    "JwtSecret": "...",
    "Issuer": "PDV-System",
    "Audience": "PDV-Users",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

> **Segurança:** O `JwtSecret` em `appsettings.json` é apenas para desenvolvimento. Em produção, deve ser fornecido via variável de ambiente `Authentication__JwtSecret` ou Azure Key Vault.

### RabbitMQ

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

Utilizado para mensageria assíncrona (envio de cupons fiscais via SEFAZ).

### SEFAZ (Integração Fiscal)

```json
{
  "Sefaz": {
    "Environment": "Homologacao",
    "UF": "SP",
    "CNPJ": "12345678000195",
    "InscricaoEstadual": "123456789",
    "WebService": {
      "Ambiente": 2,
      "Modelo": "65",
      "Versao": "4.00"
    },
    "Certificado": {
      "Arquivo": "certificado.pfx",
      "Senha": "senha_certificado"
    },
    "Contingencia": {
      "Habilitada": true,
      "TipoEmissao": 9
    },
    "Configuracoes": {
      "TimeoutWebService": 30000,
      "TentativasEnvio": 3,
      "IntervaloTentativas": 5000
    }
  }
}
```

> **Produção:** Modelo 65 = NFC-e (Nota Fiscal ao Consumidor Eletrônica). O certificado digital `.pfx` deve ser mantido em local seguro com permissões restritas.

### Pagamentos

```json
{
  "Payment": {
    "DefaultProcessor": "Mock",
    "Processors": {
      "Mock": { "Enabled": true, "SimulateDelay": true, "DelayMs": 500 },
      "Stone": { "Enabled": false, "ApiKey": "", "Environment": "sandbox" },
      "Cielo": { "Enabled": false, "MerchantId": "", "MerchantKey": "", "Environment": "sandbox" }
    },
    "Validation": {
      "ValidateCardNumber": true,
      "MaxInstallments": 12
    },
    "Timeout": {
      "CreditCard": 30000,
      "DebitCard": 20000,
      "Pix": 10000
    }
  }
}
```

Processadores de pagamento implementados: **Mock** (desenvolvimento), **Stone** e **Cielo** (produção).

### Backup

```json
{
  "BackupSchedule": {
    "EnableScheduledBackups": true,
    "DatabaseBackupSchedule": { "Frequency": "Daily", "PreferredTime": "02:00:00" },
    "FileBackupSchedule": { "Frequency": "Weekly", "PreferredTime": "03:00:00" },
    "FullBackupSchedule": { "Frequency": "Monthly", "PreferredTime": "01:00:00" },
    "DatabaseRetentionDays": 30,
    "FileRetentionDays": 7,
    "FullBackupRetentionDays": 90
  }
}
```

O `BackupSchedulerService` é registrado como `IHostedService` e executa backups automaticamente conforme a agenda configurada.

### Rate Limiting

```json
{
  "RateLimit": {
    "MaxRequests": 100,
    "WindowSizeInMinutes": 1
  }
}
```

### CORS

```json
{
  "Cors": {
    "AllowedOrigins": [
      "https://localhost:3000",
      "https://localhost:5001"
    ]
  }
}
```

> **Produção:** Configure apenas as origens exatas do frontend. Nunca use `*`.

### Serilog (Logging)

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    },
    "Enrich": [
      "FromLogContext", "WithCorrelationId", "WithEnvironmentName",
      "WithMachineName", "WithProcessId", "WithThreadId"
    ]
  }
}
```

Logs são escritos em:
- **Console** — Development
- **Arquivo** — `logs/pdv-api-YYYYMMDD.log` (retenção de 30 dias)

### Health Checks

```json
{
  "HealthChecks": {
    "EvaluationTimeInSeconds": 30,
    "UI": {
      "ApiPath": "/health-ui-api",
      "UIPath": "/health-ui"
    }
  }
}
```

Endpoints disponíveis:
- `/health` — health check básico
- `/health-ui` — dashboard visual com histórico

### Hosting

```json
{
  "Hosting": {
    "Urls": "http://localhost:7003"
  }
}
```

A porta pode ser sobrescrita via `ASPNETCORE_URLS`.

## Substituição por Variáveis de Ambiente

O projeto implementa um `EnvironmentVariableConfigurationProvider` personalizado que suporta a sintaxe `${VARIABLE_NAME}` nos arquivos de configuração. Isso permite usar variáveis de ambiente para substituir valores sensíveis em produção.

Exemplo (em `appsettings.Production.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_SERVER};Database=${DB_NAME};Uid=${DB_USER};Pwd=${DB_PASSWORD}"
  }
}
```

## Configuração Segura (SecureConfigurationService)

O `ISecureConfigurationService` oferece:
- **Mascaramento** de valores sensíveis para exibição em logs e endpoints
- **Validação** automática de configurações obrigatórias
- **Armazenamento criptografado** de valores sensíveis em memória

## API de Configuração

Endpoints disponíveis (apenas com autenticação):

| Método | Endpoint                                | Descrição                           |
|--------|-----------------------------------------|-------------------------------------|
| GET    | `/api/v1/configuration/masked`          | Exibe configurações mascaradas      |
| POST   | `/api/v1/configuration/validate`        | Valida todas as configurações       |
| GET    | `/api/v1/configuration/environment`     | Informações do ambiente atual       |
| POST   | `/api/v1/configuration/test-connectivity` | Testa conexão com dependências    |
| POST   | `/api/v1/configuration/reload`          | Recarrega configuração (dev only)   |

## Validação de Configuração

O `ConfigurationValidator` executa as seguintes verificações na inicialização:
1. Presença de configurações obrigatórias (ConnectionStrings, JWT, etc.)
2. Validação de data annotations
3. Teste de conectividade com banco de dados e RabbitMQ
4. Verificação de conformidade de segurança

## Boas Práticas

### Desenvolvimento
1. Use User Secrets para senhas locais (nunca edite `appsettings.json` com credenciais reais)
2. Consulte o documento [User-Secrets-Configuration.md](./User-Secrets-Configuration.md) para configuração

### Produção
1. Forneça todas as credenciais via variáveis de ambiente
2. Use o arquivo `.env.example` como template para criar seu `.env`
3. Nunca commite arquivos `.env` no repositório (já está no `.gitignore`)
4. Habilite HTTPS e configure CORS com origens específicas
5. Defina o `JwtSecret` com no mínimo 32 caracteres aleatórios

## Arquivo `.env.example`

```bash
DB_PASSWORD=your_database_password_here
MYSQL_ROOT_PASSWORD=your_mysql_root_password_here
RABBITMQ_PASSWORD=your_rabbitmq_password_here
JWT_SECRET=your_jwt_secret_key_here_minimum_32_characters
PDV_ENCRYPTION_KEY=your_encryption_key_here_exactly_32_chars
```

## Troubleshooting

### A aplicação não inicia
1. Verifique se todas as variáveis obrigatórias estão configuradas
2. Consulte os logs em `logs/pdv-api-*.log`
3. Use o endpoint `/api/v1/configuration/validate` para diagnóstico

### Conexão com banco falha
1. Confirme que o MySQL está rodando e acessível
2. Verifique a connection string e credenciais
3. Teste com `mysql -u root -p -h localhost`

### Variáveis de ambiente não são lidas
1. Verifique se `ASPNETCORE_ENVIRONMENT` está definido corretamente
2. Lembre que o separador no Windows é `__` (duplo underscore): `Authentication__JwtSecret`
3. Reinicie a aplicação após alterar variáveis

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/