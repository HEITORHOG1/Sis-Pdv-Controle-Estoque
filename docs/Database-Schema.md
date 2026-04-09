# Schema do Banco de Dados

## Visão Geral

O sistema utiliza **MySQL 8.0** com **Entity Framework Core 8.0** (provider Pomelo) em abordagem **Code-First**. As migrações são aplicadas automaticamente no startup da aplicação.

- **Provider:** `Pomelo.EntityFrameworkCore.MySql 8.0.3`
- **DbContext:** `PdvContext` (registrado no `Setup.cs`)
- **Migrações:** pasta `Sis-Pdv-Controle-Estoque-Infra/Migrations/`

## Entidade Base (Auditoria)

Todas as entidades herdam de `EntityBase`, que fornece:

```
┌─────────────────────────────────────────────────────────┐
│                      EntityBase                         │
├─────────────────────────────────────────────────────────┤
│ Id              : CHAR(36) PRIMARY KEY  ← Guid (UUID)  │
│ CreatedAt       : DATETIME NOT NULL     ← UTC           │
│ UpdatedAt       : DATETIME NULL                         │
│ CreatedBy       : CHAR(36) NULL         ← FK → Usuario │
│ UpdatedBy       : CHAR(36) NULL         ← FK → Usuario │
│ IsDeleted       : BIT NOT NULL DEFAULT 0 ← Soft delete  │
│ DeletedAt       : DATETIME NULL                         │
│ DeletedBy       : CHAR(36) NULL         ← FK → Usuario │
└─────────────────────────────────────────────────────────┘
```

O `AuditInterceptor` preenche esses campos automaticamente em cada `SaveChanges`.

## Tabelas de Negócio

### Produto

```
┌──────────────────────────────────────────────────────────────┐
│                          Produto                             │
├──────────────────────────────────────────────────────────────┤
│ + EntityBase (Id, CreatedAt, UpdatedAt, IsDeleted, etc.)     │
│ CodBarras                : VARCHAR(255) NOT NULL             │
│ NomeProduto              : VARCHAR(255) NOT NULL (min 3)     │
│ DescricaoProduto         : TEXT                              │
│ PrecoCusto               : DECIMAL NOT NULL (>= 0)          │
│ PrecoVenda               : DECIMAL NOT NULL (> 0)            │
│ MargemLucro              : DECIMAL NOT NULL                  │
│ DataFabricao             : DATETIME NOT NULL                 │
│ DataVencimento           : DATETIME NOT NULL (> DataFabricao)│
│ QuatidadeEstoqueProduto  : INT NOT NULL (>= 0)              │
│ MinimumStock             : DECIMAL (>= 0)                    │
│ MaximumStock             : DECIMAL (>= MinimumStock)         │
│ ReorderPoint             : DECIMAL (>= MinimumStock)         │
│ Location                 : VARCHAR(255) NULL                 │
│ StatusAtivo              : INT                               │
│ FornecedorId             : CHAR(36) FK → Fornecedor.Id       │
│ CategoriaId              : CHAR(36) FK → Categoria.Id        │
├──────────────────────────────────────────────────────────────┤
│ Navegação: Fornecedor, Categoria, StockMovements (1:N)       │
│ Métodos: IsLowStock(), IsOutOfStock(), HasSufficientStock()  │
│          UpdateStock() — cria StockMovement automaticamente  │
└──────────────────────────────────────────────────────────────┘
```

### Categoria

```
┌───────────────────────────────────────┐
│              Categoria                │
├───────────────────────────────────────┤
│ + EntityBase                          │
│ NomeCategoria : VARCHAR(255) NOT NULL │
└───────────────────────────────────────┘
```

### Departamento

```
┌──────────────────────────────────────────┐
│             Departamento                 │
├──────────────────────────────────────────┤
│ + EntityBase                             │
│ NomeDepartamento : VARCHAR(255) NOT NULL │
└──────────────────────────────────────────┘
```

### Fornecedor

