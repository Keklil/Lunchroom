using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDishToTypeRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_DishTypes_DishTypeId",
                table: "Dishes");

            migrationBuilder.RenameColumn(
                name: "DishTypeId",
                table: "Dishes",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Dishes_DishTypeId",
                table: "Dishes",
                newName: "IX_Dishes_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_DishTypes_TypeId",
                table: "Dishes",
                column: "TypeId",
                principalTable: "DishTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_DishTypes_TypeId",
                table: "Dishes");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Dishes",
                newName: "DishTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Dishes_TypeId",
                table: "Dishes",
                newName: "IX_Dishes_DishTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_DishTypes_DishTypeId",
                table: "Dishes",
                column: "DishTypeId",
                principalTable: "DishTypes",
                principalColumn: "Id");
        }
    }
}
