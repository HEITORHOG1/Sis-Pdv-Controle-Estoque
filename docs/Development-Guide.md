# Guia de Desenvolvimento

## Pré-Requisitos

| Ferramenta     | Versão  | Download                                                        |
|----------------|---------|-----------------------------------------------------------------|
| .NET SDK       | 8.0+    | https://dotnet.microsoft.com/download/dotnet/8.0                |
| MySQL          | 8.0+    | https://dev.mysql.com/downloads/                                |
| RabbitMQ       | 3.12+   | https://www.rabbitmq.com/download.html (opcional para dev)      |
| Visual Studio  | 2022+   | https://visualstudio.microsoft.com/ (recomendado)               |
| VS Code        | Latest  | https://code.visualstudio.com/ (alternativa)                    |
| Git            | 2.40+   | https://git-scm.com/download                                   |
| Docker Desktop | 24.0+   | https://www.docker.com/products/docker-desktop (opcional)       |

## Setup Inicial

### 1. Clone o Repositório

```powershell
git clone https://github.com/HEITORHOG1/Sis-Pdv-Controle-Estoque.git
cd Sis-Pdv-Controle-Estoque
```

### 2. Restaure as Dependências

```powershell
dotnet restore
```

### 3. Configure User Secrets

```powershell
cd Sis-Pdv-Controle-Estoque-API

# Senha do MySQL
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha_mysql"

# JWT Secret (minimum 32 caracteres)
dotnet user-secrets set "Authentication:JwtSecret" "chave_jwt_com_pelo_menos_32_caracteres_aqui"

# Verifique
dotnet user-secrets list

cd ..
```

> **Consulte:** [User-Secrets-Configuration.md](User-Secrets-Configuration.md) para detalhes.

### 4. Prepare o Banco de Dados

Crie o banco no MySQL:
```sql
CREATE DATABASE PDV_02 CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
```

> **Não precisa** criar tabelas manualmente. O sistema aplica migrações automaticamente no startup.

### 5. Compile e Rode

```powershell
# Build
dotnet build

# Rode a API
dotnet run --project Sis-Pdv-Controle-Estoque-API
```

Acesse:
- **API:** http://localhost:7003
- **Swagger:** http://localhost:7003/api-docs
- **Health:** http://localhost:7003/health

### 6. Primeiro Login

Na primeira execução, o sistema cria automaticamente:
- Roles padrão (SuperAdmin, Admin, Manager, Cashier, Viewer)
- Permissões associadas
- Usuário admin

Consulte o `AuthSeederService` para as credenciais iniciais.

## Estrutura do Projeto

```
Sis-Pdv-Controle-Estoque.sln
│
├── Sis-Pdv-Controle-Estoque/              → DOMAIN
│   ├── Model/                             → Entidades de domínio
│   │   ├── Base/EntityBase.cs             → Classe base com auditoria
│   │   ├── Produto.cs, Pedido.cs, etc.    → Entidades de negócio
│   │   └── Reports/                       → DTOs de relatórios
│   ├── Commands/                          → CQRS Handlers (181 arquivos)
│   │   ├── Produto/                       → Adicionar, Alterar, Remover, Listar
│   │   ├── Pedidos/                       → Operações de pedidos
│   │   ├── Payment/                       → Processamento de pagamentos
│   │   └── Usuarios/                      → Gerenciamento de usuários
│   ├── Interfaces/                        → Contratos (IRepository*, IService*)
│   ├── Validators/                        → CpfCnpj, Barcode, Business validators
│   └── Exceptions/                        → DomainException
│
├── Sis-Pdv-Controle-Estoque-Infra/        → INFRASTRUCTURE
│   ├── Context/PdvContext.cs              → DbContext (EF Core)
│   ├── Repositories/                      → Implementações dos repositórios
│   ├── Mappings/                          → Configurações Fluent API
│   └── Migrations/                        → Migrações do banco
│
├── Sis-Pdv-Controle-Estoque-API/          → API (Presentation)
│   ├── Controllers/                       → 20 controllers
│   ├── Middleware/                         → Exception, Logging, Metrics, Security, Auth
│   ├── Configuration/                     → Setup modules (Swagger, CORS, Health, etc.)
│   ├── Services/                          → Serviços de aplicação
│   ├── Setup.cs                           → Registros de DI
│   └── Program.cs                         → Configuração do pipeline
│
├── Sis-Pdv-Controle-Estoque-Form/         → DESKTOP APP
│   ├── Paginas/                           → Telas WinForms
│   │   ├── Login/                         → Autenticação
│   │   ├── PDV/                           → Frente de loja
│   │   ├── Produto/, Categoria/, etc.     → Cadastros
│   │   └── Relatorios/                    → Tela de relatórios
│   └── Services/                          → Comunicação HTTP com a API
│
├── MessageBus/                            → MENSAGERIA
│   ├── IMessageBus.cs                     → Interface
│   └── BaseMessage.cs                     → Modelo base
│
└── Sis-Pdv-Controle-Estoque.Tests/        → TESTES
    ├── Infrastructure/                    → Fixtures e base classes
    ├── UnitTests/                         → Testes unitários com Moq
    └── IntegrationTests/                  → Testes de integração
```

## Padrões de Código

### Criando uma Nova Entidade

1. Crie a classe em `Sis-Pdv-Controle-Estoque/Model/`:

```csharp
using Model.Base;

namespace Model
{
    public class MinhaEntidade : EntityBase
    {
        public MinhaEntidade() { }

        public MinhaEntidade(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório");
            Nome = nome;
        }

        public string Nome { get; set; } = string.Empty;

        public void Alterar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório");
            Nome = nome;
        }
    }
}
```

2. Crie a interface do repositório em `Interfaces/`:

