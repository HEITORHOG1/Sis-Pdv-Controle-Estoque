# Regras de Segurança

## Gestão de Segredos
- NUNCA hardcode connection strings, API keys ou credenciais
- Desenvolvimento: User Secrets (`dotnet user-secrets`)
- Produção: variáveis de ambiente ou Azure Key Vault
- Verifique que segredos estão no `.gitignore`

## Validação de Input
- Todos os DTOs/Requests devem ter validação (FluentValidation)
- Valide no início do pipeline (ValidationBehavior no MediatR)
- Use queries parametrizadas (EF Core faz isso automaticamente)
- Sanitize inputs antes de logar ou exibir

## Autenticação & Autorização
- Todos os endpoints requerem `[Authorize]` (exceto login e health)
- Use authorization policies baseadas em claims de permissão
- Nunca confie em dados do cliente sem validar claims do JWT
- Valide assinatura e expiração do token (ClockSkew = Zero)

## Logging Seguro
- NUNCA logue passwords, tokens, API keys ou dados pessoais (PII)
- Redija dados sensíveis: emails (e***@***), cartões (****1234)
- Use níveis de log apropriados (sem Trace/Debug em produção)
- Detalhes de exceção somente em Development

## CORS & Headers de Segurança
- Produção: especifique origens exatas (nunca `*`)
- Habilite HTTPS redirect em todos os ambientes
- SecurityHeadersMiddleware define: HSTS, X-Content-Type-Options, X-Frame-Options, CSP

## Tratamento de Erros
- Produção: retorne ProblemDetails genérico (sem stack trace)
- Development: inclua detalhes para debugging
- Nunca exponha implementação interna nas mensagens de erro
- Use GlobalExceptionMiddleware para captura centralizada
