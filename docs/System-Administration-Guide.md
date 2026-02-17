# Guia de Administração do Sistema PDV

## Visão Geral

Este guia é destinado a administradores responsáveis por operar, monitorar e manter o sistema PDV (Ponto de Venda) em ambiente de produção.

## Arquitetura do Sistema

### Diagrama de Componentes

```
Clientes
  │
  ├── Aplicação Desktop (WinForms)  ─── .NET 8.0-windows
  │   └── Telas de PDV, cadastros, relatórios
  │
  └── Qualquer Client HTTP (Swagger, Frontend Web, etc.)
          │
          ▼
  ┌──────────────────┐
  │    Nginx          │  ← Reverse proxy, SSL termination
  │   :80 / :443     │
  └────────┬─────────┘
           │
  ┌────────▼─────────┐
  │   PDV API         │  ← ASP.NET Core 8.0 Web API
  │   :8080           │     Controllers + MediatR (CQRS)
  │                   │     Serilog, JWT Auth, Health Checks
  └──┬─────────┬─────┘
     │         │
  ┌──▼───┐  ┌─▼──────────┐
  │MySQL │  │  RabbitMQ   │
  │ 8.0  │  │  3.12+      │
  │:3306 │  │ :5672/:15672│
  └──────┘  └─────────────┘
```

### Dependências entre Serviços

| Serviço   | Depende de       | Health Check                        |
|-----------|------------------|-------------------------------------|
| PDV API   | MySQL, RabbitMQ  | `curl http://localhost:8080/health` |
| MySQL     | —                | `mysqladmin ping`                   |
| RabbitMQ  | —                | `rabbitmq-diagnostics ping`         |
| Nginx     | PDV API          | —                                   |

## Gerenciamento de Usuários

### Roles e Permissões

O sistema implementa RBAC (Role-Based Access Control) com 5 roles pré-definidos:

| Role        | Descrição                              |
|-------------|----------------------------------------|
| SuperAdmin  | Acesso total ao sistema                |
| Admin       | Gerenciamento de usuários e negócio    |
| Manager     | Vendas, estoque e relatórios           |
| Cashier     | Operações de venda e clientes          |
| Viewer      | Acesso somente leitura                 |

### Matriz de Permissões

| Permissão          | SuperAdmin | Admin | Manager | Cashier | Viewer |
|--------------------|:----------:|:-----:|:-------:|:-------:|:------:|
| user.manage        | ✓          | ✓     | —       | —       | —      |
| role.manage        | ✓          | —     | —       | —       | —      |
| inventory.manage   | ✓          | ✓     | ✓       | —       | —      |
| inventory.read     | ✓          | ✓     | ✓       | ✓       | ✓      |
| sales.write        | ✓          | ✓     | ✓       | ✓       | —      |
| sales.read         | ✓          | ✓     | ✓       | ✓       | ✓      |
| reports.generate   | ✓          | ✓     | ✓       | —       | —      |
| reports.read       | ✓          | ✓     | ✓       | —       | ✓      |
| backup.create      | ✓          | —     | —       | —       | —      |
| backup.restore     | ✓          | —     | —       | —       | —      |
| system.admin       | ✓          | —     | —       | —       | —      |

### Endpoints de Gerenciamento de Usuários

| Método | Endpoint                     | Descrição                     | Permissão        |
|--------|------------------------------|-------------------------------|------------------|
| POST   | `/api/v1/users`              | Criar usuário                 | user.manage      |
| GET    | `/api/v1/users`              | Listar usuários               | user.manage      |
| PUT    | `/api/v1/users/{id}`         | Alterar usuário               | user.manage      |
| DELETE | `/api/v1/users/{id}`         | Desativar usuário             | user.manage      |
| POST   | `/api/v1/users/{id}/roles`   | Atribuir roles                | role.manage      |

### Seed Inicial de Autenticação

Na primeira execução, o `AuthSeederService` cria automaticamente:
- As 5 roles padrão
- Permissões associadas a cada role
- Um usuário admin padrão

> **Importante:** Altere a senha do admin imediatamente após o primeiro deploy.

### Modelo de Sessão

O sistema rastreia sessões via tabela `UserSession`:

```sql
-- Sessões ativas
SELECT u.Login, us.CreatedAt, us.ExpiresAt, us.IsActive
FROM UserSession us
JOIN Usuario u ON us.UserId = u.Id
WHERE us.IsActive = 1;

-- Revogar sessões de um usuário
UPDATE UserSession SET IsActive = 0 WHERE UserId = 'guid-do-usuario';
```

