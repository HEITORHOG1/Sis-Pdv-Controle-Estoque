# 🔴 Análise do Projeto Domain (`Sis-Pdv-Controle-Estoque-Domain`)

> Severidade geral: **CRÍTICA** — Este é o coração do sistema e contém as violações mais graves.

## 1. Dependência do EF Core no Domain (🔴 CRÍTICA)

**Problema:** O `.csproj` do Domain referencia `Microsoft.EntityFrameworkCore` diretamente.

```xml
<!-- Sis-Pdv-Controle-Estoque-Domain.csproj -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.17" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="8.0.17" />
```

**Impacto:** Violação fundamental de Clean Architecture. O Domain não deve saber qual ORM é usado.

**Causa raiz:** Os Handlers usam `.Include()` do EF Core diretamente:

```csharp
// ListarProdutoHandler.cs — DENTRO do Domain!
var _produto = _repositoryProduto.Listar()
    .Include(x => x.Fornecedor)   // ← Microsoft.EntityFrameworkCore
    .Include(x => x.Categoria)
    .ToList();
```

**Ocorrências encontradas:** 6 handlers fazem `.Include()` direto:
- `ListarProdutoHandler.cs`
- `ListarPedidoHandler.cs`
- `ListarColaboradorHandler.cs`
- `ListarPedidoPorNomeCpfCnpjHandler.cs`
- `ValidarColaboradorLoginHandler.cs`

**Correção:**
```csharp
// Interface no Domain — sem conhecimento de EF Core
public interface IRepositoryProduto : IRepositoryBase<Produto, Guid>
{
    Task<IEnumerable<Produto>> ListarComFornecedorECategoriaAsync(CancellationToken ct);
}

// Implementação na Infra — aqui sim pode usar .Include()
public async Task<IEnumerable<Produto>> ListarComFornecedorECategoriaAsync(CancellationToken ct)
{
    return await _context.Produtos
        .Include(p => p.Fornecedor)
        .Include(p => p.Categoria)
        .Where(p => !p.IsDeleted && p.StatusAtivo == 1)
        .ToListAsync(ct);
}
```

---

## 2. CQRS Invertido — Commands e Queries Misturados (🔴 CRÍTICA)

**Problema:** A pasta `Commands/` contém tanto operações de escrita quanto de leitura, violando CQRS.

```
Commands/
├── Produto/
│   ├── AdicionarProduto/        ← ✅ Isso é um Command
│   ├── AlterarProduto/          ← ✅ Isso é um Command
│   ├── RemoverProduto/          ← ✅ Isso é um Command
│   ├── ListarProduto/           ← ❌ Isso é uma QUERY
│   ├── ListarProdutoPorId/      ← ❌ Isso é uma QUERY
│   └── ListarProdutoPorNome/    ← ❌ Isso é uma QUERY
```

**Correção:** Separar em duas pastas:
```
Sis-Pdv-Controle-Estoque/
├── Commands/           ← Operações de escrita (Create, Update, Delete)
│   └── Produto/
│       ├── AdicionarProduto/
│       ├── AlterarProduto/
│       └── RemoverProduto/
└── Queries/            ← Operações de leitura (Read, List, Search)
    └── Produto/
        ├── ListarProduto/
        ├── ListarProdutoPorId/
        └── ListarProdutoPorCodBarras/
```

---

## 3. `Task.FromResult` em 40+ Handlers (🟡 MÉDIA)

**Problema:** A maioria dos handlers "antigos" retorna `await Task.FromResult(response)` — um anti-pattern que indica que o método não é verdadeiramente assíncrono.

```csharp
// AdicionarPedidoHandler.cs (e 40+ outros handlers)
Pedido = _repositoryPedido.Adicionar(Pedido);           // ← Síncrono!
var response = new Response(this, Pedido);
return await Task.FromResult(response);                 // ← Anti-pattern
```

**Por que é errado:**
1. Marca o método como `async` mas não faz nada assíncrono
2. Aloca uma state machine desnecessariamente
3. Os métodos de repositório chamados (`.Adicionar()`, `.Editar()`) são síncronos

**Correção — opção A (tornar realmente assíncrono):**
```csharp
var pedido = await _repositoryPedido.AdicionarAsync(pedido);
return new Response(this, pedido);  // sem Task.FromResult
```

**Correção — opção B (se manter síncrono):**
```csharp
// Simplesmente retorne sem await
return new Response(this, pedido);
// E remova o 'async' da assinatura do método
```

---

## 4. `_mediator` Injetado e Nunca Usado em 35+ Handlers (🟡 MÉDIA)

