using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pacientes",
                newName: "IdPaciente");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Medicos",
                newName: "IdMedico");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Consultas",
                newName: "IdConsulta");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "IdPaciente",
                table: "Pacientes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdMedico",
                table: "Medicos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdConsulta",
                table: "Consultas",
                newName: "Id");
        }
    }
}
