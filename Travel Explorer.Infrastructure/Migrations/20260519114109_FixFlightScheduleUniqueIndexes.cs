using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFlightScheduleUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_flightschedules_FlightNumber",
                table: "flight_schedules");

            migrationBuilder.DropIndex(
                name: "IX_flightschedules_IsDeleted",
                table: "flight_schedules");

            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_FlightNumber",
                table: "flight_schedules",
                column: "FlightNumber");

            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_IsDeleted",
                table: "flight_schedules",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_flightschedules_FlightNumber",
                table: "flight_schedules");

            migrationBuilder.DropIndex(
                name: "IX_flightschedules_IsDeleted",
                table: "flight_schedules");

            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_FlightNumber",
                table: "flight_schedules",
                column: "FlightNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_IsDeleted",
                table: "flight_schedules",
                column: "IsDeleted",
                unique: true);
        }
    }
}
