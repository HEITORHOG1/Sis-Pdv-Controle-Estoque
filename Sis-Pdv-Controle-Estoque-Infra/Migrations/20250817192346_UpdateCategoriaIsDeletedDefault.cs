using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisPdvControleEstoqueInfra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoriaIsDeletedDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Define valor padrão false para IsDeleted na tabela Categoria
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categoria",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            // Atualiza todos os registros existentes que possuem IsDeleted como true/1 ou NULL para false
            // Isso garante que categorias existentes que não foram explicitamente deletadas apareçam nas listagens
            migrationBuilder.Sql(@"
                UPDATE Categoria 
                SET IsDeleted = 0 
                WHERE IsDeleted IS NULL OR IsDeleted = 1;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove o valor padrão da coluna IsDeleted
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categoria",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);
        }
    }
}
