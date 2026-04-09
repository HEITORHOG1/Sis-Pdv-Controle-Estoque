# 🟡 Análise do Projeto API (`Sis-Pdv-Controle-Estoque-API`)

> Severidade geral: **MÉDIA** — Estrutura razoável, mas com inconsistências e oportunidades de melhoria.

## 1. ControllerBase Herda de `Controller` ao invés de `ControllerBase` (🟡 MÉDIA)

**Problema:**

```csharp
public class ControllerBase : Controller  // ← Microsoft.AspNetCore.Mvc.Controller
```

**Impacto:**
- `Controller` inclui suporte a Views (Razor), que uma API REST não usa
- O correto para uma API é `Microsoft.AspNetCore.Mvc.ControllerBase` (mais leve)
- O nome da classe customizada (`ControllerBase`) conflita com o `ControllerBase` do framework, criando confusão:

```csharp
// ProdutoController.cs — precisa usar fully qualified name
public class ProdutoController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
//                                ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//                                Fully qualified porque conflita com Microsoft.AspNetCore.Mvc.ControllerBase
```

**Correção:**
```csharp
public class ApiControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase  // ← Renomear e herdar correto
```

---

## 2. `ResponseAsync` faz `SaveChanges` no Controller (🔴 CRÍTICA)

**Problema:**

```csharp
public async Task<IActionResult> ResponseAsync(Response response)
{
    if (!response.Notifications.Any())
    {
        try
        {
            await _unitOfWork.SaveChangesAsync();  // ← SaveChanges NO CONTROLLER!
```

**Por que é grave:**
- O controller decide quando salvar — deveria ser responsabilidade do handler ou de um pipeline behavior
- Se o handler adiciona 3 entidades e o SaveChanges falha na 2ª, as mudanças ficam inconsistentes
- Não passa `CancellationToken` para `SaveChangesAsync()`
- Mistura responsabilidades: response + persistência

**Correção:** Mover para um pipeline behavior do MediatR:
```csharp
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        await _unitOfWork.SaveChangesAsync(ct);
        return response;
    }
}
```

---

## 3. CancellationToken Não Propagado nos Controllers (🟡 MÉDIA)

**Problema:** Vários endpoints não propagam `CancellationToken`:

```csharp
// ProdutoController.cs
[HttpGet("ListarProduto")]
public async Task<IActionResult> ListarProduto()
{
    var request = new ListarProdutoRequest();
    var response = await _mediator.Send(request);  // ← Sem CancellationToken!
    return Ok(response);
}

// Contraste com o endpoint paginado que recebe CT (implicitamente via ASP.NET):
public async Task<IActionResult> ListarProdutosPaginado([FromQuery] ListarProdutosPaginadoRequest request)
{
    var response = await _mediator.Send(request);  // ← Também sem CT!
    return await ResponseAsync(response);
}
```

**Correção:**
```csharp
public async Task<IActionResult> ListarProduto(CancellationToken ct)
{
    var response = await _mediator.Send(new ListarProdutoRequest(), ct);
    return Ok(response);
}
```

---

## 4. Rotas Inconsistentes — Verbos em Português nas URLs (🟡 MÉDIA)

**Problema:**

```csharp
[HttpPost("AdicionarProduto")]              // ← PT no endpoint
[HttpPut("AlterarProduto")]                 // ← PT no endpoint
[HttpPut("AtualizaEstoque")]                // ← PT inconsistente ("Atualiza" vs "Alterar")
[HttpDelete("RemoverProduto/{id:Guid}")]    // ← PT no endpoint
[HttpGet("ListarProduto")]                  // ← PT no endpoint

// REST convencional seria:
// POST   /api/v1/produto        (criar)
// PUT    /api/v1/produto/{id}   (atualizar)
// DELETE /api/v1/produto/{id}   (remover)
// GET    /api/v1/produto        (listar)
// GET    /api/v1/produto/{id}   (obter por ID)
```

**Impacto:** URLs não seguem padrão REST. Dificulta integração com clientes que esperam RESTful API.

