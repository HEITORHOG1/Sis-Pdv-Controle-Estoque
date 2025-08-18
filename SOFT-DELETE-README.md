# 🚀 Implementação de Soft Delete - Instruções de Execução

## ✅ Status: Implementação Concluída

A implementação de soft delete foi concluída com sucesso! Todos os repositórios foram atualizados e os scripts de migração foram criados com os nomes corretos das tabelas do seu banco de dados.

## 📋 Pré-requisitos

- SQL Server com banco de dados `pdv_02`
- Acesso de administrador ao banco de dados
- PowerShell (opcional, para execução automatizada)

## 🎯 Execução Rápida

### Opção 1: Script PowerShell Automatizado (Recomendado)

```powershell
# Execute este comando no PowerShell
.\execute-soft-delete-migration.ps1 -ConnectionString "Server=localhost;Database=pdv_02;Integrated Security=true;"

# Ou com autenticação SQL Server
.\execute-soft-delete-migration.ps1 -ConnectionString "Server=localhost;Database=pdv_02;User Id=sa;Password=suasenha;"
```

### Opção 2: Execução Manual dos Scripts SQL

Execute os scripts **nesta ordem exata**:

1. **`add-soft-delete-columns.sql`** - Adiciona as colunas de soft delete
2. **`fix-soft-delete-migration.sql`** - Corrige os dados existentes  
3. **`verify-soft-delete-setup.sql`** - Verifica se tudo está correto

## 📊 O que Será Alterado

### Tabelas Afetadas
- categoria, produto, cliente, colaborador, departamento
- fornecedor, usuario, pedido, produtopedido, role
- permission, userrole, rolepermission, usersessions
- auditlog, payments, paymentaudits, paymentitems
- fiscalreceipts, stockmovement, cupom

### Colunas Adicionadas
Cada tabela receberá 3 novas colunas:
- `IsDeleted` (bit, default: 0) - Indica se foi deletado
- `DeletedAt` (datetime2, nullable) - Quando foi deletado
- `DeletedBy` (int, nullable) - Quem deletou

## 🔧 Como Funciona Após a Migração

### Antes (Hard Delete)
```csharp
// Removia fisicamente do banco
_repository.Remover(produto);
// Produto sumia para sempre
```

### Agora (Soft Delete)
```csharp
// Marca como deletado
_repository.Remover(produto);
// Produto fica oculto mas pode ser restaurado

// Restaurar se necessário
_repository.Restaurar(produto);
```

### Consultas Automáticas
```csharp
// Retorna apenas registros não deletados
var produtos = await _repository.ListarAsync();

// Ver todos incluindo deletados
var todos = _repository.ListarTodos();

// Ver apenas deletados
var deletados = _repository.ListarDeletados();
```

## ⚠️ Importante

### Backup Recomendado
```sql
-- Faça backup antes de executar
BACKUP DATABASE pdv_02 TO DISK = 'C:\Backup\pdv_02_before_soft_delete.bak'
```

### Verificação Pós-Migração
1. Execute `verify-soft-delete-setup.sql` para confirmar
2. Compile a aplicação (deve compilar sem erros)
3. Teste uma exclusão e verificação se funciona
4. Teste uma restauração

## 📚 Documentação Completa

- **`soft-delete-implementation-guide.md`** - Guia detalhado de uso
- **`soft-delete-implementation-summary.md`** - Resumo das alterações

## 🆘 Solução de Problemas

### Erro: "Coluna já existe"
- Normal se executar o script mais de uma vez
- O script verifica antes de adicionar

### Erro: "Tabela não encontrada"
- Verifique se está conectado ao banco correto (`pdv_02`)
- Confirme se as tabelas existem

### Erro de Permissão
- Execute como administrador do banco
- Verifique se tem permissão ALTER TABLE

## ✅ Resultado Esperado

Após a execução bem-sucedida:

1. ✅ Todas as tabelas terão colunas de soft delete
2. ✅ Registros existentes marcados como não deletados
3. ✅ Aplicação compila sem erros
4. ✅ Exclusões fazem soft delete automaticamente
5. ✅ Consultas filtram registros deletados
6. ✅ Possibilidade de restaurar registros

## 🎉 Pronto para Usar!

Após executar a migração, o sistema estará pronto com soft delete completo. Todas as exclusões serão reversíveis e você terá controle total sobre os dados!

---

**Dúvidas?** Consulte os arquivos de documentação ou execute o script de verificação para confirmar se tudo está funcionando corretamente.