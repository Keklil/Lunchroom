using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefKitchenToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
