using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GoldenLadle.Migrations
{
    public partial class AddThumbToFilePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Winners",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "ThumbName",
                table: "FilePaths",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbName",
                table: "FilePaths");

            migrationBuilder.AddColumn<string[]>(
                name: "Winners",
                table: "Events",
                nullable: true);
        }
    }
}
