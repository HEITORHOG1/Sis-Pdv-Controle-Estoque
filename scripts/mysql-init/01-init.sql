-- Initial database setup for PDV System
-- This script runs automatically when the MySQL container is created for the first time

-- Create database for Blazor PDV local data
CREATE DATABASE IF NOT EXISTS pdv_local;

-- Grant permissions to pdvuser on all databases
GRANT ALL PRIVILEGES ON *.* TO 'pdvuser'@'%';
FLUSH PRIVILEGES;

-- The actual tables are created by EF Core migrations on application startup
