using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    public partial class AdicionarEnderecoCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Cliente");
        }
    }
}
