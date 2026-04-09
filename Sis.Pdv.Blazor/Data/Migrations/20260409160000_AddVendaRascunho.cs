using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sis.Pdv.Blazor.Data.Migrations;

public partial class AddVendaRascunho : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "VendasRascunho",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false),
                ColaboradorId = table.Column<Guid>(type: "char(36)", nullable: false),
                NomeOperador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                CpfCnpjCliente = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                DataAbertura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                UltimaAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                ItensJson = table.Column<string>(type: "longtext", nullable: false),
                NumeroCaixa = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VendasRascunho", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "VendasRascunho");
    }
}