## Autenticação

### JWT (JSON Web Token)

O sistema utiliza JWT para autenticação stateless:

- **Algoritmo:** HS256 (HMAC-SHA256) com `SymmetricSecurityKey`
- **Expiração do token:** 60 minutos (configurável)
- **Refresh token:** 7 dias (configurável)
- **ClockSkew:** Zero (expiração exata, sem margem)
- **Validação:** Issuer, Audience, Lifetime e SigningKey

### Middleware de Autenticação

O pipeline de middleware segue esta ordem:

```
1. GlobalExceptionMiddleware     ← Captura exceções não tratadas
2. RequestLoggingMiddleware      ← Loga request/response
3. MetricsMiddleware             ← Coleta métricas de performance
4. SecurityMiddleware            ← HTTPS, CORS, Rate Limiting, Security Headers
5. AuthenticationMiddleware      ← Validação JWT customizada
6. Controllers                   ← Lógica de negócio
```

## Administração do Banco de Dados

### Schema Principal

O banco MySQL contém as seguintes tabelas (gerenciadas pelo Entity Framework via Pomelo.EntityFrameworkCore.MySql):

**Entidades de Negócio:**

| Tabela            | Descrição                    | Auditável |
|-------------------|------------------------------|:---------:|
| Produto           | Catálogo de produtos         | ✓         |
| Categoria         | Categorias de produtos       | ✓         |
| Departamento      | Departamentos da empresa     | ✓         |
| Fornecedor        | Fornecedores cadastrados     | ✓         |
| Cliente           | Clientes cadastrados         | ✓         |
| Colaborador       | Funcionários/colaboradores   | ✓         |
| Pedido            | Pedidos de venda             | ✓         |
| ProdutoPedido     | Itens de cada pedido         | ✓         |
| StockMovement     | Movimentações de estoque     | ✓         |
| Cupom             | Cupons fiscais               | ✓         |

**Entidades de Segurança e Controle:**

| Tabela            | Descrição                    |
|-------------------|------------------------------|
| Usuario           | Usuários do sistema          |
| Role              | Roles (perfis de acesso)     |
| Permission        | Permissões granulares        |
| UserRole          | Relacionamento User ↔ Role   |
| RolePermission    | Relacionamento Role ↔ Permission |
| UserSession       | Sessões ativas de usuários   |
| AuditLog          | Log de auditoria de ações    |

**Entidades de Pagamento:**

| Tabela            | Descrição                    |
|-------------------|------------------------------|
| Payment           | Pagamentos processados       |
| PaymentItem       | Itens de cada pagamento      |
| PaymentAudit      | Auditoria de pagamentos      |
| FiscalReceipt     | Notas/cupons fiscais         |

### Campos de Auditoria

Todas as entidades herdam de `EntityBase` que fornece:

| Campo       | Tipo         | Descrição                           |
|-------------|--------------|-------------------------------------|
| Id          | `Guid`       | Identificador único (UUID v4)       |
| CreatedAt   | `DateTime`   | Data/hora de criação (UTC)          |
| UpdatedAt   | `DateTime?`  | Data/hora da última atualização     |
| CreatedBy   | `Guid?`      | ID do usuário que criou             |
| UpdatedBy   | `Guid?`      | ID do usuário que atualizou         |
| IsDeleted   | `bool`       | Soft delete flag                    |
| DeletedAt   | `DateTime?`  | Data/hora da exclusão lógica        |
| DeletedBy   | `Guid?`      | ID do usuário que excluiu           |

O `AuditInterceptor` no Entity Framework preenche esses campos automaticamente.

### Migrações

As migrações são gerenciadas pelo EF Core e aplicadas automaticamente no startup:

```csharp
// Program.cs
var db = scope.ServiceProvider.GetRequiredService<PdvContext>();
await db.Database.MigrateAsync();
```

Para criar uma nova migração manualmente:

```powershell
dotnet ef migrations add NomeDaMigracao `
  --project Sis-Pdv-Controle-Estoque-Infra `
  --startup-project Sis-Pdv-Controle-Estoque-API
```

### Queries de Monitoramento

```sql
-- Tamanho das tabelas
SELECT TABLE_NAME,
       TABLE_ROWS,
       ROUND((DATA_LENGTH + INDEX_LENGTH) / 1024 / 1024, 2) AS 'Tamanho (MB)'
FROM information_schema.TABLES
WHERE TABLE_SCHEMA = 'PDV_PROD'
ORDER BY (DATA_LENGTH + INDEX_LENGTH) DESC;

-- Conexões ativas
SHOW STATUS LIKE 'Threads_connected';

-- Slow queries
SHOW STATUS LIKE 'Slow_queries';
```

