using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ReworkOrdering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_LunchSets_LunchSetId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersOptions_Orders_OrderId",
                table: "OrdersOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersOptions",
                table: "OrdersOptions");

            migrationBuilder.DropIndex(
                name: "IX_OrdersOptions_OrderId",
                table: "OrdersOptions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_LunchSetId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "LunchSetId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LunchSetUnits",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OptionUnits",
                table: "OrdersOptions",
                newName: "Quantity");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrdersOptions");
            
            migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "OrdersOptions",
                    type: "integer",
                    nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            
            migrationBuilder.AddColumn<Guid>(
                name: "OrderLunchSetOrderId",
                table: "OrdersOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "OrderLunchSetId",
                table: "OrdersOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MenuOptionId",
                table: "OrdersOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersOptions",
                table: "OrdersOptions",
                columns: new[] { "OrderLunchSetOrderId", "OrderLunchSetId", "Id" });

            migrationBuilder.CreateTable(
                name: "OrderDish",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuDishId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDish", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderDish_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLunchSet",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<int>(type: "integer", nullable: false),
                    MenuLunchSetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLunchSet", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderLunchSet_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersOptions_OrderLunchSet_OrderLunchSetOrderId_OrderLunch~",
                table: "OrdersOptions",
                columns: new[] { "OrderLunchSetOrderId", "OrderLunchSetId" },
                principalTable: "OrderLunchSet",
                principalColumns: new[] { "OrderId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersOptions_OrderLunchSet_OrderLunchSetOrderId_OrderLunch~",
                table: "OrdersOptions");

            migrationBuilder.DropTable(
                name: "OrderDish");

            migrationBuilder.DropTable(
                name: "OrderLunchSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersOptions",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "OrderLunchSetOrderId",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "OrderLunchSetId",
                table: "OrdersOptions");

            migrationBuilder.DropColumn(
                name: "MenuOptionId",
                table: "OrdersOptions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrdersOptions",
                newName: "OptionUnits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrdersOptions");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrdersOptions",
                type: "uuid",
                nullable: false);
            
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrdersOptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrdersOptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrdersOptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Guid>(
                name: "LunchSetId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LunchSetUnits",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersOptions",
                table: "OrdersOptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersOptions_OrderId",
                table: "OrdersOptions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LunchSetId",
                table: "Orders",
                column: "LunchSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_LunchSets_LunchSetId",
                table: "Orders",
                column: "LunchSetId",
                principalTable: "LunchSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersOptions_Orders_OrderId",
                table: "OrdersOptions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
