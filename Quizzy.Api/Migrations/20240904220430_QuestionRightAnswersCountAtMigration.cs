using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzy.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuestionRightAnswersCountAtMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RightAnswersCount",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RightAnswersCount",
                table: "Questions");
        }
    }
}
