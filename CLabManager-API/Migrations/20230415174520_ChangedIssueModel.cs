using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLabManager_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIssueModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LabId",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_LabId",
                table: "Issues",
                column: "LabId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Labs_LabId",
                table: "Issues",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Labs_LabId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_LabId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "LabId",
                table: "Issues");
        }
    }
}
