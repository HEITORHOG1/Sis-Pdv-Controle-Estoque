-- Script para verificar se o soft delete foi implementado corretamente
-- Execute este script APÓS executar os scripts de migração

PRINT '=== VERIFICAÇÃO DO SOFT DELETE ===';
PRINT '';

-- Verificar se as colunas existem em cada tabela
PRINT '1. Verificando existência das colunas de soft delete:';
PRINT '';

-- categoria
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'categoria') AND name = 'IsDeleted')
    PRINT '✓ categoria: Colunas de soft delete existem'
ELSE
    PRINT '✗ categoria: Colunas de soft delete NÃO existem'

-- produto
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'produto') AND name = 'IsDeleted')
    PRINT '✓ produto: Colunas de soft delete existem'
ELSE
    PRINT '✗ produto: Colunas de soft delete NÃO existem'

-- cliente
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'cliente') AND name = 'IsDeleted')
    PRINT '✓ cliente: Colunas de soft delete existem'
ELSE
    PRINT '✗ cliente: Colunas de soft delete NÃO existem'

-- colaborador
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'colaborador') AND name = 'IsDeleted')
    PRINT '✓ colaborador: Colunas de soft delete existem'
ELSE
    PRINT '✗ colaborador: Colunas de soft delete NÃO existem'

-- departamento
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'departamento') AND name = 'IsDeleted')
    PRINT '✓ departamento: Colunas de soft delete existem'
ELSE
    PRINT '✗ departamento: Colunas de soft delete NÃO existem'

-- fornecedor
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'fornecedor') AND name = 'IsDeleted')
    PRINT '✓ fornecedor: Colunas de soft delete existem'
ELSE
    PRINT '✗ fornecedor: Colunas de soft delete NÃO existem'

-- usuario
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'usuario') AND name = 'IsDeleted')
    PRINT '✓ usuario: Colunas de soft delete existem'
ELSE
    PRINT '✗ usuario: Colunas de soft delete NÃO existem'

-- pedido
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'pedido') AND name = 'IsDeleted')
    PRINT '✓ pedido: Colunas de soft delete existem'
ELSE
    PRINT '✗ pedido: Colunas de soft delete NÃO existem'

-- produtopedido
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'produtopedido') AND name = 'IsDeleted')
    PRINT '✓ produtopedido: Colunas de soft delete existem'
ELSE
    PRINT '✗ produtopedido: Colunas de soft delete NÃO existem'

-- role
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'role') AND name = 'IsDeleted')
    PRINT '✓ role: Colunas de soft delete existem'
ELSE
    PRINT '✗ role: Colunas de soft delete NÃO existem'

-- permission
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'permission') AND name = 'IsDeleted')
    PRINT '✓ permission: Colunas de soft delete existem'
ELSE
    PRINT '✗ permission: Colunas de soft delete NÃO existem'

-- userrole
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'userrole') AND name = 'IsDeleted')
    PRINT '✓ userrole: Colunas de soft delete existem'
ELSE
    PRINT '✗ userrole: Colunas de soft delete NÃO existem'

-- rolepermission
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'rolepermission') AND name = 'IsDeleted')
    PRINT '✓ rolepermission: Colunas de soft delete existem'
ELSE
    PRINT '✗ rolepermission: Colunas de soft delete NÃO existem'

-- usersessions
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'usersessions') AND name = 'IsDeleted')
    PRINT '✓ usersessions: Colunas de soft delete existem'
ELSE
    PRINT '✗ usersessions: Colunas de soft delete NÃO existem'

-- auditlog
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'auditlog') AND name = 'IsDeleted')
    PRINT '✓ auditlog: Colunas de soft delete existem'
ELSE
    PRINT '✗ auditlog: Colunas de soft delete NÃO existem'

-- payments
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'payments') AND name = 'IsDeleted')
    PRINT '✓ payments: Colunas de soft delete existem'
ELSE
    PRINT '✗ payments: Colunas de soft delete NÃO existem'

-- paymentaudits
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'paymentaudits') AND name = 'IsDeleted')
    PRINT '✓ paymentaudits: Colunas de soft delete existem'
ELSE
    PRINT '✗ paymentaudits: Colunas de soft delete NÃO existem'

-- paymentitems
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'paymentitems') AND name = 'IsDeleted')
    PRINT '✓ paymentitems: Colunas de soft delete existem'
ELSE
    PRINT '✗ paymentitems: Colunas de soft delete NÃO existem'

-- fiscalreceipts
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'fiscalreceipts') AND name = 'IsDeleted')
    PRINT '✓ fiscalreceipts: Colunas de soft delete existem'
ELSE
    PRINT '✗ fiscalreceipts: Colunas de soft delete NÃO existem'

-- stockmovement
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'stockmovement') AND name = 'IsDeleted')
    PRINT '✓ stockmovement: Colunas de soft delete existem'
ELSE
    PRINT '✗ stockmovement: Colunas de soft delete NÃO existem'

-- cupom
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'cupom') AND name = 'IsDeleted')
    PRINT '✓ cupom: Colunas de soft delete existem'
ELSE
    PRINT '✗ cupom: Colunas de soft delete NÃO existem'

PRINT '';
PRINT '2. Verificando registros não deletados por tabela:';
PRINT '';

-- Contar registros não deletados em cada tabela
DECLARE @sql NVARCHAR(MAX);
DECLARE @tableName NVARCHAR(128);
DECLARE @count INT;

-- Lista de tabelas para verificar
DECLARE table_cursor CURSOR FOR
SELECT name FROM sys.tables 
WHERE name IN ('categoria', 'produto', 'cliente', 'colaborador', 'departamento', 
               'fornecedor', 'usuario', 'pedido', 'produtopedido', 'role', 
               'permission', 'userrole', 'rolepermission', 'usersessions', 
               'auditlog', 'payments', 'paymentaudits', 'paymentitems', 
               'fiscalreceipts', 'stockmovement', 'cupom')
ORDER BY name;

OPEN table_cursor;
FETCH NEXT FROM table_cursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Verificar se a tabela tem a coluna IsDeleted
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(@tableName) AND name = 'IsDeleted')
    BEGIN
        SET @sql = 'SELECT @count = COUNT(*) FROM ' + @tableName + ' WHERE IsDeleted = 0';
        EXEC sp_executesql @sql, N'@count INT OUTPUT', @count OUTPUT;
        PRINT @tableName + ': ' + CAST(@count AS VARCHAR(10)) + ' registros ativos (não deletados)';
    END
    ELSE
    BEGIN
        SET @sql = 'SELECT @count = COUNT(*) FROM ' + @tableName;
        EXEC sp_executesql @sql, N'@count INT OUTPUT', @count OUTPUT;
        PRINT @tableName + ': ' + CAST(@count AS VARCHAR(10)) + ' registros totais (sem soft delete)';
    END
    
    FETCH NEXT FROM table_cursor INTO @tableName;
END

CLOSE table_cursor;
DEALLOCATE table_cursor;

PRINT '';
PRINT '=== VERIFICAÇÃO CONCLUÍDA ===';