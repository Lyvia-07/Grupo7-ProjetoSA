
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    public partial class CorrigirRelacionamentosMedicao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicao_Obra_ObraId",
                table: "Medicao");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicao_Usuario_VendedorId",
                table: "Medicao");

            migrationBuilder.DropIndex(
                name: "IX_Medicao_ObraId",
                table: "Medicao");

            migrationBuilder.DropIndex(
                name: "IX_Medicao_VendedorId",
                table: "Medicao");

            migrationBuilder.DropColumn(
                name: "ObraId",
                table: "Medicao");

            migrationBuilder.DropColumn(
                name: "VendedorId",
                table: "Medicao");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_IdObra",
                table: "Medicao",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_IdVendedor",
                table: "Medicao",
                column: "IdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicao_Obra_IdObra",
                table: "Medicao",
                column: "IdObra",
                principalTable: "Obra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicao_Usuario_IdVendedor",
                table: "Medicao",
                column: "IdVendedor",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicao_Obra_IdObra",
                table: "Medicao");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicao_Usuario_IdVendedor",
                table: "Medicao");

            migrationBuilder.DropIndex(
                name: "IX_Medicao_IdObra",
                table: "Medicao");

            migrationBuilder.DropIndex(
                name: "IX_Medicao_IdVendedor",
                table: "Medicao");

            migrationBuilder.AddColumn<int>(
                name: "ObraId",
                table: "Medicao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VendedorId",
                table: "Medicao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_ObraId",
                table: "Medicao",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_VendedorId",
                table: "Medicao",
                column: "VendedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicao_Obra_ObraId",
                table: "Medicao",
                column: "ObraId",
                principalTable: "Obra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicao_Usuario_VendedorId",
                table: "Medicao",
                column: "VendedorId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
