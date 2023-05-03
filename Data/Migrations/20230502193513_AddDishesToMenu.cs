using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDishesToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LunchSetList",
                table: "LunchSets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "LunchSets");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "Options",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DishTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DishTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    LunchSetId = table.Column<Guid>(type: "uuid", nullable: true),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_DishTypes_DishTypeId",
                        column: x => x.DishTypeId,
                        principalTable: "DishTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dishes_LunchSets_LunchSetId",
                        column: x => x.LunchSetId,
                        principalTable: "LunchSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dishes_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_DishId",
                table: "Options",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_DishTypeId",
                table: "Dishes",
                column: "DishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_LunchSetId",
                table: "Dishes",
                column: "LunchSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuId",
                table: "Dishes",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Dishes_DishId",
                table: "Options",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Dishes_DishId",
                table: "Options");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "DishTypes");

            migrationBuilder.DropIndex(
                name: "IX_Options_DishId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Options");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Options",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "LunchSetList",
                table: "LunchSets",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "LunchSets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