```
┌──────────────────────────────────────────┐
│              Fornecedor                  │
├──────────────────────────────────────────┤
│ + EntityBase                             │
│ InscricaoEstadual : VARCHAR(255)         │
│ NomeFantasia      : VARCHAR(255)         │
│ Cnpj              : VARCHAR(18)          │
│ Rua               : VARCHAR(255)         │
│ Numero            : VARCHAR(20)          │
│ Complemento       : VARCHAR(255)         │
│ Bairro            : VARCHAR(255)         │
│ Cidade            : VARCHAR(255)         │
│ Uf                : VARCHAR(2)           │
│ CepFornecedor     : INT                  │
│ StatusAtivo       : INT                  │
└──────────────────────────────────────────┘
```

### Cliente

```
┌──────────────────────────────────────────────────────────┐
│                      Cliente                             │
├──────────────────────────────────────────────────────────┤
│ + EntityBase                                             │
│ CpfCnpj      : VARCHAR(18) NOT NULL (11 ou 14 dígitos)  │
│ TipoCliente   : VARCHAR(50) NOT NULL                    │
├──────────────────────────────────────────────────────────┤
│ Validação: CPF = 11 dígitos, CNPJ = 14 dígitos          │
└──────────────────────────────────────────────────────────┘
```

### Colaborador

```
┌───────────────────────────────────────────────┐
│                 Colaborador                   │
├───────────────────────────────────────────────┤
│ + EntityBase                                  │
│ NomeColaborador           : VARCHAR(255)      │
│ CpfColaborador            : VARCHAR(14)       │
│ CargoColaborador          : VARCHAR(255)      │
│ TelefoneColaborador       : VARCHAR(20)       │
│ EmailPessoalColaborador   : VARCHAR(255)      │
│ EmailCorporativo          : VARCHAR(255)      │
│ DepartamentoId            : CHAR(36) FK NULL  │
├───────────────────────────────────────────────┤
│ Navegação: Usuario (1:1), Departamento (N:1)  │
└───────────────────────────────────────────────┘
```

### Pedido

```
┌───────────────────────────────────────────────┐
│                    Pedido                     │
├───────────────────────────────────────────────┤
│ + EntityBase                                  │
│ ColaboradorId    : CHAR(36) FK NULL           │
│ ClienteId        : CHAR(36) FK NULL           │
│ Status           : INT                        │
│ DataDoPedido     : DATETIME NULL              │
│ FormaPagamento   : VARCHAR(50)                │
│ TotalPedido      : DECIMAL                    │
├───────────────────────────────────────────────┤
│ Navegação: Colaborador (N:1), Cliente (N:1)   │
└───────────────────────────────────────────────┘
```

### ProdutoPedido (Itens do Pedido)

```
┌───────────────────────────────────────────────────┐
│                  ProdutoPedido                    │
├───────────────────────────────────────────────────┤
│ + EntityBase                                      │
│ PedidoId               : CHAR(36) FK → Pedido.Id  │
│ ProdutoId              : CHAR(36) FK → Produto.Id │
│ CodBarras              : VARCHAR(255) NULL        │
│ QuantidadeItemPedido   : INT NULL                 │
│ TotalProdutoPedido     : DECIMAL NULL             │
├───────────────────────────────────────────────────┤
│ Navegação: Pedido (N:1), Produto (N:1)            │
└───────────────────────────────────────────────────┘
```

### StockMovement (Movimentação de Estoque)

```
┌─────────────────────────────────────────────────────────┐
│                    StockMovement                        │
├─────────────────────────────────────────────────────────┤
│ + EntityBase                                            │
│ ProdutoId          : CHAR(36) FK → Produto.Id           │
│ Quantity           : DECIMAL NOT NULL                   │
│ Type               : INT NOT NULL (StockMovementType)   │
│ Reason             : VARCHAR(500) NOT NULL              │
│ UnitCost           : DECIMAL                            │
│ PreviousStock      : DECIMAL                            │
│ NewStock           : DECIMAL                            │
│ MovementDate       : DATETIME (UTC)                     │
│ ReferenceDocument  : VARCHAR(255) NULL                  │
│ UserId             : CHAR(36) NULL                      │
├─────────────────────────────────────────────────────────┤
│ StockMovementType enum:                                 │
│   Entry(1), Exit(2), Adjustment(3), Sale(4),            │
│   Return(5), Transfer(6), Loss(7)                       │
└─────────────────────────────────────────────────────────┘
```