**Problema:** Dezenas de handlers recebem `IMediator` no construtor mas nunca o utilizam.

```csharp
public class AdicionarProdutoHandler : Notifiable, IRequestHandler<...>
{
    private readonly IMediator _mediator;              // ← NUNCA USADO
    private readonly IRepositoryProduto _repositoryProduto;

    public AdicionarProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
    {
        _mediator = mediator;                          // ← Alocação desperdiçada
        _repositoryProduto = repositoryProduto;
    }
}
```

**Handlers afetados:** AdicionarProduto, AlterarProduto, RemoverProduto, AdicionarPedido, AlterarPedido, RemoverPedido, AdicionarColaborador, AlterarColaborador, RemoverColaborador, AdicionarFornecedor, AlterarFornecedor, RemoverFornecedor, ListarDepartamento, ListarColaborador, ListarPedido... (35+)

**Correção:** Remover a injeção não utilizada.

---

## 5. Notificações com Mensagem Vazia (🔴 CRÍTICA)

**Problema:** 32+ instâncias de `AddNotification("Request", "")` — erro de validação sem mensagem.

```csharp
// RemoverDepartamentoHandler.cs
AddNotification("Request", "");     // ← O que isso significa para o usuário?
return new Commands.Response(this);

// Ou pior:
AddNotification("Produto", "retornoExist");  // ← String crua, não mensagem
```

**Impacto:** O frontend recebe erro sem explicação. Impossível debugar. UX terrível.

**Correção:**
```csharp
AddNotification("Departamento", "Departamento não encontrado com o ID informado.");
```

---

## 6. Nomes de Arquivos/Classes Incorretos — Copy-Paste (🔴 CRÍTICA)

**Problema:** Vários arquivos foram copiados de outro módulo e não renomeados:

| Arquivo | Está como | Deveria ser |
|---------|-----------|-------------|
| `Departamento/RemoverDepartamento/` | `RemoverCategoriaHandler.cs` | `RemoverDepartamentoHandler.cs` |
| `Departamento/ListarDepartamentoPorNome/` | `ListarDepartamentoPorNomeCategoriaHandler.cs` | `ListarDepartamentoPorNomeDepartamentoHandler.cs` |
| `Departamento/ListarDepartamentoPorNome/` | `ListarDepartamentoPorNomeCategoriaRequest.cs` | `ListarDepartamentoPorNomeDepartamentoRequest.cs` |
| `Colaborador/ListarColaboradorPorNome/` | `ListarColaboradorPorNomeCategoriaHandler.cs` | `ListarColaboradorPorNomeColaboradorHandler.cs` |
| `Colaborador/ListarColaboradorPorNome/` | `ListarColaboradorPorNomeCategoriaRequest.cs` | `ListarColaboradorPorNomeColaboradorRequest.cs` |
| `Fornecedor/ListarFornecedorPorNome/` | `ListarFornecedorPorNomeDepartamento/` (pasta!) | `ListarFornecedorPorNomeFornecedor/` |
| `Pedidos/RemoverPedido/` | `RemoverPedidorHandler.cs` | `RemoverPedidoHandler.cs` (typo: "Pedidor") |
| `Pedidos/ListarPedidorPorId/` | pasta com typo "Pedidor" | `ListarPedidoPorId/` |
| `Produto/ListarProdutoPorNomeProduto/` | `ListarProdutoPorCodBarrasHandler.cs` | Nome confuso, deveria ser separado |

**Impacto:** Confusão total na manutenção. Desenvolvedores abrem o arquivo errado.

---

## 7. Requests como Classe Mutável (🟡 MÉDIA)

**Problema:** Requests usam classes mutáveis com propriedades `{ get; set; }`:

```csharp
// AdicionarProdutoRequest.cs
public class AdicionarProdutoRequest : IRequest<Response>
{
    public string codBarras { get; set; }           // ← camelCase (deveria PascalCase)
    public string nomeProduto { get; set; }         // ← mutável
    public decimal precoCusto { get; set; }
    public int quatidadeEstoqueProduto { get; set; } // ← TYPO: "quatidade" vs "quantidade"
}
```

