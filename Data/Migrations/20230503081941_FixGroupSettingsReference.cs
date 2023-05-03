using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixGroupSettingsReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SettingsId",
                table: "Groups",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SettingsId",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
