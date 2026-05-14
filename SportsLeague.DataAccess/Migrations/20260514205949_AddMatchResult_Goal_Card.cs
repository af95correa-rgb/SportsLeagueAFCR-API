using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsLeague.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchResult_Goal_Card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Players_PlayerId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Players_PlayerId",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "HomeScore",
                table: "MatchResults",
                newName: "HomeGoals");

            migrationBuilder.RenameColumn(
                name: "AwayScore",
                table: "MatchResults",
                newName: "AwayGoals");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "MatchResults",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Matches",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Players_PlayerId",
                table: "Cards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Players_PlayerId",
                table: "Goals",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Players_PlayerId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Players_PlayerId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "MatchResults");

            migrationBuilder.RenameColumn(
                name: "HomeGoals",
                table: "MatchResults",
                newName: "HomeScore");

            migrationBuilder.RenameColumn(
                name: "AwayGoals",
                table: "MatchResults",
                newName: "AwayScore");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Players_PlayerId",
                table: "Cards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Players_PlayerId",
                table: "Goals",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
