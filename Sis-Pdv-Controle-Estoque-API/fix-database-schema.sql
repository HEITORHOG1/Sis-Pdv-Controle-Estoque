-- Script para corrigir problemas do banco de dados PDV_02

-- 1. Verificar se as colunas existem na tabela Produto
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto';

-- 2. Adicionar colunas faltantes na tabela Produto (se não existirem)
SET @sql = '';

-- Verificar e adicionar MinimumStock
SELECT COUNT(*) INTO @count 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto' 
  AND COLUMN_NAME = 'MinimumStock';

SET @sql = IF(@count = 0, 
    'ALTER TABLE Produto ADD COLUMN MinimumStock DECIMAL(18,2) DEFAULT 0;', 
    '');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificar e adicionar MaximumStock
SELECT COUNT(*) INTO @count 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto' 
  AND COLUMN_NAME = 'MaximumStock';

SET @sql = IF(@count = 0, 
    'ALTER TABLE Produto ADD COLUMN MaximumStock DECIMAL(18,2) DEFAULT 0;', 
    '');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificar e adicionar ReorderPoint
SELECT COUNT(*) INTO @count 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto' 
  AND COLUMN_NAME = 'ReorderPoint';

SET @sql = IF(@count = 0, 
    'ALTER TABLE Produto ADD COLUMN ReorderPoint DECIMAL(18,2) DEFAULT 0;', 
    '');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificar e adicionar Location
SELECT COUNT(*) INTO @count 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto' 
  AND COLUMN_NAME = 'Location';

SET @sql = IF(@count = 0, 
    'ALTER TABLE Produto ADD COLUMN Location VARCHAR(100) NULL;', 
    '');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3. Verificar se a tabela StockMovement existe
SELECT COUNT(*) INTO @table_count 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'StockMovement';

-- 4. Criar tabela StockMovement se não existir
SET @sql = IF(@table_count = 0, 
    'CREATE TABLE StockMovement (
        Id CHAR(36) NOT NULL PRIMARY KEY,
        ProdutoId CHAR(36) NOT NULL,
        Quantity DECIMAL(18,2) NOT NULL,
        Type INT NOT NULL,
        Reason VARCHAR(500) NOT NULL,
        UnitCost DECIMAL(18,2) NOT NULL,
        PreviousStock DECIMAL(18,2) NOT NULL,
        NewStock DECIMAL(18,2) NOT NULL,
        MovementDate DATETIME(6) NOT NULL,
        ReferenceDocument VARCHAR(100) NULL,
        UserId CHAR(36) NULL,
        CreatedAt DATETIME(6) NOT NULL,
        UpdatedAt DATETIME(6) NULL,
        CreatedBy CHAR(36) NULL,
        UpdatedBy CHAR(36) NULL,
        IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
        DeletedAt DATETIME(6) NULL,
        DeletedBy CHAR(36) NULL,
        INDEX IX_StockMovement_ProdutoId (ProdutoId),
        INDEX IX_StockMovement_MovementDate (MovementDate),
        INDEX IX_StockMovement_Type (Type),
        INDEX IX_StockMovement_ProdutoId_MovementDate (ProdutoId, MovementDate),
        CONSTRAINT FK_StockMovement_Produto_ProdutoId 
            FOREIGN KEY (ProdutoId) REFERENCES Produto (Id) ON DELETE RESTRICT
    );', 
    '');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 5. Verificar se precisamos de outras tabelas de autenticação
-- (Role, Permission, UserRole, etc. podem ser adicionadas conforme necessário)

-- 6. Mostrar status final
SELECT 'Produto columns:' as Info;
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'Produto'
  AND COLUMN_NAME IN ('MinimumStock', 'MaximumStock', 'ReorderPoint', 'Location');

SELECT 'StockMovement table exists:' as Info;
SELECT COUNT(*) as TableExists
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'PDV_02' 
  AND TABLE_NAME = 'StockMovement';