---

## 5. Resposta Inconsistente — `ResponseAsync` vs `Ok()` Direto (🟡 MÉDIA)

**Problema:** O mesmo controller usa dois padrões de resposta diferentes:

```csharp
// Padrão 1: Via ResponseAsync (faz SaveChanges, retorna ApiResponse)
[HttpPost("AdicionarProduto")]
public async Task<IActionResult> AdicionarProduto([FromBody] AdicionarProdutoRequest request)
{
    var response = await _mediator.Send(request);
    return await ResponseAsync(response);          // ← Com SaveChanges + ApiResponse wrapper
}

// Padrão 2: Via Ok() direto (sem SaveChanges, sem wrapper)
[HttpGet("ListarProduto")]
public async Task<IActionResult> ListarProduto()
{
    var response = await _mediator.Send(request);
    return Ok(response);                           // ← Sem SaveChanges, retorna Response cru
}
```

**Impacto:** O frontend recebe formatos de resposta diferentes dependendo do endpoint.

---

## 6. `IUnitOfWork` Injetado em TODOS os Controllers (🟡 MÉDIA)

**Problema:** Mesmo controllers que só fazem leitura (`GET`) recebem `IUnitOfWork`:

```csharp
public ProdutoController(IMediator mediator, IUnitOfWork unitOfWork) : base(unitOfWork)
//                                           ^^^^^^^^^^^^^^^^^^
// GET /ListarProduto não precisa de UnitOfWork!
```

**Impacto:** Controllers de leitura têm dependência desnecessária. Dificulta testes.

---

## 7. Exception Handling no ResponseAsync Engole Erros (🟡 MÉDIA)

**Problema:**

```csharp
catch (Exception ex)
{
    return BadRequest(ApiResponse.Error(
        "An internal server error occurred. Please contact support if the problem persists.",
        ex.Message,    // ← Expõe mensagem interna ao cliente!
        CorrelationId));
}
```

**Problemas:**
1. Deveria retornar `500 Internal Server Error`, não `400 Bad Request`
2. `ex.Message` pode conter detalhes internos (connection string, stack trace parcial)
3. Já existe `GlobalExceptionMiddleware` — por que duplicar o handling aqui?

---

## 8. Swagger XML Documentation Verbose (🟢 MENOR)

**Problema:** O `ProdutoController` tem 60+ linhas de XML comments para um único endpoint:

```csharp
/// <summary>
/// Retrieve paginated list of products with optional filtering
/// </summary>
/// <param name="request">Pagination and filtering parameters</param>
/// <returns>Paginated list of products with metadata</returns>
/// <remarks>
/// **Usage:**
/// ... 50 linhas de exemplo JSON ...
/// </remarks>
```

**Impacto:** O controller tem 191 linhas, mas apenas ~30 são lógica. O resto é documentação repetitiva. Swagger já gera os schemas automaticamente.

---

## 9. Versão de Pacotes Inconsistente (🟢 MENOR)

**Problema:** Algumas dependências estão em versões diferentes entre projetos:

```xml
<!-- API usa -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />

<!-- Infra usa -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.17" />
```

---

## Resumo

| # | Problema | Severidade | Tipo |
|---|---------|:----------:|------|
| 1 | Herança de `Controller` vs `ControllerBase` | 🟡 | Clean Code |
| 2 | `SaveChanges` no controller | 🔴 | Arquitetura |
| 3 | CancellationToken não propagado | 🟡 | Performance |
| 4 | Rotas em português, não REST | 🟡 | API Design |
| 5 | Respostas inconsistentes | 🟡 | API Design |
| 6 | IUnitOfWork em controllers de leitura | 🟡 | Clean Code |
| 7 | Exception handling expõe dados internos | 🟡 | Segurança |
| 8 | XML docs excessivos | 🟢 | Clean Code |
| 9 | Versões de pacotes inconsistentes | 🟢 | Manutenção |

---

Data da análise: 2026-02-16
