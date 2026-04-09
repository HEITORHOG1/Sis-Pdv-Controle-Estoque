# Arquitetura e Decisões de Design

## Visão Geral

O Sis-Pdv-Controle-Estoque é um sistema de Ponto de Venda (PDV) e controle de estoque que segue os princípios de **Domain-Driven Design (DDD)** com **CQRS** implementado via MediatR. A solução é dividida em 6 projetos com responsabilidades bem definidas.

## Diagrama de Dependências entre Projetos

```
                    ┌─────────────────┐
                    │   MessageBus    │  (Biblioteca independente)
                    └────────┬────────┘
                             │
            ┌────────────────▼─────────────────┐
            │         Domain                   │
            │  (Sis-Pdv-Controle-Estoque)      │
            │                                  │
            │  • Entidades (Model/)            │
            │  • Commands/Handlers (CQRS)      │
            │  • Interfaces de Repositório     │
            │  • Interfaces de Serviço         │
            │  • Validadores de Domínio        │
            │  • Exceções de Domínio           │
            └──────┬────────────────┬──────────┘
                   │                │
         ┌─────────▼──────┐   ┌────▼─────────────────────┐
         │ Infrastructure │   │          API              │
         │   (Infra)      │   │ (Sis-Pdv-...-API)        │
         │                │   │                           │
         │ • Repositórios │   │ • Controllers (20)        │
         │ • DbContext    │◄──│ • Middleware (5)           │
         │ • Migrações    │   │ • Configuração (16)       │
         │ • Mappings     │   │ • Serviços de Aplicação   │
         └────────────────┘   │ • Setup.cs (DI)           │
                              └────────┬──────────────────┘
                                       │
                              ┌────────▼──────────────────┐
                              │    Desktop (Form)         │
                              │ (Sis-Pdv-...-Form)        │
                              │                           │
                              │ • WinForms (.NET 8.0)     │
                              │ • Telas de PDV            │
                              │ • Polly (resiliência)     │
                              └───────────────────────────┘
```

> **⚠️ Nota:** O projeto Form atualmente referencia API, Infra e Domain diretamente. O ideal seria comunicar exclusivamente via HTTP com a API. Este é um ponto de melhoria futura.

## Decisões Arquiteturais (ADR)

### ADR-001: CQRS com MediatR

**Contexto:** Necessidade de separar operações de leitura e escrita para melhor organização e testabilidade.

**Decisão:** Usar o padrão CQRS implementado com a biblioteca **MediatR 12.5.0**.

**Estrutura:**
```
Commands/
├── Produto/
│   ├── AdicionarProduto/
│   │   ├── AdicionarProdutoRequest.cs      ← IRequest<Response>
│   │   └── AdicionarProdutoHandler.cs      ← IRequestHandler<...>
│   ├── AlterarProduto/
│   ├── RemoverProduto/
│   └── ListarProdutoPorNomeProduto/
├── Pedidos/
├── Cliente/
├── Payment/
└── Usuarios/
```

**Consequências:**
- ✅ Cada operação é isolada em seu próprio handler
- ✅ Facilita testes unitários (cada handler testável independentemente)
- ✅ Pipeline de comportamentos (ValidationBehavior)
- ⚠️ Overhead de classes para operações simples (muitos arquivos)

### ADR-002: Entity Framework Core (não Dapper)

**Contexto:** Necessidade de um ORM para acesso a dados. O CLAUDE.md do projeto menciona Dapper como preferência, mas o código real usa EF Core.

**Decisão:** Entity Framework Core 8.0 com Pomelo.EntityFrameworkCore.MySql como provider.

**Razões:**
- Migrações automáticas para evolução do schema
- Code-First com Fluent API para mapeamentos
- Interceptors para auditoria automática (`AuditInterceptor`)
- Lazy Loading via virtual navigation properties
- InMemory provider para testes de integração

**Consequências:**
- ✅ Migrações gerenciadas automaticamente
- ✅ Auditoria transparente via interceptors
- ❌ Domínio acoplado ao EF Core (handlers usam `.Include()`)
- ❌ Pode ter problemas de performance com lazy loading

