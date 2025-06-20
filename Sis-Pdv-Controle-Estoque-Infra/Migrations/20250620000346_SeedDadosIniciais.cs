using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisPdvControleEstoqueInfra.Migrations
{
    /// <inheritdoc />
    public partial class SeedDadosIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir Categorias
            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "NomeCategoria" },
                values: new object[,]
                {
                    { Guid.Parse("11111111-1111-1111-1111-111111111111"), "Eletrônicos" },
                    { Guid.Parse("22222222-2222-2222-2222-222222222222"), "Roupas e Acessórios" },
                    { Guid.Parse("33333333-3333-3333-3333-333333333333"), "Casa e Jardim" },
                    { Guid.Parse("44444444-4444-4444-4444-444444444444"), "Esportes e Lazer" },
                    { Guid.Parse("55555555-5555-5555-5555-555555555555"), "Alimentação" }
                });

            // Inserir Departamentos
            migrationBuilder.InsertData(
                table: "Departamento",
                columns: new[] { "Id", "NomeDepartamento" },
                values: new object[,]
                {
                    { Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Administração" },
                    { Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Vendas" },
                    { Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Estoque" },
                    { Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Financeiro" },
                    { Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Recursos Humanos" }
                });

            // Inserir Usuários
            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Login", "Senha", "StatusAtivo" },
                values: new object[,]
                {
                    { Guid.Parse("10000000-0000-0000-0000-000000000001"), "admin", "123456", true },
                    { Guid.Parse("10000000-0000-0000-0000-000000000002"), "vendedor1", "123456", true },
                    { Guid.Parse("10000000-0000-0000-0000-000000000003"), "estoquista1", "123456", true },
                    { Guid.Parse("10000000-0000-0000-0000-000000000004"), "financeiro1", "123456", true },
                    { Guid.Parse("10000000-0000-0000-0000-000000000005"), "rh1", "123456", true }
                });            // Inserir Fornecedores
            migrationBuilder.InsertData(
                table: "Fornecedor",
                columns: new[] { "Id", "InscricaoEstadual", "NomeFantasia", "Uf", "Numero", "Complemento", "Bairro", "Cidade", "CepFornecedor", "StatusAtivo", "Cnpj", "Rua" },
                values: new object[,]
                {
                    { Guid.Parse("f0000000-0000-0000-0000-000000000001"), "123456789", "TechSupply Ltda", "SP", "100", "Sala 201", "Centro", "São Paulo", 01000000, 1, "12345678000190", "Rua das Flores" },
                    { Guid.Parse("f0000000-0000-0000-0000-000000000002"), "987654321", "Fashion World", "RJ", "200", "", "Copacabana", "Rio de Janeiro", 22000000, 1, "98765432000110", "Av. Atlântica" },
                    { Guid.Parse("f0000000-0000-0000-0000-000000000003"), "456789123", "Casa & Cia", "MG", "300", "Loja A", "Savassi", "Belo Horizonte", 30000000, 1, "45678912000130", "Rua da Bahia" }
                });// Inserir Clientes
            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "CpfCnpj", "TipoCliente" },
                values: new object[,]
                {
                    { Guid.Parse("c0000000-0000-0000-0000-000000000001"), "12345678900", "Pessoa Física" },
                    { Guid.Parse("c0000000-0000-0000-0000-000000000002"), "98765432111", "Pessoa Física" },
                    { Guid.Parse("c0000000-0000-0000-0000-000000000003"), "12345678000190", "Pessoa Jurídica" },
                    { Guid.Parse("c0000000-0000-0000-0000-000000000004"), "98765432000110", "Pessoa Jurídica" }
                });// Inserir Colaboradores
            migrationBuilder.InsertData(
                table: "Colaborador",
                columns: new[] { "Id", "NomeColaborador", "DepartamentoId", "CpfColaborador", "CargoColaborador", "TelefoneColaborador", "EmailPessoalColaborador", "EmailCorporativo", "UsuarioId" },
                values: new object[,]
                {
                    { Guid.Parse("20000000-0000-0000-0000-000000000001"), "João Silva", Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "11111111111", "Administrador", "(11) 99999-9999", "joao@email.com", "joao@empresa.com", Guid.Parse("10000000-0000-0000-0000-000000000001") },
                    { Guid.Parse("20000000-0000-0000-0000-000000000002"), "Maria Santos", Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "22222222222", "Vendedora", "(11) 88888-8888", "maria@email.com", "maria@empresa.com", Guid.Parse("10000000-0000-0000-0000-000000000002") },
                    { Guid.Parse("20000000-0000-0000-0000-000000000003"), "Pedro Oliveira", Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "33333333333", "Estoquista", "(11) 77777-7777", "pedro@email.com", "pedro@empresa.com", Guid.Parse("10000000-0000-0000-0000-000000000003") },
                    { Guid.Parse("20000000-0000-0000-0000-000000000004"), "Ana Costa", Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "44444444444", "Analista Financeiro", "(11) 66666-6666", "ana@email.com", "ana@empresa.com", Guid.Parse("10000000-0000-0000-0000-000000000004") },
                    { Guid.Parse("20000000-0000-0000-0000-000000000005"), "Carlos Lima", Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "55555555555", "Analista RH", "(11) 55555-5555", "carlos@email.com", "carlos@empresa.com", Guid.Parse("10000000-0000-0000-0000-000000000005") }
                });

            // Inserir Produtos
            migrationBuilder.InsertData(
                table: "Produto",
                columns: new[] { "Id", "CodBarras", "NomeProduto", "DescricaoProduto", "PrecoCusto", "PrecoVenda", "MargemLucro", "DataFabricao", "DataVencimento", "QuatidadeEstoqueProduto", "FornecedorId", "CategoriaId", "StatusAtivo" },
                values: new object[,]
                {
                    { Guid.Parse("30000000-0000-0000-0000-000000000001"), "7891234567890", "Smartphone XYZ", "Smartphone Android com 128GB", 800.00m, 1200.00m, 50.00m, new DateTime(2024, 1, 15), new DateTime(2026, 1, 15), 50, Guid.Parse("f0000000-0000-0000-0000-000000000001"), Guid.Parse("11111111-1111-1111-1111-111111111111"), 1 },
                    { Guid.Parse("30000000-0000-0000-0000-000000000002"), "7891234567891", "Camiseta Polo", "Camiseta Polo 100% Algodão", 25.00m, 59.90m, 139.60m, new DateTime(2024, 3, 10), new DateTime(2025, 3, 10), 100, Guid.Parse("f0000000-0000-0000-0000-000000000002"), Guid.Parse("22222222-2222-2222-2222-222222222222"), 1 },
                    { Guid.Parse("30000000-0000-0000-0000-000000000003"), "7891234567892", "Jogo de Panelas", "Jogo de panelas antiaderente 5 peças", 120.00m, 299.90m, 149.92m, new DateTime(2024, 2, 20), new DateTime(2029, 2, 20), 25, Guid.Parse("f0000000-0000-0000-0000-000000000003"), Guid.Parse("33333333-3333-3333-3333-333333333333"), 1 },
                    { Guid.Parse("30000000-0000-0000-0000-000000000004"), "7891234567893", "Bola de Futebol", "Bola oficial de futebol profissional", 45.00m, 89.90m, 99.78m, new DateTime(2024, 4, 5), new DateTime(2027, 4, 5), 30, Guid.Parse("f0000000-0000-0000-0000-000000000001"), Guid.Parse("44444444-4444-4444-4444-444444444444"), 1 },
                    { Guid.Parse("30000000-0000-0000-0000-000000000005"), "7891234567894", "Café Especial 500g", "Café torrado e moído especial", 15.00m, 24.90m, 66.00m, new DateTime(2024, 5, 1), new DateTime(2025, 5, 1), 200, Guid.Parse("f0000000-0000-0000-0000-000000000003"), Guid.Parse("55555555-5555-5555-5555-555555555555"), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover dados inseridos na ordem inversa devido às dependências
            migrationBuilder.DeleteData(table: "Produto", keyColumn: "Id", keyValue: Guid.Parse("30000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Produto", keyColumn: "Id", keyValue: Guid.Parse("30000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Produto", keyColumn: "Id", keyValue: Guid.Parse("30000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Produto", keyColumn: "Id", keyValue: Guid.Parse("30000000-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "Produto", keyColumn: "Id", keyValue: Guid.Parse("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(table: "Colaborador", keyColumn: "Id", keyValue: Guid.Parse("20000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Colaborador", keyColumn: "Id", keyValue: Guid.Parse("20000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Colaborador", keyColumn: "Id", keyValue: Guid.Parse("20000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Colaborador", keyColumn: "Id", keyValue: Guid.Parse("20000000-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "Colaborador", keyColumn: "Id", keyValue: Guid.Parse("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(table: "Cliente", keyColumn: "Id", keyValue: Guid.Parse("c0000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Cliente", keyColumn: "Id", keyValue: Guid.Parse("c0000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Cliente", keyColumn: "Id", keyValue: Guid.Parse("c0000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Cliente", keyColumn: "Id", keyValue: Guid.Parse("c0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(table: "Fornecedor", keyColumn: "Id", keyValue: Guid.Parse("f0000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Fornecedor", keyColumn: "Id", keyValue: Guid.Parse("f0000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Fornecedor", keyColumn: "Id", keyValue: Guid.Parse("f0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(table: "Usuario", keyColumn: "Id", keyValue: Guid.Parse("10000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Usuario", keyColumn: "Id", keyValue: Guid.Parse("10000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Usuario", keyColumn: "Id", keyValue: Guid.Parse("10000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Usuario", keyColumn: "Id", keyValue: Guid.Parse("10000000-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "Usuario", keyColumn: "Id", keyValue: Guid.Parse("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(table: "Departamento", keyColumn: "Id", keyValue: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Departamento", keyColumn: "Id", keyValue: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
            migrationBuilder.DeleteData(table: "Departamento", keyColumn: "Id", keyValue: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
            migrationBuilder.DeleteData(table: "Departamento", keyColumn: "Id", keyValue: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
            migrationBuilder.DeleteData(table: "Departamento", keyColumn: "Id", keyValue: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(table: "Categoria", keyColumn: "Id", keyValue: Guid.Parse("11111111-1111-1111-1111-111111111111"));
            migrationBuilder.DeleteData(table: "Categoria", keyColumn: "Id", keyValue: Guid.Parse("22222222-2222-2222-2222-222222222222"));
            migrationBuilder.DeleteData(table: "Categoria", keyColumn: "Id", keyValue: Guid.Parse("33333333-3333-3333-3333-333333333333"));
            migrationBuilder.DeleteData(table: "Categoria", keyColumn: "Id", keyValue: Guid.Parse("44444444-4444-4444-4444-444444444444"));
            migrationBuilder.DeleteData(table: "Categoria", keyColumn: "Id", keyValue: Guid.Parse("55555555-5555-5555-5555-555555555555"));
        }
    }
}
