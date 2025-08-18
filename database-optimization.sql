-- Database Optimization Script for PDV System
-- This script adds missing indexes and optimizes database performance

-- ============================================
-- PERFORMANCE INDEXES
-- ============================================

-- User authentication and authorization indexes
CREATE INDEX IF NOT EXISTS IX_Usuario_Username ON Usuario(Username);
CREATE INDEX IF NOT EXISTS IX_Usuario_Email ON Usuario(Email);
CREATE INDEX IF NOT EXISTS IX_Usuario_IsActive ON Usuario(IsActive);
CREATE INDEX IF NOT EXISTS IX_Usuario_LastLoginAt ON Usuario(LastLoginAt);

-- User sessions for concurrent session management
CREATE INDEX IF NOT EXISTS IX_UserSession_UserId ON UserSession(UserId);
CREATE INDEX IF NOT EXISTS IX_UserSession_IsActive ON UserSession(IsActive);
CREATE INDEX IF NOT EXISTS IX_UserSession_ExpiresAt ON UserSession(ExpiresAt);
CREATE INDEX IF NOT EXISTS IX_UserSession_CreatedAt ON UserSession(CreatedAt);

-- Role and permission indexes
CREATE INDEX IF NOT EXISTS IX_UserRole_UserId ON UserRole(UserId);
CREATE INDEX IF NOT EXISTS IX_UserRole_RoleId ON UserRole(RoleId);
CREATE INDEX IF NOT EXISTS IX_RolePermission_RoleId ON RolePermission(RoleId);
CREATE INDEX IF NOT EXISTS IX_RolePermission_PermissionId ON RolePermission(PermissionId);

-- Product search and inventory indexes
CREATE INDEX IF NOT EXISTS IX_Produto_CodBarras ON Produto(CodBarras);
CREATE INDEX IF NOT EXISTS IX_Produto_NomeProduto ON Produto(NomeProduto);
CREATE INDEX IF NOT EXISTS IX_Produto_FornecedorId ON Produto(FornecedorId);
CREATE INDEX IF NOT EXISTS IX_Produto_CategoriaId ON Produto(CategoriaId);
CREATE INDEX IF NOT EXISTS IX_Produto_IsDeleted ON Produto(IsDeleted);
CREATE INDEX IF NOT EXISTS IX_Produto_CreatedAt ON Produto(CreatedAt);

-- Stock movement tracking indexes
CREATE INDEX IF NOT EXISTS IX_StockMovement_ProductId ON StockMovement(ProductId);
CREATE INDEX IF NOT EXISTS IX_StockMovement_Type ON StockMovement(Type);
CREATE INDEX IF NOT EXISTS IX_StockMovement_CreatedAt ON StockMovement(CreatedAt);
CREATE INDEX IF NOT EXISTS IX_StockMovement_CreatedBy ON StockMovement(CreatedBy);

-- Order and sales indexes
CREATE INDEX IF NOT EXISTS IX_Pedido_ClienteId ON Pedido(ClienteId);
CREATE INDEX IF NOT EXISTS IX_Pedido_ColaboradorId ON Pedido(ColaboradorId);
CREATE INDEX IF NOT EXISTS IX_Pedido_DataPedido ON Pedido(DataPedido);
CREATE INDEX IF NOT EXISTS IX_Pedido_StatusPedido ON Pedido(StatusPedido);
CREATE INDEX IF NOT EXISTS IX_Pedido_IsDeleted ON Pedido(IsDeleted);

-- Product order items indexes
CREATE INDEX IF NOT EXISTS IX_ProdutoPedido_PedidoId ON ProdutoPedido(PedidoId);
CREATE INDEX IF NOT EXISTS IX_ProdutoPedido_ProdutoId ON ProdutoPedido(ProdutoId);

-- Customer search indexes
CREATE INDEX IF NOT EXISTS IX_Cliente_CpfCnpj ON Cliente(CpfCnpj);
CREATE INDEX IF NOT EXISTS IX_Cliente_TipoCliente ON Cliente(TipoCliente);
CREATE INDEX IF NOT EXISTS IX_Cliente_IsDeleted ON Cliente(IsDeleted);

-- Employee indexes
CREATE INDEX IF NOT EXISTS IX_Colaborador_CpfColaborador ON Colaborador(CpfColaborador);
CREATE INDEX IF NOT EXISTS IX_Colaborador_DepartamentoId ON Colaborador(DepartamentoId);
CREATE INDEX IF NOT EXISTS IX_Colaborador_IsDeleted ON Colaborador(IsDeleted);

-- Supplier indexes
CREATE INDEX IF NOT EXISTS IX_Fornecedor_Cnpj ON Fornecedor(Cnpj);
CREATE INDEX IF NOT EXISTS IX_Fornecedor_NomeFantasia ON Fornecedor(NomeFantasia);
CREATE INDEX IF NOT EXISTS IX_Fornecedor_IsDeleted ON Fornecedor(IsDeleted);

