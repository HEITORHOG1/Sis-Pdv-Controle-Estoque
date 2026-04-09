# 🟡 Análise do Projeto de Testes (`Sis-Pdv-Controle-Estoque.Tests`)

> Severidade geral: **MÉDIA** — Infraestrutura de testes existe, mas cobertura é insuficiente.

## 1. Poucos Testes Reais Implementados (🔴 CRÍTICA)

**Problema:** O projeto tem infraestrutura robusta (TestFixture, WebApplicationTestFixture, IntegrationTestBase), mas poucos testes reais:

```
Sis-Pdv-Controle-Estoque.Tests/
├── Infrastructure/                    ← 4 arquivos de setup
│   ├── TestFixture.cs
│   ├── IntegrationTestBase.cs
│   ├── TestStartup.cs
│   └── WebApplicationTestFixture.cs
├── UnitTests/                         ← Poucos testes
│   ├── ModelTests.cs
│   └── SimpleTests.cs
└── IntegrationTests/                  ← Poucos testes
    └── BasicIntegrationTests.cs
```

**Análise:**
- A infraestrutura é 4x maior que os testes reais
- Não há testes para os 71 handlers
- Não há testes para as validações (FluentValidation)
- Não há testes para os serviços de pagamento, fiscal, estoque

**O que deveria existir:**
```
UnitTests/
├── Handlers/
│   ├── Produto/
│   │   ├── AdicionarProdutoHandlerTests.cs
│   │   ├── AlterarProdutoHandlerTests.cs
│   │   └── RemoverProdutoHandlerTests.cs
│   ├── Pedidos/
│   ├── Payment/
│   └── Inventory/
├── Validators/
│   ├── CpfCnpjValidatorTests.cs
│   ├── BarcodeValidatorTests.cs
│   └── AdicionarProdutoValidatorTests.cs
├── Domain/
│   ├── ProdutoTests.cs
│   ├── PedidoTests.cs
│   └── PaymentTests.cs
└── Services/
    ├── PaymentServiceTests.cs
    └── StockValidationServiceTests.cs
```

---

## 2. Testes de Domínio Insuficientes (🟡 MÉDIA)

**Problema:** As entidades do Domain contêm lógica de negócio que não é testada:

```csharp
// Produto.cs — métodos que precisam de testes:
public bool IsLowStock()     // ← Sem teste
public bool IsOutOfStock()   // ← Sem teste  
public bool HasSufficientStock(decimal quantity)  // ← Sem teste
public void UpdateStock(decimal quantity, StockMovementType type, ...)  // ← Sem teste

// Payment.cs
public void ProcessPayment(...)   // ← Sem teste
public void FailPayment(...)      // ← Sem teste
public void CancelPayment(...)    // ← Sem teste

// FiscalReceipt.cs
public void Authorize(...)        // ← Sem teste
public void Reject(...)           // ← Sem teste
public void Cancel(...)           // ← Sem teste
```

**Testes que deveriam existir:**
```csharp
[Fact]
public void IsLowStock_WhenStockBelowMinimum_ReturnsTrue()
{
    var produto = new Produto { QuatidadeEstoqueProduto = 5, MinimumStock = 10 };
    produto.IsLowStock().Should().BeTrue();
}

[Fact]
public void UpdateStock_WithNegativeQuantityForEntry_ThrowsDomainException()
{
    var produto = new Produto { QuatidadeEstoqueProduto = 10 };
    var act = () => produto.UpdateStock(-5, StockMovementType.Entry, "test");
    act.Should().Throw<DomainException>();
}
```

---

## 3. Testes de Validação Ausentes (🟡 MÉDIA)

**Problema:** O projeto tem ~20 validators (FluentValidation), mas nenhum é testado:

```
Validators existentes no Domain:
├── AdicionarCategoriaValidator.cs
├── AlterarCategoriaRequestValidator.cs
├── AdicionarProdutoRequestValidator.cs
├── AlterarProdutoRequestValidator.cs
├── AdicionarPedidoRequestValidator.cs
├── AlterarPedidoRequestValidator.cs
├── AdicionarColaboradorRequestValidator.cs
├── AlterarColaboradorRequestValidator.cs
├── AdicionarFornecedorRequestValidator.cs
├── AlterarFornecedorRequestValidator.cs
├── CpfCnpjValidator.cs
├── BarcodeValidator.cs
└── etc.
```

**Nenhum desses tem teste!**

---

## 4. TestFixture com Registros Incompletos (🟡 MÉDIA)

**Problema:** O `TestFixture` registra serviços no ServiceProvider, mas a lista pode estar desatualizada. Novos repositórios ou serviços adicionados ao projeto não são automaticamente refletidos no TestFixture.

**Correção:** Usar o mesmo `Setup.cs` da API para configurar os testes, substituindo apenas o DbContext.

---

## 5. InMemory Database Não Simula MySQL (🟢 MENOR)

**Problema:** Os testes usam `UseInMemoryDatabase` que:
- Não valida constraints (FK, unique)
- Não suporta transações reais
- Não testa queries específicas do MySQL (como `COLLATE`, tipos de dados)

**Nota:** Para testes unitários, InMemory é aceitável. Para testes de integração, considere usar um container Docker com MySQL de teste.

---

## Resumo

| # | Problema | Severidade | Tipo |
|---|---------|:----------:|------|
| 1 | Poucos testes reais vs infraestrutura | 🔴 | Cobertura |
| 2 | Lógica de domínio sem testes | 🟡 | DDD |
| 3 | Validators sem testes | 🟡 | Validação |
| 4 | TestFixture potencialmente desatualizado | 🟡 | Manutenção |
| 5 | InMemory não simula MySQL | 🟢 | Integração |

### Prioridade de Testes para Implementar

```
ALTA PRIORIDADE (protege regras de negócio):
├── Produto.UpdateStock() — edge cases de estoque
├── Payment.ProcessPayment() — fluxo de pagamento
├── CpfCnpjValidator — validação de documentos
├── BarcodeValidator — validação de código de barras
└── ProcessPaymentHandler — handler mais crítico

MÉDIA PRIORIDADE (protege CRUD):
├── Handlers de Adicionar* (todos)
├── Handlers de Alterar* (todos)
├── Handlers de Remover* (verificar soft delete)
└── Validators de Request (FluentValidation)

BAIXA PRIORIDADE (infraestrutura):
├── Controllers (thin layer)
├── Repository queries simples
└── Middleware tests
```

---

Data da análise: 2026-02-16
