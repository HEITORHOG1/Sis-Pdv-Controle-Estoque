-- Script simples para corrigir o banco de dados

-- Adicionar colunas faltantes na tabela Produto
ALTER TABLE Produto ADD COLUMN IF NOT EXISTS MinimumStock DECIMAL(18,2) DEFAULT 0;
ALTER TABLE Produto ADD COLUMN IF NOT EXISTS MaximumStock DECIMAL(18,2) DEFAULT 0;
ALTER TABLE Produto ADD COLUMN IF NOT EXISTS ReorderPoint DECIMAL(18,2) DEFAULT 0;
ALTER TABLE Produto ADD COLUMN IF NOT EXISTS Location VARCHAR(100) NULL;

-- Criar tabela StockMovement se não existir
CREATE TABLE IF NOT EXISTS StockMovement (
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
);

-- Verificar estrutura final
DESCRIBE Produto;
DESCRIBE StockMovement;