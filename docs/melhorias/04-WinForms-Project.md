# 🔴 Análise do Projeto WinForms (`Sis-Pdv-Controle-Estoque-Form`)

> Severidade geral: **CRÍTICA** — Acoplamento direto com API/Infra/Domain, DLL externa sem NuGet.

## 1. Acoplamento Direto com API, Infra e Domain (🔴 CRÍTICA)

**Problema:**

```xml
<!-- Sis-Pdv-Controle-Estoque-Form.csproj -->
<ProjectReference Include="..\Sis-Pdv-Controle-Estoque-API\Sis-Pdv-Controle-Estoque-API.csproj" />
<ProjectReference Include="..\Sis-Pdv-Controle-Estoque-Infra\Sis-Pdv-Controle-Estoque-Infra.csproj" />
<ProjectReference Include="..\Sis-Pdv-Controle-Estoque\Sis-Pdv-Controle-Estoque-Domain.csproj" />
```

**Impacto brutal:**
- O Form **compila junto com a API inteira** — incluindo ASP.NET Core, Swagger, Health Checks, etc.
- O Form conhece o `PdvContext`, os repositories, os handlers — pode fazer TUDO diretamente
- Bypass total da API REST — se o Form acessa o banco direto, a API vira inútil
- Se a API muda, o Form pode quebrar silenciosamente
- Build do Form demora mais (carrega 3 projetos + todas as suas dependências)
- O MediatR na Form não faz sentido se a comunicação deveria ser HTTP

**Evidência:** O Form referencia até `MediatR`:
```xml
<PackageReference Include="MediatR" Version="12.5.0" />
```

**Correção arquitetural:**
```xml
<!-- O Form deveria ter APENAS isso -->
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Polly" Version="8.6.1" />
<!-- E um projeto compartilhado de DTOs, se necessário -->
<ProjectReference Include="..\Shared.Contracts\Shared.Contracts.csproj" />
```

O Form deveria se comunicar **exclusivamente via HTTP** com a API:
```
[Form] ──HTTP──> [API] ──> [Domain/Infra/DB]
```

---

## 2. DLL Externa sem NuGet: `FontAwesome.Sharp` (🟡 MÉDIA)

**Problema:**

```xml
<Reference Include="FontAwesome.Sharp">
    <HintPath>DLL\FontAwesome.Sharp.dll</HintPath>
</Reference>
```

**Problemas:**
1. DLL binária commitada no repositório (deveria estar no `.gitignore`)
2. Sem versionamento — não sabemos qual versão é
3. `FontAwesome.Sharp` existe no NuGet (versão 6.6.0)
4. Se a DLL for incompatível com .NET 8, não há aviso até runtime

**Correção:**
```xml
<PackageReference Include="FontAwesome.Sharp" Version="6.6.0" />
```

---

## 3. Logger Customizado via `Console.WriteLine` (🟡 MÉDIA)

**Problema:** O `frmMenu.cs` define um `MenuLogger` estático que loga direto no Console:

```csharp
private static class MenuLogger
{
    public static void LogInfo(string message, string category)
    {
        Console.WriteLine($"[INFO] [Menu-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
    
    public static void LogError(string message, string category, Exception? ex = null)
    {
        Console.WriteLine($"[ERROR] [Menu-{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
}
```

**Problemas:**
1. Em um app WinForms, `Console.WriteLine` **não aparece** (não tem console)
2. Deveria usar `ILogger<T>` do Microsoft.Extensions.Logging, ou no mínimo `Debug.WriteLine()`
3. Logger estático dificulta testes
4. Sem log estruturado

**Correção:**
```csharp
// Usar ILogger<frmMenu> via DI, ou ao menos:
System.Diagnostics.Debug.WriteLine($"[INFO] [Menu-{category}] {message}");
// Ou usar Serilog (já está no projeto API, reutilize)
```

---

## 4. P/Invoke para Mover Janela (🟢 MENOR)

**Problema:**

```csharp
private void pnHeader_MouseDown(object sender, MouseEventArgs e)
{
    MoverForm.ReleaseCapture();
    MoverForm.SendMessage(this.Handle, 0x112, 0xf012, 0);
}
```

**Problemas:**
1. Usa P/Invoke (`ReleaseCapture`, `SendMessage`) — magic numbers sem constantes nomeadas
2. `0x112` = `WM_SYSCOMMAND`, `0xf012` = `SC_DRAGMOVE` — deveriam ser constantes
3. Alternativa moderna: usar a própria API do WinForms ou bibliotecas como `WindowsFormsExtensions`

---

