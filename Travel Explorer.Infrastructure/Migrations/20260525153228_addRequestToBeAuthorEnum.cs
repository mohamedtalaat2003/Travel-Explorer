using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRequestToBeAuthorEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "requestToBeAuthor",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requestToBeAuthor",
                table: "users");
        }
    }
}