## Tabelas de Pagamento

### Payment

```
┌─────────────────────────────────────────────────────┐
│                      Payment                        │
├─────────────────────────────────────────────────────┤
│ + EntityBase                                        │
│ OrderId            : CHAR(36) FK → Pedido.Id        │
│ TotalAmount        : DECIMAL NOT NULL               │
│ Status             : INT (PaymentStatus)            │
│ PaymentDate        : DATETIME                       │
│ TransactionId      : VARCHAR(255) NULL              │
│ AuthorizationCode  : VARCHAR(255) NULL              │
│ ProcessorResponse  : TEXT NULL                      │
│ ErrorMessage       : TEXT NULL                      │
│ ProcessedAt        : DATETIME NULL                  │
├─────────────────────────────────────────────────────┤
│ PaymentStatus enum:                                 │
│   Pending(0), Processing(1), Processed(2),          │
│   Failed(3), Cancelled(4), Refunded(5)              │
│                                                     │
│ Navegação: Order (Pedido), PaymentItems, FiscalReceipt │
│ Métodos: ProcessPayment(), FailPayment(), CancelPayment() │
└─────────────────────────────────────────────────────┘
```

### PaymentItem

```
┌───────────────────────────────────────────┐
│               PaymentItem                 │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ PaymentId      : CHAR(36) FK → Payment.Id │
│ (detalhes do item de pagamento)           │
└───────────────────────────────────────────┘
```

### PaymentAudit

```
┌───────────────────────────────────────────┐
│             PaymentAudit                  │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ (auditoria detalhada de pagamentos)       │
└───────────────────────────────────────────┘
```

### FiscalReceipt (Nota Fiscal / NFC-e)

```
┌────────────────────────────────────────────────────────┐
│                   FiscalReceipt                        │
├────────────────────────────────────────────────────────┤
│ + EntityBase                                           │
│ PaymentId          : CHAR(36) FK → Payment.Id          │
│ ReceiptNumber      : VARCHAR(255) NOT NULL             │
│ SerialNumber       : VARCHAR(255) NOT NULL             │
│ IssuedAt           : DATETIME NOT NULL                 │
│ Status             : INT (FiscalReceiptStatus)         │
│ SefazProtocol      : VARCHAR(255) NULL                 │
│ AccessKey          : VARCHAR(44) NULL  ← Chave NFC-e   │
│ QrCode             : TEXT NULL                         │
│ XmlContent         : TEXT NULL         ← XML SEFAZ     │
│ ErrorMessage       : TEXT NULL                         │
│ SentToSefazAt      : DATETIME NULL                     │
│ AuthorizedAt       : DATETIME NULL                     │
│ CancellationReason : TEXT NULL                         │
│ CancelledAt        : DATETIME NULL                     │
├────────────────────────────────────────────────────────┤
│ FiscalReceiptStatus enum:                              │
│   Pending(0), Sent(1), Authorized(2),                  │
│   Rejected(3), Cancelled(4)                            │
│                                                        │
│ Métodos: Authorize(), Reject(), Cancel()               │
└────────────────────────────────────────────────────────┘
```

## Tabelas de Segurança (RBAC)

### Usuario

