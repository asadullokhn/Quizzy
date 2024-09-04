using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzy.Api.Migrations
{
    /// <inheritdoc />
    public partial class AttemptFinishedAtMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Attempts",
                newName: "StartedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Attempts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Attempts");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Attempts",
                newName: "CreatedAt");
        }
    }
}
