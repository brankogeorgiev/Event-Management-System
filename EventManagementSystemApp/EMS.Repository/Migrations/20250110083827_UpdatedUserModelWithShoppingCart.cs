using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserModelWithShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCart");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCart",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCart");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCart",
                column: "OwnerId");
        }
    }
}
