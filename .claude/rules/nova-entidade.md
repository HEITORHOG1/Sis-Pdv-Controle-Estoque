# Workflow: Nova Entidade

Siga estes passos na ordem para adicionar uma nova entidade ao sistema:

## 1. Criar a Entidade no Domain

Crie `Sis-Pdv-Controle-Estoque/Model/NomeEntidade.cs`:
- Herde de `EntityBase`
- Adicione validações no construtor com `DomainException`
- Adicione método `Alterar*` para updates

## 2. Criar Interface do Repositório

Crie `Sis-Pdv-Controle-Estoque/Interfaces/IRepositoryNomeEntidade.cs`:
- Herde de `IRepositoryBase<NomeEntidade>`
- Adicione métodos específicos de consulta

## 3. Implementar o Repositório

Crie `Sis-Pdv-Controle-Estoque-Infra/Repositories/RepositoryNomeEntidade.cs`:
- Herde de `RepositoryBase<NomeEntidade>`
- Implemente os métodos da interface
- Use `CancellationToken` em todos os métodos async

## 4. Registrar no DbContext

Adicione `DbSet<NomeEntidade>` em `PdvContext.cs`

## 5. Criar Commands/Handlers

Crie pasta `Sis-Pdv-Controle-Estoque/Commands/NomeEntidade/`:
- `Adicionar*/` → Request + Handler (POST)
- `Alterar*/` → Request + Handler (PUT)
- `Remover*/` → Request + Handler (DELETE)
- `Listar*/` → Request + Handler (GET list)
- `ListarPorId/` → Request + Handler (GET by id)

## 6. Criar o Controller

Crie `Sis-Pdv-Controle-Estoque-API/Controllers/NomeEntidadeController.cs`:
- `[ApiController]`, `[Route("api/v1/[controller]")]`, `[Authorize]`
- Injete `IMediator` e delegue para handlers
- Propague `CancellationToken`

## 7. Registrar DI

Em `Setup.cs`, adicione:
```csharp
services.AddScoped<IRepositoryNomeEntidade, RepositoryNomeEntidade>();
```

## 8. Criar Migração

```powershell
dotnet ef migrations add AddNomeEntidade --project Sis-Pdv-Controle-Estoque-Infra --startup-project Sis-Pdv-Controle-Estoque-API
```

## 9. Verificar Build

```powershell
dotnet build
```

## 10. Adicionar Testes

Crie `Sis-Pdv-Controle-Estoque.Tests/UnitTests/Handlers/NomeEntidadeHandlerTests.cs`:
- Teste cada handler com Moq + FluentAssertions
- Siga padrão AAA

## 11. Rodar Testes

```powershell
dotnet test --logger "console;verbosity=detailed"
```
