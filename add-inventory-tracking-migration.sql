-- Migration script to add inventory tracking features
-- Run this script to add the new inventory management tables and columns

-- Add new columns to Produto table
ALTER TABLE Produto 
ADD COLUMN MinimumStock DECIMAL(18,2) DEFAULT 0,
ADD COLUMN MaximumStock DECIMAL(18,2) DEFAULT 0,
ADD COLUMN ReorderPoint DECIMAL(18,2) DEFAULT 0,
ADD COLUMN Location VARCHAR(100) NULL;

-- Create StockMovement table
CREATE TABLE StockMovement (
    Id CHAR(36) NOT NULL PRIMARY KEY,
    ProdutoId CHAR(36) NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    Type INT NOT NULL,
    Reason VARCHAR(500) NOT NULL,
    UnitCost DECIMAL(18,2) NOT NULL,
    PreviousStock DECIMAL(18,2) NOT NULL,
    NewStock DECIMAL(18,2) NOT NULL,
    MovementDate DATETIME NOT NULL,
    ReferenceDocument VARCHAR(100) NULL,
    UserId CHAR(36) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    CreatedBy CHAR(36) NULL,
    UpdatedBy CHAR(36) NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    DeletedAt DATETIME NULL,
    DeletedBy CHAR(36) NULL,
    
    CONSTRAINT FK_StockMovement_Produto FOREIGN KEY (ProdutoId) REFERENCES Produto(Id),
    
    INDEX IX_StockMovement_ProdutoId (ProdutoId),
    INDEX IX_StockMovement_MovementDate (MovementDate),
    INDEX IX_StockMovement_Type (Type),
    INDEX IX_StockMovement_ProdutoId_MovementDate (ProdutoId, MovementDate)
);

-- Add comments to explain the StockMovementType enum values
-- 1 = Entry (Entrada)
-- 2 = Exit (Saída)
-- 3 = Adjustment (Ajuste)
-- 4 = Sale (Venda)
-- 5 = Return (Devolução)
-- 6 = Transfer (Transferência)
-- 7 = Loss (Perda)

-- Create initial stock movements for existing products (optional)
-- This will create an initial entry for each product with their current stock
INSERT INTO StockMovement (
    Id, 
    ProdutoId, 
    Quantity, 
    Type, 
    Reason, 
    UnitCost, 
    PreviousStock, 
    NewStock, 
    MovementDate,
    CreatedAt
)
SELECT 
    UUID() as Id,
    Id as ProdutoId,
    QuatidadeEstoqueProduto as Quantity,
    1 as Type, -- Entry
    'Estoque inicial do sistema' as Reason,
    PrecoCusto as UnitCost,
    0 as PreviousStock,
    QuatidadeEstoqueProduto as NewStock,
    NOW() as MovementDate,
    NOW() as CreatedAt
FROM Produto 
WHERE StatusAtivo = 1 AND QuatidadeEstoqueProduto > 0;

-- Verify the migration
SELECT 'Migration completed successfully' as Status;
SELECT COUNT(*) as ProductsWithInventoryFields FROM Produto WHERE MinimumStock IS NOT NULL;
SELECT COUNT(*) as StockMovementsCreated FROM StockMovement;