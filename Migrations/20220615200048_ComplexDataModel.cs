using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lab5AspNetCoreEfIndividual.Migrations
{
    public partial class ComplexDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "patientname",
                table: "Patient",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal", nullable: false),
                    EstablishmentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DoctorID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_Department_Doctor_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctor",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Treatment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TreatmentTitle = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Defenition = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    DepartmentID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Treatment_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentAssignment",
                columns: table => new
                {
                    DoctorID = table.Column<int>(type: "integer", nullable: false),
                    TreatmentID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentAssignment", x => new { x.DoctorID, x.TreatmentID });
                    table.ForeignKey(
                        name: "FK_TreatmentAssignment_Doctor_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctor",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentAssignment_Treatment_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "Treatment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentContraindication",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(type: "integer", nullable: false),
                    Overview = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentContraindication", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK_TreatmentContraindication_Treatment_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "Treatment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_DoctorID",
                table: "Department",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_DepartmentID",
                table: "Treatment",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentAssignment_TreatmentID",
                table: "TreatmentAssignment",
                column: "TreatmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentAssignment");

            migrationBuilder.DropTable(
                name: "TreatmentContraindication");

            migrationBuilder.DropTable(
                name: "Treatment");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.AlterColumn<string>(
                name: "patientname",
                table: "Patient",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);
        }
    }
}
