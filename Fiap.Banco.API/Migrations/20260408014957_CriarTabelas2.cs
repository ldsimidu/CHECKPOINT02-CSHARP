using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    idFuncionarios = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.idFuncionarios);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funcionarios");
        }
    }
}
