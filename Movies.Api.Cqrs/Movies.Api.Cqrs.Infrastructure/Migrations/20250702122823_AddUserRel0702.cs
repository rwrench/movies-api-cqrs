using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Api.Cqrs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRel0702 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Movies",
                newName: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Movies",
                newName: "Id");
        }
    }
}
