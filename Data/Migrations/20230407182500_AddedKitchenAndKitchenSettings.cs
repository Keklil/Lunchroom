using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedKitchenAndKitchenSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupKitchenSettings_Groups_GroupId",
                table: "GroupKitchenSettings");

            migrationBuilder.DropIndex(
                name: "IX_GroupKitchenSettings_GroupId",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "HourExpired",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "KitchenName",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "MenuFormat",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "MinuteExpired",
                table: "GroupKitchenSettings");

            migrationBuilder.DropColumn(
                name: "PeriodicRefresh",
                table: "GroupKitchenSettings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Qr",
                table: "PaymentInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PaymentInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Kitchens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Inn = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitchens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LimitingTimeForOrder = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MenuUpdatePeriod = table.Column<int>(type: "integer", nullable: false),
                    MenuFormat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenUser",
                columns: table => new
                {
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenUser", x => new { x.KitchenId, x.ManagersId });
                    table.ForeignKey(
                        name: "FK_KitchenUser_Kitchens_KitchenId",
                        column: x => x.KitchenId,
                        principalTable: "Kitchens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitchenUser_Users_ManagersId",
                        column: x => x.ManagersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingArea",
                columns: table => new
                {
                    KitchenSettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Polygon = table.Column<Polygon>(type: "geometry (polygon)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingArea", x => new { x.KitchenSettingsId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShippingArea_KitchenSettings_KitchenSettingsId",
                        column: x => x.KitchenSettingsId,
                        principalTable: "KitchenSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SettingsId",
                table: "Groups",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenUser_ManagersId",
                table: "KitchenUser",
                column: "ManagersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupKitchenSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupKitchenSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupKitchenSettings_SettingsId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "KitchenUser");

            migrationBuilder.DropTable(
                name: "ShippingArea");

            migrationBuilder.DropTable(
                name: "Kitchens");

            migrationBuilder.DropTable(
                name: "KitchenSettings");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Groups");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Qr",
                table: "PaymentInfo",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PaymentInfo",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "GroupKitchenSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "HourExpired",
                table: "GroupKitchenSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KitchenName",
                table: "GroupKitchenSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MenuFormat",
                table: "GroupKitchenSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinuteExpired",
                table: "GroupKitchenSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodicRefresh",
                table: "GroupKitchenSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GroupKitchenSettings_GroupId",
                table: "GroupKitchenSettings",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupKitchenSettings_Groups_GroupId",
                table: "GroupKitchenSettings",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
