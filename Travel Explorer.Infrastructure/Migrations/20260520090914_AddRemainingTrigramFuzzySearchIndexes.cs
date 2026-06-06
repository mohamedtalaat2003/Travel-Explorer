using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    
    public partial class AddRemainingTrigramFuzzySearchIndexes : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_ArrivalCity_trgm",
                table: "flight_schedules",
                column: "ArrivalCity")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_flightschedules_DepartureCity_trgm",
                table: "flight_schedules",
                column: "DepartureCity")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_destinations_Name",
                table: "destinations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_Email_trgm",
                table: "contact_messages",
                column: "Email")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_FullName_trgm",
                table: "contact_messages",
                column: "FullName")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_Message_trgm",
                table: "contact_messages",
                column: "Message")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_Subject_trgm",
                table: "contact_messages",
                column: "Subject")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name_trgm",
                table: "categories",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_flightschedules_ArrivalCity_trgm",
                table: "flight_schedules");

            migrationBuilder.DropIndex(
                name: "IX_flightschedules_DepartureCity_trgm",
                table: "flight_schedules");

            migrationBuilder.DropIndex(
                name: "IX_destinations_Name",
                table: "destinations");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_Email_trgm",
                table: "contact_messages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_FullName_trgm",
                table: "contact_messages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_Message_trgm",
                table: "contact_messages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_Subject_trgm",
                table: "contact_messages");

            migrationBuilder.DropIndex(
                name: "IX_categories_Name_trgm",
                table: "categories");
        }
    }
}
