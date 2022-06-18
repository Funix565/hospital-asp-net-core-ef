using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab5AspNetCoreEfIndividual.Migrations
{
    public partial class ColumnPatientNameLower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "Patient",
                newName: "patientname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "patientname",
                table: "Patient",
                newName: "PatientName");
        }
    }
}