-- Category and department indexes
CREATE INDEX IF NOT EXISTS IX_Categoria_NomeCategoria ON Categoria(NomeCategoria);
CREATE INDEX IF NOT EXISTS IX_Categoria_IsDeleted ON Categoria(IsDeleted);
CREATE INDEX IF NOT EXISTS IX_Departamento_NomeDepartamento ON Departamento(NomeDepartamento);
CREATE INDEX IF NOT EXISTS IX_Departamento_IsDeleted ON Departamento(IsDeleted);

-- Payment system indexes
CREATE INDEX IF NOT EXISTS IX_Payment_OrderId ON Payment(OrderId);
CREATE INDEX IF NOT EXISTS IX_Payment_Status ON Payment(Status);
CREATE INDEX IF NOT EXISTS IX_Payment_PaymentMethod ON Payment(PaymentMethod);
CREATE INDEX IF NOT EXISTS IX_Payment_CreatedAt ON Payment(CreatedAt);
CREATE INDEX IF NOT EXISTS IX_Payment_ProcessedAt ON Payment(ProcessedAt);

-- Payment audit trail indexes
CREATE INDEX IF NOT EXISTS IX_PaymentAudit_PaymentId ON PaymentAudit(PaymentId);
CREATE INDEX IF NOT EXISTS IX_PaymentAudit_Action ON PaymentAudit(Action);
CREATE INDEX IF NOT EXISTS IX_PaymentAudit_UserId ON PaymentAudit(UserId);
CREATE INDEX IF NOT EXISTS IX_PaymentAudit_Timestamp ON PaymentAudit(Timestamp);

-- Fiscal receipt indexes
CREATE INDEX IF NOT EXISTS IX_FiscalReceipt_PaymentId ON FiscalReceipt(PaymentId);
CREATE INDEX IF NOT EXISTS IX_FiscalReceipt_ReceiptNumber ON FiscalReceipt(ReceiptNumber);
CREATE INDEX IF NOT EXISTS IX_FiscalReceipt_SerialNumber ON FiscalReceipt(SerialNumber);
CREATE INDEX IF NOT EXISTS IX_FiscalReceipt_IssuedAt ON FiscalReceipt(IssuedAt);

-- Audit log indexes for system monitoring
CREATE INDEX IF NOT EXISTS IX_AuditLog_EntityName ON AuditLog(EntityName);
CREATE INDEX IF NOT EXISTS IX_AuditLog_EntityId ON AuditLog(EntityId);
CREATE INDEX IF NOT EXISTS IX_AuditLog_Action ON AuditLog(Action);
CREATE INDEX IF NOT EXISTS IX_AuditLog_UserId ON AuditLog(UserId);
CREATE INDEX IF NOT EXISTS IX_AuditLog_Timestamp ON AuditLog(Timestamp);

-- ============================================
-- COMPOSITE INDEXES FOR COMPLEX QUERIES
-- ============================================

-- User authentication composite index
CREATE INDEX IF NOT EXISTS IX_Usuario_Username_IsActive ON Usuario(Username, IsActive);

-- Product search composite indexes
CREATE INDEX IF NOT EXISTS IX_Produto_Categoria_Fornecedor ON Produto(CategoriaId, FornecedorId);
CREATE INDEX IF NOT EXISTS IX_Produto_Nome_Categoria ON Produto(NomeProduto, CategoriaId);

-- Sales reporting composite indexes
CREATE INDEX IF NOT EXISTS IX_Pedido_Data_Status ON Pedido(DataPedido, StatusPedido);
CREATE INDEX IF NOT EXISTS IX_Pedido_Cliente_Data ON Pedido(ClienteId, DataPedido);
CREATE INDEX IF NOT EXISTS IX_Pedido_Colaborador_Data ON Pedido(ColaboradorId, DataPedido);

-- Stock movement reporting composite indexes
CREATE INDEX IF NOT EXISTS IX_StockMovement_Product_Date ON StockMovement(ProductId, CreatedAt);
CREATE INDEX IF NOT EXISTS IX_StockMovement_Type_Date ON StockMovement(Type, CreatedAt);

-- Payment processing composite indexes
CREATE INDEX IF NOT EXISTS IX_Payment_Status_Method ON Payment(Status, PaymentMethod);
CREATE INDEX IF NOT EXISTS IX_Payment_Order_Status ON Payment(OrderId, Status);

-- ============================================
-- QUERY OPTIMIZATION HINTS
-- ============================================

-- Analyze table statistics for query optimizer
ANALYZE TABLE Usuario;
ANALYZE TABLE UserSession;
ANALYZE TABLE UserRole;
ANALYZE TABLE Role;
ANALYZE TABLE Permission;
ANALYZE TABLE RolePermission;
ANALYZE TABLE Produto;
ANALYZE TABLE StockMovement;
ANALYZE TABLE Pedido;
ANALYZE TABLE ProdutoPedido;
ANALYZE TABLE Cliente;
ANALYZE TABLE Colaborador;
ANALYZE TABLE Fornecedor;
ANALYZE TABLE Categoria;
ANALYZE TABLE Departamento;
ANALYZE TABLE Payment;
ANALYZE TABLE PaymentAudit;
ANALYZE TABLE FiscalReceipt;
ANALYZE TABLE AuditLog;

