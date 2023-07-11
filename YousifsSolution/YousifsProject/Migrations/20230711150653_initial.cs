using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YousifsProject.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roofs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfRoof = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roofs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    RoofID = table.Column<int>(type: "int", nullable: false),
                    HaveBalcony = table.Column<bool>(type: "bit", nullable: false),
                    HaveWindow = table.Column<bool>(type: "bit", nullable: false),
                    HaveDoor = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfFloors = table.Column<int>(type: "int", nullable: false),
                    SortingOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Houses__RoofID__49C3F6B7",
                        column: x => x.RoofID,
                        principalTable: "Roofs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Houses_RoofID",
                table: "Houses",
                column: "RoofID");

            migrationBuilder.CreateIndex(
                name: "UQ__Houses__7D0C3F321B2DCE52",
                table: "Houses",
                column: "Address",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Roofs");
        }
    }
}
