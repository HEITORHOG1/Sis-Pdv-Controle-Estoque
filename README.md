# Sis-Pdv-Controle-Estoque

> Sistema de Ponto de Venda (PDV) e Controle de Estoque — open-source, construído em ASP.NET Core 8.0 com arquitetura DDD e CQRS.

## Autor

- **Heitor Gonçalves** — [LinkedIn](https://www.linkedin.com/in/heitorhog/)

## Sobre o Projeto

Sistema completo de PDV (frente de loja) e gerenciamento de estoque, ideal para aprendizado e uso em pequenos comércios. Inclui API RESTful, aplicação desktop WinForms, integração fiscal (SEFAZ/NFC-e), processamento de pagamentos e mensageria assíncrona via RabbitMQ.

## Funcionalidades

| Módulo              | Descrição                                                                |
|---------------------|--------------------------------------------------------------------------|
| **PDV (Frente de Loja)** | Vendas com seleção de produtos, cliente, forma de pagamento e cupom |
| **Cadastros**        | Produto, Categoria, Departamento, Fornecedor, Cliente, Colaborador      |
| **Estoque**          | Controle de entrada/saída, movimentações, ponto de reposição, alertas   |
| **Pagamentos**       | Processamento de pagamentos (Mock, Stone, Cielo), estorno e cancelamento|
| **Fiscal**           | Integração SEFAZ para NFC-e (modelo 65), contingência offline          |
| **Relatórios**       | Vendas, estoque, financeiro e movimentações em PDF e Excel              |
| **Autenticação**     | JWT com roles e permissões (RBAC), refresh token, auditoria            |
| **Backup**           | Agendamento automático (diário/semanal/mensal) via background service   |
| **Health Checks**    | Dashboard visual com status de MySQL, RabbitMQ e métricas do sistema    |

## Arquitetura

### Camadas do Projeto

```
Sis-Pdv-Controle-Estoque.sln
│
├── Sis-Pdv-Controle-Estoque/           → Domain (Modelos, Commands, Interfaces, Validators)
│   ├── Model/                          → Entidades: Produto, Pedido, Payment, Usuario, etc.
│   ├── Commands/                       → Handlers CQRS via MediatR (181 arquivos)
│   ├── Interfaces/                     → Contratos de repositórios e serviços
│   └── Validators/                     → Validações de domínio (CPF/CNPJ, código de barras)
│
├── Sis-Pdv-Controle-Estoque-Infra/     → Infrastructure (Repositórios, EF Core, Migrações)
│   ├── Repositories/                   → Implementações dos repositórios
│   ├── Mappings/                       → Configurações EF Core (Fluent API)
│   └── Migrations/                     → Migrações do banco de dados
│
├── Sis-Pdv-Controle-Estoque-API/       → API (Controllers, Middleware, Configuração)
│   ├── Controllers/                    → 20 controllers (Auth, Produto, Pedido, Payment, etc.)
│   ├── Middleware/                     → Exception, Logging, Metrics, Security, Auth
│   ├── Configuration/                  → Swagger, CORS, Security, Health Checks, etc.
│   └── Services/                       → Auth, Backup, Payment, Reports, Health, Stock
│
├── Sis-Pdv-Controle-Estoque-Form/      → Desktop App (WinForms .NET 8.0)
│   ├── Paginas/                        → Telas: Login, PDV, Cadastros, Relatórios
│   └── Services/                       → Comunicação com a API
│
├── MessageBus/                         → Biblioteca de mensageria (RabbitMQ)
│
├── Sis-Pdv-Controle-Estoque.Tests/     → Testes unitários e de integração
│
└── docs/                               → Documentação
```

### Padrões de Design

| Padrão                       | Implementação                                            |
|------------------------------|----------------------------------------------------------|
| **DDD (Domain-Driven Design)** | Modelos ricos com validações de domínio, `EntityBase` com auditoria |
| **CQRS**                     | Segregação Command/Query via MediatR (Request/Handler)   |
| **Repository Pattern**       | Interfaces no Domain, implementações na Infrastructure   |
| **Unit of Work**             | Transações coordenadas via `IUnitOfWork`                 |
| **Soft Delete**              | `IsDeleted`, `DeletedAt`, `DeletedBy` em todas entidades |
| **Validation Pipeline**      | FluentValidation + `ValidationBehavior` no pipeline MediatR |
| **Audit Trail**              | `AuditInterceptor` preenche campos de auditoria automaticamente |

### Diagrama de Arquitetura

```
┌─────────────────────────────────────────────────┐
│              Clientes                           │
│  ┌──────────┐  ┌──────────┐  ┌──────────────┐  │
│  │ WinForms │  │ Swagger  │  │  Frontend    │  │
│  │  (PDV)   │  │   UI     │  │   (futuro)   │  │
│  └─────┬────┘  └────┬─────┘  └──────┬───────┘  │
└────────┼────────────┼───────────────┼───────────┘
         └────────────┼───────────────┘
                      ▼
              ┌───────────────┐
              │     Nginx     │  (Reverse Proxy + SSL)
              └───────┬───────┘
                      ▼
         ┌────────────────────────┐
         │      PDV API           │
         │  ASP.NET Core 8.0      │
         │                        │
         │  [Middleware Pipeline]  │
         │  → Exception Handling  │
         │  → Request Logging     │
         │  → Metrics             │
         │  → Security Headers    │
         │  → Authentication      │
         │                        │
         │  [Controllers → MediatR│
         │   → Handlers → Repos]  │
         └──┬──────────┬──────────┘
            │          │
      ┌─────▼──┐  ┌───▼────────┐
      │ MySQL  │  │  RabbitMQ  │
      │  8.0   │  │   3.12+    │
      └────────┘  └────────────┘
```

## Stack Tecnológica

| Tecnologia                       | Versão  | Propósito                              |
|----------------------------------|---------|----------------------------------------|
| .NET / ASP.NET Core              | 8.0     | Framework da API                       |
| Entity Framework Core            | 8.0.17  | ORM para acesso a dados               |
| Pomelo.EntityFrameworkCore.MySql  | 8.0.3   | Provider MySQL para EF Core           |
| MediatR                          | 12.5.0  | Mediator pattern (CQRS)               |
| FluentValidation                 | 11.11.0 | Validação de requests                 |
| Serilog                          | 4.3.0   | Structured logging                    |
| Swashbuckle                      | 9.0.1   | Documentação Swagger/OpenAPI          |
| RabbitMQ.Client                  | 7.1.2   | Mensageria assíncrona                 |
| JWT Bearer Authentication        | 8.0.0   | Autenticação via tokens               |
| iTextSharp                       | 3.4.22  | Geração de relatórios PDF             |
| EPPlus                           | 7.5.1   | Geração de relatórios Excel           |
| Asp.Versioning                   | 8.0.0   | Versionamento de API                  |
| Polly                            | 8.6.1   | Resiliência (retry, circuit breaker)  |
| Health Checks (UI + MySql + RMQ) | 8.0.x   | Monitoramento de saúde                |
| WinForms                         | 8.0     | Interface desktop do PDV              |
| FontAwesome.Sharp                | —       | Ícones na interface desktop           |

## Quick Start

### Pré-Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0](https://dev.mysql.com/downloads/)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (opcional para desenvolvimento)

### Instalação

```powershell
# 1. Clone o repositório
git clone https://github.com/HEITORHOG1/Sis-Pdv-Controle-Estoque.git
cd Sis-Pdv-Controle-Estoque

# 2. Restaure as dependências
dotnet restore

# 3. Configure as credenciais do banco (User Secrets)
cd Sis-Pdv-Controle-Estoque-API
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha_mysql"
dotnet user-secrets set "Authentication:JwtSecret" "chave_jwt_com_pelo_menos_32_caracteres"
cd ..

# 4. Compile
dotnet build

# 5. Execute a API (migrações são aplicadas automaticamente)
dotnet run --project Sis-Pdv-Controle-Estoque-API
```

A API estará disponível em:
- **API:** `http://localhost:7003`
- **Swagger UI:** `http://localhost:7003/api-docs`
- **Health Check:** `http://localhost:7003/health`
- **Health Dashboard:** `http://localhost:7003/health-ui`

### Docker

```bash
cp .env.example .env
# Edite o .env com suas credenciais
docker-compose up -d
```

## Banco de Dados

### Schema Principal

O banco é gerenciado pelo Entity Framework Core com migrações automáticas no startup. As principais entidades:

**Negócio:** `Produto`, `Categoria`, `Departamento`, `Fornecedor`, `Cliente`, `Colaborador`, `Pedido`, `ProdutoPedido`, `StockMovement`, `Cupom`

**Pagamento:** `Payment`, `PaymentItem`, `PaymentAudit`, `FiscalReceipt`

**Segurança:** `Usuario`, `Role`, `Permission`, `UserRole`, `RolePermission`, `UserSession`, `AuditLog`

Todas herdam de `EntityBase` com: `Id (Guid)`, `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`, `IsDeleted`, `DeletedAt`, `DeletedBy`.

### Connection String

Configure em `appsettings.json` (ou via User Secrets / variáveis de ambiente):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDV_02;Uid=root;Pwd=SUA_SENHA;..."
  }
}
```

## Diagramas de Classe

Na pasta `Sis-Pdv-Controle-Estoque/Diagrams/`:
- **PDV.cd** — entidades do ponto de venda
- **SisPdv.cd** — relacionamentos entre camadas

Abra no Visual Studio para visualização interativa.

## Documentação

| Documento | Conteúdo |
|-----------|----------|
| [Architecture.md](docs/Architecture.md) | Decisões arquiteturais (ADRs), diagramas, padrões de design |
| [Database-Schema.md](docs/Database-Schema.md) | Schema completo do banco, todas as tabelas e relacionamentos |
| [Development-Guide.md](docs/Development-Guide.md) | Setup do ambiente, padrões de código, como adicionar features |
| [Configuration-Management.md](docs/Configuration-Management.md) | Todas as seções de configuração detalhadas |
| [Deployment-Guide.md](docs/Deployment-Guide.md) | Deploy via Docker e manual |
| [System-Administration-Guide.md](docs/System-Administration-Guide.md) | Operação, monitoramento, backup e troubleshooting |
| [User-Secrets-Configuration.md](docs/User-Secrets-Configuration.md) | Configuração de segredos para desenvolvimento |
| [CLAUDE.md](CLAUDE.md) | Regras do projeto para IA assistentes |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Guia de contribuição e padrões de commit |

### Regras para IA (`.claude/rules/`)

| Arquivo | Escopo |
|---------|--------|
| [api-conventions.md](.claude/rules/api-conventions.md) | Padrões de controllers, MediatR e responses |
| [security.md](.claude/rules/security.md) | Segredos, validação, auth, CORS, headers |
| [observability.md](.claude/rules/observability.md) | Serilog, health checks, métricas |
| [performance.md](.claude/rules/performance.md) | Async, EF Core, pooling, resiliência |
| [testing.md](.claude/rules/testing.md) | Estrutura, convenções, frameworks |

## Testes

```powershell
# Executar todos os testes
dotnet test --logger "console;verbosity=detailed"

# Ou com o script dedicado
.\Sis-Pdv-Controle-Estoque.Tests\run-tests.ps1
```

## Licença

Este projeto está licenciado sob a licença MIT — veja o arquivo [LICENSE.txt](LICENSE.txt).

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
