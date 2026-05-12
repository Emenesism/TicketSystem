using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class fixTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Revokeat",
                table: "Sessions",
                newName: "RevokeAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokeAt",
                table: "Sessions",
                newName: "Revokeat");
        }
    }
}
