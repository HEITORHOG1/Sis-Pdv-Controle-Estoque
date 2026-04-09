# Regras de Testes

## Estrutura
```
Sis-Pdv-Controle-Estoque.Tests/
├── Infrastructure/                 → Fixtures e base classes
│   ├── TestFixture.cs             → Setup do ServiceProvider
│   ├── IntegrationTestBase.cs     → Base para testes de integração
│   ├── TestStartup.cs             → Startup de teste
│   └── WebApplicationTestFixture.cs → Setup para testes de API
├── UnitTests/                     → Testes com mocks
│   ├── ModelTests.cs              → Testes de entidades de domínio
│   └── SimpleTests.cs             → Testes básicos
└── IntegrationTests/              → Testes com InMemory DB
    └── BasicIntegrationTests.cs   → Testes de integração
```

## Convenções

### Nomeação
- `Metodo_Cenario_ResultadoEsperado`
- Exemplo: `Handle_ValidRequest_ShouldCreateCliente`
- Exemplo: `Handle_DuplicateCpf_ShouldReturnError`

### Padrão AAA
```csharp
[Fact]
public async Task Handle_ValidRequest_ShouldCreateCliente()
{
    // Arrange - prepare mocks e dados
    var mockRepo = new Mock<IRepositoryCliente>();
    var handler = new AdicionarClienteHandler(mockRepo.Object);

    // Act - execute a ação
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert - verifique resultados
    result.Should().NotBeNull();
    result.Success.Should().BeTrue();
}
```

## Frameworks de Teste
- **xUnit** — framework de testes
- **Moq** — mocking de dependências
- **FluentAssertions** — assertions legíveis
- **EF Core InMemory** — banco em memória para integração

## O que Testar

### DEVE testar
- Handlers MediatR (toda lógica de negócio)
- Validações de domínio (construtores com DomainException)
- Validators (CPF/CNPJ, código de barras)
- Serviços de aplicação

### NÃO precisa testar
- Framework code (EF Core, ASP.NET Core)
- Controllers que apenas delegam para MediatR
- Código gerado automaticamente

## Categorias de Testes
- `Unit` — testes isolados com mocks
- `Integration` — testes com InMemory DB
- `E2E` — fluxos completos de negócio

## Executando
```powershell
# Todos
dotnet test --logger "console;verbosity=detailed"

# Com coverage
dotnet test --collect:"XPlat Code Coverage"

# Script dedicado
.\Sis-Pdv-Controle-Estoque.Tests\run-tests.ps1
```
