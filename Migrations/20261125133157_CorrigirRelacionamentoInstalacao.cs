
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    public partial class CorrigirRelacionamentoInstalacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Obra_ObraId",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Usuario_MontadorId",
                table: "Instalacao");

            migrationBuilder.DropIndex(
                name: "IX_Instalacao_MontadorId",
                table: "Instalacao");

            migrationBuilder.DropIndex(
                name: "IX_Instalacao_ObraId",
                table: "Instalacao");

            migrationBuilder.DropColumn(
                name: "MontadorId",
                table: "Instalacao");

            migrationBuilder.DropColumn(
                name: "ObraId",
                table: "Instalacao");

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_IdMontador",
                table: "Instalacao",
                column: "IdMontador");

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_IdObra",
                table: "Instalacao",
                column: "IdObra");

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao",
                column: "IdObra",
                principalTable: "Obra",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao",
                column: "IdMontador",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao");

            migrationBuilder.DropIndex(
                name: "IX_Instalacao_IdMontador",
                table: "Instalacao");

            migrationBuilder.DropIndex(
                name: "IX_Instalacao_IdObra",
                table: "Instalacao");

            migrationBuilder.AddColumn<int>(
                name: "MontadorId",
                table: "Instalacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ObraId",
                table: "Instalacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_MontadorId",
                table: "Instalacao",
                column: "MontadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_ObraId",
                table: "Instalacao",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Obra_ObraId",
                table: "Instalacao",
                column: "ObraId",
                principalTable: "Obra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Usuario_MontadorId",
                table: "Instalacao",
                column: "MontadorId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
