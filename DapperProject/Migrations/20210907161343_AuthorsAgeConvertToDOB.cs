using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreDevProj.Migrations
{
    public partial class AuthorsAgeConvertToDOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Authors",
                newName: "DOB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "Authors",
                newName: "Age");
        }
    }
}
