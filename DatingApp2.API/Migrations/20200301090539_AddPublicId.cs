﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp2.API.Migrations
{
    public partial class AddPublicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicId",
                table: "Photos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Photos");
        }
    }
}
