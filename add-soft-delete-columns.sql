-- Script para adicionar colunas de soft delete nas tabelas que não possuem
-- Execute este script ANTES do fix-soft-delete-migration.sql

-- Função para verificar se coluna existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'categoria') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE categoria ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE categoria ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE categoria ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela categoria';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'produto') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE produto ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE produto ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE produto ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela produto';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'cliente') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE cliente ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE cliente ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE cliente ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela cliente';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'colaborador') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE colaborador ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE colaborador ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE colaborador ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela colaborador';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'departamento') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE departamento ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE departamento ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE departamento ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela departamento';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'fornecedor') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE fornecedor ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE fornecedor ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE fornecedor ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela fornecedor';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'usuario') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE usuario ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE usuario ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE usuario ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela usuario';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'pedido') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE pedido ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE pedido ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE pedido ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela pedido';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'produtopedido') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE produtopedido ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE produtopedido ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE produtopedido ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela produtopedido';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'role') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE role ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE role ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE role ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela role';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'permission') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE permission ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE permission ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE permission ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela permission';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'userrole') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE userrole ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE userrole ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE userrole ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela userrole';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'rolepermission') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE rolepermission ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE rolepermission ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE rolepermission ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela rolepermission';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'usersessions') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE usersessions ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE usersessions ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE usersessions ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela usersessions';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'auditlog') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE auditlog ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE auditlog ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE auditlog ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela auditlog';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'payments') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE payments ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE payments ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE payments ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela payments';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'paymentaudits') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE paymentaudits ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE paymentaudits ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE paymentaudits ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela paymentaudits';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'paymentitems') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE paymentitems ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE paymentitems ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE paymentitems ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela paymentitems';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'fiscalreceipts') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE fiscalreceipts ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE fiscalreceipts ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE fiscalreceipts ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela fiscalreceipts';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'stockmovement') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE stockmovement ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE stockmovement ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE stockmovement ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela stockmovement';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'cupom') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE cupom ADD IsDeleted BIT NOT NULL DEFAULT 0;
    ALTER TABLE cupom ADD DeletedAt DATETIME2 NULL;
    ALTER TABLE cupom ADD DeletedBy INT NULL;
    PRINT 'Colunas de soft delete adicionadas na tabela cupom';
END

PRINT 'Script de adição de colunas de soft delete concluído!';