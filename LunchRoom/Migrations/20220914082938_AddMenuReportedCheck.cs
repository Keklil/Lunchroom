using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchRoom.Migrations
{
    public partial class AddMenuReportedCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "Menu",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "Menu");
        }
    }
}