### ADR-003: Soft Delete com Auditoria

**Contexto:** Dados não devem ser excluídos fisicamente do banco para fins de auditoria e possível recuperação.

**Decisão:** Todas as entidades herdam de `EntityBase` com campos de auditoria.

**Campos:**
```csharp
public abstract class EntityBase : IAuditableEntity
{
    public Guid Id { get; set; }          // UUID v4
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }     // ← Soft delete flag
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
```

O `AuditInterceptor` preenche automaticamente `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`, `DeletedAt`, `DeletedBy` via `SaveChangesInterceptor`.

### ADR-004: Autenticação JWT com RBAC

**Contexto:** API REST stateless requer autenticação e autorização.

**Decisão:** JWT Bearer Authentication com Role-Based Access Control (RBAC).

**Modelo de dados:**
```
Usuario ─── (N:N) ──── Role ─── (N:N) ──── Permission
         via UserRole       via RolePermission
```

**5 roles pré-definidos:** SuperAdmin, Admin, Manager, Cashier, Viewer

**Authorization Policies:** Baseadas em claims de permissão:
```csharp
options.AddPolicy("RequireUserManagementPermission",
    policy => policy.RequireClaim("permission", "user.manage"));
```

### ADR-005: Validação em Duas Camadas

**Contexto:** Validação deve ocorrer antes de qualquer lógica de negócio.

**Decisão:** Validação em dois níveis:

1. **FluentValidation** — no pipeline MediatR via `ValidationBehavior`
   - Valida requests antes de chegar ao handler
   - Retorna erros de validação estruturados
   
2. **Validação de Domínio** — nos construtores e métodos das entidades
   - Lança `DomainException` para regras de negócio violadas
   - Validadores específicos: `CpfCnpjValidator`, `BarcodeValidator`, `BusinessValidator`

### ADR-006: Mensageria com RabbitMQ

**Contexto:** Operações assíncronas (envio de NFC-e para SEFAZ) não devem bloquear o request.

**Decisão:** RabbitMQ para processamento assíncrono.

**Uso atual:** Envio de cupons fiscais para processamento pela SEFAZ. A biblioteca `MessageBus` define a abstração `IMessageBus` e `BaseMessage`.

### ADR-007: Multi-Target (API + Desktop)

**Contexto:** O sistema PDV precisa funcionar tanto como serviço web quanto como aplicação desktop.

**Decisão:** Dois projetos "cliente":
- **API** — ASP.NET Core Web API para integrações e frontend web
- **Form** — WinForms (.NET 8.0-windows) para PDV frente de loja

O Form utiliza Polly para resiliência na comunicação com a API.

## Schema do Banco de Dados

### Diagrama ER (Simplificado)

