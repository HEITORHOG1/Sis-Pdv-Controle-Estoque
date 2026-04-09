# 🟡 Análise do Projeto Infrastructure (`Sis-Pdv-Controle-Estoque-Infra`)

> Severidade geral: **MÉDIA** — Funcionalmente correto, mas com problemas de design e performance.

## 1. Duplicação de `_context` no RepositoryProduto (🟡 MÉDIA)

**Problema:** O `RepositoryProduto` declara `_context` como campo privado, mas `RepositoryBase` já tem um campo protegido:

```csharp
public class RepositoryProduto : RepositoryBase<Produto, Guid>, IRepositoryProduto
{
    private readonly PdvContext _context;  // ← Duplicado! RepositoryBase já tem _context

    public RepositoryProduto(PdvContext context) : base(context)
    {
        _context = context;  // ← Armazena duas vezes
    }
}
```

**Correção:** Usar o `_context` do `RepositoryBase` (torná-lo `protected` se não for).

---

## 2. Métodos Síncronos e Assíncronos Duplicados (🟡 MÉDIA)

**Problema:** `IRepositoryBase` define versões síncronas e assíncronas do mesmo método:

```csharp
TEntidade Adicionar(TEntidade entidade);                    // Síncrono
Task<TEntidade> AdicionarAsync(TEntidade entidade);         // Assíncrono (sem CancellationToken!)

TEntidade Editar(TEntidade entidade);                       // Síncrono
Task<TEntidade> EditarAsync(TEntidade entidade);            // Assíncrono (sem CancellationToken!)

void Remover(TEntidade entidade);                           // Síncrono
Task RemoverAsync(TEntidade entidade);                      // Assíncrono (sem CancellationToken!)

TEntidade ObterPorId(TId id, ...);                          // Síncrono
Task<TEntidade?> ObterPorIdAsync(TId id, ...);              // Assíncrono (sem CancellationToken!)
```

**Problemas:**
1. Versões síncronas encobrem chamadas de banco blocking (perigoso em servidores web)
2. Versões assíncronas **não recebem `CancellationToken`** (exceto os métodos "Enhanced")
3. Duas APIs para a mesma operação confundem o desenvolvedor

**Correção:** Remover os métodos síncronos. Adicionar `CancellationToken` em todos os assíncronos.

---

## 3. API Duplicada — Convenções Conflitantes (🟡 MÉDIA)

**Problema:** A interface tem dois conjuntos de métodos com convenções diferentes:

```csharp
// API "antiga" — Português, sem CancellationToken
TEntidade Adicionar(TEntidade entidade);
TEntidade Editar(TEntidade entidade);
void Remover(TEntidade entidade);
TEntidade ObterPorId(TId id, ...);

// API "nova" — Inglês, com CancellationToken
Task<TEntidade> AddAsync(TEntidade entity, CancellationToken cancellationToken = default);
Task<TEntidade> UpdateAsync(TEntidade entity, CancellationToken cancellationToken = default);
Task<TEntidade?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
```

**Impacto:** Desenvolvedores não sabem qual usar. Confusão sobre qual é o "correto".

**Correção:** Manter apenas um conjunto (preferencialmente os assíncronos com `CancellationToken`).

---

## 4. `Func<>` vs `Expression<Func<>>` Misturados (🔴 CRÍTICA)

**Problema:** Alguns métodos recebem `Func<>` e outros `Expression<Func<>>`:

```csharp
TEntidade ObterPor(Func<TEntidade, bool> where, ...);                      // ← Func
Task<TEntidade?> ObterPorAsync(Expression<Func<TEntidade, bool>> where, ...); // ← Expression

bool Existe(Func<TEntidade, bool> where);                                   // ← Func  
Task<bool> ExisteAsync(Expression<Func<TEntidade, bool>> where);            // ← Expression
```

**Por que é grave:**
- `Func<>` executa **client-side** — carrega TODOS os registros na memória, depois filtra em C#
- `Expression<>` traduz para SQL e filtra no banco

Exemplo do impacto:
```csharp
// RepositoryProduto usa:
_repositoryProduto.Existe(x => x.CodBarras == request.codBarras)
//                ^^^^^^ Func<> → Carrega TODOS os produtos na memória!
// Em produção com 50.000 produtos, isso é desastroso.
```

**Correção:** Usar SEMPRE `Expression<Func<>>`:
```csharp
bool Existe(Expression<Func<TEntidade, bool>> where);
```

---

## 5. `Listar()` sem Paginação Retorna Tabela Inteira (🔴 CRÍTICA)

**Problema:** O método `Listar()` retorna `IQueryable` sem limite, e handlers fazem `.ToList()`:

```csharp
// ListarColaboradorHandler.cs
var grupoCollection = _repositoryColaborador.Listar()
    .Include(x => x.Usuario)
    .Include(x => x.Departamento)
    .ToList();  // ← Carrega TODOS os colaboradores + todos os joins!
```

**Impacto:** Em produção com milhares de registros, carrega tudo na memória. Sem paginação.

**Correção:** Sempre usar paginação:
```csharp
var result = await _repositoryColaborador.ListarPaginadoAsync(
    pageNumber: 1,
    pageSize: 50,
    cancellationToken: ct);
```

---

## 6. Falta de Global Query Filter para Soft Delete (🟡 MÉDIA)

**Problema:** Cada query precisa manualmente filtrar `!IsDeleted`:

```csharp
// RepositoryProduto.cs
IQueryable<Produto> query = _context.Set<Produto>()
    .Where(x => !x.IsDeleted && x.StatusAtivo == 1);  // ← Filtro manual em CADA método
```

**Correção:** Usar EF Core Global Query Filter:
```csharp
// PdvContext.cs — OnModelCreating
modelBuilder.Entity<Produto>().HasQueryFilter(p => !p.IsDeleted);
// Agora TODAS as queries filtram IsDeleted automaticamente
```

---

## 7. `new` Shadowing — `Include` Method (🟢 MENOR)

**Problema:**
```csharp
protected new IQueryable<Produto> Include(...)
//         ^^^ Shadowing de método do RepositoryBase com 'new'
```

**Impacto:** Indica design frágil. Se o tipo for referenciado pela base class, o `Include` errado é chamado.

---

## 8. Métodos Assíncronos sem Cancelamento (🟡 MÉDIA)

**Problema:** `DesativarAsync`, `AtivarAsync`, `CountAsync`, `GetLowStockProductsAsync` não recebem `CancellationToken`:

```csharp
public async Task<int> CountAsync()
{
    return await _context.Produtos
        .Where(p => !p.IsDeleted && p.StatusAtivo == 1)
        .CountAsync();  // ← CountAsync() aceita CancellationToken, mas não é passado
}
```

---

## Resumo

| # | Problema | Severidade | Tipo |
|---|---------|:----------:|------|
| 1 | `_context` duplicado | 🟡 | Clean Code |
| 2 | Métodos sync/async duplicados | 🟡 | Design |
| 3 | Convenções conflitantes (PT vs EN) | 🟡 | Clean Code |
| 4 | `Func<>` vs `Expression<>` | 🔴 | Performance |
| 5 | `Listar()` sem paginação | 🔴 | Performance |
| 6 | Sem Global Query Filter | 🟡 | DDD |
| 7 | `new` shadowing em Include | 🟢 | Clean Code |
| 8 | Async sem CancellationToken | 🟡 | Performance |

---

Data da análise: 2026-02-16
