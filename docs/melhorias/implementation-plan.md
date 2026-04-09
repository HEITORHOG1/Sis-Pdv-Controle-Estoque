# Planejamento de Implementação das Melhorias

## Fase 1 — Quick Wins (Concluído)
- [x] 1.1 Corrigir nomes de arquivos/classes copy-paste (Domain #6)
- [x] 1.2 Corrigir typo "Resquest" → "Request" (Cross-Cutting #10)
- [x] 1.3 Preencher mensagens de notificação vazias (Domain #5)
- [x] 1.4 Remover `_mediator` não usado dos handlers (Domain #4)
- [x] 1.5 Substituir `Task.FromResult` por retorno direto (Domain #3)
- [x] 1.6 `dotnet build` + `dotnet test` — validar Fase 1

## Fase 2 — Domain: Corrigir Requests e Convenções (Concluído)
- [x] 2.1 Corrigir typo `quatidade` → `quantidade` nos Requests (Domain #7)
- [x] 2.2 Converter propriedades de Requests para PascalCase (Domain #7)
- [x] 2.3 Propagar CancellationToken nos handlers (Domain #8)
- [x] 2.4 Remover validação duplicada (manual + pipeline) (Domain #12)
- [x] 2.5 `dotnet build` + `dotnet test` — validar Fase 2

## Fase 3 — Infrastructure: Corrigir Performance (Concluído)
- [x] 3.1 Trocar `Func<>` por `Expression<Func<>>` em Existe() e ObterPor() (Infra #4)
- [x] 3.2 Remover `_context` duplicado do RepositoryProduto (Infra #1)
- [x] 3.3 Adicionar CancellationToken nos métodos assíncronos da Infra (Infra #8)
- [x] 3.4 `dotnet build` + `dotnet test` — validar Fase 3

## Fase 4 — Domain: Remover EF Core (Concluído)
- [x] 4.1 Criar métodos tipados nas interfaces de repositório (substituir .Include)
- [x] 4.2 Implementar métodos tipados nos repositórios da Infra
- [x] 4.3 Atualizar handlers para usar métodos tipados (sem .Include)
- [x] 4.4 Remover EF Core do Domain .csproj
- [x] 4.5 `dotnet build` + `dotnet test` — validar Fase 4

## Fase 5 — API: Corrigir Controller e Respostas (Concluído)
- [x] 5.1 Propagar CancellationToken nos controllers (API #3)
- [x] 5.2 Corrigir herança: Controller → ControllerBase do framework (API #1)
- [x] 5.3 Padronizar respostas (ResponseAsync vs Ok direto) (API #5)
- [x] 5.4 Corrigir exception handling no ResponseAsync (API #7)
- [x] 5.5 `dotnet build` + `dotnet test` — validar Fase 5

## Fase 6 — WinForms: Desacoplar (Concluído)
- [x] 6.1 Remover MediatR do WinForms .csproj (WinForms #1)
- [x] 6.2 Remover RabbitMQ.Client do WinForms .csproj (WinForms #9)
- [x] 6.3 Migrar FontAwesome.Sharp de DLL para NuGet (WinForms #2)
- [x] 6.4 Substituir Console.WriteLine por Debug.WriteLine (WinForms #3)
- [x] 6.5 Extrair mapeamento de botões para dicionário (WinForms #5/#6)
- [x] 6.6 `dotnet build` — validar Fase 6

## Fase 7 — Testes (Concluído)
- [x] 7.1 Criar testes unitários para entidades do Domain (Tests #2)
- [x] 7.2 Criar testes para validators do FluentValidation (Tests #3)
- [x] 7.3 `dotnet test` — validar Fase 7

## Fase 8 — Build Final e Walkthrough (Concluído)
- [x] 8.1 `dotnet build` completo — 0 erros, 0 warnings
- [x] 8.2 `dotnet test` completo — 104 testes, 0 falhas
- [x] 8.3 Criar walkthrough.md com resultados