```
┌──────────────┐     ┌──────────────┐     ┌───────────────┐
│  Departamento │     │  Categoria   │     │  Fornecedor   │
│──────────────│     │──────────────│     │───────────────│
│ Nome         │     │ NomeCategoria│     │ NomeFantasia  │
└──────┬───────┘     └──────┬───────┘     │ CNPJ          │
       │                    │             │ Endereço...   │
       │                    │             └──────┬────────┘
       │                    │                    │
┌──────▼───────┐     ┌──────▼────────────────────▼─────────┐
│ Colaborador  │     │              Produto                │
│──────────────│     │─────────────────────────────────────│
│ Nome         │     │ CodBarras, NomeProduto              │
│ CPF          │     │ PrecoCusto, PrecoVenda, MargemLucro │
│ Cargo        │     │ QuantidadeEstoque                   │
│ Email        │     │ MinimumStock, MaximumStock           │
│ Departamento ├─┐   │ ReorderPoint, Location              │
│ Usuario      │ │   │ FornecedorId, CategoriaId           │
└──────────────┘ │   │ DataFabricao, DataVencimento        │
                 │   └──────────┬──────────────────────────┘
                 │              │
                 │   ┌──────────▼───────────┐
                 │   │   StockMovement      │
                 │   │─────────────────────│
                 │   │ Quantity, Type       │
                 │   │ Reason, UnitCost     │
                 │   │ PreviousStock        │
                 │   │ NewStock, UserId     │
                 │   └─────────────────────┘
                 │
        ┌────────▼─────────────────────────────────────┐
        │                   Pedido                     │
        │──────────────────────────────────────────────│
        │ ColaboradorId, ClienteId                     │
        │ Status, DataDoPedido                         │
        │ FormaPagamento, TotalPedido                  │
        └──────────┬────────────────┬──────────────────┘
                   │                │
        ┌──────────▼──────┐  ┌─────▼──────────────┐
        │ ProdutoPedido   │  │     Payment         │
        │─────────────────│  │────────────────────│
        │ PedidoId        │  │ OrderId             │
        │ ProdutoId       │  │ TotalAmount          │
        │ CodBarras       │  │ Status (enum)       │
        │ Quantidade      │  │ TransactionId       │
        │ TotalProduto    │  │ AuthorizationCode   │
        └─────────────────┘  └─────────┬──────────┘
                                       │
                              ┌────────▼──────────┐
                              │  FiscalReceipt    │
                              │──────────────────│
                              │ ReceiptNumber     │
                              │ SefazProtocol     │
                              │ AccessKey         │
                              │ QrCode            │
                              │ XmlContent        │
                              │ Status (enum)     │
                              └───────────────────┘

┌──────────────────────────────────────────────────────────────┐
│                     Segurança (RBAC)                        │
│                                                              │
│  Usuario ──(N:N)── Role ──(N:N)── Permission                │
│           UserRole       RolePermission                      │
│                                                              │
│  UserSession (sessões ativas)                                │
│  AuditLog (log de ações)                                     │
│  PaymentAudit (auditoria de pagamentos)                      │
└──────────────────────────────────────────────────────────────┘
```

### Enums do Sistema

| Enum                  | Valores                                                    |
|-----------------------|------------------------------------------------------------|
| `PaymentStatus`       | Pending(0), Processing(1), Processed(2), Failed(3), Cancelled(4), Refunded(5) |
| `FiscalReceiptStatus` | Pending(0), Sent(1), Authorized(2), Rejected(3), Cancelled(4) |
| `StockMovementType`   | Entry(1), Exit(2), Adjustment(3), Sale(4), Return(5), Transfer(6), Loss(7) |

## Pipeline de Middleware

A ordem dos middlewares no `Program.cs` é crítica:

```
Request →
  1. GlobalExceptionMiddleware      ← Captura toda exceção não tratada
  2. RequestLoggingMiddleware       ← Loga início/fim + duração de cada request
  3. MetricsMiddleware              ← Coleta métricas (status code, latência)
  4. SecurityMiddleware (composite) ← HTTPS redirect, CORS, Rate Limiting, Security Headers
  5. StaticFiles                    ← Arquivos estáticos (wwwroot)
  6. AuthenticationMiddleware       ← Validação JWT customizada
  7. Health Check Endpoints         ← /health, /health-ui
  8. Swagger                        ← /api-docs
  9. Controllers                    ← Lógica de negócio
→ Response
```

## Áreas de Melhoria Identificadas

| Prioridade | Área | Descrição |
|:----------:|------|-----------|
| 🔴 Alta    | Domain ↔ EF Core | Remover dependência do EF Core no Domain. Handlers devem usar apenas interfaces. |
| 🔴 Alta    | Form ↔ API | Form deveria comunicar via HTTP, não referenciar projetos internos. |
| 🟡 Média   | CancellationToken | Nem todos os handlers propagam `CancellationToken`. Verificar e corrigir. |
| 🟡 Média   | Testes | Cobertura de testes é baixa. Poucos testes reais implementados. |
| 🟢 Baixa   | Naming | Mix de português e inglês nos nomes de propriedades e métodos. |
| 🟢 Baixa   | DTOs | `CupomDTO.cs` está no Domain. DTOs pertencem à camada de API. |

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
