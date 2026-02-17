# Configuração de User Secrets para Desenvolvimento

## O que são User Secrets?

[User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) é o mecanismo do .NET para armazenar dados sensíveis **fora do código-fonte** durante o desenvolvimento. Os segredos são armazenados no perfil do usuário do sistema operacional e **nunca** entram no repositório.

## Pré-Requisito

O projeto `Sis-Pdv-Controle-Estoque-API` já está configurado com um `UserSecretsId` no `.csproj`:

```xml
<PropertyGroup>
  <UserSecretsId>be13dd57-4034-4b77-b15d-1c9a2fee9dfd</UserSecretsId>
</PropertyGroup>
```

## Como Configurar

### 1. Definir os Segredos Necessários

Todos os comandos abaixo devem ser executados a partir da pasta do projeto da API:

```powershell
cd Sis-Pdv-Controle-Estoque-API

# Senha do banco MySQL
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha_mysql"

# JWT Secret (mínimo 32 caracteres - obrigatório)
dotnet user-secrets set "Authentication:JwtSecret" "sua_chave_jwt_super_secreta_com_32_chars"

# Senha do RabbitMQ
dotnet user-secrets set "RabbitMQ:Password" "sua_senha_rabbitmq"
```

### 2. Verificar Segredos Configurados

```powershell
dotnet user-secrets list
```

Saída esperada:
```
ConnectionStrings:DatabasePassword = sua_senha_mysql
Authentication:JwtSecret = sua_chave_jwt_super_secreta_com_32_chars
RabbitMQ:Password = sua_senha_rabbitmq
```

### 3. Gerenciar Segredos

```powershell
# Remover um segredo específico
dotnet user-secrets remove "ConnectionStrings:DatabasePassword"

# Limpar todos os segredos
dotnet user-secrets clear
```

## Onde Ficam Armazenados?

| Sistema Operacional | Caminho                                                                     |
|---------------------|-----------------------------------------------------------------------------|
| Windows             | `%APPDATA%\Microsoft\UserSecrets\be13dd57-4034-4b77-b15d-1c9a2fee9dfd\secrets.json` |
| Linux/macOS         | `~/.microsoft/usersecrets/be13dd57-4034-4b77-b15d-1c9a2fee9dfd/secrets.json` |

O conteúdo é um arquivo JSON simples:
```json
{
  "ConnectionStrings:DatabasePassword": "sua_senha_mysql",
  "Authentication:JwtSecret": "sua_chave_jwt_super_secreta_com_32_chars",
  "RabbitMQ:Password": "sua_senha_rabbitmq"
}
```

> **Atenção:** User Secrets **NÃO são criptografados**. A segurança vem de estarem **fora** do repositório, não de criptografia.

## Como São Usados no Código

No `Program.cs` e `Setup.cs`, os segredos são lidos automaticamente pelo pipeline de configuração do ASP.NET Core:

```csharp
// A senha é lida automaticamente quando o ambiente é Development
var dbPassword = builder.Configuration["ConnectionStrings:DatabasePassword"];

// O JWT secret é lido na configuração de autenticação
var jwtSecret = configuration["Authentication:JwtSecret"]
    ?? throw new InvalidOperationException("JWT Secret not configured");
```

User Secrets só são carregados quando `ASPNETCORE_ENVIRONMENT=Development` (que é o padrão ao rodar via `dotnet run` no Visual Studio ou CLI).

## Configuração para Outros Ambientes

### Docker / Docker Compose

Use variáveis de ambiente com o separador `__` (duplo underscore):

```yaml
# docker-compose.yml
services:
  pdv-api:
    environment:
      - ConnectionStrings__DefaultConnection=Server=mysql;Database=PDV_PROD;Uid=pdvuser;Pwd=${DB_PASSWORD}
      - Authentication__JwtSecret=${JWT_SECRET}
      - RabbitMQ__Password=${RABBITMQ_PASSWORD}
```

E defina os valores no arquivo `.env`:
```bash
DB_PASSWORD=senha_forte_de_producao
JWT_SECRET=chave_jwt_forte_de_producao_com_32_chars
RABBITMQ_PASSWORD=senha_rabbitmq_producao
```

### Azure App Service

Configure como **Application Settings** no portal do Azure:
- `ConnectionStrings:DatabasePassword`
- `Authentication:JwtSecret`
- `RabbitMQ:Password`

### Kubernetes

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: pdv-secrets
type: Opaque
data:
  db-password: <base64-encoded-password>
  jwt-secret: <base64-encoded-secret>
  rabbitmq-password: <base64-encoded-password>
```

### Linux (variáveis de ambiente)

```bash
export ConnectionStrings__DatabasePassword="senha"
export Authentication__JwtSecret="chave_secreta_com_32_chars"
export RabbitMQ__Password="senha_rabbitmq"
```

## Regras de Segurança

1. **NUNCA** commite senhas em `appsettings.json` ou `appsettings.Development.json`
2. O `appsettings.json` do repositório contém senhas padrão (`root`/`guest`) que são **apenas para referência** e devem ser sobrescritas via User Secrets
3. Para produção, sempre use variáveis de ambiente ou soluções como Azure Key Vault / AWS Secrets Manager
4. O arquivo `.env` está no `.gitignore` e **nunca deve ser commitado**
5. Rode `dotnet user-secrets list` para confirmar que seus segredos estão configurados antes de rodar a API

## Troubleshooting

### User Secrets não carregam

**Causa:** O ambiente não é "Development".

**Solução:** Verifique a variável de ambiente:
```powershell
$env:ASPNETCORE_ENVIRONMENT
# Deve retornar "Development"
```

### `UserSecretsId` não encontrado no `.csproj`

**Solução:** Confirme que o `.csproj` da API contém:
```xml
<UserSecretsId>be13dd57-4034-4b77-b15d-1c9a2fee9dfd</UserSecretsId>
```

Ou reinicialize:
```powershell
dotnet user-secrets init
```

### Erro de conexão com banco

**Diagnóstico:**
```powershell
dotnet user-secrets list
```

Deve mostrar:
```
ConnectionStrings:DatabasePassword = sua_senha
```

Se não aparecer, configure novamente:
```powershell
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha_correta"
```

### JWT Secret inválido

O JWT Secret deve ter **no mínimo 32 caracteres**. Se for menor, a API lançará uma exceção na inicialização.

## Referências

- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
