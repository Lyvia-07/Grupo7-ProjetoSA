using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarValorObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorObra",
                table: "Obra",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorObra",
                table: "Obra");
        }
    }
}
