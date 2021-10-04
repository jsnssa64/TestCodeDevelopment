using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreDevProj.Migrations
{
    public partial class ForeignKeyRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_Books_BookID",
                table: "Books_Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_Genres_GenreID",
                table: "Books_Genres");

            migrationBuilder.RenameColumn(
                name: "GenreID",
                table: "Books_Genres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "BookID",
                table: "Books_Genres",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Genres_GenreID",
                table: "Books_Genres",
                newName: "IX_Books_Genres_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Genres_BookID",
                table: "Books_Genres",
                newName: "IX_Books_Genres_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_Books_BookId",
                table: "Books_Genres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_Genres_GenreId",
                table: "Books_Genres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_Books_BookId",
                table: "Books_Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_Genres_GenreId",
                table: "Books_Genres");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Books_Genres",
                newName: "GenreID");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books_Genres",
                newName: "BookID");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Genres_GenreId",
                table: "Books_Genres",
                newName: "IX_Books_Genres_GenreID");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Genres_BookId",
                table: "Books_Genres",
                newName: "IX_Books_Genres_BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_Books_BookID",
                table: "Books_Genres",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_Genres_GenreID",
                table: "Books_Genres",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
