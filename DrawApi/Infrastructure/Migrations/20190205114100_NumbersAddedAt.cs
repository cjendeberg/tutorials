using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Migrations
{
    public partial class NumbersAddedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NumbersAddedAt",
                table: "Drawing",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumbersAddedAt",
                table: "Drawing");
        }
    }
}
