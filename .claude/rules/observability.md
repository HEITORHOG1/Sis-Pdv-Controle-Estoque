# Regras de Observabilidade

## Structured Logging (Serilog)
- Use structured logging (NÃO string interpolation nos templates)
- ✅ Correto: `logger.LogInformation("User {UserId} logged in", userId);`
- ❌ Errado: `logger.LogInformation($"User {userId} logged in");`
- Inclua: CorrelationId, UserId, EnvironmentName no contexto

## Enrichers Configurados
- `FromLogContext` — contexto dinâmico por request
- `WithCorrelationId` — ID único por request
- `WithEnvironmentName` — Production/Development/Staging
- `WithMachineName` — nome do servidor
- `WithProcessId` / `WithThreadId` — diagnóstico de concorrência

## Middlewares de Monitoramento
- `RequestLoggingMiddleware` — loga início/fim + duração de cada request
- `MetricsMiddleware` — coleta métricas (status code, latência, endpoint)
- Ambos são registrados no pipeline antes dos controllers

## Health Checks
- Implementados para todas as dependências:
  - MySQL (via AspNetCore.HealthChecks.MySql)
  - RabbitMQ (via AspNetCore.HealthChecks.Rabbitmq)
  - EF Core (via HealthChecks.EntityFrameworkCore)
  - BusinessHealthCheck (custom)
  - SystemMetricsHealthCheck (custom)
  - MetricsCollectionService (custom)
- Endpoints: `/health` (liveness), `/health-ui` (dashboard)
- Intervalo de avaliação: 30s

## Níveis de Log
- Trace: dev only, detalhes granulares
- Debug: informação diagnóstica, dev/staging
- Information: eventos importantes do fluxo normal
- Warning: anomalias recuperáveis
- Error: falhas que impedem operação mas app continua
- Critical: requer atenção imediata
