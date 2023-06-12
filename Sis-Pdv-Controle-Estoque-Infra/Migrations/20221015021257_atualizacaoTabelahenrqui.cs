using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sis_Pdv_Controle_Estoque_Infra.Migrations
{
    public partial class atualizacaoTabelahenrqui : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
