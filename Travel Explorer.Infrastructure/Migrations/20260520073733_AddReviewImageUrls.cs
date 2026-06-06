using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    
    public partial class AddReviewImageUrls : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "ImageUrls",
                table: "reviews",
                type: "text[]",
                nullable: false);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "reviews");
        }
    }
}
