using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Lab5AspNetCoreEfIndividual.Migrations
{
    public partial class Inheritance : Migration
    {
        // [database update] command will result in lost data because it will drop the Doctor table and rename the Patient table to Person
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Own changes due to refactoring
            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "Doctor",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DoctorName",
                table: "Doctor",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "patientname",
                table: "Patient",
                newName: "Name");
            // Own changes due to refactoring ^^^

            // Remove FK constraints and indexes that point to the Patient table
            // We will drop Patient table
            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Patient_PatientID",
                table: "Consultation"
                );

            migrationBuilder.DropIndex(name: "IX_Consultation_PatientID", table: "Consultation");

            // Rename the Doctor table as Person and make changes to store Patient data
            // Make entity-only columns nullable:true
            migrationBuilder.RenameTable(name: "Doctor", newName: "Person");
            migrationBuilder.AlterColumn<string>(name: "JobTitle", table: "Person", nullable: true);
            migrationBuilder.AddColumn<int>(name: "InsuranceId", table: "Person", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Diagnosis", table: "Person", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Discriminator", table: "Person", nullable: false, maxLength: 128, defaultValue: "Doctor");
            // Temporary field that will be used to update foreign keys that point to patients.
            // When we copy patients into the Person table they will get new primary key values.
            migrationBuilder.AddColumn<int>(name: "OldId", table: "Person", nullable: true);

            // Copy existing Patient data into new Person table. They get new primary key values
            string copySql = @"INSERT INTO public.""Person"" (""Name"", ""JobTitle"", ""InsuranceId"", ""Diagnosis"", ""Discriminator"", ""OldId"") SELECT ""Name"", null AS ""JobTitle"", ""InsuranceId"", ""Diagnosis"", 'Patient' AS ""Discriminator"", ""ID"" AS ""OldId"" FROM public.""Patient""";
            migrationBuilder.Sql(copySql);

            // Fix up existing relationships to match new PK's.
            string updSql = @"UPDATE public.""Consultation"" SET ""PatientID"" = (SELECT ""ID"" FROM public.""Person"" WHERE ""OldId"" = ""Consultation"".""PatientID"" AND ""Discriminator"" = 'Patient')";
            migrationBuilder.Sql(updSql);

            // Remove temporary key
            migrationBuilder.DropColumn(name: "OldId", table: "Person");

            migrationBuilder.DropTable(name: "Patient");

            // Re-create foreign key constraints and indexes, now pointing them to the Person table.
            migrationBuilder.CreateIndex(
                name: "IX_Consultation_PatientID",
                table: "Consultation",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Person_PatientID",
                table: "Consultation",
                column: "PatientID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);


            //migrationBuilder.DropColumn(
            //    name: "patientname",
            //    table: "Patient");

            //migrationBuilder.DropColumn(
            //    name: "DoctorName",
            //    table: "Doctor");

            //migrationBuilder.RenameColumn(
            //    name: "DoctorID",
            //    table: "Doctor",
            //    newName: "ID");

            //migrationBuilder.CreateTable(
            //    name: "Person",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Person", x => x.ID);
            //    });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Doctor_Person_ID",
            //    table: "Doctor",
            //    column: "ID",
            //    principalTable: "Person",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Patient_Person_ID",
            //    table: "Patient",
            //    column: "ID",
            //    principalTable: "Person",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        // In a production system you would make corresponding changes to the Down method
        // in case you ever had to use that to go back to the previous database version.
        // For this tutorial you won't be using the Down method
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Person_ID",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Person_ID",
                table: "Patient");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Doctor",
                newName: "DoctorID");

            migrationBuilder.AddColumn<string>(
                name: "patientname",
                table: "Patient",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Doctor",
                type: "text",
                nullable: true);
        }
    }
}
