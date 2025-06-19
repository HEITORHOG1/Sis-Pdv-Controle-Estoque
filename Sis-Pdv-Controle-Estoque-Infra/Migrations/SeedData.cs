using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sis_Pdv_Controle_Estoque_Infra.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "NomeCategoria" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Geral" });

            migrationBuilder.InsertData(
                table: "Departamento",
                columns: new[] { "Id", "NomeDepartamento" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Administrativo" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Login", "Senha", "statusAtivo" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "admin", "123456", true });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "CpfCnpj", "tipoCliente" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), "00000000000", "FISICA" });

            migrationBuilder.InsertData(
                table: "Fornecedor",
                columns: new[] { "Id", "Bairro", "Cidade", "Cnpj", "Complemento", "Numero", "Rua", "Uf", "cepFornecedor", "inscricaoEstadual", "nomeFantasia", "statusAtivo" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), "Centro", "Sao Paulo", "12345678000100", "Sala 1", "100", "Rua A", "SP", 12345678, "12345", "Fornecedor Teste", 1 });

            migrationBuilder.InsertData(
                table: "Colaborador",
                columns: new[] { "Id", "DepartamentoId", "UsuarioId", "cargoColaborador", "cpfColaborador", "emailCorporativo", "emailPessoalColaborador", "nomeColaborador", "telefoneColaborador" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("33333333-3333-3333-3333-333333333333"), "Gerente", "00000000000", "gerente@empresa.com", "gerente@gmail.com", "Colaborador Admin", "11999999999" });

            migrationBuilder.InsertData(
                table: "Produto",
                columns: new[] { "Id", "CategoriaId", "FornecedorId", "codBarras", "dataFabricao", "dataVencimento", "descricaoProduto", "margemLucro", "nomeProduto", "precoCusto", "precoVenda", "quatidadeEstoqueProduto", "statusAtivo" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("55555555-5555-5555-5555-555555555555"), "1234567890123", DateTime.UtcNow, DateTime.UtcNow.AddYears(1), "Produto de teste", 0.2m, "Produto Teste", 10m, 12m, 100, 1 });

            migrationBuilder.InsertData(
                table: "Pedido",
                columns: new[] { "Id", "ClienteId", "ColaboradorId", "Status", "dataDoPedido", "formaPagamento", "totalPedido" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("66666666-6666-6666-6666-666666666666"), 1, DateTime.UtcNow, "Dinheiro", 12m });

            migrationBuilder.InsertData(
                table: "ProdutoPedido",
                columns: new[] { "Id", "PedidoId", "ProdutoId", "codBarras", "quantidadeItemPedido", "totalProdutoPedido" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("88888888-8888-8888-8888-888888888888"), new Guid("77777777-7777-7777-7777-777777777777"), "1234567890123", 1, 12m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProdutoPedido",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Pedido",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Produto",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Colaborador",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Fornecedor",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Departamento",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
