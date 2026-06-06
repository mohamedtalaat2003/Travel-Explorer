using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    
    public partial class AddGenderAndNationalityToFlightBooking : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "flight_bookings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "flight_bookings",
                type: "text",
                nullable: true);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "flight_bookings");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "flight_bookings");
        }
    }
}
