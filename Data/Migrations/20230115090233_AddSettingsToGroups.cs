using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsToGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupKitchenSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    KitchenName = table.Column<string>(type: "text", nullable: false),
                    HourExpired = table.Column<int>(type: "integer", nullable: false),
                    MinuteExpired = table.Column<int>(type: "integer", nullable: false),
                    PeriodicRefresh = table.Column<int>(type: "integer", nullable: false),
                    MenuFormat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupKitchenSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupKitchenSettings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupKitchenSettings_GroupId",
                table: "GroupKitchenSettings",
                column: "GroupId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupKitchenSettings");
        }
    }
}
