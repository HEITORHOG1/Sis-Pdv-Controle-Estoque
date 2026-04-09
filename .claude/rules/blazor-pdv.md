# Blazor PDV — Regras de Código

## Estrutura de Pastas
- `Configuration/` → Classes de configuração (IOptions pattern)
- `Extensions/` → Métodos de extensão para DI, HttpClient, etc.
- `Models/` → DTOs e Value Objects (sem lógica de infra)
- `Services/` → Interfaces + Implementações de comunicação com API
- `ViewModels/` → ViewModels (MVVM) com INotifyPropertyChanged
- `Components/` → Componentes Razor reutilizáveis
- `Pages/` → Páginas roteáveis (@page)

## Clean Code
- UMA classe por arquivo, nome do arquivo = nome da classe
- Classes seladas (`sealed`) quando não projetadas para herança
- Propriedades `init` para DTOs imutáveis
- Interfaces em arquivo separado (mesmo diretório)
- Extension methods para registro de DI (`AddPdvServices()`, `AddPdvViewModels()`)
- Métodos curtos (< 20 linhas idealmente, máximo 30)
- Sem magic strings — usar `const` ou `nameof()`

## MVVM
- ViewModels herdam de `ViewModelBase`
- Propriedades notificam mudança via `SetProperty<T>()`
- Lógica de negócio nos ViewModels, NÃO nos componentes Razor
- Componentes apenas fazem bind e chamam métodos do ViewModel
- Services injetados via construtor no ViewModel

## Naming
- Interfaces: `I{Nome}Service`, `I{Nome}Repository`  
- Implementações: `{Nome}Service`, `{Nome}Repository`
- ViewModels: `{Tela}ViewModel`
- Componentes: PascalCase descritivo (`PainelProduto.razor`)
- CSS classes: BEM com prefixo `pdv-` (`pdv-panel-left__header`)

## Arquitetura de Dados (Offline-First)
- O PDV é **AUTÔNOMO**: opera 100% offline com banco local (MySQL)
- **Leitura**: Sempre do banco local (rápido, sem dependência de rede)
- **Escrita (Vendas)**: Sempre no banco local primeiro
- **Sincronização (Down)**: RabbitMQ traz dados do ERP (Produtos, Usuários, Clientes) para o PDV em background
- **Sincronização (Up)**: Um Worker envia as vendas realizadas para o ERP quando há conexão
- **ORM**: Entity Framework Core (Code-First)
- **Acesso a Dados**: Repositories encapsulam o DbContext

## Repositories (Local Data Access)
- `Repositories/` → Interfaces + Implementações de acesso via EF Core
- Interface: `I{Nome}Repository`
- Implementação: `{Nome}Repository`
- Todos os métodos recebem `CancellationToken`
- Usar `AsNoTracking()` para leituras de alta performance
- Usar índices adequados no MySQL para busca por código de barras e nome

## Mensageria (RabbitMQ)
- `Messaging/Consumers/` → Listeners que atualizam o banco local
- O consumidor deve ser idempotente (tratar mensagens repetidas)
- Upsert: Se o registro existe, atualiza; senão, cria
- Tratamento de erros: DLQ (Dead Letter Queue) ou Retry Policy

## Async
- SEMPRE passar CancellationToken
- NUNCA usar .Result ou .Wait()
- Métodos async terminam em `Async`
- Tratar exceções com try/catch específico

## DI
- DbContext: Scoped (Default do EF Core)
- Repositories: Scoped
- Services: Scoped
- BackgroundService (RabbitMQ): Singleton ou Scoped via IServiceScopeFactory
- ViewModels: Scoped
- Configurações: IOptions<T> pattern

## Async
- SEMPRE passar CancellationToken
- NUNCA usar .Result ou .Wait()
- Métodos async terminam em `Async`
- Tratar exceções com try/catch específico

## DI
- Repositories: Scoped (um por circuito Blazor)
- Services: Scoped
- ViewModels: Scoped
- Configurações: IOptions<T> pattern
