using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsSocialNetwork.Migrations
{
    public partial class AddedFieldPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "Photo",
                 table: "Playgrounds");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Playgrounds",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Playgrounds",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
