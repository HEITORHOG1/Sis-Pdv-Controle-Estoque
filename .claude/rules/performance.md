# Regras de Performance

## Async/Await
- SEMPRE passe CancellationToken em chamadas assíncronas
- NUNCA use sync-over-async (`.Result`, `.Wait()`, `.GetAwaiter().GetResult()`)
- NUNCA use `Task.Run` em handlers de requisição
- Evite `async void` (exceto event handlers no WinForms)

## Entity Framework Core
- Use `.Include()` com critério — não carregue grafo inteiro
- EVITE N+1 queries — use joins ou batch queries
- Use `.AsNoTracking()` para queries de leitura
- Projeções com `.Select()` quando não precisa da entidade completa
- Connection pooling já configurado (Min=5, Max=50)

## Connection Pooling (MySQL)
```
Pooling=true
MinPoolSize=5
MaxPoolSize=50
ConnectionTimeout=30
ConnectionLifeTime=300
ConnectionIdleTimeout=180
```

## HTTP Clients (Polly)
- O projeto Form usa Polly para resiliência na comunicação com a API
- Configure timeout (padrão 10s por endpoint)
- Use retry com exponential backoff para operações idempotentes
- Circuit breaker para dependências externas (SEFAZ, processadores de pagamento)

## Caching
- Não há caching implementado atualmente (oportunidade de melhoria)
- Se implementar: use TTL com jitter para evitar thundering herd
- Considere IMemoryCache para dados hot + IDistributedCache para dados compartilhados

## Bulk Operations
- Para operações com mais de 100 itens, considere batch inserts
- Use `ExecuteUpdateAsync()` / `ExecuteDeleteAsync()` do EF Core 8 para bulk updates
- Evite loop de save individual

## Monitoramento
- `MetricsMiddleware` coleta duração e status de cada request
- `RequestLoggingMiddleware` registra timing detalhado
- Health checks com intervalo de 30s para detectar degradação
