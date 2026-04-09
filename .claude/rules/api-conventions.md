# Convenções de API

## Controllers

- Todos os controllers herdam de `ControllerBase`
- Rota padrão: `/api/v1/[controller]`
- Atributo `[Authorize]` em todos os controllers (exceto login e health)
- Use `[FromBody]` para requests POST/PUT
- Use `[FromQuery]` para parâmetros de filtro em GET
- Sempre propague `CancellationToken` recebido do ASP.NET

## MediatR

- Requests devem implementar `IRequest<Response>`
- Handlers implementam `IRequestHandler<TRequest, Response>`
- Sem lógica de negócio nos controllers — tudo vai pro handler
- Use `ValidationBehavior` no pipeline para validação automática

## Responses

- Handlers retornam `Response` (sucesso/falha + mensagem)
- Controllers traduzem para HTTP status codes:
  - 200 OK para sucesso
  - 400 BadRequest para validação
  - 404 NotFound para entidade inexistente
  - 500 InternalServerError para erros inesperados

## Versionamento

- Versão da API é definida via `Asp.Versioning`
- Versão atual: v1
- URL pattern: `/api/v1/endpoint`

## Documentação

- Swagger disponível em `/api-docs`
- Todos os endpoints devem ter atributos `[ProducesResponseType]`
