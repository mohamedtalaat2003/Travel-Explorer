using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserManagementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "users");
        }
    }
}
