using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_transactions_destination_bookings_DestinationBookId",
                table: "payment_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_transactions_flight_bookings_FlightBookId",
                table: "payment_transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DestinationBookId",
                table: "payment_transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_FlightBookId",
                table: "payment_transactions");

            migrationBuilder.DropColumn(
                name: "DestinationBookId",
                table: "payment_transactions");

            migrationBuilder.DropColumn(
                name: "FlightBookId",
                table: "payment_transactions");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "flight_bookings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "destination_bookings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_flightBookings_PaymentId",
                table: "flight_bookings",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_destinationBookings_PaymentId",
                table: "destination_bookings",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_destination_bookings_payment_transactions_PaymentId",
                table: "destination_bookings",
                column: "PaymentId",
                principalTable: "payment_transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_flight_bookings_payment_transactions_PaymentId",
                table: "flight_bookings",
                column: "PaymentId",
                principalTable: "payment_transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_destination_bookings_payment_transactions_PaymentId",
                table: "destination_bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_flight_bookings_payment_transactions_PaymentId",
                table: "flight_bookings");

            migrationBuilder.DropIndex(
                name: "IX_flightBookings_PaymentId",
                table: "flight_bookings");

            migrationBuilder.DropIndex(
                name: "IX_destinationBookings_PaymentId",
                table: "destination_bookings");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "flight_bookings");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "destination_bookings");

            migrationBuilder.AddColumn<int>(
                name: "DestinationBookId",
                table: "payment_transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlightBookId",
                table: "payment_transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DestinationBookId",
                table: "payment_transactions",
                column: "DestinationBookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FlightBookId",
                table: "payment_transactions",
                column: "FlightBookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_transactions_destination_bookings_DestinationBookId",
                table: "payment_transactions",
                column: "DestinationBookId",
                principalTable: "destination_bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_transactions_flight_bookings_FlightBookId",
                table: "payment_transactions",
                column: "FlightBookId",
                principalTable: "flight_bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