## Backup e Recuperação

### Backups Automáticos

O `BackupSchedulerService` (registered as `IHostedService`) executa backups automaticamente:

| Tipo        | Frequência | Horário    | Retenção   |
|-------------|------------|------------|------------|
| Database    | Diário     | 02:00 AM   | 30 dias    |
| Arquivos    | Semanal    | 03:00 AM   | 7 dias     |
| Completo    | Mensal     | 01:00 AM   | 90 dias    |

### Endpoints de Backup

| Método | Endpoint                              | Descrição                    | Permissão       |
|--------|---------------------------------------|------------------------------|-----------------|
| POST   | `/api/v1/backup/create`               | Criar backup manual          | backup.create   |
| GET    | `/api/v1/backup/list`                 | Listar backups disponíveis   | backup.create   |
| POST   | `/api/v1/backup/restore`              | Restaurar backup             | backup.restore  |

### Backup Manual via CLI

```bash
# Backup do banco
mysqldump -u pdvuser -p \
  --single-transaction \
  --routines \
  --triggers \
  PDV_PROD > backup_$(date +%Y%m%d_%H%M%S).sql

# Backup dos arquivos de configuração
tar -czf config_backup_$(date +%Y%m%d).tar.gz \
  .env appsettings.Production.json
```

### Restauração

```bash
# 1. Pare a API
docker-compose stop pdv-api

# 2. Restaure o banco
docker-compose exec -T mysql mysql -u root -p PDV_PROD < backup_file.sql

# 3. Reinicie a API
docker-compose start pdv-api

# 4. Verifique
curl http://localhost:8080/health
```

## Monitoramento

### Health Checks

| Endpoint              | Tipo       | Descrição                           |
|-----------------------|------------|-------------------------------------|
| `/health`             | Liveness   | Verifica se a aplicação responde    |
| `/health/ready`       | Readiness  | Verifica conexão com dependências   |
| `/health/live`        | Liveness   | Probe de liveness para Kubernetes   |
| `/health-ui`          | Dashboard  | Interface visual com histórico      |
| `/health-ui-api`      | API        | Dados brutos dos health checks      |

Health checks configurados:
- **MySQL** — via `AspNetCore.HealthChecks.MySql`
- **RabbitMQ** — via `AspNetCore.HealthChecks.Rabbitmq`
- **EF Core** — via `HealthChecks.EntityFrameworkCore`
- **Business** — `BusinessHealthCheck` (custom)
- **System Metrics** — `SystemMetricsHealthCheck` (custom)
- **Metrics Collection** — `MetricsCollectionService` (custom)

### Logging (Serilog)

O sistema utiliza **Serilog** com structured logging e os seguintes enrichers:

| Enricher              | Informação adicionada         |
|-----------------------|-------------------------------|
| `CorrelationId`       | ID único por request          |
| `EnvironmentName`     | Nome do ambiente (Prod/Dev)   |
| `MachineName`         | Nome do servidor              |
| `ProcessId`           | PID do processo               |
| `ThreadId`            | ID da thread                  |

**Destinos dos logs:**

| Destino  | Level Mínimo  | Formato                                    | Retenção  |
|----------|---------------|--------------------------------------------|-----------|
| Console  | Debug (Dev)   | `[HH:mm:ss LVL] Message {Properties}`     | —         |
| Arquivo  | Information   | `[yyyy-MM-dd HH:mm:ss.fff LVL] CorrId...` | 30 dias   |

**Localização dos logs:** `logs/pdv-api-YYYYMMDD.log`

### Middleware de Métricas

O `MetricsMiddleware` coleta automaticamente:
- Duração de cada request
- Status code da resposta
- Endpoint acessado

O `RequestLoggingMiddleware` registra:
- Início e fim de cada request
- Método HTTP e URL
- Status code e tempo de resposta

### Aplicação Desktop (WinForms)

O formulário principal (`frmMenu`) possui telas para:
- **Login** — autenticação do operador
- **PDV** — frente de loja (vendas)
- **Cadastros** — Produto, Categoria, Cliente, Colaborador, Departamento, Fornecedor
- **Cupom/Dinheiro** — operações financeiras
- **Relatórios** — vendas, estoque, financeiro

## Relatórios

O sistema gera 4 tipos de relatórios:

