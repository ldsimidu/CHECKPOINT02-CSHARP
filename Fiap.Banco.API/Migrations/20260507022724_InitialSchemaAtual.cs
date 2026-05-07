using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchemaAtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencias",
                columns: table => new
                {
                    idAgencia = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nmAgencia = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dsEndereco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencias", x => x.idAgencia);
                });

            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    idBanco = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeBanco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dtCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.idBanco);
                });

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

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    idProduto = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeProduto = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ativo = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    TipoProduto = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    valorSolicitado = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    taxaJurosMensal = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    quantidadeParcelas = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    taxaMdrPercentual = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    aluguelMensal = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    cnpjConvenio = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    limiteAntecipacaoPercentual = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.idProduto);
                });

            migrationBuilder.CreateTable(
                name: "AgenciaClientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    agenciaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoCliente = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    cpf = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: true),
                    dataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    cnpj = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    razaoSocial = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenciaClientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_AgenciaClientes_Agencias_agenciaId",
                        column: x => x.agenciaId,
                        principalTable: "Agencias",
                        principalColumn: "idAgencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contratacoes",
                columns: table => new
                {
                    idContratacao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    clienteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    produtoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dataSolicitacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    status = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    observacao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratacoes", x => x.idContratacao);
                    table.ForeignKey(
                        name: "FK_Contratacoes_AgenciaClientes_clienteId",
                        column: x => x.clienteId,
                        principalTable: "AgenciaClientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Produtos_produtoId",
                        column: x => x.produtoId,
                        principalTable: "Produtos",
                        principalColumn: "idProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgenciaClientes_agenciaId",
                table: "AgenciaClientes",
                column: "agenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_clienteId",
                table: "Contratacoes",
                column: "clienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_produtoId",
                table: "Contratacoes",
                column: "produtoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropTable(
                name: "Contratacoes");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "AgenciaClientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Agencias");
        }
    }
}
