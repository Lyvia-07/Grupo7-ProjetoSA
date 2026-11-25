using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    public partial class CorrigirRelacionamentoFotoInstalacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FotoInstalacao_RegistroInstalacao_RegistroInstalacaoId",
                table: "FotoInstalacao");

            migrationBuilder.DropIndex(
                name: "IX_FotoInstalacao_RegistroInstalacaoId",
                table: "FotoInstalacao");

            migrationBuilder.DropColumn(
                name: "RegistroInstalacaoId",
                table: "FotoInstalacao");

            migrationBuilder.CreateIndex(
                name: "IX_FotoInstalacao_IdRegistro",
                table: "FotoInstalacao",
                column: "IdRegistro");

            migrationBuilder.AddForeignKey(
                name: "FK_FotoInstalacao_RegistroInstalacao_IdRegistro",
                table: "FotoInstalacao",
                column: "IdRegistro",
                principalTable: "RegistroInstalacao",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FotoInstalacao_RegistroInstalacao_IdRegistro",
                table: "FotoInstalacao");

            migrationBuilder.DropIndex(
                name: "IX_FotoInstalacao_IdRegistro",
                table: "FotoInstalacao");

            migrationBuilder.AddColumn<int>(
                name: "RegistroInstalacaoId",
                table: "FotoInstalacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FotoInstalacao_RegistroInstalacaoId",
                table: "FotoInstalacao",
                column: "RegistroInstalacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FotoInstalacao_RegistroInstalacao_RegistroInstalacaoId",
                table: "FotoInstalacao",
                column: "RegistroInstalacaoId",
                principalTable: "RegistroInstalacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
