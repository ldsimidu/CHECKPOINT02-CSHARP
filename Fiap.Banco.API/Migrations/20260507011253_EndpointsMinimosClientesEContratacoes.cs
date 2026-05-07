using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class EndpointsMinimosClientesEContratacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Bancos_BancoId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "BancoId",
                table: "Clientes",
                newName: "BancoidBanco");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_BancoId",
                table: "Clientes",
                newName: "IX_Clientes_BancoidBanco");

            migrationBuilder.AddColumn<int>(
                name: "agenciaId",
                table: "Clientes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

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
                        name: "FK_Contratacoes_Clientes_clienteId",
                        column: x => x.clienteId,
                        principalTable: "Clientes",
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
                name: "IX_Clientes_agenciaId",
                table: "Clientes",
                column: "agenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_clienteId",
                table: "Contratacoes",
                column: "clienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_produtoId",
                table: "Contratacoes",
                column: "produtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Agencias_agenciaId",
                table: "Clientes",
                column: "agenciaId",
                principalTable: "Agencias",
                principalColumn: "idAgencia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Bancos_BancoidBanco",
                table: "Clientes",
                column: "BancoidBanco",
                principalTable: "Bancos",
                principalColumn: "idBanco");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Agencias_agenciaId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Bancos_BancoidBanco",
                table: "Clientes");

            migrationBuilder.DropTable(
                name: "Contratacoes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_agenciaId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "agenciaId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "BancoidBanco",
                table: "Clientes",
                newName: "BancoId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_BancoidBanco",
                table: "Clientes",
                newName: "IX_Clientes_BancoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Bancos_BancoId",
                table: "Clientes",
                column: "BancoId",
                principalTable: "Bancos",
                principalColumn: "idBanco");
        }
    }
}
