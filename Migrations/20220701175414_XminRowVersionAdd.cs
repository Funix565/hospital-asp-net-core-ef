using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab5AspNetCoreEfIndividual.Migrations
{
    public partial class XminRowVersionAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "xmin",
                table: "Department",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Department");
        }
    }
}
