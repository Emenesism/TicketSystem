using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Infrastructure.src.TicketSystem.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAndSomeCoulmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Admins_AdminId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Users_UserId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMessage_Ticket_TicketId",
                table: "TicketMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketMessage",
                table: "TicketMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "TicketMessage",
                newName: "TicketMessages");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_TicketMessage_TicketId",
                table: "TicketMessages",
                newName: "IX_TicketMessages_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_UserId",
                table: "Tickets",
                newName: "IX_Tickets_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_AdminId",
                table: "Tickets",
                newName: "IX_Tickets_AdminId");

            migrationBuilder.AddColumn<bool>(
                name: "Solved",
                table: "Tickets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketMessages",
                table: "TicketMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMessages_Tickets_TicketId",
                table: "TicketMessages",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Admins_AdminId",
                table: "Tickets",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketMessages_Tickets_TicketId",
                table: "TicketMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Admins_AdminId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketMessages",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "Solved",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameTable(
                name: "TicketMessages",
                newName: "TicketMessage");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_UserId",
                table: "Ticket",
                newName: "IX_Ticket_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_AdminId",
                table: "Ticket",
                newName: "IX_Ticket_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketMessages_TicketId",
                table: "TicketMessage",
                newName: "IX_TicketMessage_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketMessage",
                table: "TicketMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Admins_AdminId",
                table: "Ticket",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Users_UserId",
                table: "Ticket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMessage_Ticket_TicketId",
                table: "TicketMessage",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
