-- Manual SQL script to add audit fields and user management entities
-- This script should be run if the EF migration fails

-- Add audit fields to existing tables
ALTER TABLE `Usuario` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `Email` varchar(255) CHARACTER SET utf8mb4 NULL,
ADD COLUMN `Nome` varchar(255) CHARACTER SET utf8mb4 NULL,
ADD COLUMN `LastLoginAt` datetime(6) NULL,
ADD COLUMN `RefreshToken` varchar(500) CHARACTER SET utf8mb4 NULL,
ADD COLUMN `RefreshTokenExpiryTime` datetime(6) NULL;

-- Modify existing columns
ALTER TABLE `Usuario` MODIFY COLUMN `Senha` varchar(255) CHARACTER SET utf8mb4 NOT NULL;

-- Add audit fields to other existing tables
ALTER TABLE `Produto` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Cliente` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Fornecedor` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Categoria` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Departamento` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Colaborador` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Pedido` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `ProdutoPedido` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `Cupom` 
ADD COLUMN `CreatedAt` datetime(6) NOT NULL DEFAULT '2024-08-16 00:00:00',
ADD COLUMN `UpdatedAt` datetime(6) NULL,
ADD COLUMN `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
ADD COLUMN `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
ADD COLUMN `DeletedAt` datetime(6) NULL,
ADD COLUMN `DeletedBy` char(36) COLLATE ascii_general_ci NULL;

-- Create new user management tables
CREATE TABLE `Role` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
    `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    `DeletedAt` datetime(6) NULL,
    `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_Role` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Permission` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `Resource` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Action` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
    `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    `DeletedAt` datetime(6) NULL,
    `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_Permission` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `UserRole` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
    `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    `DeletedAt` datetime(6) NULL,
    `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_UserRole` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserRole_Usuario_UserId` FOREIGN KEY (`UserId`) REFERENCES `Usuario` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserRole_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `RolePermission` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    `PermissionId` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
    `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    `DeletedAt` datetime(6) NULL,
    `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_RolePermission` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RolePermission_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RolePermission_Permission_PermissionId` FOREIGN KEY (`PermissionId`) REFERENCES `Permission` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AuditLog` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `EntityName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `EntityId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Action` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Changes` TEXT CHARACTER SET utf8mb4 NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Timestamp` datetime(6) NOT NULL,
    `OldValues` TEXT CHARACTER SET utf8mb4 NULL,
    `NewValues` TEXT CHARACTER SET utf8mb4 NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CreatedBy` char(36) COLLATE ascii_general_ci NULL,
    `UpdatedBy` char(36) COLLATE ascii_general_ci NULL,
    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
    `DeletedAt` datetime(6) NULL,
    `DeletedBy` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_AuditLog` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AuditLog_Usuario_UserId` FOREIGN KEY (`UserId`) REFERENCES `Usuario` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

-- Create indexes
CREATE INDEX `IX_Usuario_Login` ON `Usuario` (`Login`);
CREATE INDEX `IX_Usuario_RefreshToken` ON `Usuario` (`RefreshToken`);
CREATE UNIQUE INDEX `IX_Permission_Name` ON `Permission` (`Name`);
CREATE INDEX `IX_Permission_Resource_Action` ON `Permission` (`Resource`, `Action`);
CREATE UNIQUE INDEX `IX_Role_Name` ON `Role` (`Name`);
CREATE INDEX `IX_UserRole_UserId` ON `UserRole` (`UserId`);
CREATE INDEX `IX_UserRole_RoleId` ON `UserRole` (`RoleId`);
CREATE INDEX `IX_RolePermission_RoleId` ON `RolePermission` (`RoleId`);
CREATE INDEX `IX_RolePermission_PermissionId` ON `RolePermission` (`PermissionId`);
CREATE INDEX `IX_AuditLog_EntityName_EntityId` ON `AuditLog` (`EntityName`, `EntityId`);
CREATE INDEX `IX_AuditLog_UserId` ON `AuditLog` (`UserId`);
CREATE INDEX `IX_AuditLog_Timestamp` ON `AuditLog` (`Timestamp`);