-- ============================================
-- PERFORMANCE MONITORING VIEWS
-- ============================================

-- View for monitoring slow queries
CREATE OR REPLACE VIEW v_slow_queries AS
SELECT 
    DIGEST_TEXT as query_text,
    COUNT_STAR as exec_count,
    AVG_TIMER_WAIT/1000000000 as avg_time_seconds,
    MAX_TIMER_WAIT/1000000000 as max_time_seconds,
    SUM_ROWS_EXAMINED as total_rows_examined,
    SUM_ROWS_SENT as total_rows_sent
FROM performance_schema.events_statements_summary_by_digest
WHERE AVG_TIMER_WAIT > 1000000000  -- Queries taking more than 1 second
ORDER BY AVG_TIMER_WAIT DESC
LIMIT 20;

-- View for monitoring index usage
CREATE OR REPLACE VIEW v_index_usage AS
SELECT 
    OBJECT_SCHEMA as database_name,
    OBJECT_NAME as table_name,
    INDEX_NAME as index_name,
    COUNT_FETCH as times_used,
    COUNT_INSERT as inserts,
    COUNT_UPDATE as updates,
    COUNT_DELETE as deletes
FROM performance_schema.table_io_waits_summary_by_index_usage
WHERE OBJECT_SCHEMA = DATABASE()
ORDER BY COUNT_FETCH DESC;

-- View for monitoring table access patterns
CREATE OR REPLACE VIEW v_table_access AS
SELECT 
    OBJECT_NAME as table_name,
    COUNT_READ as read_operations,
    COUNT_WRITE as write_operations,
    COUNT_FETCH as fetch_operations,
    SUM_TIMER_FETCH/1000000000 as total_fetch_time_seconds
FROM performance_schema.table_io_waits_summary_by_table
WHERE OBJECT_SCHEMA = DATABASE()
ORDER BY COUNT_FETCH DESC;

-- ============================================
-- MAINTENANCE PROCEDURES
-- ============================================

-- Procedure to update table statistics
DELIMITER //
CREATE OR REPLACE PROCEDURE UpdateTableStatistics()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE table_name VARCHAR(255);
    DECLARE cur CURSOR FOR 
        SELECT TABLE_NAME 
        FROM information_schema.TABLES 
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_TYPE = 'BASE TABLE';
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO table_name;
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        SET @sql = CONCAT('ANALYZE TABLE ', table_name);
        PREPARE stmt FROM @sql;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
    END LOOP;
    CLOSE cur;
END//
DELIMITER ;

-- Procedure to clean up old audit logs (keep last 6 months)
DELIMITER //
CREATE OR REPLACE PROCEDURE CleanupOldAuditLogs()
BEGIN
    DELETE FROM AuditLog 
    WHERE Timestamp < DATE_SUB(NOW(), INTERVAL 6 MONTH);
    
    DELETE FROM PaymentAudit 
    WHERE Timestamp < DATE_SUB(NOW(), INTERVAL 6 MONTH);
    
    -- Clean up old user sessions
    DELETE FROM UserSession 
    WHERE ExpiresAt < DATE_SUB(NOW(), INTERVAL 30 DAY);
END//
DELIMITER ;

-- ============================================
-- PERFORMANCE CONFIGURATION RECOMMENDATIONS
-- ============================================

-- These settings should be applied to MySQL configuration (my.cnf)
-- Uncomment and adjust based on your server specifications

/*
# InnoDB Buffer Pool (set to 70-80% of available RAM)
innodb_buffer_pool_size = 2G

# Query Optimization (for read-heavy workloads)
innodb_log_file_size = 256M
innodb_log_buffer_size = 64M

# Connection settings
max_connections = 200
max_connect_errors = 1000000

# InnoDB settings for better performance
innodb_log_file_size = 256M
innodb_log_buffer_size = 16M
innodb_flush_log_at_trx_commit = 2
innodb_file_per_table = 1

# MyISAM settings (if using MyISAM tables)
key_buffer_size = 128M

# General performance settings
tmp_table_size = 64M
max_heap_table_size = 64M
sort_buffer_size = 2M
read_buffer_size = 2M
read_rnd_buffer_size = 8M

# Slow query log for monitoring
slow_query_log = 1
slow_query_log_file = /var/log/mysql/slow.log
long_query_time = 2
log_queries_not_using_indexes = 1
*/

-- ============================================
-- COMPLETION MESSAGE
-- ============================================

SELECT 'Database optimization completed successfully!' as Status,
       'Indexes created, statistics updated, monitoring views added' as Details,
       NOW() as CompletedAt;