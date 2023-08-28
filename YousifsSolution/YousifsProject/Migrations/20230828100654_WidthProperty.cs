using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YousifsProject.Migrations
{
    public partial class WidthProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 12);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Width",
                table: "Houses");
        }
    }
}
