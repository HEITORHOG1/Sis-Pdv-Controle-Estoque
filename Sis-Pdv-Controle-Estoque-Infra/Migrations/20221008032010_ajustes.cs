using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sis_Pdv_Controle_Estoque_Infra.Migrations
{
    public partial class ajustes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Fornecedor_FornecedorId",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Fornecedor",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ufPessoa",
                table: "Fornecedor",
                newName: "Uf");

            migrationBuilder.RenameColumn(
                name: "numeroPessoa",
                table: "Fornecedor",
                newName: "Numero");

            migrationBuilder.RenameColumn(
                name: "complementoPessoa",
                table: "Fornecedor",
                newName: "Complemento");

            migrationBuilder.RenameColumn(
                name: "cidadePessoa",
                table: "Fornecedor",
                newName: "Cidade");

            migrationBuilder.RenameColumn(
                name: "bairroPessoa",
                table: "Fornecedor",
                newName: "Bairro");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Fornecedor",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Fornecedor_FornecedorId",
                table: "Produto",
                column: "FornecedorId",
                principalTable: "Fornecedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Fornecedor_FornecedorId",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Fornecedor");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Fornecedor",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Uf",
                table: "Fornecedor",
                newName: "ufPessoa");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "Fornecedor",
                newName: "numeroPessoa");

            migrationBuilder.RenameColumn(
                name: "Complemento",
                table: "Fornecedor",
                newName: "complementoPessoa");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Fornecedor",
                newName: "cidadePessoa");

            migrationBuilder.RenameColumn(
                name: "Bairro",
                table: "Fornecedor",
                newName: "bairroPessoa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Fornecedor_FornecedorId",
                table: "Produto",
                column: "FornecedorId",
                principalTable: "Fornecedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
