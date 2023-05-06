using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentInfoSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "PaymentInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups",
                column: "SelectedKitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "PaymentInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    Qr = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInfo_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_GroupId",
                table: "PaymentInfo",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Kitchens_SelectedKitchenId",
                table: "Groups",
                column: "SelectedKitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
