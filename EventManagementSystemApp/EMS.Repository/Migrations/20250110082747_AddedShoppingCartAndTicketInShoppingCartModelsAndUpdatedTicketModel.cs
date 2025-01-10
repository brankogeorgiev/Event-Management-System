using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedShoppingCartAndTicketInShoppingCartModelsAndUpdatedTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCart_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketInShoppingCart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInShoppingCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCart_ShoppingCart_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCart_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCart",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCart_ShoppingCartId",
                table: "TicketInShoppingCart",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCart_TicketId",
                table: "TicketInShoppingCart",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketInShoppingCart");

            migrationBuilder.DropTable(
                name: "ShoppingCart");
        }
    }
}
