# Configuração de Segredos para Desenvolvimento

Este projeto utiliza **User Secrets** para armazenar dados sensíveis durante o desenvolvimento.

## Como Configurar

### 1. Inicializar User Secrets (já configurado)
```powershell
cd Sis-Pdv-Controle-Estoque-API
dotnet user-secrets init
```

### 2. Configurar Senhas Locais

```powershell
# Senha do banco de dados MySQL
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha_aqui"

# JWT Secret (mínimo 32 caracteres)
dotnet user-secrets set "Authentication:JwtSecret" "sua_chave_jwt_super_secreta_aqui"

# Senha do RabbitMQ (se aplicável)
dotnet user-secrets set "RabbitMQ:Password" "sua_senha_rabbitmq"
```

### 3. Listar Segredos Configurados
```powershell
dotnet user-secrets list
```

### 4. Remover um Segredo
```powershell
dotnet user-secrets remove "ConnectionStrings:DatabasePassword"
```

### 5. Limpar Todos os Segredos
```powershell
dotnet user-secrets clear
```

## Configuração para Produção

Em produção, **NÃO use User Secrets**. Use variáveis de ambiente:

### Docker / Docker Compose
```yaml
environment:
  - ConnectionStrings__DatabasePassword=${DB_PASSWORD}
  - Authentication__JwtSecret=${JWT_SECRET}
  - RabbitMQ__Password=${RABBITMQ_PASSWORD}
```

### Azure App Service
Configure as Application Settings no portal do Azure:
- `ConnectionStrings:DatabasePassword`
- `Authentication:JwtSecret`
- `RabbitMQ:Password`

### Kubernetes
Use Kubernetes Secrets:
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: pdv-secrets
type: Opaque
data:
  db-password: <base64-encoded-password>
  jwt-secret: <base64-encoded-secret>
```

### AWS / Linux
```bash
export ConnectionStrings__DatabasePassword="senha"
export Authentication__JwtSecret="chave_secreta"
export RabbitMQ__Password="senha"
```

## Notas Importantes

1. **NUNCA** commite senhas no `appsettings.json` ou `appsettings.Development.json`
2. User Secrets são armazenados em: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json` (Windows)
3. User Secrets **NÃO são criptografados**, apenas separados do código-fonte
4. Para produção, sempre use soluções seguras como Azure Key Vault, AWS Secrets Manager, ou variáveis de ambiente

## Onde os Segredos São Usados

As configurações são lidas no `Program.cs` e `Setup.cs`:

```csharp
// Connection String com senha do User Secrets
var dbPassword = builder.Configuration["ConnectionStrings:DatabasePassword"];
var connectionString = $"Server=localhost;Database=PDV;Uid=root;Pwd={dbPassword};...";

// JWT Secret do User Secrets
var jwtSecret = builder.Configuration["Authentication:JwtSecret"];
```

## Troubleshooting

### User Secrets não estão sendo carregados
Verifique se o `<UserSecretsId>` está presente no arquivo `.csproj`:
```xml
<PropertyGroup>
  <UserSecretsId>be13dd57-4034-4b77-b15d-1c9a2fee9dfd</UserSecretsId>
</PropertyGroup>
```

### Erro de conexão ao banco
Verifique se configurou a senha correta:
```powershell
dotnet user-secrets list
```

Deve mostrar:
```
ConnectionStrings:DatabasePassword = sua_senha
```

## Referências

- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
