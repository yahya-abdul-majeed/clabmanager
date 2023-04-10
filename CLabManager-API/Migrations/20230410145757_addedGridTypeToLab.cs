using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLabManager_API.Migrations
{
    /// <inheritdoc />
    public partial class addedGridTypeToLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GridType",
                table: "Labs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GridType",
                table: "Labs");
        }
    }
}
