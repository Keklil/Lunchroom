using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ReworkSettingsReferenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingArea_KitchenSettings_KitchenSettingsId",
                table: "ShippingArea");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "KitchenSettings");

            migrationBuilder.RenameColumn(
                name: "KitchenSettingsId",
                table: "ShippingArea",
                newName: "KitchenSettingsKitchenId");

            migrationBuilder.AddColumn<string>(
                name: "Contacts_Email",
                table: "Kitchens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contacts_Phone",
                table: "Kitchens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Settings_LimitingTimeForOrder",
                table: "Kitchens",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_MenuFormat",
                table: "Kitchens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_MenuUpdatePeriod",
                table: "Kitchens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingArea_Kitchens_KitchenSettingsKitchenId",
                table: "ShippingArea",
                column: "KitchenSettingsKitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingArea_Kitchens_KitchenSettingsKitchenId",
                table: "ShippingArea");

            migrationBuilder.DropColumn(
                name: "Contacts_Email",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "Contacts_Phone",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "Settings_LimitingTimeForOrder",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "Settings_MenuFormat",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "Settings_MenuUpdatePeriod",
                table: "Kitchens");

            migrationBuilder.RenameColumn(
                name: "KitchenSettingsKitchenId",
                table: "ShippingArea",
                newName: "KitchenSettingsId");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.KitchenId);
                    table.ForeignKey(
                        name: "FK_Contacts_Kitchens_KitchenId",
                        column: x => x.KitchenId,
                        principalTable: "Kitchens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false),
                    LimitingTimeForOrder = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MenuFormat = table.Column<int>(type: "integer", nullable: false),
                    MenuUpdatePeriod = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenSettings_Kitchens_KitchenId",
                        column: x => x.KitchenId,
                        principalTable: "Kitchens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenSettings_KitchenId",
                table: "KitchenSettings",
                column: "KitchenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingArea_KitchenSettings_KitchenSettingsId",
                table: "ShippingArea",
                column: "KitchenSettingsId",
                principalTable: "KitchenSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
