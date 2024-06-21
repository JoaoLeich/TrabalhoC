using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CProjeto.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "clienteid",
                table: "Venda",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Venda_clienteid",
                table: "Venda",
                column: "clienteid");

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Cliente_clienteid",
                table: "Venda",
                column: "clienteid",
                principalTable: "Cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Cliente_clienteid",
                table: "Venda");

            migrationBuilder.DropIndex(
                name: "IX_Venda_clienteid",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "clienteid",
                table: "Venda");
        }
    }
}
