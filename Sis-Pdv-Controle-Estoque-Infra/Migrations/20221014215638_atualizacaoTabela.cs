using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sis_Pdv_Controle_Estoque_Infra.Migrations
{
    public partial class atualizacaoTabela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colaborador_Departamento_DepartamentoId",
                table: "Colaborador");

            migrationBuilder.AddColumn<string>(
                name: "NomeProduto",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PrecoUnit",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusAtivo",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "codItem",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "codigoBarras",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "novaDescricao",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "quantidade",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "total",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "valorUnitario",
                table: "ProdutoPedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartamentoId",
                table: "Colaborador",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Colaborador_Departamento_DepartamentoId",
                table: "Colaborador",
                column: "DepartamentoId",
                principalTable: "Departamento",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colaborador_Departamento_DepartamentoId",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "NomeProduto",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "PrecoUnit",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "StatusAtivo",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "codItem",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "codigoBarras",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "novaDescricao",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "quantidade",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "total",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "valorUnitario",
                table: "ProdutoPedido");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartamentoId",
                table: "Colaborador",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Colaborador_Departamento_DepartamentoId",
                table: "Colaborador",
                column: "DepartamentoId",
                principalTable: "Departamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
