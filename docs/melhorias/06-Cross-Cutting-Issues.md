# 🔴 Problemas Transversais (Cross-Cutting)

> Severidade geral: **CRÍTICA** — Problemas que afetam todos os projetos simultaneamente.

## 1. Mix de Português e Inglês (🟡 MÉDIA)

**Problema:** O codebase inteiro mistura idiomas sem consistência:

| Elemento | Português | Inglês |
|----------|-----------|--------|
| Entidades | `Produto`, `Pedido`, `Categoria`, `Colaborador`, `Fornecedor`, `Departamento`, `Cliente` | `Payment`, `StockMovement`, `FiscalReceipt`, `Role`, `Permission` |
| Métodos | `AlterarProduto()`, `AtualizarDados()`, `Listar()`, `Adicionar()` | `ProcessPayment()`, `UpdateStock()`, `CancelPayment()` |
| Pastas | `Pedidos/`, `Colaborador/` | `Payment/`, `Inventory/` |
| Propriedades | `NomeProduto`, `PrecoVenda`, `DataDoPedido` | `TotalAmount`, `TransactionId`, `MovementDate` |
| Interfaces | `IRepositoryProduto`, `IRepositoryPedido` | `IPaymentService`, `IStockValidationService` |
| Comentários | `//Criar meu objeto de resposta` | `// Validation is now handled by...` |

**Padrão observado:** Entidades "originais" são em português. Entidades adicionadas depois (Payment, StockMovement, FiscalReceipt, Role, Permission) são em inglês. Isso confirma que foram adicionadas em fases diferentes.

**Recomendação:** Manter um idioma só. Se o projeto crescer, migrar tudo para inglês gradualmente.

---

## 2. Ausência de Unit of Work nas Operações de Escrita (🔴 CRÍTICA)

**Problema:** Os handlers que fazem escrita usam diretamente o repositório, sem transação:

```csharp
// AdicionarPedidoHandler.cs
Pedido = _repositoryPedido.Adicionar(Pedido);  // ← Adiciona ao context
// Mas NÃO faz SaveChanges aqui!
// O SaveChanges acontece no ControllerBase.ResponseAsync()!
```

**Fluxo atual (perigoso):**
```
Request → Controller → Mediator → Handler (adiciona ao context) → Controller.ResponseAsync (SaveChanges)
```

**Problemas:**
1. Se o handler adiciona 2 entidades e o SaveChanges falha, ambas são perdidas
2. Não há transação explícita — operações multi-entidade não são atômicas
3. O handler não tem controle sobre quando o save acontece
4. Em cenários complexos (Payment + FiscalReceipt + ProdutoPedido), falta atomicidade

**Correção:** Implementar TransactionBehavior no pipeline MediatR:
```csharp
public class TransactionBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes>
{
    public async Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, CancellationToken ct)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            var response = await next();
            await _context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}
```

---

## 3. `prmToolkit.NotificationPattern` — Biblioteca Obsoleta (🟡 MÉDIA)

**Problema:** O projeto depende de `prmToolkit.NotificationPattern` v1.1.5:

```xml
<PackageReference Include="prmToolkit.NotificationPattern" Version="1.1.5" />
```

**Problemas:**
- Última atualização: anos atrás
- Poucos downloads no NuGet
- Faz handlers herdar de `Notifiable` (acumula estado)
- Não é thread-safe
- Padrão moderno: usar `Result<T>`, `OneOf<T>`, ou `FluentResults`

**Impacto sistêmico:** TODOS os handlers herdam de `Notifiable`:
```csharp
public class AdicionarCategoriaHandler : Notifiable, IRequestHandler<...>
```

**Correção gradual (sem quebrar tudo):**
```csharp
// Fase 1: Criar Result<T> complementar
public record Result<T>(bool Success, T? Data, List<string> Errors);

// Fase 2: Novos handlers usam Result<T>
public class AdicionarCategoriaHandler : IRequestHandler<AdicionarCategoriaRequest, Result<Categoria>>
{
    public async Task<Result<Categoria>> Handle(...)
    {
        if (invalid)
            return Result<Categoria>.Failure("Mensagem clara");
        return Result<Categoria>.Ok(categoria);
    }
}

// Fase 3: Migrar handlers antigos gradualmente
```

---

## 4. Sem Caching (🟡 MÉDIA)

**Problema:** Nenhum mecanismo de cache implementado. Queries repetitivas (categorias, departamentos, roles, permissões) vão ao banco toda vez.

