using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixKitchenSettingsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kitchens_KitchenSettings_SettingsId",
                table: "Kitchens");

            migrationBuilder.DropIndex(
                name: "IX_Kitchens_SettingsId",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Kitchens");

            migrationBuilder.AddColumn<Guid>(
                name: "KitchenId",
                table: "KitchenSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_KitchenSettings_KitchenId",
                table: "KitchenSettings",
                column: "KitchenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KitchenSettings_Kitchens_KitchenId",
                table: "KitchenSettings",
                column: "KitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KitchenSettings_Kitchens_KitchenId",
                table: "KitchenSettings");

            migrationBuilder.DropIndex(
                name: "IX_KitchenSettings_KitchenId",
                table: "KitchenSettings");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "KitchenSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Kitchens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kitchens_SettingsId",
                table: "Kitchens",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchens_KitchenSettings_SettingsId",
                table: "Kitchens",
                column: "SettingsId",
                principalTable: "KitchenSettings",
                principalColumn: "Id");
        }
    }
}
