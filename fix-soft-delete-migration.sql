-- Script para corrigir registros existentes com soft delete
-- Este script deve ser executado após a implementação do soft delete nos repositórios

-- Atualizar registros existentes para IsDeleted = false onde for NULL
-- Isso garante que registros antigos não sejam considerados deletados

-- categoria
UPDATE categoria 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- produto
UPDATE produto 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- cliente
UPDATE cliente 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- colaborador
UPDATE colaborador 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- departamento
UPDATE departamento 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- fornecedor
UPDATE fornecedor 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- usuario
UPDATE usuario 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- pedido
UPDATE pedido 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- produtopedido
UPDATE produtopedido 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- role
UPDATE role 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- permission
UPDATE permission 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- userrole
UPDATE userrole 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- rolepermission
UPDATE rolepermission 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- usersessions
UPDATE usersessions 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- auditlog
UPDATE auditlog 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- payments
UPDATE payments 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- paymentaudits
UPDATE paymentaudits 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- paymentitems
UPDATE paymentitems 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- fiscalreceipts
UPDATE fiscalreceipts 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- stockmovement
UPDATE stockmovement 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- cupom
UPDATE cupom 
SET IsDeleted = 0, DeletedAt = NULL, DeletedBy = NULL 
WHERE IsDeleted IS NULL;

-- Verificar se há registros que devem ser marcados como deletados
-- baseado em algum critério específico (exemplo: StatusAtivo = 0 para produtos)

-- Exemplo: Marcar produtos inativos como não deletados mas inativos
-- (não fazemos isso automaticamente, deixamos para decisão manual)

PRINT 'Soft delete migration completed successfully!';
PRINT 'All existing records have been marked as not deleted (IsDeleted = 0)';
PRINT 'Review your data and manually mark records as deleted if needed using UPDATE statements like:';
PRINT 'UPDATE TableName SET IsDeleted = 1, DeletedAt = GETUTCDATE(), DeletedBy = @UserId WHERE SomeCondition;';