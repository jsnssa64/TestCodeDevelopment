using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreDevProj.Migrations
{
    public partial class AuthorDOBExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorsDOB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsDOB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorsDOB_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsDOB_AuthorId",
                table: "AuthorsDOB",
                column: "AuthorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorsDOB");

            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
