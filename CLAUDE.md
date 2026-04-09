# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

Sistema de Ponto de Venda (PDV) e Controle de Estoque — .NET 8.0 backend API, Blazor Server PDV front-end, WinForms desktop app, and Brazilian fiscal integration (NFC-e/SEFAZ).

## Commands

```powershell
# Build entire solution
dotnet build

# Run all tests
dotnet test --logger "console;verbosity=detailed"

# Run a single test by name
dotnet test --filter "Handle_ValidRequest_ShouldCreateCliente" --logger "console;verbosity=detailed"

# Run tests in a specific class
dotnet test --filter "FullyQualifiedName~CategoriaTests" --logger "console;verbosity=detailed"

# Run the API (port 7003)
dotnet run --project Sis-Pdv-Controle-Estoque-API

# Run the Blazor PDV
dotnet run --project Sis.Pdv.Blazor

# Run with Docker (API on 8080, MySQL 3306, RabbitMQ 5672/15672)
docker-compose up -d

# Create EF Core migration
dotnet ef migrations add NomeMigracao --project Sis-Pdv-Controle-Estoque-Infra --startup-project Sis-Pdv-Controle-Estoque-API

# Format code
dotnet format
```

## Architecture

Three-layer architecture: **Domain** → **Infrastructure** → **API/UI**. Do not introduce new layers.

```text
Sis-Pdv-Controle-Estoque/          → Domain (models, commands/handlers, interfaces, validators)
Sis-Pdv-Controle-Estoque-Infra/    → Infrastructure (EF Core repos, migrations, mappings, interceptors)
Sis-Pdv-Controle-Estoque-API/      → API (controllers, middleware, configuration, services)
Sis.Pdv.Blazor/                    → Blazor Server PDV (offline-first, local MySQL, RabbitMQ sync)
Sis-Pdv-Controle-Estoque-Form/     → WinForms desktop PDV
MessageBus/                        → RabbitMQ messaging library
Sis-Pdv-Controle-Estoque.Tests/    → xUnit tests (Moq, FluentAssertions, AutoFixture)
```

### Request Pipeline (API)

1. `GlobalExceptionMiddleware` — catches all unhandled exceptions, maps to HTTP status codes
2. `RequestLoggingMiddleware` — structured request logging with Serilog
3. `MetricsMiddleware` — collects response times and status codes
4. Security middleware (CORS, headers, rate limiting, HTTPS redirect)
5. JWT Authentication + custom `AuthenticationMiddleware`
6. Controller → MediatR → `ValidationBehavior` (FluentValidation) → Handler

### CQRS Flow

Controllers inject `IMediator` and delegate everything. No business logic in controllers.

- Commands/Queries: `Sis-Pdv-Controle-Estoque/Commands/{Entity}/{Operation}/`
- Each operation has: `{Operation}Request.cs` (implements `IRequest<Response>`) + `{Operation}Handler.cs`
- Validators: `{Operation}RequestValidator.cs` in the same folder — auto-discovered by `ValidationBehavior`
- All handlers return `Response` (from prmToolkit.NotificationPattern)

### DI Registration

- **`Setup.cs`** — repositories (transient), services (scoped), MediatR, validators
- **`Program.cs`** — middleware pipeline, Serilog, health checks, Swagger
- **`Configuration/`** — security, health checks, Swagger, environment config (IOptions pattern with validators)

### Entity & Repository Patterns

- All entities inherit `EntityBase` (Id, CreatedAt, UpdatedAt, IsDeleted, audit fields)
- `AuditInterceptor` auto-populates audit fields on save
- Soft delete by default (`IsDeleted` flag); hard delete via `RemoverFisicamente()`
- `RepositoryBase<T>` provides CRUD, pagination, includes, soft-delete filtering
- `IUnitOfWork` wraps transactions with deadlock detection

### Database

- MySQL 8.0 via Pomelo EF Core provider
- Connection string key: `"ControleFluxoCaixaConnectionString"`
- Migrations auto-applied on startup (`MigrateAsync()`)
- Entity mappings in `Sis-Pdv-Controle-Estoque-Infra/Mappings/` (IEntityTypeConfiguration)
- DbContext: `PdvContext` with 45+ DbSets

### Blazor PDV (Sis.Pdv.Blazor)

Offline-first PDV that operates autonomously with its own local MySQL database.

- **MVVM pattern**: ViewModels (`ViewModelBase` + `INotifyPropertyChanged`) bind to Razor components
- **Local data**: `PdvDbContext` (separate from API's `PdvContext`) with `ProdutoEntity`, `VendaEntity`, `ItemVendaEntity`, `UsuarioLocalEntity`
- **Sync down**: MassTransit consumers receive `ProdutoAlteradoEvent` etc. from RabbitMQ to update local DB
- **Sync up**: `SalesUploaderWorker` background service pushes offline sales to the API when connected
- **DI extensions**: `ServiceCollectionExtensions.cs` — `AddPdvData()`, `AddPdvServices()`, `AddPdvViewModels()`, `AddPdvMessaging()`
- **UI**: Radzen Blazor components

### Key Entities

- `Produto` — barcode, prices, stock, supplier, category
- `Pedido` / `ProdutoPedido` — sales orders with line items
- `Payment` / `FiscalReceipt` — payment processing + NFC-e fiscal documents
- `StockMovement` — inventory tracking with audit trail
- `Usuario` → `UserRole` → `Role` → `RolePermission` → `Permission` — RBAC chain

## Conventions

Detailed rules are in `.claude/rules/` (auto-loaded). Key points not covered there:

- Connection string comes from User Secrets (dev) or environment variables (prod) — never hardcoded
- Authorization policies use claim-based permissions: `user.manage`, `role.manage`, `inventory.manage`, etc.
- Health checks at `/health` (liveness) and `/health-ui` (dashboard)
- Swagger at `/api-docs`

## Known Technical Debt

The Domain project references `Microsoft.EntityFrameworkCore` because handlers use `.Include()` directly. Handlers should use repository interfaces only, with `.Include()` encapsulated in repositories.
