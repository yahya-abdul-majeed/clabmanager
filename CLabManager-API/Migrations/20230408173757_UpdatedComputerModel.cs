using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLabManager_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedComputerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers");

            migrationBuilder.AlterColumn<int>(
                name: "LabId",
                table: "Computers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GridType",
                table: "Computers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionOnGrid",
                table: "Computers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ComputerDTO",
                columns: table => new
                {
                    ComputerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPositioned = table.Column<bool>(type: "bit", nullable: false),
                    GridType = table.Column<int>(type: "int", nullable: false),
                    PositionOnGrid = table.Column<int>(type: "int", nullable: false),
                    LabId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerDTO", x => x.ComputerId);
                    table.ForeignKey(
                        name: "FK_ComputerDTO_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputerDTO_LabId",
                table: "ComputerDTO",
                column: "LabId");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers");

            migrationBuilder.DropTable(
                name: "ComputerDTO");

            migrationBuilder.DropColumn(
                name: "GridType",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "PositionOnGrid",
                table: "Computers");

            migrationBuilder.AlterColumn<int>(
                name: "LabId",
                table: "Computers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
