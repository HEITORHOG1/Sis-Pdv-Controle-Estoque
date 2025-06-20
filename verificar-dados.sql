-- Script para verificar os dados inseridos via migration

-- Verificar Categorias
SELECT 'Categorias' as Tabela, COUNT(*) as Total FROM Categoria;
SELECT * FROM Categoria;

-- Verificar Departamentos
SELECT 'Departamentos' as Tabela, COUNT(*) as Total FROM Departamento;
SELECT * FROM Departamento;

-- Verificar Usuários
SELECT 'Usuários' as Tabela, COUNT(*) as Total FROM Usuario;
SELECT * FROM Usuario;

-- Verificar Fornecedores
SELECT 'Fornecedores' as Tabela, COUNT(*) as Total FROM Fornecedor;
SELECT * FROM Fornecedor;

-- Verificar Clientes
SELECT 'Clientes' as Tabela, COUNT(*) as Total FROM Cliente;
SELECT * FROM Cliente;

-- Verificar Colaboradores
SELECT 'Colaboradores' as Tabela, COUNT(*) as Total FROM Colaborador;
SELECT * FROM Colaborador;

-- Verificar Produtos
SELECT 'Produtos' as Tabela, COUNT(*) as Total FROM Produto;
SELECT * FROM Produto;

-- Verificar relacionamentos
SELECT 
    c.NomeColaborador,
    d.NomeDepartamento,
    u.Login
FROM Colaborador c
JOIN Departamento d ON c.DepartamentoId = d.Id
JOIN Usuario u ON c.UsuarioId = u.Id;

SELECT 
    p.NomeProduto,
    cat.NomeCategoria,
    f.NomeFantasia as Fornecedor
FROM Produto p
JOIN Categoria cat ON p.CategoriaId = cat.Id
JOIN Fornecedor f ON p.FornecedorId = f.Id;
