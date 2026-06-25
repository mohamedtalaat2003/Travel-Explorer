using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRowVersionTOFlightBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "flight_bookings",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "flight_bookings");
        }
    }
}
