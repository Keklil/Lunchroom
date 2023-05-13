using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ReworkGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupKitchenSettings_SettingsId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedKitchenId",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<Point>(type: "geometry (point)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSettings", x => x.Id);
                });
            
            // Добавлено для соответвия SRID полигона у области доставки кухни
            migrationBuilder.Sql("SELECT updategeometrysrid('GroupSettings','Location',4326)");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SelectedKitchenId",
                table: "Groups",
                column: "SelectedKitchenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups",
                column: "SelectedKitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupSettings");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SelectedKitchenId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SelectedKitchenId",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Menu",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GroupKitchenSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupKitchenSettings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupKitchenSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupKitchenSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