```csharp
public interface IRepositoryMinhaEntidade : IRepositoryBase<MinhaEntidade>
{
    Task<MinhaEntidade?> GetByNomeAsync(string nome, CancellationToken ct);
}
```

3. Implemente o repositório em `Infra/Repositories/`:

```csharp
public class RepositoryMinhaEntidade : RepositoryBase<MinhaEntidade>, IRepositoryMinhaEntidade
{
    public RepositoryMinhaEntidade(PdvContext context) : base(context) { }

    public async Task<MinhaEntidade?> GetByNomeAsync(string nome, CancellationToken ct)
    {
        return await _context.Set<MinhaEntidade>()
            .FirstOrDefaultAsync(x => x.Nome == nome && !x.IsDeleted, ct);
    }
}
```

4. Crie os Commands/Handlers em `Commands/MinhaEntidade/`:

```csharp
// AdicionarMinhaEntidade/AdicionarMinhaEntidadeRequest.cs
public record AdicionarMinhaEntidadeRequest(string Nome) : IRequest<Response>;

// AdicionarMinhaEntidade/AdicionarMinhaEntidadeHandler.cs
public class AdicionarMinhaEntidadeHandler : IRequestHandler<AdicionarMinhaEntidadeRequest, Response>
{
    private readonly IRepositoryMinhaEntidade _repository;

    public AdicionarMinhaEntidadeHandler(IRepositoryMinhaEntidade repository)
    {
        _repository = repository;
    }

    public async Task<Response> Handle(
        AdicionarMinhaEntidadeRequest request,
        CancellationToken cancellationToken)
    {
        var entidade = new MinhaEntidade(request.Nome);
        _repository.Add(entidade);
        return new Response("Entidade criada com sucesso");
    }
}
```

5. Crie o controller em `API/Controllers/`:

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MinhaEntidadeController : ControllerBase
{
    private readonly IMediator _mediator;

    public MinhaEntidadeController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AdicionarMinhaEntidadeRequest request,
        CancellationToken ct)
    {
        var response = await _mediator.Send(request, ct);
        return Ok(response);
    }
}
```

6. Registre no DI (`Setup.cs`):

```csharp
services.AddScoped<IRepositoryMinhaEntidade, RepositoryMinhaEntidade>();
```

7. Adicione ao `PdvContext`:

```csharp
public DbSet<MinhaEntidade> MinhaEntidades { get; set; }
```

8. Crie a migração:

```powershell
dotnet ef migrations add AddMinhaEntidade `
  --project Sis-Pdv-Controle-Estoque-Infra `
  --startup-project Sis-Pdv-Controle-Estoque-API
```

### Convenções de Nomeação

| Item            | Convenção                        | Exemplo                          |
|-----------------|----------------------------------|----------------------------------|
| Entidade        | PascalCase, substantivo singular | `Produto`, `StockMovement`       |
| Repository      | I + Repository + Entidade        | `IRepositoryProduto`             |
| Handler         | Ação + Entidade + Handler        | `AdicionarProdutoHandler`        |
| Request         | Ação + Entidade + Request        | `AdicionarProdutoRequest`        |
| Controller      | Entidade + Controller            | `ProdutoController`              |
| Métodos domain  | Verbo em português               | `AlterarProduto()`, `AtualizarDados()` |
| Propriedades    | PascalCase                       | `NomeProduto`, `TotalPedido`     |

> **Nota:** O projeto usa uma mistura de português e inglês. Entidades de negócio originais estão em português. Entidades mais recentes (Payment, StockMovement, FiscalReceipt) estão em inglês.

### Padrão de Response

Handlers retornam um `Response` genérico que encapsula sucesso/falha e mensagem.

## Rodando os Testes

```powershell
# Todos os testes
dotnet test --logger "console;verbosity=detailed"

# Com coverage
dotnet test --collect:"XPlat Code Coverage"

# Script dedicado
.\Sis-Pdv-Controle-Estoque.Tests\run-tests.ps1
```

### Escrevendo Testes

Siga o padrão AAA com nomes descritivos:

```csharp
[Fact]
public async Task Handle_ValidRequest_ShouldCreateCliente()
{
    // Arrange
    var mockRepo = new Mock<IRepositoryCliente>();
    mockRepo.Setup(x => x.Existe(It.IsAny<Func<Cliente, bool>>())).Returns(false);
    var handler = new AdicionarClienteHandler(mockRepo.Object);
    var request = new AdicionarClienteRequest("12345678901", "PF");

    // Act
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Success.Should().BeTrue();
    mockRepo.Verify(x => x.Add(It.IsAny<Cliente>()), Times.Once);
}
```

## Ferramentas Úteis

### Swagger UI
Acesse `http://localhost:7003/api-docs` para testar endpoints interativamente.

### Health Check Dashboard
Acesse `http://localhost:7003/health-ui` para monitorar dependências.

### Postman Collection
Disponível em `Sis-Pdv-Controle-Estoque-API/Documentation/PDV-Control-System-API.postman_collection.json`.

## Possíveis Problemas Comuns

### Erro ao rodar: "JWT Secret not configured"
Configure o user secret:
```powershell
cd Sis-Pdv-Controle-Estoque-API
dotnet user-secrets set "Authentication:JwtSecret" "chave_com_32_caracteres_aqui_no_minimo"
```

### Erro de conexão com MySQL
1. Verifique se o MySQL está rodando
2. Verifique se o banco `PDV_02` existe
3. Confirme o user secret da senha

### Build falha no WinForms (Linux/macOS)
O projeto Form (`net8.0-windows`) só compila no Windows. Ignore-o no Linux:
```bash
dotnet build --no-restore -p:ExcludeProjects=*Form*
```

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