**Problemas:**
1. **Requests são mutáveis** — deveriam ser `record` imutáveis
2. **Convenção de nomeação violada** — propriedades em `camelCase` (C# usa `PascalCase`)
3. **Typo** na propriedade `quatidadeEstoqueProduto` (falta o "n" em "quantidade")
4. **Sem nullable annotations** — `string` pode ser null sem aviso

**Correção:**
```csharp
public record AdicionarProdutoRequest(
    string CodBarras,
    string NomeProduto,
    string DescricaoProduto,
    decimal PrecoCusto,
    decimal PrecoVenda,
    decimal MargemLucro,
    DateTime DataFabricao,
    DateTime DataVencimento,
    int QuantidadeEstoqueProduto,  // ← Nome corrigido
    Guid FornecedorId,
    Guid CategoriaId,
    int StatusAtivo
) : IRequest<Response>;
```

---

## 8. CancellationToken Ignorado (🟡 MÉDIA)

**Problema:** Os handlers recebem `CancellationToken` mas não o propagam para os repositórios:

```csharp
public async Task<Response> Handle(AdicionarProdutoRequest request, CancellationToken cancellationToken)
{
    Produto = _repositoryProduto.Adicionar(Produto);   // ← Não passa cancellationToken!
    return await Task.FromResult(response);
}
```

Isso significa que se o cliente cancelar a request, a operação de banco continua executando.

---

## 9. `IRepositoryBase` Expondo IQueryable (🟡 MÉDIA)

**Problema:** A interface base expõe `IQueryable<TEntidade>`:

```csharp
IQueryable<TEntidade> Listar(...);
IQueryable<TEntidade> ListarPor(...);
IQueryable<TEntidade> ListarEOrdenadosPor<TKey>(...);
```

**Impacto:** Vazamento de abstração. Qualquer camada pode construir queries arbitrárias sobre o IQueryable, bypassando a responsabilidade do repositório. Permite que handlers façam `.Include()` diretamente.

**Interface inchada:** 30+ métodos na interface base (8 tipos diferentes de Listar, 4 de Remover, etc.). Viola o **Interface Segregation Principle**.

---

## 10. `prmToolkit.NotificationPattern` como Base dos Handlers (🟡 MÉDIA)

**Problema:** Todos os handlers herdam de `Notifiable`, acumulando estado:

```csharp
public class AdicionarCategoriaHandler : Notifiable, IRequestHandler<...>
```

**Problemas:**
1. `Notifiable` é uma classe com estado mutável — handlers deveriam ser stateless
2. Se o handler for singleton ou reutilizado, notificações de requests anteriores vazam
3. Biblioteca `prmToolkit.NotificationPattern` v1.1.5 é antiga e pouco mantida
4. Padrão moderno: usar `Result<T>` ou `OneOf` para representar sucesso/falha

---

## 11. `Response.Data` com tipo `object` (🟡 MÉDIA)

**Problema:**

```csharp
public class Response
{
    public object Data { get; set; }  // ← Sem tipagem forte
}
```

**Impacto:** O frontend não sabe o tipo do `Data`. Força casting. Impossível gerar contratos tipados (ex: TypeScript). Deveria ser `Response<T>`.

---

## 12. Validação Duplicada (🟢 MENOR)

**Problema:** Alguns handlers instanciam validadores manualmente enquanto o pipeline já tem `ValidationBehavior`:

```csharp
// AdicionarCategoriaHandler.cs — Validação MANUAL
var validator = new AdicionarCategoriaValidator();
var validationResult = await validator.ValidateAsync(request, cancellationToken);

// Enquanto isso, existe um ValidationBehavior registrado no pipeline do MediatR
// que faz a mesma validação AUTOMATICAMENTE antes de chegar ao handler!
```

---

## Resumo

| # | Problema | Severidade | Tipo |
|---|---------|:----------:|------|
| 1 | EF Core no Domain | 🔴 | Arquitetura |
| 2 | Commands + Queries misturados | 🔴 | CQRS |
| 3 | `Task.FromResult` (40+ handlers) | 🟡 | Clean Code |
| 4 | `_mediator` nunca usado (35+ handlers) | 🟡 | Clean Code |
| 5 | Notificações vazias `""` (32+ instâncias) | 🔴 | UX / Debug |
| 6 | Nomes de arquivo copy-paste (5+ erros) | 🔴 | Clean Code |
| 7 | Requests mutáveis + camelCase + typo | 🟡 | C# Conventions |
| 8 | CancellationToken ignorado | 🟡 | Performance |
| 9 | IQueryable exposto no IRepositoryBase | 🟡 | Arquitetura |
| 10 | Handlers com estado via Notifiable | 🟡 | DDD |
| 11 | Response.Data como `object` | 🟡 | Type Safety |
| 12 | Validação duplicada (manual + pipeline) | 🟢 | Clean Code |

---

Data da análise: 2026-02-16
