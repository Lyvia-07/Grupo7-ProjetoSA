using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    public partial class CorrigirRelacionamentoRegistroInstalacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroInstalacao_Instalacao_InstalacaoId",
                table: "RegistroInstalacao");

            migrationBuilder.DropIndex(
                name: "IX_RegistroInstalacao_InstalacaoId",
                table: "RegistroInstalacao");

            migrationBuilder.DropColumn(
                name: "InstalacaoId",
                table: "RegistroInstalacao");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroInstalacao_IdInstalacao",
                table: "RegistroInstalacao",
                column: "IdInstalacao");

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao",
                column: "IdObra",
                principalTable: "Obra",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao",
                column: "IdMontador",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroInstalacao_Instalacao_IdInstalacao",
                table: "RegistroInstalacao",
                column: "IdInstalacao",
                principalTable: "Instalacao",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroInstalacao_Instalacao_IdInstalacao",
                table: "RegistroInstalacao");

            migrationBuilder.DropIndex(
                name: "IX_RegistroInstalacao_IdInstalacao",
                table: "RegistroInstalacao");

            migrationBuilder.AddColumn<int>(
                name: "InstalacaoId",
                table: "RegistroInstalacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegistroInstalacao_InstalacaoId",
                table: "RegistroInstalacao",
                column: "InstalacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Obra_IdObra",
                table: "Instalacao",
                column: "IdObra",
                principalTable: "Obra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instalacao_Usuario_IdMontador",
                table: "Instalacao",
                column: "IdMontador",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroInstalacao_Instalacao_InstalacaoId",
                table: "RegistroInstalacao",
                column: "InstalacaoId",
                principalTable: "Instalacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
