using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.BancoDigital.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PB_AGENCIAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Numero = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Estado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_AGENCIAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PB_PRODUTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TIPO_PRODUTO = table.Column<string>(type: "NVARCHAR2(13)", maxLength: 13, nullable: false),
                    ValorSolicitado = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    QuantidadeParcelas = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RendaMensalDeclarada = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    TaxaJurosMensal = table.Column<decimal>(type: "DECIMAL(10,4)", precision: 10, scale: 4, nullable: true),
                    ScoreCalculado = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_PRODUTOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PB_CLIENTES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AgenciaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_CLIENTE = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Cnpj = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    RazaoSocial = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_CLIENTES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PB_CLIENTES_PB_AGENCIAS_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "PB_AGENCIAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PB_CONTRATACOES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ClienteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProdutoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    MensagemResultado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_CONTRATACOES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PB_CONTRATACOES_PB_CLIENTES_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "PB_CLIENTES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PB_CONTRATACOES_PB_PRODUTOS_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PB_PRODUTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_AgenciaId",
                table: "PB_CLIENTES",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_Cnpj",
                table: "PB_CLIENTES",
                column: "Cnpj",
                unique: true,
                filter: "\"Cnpj\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_Cpf",
                table: "PB_CLIENTES",
                column: "Cpf",
                unique: true,
                filter: "\"Cpf\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CONTRATACOES_ClienteId",
                table: "PB_CONTRATACOES",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CONTRATACOES_ProdutoId",
                table: "PB_CONTRATACOES",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PB_CONTRATACOES");

            migrationBuilder.DropTable(
                name: "PB_CLIENTES");

            migrationBuilder.DropTable(
                name: "PB_PRODUTOS");

            migrationBuilder.DropTable(
                name: "PB_AGENCIAS");
        }
    }
}