## 5. Botões Hardcoded com Comparação por Referência (🟡 MÉDIA)

**Problema:**

```csharp
private Color GetCorModulo(Button botao)
{
    if (botao == btnHome) return Color.FromArgb(46, 204, 113);
    if (botao == btnProdutos) return Color.FromArgb(52, 152, 219);
    if (botao == btnColaboradores) return Color.FromArgb(155, 89, 182);
    // ... 9 comparações por referência
    return Color.FromArgb(52, 152, 219);
}
```

**Problemas:**
1. Se um botão for renomeado, este switch quebra silenciosamente (retorna cor padrão)
2. Cores hardcoded — deveriam ser em um tema/constantes centralizadas
3. Adicionar novo botão requer alterar 3 métodos: `GetCorModulo`, `OcultarTextoBotoes`, `MostrarTextoBotoes`

**Correção:** Usar dicionário de configuração:
```csharp
private readonly Dictionary<Button, (string Icone, string Texto, Color Cor)> _modulos;
```

---

## 6. Mapeamento Ícone/Texto Repetido em 3 Lugares (🟡 MÉDIA)

**Problema:** A relação botão → ícone → texto está duplicada em 3 métodos:

```csharp
// Método 1: OcultarTextoBotoes → (btnHome, "🏠")
// Método 2: MostrarTextoBotoes → (btnHome, "🏠 INÍCIO")
// Método 3: Eventos Click → "🏠", "INÍCIO", "Página inicial com resumo do sistema"
```

Se adicionar um novo módulo, precisa editar 3 métodos + criar o evento Click.

---

## 7. Formulário com 610 Linhas (🟡 MÉDIA)

**Problema:** `frmMenu.cs` tem 610 linhas. Embora use `#region`, o arquivo concentra:
- Navegação de módulos
- Logger customizado
- Gerenciamento de botões
- Atalhos de teclado
- Timer
- Controles da janela (minimize, maximize, close)

**Correção:** Extrair responsabilidades:
- `MenuNavigationService` — lógica de navegação
- `MenuThemeManager` — cores, ícones, estados de botão
- `KeyboardShortcutHandler` — atalhos
- Logger — usar ILogger do framework

---

## 8. Falta de `async` nos Serviços HTTP (🟡 MÉDIA)

**Problema:** O Form tem 14 serviços em `Services/`:

```
Services/
├── Auth/AuthApiService.cs
├── Produto/ProdutoService.cs
├── Pedido/PedidoService.cs
├── Cliente/ClienteService.cs
...
```

Esses serviços fazem chamadas HTTP para a API. Precisam ser verificados se:
- Usam `async/await` corretamente
- Propagam erros de rede
- Fazem retry via Polly
- Tratam timeout
- Tratam token expirado (refresh token)

**Nota:** O projeto já inclui Polly 8.6.1 — bom sinal. Mas as referências diretas a API/Infra/Domain sugerem que nem todos os serviços usam HTTP.

---

## 9. `RabbitMQ.Client` no WinForms (🔴 CRÍTICA)

**Problema:**

```xml
<PackageReference Include="RabbitMQ.Client" Version="7.1.2" />
```

**Por que:**
- Um app desktop **não deveria** se conectar diretamente ao RabbitMQ
- Mensageria é responsabilidade do backend
- Se o Form envia mensagens diretamente, bypassa a API novamente
- Risco de segurança: a connection string do RabbitMQ ficaria no client

---

## 10. Escape Fecha o App (🟢 MENOR)

**Problema:**

```csharp
case Keys.Escape:
    btnLogout_Click(this, EventArgs.Empty);
    return true;
```

Tecla `Escape` abre diálogo de logout. Usuários podem fechar acidentalmente.

---

## Resumo

| # | Problema | Severidade | Tipo |
|---|---------|:----------:|------|
| 1 | Referência direta API/Infra/Domain | 🔴 | Arquitetura |
| 2 | DLL externa sem NuGet | 🟡 | Manutenção |
| 3 | Console.WriteLine em WinForms | 🟡 | Observabilidade |
| 4 | P/Invoke com magic numbers | 🟢 | Clean Code |
| 5 | Cores hardcoded por referência de botão | 🟡 | Design |
| 6 | Mapeamento duplicado em 3 métodos | 🟡 | DRY |
| 7 | Arquivo monolítico (610 linhas) | 🟡 | Clean Code |
| 8 | Serviços HTTP sem verificação async | 🟡 | Performance |
| 9 | RabbitMQ no desktop | 🔴 | Arquitetura |
| 10 | Escape abre logout | 🟢 | UX |

---

Data da análise: 2026-02-16
