using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisPdvControleEstoqueInfra.Migrations
{
    /// <inheritdoc />
    public partial class DomainSeparationRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Create backup table for existing data
            migrationBuilder.Sql(@"
                CREATE TABLE Produtos_Backup AS 
                SELECT * FROM Produto;
            ");

            // Step 2: Add IsPerecivel column to Produto table
            migrationBuilder.AddColumn<bool>(
                name: "IsPerecivel",
                table: "Produto",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            // Step 3: Create InventoryBalances table
            migrationBuilder.CreateTable(
                name: "InventoryBalances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    ProdutoId = table.Column<string>(type: "char(36)", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    ReservedStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    MaximumStock = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    Location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "char(36)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<string>(type: "char(36)", nullable: true)
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
                });

            // Step 4: Create StockMovementDetails table
            migrationBuilder.CreateTable(
                name: "StockMovementDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    StockMovementId = table.Column<string>(type: "char(36)", nullable: false),
                    Lote = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DataValidade = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "char(36)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<string>(type: "char(36)", nullable: true)
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
                });

            // Step 5: Add new columns to StockMovement table
            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "StockMovement",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataValidade",
                table: "StockMovement",
                type: "datetime(6)",
                nullable: true);

            // Step 6: Migrate existing data to InventoryBalances
            migrationBuilder.Sql(@"
                INSERT INTO InventoryBalances (Id, ProdutoId, CurrentStock, ReservedStock, MinimumStock, MaximumStock, ReorderPoint, Location, LastUpdated, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted, DeletedAt, DeletedBy)
                SELECT 
                    UUID() as Id,
                    Id as ProdutoId,
                    COALESCE(QuatidadeEstoqueProduto, 0) as CurrentStock,
                    0 as ReservedStock,
                    COALESCE(MinimumStock, 0) as MinimumStock,
                    COALESCE(MaximumStock, 0) as MaximumStock,
                    COALESCE(ReorderPoint, 0) as ReorderPoint,
                    Location,
                    NOW() as LastUpdated,
                    CreatedAt,
                    UpdatedAt,
                    CreatedBy,
                    UpdatedBy,
                    IsDeleted,
                    DeletedAt,
                    DeletedBy
                FROM Produto;
            ");

            // Step 7: Create initial stock movements for existing quantities
            migrationBuilder.Sql(@"
                INSERT INTO StockMovement (Id, ProdutoId, Quantity, Type, Reason, UnitCost, PreviousStock, NewStock, MovementDate, ReferenceDocument, UserId, Lote, DataValidade, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted, DeletedAt, DeletedBy)
                SELECT 
                    UUID(),
                    Id,
                    COALESCE(QuatidadeEstoqueProduto, 0),
                    1, -- Entry
                    'Migração inicial do sistema - saldo existente',
                    COALESCE(PrecoCusto, 0),
                    0,
                    COALESCE(QuatidadeEstoqueProduto, 0),
                    NOW(),
                    'MIGRATION_INITIAL',
                    NULL,
                    NULL,
                    NULL,
                    NOW(),
                    NULL,
                    NULL,
                    NULL,
                    0,
                    NULL,
                    NULL
                FROM Produto 
                WHERE COALESCE(QuatidadeEstoqueProduto, 0) > 0;
            ");

            // Step 8: Remove old columns from Produto table
            migrationBuilder.DropColumn(
                name: "PrecoCusto",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PrecoVenda",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MargemLucro",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataFabricao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "QuatidadeEstoqueProduto",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "ReorderPoint",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Produto");

            // Step 9: Create indexes for performance
            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_ProdutoId",
                table: "InventoryBalances",
                column: "ProdutoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_LastUpdated",
                table: "InventoryBalances",
                column: "LastUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_Location",
                table: "InventoryBalances",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_StockMovementId",
                table: "StockMovementDetails",
                column: "StockMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_Lote",
                table: "StockMovementDetails",
                column: "Lote");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_DataValidade",
                table: "StockMovementDetails",
                column: "DataValidade");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_Lote",
                table: "StockMovement",
                column: "Lote");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_DataValidade",
                table: "StockMovement",
                column: "DataValidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop new indexes
            migrationBuilder.DropIndex(
                name: "IX_StockMovement_DataValidade",
                table: "StockMovement");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_Lote",
                table: "StockMovement");

            migrationBuilder.DropIndex(
                name: "IX_StockMovementDetails_DataValidade",
                table: "StockMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockMovementDetails_Lote",
                table: "StockMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockMovementDetails_StockMovementId",
                table: "StockMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryBalances_Location",
                table: "InventoryBalances");

            migrationBuilder.DropIndex(
                name: "IX_InventoryBalances_LastUpdated",
                table: "InventoryBalances");

            migrationBuilder.DropIndex(
                name: "IX_InventoryBalances_ProdutoId",
                table: "InventoryBalances");

            // Drop new tables
            migrationBuilder.DropTable(
                name: "StockMovementDetails");

            migrationBuilder.DropTable(
                name: "InventoryBalances");

            // Remove new columns from StockMovement
            migrationBuilder.DropColumn(
                name: "DataValidade",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "Lote",
                table: "StockMovement");

            // Restore old columns to Produto table
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

            migrationBuilder.AddColumn<decimal>(
                name: "MargemLucro",
                table: "Produto",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<int>(
                name: "QuatidadeEstoqueProduto",
                table: "Produto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStock",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
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
                name: "ReorderPoint",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Produto",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            // Remove IsPerecivel column
            migrationBuilder.DropColumn(
                name: "IsPerecivel",
                table: "Produto");

            // Restore data from backup
            migrationBuilder.Sql(@"
                UPDATE Produto p
                INNER JOIN Produtos_Backup pb ON p.Id = pb.Id
                SET 
                    p.PrecoCusto = pb.PrecoCusto,
                    p.PrecoVenda = pb.PrecoVenda,
                    p.MargemLucro = pb.MargemLucro,
                    p.DataFabricao = pb.DataFabricao,
                    p.DataVencimento = pb.DataVencimento,
                    p.QuatidadeEstoqueProduto = pb.QuatidadeEstoqueProduto,
                    p.MinimumStock = pb.MinimumStock,
                    p.MaximumStock = pb.MaximumStock,
                    p.ReorderPoint = pb.ReorderPoint,
                    p.Location = pb.Location;
            ");

            // Drop backup table
            migrationBuilder.Sql("DROP TABLE Produtos_Backup;");
        }
    }
}