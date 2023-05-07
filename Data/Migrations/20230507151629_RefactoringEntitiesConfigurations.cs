using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringEntitiesConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Menu");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Options",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Options",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Menu",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DishTypes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DishTypes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CreatedAt",
                table: "Menu",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menu_CreatedAt",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DishTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DishTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Menu",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
