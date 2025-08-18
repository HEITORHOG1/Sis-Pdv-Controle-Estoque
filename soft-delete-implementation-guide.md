# Guia de Implementação de Soft Delete

## ✅ Alterações Implementadas

### 1. RepositoryBase Atualizado

**Métodos de Exclusão (Soft Delete):**
- `Remover()` - Agora marca `IsDeleted = true` ao invés de deletar fisicamente
- `RemoverAsync()` - Versão assíncrona do soft delete
- `RemoverFisicamente()` - Novo método para hard delete quando necessário

**Métodos de Consulta:**
- `Listar()` - Agora filtra automaticamente registros deletados (`WHERE !IsDeleted`)
- `ListarTodos()` - Lista todos os registros incluindo deletados
- `ListarDeletados()` - Lista apenas registros deletados

**Métodos de Restauração:**
- `Restaurar()` - Restaura um registro deletado
- `RestaurarAsync()` - Versão assíncrona da restauração

### 2. Repositórios Especializados

#### RepositoryProduto
- **Filtros:** Produtos ativos (`StatusAtivo = 1`) e não deletados
- **Métodos específicos:**
  - `ListarTodosAtivos()` - Todos os produtos não deletados
  - `ListarInativos()` - Produtos inativos mas não deletados
  - `Desativar()` / `Ativar()` - Controle de status
  - `DesativarAsync()` / `AtivarAsync()` - Versões assíncronas

#### RepositoryUsuario
- **Filtros:** Usuários ativos (`StatusAtivo = true`) e não deletados
- **Métodos específicos:**
  - `ListarTodosAtivos()` - Todos os usuários não deletados
  - `ListarInativos()` - Usuários inativos mas não deletados
  - `Desativar()` / `Ativar()` - Controle de status
  - `DesativarAsync()` / `AtivarAsync()` - Versões assíncronas

### 3. Interfaces Atualizadas
- `IRepositoryBase` - Adicionados métodos de soft delete
- `IRepositoryProduto` - Métodos específicos de controle de status
- `IRepositoryUsuario` - Métodos específicos de controle de status

## 🔧 Como Usar

### Exclusão Soft Delete (Recomendado)
```csharp
// Marcar como deletado (soft delete)
await _repository.RemoverAsync(produto);
await _unitOfWork.CommitAsync();
```

### Exclusão Física (Use com cuidado)
```csharp
// Deletar fisicamente do banco
_repository.RemoverFisicamente(produto);
await _unitOfWork.CommitAsync();
```

### Restaurar Registro Deletado
```csharp
// Restaurar um registro
var sucesso = await _repository.RestaurarAsync(produtoId);
if (sucesso)
{
    await _unitOfWork.CommitAsync();
}
```

### Consultas Específicas
```csharp
// Listar apenas ativos (padrão)
var produtosAtivos = await _produtoRepository.ListarAsync();

// Listar todos incluindo inativos
var todosProdutos = _produtoRepository.ListarTodosAtivos();

// Listar apenas inativos
var produtosInativos = _produtoRepository.ListarInativos();

// Listar apenas deletados
var produtosDeletados = _produtoRepository.ListarDeletados();

// Listar absolutamente tudo
var todosIncluindoDeletados = _produtoRepository.ListarTodos();
```

### Controle de Status (Produtos e Usuários)
```csharp
// Desativar sem deletar
await _produtoRepository.DesativarAsync(produtoId);

// Ativar
await _produtoRepository.AtivarAsync(produtoId);

// Para usuários
await _usuarioRepository.DesativarAsync(usuarioId);
await _usuarioRepository.AtivarAsync(usuarioId);
```

## 📊 Estados dos Registros

### Produto
1. **Ativo:** `IsDeleted = false` e `StatusAtivo = 1`
2. **Inativo:** `IsDeleted = false` e `StatusAtivo = 0`
3. **Deletado:** `IsDeleted = true` (StatusAtivo irrelevante)

### Usuário
1. **Ativo:** `IsDeleted = false` e `StatusAtivo = true`
2. **Inativo:** `IsDeleted = false` e `StatusAtivo = false`
3. **Deletado:** `IsDeleted = true` (StatusAtivo irrelevante)

### Outras Entidades
1. **Ativo:** `IsDeleted = false`
2. **Deletado:** `IsDeleted = true`

## 🗄️ Migração do Banco de Dados

### Scripts de Migração (executar nesta ordem):

1. **add-soft-delete-columns.sql** - Adiciona as colunas de soft delete nas tabelas que não possuem
2. **fix-soft-delete-migration.sql** - Define `IsDeleted = false` para todos os registros existentes
3. **verify-soft-delete-setup.sql** - Verifica se a implementação está correta

### Tabelas Afetadas (nomes corretos):
- categoria, produto, cliente, colaborador, departamento
- fornecedor, usuario, pedido, produtopedido, role
- permission, userrole, rolepermission, usersessions
- auditlog, payments, paymentaudits, paymentitems
- fiscalreceipts, stockmovement, cupom

## ⚠️ Considerações Importantes

### Comportamento Padrão
- **Consultas normais:** Retornam apenas registros não deletados
- **Exclusões:** Fazem soft delete por padrão
- **Produtos/Usuários:** Filtram também por status ativo

### Quando Usar Hard Delete
- Limpeza de dados de teste
- Conformidade com LGPD/GDPR
- Dados temporários ou de cache
- Logs muito antigos

### Performance
- Índices recomendados:
  ```sql
  CREATE INDEX IX_EntityName_IsDeleted ON EntityName (IsDeleted);
  CREATE INDEX IX_Produtos_StatusAtivo_IsDeleted ON Produtos (StatusAtivo, IsDeleted);
  CREATE INDEX IX_Usuarios_StatusAtivo_IsDeleted ON Usuarios (StatusAtivo, IsDeleted);
  ```

## 🔍 Verificação

### Testar Soft Delete
```csharp
// 1. Criar um produto
var produto = new Produto("123", "Teste", "Descrição", ...);
await _repository.AdicionarAsync(produto);
await _unitOfWork.CommitAsync();

// 2. Verificar se aparece na listagem
var produtos = await _repository.ListarAsync();
Assert.Contains(produto, produtos);

// 3. Deletar (soft delete)
await _repository.RemoverAsync(produto);
await _unitOfWork.CommitAsync();

// 4. Verificar se não aparece na listagem normal
produtos = await _repository.ListarAsync();
Assert.DoesNotContain(produto, produtos);

// 5. Verificar se aparece na listagem de deletados
var deletados = await _repository.ListarDeletados().ToListAsync();
Assert.Contains(produto, deletados);

// 6. Restaurar
await _repository.RestaurarAsync(produto.Id);
await _unitOfWork.CommitAsync();

// 7. Verificar se voltou para a listagem normal
produtos = await _repository.ListarAsync();
Assert.Contains(produto, produtos);
```

## 📝 Próximos Passos

1. **Executar migração SQL** - `fix-soft-delete-migration.sql`
2. **Testar repositórios** - Verificar se consultas funcionam corretamente
3. **Atualizar controllers** - Se necessário, ajustar lógica de negócio
4. **Criar índices** - Para melhor performance
5. **Documentar para equipe** - Explicar novo comportamento

## ✅ Benefícios

- **Recuperação de dados:** Registros podem ser restaurados
- **Auditoria:** Histórico completo de exclusões
- **Integridade referencial:** Relacionamentos preservados
- **Conformidade:** Atende requisitos de retenção de dados
- **Performance:** Consultas normais filtram automaticamente