```
┌───────────────────────────────────────────┐
│                 Usuario                   │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ Login                  : VARCHAR(255)     │
│ Senha                  : VARCHAR(255)     │ ← Hash (PasswordService)
│ Email                  : VARCHAR(255) NULL│
│ Nome                   : VARCHAR(255) NULL│
│ StatusAtivo            : BIT DEFAULT TRUE │
│ LastLoginAt            : DATETIME NULL    │
│ RefreshToken           : VARCHAR(500) NULL│
│ RefreshTokenExpiryTime : DATETIME NULL    │
├───────────────────────────────────────────┤
│ Navegação: UserRoles (1:N)                │
└───────────────────────────────────────────┘
```

### Role

```
┌───────────────────────────────────────────┐
│                   Role                    │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ Name        : VARCHAR(255) NOT NULL       │
│ Description : VARCHAR(500)                │
│ IsActive    : BIT DEFAULT TRUE            │
├───────────────────────────────────────────┤
│ Navegação: RolePermissions, UserRoles     │
│                                           │
│ Roles pré-definidos:                      │
│   SuperAdmin, Admin, Manager,             │
│   Cashier, Viewer                         │
└───────────────────────────────────────────┘
```

### Permission

```
┌───────────────────────────────────────────┐
│                Permission                 │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ Name        : VARCHAR(255) NOT NULL       │
│ Description : VARCHAR(500)                │
└───────────────────────────────────────────┘
```

### Tabelas de Relacionamento

```
UserRole           : UserId (FK → Usuario) + RoleId (FK → Role)
RolePermission     : RoleId (FK → Role) + PermissionId (FK → Permission)
```

### UserSession

```
┌───────────────────────────────────────────┐
│               UserSession                 │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ UserId     : CHAR(36) FK → Usuario.Id     │
│ CreatedAt  : DATETIME                     │
│ ExpiresAt  : DATETIME                     │
│ IsActive   : BIT                          │
└───────────────────────────────────────────┘
```

### AuditLog

```
┌───────────────────────────────────────────┐
│                AuditLog                   │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ UserId    : CHAR(36) NULL                 │
│ Action    : VARCHAR(255)                  │
│ Details   : TEXT                          │
│ Timestamp : DATETIME                      │
│ IpAddress : VARCHAR(45) NULL              │
└───────────────────────────────────────────┘
```

## Outras Tabelas

### Cupom

```
┌───────────────────────────────────────────┐
│                  Cupom                    │
├───────────────────────────────────────────┤
│ + EntityBase                              │
│ (dados do cupom fiscal/desconto)          │
└───────────────────────────────────────────┘
```

## Migrações

| Data       | Nome                               | Descrição                        |
|------------|-------------------------------------|----------------------------------|
| 2025-08-16 | `InitialCreate`                     | Criação de todas as tabelas      |
| 2025-08-17 | `UpdateCategoriaIsDeletedDefault`   | Default para IsDeleted           |
| 2025-08-17 | `ApplyCategoriaDefaultValue`        | Aplicar valor padrão existente   |

Para criar nova migração:
```powershell
dotnet ef migrations add NomeDaMigracao `
  --project Sis-Pdv-Controle-Estoque-Infra `
  --startup-project Sis-Pdv-Controle-Estoque-API
```

## Queries Úteis

```sql
-- Verificar tamanho das tabelas
SELECT TABLE_NAME, TABLE_ROWS,
       ROUND((DATA_LENGTH + INDEX_LENGTH) / 1024 / 1024, 2) AS 'Tamanho (MB)'
FROM information_schema.TABLES
WHERE TABLE_SCHEMA = DATABASE()
ORDER BY (DATA_LENGTH + INDEX_LENGTH) DESC;

-- Produtos com estoque baixo
SELECT NomeProduto, QuatidadeEstoqueProduto, ReorderPoint, MinimumStock
FROM Produto
WHERE QuatidadeEstoqueProduto <= ReorderPoint AND IsDeleted = 0;

-- Movimentações de estoque recentes
SELECT p.NomeProduto, sm.Type, sm.Quantity, sm.Reason, sm.MovementDate
FROM StockMovement sm
JOIN Produto p ON sm.ProdutoId = p.Id
ORDER BY sm.MovementDate DESC
LIMIT 20;
```

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
