using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefMenuToKitchen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KitchenId",
                table: "Menu",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Menu_KitchenId",
                table: "Menu",
                column: "KitchenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Kitchens_KitchenId",
                table: "Menu",
                column: "KitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Kitchens_KitchenId",
                table: "Menu");

            migrationBuilder.DropIndex(
                name: "IX_Menu_KitchenId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "Menu");
        }
    }
}
