using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToScheduleEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledEvents_Events_EventId",
                table: "ScheduledEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledEvents_Events_EventId",
                table: "ScheduledEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledEvents_Events_EventId",
                table: "ScheduledEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledEvents_Events_EventId",
                table: "ScheduledEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
