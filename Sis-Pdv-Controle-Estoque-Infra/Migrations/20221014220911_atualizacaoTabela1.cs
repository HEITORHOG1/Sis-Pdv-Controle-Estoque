using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sis_Pdv_Controle_Estoque_Infra.Migrations
{
    public partial class atualizacaoTabela1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoPedido_Pedido_PedidoId",
                table: "ProdutoPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoPedido_Produto_ProdutoId",
                table: "ProdutoPedido");

            migrationBuilder.DropIndex(
                name: "IX_ProdutoPedido_PedidoId",
                table: "ProdutoPedido");

            migrationBuilder.DropIndex(
                name: "IX_ProdutoPedido_ProdutoId",
                table: "ProdutoPedido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProdutoPedido_PedidoId",
                table: "ProdutoPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoPedido_ProdutoId",
                table: "ProdutoPedido",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoPedido_Pedido_PedidoId",
                table: "ProdutoPedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoPedido_Produto_ProdutoId",
                table: "ProdutoPedido",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
