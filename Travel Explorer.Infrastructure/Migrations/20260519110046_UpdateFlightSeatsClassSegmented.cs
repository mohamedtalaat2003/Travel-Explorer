using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    
    public partial class UpdateFlightSeatsClassSegmented : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "flight_schedules",
                newName: "AvailableFirstClassSeats");

            migrationBuilder.AddColumn<int>(
                name: "AvailableBusinessSeats",
                table: "flight_schedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableEconomySeats",
                table: "flight_schedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBusinessSeats",
                table: "flight_schedules");

            migrationBuilder.DropColumn(
                name: "AvailableEconomySeats",
                table: "flight_schedules");

            migrationBuilder.RenameColumn(
                name: "AvailableFirstClassSeats",
                table: "flight_schedules",
                newName: "AvailableSeats");
        }
    }
}