**Dados candidatos a cache:**
| Dado | Frequência de Leitura | Frequência de Escrita | TTL Sugerido |
|------|:---------------------:|:---------------------:|:------------:|
| Categorias | Alta | Baixa | 5 min |
| Departamentos | Alta | Muito Baixa | 10 min |
| Roles + Permissões | Muito Alta | Rara | 30 min |
| Fornecedores | Média | Baixa | 5 min |
| Produtos (por código de barras) | Altíssima (PDV) | Média | 1 min + jitter |

---

## 5. Sem Paginação Padrão (🟡 MÉDIA)

**Problema:** A maioria dos endpoints `Listar*` retorna TODOS os registros:

```csharp
// ListarCategoriaHandler — retorna TODAS as categorias
var grupoCollection = _repositoryCategoria.Listar().ToList();

// ListarProdutoHandler — retorna TODOS os produtos (com Include!)
var _produto = _repositoryProduto.Listar()
    .Include(x => x.Fornecedor).Include(x => x.Categoria).ToList();
```

**Exceção:** `ListarProdutosPaginado` e `ListarClientesPaginado` existem — mas são exceções, não regra.

**Correção:** Paginação deve ser padrão para todas as listagens.

---

## 6. Sem Logging Estruturado nos Handlers (🟡 MÉDIA)

**Problema:** Os handlers antigos (Categoria, Departamento, Fornecedor, Colaborador, Pedido) não têm logging nenhum:

```csharp
// AdicionarCategoriaHandler.cs — SEM logging
public async Task<Response> Handle(AdicionarCategoriaRequest request, CancellationToken cancellationToken)
{
    // ... operação executada sem rastreabilidade
}
```

**Contraste com os handlers novos:**
```csharp
// ProcessPaymentHandler.cs — COM logging
_logger.LogInformation("Processing payment for order {OrderId} with amount {Amount}", 
    request.OrderId, request.TotalAmount);
```

---

## 7. Sem Rate Limiting nos Endpoints Críticos (🟡 MÉDIA)

**Problema:** A API já tem Rate Limiting configurado como middleware geral, mas endpoints críticos (login, pagamento) não têm políticas específicas.

Endpoints que precisam de rate limiting rigoroso:
- `POST /api/auth/login` — prevenir brute force
- `POST /api/v1/payment/process` — prevenir abuso
- `POST /api/v1/pedido/AdicionarPedido` — prevenir duplicatas

---

## 8. Diagrama de Dependências Circular Potencial (🟢 MENOR)

```
Form → API → Infra → Domain
Form → Infra → Domain
Form → Domain

// A dependência do Form cria um grafo denso:
//   Form conhece TUDO
//   API conhece Infra e Domain
//   Infra conhece Domain
//   Domain é independente (exceto EF Core — problema #1)
```

---

## 9. Sem API de Referência de DTOs Compartilhados (🟡 MÉDIA)

**Problema:** Não existe projeto `Shared` ou `Contracts`:
- O Form usa diretamente os Requests/Responses do Domain
- Se o Form fosse desacoplado, precisaria de DTOs compartilhados
- Os `CupomDTO.cs` e `Reports/` estão no Domain (local errado)

---

## 10. `Resquest` — Typo Espalhado (🟢 MENOR)

**Problema:** O typo `Resquest` (faltando o "e" em "Request") aparece em vários arquivos:

```csharp
RemoverCategoriaResquest        // ← "Resquest" ao invés de "Request"
RemoverDepartamentoResquest
RemoverProdutoResquest
RemoverPedidoResquest
```

---

## Quadro Geral de Prioridades

```
╔══════════════════════════════════════════════════════════════════════╗
║  PRIORIDADE 1 — Risco de Bug em Produção                          ║
║  ├── #2  Unit of Work (operações não-atômicas)                     ║
║  ├── #4  Func<> vs Expression<> (tabela inteira na memória)        ║
║  └── #5  Listar sem paginação (OOM em produção)                    ║
╠══════════════════════════════════════════════════════════════════════╣
║  PRIORIDADE 2 — Dívida Técnica Alta                               ║
║  ├── #1  Idioma único (PT ou EN)                                   ║
║  ├── #3  Migrar de prmToolkit para Result<T>                       ║
║  ├── #6  Adicionar logging nos handlers antigos                    ║
║  └── #9  Criar projeto Shared.Contracts                            ║
╠══════════════════════════════════════════════════════════════════════╣
║  PRIORIDADE 3 — Melhorias                                         ║
║  ├── #4  Implementar caching                                      ║
║  ├── #7  Rate limiting por endpoint                                ║
║  ├── #8  Desacoplar Form                                           ║
║  └── #10 Corrigir typo "Resquest"                                  ║
╚══════════════════════════════════════════════════════════════════════╝
```

---

Data da análise: 2026-02-16
Analista: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
