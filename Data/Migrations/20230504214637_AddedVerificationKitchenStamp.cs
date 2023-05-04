using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedVerificationKitchenStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KitchenVerificationStamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VerificationTimestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CheckerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenVerificationStamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenVerificationStamps_Users_CheckerId",
                        column: x => x.CheckerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenVerificationStamps_CheckerId",
                table: "KitchenVerificationStamps",
                column: "CheckerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KitchenVerificationStamps");
        }
    }
}
