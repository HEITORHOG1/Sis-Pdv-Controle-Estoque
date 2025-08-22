using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisPdvControleEstoqueInfra.Migrations
{
    /// <inheritdoc />
    public partial class SchemaReconciliation_AddIsPerecivel_StockMovementDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFabricao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MargemLucro",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PrecoCusto",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PrecoVenda",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "QuatidadeEstoqueProduto",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "ReorderPoint",
                table: "Produto");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataValidade",
                table: "StockMovement",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "StockMovement",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsPerecivel",
                table: "Produto",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InventoryBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    ReservedStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    MaximumStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    Location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBalances_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockMovementDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StockMovementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Lote = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataValidade = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovementDetails_StockMovement_StockMovementId",
                        column: x => x.StockMovementId,
                        principalTable: "StockMovement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_DataValidade",
                table: "StockMovement",
                column: "DataValidade");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_Lote",
                table: "StockMovement",
                column: "Lote");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_LastUpdated",
                table: "InventoryBalances",
                column: "LastUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_Location",
                table: "InventoryBalances",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_ProdutoId",
                table: "InventoryBalances",
                column: "ProdutoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_DataValidade",
                table: "StockMovementDetails",
                column: "DataValidade");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_Lote",
                table: "StockMovementDetails",
                column: "Lote");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_StockMovementId",
                table: "StockMovementDetails",
                column: "StockMovementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryBalances");

            migrationBuilder.DropTable(
                name: "StockMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_DataValidade",
                table: "StockMovement");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_Lote",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "DataValidade",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "Lote",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "IsPerecivel",
                table: "Produto");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFabricao",
                table: "Produto",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "Produto",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Produto",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "MargemLucro",
                table: "Produto",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumStock",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStock",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoCusto",
                table: "Produto",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoVenda",
                table: "Produto",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QuatidadeEstoqueProduto",
                table: "Produto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ReorderPoint",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
