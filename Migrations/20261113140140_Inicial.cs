using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S.A_Esquadrias.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Obra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdVendedor = table.Column<int>(type: "int", nullable: false),
                    NomeObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObsGerais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obra_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Obra_Usuario_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Instalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    IdMontador = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObraId = table.Column<int>(type: "int", nullable: false),
                    MontadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instalacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instalacao_Obra_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instalacao_Usuario_MontadorId",
                        column: x => x.MontadorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Medicao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    IdVendedor = table.Column<int>(type: "int", nullable: false),
                    DataMedicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoEsquadria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LarguraNominal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AlturaNominal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ObsTecnicas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObraId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicao_Obra_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Medicao_Usuario_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RegistroInstalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInstalacao = table.Column<int>(type: "int", nullable: false),
                    TipoProblema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObservacoesGerais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoProblema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstalacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroInstalacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroInstalacao_Instalacao_InstalacaoId",
                        column: x => x.InstalacaoId,
                        principalTable: "Instalacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FotoInstalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRegistro = table.Column<int>(type: "int", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistroInstalacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoInstalacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotoInstalacao_RegistroInstalacao_RegistroInstalacaoId",
                        column: x => x.RegistroInstalacaoId,
                        principalTable: "RegistroInstalacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FotoInstalacao_RegistroInstalacaoId",
                table: "FotoInstalacao",
                column: "RegistroInstalacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_MontadorId",
                table: "Instalacao",
                column: "MontadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Instalacao_ObraId",
                table: "Instalacao",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_ObraId",
                table: "Medicao",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_VendedorId",
                table: "Medicao",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_ClienteId",
                table: "Obra",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_VendedorId",
                table: "Obra",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroInstalacao_InstalacaoId",
                table: "RegistroInstalacao",
                column: "InstalacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotoInstalacao");

            migrationBuilder.DropTable(
                name: "Medicao");

            migrationBuilder.DropTable(
                name: "RegistroInstalacao");

            migrationBuilder.DropTable(
                name: "Instalacao");

            migrationBuilder.DropTable(
                name: "Obra");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
