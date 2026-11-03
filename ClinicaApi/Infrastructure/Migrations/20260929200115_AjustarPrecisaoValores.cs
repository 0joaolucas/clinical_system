using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustarPrecisaoValores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Medicos_MedicoIdMedico",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_PacienteIdPaciente",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_MedicoIdMedico",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_PacienteIdPaciente",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "MedicoIdMedico",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "PacienteIdPaciente",
                table: "Consultas");

            migrationBuilder.AddColumn<int>(
                name: "IdPaciente",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdRemedio",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cpf = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Perfil = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_IdPaciente",
                table: "Transacoes",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_IdRemedio",
                table: "Transacoes",
                column: "IdRemedio");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_IdMedico",
                table: "Consultas",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_IdPaciente",
                table: "Consultas",
                column: "IdPaciente");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Medicos_IdPaciente",
                table: "Consultas",
                column: "IdPaciente",
                principalTable: "Medicos",
                principalColumn: "IdMedico",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_IdMedico",
                table: "Consultas",
                column: "IdMedico",
                principalTable: "Pacientes",
                principalColumn: "IdPaciente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_Pacientes_IdPaciente",
                table: "Transacoes",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "IdPaciente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacoes_Remedios_IdRemedio",
                table: "Transacoes",
                column: "IdRemedio",
                principalTable: "Remedios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Medicos_IdPaciente",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_IdMedico",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_Pacientes_IdPaciente",
                table: "Transacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacoes_Remedios_IdRemedio",
                table: "Transacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Transacoes_IdPaciente",
                table: "Transacoes");

            migrationBuilder.DropIndex(
                name: "IX_Transacoes_IdRemedio",
                table: "Transacoes");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_IdMedico",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_IdPaciente",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "IdPaciente",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "IdRemedio",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Transacoes");

            migrationBuilder.AddColumn<int>(
                name: "MedicoIdMedico",
                table: "Consultas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PacienteIdPaciente",
                table: "Consultas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_MedicoIdMedico",
                table: "Consultas",
                column: "MedicoIdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteIdPaciente",
                table: "Consultas",
                column: "PacienteIdPaciente");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Medicos_MedicoIdMedico",
                table: "Consultas",
                column: "MedicoIdMedico",
                principalTable: "Medicos",
                principalColumn: "IdMedico",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_PacienteIdPaciente",
                table: "Consultas",
                column: "PacienteIdPaciente",
                principalTable: "Pacientes",
                principalColumn: "IdPaciente",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
