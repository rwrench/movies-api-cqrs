using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Api.Cqrs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Year_Title",
                table: "Movies",
                columns: new[] { "YearOfRelease", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_YearOfRelease",
                table: "Movies",
                column: "YearOfRelease");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Title",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_UserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Year_Title",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_YearOfRelease",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
