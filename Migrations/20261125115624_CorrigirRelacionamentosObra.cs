using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelacionamentosObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Cliente_ClienteId",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_VendedorId",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_ClienteId",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_VendedorId",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "VendedorId",
                table: "Obra");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdCliente",
                table: "Obra",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdVendedor",
                table: "Obra",
                column: "IdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Cliente_IdCliente",
                table: "Obra",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdVendedor",
                table: "Obra",
                column: "IdVendedor",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Cliente_IdCliente",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdVendedor",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdCliente",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdVendedor",
                table: "Obra");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Obra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VendedorId",
                table: "Obra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Obra_ClienteId",
                table: "Obra",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_VendedorId",
                table: "Obra",
                column: "VendedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Cliente_ClienteId",
                table: "Obra",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_VendedorId",
                table: "Obra",
                column: "VendedorId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
