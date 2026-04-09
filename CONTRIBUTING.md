# Contribuindo com o Sis-Pdv-Controle-Estoque

Obrigado por considerar contribuir! Este projeto é open-source e agradecemos contribuições de qualquer tipo.

## Como Contribuir

### 1. Issues

Antes de começar qualquer trabalho:
- Verifique se já existe uma issue relacionada
- Se não existir, crie uma descrevendo o bug ou melhoria
- Aguarde confirmação antes de iniciar

### 2. Fork e Branch

```bash
# Fork o repositório no GitHub
# Clone seu fork
git clone https://github.com/SEU_USUARIO/Sis-Pdv-Controle-Estoque.git
cd Sis-Pdv-Controle-Estoque

# Crie uma branch descritiva
git checkout -b feature/nome-da-funcionalidade
# ou
git checkout -b fix/descricao-do-bug
```

### 3. Desenvolvimento

Siga o [Guia de Desenvolvimento](docs/Development-Guide.md) para configurar seu ambiente.

#### Regras obrigatórias:

- ✅ **Sempre passe `CancellationToken`** em métodos assíncronos
- ✅ **Novas entidades herdam de `EntityBase`** (auditoria automática)
- ✅ **Validações no domínio** usando `DomainException`
- ✅ **Handlers via MediatR** para novos commands/queries
- ✅ **Structured logging** via Serilog (não string interpolation)
- ✅ **Testes** para handlers e lógica de negócio
- ❌ **Nunca** hardcode credenciais
- ❌ **Nunca** use `.Result` ou `.Wait()` (sync-over-async)
- ❌ **Nunca** adicione frameworks não aprovados

### 4. Testes

```powershell
# Rode todos os testes antes de abrir PR
dotnet test --logger "console;verbosity=detailed"

# Garanta que o build compila
dotnet build
```

### 5. Commit e PR

```bash
git add .
git commit -m "feat: descrição curta e clara da mudança"
git push origin feature/nome-da-funcionalidade
```

Abra um Pull Request no GitHub com:
- Descrição clara do que foi feito
- Issue relacionada (se houver)
- Screenshots (se mudança visual)

## Padrão de Commits

Siga o [Conventional Commits](https://www.conventionalcommits.org/):

| Tipo     | Uso                                    |
|----------|----------------------------------------|
| `feat`   | Nova funcionalidade                    |
| `fix`    | Correção de bug                        |
| `docs`   | Alteração na documentação              |
| `refactor` | Refatoração sem mudar comportamento  |
| `test`   | Adição ou correção de testes           |
| `chore`  | Manutenção, CI/CD, dependências        |

Exemplos:
```
feat: adicionar endpoint de relatório financeiro
fix: corrigir cálculo de margem de lucro no Produto
docs: atualizar instruções de deploy no README
refactor: extrair validação de CPF para classe dedicada
test: adicionar testes para AdicionarProdutoHandler
```

## Estrutura de Pastas para Novos Recursos

Ao adicionar um novo recurso, crie os arquivos nas pastas corretas:

```
1. Model/NovEntidade.cs                          → Domain
2. Interfaces/IRepositoryNovaEntidade.cs          → Domain
3. Commands/NovaEntidade/Adicionar*/              → Domain
4. Infra/Repositories/RepositoryNovaEntidade.cs   → Infrastructure
5. API/Controllers/NovaEntidadeController.cs      → API
6. Tests/UnitTests/Handlers/NovaEntidadeTests.cs  → Tests
```

## Código de Conduta

- Seja respeitoso e construtivo nos reviews
- Foque na qualidade do código, não da pessoa
- Aceite feedback e melhore iterativamente

## Licença

Ao contribuir, você concorda que suas contribuições serão licenciadas sob a mesma licença MIT do projeto.

---

Mantainer: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/