| Tipo            | Formato    | Descrição                                 |
|-----------------|------------|-------------------------------------------|
| Vendas          | PDF/Excel  | Volume, receita, ticket médio por período |
| Estoque         | PDF/Excel  | Níveis atuais, itens em falta, validades  |
| Financeiro      | PDF/Excel  | Faturamento, custos, margens              |
| Movimentações   | PDF/Excel  | Histórico de entradas/saídas de estoque   |

Gerados via **iTextSharp** (PDF) e **EPPlus** (Excel).

## Endpoints da API (Resumo)

### Autenticação
- `POST /api/v1/auth/login` — Login
- `POST /api/v1/auth/refresh` — Renovar token
- `POST /api/v1/auth/logout` — Logout

### Catálogo
- `GET/POST/PUT/DELETE /api/v1/categorias`
- `GET/POST/PUT/DELETE /api/v1/produtos`
- `GET/POST/PUT/DELETE /api/v1/departamentos`
- `GET/POST/PUT/DELETE /api/v1/fornecedores`

### Vendas
- `GET/POST/PUT/DELETE /api/v1/pedidos`
- `GET/POST/PUT/DELETE /api/v1/produtospedido`
- `GET/POST /api/v1/clientes`

### Pagamento
- `POST /api/v1/payment/process`
- `POST /api/v1/payment/refund`
- `POST /api/v1/payment/cancel`

### Fiscal
- `POST /api/v1/fiscal/emit`
- `GET /api/v1/fiscal/{id}`

### Inventário
- `POST /api/v1/inventory/adjust`
- `GET /api/v1/inventory/movements`
- `GET /api/v1/inventory/low-stock`

### Relatórios
- `GET /api/v1/reports/sales`
- `GET /api/v1/reports/inventory`
- `GET /api/v1/reports/financial`
- `GET /api/v1/reports/stock-movements`

### Administração
- `GET /api/v1/configuration/masked`
- `POST /api/v1/configuration/validate`
- `POST /api/v1/backup/create`
- `GET /api/v1/seed` — Popular dados iniciais

> A documentação completa da API está disponível em `/api-docs` (Swagger UI) com versionamento via `Asp.Versioning`.

## Segurança

### Headers de Segurança

O `SecurityHeadersMiddleware` adiciona:
- `Strict-Transport-Security` (HSTS)
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Content-Security-Policy`

### Rate Limiting

Configurado via middleware: 100 requests por minuto por IP (configurável).

### Audit Log

Ações críticas são registradas na tabela `AuditLog`:

```sql
-- Ações recentes
SELECT * FROM AuditLog
WHERE Timestamp >= DATE_SUB(NOW(), INTERVAL 24 HOUR)
ORDER BY Timestamp DESC
LIMIT 50;

-- Limpeza periódica (manter apenas 6 meses)
DELETE FROM AuditLog
WHERE Timestamp < DATE_SUB(NOW(), INTERVAL 6 MONTH);
```

## Manutenção Programada

### Diária
- [ ] Verificar health check: `curl http://localhost:8080/health`
- [ ] Revisar logs de erro: `grep ERROR logs/pdv-api-*.log`
- [ ] Confirmar backup concluído

### Semanal
- [ ] Verificar espaço em disco
- [ ] Revisar logs de segurança (falhas de login)
- [ ] Atualizar pacotes do sistema operacional

### Mensal
- [ ] Testar restauração de backup
- [ ] Verificar dependências desatualizadas: `dotnet list package --outdated`
- [ ] Revisar métricas de performance
- [ ] Verificar expiração de certificados SSL

## Troubleshooting

### API não responde
```bash
# Verificar containers
docker-compose ps

# Verificar logs
docker-compose logs --tail=50 pdv-api

# Verificar saúde do MySQL
docker-compose exec mysql mysqladmin ping -u root -p

# Verificar memória/CPU
docker stats
```

### Erro de conexão com banco
```bash
# Testar conexão direta
docker-compose exec mysql mysql -u pdvuser -p PDV_PROD -e "SELECT 1"

# Verificar limites de conexão
docker-compose exec mysql mysql -u root -p -e "SHOW STATUS LIKE 'Max_used_connections'"
```

### Alto uso de memória
```bash
# Verificar processos
docker stats pdv-api

# Coletar dump para análise
dotnet-dump collect -p $(pgrep -f "Sis-Pdv-Controle-Estoque-API")
```

## Contatos de Emergência

- **Administrador do Sistema**: admin@company.com
- **DBA**: dba@company.com

---

*Este guia deve ser revisado trimestralmente para garantir precisão.*

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/